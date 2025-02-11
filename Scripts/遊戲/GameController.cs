using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using OutsideTheSchoolPUN;
using Photon.Pun;

public class GameController : MonoBehaviour
{
    //PhotonView
    private PhotonView view;
    //遊戲GameObject (玩家、遊戲UI)
    public GameObject player,allui,stopmenu,menu,setting,Loading,isDeadui,lobbyui,waittingplayerui,replayui;
    //死亡UI GameObject
    public GameObject[] isDeaduiclose;
    //前導Text、等待Text、Loading數字
    public Text storytext,waittingttext,loadingtextcontent;
    //設定選單狀況保留
    public Slider audioslider;
    public Toggle fullscreen;
    public Dropdown quality,resolutions;
    //場景編號
    private int scenenum;
    //死亡、Coroutine開關、P1P2玩家重新遊玩是否準備
    private bool isDead,isSwitch0,isSwitch1,isSwitch2,isP1Replay,isP2Replay = false;
    
    void Start()
    {
        //限制FPS
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 120;
        
        //GetComponent
        view = this.gameObject.GetComponent<PhotonView>();
        player = GameObject.FindGameObjectWithTag("Player");
        scenenum = SceneManager.GetActiveScene().buildIndex; 
        
        //主選單
        if(scenenum == 0)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }      
    }
        
    void Update()
    {
        //主選單
        if(scenenum == 0)
        {
            if(Input.GetKeyDown("escape") && lobbyui.activeInHierarchy == true)
            {
                BackMenu();
            }
        }

        //判斷玩家是否死亡
        Dead();
    }

    //玩家死亡
    public void Dead()
    {
        if(Player.currentHP == 0 && isDead == false)
        {
            isDead = true;
            isDeadui.SetActive(true);
            stopmenu.SetActive(false);
        }
    }

    //連線大廳(開始新遊戲)
    public void Lobby()
    {
        GameObject.Find("EventSystem").GetComponent<DeviceSelect>().enabled = false;
        AppearCursor();
        OriginMode.isGetSavedData = false;
        PuzzleObjController.puzzleDictionary["End"] = false;
        menu.SetActive(false);
        lobbyui.SetActive(true);
    }
    
    //連線大廳(繼續遊戲)
    public void ContinueLobby()
    {
        GameObject.Find("EventSystem").GetComponent<DeviceSelect>().enabled = false;
        AppearCursor();
        OriginMode.isGetSavedData = true;
        PuzzleObjController.puzzleDictionary["End"] = false;
        menu.SetActive(false);
        lobbyui.SetActive(true);
    }

    //設定選單
    public void Setting()
    {
        menu.SetActive(false);
        setting.SetActive(true);
        DeviceSelect.isKeyboardUsed = false;
    }

    //設定選單返回主選單or暫定選單
    public void BackMenu()
    {
        GameObject.Find("EventSystem").GetComponent<DeviceSelect>().enabled = true;
        menu.SetActive(true);
        setting.SetActive(false);
        OriginMode.isGetSavedData = false;
        
        if(scenenum == 0)
        {
            lobbyui.SetActive(false);
        }
    }

    //死亡畫面返回遊戲主選單
    public void BackToMenu()
    {
        Time.timeScale = 1f;
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadSceneAsync(0);
    }

    //退出遊戲房間
    public void QuitGameRoom()
    {
        //退出連線房間
        PhotonNetwork.LeaveRoom();
        //連線大廳
        lobbyui.SetActive(true);
        //等待玩家
        waittingplayerui.SetActive(false);
        waittingplayerui.GetComponent<WaittingForPlayerText>().waittingttext.text = "尋找玩家中";
        waittingplayerui.GetComponent<WaittingForPlayerText>().isTextAnim = false;
        DeviceSelect.isKeyboardUsed = false;
        //玩家12控制
        OutsideTheSchoolPUN.ServerConnect.isPlayerP1 = false;
        OutsideTheSchoolPUN.ServerConnect.isPlayerP2 = false;
        OutsideTheSchoolPUN.ServerConnect.playerID = 0;
    }

    //選單回復到遊戲中
    public void Continue()
    {
        Player.isPlayerActive = true;
        allui.SetActive(true);
        stopmenu.SetActive(false);
        MouseLook.isMouseLook = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1f;
    }

    //重新遊玩
    public void Replay()
    {      
        OriginMode.isGetSavedData = true;
        replayui.SetActive(true);

        for(int i = 0 ; i < 4; i++)
        {
            isDeaduiclose[i].SetActive(false);
        }
         
        //玩家1
        if(OutsideTheSchoolPUN.ServerConnect.playerID == 1)
        {
            view.RPC("P1replay",RpcTarget.All);
        } 
        //玩家2
        else if(OutsideTheSchoolPUN.ServerConnect.playerID == 2)
        {
            view.RPC("P2replay",RpcTarget.All);
        }
    }

    //退出遊戲
    public void Quit()
    {
        Application.Quit();
    }

    //出現游標
    void AppearCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    //P1玩家準備
    [PunRPC]
    public void P1replay()
    {
        isP1Replay = true;
        
        if(isSwitch0 == false)
        {
            isSwitch0 = true;
            StartCoroutine(ReplayCD());
        }
    }

    //P2玩家準備
    [PunRPC]
    public void P2replay()
    {
        isP2Replay = true;
        
        if(isSwitch1 == false)
        {
            isSwitch1 = true;
            StartCoroutine(ReplayCD());
        }
    }

    //重新遊玩
    IEnumerator ReplayCD()
    {
        yield return new WaitForSeconds(1f);
        
        if(isP1Replay == true && isP2Replay == true && replayui.activeInHierarchy == true && isSwitch2 == false)
        {
            isSwitch2 = true;
            StartCoroutine(LoadLevelAsync());
        }
    }

    //異步加載Scene1
    IEnumerator LoadLevelAsync()
    {
        PhotonNetwork.LoadLevel("Scene1");

        while (PhotonNetwork.LevelLoadingProgress < 1)
        {
            loadingtextcontent.text = (int)(PhotonNetwork.LevelLoadingProgress * 100) + "%";
            yield return new WaitForEndOfFrame();
        }     
    }
}