using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using static UnityEngine.GraphicsBuffer;

public class SkyboxController : MonoBehaviour
{
    public Material[] skyboxMaterial;
    public GameObject[] canvas;
    public Vector3[] cameraPos, cameraOrient;
    private int[,] angleOffset = new int[11, 11];
    private Transform cameraTransform;
    private CanvasGroup canvasGroup;

    void Start()
    {
        cameraTransform = GameObject.Find("OVRCameraRig").GetComponent<Transform>();
        canvasGroup = GameObject.FindGameObjectWithTag("mask").GetComponent<CanvasGroup>();
        canvasGroup.alpha = 1.0f;
        for (int i = 0; i < canvas.Length; i++)
            canvas[i].SetActive(false);
        RenderSettings.skybox = skyboxMaterial[SceneData.Instance.target];
        cameraTransform.rotation = Quaternion.Euler(SceneData.Instance.angle_to);

        angleOffset[0, 1] = -90; angleOffset[1, 0] = 90;
        angleOffset[0, 2] = -135; angleOffset[2, 0] = 135;
        angleOffset[0, 4] = -75; angleOffset[4, 0] = 75;
        angleOffset[1, 5] = 135; angleOffset[5, 1] = -135;
        angleOffset[5, 6] = -45; angleOffset[6, 5] = 45;
        angleOffset[6, 7] = -135; angleOffset[7, 6] = 135;
        angleOffset[7, 8] = 180; angleOffset[8, 7] = 180;
        angleOffset[7, 10] = 90; angleOffset[10, 7] = -90;

        StartCoroutine("FadeIn");
    }

    void Update()
    {

    }

    public void Move(int from, int to, int type)
    {
        Debug.Log("Move from " + from.ToString() + " to " + to.ToString() + " type " + type.ToString());
        if (type == 1)
        {
            SceneData.Instance.pos_from = cameraPos[from];
            SceneData.Instance.pos_to = cameraPos[to];
            SceneData.Instance.orient_from = cameraOrient[from] + cameraTransform.rotation.eulerAngles;
            SceneData.Instance.target = to;
            SceneData.Instance.angle_to = cameraTransform.rotation.eulerAngles + new Vector3(0, angleOffset[from, to], 0);
            StartCoroutine("FadeOut");
            return;
        }
        else if (type == 2)
        {
            return;
        }
        else
        {
            RenderSettings.skybox = skyboxMaterial[to];
            cameraTransform.Rotate(new Vector3(0, angleOffset[from, to], 0));
            canvas[from].SetActive(false); canvas[to].SetActive(true);
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
