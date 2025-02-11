using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class ButtonEInteractionDoorSwitch : MonoBehaviour
{
    //PhotonView
    private PhotonView view;
    //按鍵E互動GameObject
    public GameObject irondoors,smoke,gallerysmoke;
    //Text Animator
    public Animator animator;
    //Text Animation
    public Animation animation;
    //Text AnimationClip
    public AnimationClip[] animationClips;
    //按鍵E互動GameObject的音頻源
    public AudioSource audioSource;
    //按鍵E互動GameObject的音頻片段
    public AudioClip[] audioClips;
    //遊戲對話Text
    public Text text;
    //Coroutine開關
    private bool isSwitch0,isSwitch1 = false;
    //取得開關門Bool
    public bool isOpen;
    
    // Start is called before the first frame update
    void Start()
    {
        //GetComponent
        view = this.gameObject.GetComponent<PhotonView>();
        animation = GameObject.Find("對話文字").GetComponent<Animation>();
        text = GameObject.Find("對話文字").GetComponent<Text>();

        //依照tag GetComponent
        switch(this.gameObject.tag)
        {
            case "Door": case "StudentDoor": case "GeneralDoor": case "BiologyDoor": case "IronDoorSwitch": case "LibraryDoor":
            {
                animator = this.gameObject.GetComponentInParent<Animator>();
                audioSource = this.gameObject.GetComponentInParent<AudioSource>();
            }
            break;
            
            case "ToiletDoor": case "BiologyPuzzleDoor": case "SteamBoxSwitch":
            {
                animator = this.gameObject.GetComponent<Animator>();
                audioSource = this.gameObject.GetComponent<AudioSource>();
            }
            break;
        }
    }
   
    //門開關控制
    private void DoorOpenClose()
    {
        //取得動畫Bool-Open
        isOpen = animator.GetBool("Open");
        
        //GameObject Tag判斷
        switch(this.gameObject.tag)
        {
            //學務處門判斷
            case "StudentDoor":
            {
                if(Input.GetKeyDown(KeyCode.E) && isSwitch0 == false && PuzzleObjController.puzzleDictionary["Key_Student"] == true)
                {
                    //開門
                    if(isOpen == false)
                    {
                        DoorOpenSfx();

                        //尋找生物教室保管表文字
                        if(PlayerText.playerTextTriggeredDictionary["StudentText"] == false)
                        {
                            PlayerText.playerTextTriggeredDictionary["StudentText"] = true;
                        }
                    }
                    //關門
                    else if(isOpen == true)
                    {
                        DoorCloseSfx();
                    }
                }
                //門鎖住
                else if(Input.GetKeyDown(KeyCode.E) && PuzzleObjController.puzzleDictionary["Key_Student"] == false)
                {
                    DoorLocked();
                }
            }
            break;

            //教務處門判斷
            case "GeneralDoor":
            {
                if(Input.GetKeyDown(KeyCode.E) && isSwitch0 == false && PuzzleObjController.puzzleDictionary["Key_General"] == true)
                {
                    //開門
                    if(isOpen == false)
                    {
                        DoorOpenSfx();
                    }
                    //關門
                    else if(isOpen == true)
                    {
                        DoorCloseSfx();
                    }
                }
                //門鎖住
                else if(Input.GetKeyDown(KeyCode.E) && PuzzleObjController.puzzleDictionary["Key_General"] == false)
                {
                    DoorLocked();
                }
            }
            break;

            //生物教室門判斷
            case "BiologyDoor":
            {
                if(Input.GetKeyDown(KeyCode.E) && isSwitch0 == false && PuzzleObjController.puzzleDictionary["Key_Biology"] == true)
                {
                    //開門
                    if(isOpen == false)
                    {
                        DoorOpenSfx();
                    }   
                    //關門
                    else if(isOpen == true)
                    {
                        DoorCloseSfx();
                    }
                }      
                //門鎖住
                else if(Input.GetKeyDown(KeyCode.E) && PuzzleObjController.puzzleDictionary["Key_Biology"] == false)
                {
                    DoorLocked();
                }
            }
            break;
            
            //圖書館門判斷
            case "LibraryDoor":
            {
                if(OutsideTheSchoolPUN.ServerConnect.playerID == 1)
                {
                    if(Input.GetKeyDown(KeyCode.E) && isSwitch0 == false && PuzzleObjController.puzzleDictionary["StudentCardP1"] == true)
                    {
                        //開門
                        if(isOpen == false)
                        {
                            DoorOpenSfx();
                        }     
                        //關門
                        else if(isOpen == true)
                        {
                            DoorCloseSfx();
                        }
                    }          
                    //門鎖住
                    else if(Input.GetKeyDown(KeyCode.E) && PuzzleObjController.puzzleDictionary["StudentCardP1"] == false)
                    {
                        DoorLocked();
                    }
                }
                else if(OutsideTheSchoolPUN.ServerConnect.playerID == 2)
                {
                    if(Input.GetKeyDown(KeyCode.E) && isSwitch0 == false && PuzzleObjController.puzzleDictionary["StudentCardP2"] == true)
                    {
                        //開門
                        if(isOpen == false)
                        {
                            DoorOpenSfx();
                        }        
                        //關門
                        else if(isOpen == true)
                        {
                            DoorCloseSfx();
                        }
                    }     
                    //門鎖住
                    else if(Input.GetKeyDown(KeyCode.E) && PuzzleObjController.puzzleDictionary["StudentCardP2"] == false)
                    {
                        DoorLocked();
                    }
                }
            }
            break;

            //一般門判斷
            default:
            {
                if(Input.GetKeyDown(KeyCode.E) && isSwitch0 == false)
                {
                    //開門
                    if(isOpen == false)
                    {
                        DoorOpenSfx();
                    }        
                    //關門
                    else if(isOpen == true)
                    {
                        DoorCloseSfx();
                    }
                }
            }
            break;
        }
    }
    
    //連線機關控制
    private void Switch()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            //GameObject Tag判斷
            switch(this.gameObject.tag)
            {
                //鐵捲門開關
                case "IronDoorSwitch":
                {
                    if(PuzzleObjController.puzzleDictionary["IronDoorSwitch"] == false && isOpen == false && isSwitch0 == false)
                    {
                        //鐵捲門開關開啟
                        PuzzleObjController.puzzleDictionary["IronDoorSwitch"] = true;
                
                        //同步玩家鐵捲門打開
                        view.RPC("IrondoorOpen",RpcTarget.All);

                        //開啟鐵捲門文字
                        if(PlayerText.playerTextTriggeredDictionary["IronDoorOpenText"] == false)
                        {
                            PlayerText.playerTextTriggeredDictionary["IronDoorOpenText"] = true;
                        }
                    }
                }
                break;

                //蒸飯室引爆
                case "SteamBoxSwitch":
                {
                    if(PuzzleObjController.puzzleDictionary["SteamBoxSwitch"] == false)
                    {
                        //蒸飯箱引爆成功
                        PuzzleObjController.puzzleDictionary["SteamBoxSwitch"] = true;
                        smoke.SetActive(true);
                        StartCoroutine("smokeCD");
                        audioSource.Play();

                        //長廊黑影消失
                        view.RPC("GallerySmoke",RpcTarget.All);
                    }
                }
                break;
            }
        }   
    }

    #region 門相關控制
    //開門音效
    private void DoorOpenSfx()
    {
        view.RPC("DoorOpen",RpcTarget.All);
        audioSource.clip = audioClips[1];
        audioSource.Play();
    }
    
    //關門音效
    private void DoorCloseSfx()
    {
        view.RPC("DoorClose",RpcTarget.All);
        audioSource.clip = audioClips[0];
        audioSource.Play();
    }

    //門鎖住文字
    private void DoorLocked()
    {
        if(isSwitch1 == false)
        {
            isSwitch1 = true;
            text.text = "門鎖住了。";
            animation.clip = animationClips[1];
            animation.Play();
            StartCoroutine(textCD());
        }
    }

    //開門動畫
    [PunRPC]
    public void DoorOpen()
    {
        isSwitch0 = true;
        animator.SetBool("Open",true);
        StartCoroutine(timeCD());
    }

    //關門動畫
    [PunRPC]
    public void DoorClose()
    {
        isSwitch0 = true;
        animator.SetBool("Open",false);
        StartCoroutine(timeCD());
    }

    //鐵捲門開門
    [PunRPC]
    public void IrondoorOpen()
    {
        irondoors.SetActive(false);
        DoorOpen();
        audioSource.Play();
    }
    #endregion

    //長廊黑影消失
    [PunRPC]
    public void GallerySmoke()
    {
        gallerysmoke.SetActive(false);
    }

    //動畫CD
    IEnumerator timeCD()
    {
        yield return new WaitForSeconds(1f);
        isSwitch0 = false;
    }

    //文自動畫CD
    IEnumerator textCD()
    {
        yield return new WaitForSeconds(2.5f);
        isSwitch1 = false;
        animation.clip = animationClips[0];
        animation.Play();  
    }

    //蒸飯箱失火CD
    IEnumerator smokeCD()
    {
        yield return new WaitForSeconds(20f);
        smoke.SetActive(false);
    }
}