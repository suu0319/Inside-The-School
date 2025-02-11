using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;
using EnemyAInamespace;

public class HateLightGhost : EnemyAI
{
    // Start is called before the first frame update
    public override void Start()
    {
        //GetComponent
        agent = this.gameObject.GetComponent<NavMeshAgent>();
        animator = this.gameObject.GetComponent<Animator>();
        audioSource = this.gameObject.GetComponent<AudioSource>();
        
        agent.autoBraking = false;
        
        GotoNextPoint();
    }

    // Update is called once per frame
    void Update()
    {   
        EnemyControl();
    }

    //怪物追蹤判定
    public override void EnemyControl()
    {
        //尋找P2玩家
        player = GameObject.Find("Player2");
        //與P2玩家的距離
        distance = Vector3.Distance(player.transform.position,this.gameObject.transform.position);
        
        //追蹤P2玩家
        if(Player.isSpotLightOpen == true && RayControl.isHateLightGhostTrack == true && distance < 5f)
        {
            agent.SetDestination(player.transform.position);
        }
        else
        {
            GotoPoint();
        }

        //暫停 or 玩家死亡
        if(Time.timeScale == 0 || Player.currentHP == 0)
        {
            audioSource.Stop();
        }
        //播放音效
        else if(!audioSource.isPlaying)
        {
            audioSource.Play();
        }

        //玩家死亡 轉向玩家
        if(Player.currentHP == 0)
        {
            gameObject.transform.GetChild(1).LookAt(player.transform);
            animator.SetBool("Attack",true);
        }
    }
}