using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanRotate : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        Rotate();
    }

    //旋轉
    void Rotate()
    {
        this.gameObject.transform.Rotate(0,150*Time.deltaTime,0f);
    }
}