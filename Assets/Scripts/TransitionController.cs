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
    public Cubemap[] skyboxCubemap;
    public GameObject[] canvas;
    public Vector3[] cameraPos, cameraOrient;
    public Dome dome;
    private Transform cameraTransform, centerEyeTransform;
    private CanvasGroup canvasGroup;
    public Vector3 direction, orientation_from, orientation_to;
    void Start()
    {
        cameraTransform = GameObject.Find("OVRCameraRig").GetComponent<Transform>();
        centerEyeTransform = GameObject.Find("CenterEyeAnchor").GetComponent<Transform>();
        canvasGroup = GameObject.FindGameObjectWithTag("mask").GetComponent<CanvasGroup>();
        canvasGroup.alpha = 1.0f;
        for (var i = 0; i < canvas.Length; i++)
            canvas[i].SetActive(false);
        RenderSettings.skybox = skyboxMaterial[SceneData.Instance.target];
        cameraTransform.rotation = Quaternion.Euler(SceneData.Instance.angle_to);
        dome = GameObject.Find("Dome").GetComponent<Dome>();
        dome.gameObject.SetActive(false);
        StartCoroutine("FadeIn");
    }

    public void Move(int from, int to, int type)
    {
        //Debug.Log("Move from " + from.ToString() + " to " + to.ToString() + " type " + type.ToString());
        if (type == 0)
        {
            RenderSettings.skybox = skyboxMaterial[to];
            cameraTransform.Rotate(new Vector3(0, cameraOrient[from].y - cameraOrient[to].y, 0));
            canvas[from].SetActive(false); canvas[to].SetActive(true);
        }
        else if (type == 1)
        {
            SceneData.Instance.pos_from = cameraPos[from];
            SceneData.Instance.pos_to = cameraPos[to];
            SceneData.Instance.orient_from = cameraOrient[from] + cameraTransform.rotation.eulerAngles;
            SceneData.Instance.target = to;
            SceneData.Instance.angle_to = cameraTransform.rotation.eulerAngles + new Vector3(0, cameraOrient[from].y - cameraOrient[to].y, 0);
            StartCoroutine("FadeOut");
        }
        else if (type == 2)
        {
            StartCoroutine(MobiusTransit(from, to));
        }
    }

    IEnumerator FadeIn()
    {
        for (float f = 1f; f >= 0; f -= 0.02f)
        {
            canvasGroup.alpha = f;
            yield return null;
        }
        canvas[SceneData.Instance.target].SetActive(true);
    }

    IEnumerator FadeOut()
    {
        for (float f = 0f; f <= 1; f += 0.02f)
        {
            canvasGroup.alpha = f;
            yield return null;
        }
        SceneManager.LoadScene(1);
    }

    IEnumerator MobiusTransit(int from, int to)
    {
        var currOrientation = centerEyeTransform.forward;
        dome.material.SetTexture("_CubemapFrom", skyboxCubemap[from]);
        dome.material.SetTexture("_CubemapTo", skyboxCubemap[to]);
        dome.material.SetVector("_Orientation", currOrientation);
        dome.gameObject.SetActive(true);
        for (var f = 0f; f <= 1; f += 0.0025f)
        {
            dome.material.SetFloat("_Process", f);
            yield return new WaitForSeconds(0.01f);
        }
        dome.gameObject.SetActive(false);
        centerEyeTransform.Rotate(new Vector3(0, cameraOrient[from].y - cameraOrient[to].y, 0));
        RenderSettings.skybox = skyboxMaterial[to];
        canvas[from].SetActive(false); canvas[to].SetActive(true);
    }
}
