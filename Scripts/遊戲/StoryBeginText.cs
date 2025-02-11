using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class StoryBeginText : MonoBehaviour
{
    //Text
    public Text text;
    //前導Text String
    public string storytext;
    //前導Text字元
    public char[] storychar;
    //前導Text是否出現
    private bool isTextAppear = false;
    //場景編號
    private int scenenum;
    
    // Start is called before the first frame update
    void Start()
    {
        //取得場景編號
        scenenum = SceneManager.GetActiveScene().buildIndex;  
        
        storytext = text.text;
        text.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        if(isTextAppear == false)
        {
            isTextAppear = true;
            StartCoroutine(Texttimer());
        }
    }

    //前導Text打字機動畫
    IEnumerator Texttimer()
    {
        yield return new WaitForSeconds(2f);
        
        foreach(char storychar in storytext.ToCharArray())
        {
            text.text += storychar;

            if(scenenum == 1)
            {
                yield return new WaitForSeconds(0.1f);
            }
            else
            {
                yield return new WaitForSeconds(0.2f);
            }
        }
    }
}