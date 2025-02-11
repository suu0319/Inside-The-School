using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class PlayerText : MonoBehaviour
{
    //PhotonView
    private PhotonView view;
    //玩家文字GameObject
    public GameObject playertextobj;
    //娃娃GameObject
    public GameObject[] dolls;
    //玩家Text
    public Text playertext;
    //Text Animation
    public Animation animation;
    //Text AnimationClip
    public AnimationClip[] animationClips;
    //Text是否觸發
    private Dictionary<string,bool> playerTextDictionary = new Dictionary<string, bool>()
    {
        {"keystudenttext",false},{"keygeneraltext",false},{"keybiologytext",false},
        {"smoketext",false},{"fireextinguish",false},{"studenttext",false},
        {"irondoortext",false},{"generaltext",false},{"irondooropentext",false},
        {"enter2andfloor",false},{"sheettext",false},{"studentcardp1text",false},
        {"studentcardp2text",false},{"eightpuzzletext",false},{"pianofinishtext",false},
        {"steamboxtext",false},{"steamboxswitchtext",false},{"dollsbool1",false},
        {"dollsbool2",false},{"dollsbool3",false},{"passwordcorrect",false}
    };
    
    //Text是否觸發完畢
    public static Dictionary<string,bool> playerTextTriggeredDictionary = new Dictionary<string, bool>()
    {
        {"isGuideClose",false},{"StudentText",false},{"IronDoorText",false},
        {"GeneralText",false},{"IronDoorOpenText",false},{"isSmokeTrigger",false},
        {"isEnter2ndFloor",false},{"EightPuzzleText",false},{"ToiletText",false},
        {"SteamBoxText",false}
    };
    
    // Start is called before the first frame update
    void Start()
    {
        playertext = playertextobj.GetComponent<Text>();
        animation = playertextobj.GetComponent<Animation>();
        view = this.gameObject.GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        Playertext();
    }

    //玩家文字
    public void Playertext()
    {
        //P1玩家
        if(OutsideTheSchoolPUN.ServerConnect.playerID == 1)
        {
            //存檔點1未存檔
            if(GameData.GameData.isSave0 == false)
            {
                if(Guide.isGuideClose == true && playerTextTriggeredDictionary["isGuideClose"] == false)
                {
                    playerTextTriggeredDictionary["isGuideClose"] = true;
                    playertext.text = "先來看看辦公室裡有什麼吧。";
                    animation.clip = animationClips[1];
                    animation.Play();
                    StartCoroutine(textCD());
                }
                else if(PuzzleObjController.puzzleDictionary["Key_Student"] == true && playerTextDictionary["keystudenttext"] == false)
                {
                    playerTextDictionary["keystudenttext"] = true;
                    playertext.text = "取得學務處鑰匙。";
                    animation.clip = animationClips[1];
                    animation.Play();
                    StartCoroutine(textCD());
                }
                else if(playerTextTriggeredDictionary["StudentText"] == true && playerTextDictionary["studenttext"] == false)
                {
                    playerTextDictionary["studenttext"] = true;
                    playertext.text = "樓梯間的鐵捲門拉下了，不知道有沒有辦法打開。";
                    animation.clip = animationClips[1];
                    animation.Play();
                    StartCoroutine(textCD());
                }
                else if(PuzzleObjController.puzzleDictionary["Key_Biology"] == true && playerTextDictionary["keybiologytext"] == false)
                {
                    playerTextDictionary["keybiologytext"] = true;
                    playertext.text = "取得生物教室鑰匙。";
                    animation.clip = animationClips[1];
                    animation.Play();
                    StartCoroutine(textCD());
                }
                else if(playerTextTriggeredDictionary["IronDoorText"] == true && playerTextDictionary["irondoortext"] == false)
                {
                    playerTextDictionary["irondoortext"] = true;
                    playertext.text = "器材室在哪裡呢?去樓梯間看看地圖吧。";
                    animation.clip = animationClips[1];
                    animation.Play();
                    StartCoroutine(textCD());
                }
            }
            //存檔點2未存檔
            else if(GameData.GameData.isSave1 == false)
            {
                if(Trigger.isEnter2ndFloor == true && playerTextDictionary["enter2andfloor"] == false) 
                {
                    playerTextDictionary["enter2andfloor"] = true;
                    playerTextDictionary["irondoortext"] = true;
                    playertext.text = "鋼琴曲…?是從音樂教室傳來的?";
                    animation.clip = animationClips[1];
                    animation.Play();
                    StartCoroutine(textCD());
                }
                else if(PuzzleObjController.puzzleDictionary["SheetMusic902"] == true && playerTextDictionary["sheettext"] == false)
                {
                    playerTextDictionary["sheettext"] = true;
                    playertext.text = "取得樂譜。";
                    animation.clip = animationClips[1];
                    animation.Play();
                    StartCoroutine(textCD());
                }
                else if(PuzzleObjController.puzzleDictionary["PianoFinish"] == true && playerTextDictionary["pianofinishtext"] == false)
                {
                    playerTextDictionary["pianofinishtext"] = true;
                    playertext.text = "鋼琴似乎掉出了什麼。";
                    animation.clip = animationClips[1];
                    animation.Play();
                    StartCoroutine(textCD());
                }
                else if(PuzzleObjController.puzzleDictionary["StudentCardP1"] == true && playerTextDictionary["studentcardp1text"] == false)
                {
                    playerTextDictionary["studentcardp1text"] = true;
                    playertext.text = "...借書證?似乎可以在三樓圖書館進出用到。";
                    animation.clip = animationClips[1];
                    animation.Play();
                    StartCoroutine(textCD());
                }
                else if(PuzzleObjController.puzzleDictionary["SteamBoxSwitch"] == true && playerTextDictionary["steamboxswitchtext"] == false)
                {
                    playerTextDictionary["steamboxswitchtext"] = true;
                    playertext.text = "...這是什麼情況?";
                    animation.clip = animationClips[1];
                    animation.Play();
                    StartCoroutine(textCD());
                }
            }
            //3樓
            else if(GameData.GameData.isSave1 == true)
            {
                if(PuzzleObjController.puzzleDictionary["PasswordCorrect"] == true && playerTextDictionary["passwordcorrect"] == false)
                {
                    playerTextDictionary["passwordcorrect"] = true;
                    playertext.text = "幫助阿凱蒐集完巫毒毒娃娃後，就返回書櫃那裡跟阿凱會合吧!";
                    animation.clip = animationClips[1];
                    animation.Play();
                    StartCoroutine(textCD());
                }
            }
        }
        //P2玩家
        else if(OutsideTheSchoolPUN.ServerConnect.playerID == 2)
        {
            //存檔點1未存檔
            if(GameData.GameData.isSave0 == false)
            {
                if(Guide.isGuideClose == true && playerTextTriggeredDictionary["isGuideClose"] == false)
                {
                    playerTextTriggeredDictionary["isGuideClose"] = true;
                    playertext.text = "先照著阿浩說的來翻翻看辦公室吧。";
                    animation.clip = animationClips[1];
                    animation.Play();
                    StartCoroutine(textCD());
                }
                else if(Trigger.isSmoke == true && playerTextDictionary["smoketext"] == false)
                {
                    playerTextDictionary["smoketext"] = true;
                    playertext.text = "這裡怎麼冒煙了?快快快!有沒有可以滅火的東西?";
                    animation.clip = animationClips[1];
                    animation.Play();
                    StartCoroutine(textCD());
                }
                else if(PuzzleObjController.puzzleDictionary["FireExtinguish"] == true && playerTextDictionary["fireextinguish"] == false)
                {
                    playerTextDictionary["fireextinguish"] = true;
                    playertext.text = "取得滅火器。";
                    animation.clip = animationClips[1];
                    animation.Play();
                    StartCoroutine(textCD());
                }
                else if(PuzzleObjController.puzzleDictionary["Key_General"] == true && playerTextDictionary["keygeneraltext"] == false)
                {
                    playerTextDictionary["keygeneraltext"] = true;
                    playertext.text = "取得教務處鑰匙。";
                    animation.clip = animationClips[1];
                    animation.Play();
                    StartCoroutine(textCD());
                }
                else if(playerTextTriggeredDictionary["GeneralText"] == true && playerTextDictionary["generaltext"] == false)
                {
                    playerTextDictionary["generaltext"] = true;
                    playertext.text = "801旁的廁所壞了嗎?真慘。";
                    animation.clip = animationClips[1];
                    animation.Play();
                    StartCoroutine(textCD());
                }
                else if(playerTextTriggeredDictionary["IronDoorOpenText"] == true && playerTextDictionary["irondooropentext"] == false)
                {
                    playerTextDictionary["irondooropentext"] = true;
                    playertext.text = "打開這個就能上二樓探索了!!";
                    animation.clip = animationClips[1];
                    animation.Play();
                    StartCoroutine(textCD());   
                    view.RPC("SaveData",RpcTarget.All);
                }
            }
            //存檔點2未存檔
            else if(GameData.GameData.isSave1 == false)
            {
                if(Trigger.isEnter2ndFloor == true && playerTextDictionary["enter2andfloor"] == false) 
                {
                    playerTextDictionary["enter2andfloor"] = true;
                    playerTextDictionary["irondoortext"] = true;
                    playertext.text = "鋼琴曲?該不會是鬼魂在彈奏的吧…";
                    animation.clip = animationClips[1];
                    animation.Play();
                    StartCoroutine(textCD());
                }
                else if(PuzzleObjController.puzzleDictionary["SheetMusic802"] == true && playerTextDictionary["sheettext"] == false)
                {
                    playerTextDictionary["sheettext"] = true;
                    playertext.text = "取得樂譜。";
                    animation.clip = animationClips[1];
                    animation.Play();
                    StartCoroutine(textCD());
                }
                else if(playerTextTriggeredDictionary["EightPuzzleText"] == true && playerTextDictionary["eightpuzzletext"] == false)
                {
                    playerTextDictionary["eightpuzzletext"] = true;
                    playertext.text = "888...?似乎是某種提示，好像其他間廁所也有類似的惡作劇??";
                    animation.clip = animationClips[1];
                    animation.Play();
                    StartCoroutine(textCD());
                }
                else if(playerTextTriggeredDictionary["EightPuzzleText"] == true && PuzzleObjController.puzzleDictionary["ZeroPuzzle"] == true
                && PuzzleObjController.puzzleDictionary["FivePuzzle"] == true && playerTextTriggeredDictionary["ToiletText"] == false)
                {
                    playerTextTriggeredDictionary["ToiletText"] = true;
                    playertext.text = "這三個提示代表什麼呢...?";
                    animation.clip = animationClips[1];
                    animation.Play();
                    StartCoroutine(textCD());
                }
                else if(playerTextTriggeredDictionary["SteamBoxText"] == true && playerTextDictionary["steamboxtext"] == false)
                {
                    playerTextDictionary["steamboxtext"] = true;
                    playertext.text = "蒸飯室在哪裡呢?去樓梯間看看地圖吧。";
                    animation.clip = animationClips[1];
                    animation.Play();
                    StartCoroutine(textCD());
                }
                else if(PuzzleObjController.puzzleDictionary["PianoFinish"] == true && playerTextDictionary["pianofinishtext"] == false)
                {
                    playerTextDictionary["pianofinishtext"] = true;
                    playertext.text = "鋼琴似乎掉出了什麼。";
                    animation.clip = animationClips[1];
                    animation.Play();
                    StartCoroutine(textCD());
                }
                else if(PuzzleObjController.puzzleDictionary["StudentCardP2"] == true && playerTextDictionary["studentcardp2text"] == false)
                {
                    playerTextDictionary["studentcardp2text"] = true;
                    playertext.text = "...借書證?似乎可以在三樓圖書館進出用到。";
                    animation.clip = animationClips[1];
                    animation.Play();
                    StartCoroutine(textCD());
                    view.RPC("SaveData",RpcTarget.All);
                }
            }
            //3樓
            else if(GameData.GameData.isSave1 == true)
            {
                if(dolls[0] == null && playerTextDictionary["dollsbool1"] == false)
                {
                    playerTextDictionary["dollsbool1"]= true;
                    
                    if(dolls[0] == null && dolls[1] == null && dolls[2] == null)
                    {
                        playertext.text = "蒐集完巫毒娃娃了，返回書櫃那裡跟阿浩會合吧!";
                    }
                    else
                    {
                        playertext.text = "取得巫毒娃娃。";
                    }
    
                    animation.clip = animationClips[1];
                    animation.Play();
                    StartCoroutine(textCD());
                }
                else if(dolls[1] == null && playerTextDictionary["dollsbool2"] == false)
                {
                    playerTextDictionary["dollsbool2"] = true;
                    
                    if(dolls[0] == null && dolls[1] == null && dolls[2] == null)
                    {
                        playertext.text = "蒐集完巫毒娃娃了，返回書櫃那裡跟阿浩會合吧!";
                    }
                    else
                    {
                        playertext.text = "取得巫毒娃娃。";
                    }
                    
                    animation.clip = animationClips[1];
                    animation.Play();
                    StartCoroutine(textCD());
                }
                else if(dolls[2] == null && playerTextDictionary["dollsbool3"] == false)
                {
                    playerTextDictionary["dollsbool3"] = true;
                    
                    if(dolls[0] == null && dolls[1] == null && dolls[2] == null)
                    {
                        playertext.text = "蒐集完巫毒娃娃了，返回書櫃那裡跟阿浩會合吧!";
                    }
                    else
                    {
                        playertext.text = "取得巫毒娃娃。";
                    }
                    
                    animation.clip = animationClips[1];
                    animation.Play();
                    StartCoroutine(textCD());
                }
            }
        }
    }

    //存檔
    [PunRPC]
    public void SaveData()
    {
        if(GameData.GameData.isSave0 == true)
        {
            GameData.GameData.isSave1 = true;
        }
        else
        {
            GameData.GameData.isSave0 = true;
        }
    
        GameObject.Find("GameData").SendMessage("SaveData");
    }

    //文字動畫Coroutine
    IEnumerator textCD()
    {
        yield return new WaitForSeconds(2.5f);
        animation.clip = animationClips[0];
        animation.Play();  
        yield return new WaitForSeconds(2.5f);
        playertext.text = "";
    }
}