using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonClick : MonoBehaviour
{
    public int from, to, type;
    private TransitionController transitionController;
    // Start is called before the first frame update
    void Start()
    {
        transitionController = GameObject.Find("TransitionController").GetComponent<TransitionController>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Click()
    {
        transitionController.Move(from, to, type);
    }
}
