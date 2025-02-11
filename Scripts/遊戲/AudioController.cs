using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class AudioController : MonoBehaviour
{
    //All AudioSource of Game
    public AudioSource[] audioSources;

    // Update is called once per frame
    void Update()
    {
        //玩家人數 < 2 關閉AudioSource.enabled
        if(PhotonNetwork.CurrentRoom.PlayerCount < 2)
        {
            for(int i = 0 ; i < audioSources.Length ; i++)
            {
                audioSources[i].enabled = false;
            }
        }
    }
}
