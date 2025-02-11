using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dolls : MonoBehaviour
{
    //娃娃GameObject
    public GameObject[] dolls;
    //娃娃GameObject是否出現
    private bool isSwitch0 = false;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // Update is called once per frame
    void Update()
    {
        //娃娃出現
        if(PuzzleObjController.puzzleDictionary["BookPuzzle"] == true && PuzzleObjController.puzzleDictionary["ArtPuzzle"] == true
        && PuzzleObjController.puzzleDictionary["PasswordCorrect"] == true && isSwitch0 == false)
        {
            isSwitch0 = true;
            
            for(int i = 0 ; i < 3 ; i++)
            {
                dolls[i].SetActive(true);
            }
        }

        //娃娃全部拾取
        if(dolls[0] == null && dolls[1] == null && dolls[2] == null && PuzzleObjController.puzzleDictionary["DollsPuzzle"] == false)
        {
            PuzzleObjController.puzzleDictionary["DollsPuzzle"] = true;
        }
    }
}
