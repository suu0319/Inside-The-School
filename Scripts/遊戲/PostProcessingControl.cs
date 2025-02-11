using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Photon.Pun;
//using UnityEngine.Rendering.PostProcessing;

public class PostProcessingControl : MonoBehaviour
{
    //PostProcessing Volume
    private Volume postprocessing;
    private WhiteBalance whitebalance;
    private ColorCurves colorcurves;
    private FilmGrain filmgrain;
    private ChromaticAberration chromatic;
    //PostProcessing AudioSource
    private AudioSource audioSource;
    //PostProcessing AudioClip
    public AudioClip[] audioClips;
    //濾鏡參數、音效是否觸發
    private bool isFilmGrain,isFilmGrainEnd,isPlaySfx = false;
    
    // Start is called before the first frame update
    void Start()
    {
        //GetComponent
        postprocessing = this.gameObject.GetComponent<Volume>();
        audioSource = this.gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //P1玩家
        if(OutsideTheSchoolPUN.ServerConnect.playerID == 1)
        {
            //引爆蒸飯室
            if(PuzzleObjController.puzzleDictionary["SteamBoxSwitch"] == true)
            {
                PostProcessingP1();
            }   
        }
        //P2玩家
        else if(OutsideTheSchoolPUN.ServerConnect.playerID == 2)
        {
            //進入二樓範圍內
            if(Trigger.isEnter2ndFloor == true)
            {
                PostProcessingP2();
            }
        }
    }

    //P1玩家濾鏡
    public void PostProcessingP1()
    {
        if(postprocessing.profile.TryGet(out filmgrain) && postprocessing.profile.TryGet(out colorcurves) && postprocessing.profile.TryGet(out chromatic))
        {
            if(filmgrain.intensity.value < 1f && isFilmGrain == false && isFilmGrainEnd == false && colorcurves.active == false && GameData.GameData.isSave1 == false)
            {
                if(isPlaySfx == false)
                {
                    isPlaySfx = true;
                    audioSource.clip = audioClips[0];
                    audioSource.Play();
                }
                
                StartCoroutine(P1FilmGraintimer());
                isFilmGrain = true;
            }
            else if(Player.currentHP !=0 && filmgrain.intensity.value == 1f && isFilmGrainEnd == true && colorcurves.active == false || (GameData.GameData.isSave1 == true && colorcurves.active == false))
            {
                audioSource.clip = audioClips[1];
                audioSource.volume = 0.3f;
                audioSource.loop = true;
                audioSource.Play();
                filmgrain.intensity.value = 0.01f;
                colorcurves.active = true;
            }
        }
    }

    //P2玩家濾鏡
    public void PostProcessingP2()
    {
        if(postprocessing.profile.TryGet(out filmgrain))
        {
            if(filmgrain.intensity.value < 1f && isFilmGrain == false && isFilmGrainEnd == false && GameData.GameData.isSave1 == false)
            {
                StartCoroutine(P2FilmGraintimer());
                isFilmGrain = true;
            }
            else if(Player.currentHP !=0 && filmgrain.intensity.value == 1f && isFilmGrainEnd == true || (GameData.GameData.isSave1 == true))
            {
                filmgrain.intensity.value = 0.01f;
            }
        }
    }

    //P1雜訊Coroutine
    IEnumerator P1FilmGraintimer()
    {
        yield return new WaitForSeconds(0.01f);
        
        chromatic.intensity.value += 0.006f;
        filmgrain.intensity.value += 0.006f;

        if(filmgrain.intensity.value == 1f)
        {
            isFilmGrainEnd = true;
        }

        isFilmGrain = false;
    }

    //P2雜訊Coroutine
    IEnumerator P2FilmGraintimer()
    {
        yield return new WaitForSeconds(0.1f);
        filmgrain.intensity.value += 0.075f;
        
        if(filmgrain.intensity.value == 1f)
        {
            isFilmGrainEnd = true;
        }
        
        isFilmGrain = false;
    }
}