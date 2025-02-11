using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class Player : MonoBehaviourPunCallbacks
{
    //PhotonView
    private PhotonView view;
    //PostProcessing
    public Volume postprocessing;
    //暈眩效果 (postprocessing 參數)
    private ChromaticAberration chromatic;
    private Vignette vignette;
    private DepthOfField depth;
    //角色AudioClip                  
    public AudioClip[] audioClips;
    //角色AudioSource 
    private AudioSource audioSource;
    //角色Transform 
    public Transform groundCheck;
    //groundMask為掉落要碰到的圖層名稱
    public LayerMask groundMask;
    //轉向殺手的座標
    private Vector3 ghostlook;
    //加速度
    private Vector3 velocity;
    //CharacterController
    public CharacterController controller;
    //角色相關GameObject
    public GameObject playercamera,postprocessingobj,canvas,spotlight,allui,stopmenu,backmenuUI,ReplayUI,RawUI,buttonEUI;      
    public GameObject[] ghost;      
    //場景編號
    private int sceneindex;         
    //角色各參數Float
    public float speed,gravity,JumpHeight,distance,groundDistance;
    //角色是否在地板上 
    public bool isGrounded; 
    //角色相關bool (手電筒是否使用、是否死亡、是否疲憊、是否回復體力、是否跑步、Coroutine)
    private bool isSpotLightUse,isDead,isTired,isReAP,isRunning,isSwitch0 = false;
    //手電筒是否開關、玩家狀態是否活躍
    public static bool isSpotLightOpen,isPlayerActive = false;
    //血量
    private static int hp;
    public static int currentHP 
    { 
        get
        {
            return hp;
        } 
        set
        {
            if(value < 0) value = 0;
            hp = value;
        }
    }       
    //體力
    private static int ap;
    public static int currentAP 
    { 
        get
        {
            return ap;
        } 
        set
        {
            if(value < 0) value = 0;
            ap = value;
        }
    }      

    void Awake()
    {
        view = this.gameObject.GetComponent<PhotonView>();
        
        if(!view.IsMine)
        {
            Destroy(GetComponentInChildren<Camera>().gameObject);
            Destroy(GetComponentInChildren<AudioListener>());
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //抓取Canvas
        canvas = GameObject.Find("Canvas");
        //抓取MainCamera
        playercamera = this.gameObject.transform.GetChild(0).gameObject;
        //抓取角色控制器
        controller = this.gameObject.GetComponent<CharacterController>();
        //抓取地板判定
        groundCheck = this.gameObject.transform.GetChild(2);
        //取得角色音效撥放器
        audioSource = this.gameObject.GetComponent<AudioSource>();
        //音效循環 = false
        audioSource.loop = false;
        //獲取場景編號
        sceneindex = SceneManager.GetActiveScene().buildIndex;
        //尋找UI
        allui = canvas.transform.GetChild(1).gameObject;
        //Raw UI
        RawUI = allui.transform.GetChild(0).gameObject;
        //ButtonE UI
        buttonEUI = RawUI .transform.GetChild(1).gameObject;
        //尋找暫停選單
        stopmenu = canvas.transform.GetChild(3).gameObject;
        //尋找Postprocessing
        postprocessingobj = GameObject.Find("PostProcessing Volume");
        postprocessing = postprocessingobj.GetComponent<Volume>();
        //尋找手電筒
        spotlight = this.gameObject.transform.GetChild(1).gameObject;
    }

    void Update()
    {
        if(view.IsMine)
        {
            CharacterControl(); 
        }   
    } 
   
    //角色控制
    // ReSharper disable Unity.PerformanceAnalysis
    public void CharacterControl()
    {
        //手電筒控制
        isSpotLightOpen = isSpotLightUse;
            
        //AP控制
        currentAP = Mathf.Clamp(currentAP,0,100);

        //怪物
        ghost = GameObject.FindGameObjectsWithTag("Ghost");
    
        //作弊模式1
        if(Input.GetKeyDown(KeyCode.P))
        {
            speed = 10f;
            currentHP = 9999;
            currentAP = 9999;
            OriginMode.isGod = true;
        }
        //作弊模式2
        if(Input.GetKeyDown(KeyCode.I))
        {
            PuzzleObjController.puzzleDictionary["PasswordCorrect"] = true;
            PuzzleObjController.puzzleDictionary["DollsPuzzle"] = true;
        }
        //瞬移
        if(Input.GetKeyDown(KeyCode.L))
        {
            StartCoroutine(OPMove());
        }
        
        //角色活躍 = true
        if(isPlayerActive == true)
        {
            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

            if(isGrounded && velocity.y < 0)
            {
                velocity.y = -2f;
            }

            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");
            
            //transform.right = X軸 , transform.forward = Z軸
            Vector3 move = transform.right * x + transform.forward * z;

            //移動
            controller.Move(move * speed * Time.deltaTime);

            //疲勞濾鏡效果
            if(postprocessing.profile.TryGet(out vignette) && postprocessing.profile.TryGet(out chromatic) && postprocessing.profile.TryGet(out depth))
            {
                //跳躍
                if(Input.GetButtonDown("Jump") && isGrounded && speed != 1.5f)
                {
                    //Mathf.Sqrf = 取平方根 (根據物理的結果)
                    velocity.y = Mathf.Sqrt(JumpHeight * -2f * gravity);
                    currentAP = currentAP - 10;
                    
                    if(vignette.intensity.value < 0.5f)
                    {
                        vignette.intensity.value += 0.05f;
                    }
                    
                    if(depth.focalLength.value < 200)
                    {
                        depth.focalLength.value += 20;
                    }
                    
                    chromatic.intensity.value += 0.1f;
                }
                
                //跑步
                if(Input.GetKey(KeyCode.LeftShift) && currentAP > 0 && isTired == false && isSwitch0 == false)
                {   
                    isSwitch0 = true;
                    isRunning  = true;
                    isReAP = false;
                    speed = 6f;
                    StartCoroutine(Runtime());
                }
                
                //取消跑步 or 沒體力
                if((Input.GetKeyUp(KeyCode.LeftShift) || currentAP <= 0))
                {
                    isRunning = false;
                    isReAP = true;
                    
                    if(currentAP <= 0)
                    {
                        isTired = true;
                        speed = 1.5f;
                    }
                    else
                    {
                        speed = 3.5f;
                    }
                }
                
                //恢復體力
                if(((isRunning == false && isReAP == true) || isGrounded) && isSwitch0 == false)
                {
                    isSwitch0 = true;
                    StartCoroutine(isReAPtime());
                }
            }
          
            //開關手電筒
            if(Input.GetKeyDown(KeyCode.F))
            {                  
                audioSource.clip = audioClips[1];
                audioSource.loop = false;
                audioSource.Play();         
                view.RPC("spotlightRPC",RpcTarget.All);
            }

            //暫停選單
            if(Input.GetKeyDown(KeyCode.Tab) && Ending.isEndReady == false)
            {
                GameObject.Find("EventSystem").GetComponent<DeviceSelect>().enabled = true;
                stopmenu.SetActive(true);
                allui.SetActive(false);
                isPlayerActive = false;
                MouseLook.isMouseLook = false;
                audioSource.Stop();
                Time.timeScale = 0f;
                DeviceSelect.isKeyboardUsed = false;
            }
                
            //體力正常狀態
            if(audioSource.isPlaying && currentAP >= 40)
            {
                isTired = false;
                audioSource.clip = audioClips[1];
                audioSource.loop = false;
            }
            //疲勞狀態
            else if(!audioSource.isPlaying && currentAP < 40)
            {
                audioSource.clip = audioClips[0];
                audioSource.loop = true;
                audioSource.Play();
            }
      
            //跳躍重力
            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);    
        }
        
        //死亡
        for(int i = 0 ; i < ghost.Length ; i++)
        {
            //作弊模式3
            if(Input.GetKeyDown(KeyCode.O))
            {
                ghost[i].SetActive(false);
            }
            
            //與怪物的距離
            distance = Vector3.Distance(ghost[i].transform.position,this.transform.position);
    
            if(distance < 2.5f && OriginMode.isGod == false)
            {
                switch(ghost[i].name)
                {
                    //木頭人
                    case "木頭人":
                    {
                        if(RayControl.isWoodGhostStop == false)
                        {
                            ghostlook = ghost[i].transform.position - transform.GetChild(0).position;
                            PlayerDead();
                        }
                    }
                    break;
                    
                    //厭光怪
                    case "厭光怪1": case "厭光怪2":
                    {
                        if(RayControl.isHateLightGhostTrack == true && isSpotLightOpen == true)
                        {
                            ghostlook = ghost[i].transform.position - transform.GetChild(0).position;
                            PlayerDead();
                        }
                    } 
                    break;

                    //靈界鬼魂
                    case "靈界鬼魂1": case "靈界鬼魂2": case "殭屍1": case "殭屍2":
                    {
                        ghostlook = ghost[i].transform.position - transform.GetChild(0).position;
                        PlayerDead();
                    } 
                    break;
                }
            }
        }
    }

    //玩家死亡
    public void PlayerDead()
    {
        view.RPC("GameOver",RpcTarget.All);
        //殺手角度
        Quaternion rotation = Quaternion.LookRotation(new Vector3(ghostlook.x,ghostlook.y + 1.5f,ghostlook.z));
        //緩慢轉向殺手         
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation , 150f * Time.deltaTime);
    }

    //同步開關手電筒
    [PunRPC]
    void spotlightRPC()
    {
        isSpotLightUse = !isSpotLightUse;
        spotlight.GetComponent<Light>().enabled = isSpotLightUse;   
    }

    //同步GAMEOVER
    [PunRPC]
    void GameOver()
    {
        Time.timeScale = 1f;
        currentHP = 0;
        isPlayerActive = false;
    }

    //跑步
    IEnumerator Runtime()
    {
        currentAP = currentAP - 1;
        
        if(vignette.intensity.value < 0.5f)
        {
            vignette.intensity.value += 0.005f;
        }
        if(depth.focalLength.value < 200)
        {
            vignette.intensity.value += 0.005f;
        }
        
        vignette.intensity.value += 0.005f;
        
        yield return new WaitForSeconds(0.1f);
        isSwitch0 = false;
    }

    //回復AP
    IEnumerator isReAPtime()
    {
        if(currentAP <= 0)
        {
            yield return new WaitForSeconds(3f);
        }
        
        currentAP = currentAP + 1;
        vignette.intensity.value -= 0.005f;
        chromatic.intensity.value -= 0.01f;
        depth.focalLength.value -= 2;
        yield return new WaitForSeconds(0.1f);
        isSwitch0 = false;
    }

    //瞬移至圖書館
    IEnumerator OPMove()
    {
        yield return new WaitForEndOfFrame();
        this.gameObject.transform.position = new Vector3(50.4f,6f,-4.9f);
    }
}