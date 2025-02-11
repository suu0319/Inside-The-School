using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class PlayerName : MonoBehaviourPunCallbacks
{
    //P1P2玩家名稱Text
    public Text playername;
    //P1P2玩家內容Text
    public Text[] playertext;
    // Start is called before the first frame update
    void Start()
    {
        //取得玩家名稱Text
        playername = this.gameObject.GetComponent<Text>();

        //P1玩家
        if(OutsideTheSchoolPUN.ServerConnect.playerID == 1)
        {
            playername.text = "你是阿浩";
            playertext[1].color = Color.gray;
            playertext[3].color = Color.gray;
            playertext[6].color = Color.gray;
        }
        //P2玩家
        else if(OutsideTheSchoolPUN.ServerConnect.playerID == 2)
        {
            playername.text = "你是阿凱";
            playertext[0].color = Color.gray;
            playertext[2].color = Color.gray;
            playertext[5].color = Color.gray;
        }
        
        playertext[4].color = new Color(0.6f,0,0,1f);
    }
}