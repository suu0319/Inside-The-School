using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPuzzleControl : MonoBehaviour
{
    //謎題GameObject (遊戲ui、camera、謎題ui、書本謎題、P1P2文字)
    public GameObject rawui,cam,playercam1,playercam2,uiclose,bookpuzzleP1,bookpuzzleP2,puzzle,textP1,textP2;
    //謎題GameObject是否觸發 (是否切換camera)
    private bool isSwitch0 = false;

    // Update is called once per frame
    void Update()
    {
        //尋找P1玩家的Camera
        if(OutsideTheSchoolPUN.ServerConnect.playerID == 1 && playercam1 == null)
        {
            playercam1 = GameObject.Find("Player1").transform.GetChild(0).gameObject;
        }
        //尋找P2玩家的Camera
        else if(OutsideTheSchoolPUN.ServerConnect.playerID == 2 && playercam2 == null)
        {
            playercam2 = GameObject.Find("Player2").transform.GetChild(0).gameObject;
        }

        //謎題是否進行
        if(puzzle.activeInHierarchy == true)
        {
            Time.timeScale = 1f;
            OriginMode.isGod = true;
            Player.isPlayerActive = false;
            MouseLook.isMouseLook = false;
            ButtonEInteractionObj.isPuzzleUiAppear = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            uiclose.SetActive(true);
            rawui.SetActive(false);

            //P1玩家
            if(OutsideTheSchoolPUN.ServerConnect.playerID == 1)
            {
                playercam1.SetActive(false);
            }
            //P2玩家
            else if(OutsideTheSchoolPUN.ServerConnect.playerID == 2)
            {
                playercam2.SetActive(false);
            }
        }

        //是否退出謎題進行
        if((Input.GetKeyDown("escape") && puzzle.activeInHierarchy == true)
        || (this.gameObject.name == "謎題攝影機控制" && PuzzleObjController.puzzleDictionary["BookPuzzle"] == true && isSwitch0 == false)
        || (this.gameObject.name == "謎題攝影機控制2" && PuzzleObjController.puzzleDictionary["ArtPuzzle"] == true && isSwitch0 == false))
        {
            //無敵模式
            OriginMode.isGod = false;
            
            Time.timeScale = 1f;
            ButtonEInteractionObj.isPuzzleUiAppear = false;      
            uiclose.SetActive(false);
            rawui.SetActive(true);

            //P1玩家
            if(OutsideTheSchoolPUN.ServerConnect.playerID == 1)
            {
                if(this.gameObject.name == "謎題攝影機控制")
                {
                    textP1.SetActive(false);
                    bookpuzzleP1.SetActive(true);
                }
                else if(this.gameObject.name == "謎題攝影機控制3")
                {
                    GameObject.Find("EventSystem").GetComponent<DeviceSelect>().enabled = true;
                    textP1.SetActive(false);
                }

                playercam1.SetActive(true);
            }
            //P2玩家
            else if(OutsideTheSchoolPUN.ServerConnect.playerID == 2)
            {   
                if(this.gameObject.name == "謎題攝影機控制")
                {
                    textP2.SetActive(false);

                    if(PuzzleObjController.puzzleDictionary["BookPuzzle"] == true)
                    {
                        isSwitch0 = true;
                        bookpuzzleP2.SetActive(true);
                    }
                }
                else if(this.gameObject.name == "謎題攝影機控制2")
                {
                    textP2.SetActive(false);

                    if(PuzzleObjController.puzzleDictionary["ArtPuzzle"] == true)
                    {
                        isSwitch0 = true;
                        bookpuzzleP2.SetActive(true);
                    }
                }
                
                playercam2.SetActive(true);
            }

            //Player活躍狀態
            StartCoroutine(isPlayerActive());
            puzzle.SetActive(false);
        }
    }

    //玩家恢復操作 (退出謎題進行)
    IEnumerator isPlayerActive()
    {
        yield return new WaitForEndOfFrame();
        Player.isPlayerActive = true;
        MouseLook.isMouseLook = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
