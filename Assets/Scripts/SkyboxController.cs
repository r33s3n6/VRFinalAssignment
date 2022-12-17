using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxController : MonoBehaviour
{
    public Material[] skyboxMaterial;
    public GameObject[] canvas;
    private int[,] angleOffset = new int[11, 11];
    private Transform cameraTransform;

    // Start is called before the first frame update
    void Start()
    {
        cameraTransform = GameObject.Find("OVRCameraRig").GetComponent<Transform>();
        RenderSettings.skybox = skyboxMaterial[0];
        cameraTransform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        angleOffset[0, 1] = -90; angleOffset[1, 0] = 90;
        angleOffset[0, 2] = -135; angleOffset[2, 0] = 135;
        angleOffset[0, 4] = -75; angleOffset[4, 0] = 75;
        angleOffset[1, 5] = 135; angleOffset[5, 1] = -135;
        angleOffset[5, 6] = -45; angleOffset[6, 5] = 45;
        angleOffset[6, 7] = -135; angleOffset[7, 6] = 135;
        angleOffset[7, 8] = 180; angleOffset[8, 7] = 180;
        angleOffset[7, 10] = 90; angleOffset[10, 7] = -90;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Move(int from, int to)
    {
        Debug.Log("Move from " + from.ToString() + "to " + to.ToString());
        RenderSettings.skybox = skyboxMaterial[to];
        cameraTransform.Rotate(new Vector3(0, angleOffset[from, to], 0));
        canvas[from].SetActive(false); canvas[to].SetActive(true);
    }
}
