using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionsManager : MonoBehaviour
{
    public GameObject ui_teach;
    public GameObject[] ui_missions;

    // Update is called once per frame
    void Update()
    {
        if(PuzzleObjController.puzzleDictionary["BookPuzzle"] == true)
        {
            if(ui_missions[2].activeInHierarchy == true)
                ui_missions[2].SetActive(false);
            ui_missions[3].SetActive(true);
        }
        else if(PuzzleObjController.puzzleDictionary["StudentCardP1"] == true || PuzzleObjController.puzzleDictionary["StudentCardP2"] == true)
        {
            if(ui_missions[1].activeInHierarchy == true)
                ui_missions[1].SetActive(false);
            ui_missions[2].SetActive(true);
        }
        else if(Trigger.isEnter2ndFloor == true)
        {
            if(ui_missions[0].activeInHierarchy == true)
                ui_missions[0].SetActive(false);
            ui_missions[1].SetActive(true);
        }
        else if(ui_teach.activeInHierarchy == false)
        {
            ui_missions[0].SetActive(true);
        }
    }
}
