using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class DeviceSelect : MonoBehaviour
{
    //按鍵GameObject
    public GameObject firstbutton,settingbutton,deadfirstbutton,replayfirstbutton,setting,teachui,rawui,replayui;
    //場景編號
    private int scenenum;
    //游標Coroutine
    private bool isMenuDeadCursor = false;
    //鍵盤是否使用
    public static bool isKeyboardUsed = false;

    void Start() 
    {
        //取得場景編號
        scenenum = SceneManager.GetActiveScene().buildIndex;
        
        //主選單
        if(scenenum == 0)
        {
            HideCursor();
            StartCoroutine(CursorCD());
        }
    }

    // Update is called once per frame
    void Update()
    {
        KeyboardCursor();
    }

    //游標鍵盤判定
    public void KeyboardCursor()
    {
        //玩家死亡 游標控制
        if(Player.currentHP == 0 && isMenuDeadCursor == false)
        {
            HideCursor();
            StartCoroutine(CursorCD());
        }
        
        //遊戲中游標鍵盤控制
        if(scenenum != 0 || isMenuDeadCursor == true)
        {
            if(Player.currentHP == 0)
            {
                StartCoroutine(CursorCD());
            }
            else if(ButtonEInteractionObj.isPuzzleUiAppear == false)
            {
                DeviceManager();
            }
        }

        //新手教學UI 游標控制
        if(scenenum == 2)
        {
            if(teachui.activeInHierarchy == true)
            {
                HideCursor();
                StartCoroutine(CursorCD());
            }
        }
    }
    
    //游標鍵盤判定
    public void DeviceManager()
    {
        //使用滑鼠
        if(Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0 || Input.GetMouseButtonDown(0)) 
        {
            isKeyboardUsed = false;
            EventSystem.current.SetSelectedGameObject(null);

            if(scenenum == 0)
            {
                AppearCursor();
            }
            else if(Player.isPlayerActive == false && (teachui.activeInHierarchy == false || rawui.activeInHierarchy == false))
            {
                AppearCursor();
            }
        }
        //使用鍵盤
        else if(Input.anyKeyDown && isKeyboardUsed == false) 
        {
            isKeyboardUsed = true;
            
            if(setting.activeInHierarchy == false)
            {
                if(Player.currentHP == 0)
                {
                    if(replayui.activeInHierarchy == true)
                    {
                        EventSystem.current.SetSelectedGameObject(replayfirstbutton);
                    }
                    else
                    {
                        EventSystem.current.SetSelectedGameObject(deadfirstbutton);
                    }
                }
                else
                {   
                    EventSystem.current.SetSelectedGameObject(firstbutton);
                }
                
                HideCursor();
            }
            else if(setting.activeInHierarchy == true)
            {
                EventSystem.current.SetSelectedGameObject(settingbutton);
                HideCursor();
            }
        }
    }

    //隱藏游標
    public void HideCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    //出現游標
    public void AppearCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    //游標出現Coroutine
    IEnumerator CursorCD()
    {
        yield return new WaitForSeconds(12f);
        isMenuDeadCursor = true;
        
        //玩家死亡
        if(Player.currentHP == 0)
        {
            DeviceManager();
        }
    }
}