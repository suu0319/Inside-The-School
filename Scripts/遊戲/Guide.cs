using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Guide : MonoBehaviour
{
    //新手教學UI
    public GameObject RawUI,guideui;
    //新手教學已經關閉
    private bool isSwitch0 = false;
    //新手教學是否關閉
    public static bool isGuideClose = false;
    
    // Start is called before the first frame update
    void Start()
    {
        //Get guideui.gameobject
        guideui = this.gameObject.transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        GuideUI();
    }

    //判斷新手教學UI
    public void GuideUI()
    {
        //新手教學UI出現
        if(guideui.activeSelf == true)
        {
            Player.isPlayerActive = false;
            MouseLook.isMouseLook = false;

            if(Input.anyKeyDown)
            {
                StartCoroutine(timeCD());
                guideui.SetActive(false);
                RawUI.SetActive(true);
                isGuideClose = true;
            }
        }
        //新手教學UI關閉
        else if(guideui.activeSelf == false && isSwitch0 == false)
        {
            isSwitch0 = true;
            RawUI.SetActive(true);
        }
    }
    
    //玩家狀態激活(關閉新手教學UI)
    IEnumerator timeCD()
    {
        yield return null;
        Player.isPlayerActive = true;
        MouseLook.isMouseLook = true;
    }
}