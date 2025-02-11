using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class Ending : MonoBehaviour
{
    //PhotonView
    private PhotonView view;
    //結局相關GameObject (遊戲UI、Camera、Text)
    public GameObject rawui,playercam1,playercam2,waitting,ending;
    //等待玩家Text
    public Text waittingtext;
    //結局Trigger
    public BoxCollider trigger;
    //結局AudioSource
    public AudioSource audioSource;
    //P1P2玩家是否完成任務、是否進入結局Trigger
    public bool isPassP1,isPassP2,isTriggerEnterP1,isTriggerEnterP2 = false;
    //結局是否準備撥放
    public static bool isEndReady = false;

    // Start is called before the first frame update
    void Start()
    {
        //GetComponent
        view = this.gameObject.GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        //尋找P1玩家的Camera
        if(OutsideTheSchoolPUN.ServerConnect.playerID == 1 && playercam1 == null)
        {
            playercam1 = GameObject.Find("Player1").transform.GetChild(0).gameObject;
        }
        //尋找P1玩家2的Camera
        else if(OutsideTheSchoolPUN.ServerConnect.playerID == 2 && playercam2 == null)
        {
            playercam2 = GameObject.Find("Player2").transform.GetChild(0).gameObject;
        }

        //P1玩家
        if(OutsideTheSchoolPUN.ServerConnect.playerID == 1)
        {
            if(PuzzleObjController.puzzleDictionary["PasswordCorrect"] == true)
            {
                view.RPC("PassP1",RpcTarget.All);
            }
        }
        //P2玩家
        else if(OutsideTheSchoolPUN.ServerConnect.playerID == 2)
        {
            if(PuzzleObjController.puzzleDictionary["DollsPuzzle"] == true)
            {
                view.RPC("PassP2",RpcTarget.All);
            }
        }

        //玩家是否完成任務
        if(isPassP1 == true && isPassP2 == true)
        {
            trigger.enabled = true;
        }

        //玩家是否進入結局Trigger
        if(isTriggerEnterP1 == true && isTriggerEnterP2 == true)
        {
            audioSource.Stop();
        }
    }

    //停留Trigger
    void OnTriggerStay(Collider other) 
    {
        //P1玩家停留Trigger
        if(other.name == "Player1" && OutsideTheSchoolPUN.ServerConnect.playerID == 1)
        {
            isEndReady = true;
            waittingtext.text = "等待阿凱吧!";
            
            view.RPC("TriggerEnterP1",RpcTarget.All);

            if(isTriggerEnterP1 == true && isTriggerEnterP2 == true && PuzzleObjController.puzzleDictionary["End"] == false)
            {
                Destroy(playercam1);
                Destroy(waitting);
                PuzzleObjController.puzzleDictionary["End"] = true;
                OriginMode.isGod = true;
                rawui.SetActive(false);
                ending.SetActive(true);
                StartCoroutine("AnimCD");
            }
            else
            {
                waitting.SetActive(true);
            }
        }
        //P2玩家停留Trigger
        else if(other.name == "Player2" && OutsideTheSchoolPUN.ServerConnect.playerID == 2)
        {
            isEndReady = true;
            waittingtext.text = "等待阿浩吧!";

            view.RPC("TriggerEnterP2",RpcTarget.All);

            if(isTriggerEnterP1 == true && isTriggerEnterP2 == true && PuzzleObjController.puzzleDictionary["End"] == false)
            {
                Destroy(playercam2);
                Destroy(waitting);
                PuzzleObjController.puzzleDictionary["End"] = true;
                OriginMode.isGod = true;
                rawui.SetActive(false);
                ending.SetActive(true);
                StartCoroutine("AnimCD");
            }
            else
            {
                waitting.SetActive(true);
            }
        }
    }

    //離開Trigger
    void OnTriggerExit(Collider other)
    {
        //P1玩家離開Trigger
        if(other.name == "Player1" && OutsideTheSchoolPUN.ServerConnect.playerID == 1)
        {
            isEndReady = false;
            view.RPC("TriggerExitP1",RpcTarget.All);
            waitting.SetActive(false);
        }
        //P2玩家離開Trigger
        else if(other.name == "Player2" && OutsideTheSchoolPUN.ServerConnect.playerID == 2)
        {
            isEndReady = false;
            view.RPC("TriggerExitP2",RpcTarget.All);
            waitting.SetActive(false);
        }
    }

    //玩家是否完成任務、進入結局Trigger
    #region  玩家是否完成任務、進入結局Trigger
    [PunRPC]
    public void PassP1()
    {
        isPassP1 = true;
    }

    [PunRPC]
    public void PassP2()
    {
        isPassP2 = true;
    }

    [PunRPC]
    public void TriggerEnterP1()
    {
        isTriggerEnterP1 = true;
    }

    [PunRPC]
    public void TriggerEnterP2()
    {
        isTriggerEnterP2 = true;
    }

    [PunRPC]
    public void TriggerExitP1()
    {
        isTriggerEnterP1 = false;
    }

    [PunRPC]
    public void TriggerExitP2()
    {
        isTriggerEnterP2 = false;
    }
    #endregion

    //結局動畫Coroutine
    IEnumerator AnimCD()
    {
        yield return new WaitForSeconds(13.5f);
        PhotonNetwork.LoadLevel(0);
    }
}
