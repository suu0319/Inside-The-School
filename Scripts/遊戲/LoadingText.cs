using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class LoadingText : MonoBehaviourPunCallbacks
{
    //PhotonView
    private PhotonView view;
    //Coroutine CD P1P2玩家是否跳過
    private bool isSwitch0,isSwitch1,isP1Skip,isP2Skip = false;
    //Loading文字GameObject
    public GameObject text,skiptext,playername;
    //跳過文字
    public Text skiptextcontent;
    //Text Animation
    public Animation animation;
    //異步加載
    AsyncOperation async;
    
    void Start() 
    {
        //游標隱藏
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        //GetComponent
        animation = skiptext.GetComponent<Animation>();
        skiptextcontent = skiptext.GetComponent<Text>();
        view = this.gameObject.GetComponent<PhotonView>();
    }

    void Update() 
    {
        LoadingReady();
    }

    //讀檔準備
    public void LoadingReady()
    {
        //讀檔判斷
        switch(OriginMode.isGetSavedData)
        {
            //開始新遊戲
            case false:
            {
                if(isSwitch0 == false)
                {
                    isSwitch0 = true;
                    StartCoroutine(Skiploading());
                }
                
                if(Input.anyKeyDown && skiptext.activeInHierarchy == true)
                {
                    //P1玩家
                    if(OutsideTheSchoolPUN.ServerConnect.playerID == 1)
                    {
                        view.RPC("P1SkipLoading",RpcTarget.All);
                        skiptextcontent.text = "等待玩家跳過";
                    }
                    //P2玩家
                    else if(OutsideTheSchoolPUN.ServerConnect.playerID == 2)
                    {
                        view.RPC("P2SkipLoading",RpcTarget.All);
                        skiptextcontent.text = "等待玩家跳過";
                    }
                }
            }
            break;
            
            //繼續遊戲
            case true:
            {
                if(isSwitch0 == false)
                {
                    view.RPC("LoadGame",RpcTarget.All);
                }
            }
            break;
        }
    }

    //P1玩家跳過
    [PunRPC]
    public void P1SkipLoading()
    {
        isP1Skip = true;
        if(isP1Skip == true && isP2Skip == true && isSwitch1 == false)
        {
            isSwitch1 = true;
            StartCoroutine(LoadLevelAsync());
        }
    }

    //P2玩家跳過
    [PunRPC]
    public void P2SkipLoading()
    {
        isP2Skip = true;
        if(isP1Skip == true && isP2Skip == true && isSwitch1 == false)
        {
            isSwitch1 = true;
            StartCoroutine(LoadLevelAsync());
        }
    }

    //載入Scene
    [PunRPC]
    public void LoadGame()
    {
        isSwitch0 = true;
        text.SetActive(false);
        skiptext.SetActive(true);
        playername.SetActive(false);
        StartCoroutine(LoadLevelAsync());
    }
    
    //跳過Text出現
    IEnumerator Skiploading()
    {
        yield return new WaitForSeconds(8f);
        skiptext.SetActive(true);
    }

    //異步加載Scene1
    IEnumerator LoadLevelAsync()
    {
        PhotonNetwork.LoadLevel("Scene1");

        while (PhotonNetwork.LevelLoadingProgress < 1)
        {
            skiptextcontent.text = (int)(PhotonNetwork.LevelLoadingProgress * 100) + "%";
            yield return new WaitForEndOfFrame();
        }     
    }
}