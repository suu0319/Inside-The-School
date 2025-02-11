using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class Options : MonoBehaviour
{
    [SerializeField]
    OptionsData data;
    //靜態設定選單變數
    public float audiovolume;
    public bool Fullscreen;
    public int quality,resolutionsnum,fullscreenbool;
    
    //audioMixer (總音量混音控制)
    public AudioMixer audioMixer;
    
    //解析度+畫質UI
    public Dropdown resolutionDropdown,qualityDropdown;
    //全屏UI
    public Toggle fullscreentoggle;
    //音量UI
    public Slider volumeslider;
    //解析度陣列
    private Resolution[] resolutions;

    void Awake() 
    {
        //讀取上次的設定資料
        volumeslider.value = data.volume = PlayerPrefs.GetFloat("volume");
        resolutionsnum = data.resolution = PlayerPrefs.GetInt("resolutionsnum");
        qualityDropdown.value = quality = data.quality = PlayerPrefs.GetInt("quality");
        data.fullscreen = PlayerPrefs.GetInt("fullscreen");

        if(data.fullscreen == 1)
        {
            fullscreentoggle.isOn = true;
        }
        else
        {
            fullscreentoggle.isOn = false;
        }
    }
    
    //取得初始設定資料
    void Start()
    {
        audioMixer.SetFloat("volume",data.volume);

        resolutions = Screen.resolutions.Select(resolution => new Resolution { width = resolution.width, height = resolution.height }).Distinct().ToArray();
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();

        int currentResolutionIndex = 0;
        
        for(int i = 0 ; i<resolutions.Length ; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);

            if(resolutions[i].width == Screen.width &&
            resolutions[i].height == Screen.height)
            {
                currentResolutionIndex = i;
            } 
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = resolutionsnum;
        resolutionDropdown.RefreshShownValue();
    }

    //設置總音量
    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume",volume);
        audiovolume = volume;
        PlayerPrefs.SetFloat("volume",audiovolume);
    }

    //設置解析度
    public void SetResolution(int resolutionIndex)
    {
        resolutionsnum = resolutionIndex;
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width,resolution.height,Screen.fullScreen);
        PlayerPrefs.SetInt("resolutionsnum",resolutionsnum);
    }
    
    //設置畫質
    public void SetQuality(int qualityIndex)
    {
        quality = qualityIndex;
        QualitySettings.SetQualityLevel(qualityIndex);
        PlayerPrefs.SetInt("quality",quality);
    }

    //設置全螢幕
    public void SetFullscreen(bool isFullscreen)
    {
        Fullscreen = isFullscreen;
        Screen.fullScreen = isFullscreen;
        
        if(Fullscreen == true)
        {
            fullscreenbool = 1;
            PlayerPrefs.SetInt("fullscreen",fullscreenbool);
        }
        else
        {
            fullscreenbool = 0;
            PlayerPrefs.SetInt("fullscreen",fullscreenbool);
        }
    }
}

//設定類別
[System.Serializable]
public class OptionsData
{
    //音量
    public float volume;
    //解析度
    public int resolution;
    //畫質
    public int quality;
    //全螢幕
    public int fullscreen;
}   