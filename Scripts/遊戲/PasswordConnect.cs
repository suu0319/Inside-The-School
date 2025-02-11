using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PasswordConnect : MonoBehaviour
{
    //PhotonView
    private PhotonView view;
    //密碼謎題是否同步
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
        //連線謎題通關
        if(PuzzleObjController.puzzleDictionary["PasswordCorrect"] == true && isSwitch0 == false)
        {
            isSwitch0 = true;
            view.RPC("Correct",RpcTarget.All);
        }
    }

    //同步連線謎題通關
    [PunRPC]
    public void Correct()
    {
        PuzzleObjController.puzzleDictionary["PasswordCorrect"] = true;
    }
}
