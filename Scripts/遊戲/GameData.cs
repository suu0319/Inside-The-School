using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using OutsideTheSchoolPUN;

namespace GameData
{
    public class GameData : MonoBehaviour
    {
        [SerializeField]
        PlayerData data;
        //存檔TextGameObject
        public GameObject SaveTextObj;
        //場景GameObject
        public GameObject[] Player1Obj,Player2Obj,Player12Obj;
        //存檔Text
        public Text SaveText;
        //存檔Text Animation
        public Animation animation;
        //存檔Text AnimationClip
        public AnimationClip[] animationClips;
        //場景編號
        private int scenenum;
        //是否觸發存檔TextAnimation
        private bool SaveAnimationBool = false;
        //P1玩家存檔資料
        private string[] PlayerP1Data = new string[]
        {
            "Key_Student","Key_Biology","SheetMusic902",
            "Key_BiologyPuzzle","StudentCardP1","IronDoorPuzzle"
            ,"PianoFinish","SteamBoxSwitch",
        };
        //P2玩家存檔資料
        private string[] PlayerP2Data = new string[]
        {
            "FireExtinguish","Key_General","SheetMusic802",
            "StudentCardP2","SteamBoxPuzzle","PianoFinish",
            "IronDoorSwitch","EightPuzzle","ZeroPuzzle",
            "FivePuzzle","SmokeBool"
        };
        //P2玩家存檔Text
        private string[] PlayerP2Text = new string[]
        {
            "isSmokeTrigger","GeneralText","IronDoorOpenText",
            "EightPuzzleText","ToiletText","SteamBoxText"
        };
        //玩家存檔點觸發
        public static bool isSave0,isSave1 = false;

        //Singleton
        private static GameData _singleton = null;
        
        public static GameData singleton
        {
            get
            {
                if(_singleton == null)
                {
                    _singleton = new GameData();
                }
                
                return _singleton;
            }
        }

        void Awake()
        {
            if(_singleton != null)
            {
                return;
            }
            
            _singleton = this;
        }
        
        // Start is called before the first frame update
        void Start()
        {
            //GetComponent
            scenenum = SceneManager.GetActiveScene().buildIndex;
            SaveTextObj = GameObject.Find("存檔點Text");
            SaveText = SaveTextObj.GetComponent<Text>();
            animation = SaveTextObj.GetComponent<Animation>();
            
            //結局結束
            if(PuzzleObjController.puzzleDictionary["End"] == true)
            {
                SceneManager.LoadScene(0);
            }
        }

        //初始資料
        public void OriginData()
        {
            //尚未讀檔(開始新遊戲)
            if(OriginMode.isGetSavedData == false)
            {
                //尚未存檔
                isSave0 = false;
                isSave1 = false;
            
                //玩家共同
                Guide.isGuideClose = false;
                PlayerText.playerTextTriggeredDictionary["isGuideClose"] = false;
                Trigger.isEnter2ndFloor = false;
                
                //書籍謎題
                PuzzleObjController.puzzleDictionary["BookPuzzle"] = false;
                //電腦密碼
                PuzzleObjController.puzzleDictionary["PasswordCorrect"] = false;
                //藝術裝置謎題
                PuzzleObjController.puzzleDictionary["ArtPuzzle"] = false;
                //巫毒娃娃謎題
                PuzzleObjController.puzzleDictionary["DollsPuzzle"] = false;
                //破關準備
                Ending.isEndReady = false;
                //破關
                PuzzleObjController.puzzleDictionary["End"] = false;

                //P1玩家
                if(OutsideTheSchoolPUN.ServerConnect.playerID == 1)
                {
                    //GameObject
                    for(int i = 0; i < PlayerP1Data.Length; i++)
                    {
                        PuzzleObjController.puzzleDictionary[PlayerP1Data[i]] = false;
                    }

                    //Text
                    PlayerText.playerTextTriggeredDictionary["StudentText"] = false;
                    PlayerText.playerTextTriggeredDictionary["IronDoorText"] = false;
                }
                //P2玩家
                else if(OutsideTheSchoolPUN.ServerConnect.playerID == 2)
                {     
                    //GameObject
                    for(int i = 0; i < PlayerP2Data.Length; i++)
                    {
                        PuzzleObjController.puzzleDictionary[PlayerP2Data[i]] = false;
                    }
                
                    //Text
                    for(int i = 0; i < PlayerP2Text.Length; i++)
                    {
                        if(i == 0)
                            Trigger.isSmoke = false; 
                        else
                            PlayerText.playerTextTriggeredDictionary[PlayerP2Text[i]] = false;
                    }                      
                }
            }
            //讀檔(繼續遊戲)
            else
            {
                LoadData();
            }
        }

