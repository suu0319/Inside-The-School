using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ThreePuzzleController : MonoBehaviour
{
    //PhotonView
    private PhotonView view;
    //謎題GameObject
    public GameObject[] puzzle;
    //謎題是否觸發出現
    private bool isSwitch0,isSwitch1 = false;

    // Start is called before the first frame update
    void Start()
    {
        view = this.gameObject.GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        //P1玩家
        if(OutsideTheSchoolPUN.ServerConnect.playerID == 1)
        {
            if(puzzle[0].activeInHierarchy == true && isSwitch0 == false)
            {
                isSwitch0 = true;
                puzzle[1].SetActive(true);
                puzzle[2].SetActive(true);
                puzzle[3].SetActive(true);
            }
        }
        //P2玩家
        else if(OutsideTheSchoolPUN.ServerConnect.playerID == 2)
        {
            if(PuzzleObjController.puzzleDictionary["BookPuzzle"] == true && isSwitch1 == false)
            {
                isSwitch1 = true;
                puzzle[4].SetActive(true);
                puzzle[5].SetActive(true);
                puzzle[6].SetActive(true);
            }
            if(PuzzleObjController.puzzleDictionary["DollsPuzzle"] == true)
            {
                view.RPC("End",RpcTarget.All);
            }
        }
    }

    //結局
    [PunRPC]
    public void End()
    {
        puzzle[7].SetActive(true);
    }
}
