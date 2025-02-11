using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PianoPuzzle : MonoBehaviour
{
    //鋼琴謎題正確次數計算
    public int i = 0;
    //鋼琴AudioSource
    private AudioSource audioSource;
    //鋼琴AudioClip
    public AudioClip[] audioClips;
    //鋼琴Animator
    public Animator animator;
    //鋼琴謎題GameObject (學生證、ui)
    public GameObject studentcard,ui2d,uiclose;
    //樂譜UI GameObject
    public GameObject[] sheets;

    // Start is called before the first frame update
    void Start()
    {
        //初始化
        i = 0;
        
        //GetComponent
        audioSource = this.gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //P1玩家 鋼琴謎題
        if(OutsideTheSchoolPUN.ServerConnect.playerID == 1 && PuzzleObjController.puzzleDictionary["PianoFinish"] == false)
        {
            if(i == 6)
            {
                Time.timeScale = 1f;
                ButtonEInteractionObj.isPuzzleUiAppear = false;      
                Player.isPlayerActive = true;
                MouseLook.isMouseLook = true;
                ui2d.SetActive(false);
                uiclose.SetActive(false);

                PuzzleObjController.puzzleDictionary["PianoFinish"] = true;
                animator.SetBool("Open",true);
                studentcard.SetActive(true);
                audioSource.clip = audioClips[7];
                audioSource.Play();
            }
            else if(Input.GetKeyDown(KeyCode.Q))
            {
                i = 0;

                audioSource.clip = audioClips[0];
                audioSource.Play();

                for(int y = 0 ; y < 5 ; y++)
                {
                    if(sheets[y].activeInHierarchy == true)
                    {
                        sheets[y].SetActive(false);
                    }
                 }
            }  
            else if(Input.GetKeyDown(KeyCode.W))
            {
                i = 0;

                audioSource.clip = audioClips[1];
                audioSource.Play();

                for(int y = 0 ; y < 5 ; y++)
                {
                    if(sheets[y].activeInHierarchy == true)
                    {
                        sheets[y].SetActive(false);
                    }
                }
            } 
            else if(Input.GetKeyDown(KeyCode.E))
            {
                if(i == 3)
                {
                    i += 1;
                    sheets[3].SetActive(true);
                }
                else
                {
                    i = 0;

                    for(int y = 0 ; y < 5 ; y++)
                    {
                        if(sheets[y].activeInHierarchy == true)
                        {
                            sheets[y].SetActive(false);
                        }
                    }
                }

                audioSource.clip = audioClips[2];
                audioSource.Play();
            }  
            else if(Input.GetKeyDown(KeyCode.R))
            {
                i = 0;

                audioSource.clip = audioClips[3];
                audioSource.Play();

                for(int y = 0 ; y < 5 ; y++)
                {
                    if(sheets[y].activeInHierarchy == true)
                    {
                        sheets[y].SetActive(false);
                    }
                }
            } 
            else if(Input.GetKeyDown(KeyCode.T))
            {
                i = 0;

                audioSource.clip = audioClips[4];
                audioSource.Play();

                for(int y = 0 ; y < 5 ; y++)
                {
                    if(sheets[y].activeInHierarchy == true)
                    {
                        sheets[y].SetActive(false);
                    }
                }
            } 
            else if(Input.GetKeyDown(KeyCode.Y))
            {
                if(i == 0)
                {
                    i += 1;
                    sheets[0].SetActive(true);
                }
                else if(i == 5)
                {
                    i += 1;
                    sheets[5].SetActive(true);
                }
                else
                {
                    i = 0;

                    for(int y = 0 ; y < 5 ; y++)
                    {
                        if(sheets[y].activeInHierarchy == true)
                        {
                            sheets[y].SetActive(false);
                        }
                    }
                }
                
                audioSource.clip = audioClips[5];
                audioSource.Play();
            }
            else if(Input.GetKeyDown(KeyCode.U))
            {
                if(i == 1)
                {
                    i += 1;
                    sheets[1].SetActive(true);
                }
                else if(i == 2)
                {
                    i += 1;
                    sheets[2].SetActive(true);
                }
                else if(i == 4)
                {
                    i += 1;
                    sheets[4].SetActive(true);
                }
                else
                {
                    i = 0;

                    for(int y = 0 ; y < 5 ; y++)
                    {
                        if(sheets[y].activeInHierarchy == true)
                        {
                            sheets[y].SetActive(false);
                        }
                    }
                }
                
                audioSource.clip = audioClips[6];
                audioSource.Play();
            }
        }
        //P2玩家 鋼琴謎題
        else if(OutsideTheSchoolPUN.ServerConnect.playerID == 2 && PuzzleObjController.puzzleDictionary["PianoFinish"] == false)
        {
            if(i == 6)
            {
                Time.timeScale = 1f;
                ButtonEInteractionObj.isPuzzleUiAppear = false;      
                Player.isPlayerActive = true;
                MouseLook.isMouseLook = true;
                ui2d.SetActive(false);
                uiclose.SetActive(false);

                PuzzleObjController.puzzleDictionary["PianoFinish"] = true;
                animator.SetBool("Open",true);
                studentcard.SetActive(true);
                audioSource.clip = audioClips[7];
                audioSource.Play();
            }
            else if(Input.GetKeyDown(KeyCode.Q))
            {
                if(i == 1)
                {
                    i += 1;
                    sheets[1].SetActive(true);
                }
                else
                {
                    i = 0;

                    for(int y = 0 ; y < 5 ; y++)
                    {
                        if(sheets[y].activeInHierarchy == true)
                        {
                            sheets[y].SetActive(false);
                        }
                    }
                }
                
                audioSource.clip = audioClips[0];
                audioSource.Play();
            }
            else if(Input.GetKeyDown(KeyCode.W))
            {
                if(i == 5)
                {
                    i += 1;
                    sheets[5].SetActive(true);
                }
                else
                {
                    i = 0;

                    for(int y = 0 ; y < 5 ; y++)
                    {
                        if(sheets[y].activeInHierarchy == true)
                        {
                            sheets[y].SetActive(false);
                        }
                    }
                }
                
                audioSource.clip = audioClips[1];
                audioSource.Play();
            }
            else if(Input.GetKeyDown(KeyCode.E))
            {
                if(i == 3)
                {
                    i += 1;
                    sheets[3].SetActive(true);
                }
                else if(i == 4)
                {
                    i += 1;
                    sheets[4].SetActive(true);
                }
                else
                {
                    i = 0;

                    for(int y = 0 ; y < 5 ; y++)
                    {
                        if(sheets[y].activeInHierarchy == true)
                        {
                            sheets[y].SetActive(false);
                        }
                    }
                }
                
                audioSource.clip = audioClips[2];
                audioSource.Play();
            }
            else if(Input.GetKeyDown(KeyCode.R))
            {
                if(i == 0)
                {
                    i += 1;
                    sheets[0].SetActive(true);
                }
                else
                {
                    i = 0;

                    for(int y = 0 ; y < 5 ; y++)
                    {
                        if(sheets[y].activeInHierarchy == true)
                        {
                            sheets[y].SetActive(false);
                        }
                    }
                }
                
                audioSource.clip = audioClips[3];
                audioSource.Play();
            }
            else if(Input.GetKeyDown(KeyCode.T))
            {
                if(i == 2)
                {
                    i += 1;
                    sheets[2].SetActive(true);
                }
                else
                {
                    i = 0;

                    for(int y = 0 ; y < 5 ; y++)
                    {
                        if(sheets[y].activeInHierarchy == true)
                        {
                            sheets[y].SetActive(false);
                        }
                    }
                }
                
                audioSource.clip = audioClips[4];
                audioSource.Play();
            }
            else if(Input.GetKeyDown(KeyCode.Y))
            {
                i = 0;

                for(int y = 0 ; y < 5 ; y++)
                {
                    if(sheets[y].activeInHierarchy == true)
                    {
                        sheets[y].SetActive(false);
                    }
                }

                audioSource.clip = audioClips[5];
                audioSource.Play();
            } 
            else if(Input.GetKeyDown(KeyCode.U))
            {
                i = 0;

                for(int y = 0 ; y < 5 ; y++)
                {
                    if(sheets[y].activeInHierarchy == true)
                    {
                        sheets[y].SetActive(false);
                    }
                }

                audioSource.clip = audioClips[6];
                audioSource.Play();
            } 
        }
    }
}
