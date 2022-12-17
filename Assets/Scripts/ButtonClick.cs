using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonClick : MonoBehaviour
{
    public int from, to;
    private SkyboxController skyboxController;
    // Start is called before the first frame update
    void Start()
    {
        skyboxController = GameObject.Find("SkyboxController").GetComponent<SkyboxController>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Click()
    {
        skyboxController.Move(from, to);
    }
}
