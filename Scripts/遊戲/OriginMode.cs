using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using OutsideTheSchoolPUN;

public class OriginMode : MonoBehaviour
{
    //場景編號
    private int scenenum;
    //是否存過檔
    private bool isSaved = false;
    //是否取得存檔、是否為無敵模式
    public static bool isGetSavedData,isGod = false;
    
    // Start is called before the first frame update
    void Start()
    {
        //取得場景編號
        scenenum = SceneManager.GetActiveScene().buildIndex; 

        //取得存檔
        isSaved = GameData.GameData.isSave0 = GameData.GameData.inttobool(PlayerPrefs.GetInt("isSave0"));
        //正長時間速度
        Time.timeScale = 1f;     
        //隱藏滑鼠        
        Cursor.visible = false;
        //把滑鼠鎖定到螢幕中間
        Cursor.lockState = CursorLockMode.Locked;
        //原始血量 
        Player.currentHP = 100;      
        //原始體力   
        Player.currentAP = 100;          
        //角色是否活躍(動畫 音效等)  
        Player.isPlayerActive = true;  
        //視角是否可以轉動
        MouseLook.isMouseLook = true;  
        //選取第一個Button
        DeviceSelect.isKeyboardUsed = false; 
        //玩家ID
        ServerConnect.isPlayerP1 = false;
        ServerConnect.isPlayerP2 = false;
        //手電筒開關判定
        Player.isSpotLightOpen = false;
        //UI出現判定
        ButtonEInteractionObj.isPuzzleUiAppear = false;
        //無敵模式
        isGod = false;

        //尚未讀檔
        if(scenenum == 0)
        {
            isGetSavedData = false;
        }
        //取得初始資料
        else if(scenenum == 2)
        {
            GameObject.Find("GameData").SendMessage("OriginData");
        }
    }
}