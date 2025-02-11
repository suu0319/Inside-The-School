using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PuzzleObjController : MonoBehaviour
{
    //PhotonView
    private PhotonView view;
    //木頭人GameObject
    public GameObject woodghost;
    //P1P2玩家GameObject、P2玩家謎題垃圾桶
    public GameObject[] PlayerNumber,PlayerP2;
    //805謎題是否完成、木頭人是否消失
    private bool isEightZeroFive,isWoodGhostDisappear = false;
    //謎題GameObject是否完成
    public static Dictionary<string,bool> puzzleDictionary = new Dictionary<string, bool>()
    {
        {"Key_Student",false},{"Key_Biology",false},{"Key_General",false},
        {"Key_BiologyPuzzle",false},{"FireExtinguish",false},{"SmokeBool",false},
        {"IronDoorPuzzle",false},{"IronDoorSwitch",false},{"EightPuzzle",false},
        {"ZeroPuzzle",false},{"FivePuzzle",false},{"SheetMusic902",false},
        {"SheetMusic802",false},{"PianoFinish",false},{"SteamBoxPuzzle",false},
        {"SteamBoxSwitch",false},{"StudentCardP1",false},{"StudentCardP2",false},
        {"BookPuzzle",false},{"PasswordCorrect",false},{"ArtPuzzle",false},
        {"DollsPuzzle",false},{"End",false}
    };
    
    void Awake() 
    {
        //GetComponent
        view = this.gameObject.GetComponent<PhotonView>();

        //P1玩家
        if(OutsideTheSchoolPUN.ServerConnect.playerID == 1)
        {
            PlayerNumber[0].SetActive(true);
        }
        //P2玩家
        else if(OutsideTheSchoolPUN.ServerConnect.playerID == 2)
        {
            PlayerNumber[1].SetActive(true);
        }
    }

    void Update()
    {
        //805謎題判定
        if(puzzleDictionary["EightPuzzle"]  == true && puzzleDictionary["ZeroPuzzle"] == true && puzzleDictionary["FivePuzzle"] == true && isEightZeroFive == false) 
        {
            isEightZeroFive = true;
            view.RPC("Trash",RpcTarget.All);
        }

        //蒸飯箱謎題判定
        if(puzzleDictionary["SteamBoxSwitch"] == true && isWoodGhostDisappear == false)
        {
            isWoodGhostDisappear = true;
            woodghost.SetActive(false);
        }
    }

    //垃圾桶謎題
    [PunRPC]
    public void Trash()
    {
        PlayerP2[0].SetActive(false);
        PlayerP2[1].SetActive(true);
    }
}