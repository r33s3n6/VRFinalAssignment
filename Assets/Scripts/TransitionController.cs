using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using static UnityEngine.GraphicsBuffer;

public class TransitionController : MonoBehaviour
{
    public Material[] skyboxMaterial;
    public GameObject[] canvas;
    public Vector3[] cameraPos, cameraOrient;
    private Transform cameraTransform;
    private CanvasGroup canvasGroup;
    public Vector3 direction, orientation_from, orientation_to;
    void Start()
    {
        cameraTransform = GameObject.Find("OVRCameraRig").GetComponent<Transform>();
        canvasGroup = GameObject.FindGameObjectWithTag("mask").GetComponent<CanvasGroup>();
        canvasGroup.alpha = 1.0f;
        for (int i = 0; i < canvas.Length; i++)
            canvas[i].SetActive(false);
        RenderSettings.skybox = skyboxMaterial[SceneData.Instance.target];
        cameraTransform.rotation = Quaternion.Euler(SceneData.Instance.angle_to);

        StartCoroutine("FadeIn");
    }

    void Update()
    {

    }

    public void Move(int from, int to, int type)
    {
        Debug.Log("Move from " + from.ToString() + " to " + to.ToString() + " type " + type.ToString());
        if (type == 0)
        {
            RenderSettings.skybox = skyboxMaterial[to];
            cameraTransform.Rotate(new Vector3(0, cameraOrient[from].y - cameraOrient[to].y, 0));
            canvas[from].SetActive(false); canvas[to].SetActive(true);
            return;
        }
        else if (type == 1)
        {
            SceneData.Instance.pos_from = cameraPos[from];
            SceneData.Instance.pos_to = cameraPos[to];
            SceneData.Instance.orient_from = cameraOrient[from] + cameraTransform.rotation.eulerAngles;
            SceneData.Instance.target = to;
            SceneData.Instance.angle_to = cameraTransform.rotation.eulerAngles + new Vector3(0, cameraOrient[from].y - cameraOrient[to].y, 0);
            StartCoroutine("FadeOut");
            return;
        }
        else if (type == 2)
        {
            direction = Vector3.Normalize(cameraPos[to] - cameraPos[from]);
            orientation_from = cameraOrient[from];
            orientation_to = cameraOrient[to];
            // TODO
            return;
        }
    }

    IEnumerator FadeIn()
    {
        for (float f = 1f; f >= 0; f -= 0.01f)
        {
            canvasGroup.alpha = f;
            yield return null;
        }
        canvas[SceneData.Instance.target].SetActive(true);
    }

    IEnumerator FadeOut()
    {
        for (float f = 0f; f <= 1; f += 0.01f)
        {
            canvasGroup.alpha = f;
            yield return null;
        }
        SceneManager.LoadScene(1);
    }
}
