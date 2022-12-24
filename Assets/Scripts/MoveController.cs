using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveController : MonoBehaviour
{
    private Transform cameraTransform;
    private bool isMoving = false;
    private Vector3 pos_from;
    private Vector3 pos_to;
    private Vector3 orient_from;
    private int target;
    private Vector3 angle_to;
    private CanvasGroup canvasGroup;
    // Start is called before the first frame update
    void Start()
    {
        cameraTransform = GameObject.Find("OVRCameraRig").GetComponent<Transform>();
        canvasGroup = GameObject.FindGameObjectWithTag("mask").GetComponent<CanvasGroup>();
        canvasGroup.alpha = 1.0f;
        pos_from = SceneData.Instance.pos_from;
        pos_to= SceneData.Instance.pos_to;
        orient_from= SceneData.Instance.orient_from;
        target = SceneData.Instance.target;
        angle_to= SceneData.Instance.angle_to;
        cameraTransform.position = pos_from;
        cameraTransform.rotation = Quaternion.Euler(orient_from);

        StartCoroutine("FadeIn");
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving)
        {
            cameraTransform.position = Vector3.MoveTowards(cameraTransform.position, pos_to, Time.deltaTime * 0.6f);
            if (cameraTransform.position == pos_to)
            {
                isMoving = false;
                endMove();
            }
        }
    }

    void endMove()
    {
        SceneData.Instance.target = target;
        SceneData.Instance.angle_to = angle_to;
        StartCoroutine("FadeOut");
    }

    IEnumerator FadeIn()
    {
        for (float f = 1f; f >= 0; f -= 0.02f)
        {
            canvasGroup.alpha = f;
            yield return null;
        }
        isMoving = true;
    }

    IEnumerator FadeOut()
    {
        for (float f = 0f; f <= 1; f += 0.02f)
        {
            canvasGroup.alpha = f;
            yield return null;
        }
        SceneManager.LoadScene(0);
    }
}
