using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PasswordPuzzle : MonoBehaviour
{
    //密碼InputField
    public InputField password;
    //密碼謎題GameObject
    public GameObject ui,error,successful;

    // Update is called once per frame
    void Update()
    {
        //無敵模式
        OriginMode.isGod = true;

        //密碼ui判定
        if(ui.activeInHierarchy == false)
        {
            ui.SetActive(true);
        }
        
        //密碼輸入框判定
        if(password.IsActive())
        {
            password.ActivateInputField();
        }

        GameObject.Find("EventSystem").GetComponent<DeviceSelect>().enabled = false;
        
        //輸入密碼確定
        if(Input.GetKeyDown(KeyCode.Return) && PuzzleObjController.puzzleDictionary["PasswordCorrect"] == false)
        {
            if(password.text == "89221")
            {
                successful.SetActive(true);
                GameObject.Find("EventSystem").GetComponent<DeviceSelect>().enabled = true;
                PuzzleObjController.puzzleDictionary["PasswordCorrect"] = true;
            }
            else
            {
                error.SetActive(true);
                StartCoroutine(textCD());
            }
        }
        //密碼謎題成功
        else if(PuzzleObjController.puzzleDictionary["PasswordCorrect"] == true)
        {
            successful.SetActive(true);
        }
    }

    //文字動畫Coroutine
    IEnumerator textCD()
    {
        yield return new WaitForSeconds(2.5f);
        error.SetActive(false);
    }
}
