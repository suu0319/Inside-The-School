using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MouseLook : MonoBehaviour
{
    //PhotonView
    private PhotonView view;
    //角色Transform
    public Transform playerBody;
    //滑鼠靈敏度
    public float mouseSensitivity;
    //X座標
    public float xRotation = 0f;
    //晃動
    private bool isShake = false;
    //滑鼠是否可以移動 (鏡頭移動)
    public static bool isMouseLook = true;
    
    void Start()
    {
        //GetComponet
        view = this.gameObject.GetComponent<PhotonView>();
    }

    void FixedUpdate()
    {
        if(view.IsMine)
        {
            PlayerCamera();
        }
    }

    //玩家鏡頭
    public void PlayerCamera()
    {
        //鏡頭可以移動
        if(isMouseLook == true)
        {
            //計算鏡頭移動速度
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;  

            xRotation -= mouseY;
            
            //計算鏡頭移動範圍)
            xRotation = Mathf.Clamp(xRotation, -60f, 60f);
            //鏡頭轉向 (Quaternion.Euler是轉xRotation)
            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            //Vector3.up = Vector3(0, 1, 0)
            playerBody.Rotate(Vector3.up * mouseX); 
            
            //如果進入死亡動畫
            if(Player.currentHP == 0)
            {
                isMouseLook = false;
            }   

            //蒸飯箱引爆
            if(PuzzleObjController.puzzleDictionary["SteamBoxSwitch"] == true && isShake == false && GameData.GameData.isSave1 == false)
            {
                isShake = true;
                StartCoroutine(Shake(2f,3f));
            }
        }
    }

    //晃動Coroutine
    IEnumerator Shake(float duration,float magnitude)
    {
        Vector3 originPos = transform.localPosition;
        float elapsed = 0.0f;

        while(elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(x, y, originPos.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = originPos;
    }
}