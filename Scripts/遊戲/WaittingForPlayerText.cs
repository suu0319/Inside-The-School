using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaittingForPlayerText : MonoBehaviour
{
    //等待玩家Text
    public Text waittingttext;
    //Text動畫觸發
    public bool isTextAnim = false;
    
    // Start is called before the first frame update
    void Start()
    {
        //尋找等待玩家Text
        waittingttext = this.gameObject.GetComponentInChildren<Text>();
        waittingttext.text = "等待玩家中";
    }

    // Update is called once per frame
    void Update()
    {
        if(isTextAnim == false)
        {
            isTextAnim = true;
            StartCoroutine(WaittingAnimation());
        }
    }

    //等待玩家Text動畫
    IEnumerator WaittingAnimation()
    {
        for(int i = 0 ; i < 3 ; i++)
        {
            waittingttext.text += ".";
            yield return new WaitForSeconds(1f);
            
            if(waittingttext.text == "等待玩家中...")
            {
                waittingttext.text = "等待玩家中";
                isTextAnim = false;
            }
        }
    }
}