using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlash : MonoBehaviour
{
    //閃爍時間
    public float flashtime;
    //燈光GameObject
    public GameObject[] lightobj;
    //燈光
    public Light[] light;
    
    // Start is called before the first frame update
    void Start()
    {
        //GetComponent
        light[0] = lightobj[0].GetComponent<Light>();
        light[1] = lightobj[1].GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        //閃爍時間
        flashtime += Time.deltaTime;
        
        if(flashtime % 1 > 0.5f)
        {
            light[0].enabled = false;
            light[1].enabled = false;
        }
        else
        {
            light[0].enabled = true;
            light[1].enabled = true;
        }
    }
}