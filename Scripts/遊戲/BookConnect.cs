using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BookConnect : MonoBehaviour
{
    //PhotonView
    private PhotonView view;
    //書本謎題通關是否同步
    private bool isSwitch0 = false;

    // Start is called before the first frame update
    void Start()
    {
        //GetComponent
        view = this.gameObject.GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        //書本謎題通關
        if(PuzzleObjController.puzzleDictionary["BookPuzzle"] == true && isSwitch0 == false)
        {
            isSwitch0 = true;
            view.RPC("Correct",RpcTarget.All);
        }
    }

    //書本謎題同步通關
    [PunRPC]
    public void Correct()
    {
        PuzzleObjController.puzzleDictionary["BookPuzzle"] = true;
    }
}