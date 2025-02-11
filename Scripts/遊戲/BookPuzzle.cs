using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BookPuzzle : MonoBehaviour
{
    //書本謎題AudioSource
    private AudioSource audioSource;
    //書本謎題相關GameObject (怪物、P1P2文字)
    public GameObject ghost1,ghost2,zombie1,zombie2,hatelight1,hatelight2,textP1,textP2;
    //書本謎題GameObject (所有書本、交換的兩本書本)
    public GameObject[] book,bookchange;
    //書本座標紀錄 (第一本書)
    public Vector3 bookchanged0;
    //正確書本座標
    public Vector3[] bookposition;
    //交換書本Transform (交換的所有書本，交換的兩本書本)
    public Transform[] booktransform,booktransformchange;
    //選擇書本數量
    public int i = 0;

    void Start()
    {
        //GetComponent
        audioSource = this.gameObject.GetComponent<AudioSource>();

        //P1玩家
        if(OutsideTheSchoolPUN.ServerConnect.playerID == 1)
        {
            ghost1.SetActive(true);
            ghost2.SetActive(true);
        }
        //P2玩家
        else if(OutsideTheSchoolPUN.ServerConnect.playerID == 2)
        {
            hatelight1.SetActive(false);
            hatelight2.SetActive(false);
            zombie1.SetActive(true);
            zombie2.SetActive(true);
        }
    }

    void Update()
    {
        //無敵模式
        OriginMode.isGod = true;
        
        //P1玩家
        if(OutsideTheSchoolPUN.ServerConnect.playerID == 1)
        {
            if(this.gameObject.activeInHierarchy == true)
            {
                textP1.SetActive(true);
            }
        }

        //P2玩家
        if(OutsideTheSchoolPUN.ServerConnect.playerID == 2)
        {
            if(this.gameObject.activeInHierarchy == true && PuzzleObjController.puzzleDictionary["BookPuzzle"] == false)
            {
                textP2.SetActive(true);
            }
            if((booktransform[0].transform.localPosition == bookposition[0]) && (booktransform[1].transform.localPosition == bookposition[1])
            && (booktransform[2].transform.localPosition == bookposition[2]) && (booktransform[3].transform.localPosition == bookposition[3])
            && (booktransform[4].transform.localPosition == bookposition[4]) && PuzzleObjController.puzzleDictionary["BookPuzzle"] == false)
            {
                PuzzleObjController.puzzleDictionary["BookPuzzle"] = true;
                Player.isPlayerActive = true;
                MouseLook.isMouseLook = true;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                this.gameObject.SetActive(false);
            }
        }
    }

    //Button OnClick (5本書)
    #region Button onClick
    public void book0()
    {
        ChangePosition(0);
    }
    public void book1()
    {
        ChangePosition(1);
    }
    public void book2()
    {
        ChangePosition(2);
    }
    public void book3()
    {
        ChangePosition(3);
    }
    public void book4()
    {
        ChangePosition(4);
    }
    #endregion

    //交換座標function
    public void ChangePosition(int x)
    {
        //交換的第一本書
        if(i == 0)
        {
            i += 1;
            booktransformchange[0] = booktransform[x];
            bookchanged0 = booktransformchange[0].transform.position;
            bookchange[0] = book[x];
        }
        //交換的第二本書
        else if(i == 1)
        {
            i = 0;
            audioSource.Play();
            booktransformchange[1] = booktransform[x];
            bookchange[1] = book[x];
            bookchange[0].transform.position = booktransformchange[1].transform.position;
            bookchange[1].transform.position = bookchanged0;
        }
    }
}
