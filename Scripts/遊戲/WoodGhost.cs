using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;
using EnemyAInamespace;

public class WoodGhost : EnemyAI
{
    // Start is called before the first frame update
    public override void Start()
    {
        //GetComponent
        agent = this.gameObject.GetComponent<NavMeshAgent>();
        animator = this.gameObject.GetComponent<Animator>();
        audioSource = this.gameObject.GetComponent<AudioSource>();
        
        //尋找P1玩家
        player = GameObject.Find("Player1");
        
        agent.autoBraking = false;
    }

    // Update is called once per frame
    void Update()
    {
        EnemyControl();
    }

    //怪物控制
    public override void EnemyControl()
    {
        //追蹤P1玩家
        if(RayControl.isWoodGhostStop == false)
        {   
            agent.SetDestination(player.transform.position);
            agent.isStopped = false;
            animator.enabled = true;
            
            if(!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        //移動暫停 停止追蹤
        else
        {
            agent.isStopped = true;
            animator.enabled = false;
            
            if(audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }
            
        //遊戲暫停    
        if(Time.timeScale == 0)
        {
            audioSource.Stop();
        }

        //玩家死亡
        if(Player.currentHP == 0)
        {
            gameObject.transform.GetChild(1).LookAt(player.transform);
            animator.SetBool("Attack",true);
            
            if(audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }
    }
}