using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Trigger : MonoBehaviour
{
    //PhotonView
    private PhotonView view;
    //Trigger相關GameObject (怪物、血跡)
    public GameObject hatelightghost1,hatelightghost2,woodghost,woodghostfake,blood;
    //AudioSource
    public AudioSource audioSource;
    //AudioClip
    public AudioClip[] audioClips;
    //Animator
    public Animator animator;
    //Coroutine CD
    private bool isSwitch,isSwitch2,isSwitch3,isSwitch4,shining = false;
    //是否進入Trigger
    public static bool isSmoke,isEnter2ndFloor = false;

    void Start()
    {
        //GetComponent
        audioSource = this.gameObject.GetComponent<AudioSource>();
    }

    //進入Trigger
    void OnTriggerEnter(Collider other)
    {
        view = other.gameObject.GetComponent<PhotonView>();
        
        if(view.IsMine)
        {
            if(other.gameObject.tag == "Player")
            {
                //GameObject Name判斷
                switch(this.gameObject.name)
                {
                    //失火濃煙
                    case "Smoke":
                    {
                        if(isSmoke == false)
                        {
                            isSmoke = true;
                        }
                    }
                    break;
                
                    //上二樓
                    case "進入二樓":
                    {
                        //P1玩家
                        if(OutsideTheSchoolPUN.ServerConnect.playerID == 1)
                        {
                            if(isEnter2ndFloor == false)
                            {
                                isEnter2ndFloor = true;
                                woodghostfake.SetActive(false);
                                woodghost.SetActive(true);
                                audioSource.clip = audioClips[0];
                                audioSource.Play();
                                StartCoroutine(PianoSfx());
                            }
                        }
                        //P2玩家
                        else if(OutsideTheSchoolPUN.ServerConnect.playerID == 2)
                        {
                            if(isEnter2ndFloor == false)
                            {
                                isEnter2ndFloor = true;
                                hatelightghost1.SetActive(true);
                                hatelightghost2.SetActive(true);
                                audioSource.clip = audioClips[1];
                                audioSource.Play();
                                StartCoroutine(PianoSfx());
                            }
                        }
                    }
                    break;

                    //國文課本廁所JunpScare
                    case "國文課本JumpScare":
                    {
                        if(isSwitch == false)
                        {
                            isSwitch = true;
                            animator.SetBool("Open",true);
                            audioSource.Play();
                        }
                    }
                    break;

                    //P1玩家聯絡簿血跡
                    case "P1聯絡簿血跡":
                    {
                        if(isSwitch2 == false)
                        {
                            isSwitch2 = true;
                            blood.SetActive(true);
                            animator.SetBool("Open",true);
                            audioSource.Play();
                        }
                    }
                    break;

                    //P2玩家聯絡簿血跡
                    case "P2聯絡簿血跡":
                    {
                        if(isSwitch3 == false)
                        {
                            isSwitch3 = true;
                            blood.SetActive(true);
                            animator.SetBool("Open",true);
                            audioSource.Play();
                        }
                    }
                    break;

                    //P1玩家音樂教室黑板血跡
                    case "P1音樂教室黑板血跡":
                    {
                        if(isSwitch4 == false)
                        {
                            isSwitch4 = true;
                            blood.SetActive(true);
                            audioSource.Play();
                        }
                    }
                    break;
                }
            }
        }
    }

    //泥娃娃鋼琴聲
    IEnumerator PianoSfx()
    {
        for(int i = 0 ; i < 15 ; i++)
        {
            yield return new WaitForSeconds(1f);
            audioSource.volume -= 0.075f;
        }
    }
}