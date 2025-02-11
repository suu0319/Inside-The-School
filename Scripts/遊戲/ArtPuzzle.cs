using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArtPuzzle : MonoBehaviour
{
    //藝術裝置謎題GameObject
    public GameObject art,ui;
    //藝術裝置AudioSource
    private AudioSource audioSources;
    //藝術裝置AudioClip
    public AudioClip[] audioClips;
    //藝術裝置正確次數計算
    public int i = 0;

    // Start is called before the first frame update
    void Start()
    {
        //GetComponent
        audioSources = this.gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //無敵模式
        OriginMode.isGod = true;
        
        //P2玩家
        if(OutsideTheSchoolPUN.ServerConnect.playerID == 2)
        {
            //開啟謎題UI
            if(ui.activeInHierarchy == false)
            {
                ui.SetActive(true);
            }
            //謎題完成
            else if(i == 5 && PuzzleObjController.puzzleDictionary["ArtPuzzle"] == false)
            {
                PuzzleObjController.puzzleDictionary["ArtPuzzle"] = true;
                Player.isPlayerActive = true;
                MouseLook.isMouseLook = true;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                this.gameObject.SetActive(false);
            }
        }
    }

    //UI向右旋轉
    public void RightRotate()
    {
        //向右旋轉
        art.transform.Rotate(0f,0f,-90f);
        
        if(i == 1 || i == 4)
        {
            audioSources.clip = audioClips[1];
            audioSources.Play();
            
            i += 1;
        }
        else
        {
            audioSources.clip = audioClips[0];
            audioSources.Play();

            i = 0;
        }
    }

    //UI向左旋轉
    public void LeftRotate()
    {
        //向左旋轉
        art.transform.Rotate(0f,0f,90f);
        
        if(i == 0 || i == 2 || i == 3)
        {
            audioSources.clip = audioClips[1];
            audioSources.Play();

            i += 1;
        }
        else
        {
            audioSources.clip = audioClips[0];
            audioSources.Play();

            i = 0;
        }
    }
}