        //場景資料
        public void SceneData()
        {
            if(OriginMode.isGetSavedData == true && (isSave0 == true || isSave1 == true))
            {
                //玩家GameObject
                Player12Obj[0].SetActive(false);
                Player12Obj[1].SetActive(false);

                //書籍謎題
                PuzzleObjController.puzzleDictionary["BookPuzzle"] = false;
                //電腦密碼
                PuzzleObjController.puzzleDictionary["PasswordCorrect"] = false;
                //藝術裝置謎題
                PuzzleObjController.puzzleDictionary["ArtPuzzle"] = false;
                //巫毒娃娃謎題
                PuzzleObjController.puzzleDictionary["DollsPuzzle"] = false;
                //破關準備
                Ending.isEndReady = false;
                //破關
                PuzzleObjController.puzzleDictionary["End"] = false;
                
                if(isSave1 == true)
                {
                    Player12Obj[2].SetActive(false);
                }
                
                //P1玩家
                if(OutsideTheSchoolPUN.ServerConnect.playerID == 1)
                {
                    for(int i = 0; i < PlayerP1Data.Length; i++)
                    {
                        if(PuzzleObjController.puzzleDictionary[PlayerP1Data[i]] == true && Player1Obj[i])
                        {
                            Player1Obj[i].SetActive(false);
                        }
                    }
                }
                //P2玩家
                else if(OutsideTheSchoolPUN.ServerConnect.playerID == 2)
                {
                    for(int i = 0; i < PlayerP2Data.Length; i++)
                    {
                        if(PuzzleObjController.puzzleDictionary[PlayerP2Data[i]] == true && Player2Obj[i])
                        {
                            Player2Obj[i].SetActive(false);

                            if(i == 5)
                            {
                                Player2Obj[4].SetActive(true);
                                Player2Obj[5].SetActive(true);
                            }
                        }
                    }
                }
            }
        }

        //存檔
        public void SaveData()
        {
            Debug.Log("存檔完成");

            //存檔Text動畫
            if(SaveAnimationBool == false)
            {
                SaveAnimationBool = true;
                animation.clip = animationClips[1];
                animation.Play();
                StartCoroutine(SaveTextAnimation());
            }
            
            //玩家共同
            PlayerPrefs.SetInt("isGuideClose",booltoint(PlayerText.playerTextTriggeredDictionary["isGuideClose"]));
            PlayerPrefs.SetInt("isEnter2ndFloor",booltoint(Trigger.isEnter2ndFloor));

            //P1玩家
            if(OutsideTheSchoolPUN.ServerConnect.playerID == 1)
            {
                //座標
                PlayerPrefs.SetFloat("Player1x",GameObject.Find("Player1").transform.position.x);
                PlayerPrefs.SetFloat("Player1y",GameObject.Find("Player1").transform.position.y);
                PlayerPrefs.SetFloat("Player1z",GameObject.Find("Player1").transform.position.z);
                
                //GameObject
                for(int i = 0; i < PlayerP1Data.Length; i++)
                {
                    PlayerPrefs.SetInt(PlayerP1Data[i],booltoint(PuzzleObjController.puzzleDictionary[PlayerP1Data[i]]));
                }

                //Text
                PlayerPrefs.SetInt("StudentText",booltoint(PlayerText.playerTextTriggeredDictionary["StudentText"]));
                PlayerPrefs.SetInt("IronDoorText",booltoint(PlayerText.playerTextTriggeredDictionary["IronDoorText"]));
            }
            //P2玩家
            else if(OutsideTheSchoolPUN.ServerConnect.playerID == 2)
            {
                //座標
                PlayerPrefs.SetFloat("Player2x",GameObject.Find("Player2").transform.position.x);
                PlayerPrefs.SetFloat("Player2y",GameObject.Find("Player2").transform.position.y);
                PlayerPrefs.SetFloat("Player2z",GameObject.Find("Player2").transform.position.z);
                
                //GameObject
                for(int i = 0; i < PlayerP2Data.Length; i++)
                {
                    PlayerPrefs.SetInt(PlayerP2Data[i],booltoint(PuzzleObjController.puzzleDictionary[PlayerP2Data[i]]));
                }

                //Text
                for(int i = 0; i < PlayerP2Text.Length; i++)
                {
                    if(i == 0)
                        PlayerPrefs.SetInt(PlayerP2Text[i],booltoint(Trigger.isSmoke));
                    else
                        PlayerPrefs.SetInt(PlayerP2Text[i],booltoint(PlayerText.playerTextTriggeredDictionary[PlayerP2Text[i]]));
                }               
            }
            
            //存檔點1觸發存檔
            PlayerPrefs.SetInt("isSave0",booltoint(isSave0));
            //存檔點2觸發存檔
            PlayerPrefs.SetInt("isSave1",booltoint(isSave1));
            //PlayerPrefs To Json
            PlayerPrefs.SetString("savefile",JsonUtility.ToJson(data));
        }

