using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class ButtonEInteractionObj : MonoBehaviour
{
    //PhotonView
    private PhotonView view;
    //按鍵E互動GameObject (遊戲UI、謎題UI、謎題GameObject)
    public GameObject ui2d,uiclose,puzzleobj,pianoobj,playercam,rawui;
    //按鍵E互動GameObject的AudioSource
    private AudioSource audioSource;
    //謎題UI是否出現
    public static bool isPuzzleUiAppear = false;
    
    // Start is called before the first frame update
    void Start()
    {
        //GetComponent
        audioSource = this.gameObject.GetComponent<AudioSource>();

        //依照GameObject Name GetComponent
        if(this.gameObject.name == "配置圖" || this.gameObject.name == "蒸飯箱使用手冊")
        {
            view = this.gameObject.GetComponent<PhotonView>();
        }
    }

    //撿取物品(刪掉GameObject)
    private void GetObject()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            //GameObject Name判斷
            switch(this.gameObject.name)
            {
                //撿取學務處鑰匙
                case "Key_Student":
                {
                    if(PuzzleObjController.puzzleDictionary["Key_Student"] == false)
                    {
                        PuzzleObjController.puzzleDictionary["Key_Student"] = true;
                        Destroy(puzzleobj);
                    }
                }
                break;

                //撿取生物教室鑰匙
                case "Key_Bilogy":
                {
                    if(PuzzleObjController.puzzleDictionary["Key_Biology"] == false)
                    {
                        PuzzleObjController.puzzleDictionary["Key_Biology"] = true;
                        Destroy(puzzleobj);
                    }
                }
                break;

                //撿取教務處鑰匙
                case "Key_General":
                {
                    if(PuzzleObjController.puzzleDictionary["Key_General"] == false)
                    {
                        PuzzleObjController.puzzleDictionary["Key_General"] = true;
                        Destroy(puzzleobj);
                    }
                }
                break;
                
                //撿取滅火器
                case "FireExtinguish":
                {
                    if(PuzzleObjController.puzzleDictionary["FireExtinguish"] == false)
                    {
                        PuzzleObjController.puzzleDictionary["FireExtinguish"] = true;
                        Destroy(puzzleobj);
                    }
                }
                break;

                //滅火
                case "Smoke":
                {
                    if(PuzzleObjController.puzzleDictionary["FireExtinguish"] == true && PuzzleObjController.puzzleDictionary["SmokeBool"] == false)
                    {
                        PuzzleObjController.puzzleDictionary["SmokeBool"] = true;
                        audioSource.Play();
                        StartCoroutine(ObjSfx());
                    }
                }
                break;

                //樂譜上902
                case "樂譜上902":
                {
                    if(PuzzleObjController.puzzleDictionary["SheetMusic902"] == false)
                    {
                        PuzzleObjController.puzzleDictionary["SheetMusic902"] = true;
                        Destroy(puzzleobj);
                    }
                }
                break;

                //樂譜下802
                case "樂譜下802":
                {
                    if(PuzzleObjController.puzzleDictionary["SheetMusic802"] == false)
                    {
                        PuzzleObjController.puzzleDictionary["SheetMusic802"] = true;
                        Destroy(puzzleobj);
                    }
                }
                break;

                //P1阿浩學生證
                case "阿浩學生證":
                {
                    if(PuzzleObjController.puzzleDictionary["StudentCardP1"] == false)
                    {
                        PuzzleObjController.puzzleDictionary["StudentCardP1"] = true;
                        Destroy(puzzleobj);
                    }
                }
                break;

                //P2阿凱學生證
                case "阿凱學生證":
                {
                    if(PuzzleObjController.puzzleDictionary["StudentCardP2"] == false)
                    {
                        PuzzleObjController.puzzleDictionary["StudentCardP2"] = true;
                        Destroy(puzzleobj);
                    }
                }
                break;

                //巫毒娃娃
                case "wudu1": case "wudu2": case "wudu3":
                {
                    Destroy(puzzleobj);
                }
                break;
            }
        }
    }

    //顯示2DUI
    private void Uiappear()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            //GameObject Name不是Piano
            if(this.gameObject.name != "Piano")
            {
                Time.timeScale = 0f;
                Player.isPlayerActive = false;
                MouseLook.isMouseLook = false;
                isPuzzleUiAppear = true;
                ui2d.SetActive(true);
                uiclose.SetActive(true);
            }

            //P1玩家
            if(OutsideTheSchoolPUN.ServerConnect.playerID == 1)
            {
                //GameObject Name判斷
                switch(this.gameObject.name)
                {
                    //生物教室鑰匙謎題
                    case "生物教室保管表":
                    {
                        PuzzleObjController.puzzleDictionary["Key_BiologyPuzzle"] = true;
                    }   
                    break;

                    //電器配置圖謎題
                    case "配置圖":
                    {
                        view.RPC("IronDoorSwitch",RpcTarget.All);
                    }
                    break;

                    //鋼琴謎題GameObject
                    case "Piano":
                    {
                        if(PuzzleObjController.puzzleDictionary["SheetMusic902"] == true && PuzzleObjController.puzzleDictionary["PianoFinish"] == false)
                        {   
                            Time.timeScale = 0f;
                            Player.isPlayerActive = false;
                            MouseLook.isMouseLook = false;
                            isPuzzleUiAppear = true;
                            ui2d.SetActive(true);
                            uiclose.SetActive(true);
                            pianoobj.SetActive(true);
                        }
                    }
                    break;
                }
            }
            //P2玩家
            else if(OutsideTheSchoolPUN.ServerConnect.playerID == 2)
            {
                //GameObject Name判斷
                switch(this.gameObject.name)
                {
                    //國文課本(0)謎題
                    case "國文課本(0)":
                    {
                        PuzzleObjController.puzzleDictionary["ZeroPuzzle"] = true;
                    }
                    break;

                    //廁所冷笑話(5)謎題
                    case "廁所冷笑話(5)":
                    {
                        PuzzleObjController.puzzleDictionary["FivePuzzle"] = true;
                    }
                    break;

                    //鋼琴謎題GameObject
                    case "Piano":
                    {
                        if(PuzzleObjController.puzzleDictionary["SheetMusic802"] == true && PuzzleObjController.puzzleDictionary["PianoFinish"] == false)
                        {   
                            Time.timeScale = 0f;
                            Player.isPlayerActive = false;
                            MouseLook.isMouseLook = false;
                            isPuzzleUiAppear = true;
                            ui2d.SetActive(true);
                            uiclose.SetActive(true);
                            pianoobj.SetActive(true);
                        }
                    }
                    break;
                }
            } 
        }
        else if(Input.GetKeyDown("escape"))
        {
            Time.timeScale = 1f;
            isPuzzleUiAppear = false;      
            Player.isPlayerActive = true;
            MouseLook.isMouseLook = true;
            ui2d.SetActive(false);
            uiclose.SetActive(false);

            //P1玩家
            if(OutsideTheSchoolPUN.ServerConnect.playerID == 1)
            {
                //GameObject Name判斷
                switch(this.gameObject.name)
                {
                    //電器配置圖謎題
                    case "配置圖":
                    {
                        if(PlayerText.playerTextTriggeredDictionary["IronDoorText"] == false)
                        {
                            PlayerText.playerTextTriggeredDictionary["IronDoorText"] = true;
                        }
                    }
                    break;

                    //鋼琴謎題GameObject
                    case "Piano":
                    {
                        pianoobj.SetActive(false);
                    }
                    break;

                    //電腦謎題
                    case "電腦謎題":
                    {
                        Cursor.lockState = CursorLockMode.Locked;
                        Cursor.visible = false;
                        puzzleobj.SetActive(false);
                    }
                    break;
                }
            }
            //P2玩家
            else if(OutsideTheSchoolPUN.ServerConnect.playerID == 2)
            {
                //GameObject Name判斷
                switch(this.gameObject.name)
                {
                    //組長字條
                    case "組長報修":
                    {
                        if(PlayerText.playerTextTriggeredDictionary["GeneralText"] == false)
                        {
                            PlayerText.playerTextTriggeredDictionary["GeneralText"] = true;
                        }
                    }
                    break;

                    //蒸飯箱使用手冊文字
                    case "蒸飯箱使用手冊":
                    {
                        if(PlayerText.playerTextTriggeredDictionary["SteamBoxText"] == false)
                        {
                            PlayerText.playerTextTriggeredDictionary["SteamBoxText"] = true;
                        }
                        
                        view.RPC("SteamBoxSwitch",RpcTarget.All);
                    }
                    break;

                    //鋼琴謎題GameObject
                    case "Piano":
                    {
                        pianoobj.SetActive(false);
                    }
                    break;
                }
            }
        }
    }

    //文字觸發
    private void Textappear()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            //P2玩家
            if(OutsideTheSchoolPUN.ServerConnect.playerID == 2)
            {
                //GameObject Name判斷
                switch(this.gameObject.name)
                {
                    //塗鴉牆
                    case "塗鴉牆(8)":
                    {
                        PuzzleObjController.puzzleDictionary["EightPuzzle"] = true;

                        if(PlayerText.playerTextTriggeredDictionary["EightPuzzleText"] == false)
                        {
                            PlayerText.playerTextTriggeredDictionary["EightPuzzleText"] = true;
                        }
                    }
                    break;
                }
            }
        }
    }

    //鐵捲門開關觸發
    [PunRPC]
    public void IronDoorSwitch()
    {
        Debug.Log("鐵捲門可以按了");
        PuzzleObjController.puzzleDictionary["IronDoorPuzzle"] = true;
    }

    //蒸飯室可以引爆
    [PunRPC]
    public void SteamBoxSwitch()
    {
        Debug.Log("蒸飯室可以引爆了");
        PuzzleObjController.puzzleDictionary["SteamBoxPuzzle"] = true;
    }

    //謎題GameObject觸發音效
    IEnumerator ObjSfx()
    {
        yield return new WaitForSeconds(1f);
        Destroy(puzzleobj);
    }
}