using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dome : MonoBehaviour
{
    public Transform cameraTransform;
    public Shader shader;
    public Material material;
    
    // Start is called before the first frame update
    void Start()
    {
        cameraTransform = GameObject.Find("OVRCameraRig").GetComponent<Transform>();
        material = new Material(shader);
        material.hideFlags = HideFlags.DontSave;
        gameObject.GetComponent<MeshRenderer>().material = material;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = cameraTransform.position;
    }
}
