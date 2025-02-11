using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace OutsideTheSchoolPUN
{
    public class ServerConnect : MonoBehaviourPunCallbacks
    {
        //PhotonView
        private PhotonView view;
        //連線房間InputField roomID
        public InputField roomID;
        //連線房間GameObject
        public GameObject lobby,waittingplayer,joinfail;
        //Text Animation
        public Animation animation;
        //Text AnimationClip
        public AnimationClip[] animationClips;
        //連線相關Bool
        private bool isGameStart,isJoinFailText,isFixReturnBug = false;
        //P1P2玩家是否進入連線房間
        public static bool isPlayerP1,isPlayerP2 = false;
        //玩家ID
        public static int playerID { get; set; }
 
        // Start is called before the first frame update
        void Start()
        {
            //遊戲結束，離開連線房間
            if(PuzzleObjController.puzzleDictionary["End"] == true)
            {
                PhotonNetwork.LeaveRoom();
            }
            
            //離開連線房間
            if(PhotonNetwork.IsConnected)
            {
                PhotonNetwork.Disconnect();
            }

            //PUN連線設定
            PhotonNetwork.ConnectUsingSettings();

            //GetComponent
            view = this.gameObject.GetComponent<PhotonView>();
            animation = joinfail.GetComponent<Animation>();
            
            //PUN自動同步
            PhotonNetwork.AutomaticallySyncScene = true;
        }

        void Update()
        {
            //進入連線大廳
            if(roomID.IsActive())
            {   
                roomID.ActivateInputField();
                StartCoroutine(ReturnBug());

                if(Input.GetKeyDown(KeyCode.Return) && isFixReturnBug == true)
                {
                    JoinOrCreatePrivateRoom();
                }
            }
            //尚未進入連線大廳
            else
            {
                isFixReturnBug = false;
            }

            //玩家準備完畢 進入遊戲
            if(isPlayerP1 == true && isPlayerP2 == true && isGameStart == false)
            {
                isGameStart = true;
                PhotonNetwork.LoadLevel("前導");
            }
        }

        //連線至Photon伺服器
        public override void OnConnectedToMaster()
        {
            Debug.Log("成功連線至伺服器");
        }
        
        //創建私人連線房間
        private void JoinOrCreatePrivateRoom()
        {
            //空白房間ID
            if(roomID.text == "")
            {
                if(isJoinFailText == false)
                {
                    isJoinFailText = true;
                    joinfail.SetActive(true);
                    StartCoroutine(Joinfailcount());
                }
            }
            
            //房間設定
            RoomOptions roomOptions = new RoomOptions { MaxPlayers = 2 };
            PhotonNetwork.JoinOrCreateRoom(roomID.text,roomOptions,null);
            roomOptions.IsVisible = false;
        }

        //加入連線房間
        public override void OnJoinedRoom()
        {
            //玩家人數 < 2 等待玩家加入
            if(PhotonNetwork.CurrentRoom.PlayerCount < 2) 
            {
                lobby.SetActive(false);
                waittingplayer.SetActive(true);
            }
            
            //玩家人數 = 1 創建房間
            if(PhotonNetwork.CurrentRoom.PlayerCount == 1)
            {
                playerID = 1;
                view.RPC("PlayerCreateRoom",RpcTarget.All);
            }
            //玩家人數 = 2 加入房間
            else if(PhotonNetwork.CurrentRoom.PlayerCount == 2)
            {
                playerID = 2;
                view.RPC("PlayerJoinRoom",RpcTarget.All);
            }
        }

        //創建連線房間失敗
        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            //創建失敗文字
            if(isJoinFailText == false)
            {
                isJoinFailText = true;
                joinfail.SetActive(true);
                StartCoroutine(Joinfailcount());
            }
        }
        
        //加入連線房間失敗
        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            //加入失敗文字
            if(isJoinFailText == false)
            {
                isJoinFailText = true;
                joinfail.SetActive(true);
                StartCoroutine(Joinfailcount());
            }
        }

        //P1玩家創建連線房間
        [PunRPC]
        public void PlayerCreateRoom()
        {
            isPlayerP1 = true;
            //Debug.Log("玩家1");
        }
    
        //P2玩家加入連線房間
        [PunRPC]
        public void PlayerJoinRoom()
        {
            isPlayerP2 = true;
            //Debug.Log("玩家2");
        }
        
        //加入失敗文字動畫
        IEnumerator Joinfailcount()
        {
            animation.clip = animationClips[1];
            animation.Play();
            yield return new WaitForSeconds(3f);
            joinfail.SetActive(false);
            isJoinFailText = false;
        }

        //防止Enter輸入兩次
        IEnumerator ReturnBug()
        {
            yield return new WaitForEndOfFrame();
            isFixReturnBug = true;
        }
    }
}

