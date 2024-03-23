using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacingCamera : MonoBehaviour
{


    // Update is called once per frame
    void LateUpdate()
    {
        gameObject.transform.forward=Camera.main.transform.forward;
    }
}
