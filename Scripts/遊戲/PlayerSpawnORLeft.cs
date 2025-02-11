using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
using Photon.Pun;

public class PlayerSpawnORLeft : MonoBehaviourPunCallbacks
{
    //PhotonView
    private PhotonView view;
    //玩家連線相關GameObject
    public GameObject PlayerPrefs,Disconnect,stopmenu;
    //P1P2玩家GameObject
    public GameObject[] Playerobj;
    //P1P2玩家CharacterController
    public CharacterController[] PlayerCollider;
    //玩家座標
    public static float Player1x,Player1y,Player1z,
                        Player2x,Player2y,Player2z;
    //出生、斷線是否同步
    private bool spawnremote,disconnect  = false;
    
    void Start() 
    {
        view = this.gameObject.GetComponent<PhotonView>();
        
        PhotonNetwork.MinimalTimeScaleToDispatchInFixedUpdate = 1f;

        //P1玩家
        if(OutsideTheSchoolPUN.ServerConnect.playerID == 1)
        {
            if(GameData.GameData.isSave0 == true && OriginMode.isGetSavedData == true)
            {
                PlayerPrefs.transform.position = new Vector3(Player1x,Player1y,Player1z);
            }
            else if(GameData.GameData.isSave1 == true && OriginMode.isGetSavedData == true)
            {
                PlayerPrefs.transform.position = new Vector3(Player1x,Player1y,Player1z);
            }
            else
            {
                PlayerPrefs.transform.position = new Vector3(9.209f,0.252f,-43.461f);
            }
        }
        //P2玩家
        else if(OutsideTheSchoolPUN.ServerConnect.playerID == 2)
        {
            if(GameData.GameData.isSave0 == true && OriginMode.isGetSavedData == true)
            {
                PlayerPrefs.transform.position = new Vector3(Player2x,Player2y,Player2z);
            }
            else if(GameData.GameData.isSave1 == true && OriginMode.isGetSavedData == true)
            {
                PlayerPrefs.transform.position = new Vector3(Player2x,Player2y,Player2z);
            }
            else
            {
                PlayerPrefs.transform.position = new Vector3(49.529f,0.252f,-43.376f);
            }
        }
        
        //生成玩家
        PhotonNetwork.Instantiate(PlayerPrefs.name,PlayerPrefs.transform.position,Quaternion.identity);
    }
    
    void Update()
    {
        PlayerStatement();
    }

    //玩家離開房間
    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        view.RPC("disconnectfuntion",RpcTarget.All);
    }

    //玩家連線狀態
    public void PlayerStatement()
    {
        //玩家人數 < 2 全部離開房間
        if(PhotonNetwork.CurrentRoom.PlayerCount < 2)
        {
            view.RPC("disconnectfuntion",RpcTarget.All);
        }
        
        //玩家ID = 1 P1玩家重生
        if(OutsideTheSchoolPUN.ServerConnect.playerID == 1)
        {
            view.RPC("Player1SpawnFind",RpcTarget.All);
        }
        //玩家ID = 2 P2玩家重生
        else if(OutsideTheSchoolPUN.ServerConnect.playerID == 2)
        {
            view.RPC("Player2SpawnFind",RpcTarget.All);
        }

        //P1P2玩家CharacterController 忽略碰撞
        if(PlayerCollider[0] != null && PlayerCollider[1] != null)
        {
            Physics.IgnoreCollision(PlayerCollider[0],PlayerCollider[1]);
        }
    }

    //尋找玩家1
    [PunRPC]
    public void Player1SpawnFind()
    {
        if(Playerobj[0] == null)
        {
            Playerobj[0] = GameObject.Find(PlayerPrefs.name + "(Clone)");
            Playerobj[0].name = "Player1";
        }
           
        //Get CharacterController
        PlayerCollider[0] = Playerobj[0].GetComponent<CharacterController>();
    }

    //尋找玩家2
    [PunRPC]
    public void Player2SpawnFind()
    {
        if(Playerobj[1] == null)
        {
            Playerobj[1] = GameObject.Find(PlayerPrefs.name + "(Clone)");
            Playerobj[1].name = "Player2";
        }
           
        //Get CharacterController   
        PlayerCollider[1] = Playerobj[1].GetComponent<CharacterController>();
    }

    //斷線
    [PunRPC]
    public void disconnectfuntion()
    {
        Time.timeScale = 1f;
        Disconnect.SetActive(true);
        PhotonNetwork.LeaveRoom();
        
        if(disconnect == false)
        {
            disconnect = true;
            Player.isPlayerActive = false;  
            StartCoroutine(DisconnectUI());
        }
    }

    //斷線異步加載主選單
    IEnumerator DisconnectUI()
    {
        yield return new WaitForSeconds(8f);
        SceneManager.LoadScene(0);
    }
}