        //讀檔
        public void LoadData()
        {   
            Debug.Log("讀檔完成");
            //Json To PlayerPrefs
            data = JsonUtility.FromJson<PlayerData>(PlayerPrefs.GetString("savefile"));
        
            //玩家共同
            PlayerText.playerTextTriggeredDictionary["isGuideClose"] = data.playerTextTriggeredDictionary["isGuideClose"] = inttobool(PlayerPrefs.GetInt("isGuideClose"));
            Trigger.isEnter2ndFloor = data.playerTextTriggeredDictionary["isEnter2ndFloor"] = inttobool(PlayerPrefs.GetInt("isEnter2ndFloor"));

            //P1玩家
            if(OutsideTheSchoolPUN.ServerConnect.playerID == 1)
            {
                //座標
                PlayerSpawnORLeft.Player1x = data.Player1x = PlayerPrefs.GetFloat("Player1x");
                PlayerSpawnORLeft.Player1y = data.Player1y = PlayerPrefs.GetFloat("Player1y");
                PlayerSpawnORLeft.Player1z = data.Player1z =  PlayerPrefs.GetFloat("Player1z");
                
                //GameObject
                for(int i = 0; i < PlayerP1Data.Length; i++)
                {
                    PuzzleObjController.puzzleDictionary[PlayerP1Data[i]] = data.playerDataDictionary[PlayerP1Data[i]] = inttobool(PlayerPrefs.GetInt(PlayerP1Data[i]));
                }
                
                //Text
                PlayerText.playerTextTriggeredDictionary["StudentText"] = data.playerTextTriggeredDictionary["StudentText"] = inttobool(PlayerPrefs.GetInt("StudentText"));
                PlayerText.playerTextTriggeredDictionary["IronDoorText"] = data.playerTextTriggeredDictionary["IronDoorText"] = inttobool(PlayerPrefs.GetInt("IronDoorText"));
            }
            //P2玩家
            else if(OutsideTheSchoolPUN.ServerConnect.playerID == 2)
            {
                //座標
                PlayerSpawnORLeft.Player2x = data.Player2x = PlayerPrefs.GetFloat("Player2x");
                PlayerSpawnORLeft.Player2y = data.Player2y = PlayerPrefs.GetFloat("Player2y");
                PlayerSpawnORLeft.Player2z = data.Player2z = PlayerPrefs.GetFloat("Player2z");
                
                //GameObject
                //GameObject
                for(int i = 0; i < PlayerP2Data.Length; i++)
                {
                    PuzzleObjController.puzzleDictionary[PlayerP2Data[i]] = data.playerDataDictionary[PlayerP2Data[i]] = inttobool(PlayerPrefs.GetInt(PlayerP2Data[i]));
                }

                //Text
                for(int i = 0; i < PlayerP2Text.Length; i++)
                {
                    if(i == 0)
                        Trigger.isSmoke = data.playerTextTriggeredDictionary["isSmokeTrigger"] = inttobool(PlayerPrefs.GetInt("isSmokeTrigger"));
                    else
                        PlayerText.playerTextTriggeredDictionary[PlayerP2Text[i]] = data.playerTextTriggeredDictionary[PlayerP2Text[i]] = inttobool(PlayerPrefs.GetInt(PlayerP2Text[i]));
                }                
            }

            //存檔點1觸發讀檔
            isSave0 = data.isSave0 = inttobool(PlayerPrefs.GetInt("isSave0"));
            //存檔點2觸發讀檔
            isSave1 = data.isSave1 = inttobool(PlayerPrefs.GetInt("isSave1"));
            SceneData();
        }

        //bool與int切換
        #region bool與int切換
        public static int booltoint(bool i)
        {
            if(i)
                return 1;
            else
                return 0;
        }
        public static bool inttobool(int i)
        {
            if(i != 0)
                return true;
            else
                return false;
        }   
        #endregion

        //存檔Text動畫Coroutine
        IEnumerator SaveTextAnimation()
        {
            SaveText.text = "存檔中";
            
            for(int i = 0 ; i < 3 ; i++)
            {
                SaveText.text += ".";
                yield return new WaitForSeconds(1f);
            }
            
            animation.clip = animationClips[0];
            animation.Play();
            yield return new WaitForSeconds(2.5f);
            SaveText.text = "";
            SaveAnimationBool = false;
        }
    }
}

//存檔類別
[System.Serializable]
public class PlayerData
{
    //存檔點
    public bool isSave0,isSave1;
    
    //玩家座標
    public float Player1x,Player1y,Player1z,Player2x,Player2y,Player2z;
 
    //謎題GameObject判斷
    public Dictionary<string,bool> playerDataDictionary = new Dictionary<string, bool>()
    {
        {"Key_Student",false},{"Key_General",false},{"Key_Biology",false},
        {"FireExtinguish",false},{"SmokeBool",false},{"Key_BiologyPuzzle",false},
        {"IronDoorPuzzle",false},{"IronDoorSwitch",false},{"EightPuzzle",false},
        {"ZeroPuzzle",false},{"FivePuzzle",false},{"SheetMusic902",false},
        {"SheetMusic802",false},{"PianoFinish",false},{"SteamBoxPuzzle",false},
        {"SteamBoxSwitch",false},{"StudentCardP1",false},{"StudentCardP2",false},
    };

    //Text觸發
    public Dictionary<string,bool> playerTextTriggeredDictionary = new Dictionary<string, bool>()
    {
        {"isGuideClose",false},{"StudentText",false},{"IronDoorText",false},
        {"GeneralText",false},{"IronDoorOpenText",false},{"isSmokeTrigger",false},
        {"isEnter2ndFloor",false},{"EightPuzzleText",false},{"ToiletText",false},
        {"SteamBoxText",false}
    };
}