using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class RayControl : MonoBehaviour
{
    //PhotonView
    private PhotonView view;
    //Ray
    private Ray ray;
    //RaycastHit
    public RaycastHit hit,ghosthit;
    //射線判定相關GameObject
    public GameObject canvas,ui,raw,ButtonE,woodghost,hatelightghost1,hatelightghost2;
    //射線距離
    private float raylength = 1.5f;
    //怪物是否出現
    private bool isGhostAppear = false;
    //怪物追蹤判定
    public static bool isWoodGhostStop,isHateLightGhostTrack;
    
    void Start()
    {
        //抓取GameObject
        canvas = GameObject.Find("Canvas");
        ui = canvas.transform.GetChild(1).gameObject;
        raw = ui.transform.GetChild(0).gameObject;
        ButtonE = raw.transform.GetChild(1).gameObject;

        //GetComponent
        view = this.gameObject.GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        RayInteraction();
    }

    //射線互動判斷
    public void RayInteraction()
    {
        //預設準心
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector3 v3Pos = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0f));

        //射線範圍內    
        if(Physics.Raycast(ray,out hit, raylength,1 << 11))
        {
            Tagtext();        
            Debug.DrawRay(Input.mousePosition,hit.point,Color.yellow);
            //Debug.Log(hit.transform.name);
        }
        //設線範圍外
        else
        {
            ButtonE.SetActive(false);
        }

        //P1玩家
        if(OutsideTheSchoolPUN.ServerConnect.playerID == 1 && Time.timeScale == 1f)
        {
            //木頭人出現
            if(Trigger.isEnter2ndFloor == true && isGhostAppear == false)
            {
                woodghost = GameObject.Find("木頭人");
                
                if(woodghost != null)
                {
                    isGhostAppear = true;
                }
            }
            
            //視野範圍判斷木頭人
            if(isGhostAppear == true)
            {
                //木頭人判定
                if (IsInView(woodghost.transform.position) && Player.currentHP != 0)   
                {
                    isWoodGhostStop = true;
                }
                else
                {
                    isWoodGhostStop = false;
                }
            }
        }
        //P2玩家
        else if(OutsideTheSchoolPUN.ServerConnect.playerID == 2 && Time.timeScale == 1f)
        {
            //厭光怪出現
            if(Trigger.isEnter2ndFloor == true && isGhostAppear == false)
            {
                hatelightghost1 = GameObject.Find("厭光怪1");
                hatelightghost2 = GameObject.Find("厭光怪2");
                
                if(hatelightghost1 != null || hatelightghost2 != null)
                {
                    isGhostAppear = true;
                }
            }
            //手電筒範圍判斷厭光怪
            if(isGhostAppear == true)
            {
                //厭光怪判定
                if (IsInView(hatelightghost1.transform.position) || IsInView(hatelightghost2.transform.position))   
                {
                    isHateLightGhostTrack = true;
                }
                else
                {
                    isHateLightGhostTrack = false;
                }
            }
        }
    }

    //GameObject互動判斷
    public void Tagtext()
    {
        //GameObject Tag判斷
        switch(hit.collider.gameObject.tag)
        {
            //取得物品
            case "InteractionGet": case "FireExtinguish":
            {
                ButtonE.SetActive(true);
                hit.transform.SendMessage("GetObject",gameObject,SendMessageOptions.DontRequireReceiver);
            }
            break;

            //滅火
            case "Smoke":
            {
                if(PuzzleObjController.puzzleDictionary["FireExtinguish"] == true)
                {
                    ButtonE.SetActive(true);
                    hit.transform.SendMessage("GetObject",gameObject,SendMessageOptions.DontRequireReceiver);
                }
            }
            break;

            //開關門
            case "Door": case "ToiletDoor": case "StudentDoor": case "GeneralDoor": case "BiologyDoor": case "LibraryDoor":
            {
                ButtonE.SetActive(true);
                hit.transform.SendMessage("DoorOpenClose",gameObject,SendMessageOptions.DontRequireReceiver);
            }
            break;

            //置物櫃開關門
            case "BiologyPuzzleDoor":
            {
                if(PuzzleObjController.puzzleDictionary["Key_BiologyPuzzle"] == true)
                {
                    ButtonE.SetActive(true);
                    hit.transform.SendMessage("DoorOpenClose",gameObject,SendMessageOptions.DontRequireReceiver);
                }
            }
            break;

            //鐵捲門開關
            case "IronDoorSwitch":
            {
                if(PuzzleObjController.puzzleDictionary["IronDoorPuzzle"] == true && PuzzleObjController.puzzleDictionary["IronDoorSwitch"] == false 
                && OutsideTheSchoolPUN.ServerConnect.playerID == 2)
                {
                    ButtonE.SetActive(true);
                    hit.transform.SendMessage("Switch",gameObject,SendMessageOptions.DontRequireReceiver);
                }
            }
            break;

            //蒸飯箱開關
            case "SteamBoxSwitch":
            {
                if(PuzzleObjController.puzzleDictionary["SteamBoxPuzzle"] == true && PuzzleObjController.puzzleDictionary["SteamBoxSwitch"] == false 
                && OutsideTheSchoolPUN.ServerConnect.playerID == 1)
                {
                    ButtonE.SetActive(true);
                    hit.transform.SendMessage("Switch",gameObject,SendMessageOptions.DontRequireReceiver);
                }
            }
            break;

            //2DUI
            case "Interaction2DUI":
            {
                if(hit.collider.gameObject.name == "Piano")
                {
                    if(PuzzleObjController.puzzleDictionary["PianoFinish"] == true)
                    {
                        ButtonE.SetActive(false);
                    }
                    else if((PuzzleObjController.puzzleDictionary["SheetMusic902"] == true || PuzzleObjController.puzzleDictionary["SheetMusic802"] == true)
                    && PuzzleObjController.puzzleDictionary["PianoFinish"] == false)
                    {
                        ButtonE.SetActive(true);
                        hit.transform.SendMessage("Uiappear",gameObject,SendMessageOptions.DontRequireReceiver);
                    }
                }
                else if(hit.collider.gameObject.name == "P1書籍謎題" || hit.collider.gameObject.name == "P2書籍謎題")
                {
                    if(PuzzleObjController.puzzleDictionary["BookPuzzle"] == true)
                    {
                        ButtonE.SetActive(false);
                    }
                    else
                    {
                        ButtonE.SetActive(true);
                        hit.transform.SendMessage("Uiappear",gameObject,SendMessageOptions.DontRequireReceiver);
                    }
                }
                else if(hit.collider.gameObject.name == "裝置藝術")
                {
                    if(PuzzleObjController.puzzleDictionary["ArtPuzzle"] == true)
                    {
                        ButtonE.SetActive(false);
                    }
                    else
                    {
                        ButtonE.SetActive(true);
                        hit.transform.SendMessage("Uiappear",gameObject,SendMessageOptions.DontRequireReceiver);
                    }
                }
                else
                {
                    ButtonE.SetActive(true);
                    hit.transform.SendMessage("Uiappear",gameObject,SendMessageOptions.DontRequireReceiver);
                }
            }   
            break;

            //Text觸發
            case "InteractionText":
            {
                ButtonE.SetActive(true);
                hit.transform.SendMessage("Textappear",gameObject,SendMessageOptions.DontRequireReceiver);
            }   
            break;

            default:
            {
                ButtonE.SetActive(false);
            }
            break;
        }
    }

    //判斷物體是否在相機前面
    public bool IsInView(Vector3 worldPos)
    {
        Transform camTransform = Camera.main.transform;
        Vector2 viewPos = Camera.main.WorldToViewportPoint(worldPos);
        Vector3 dir = (worldPos - camTransform.position).normalized;
        float dot = Vector3.Dot(camTransform.forward, dir);     

        if(dot > 0 && viewPos.x >= 0 && viewPos.x <= 1 && viewPos.y >= 0 && viewPos.y <= 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}