using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;
using EnemyAInamespace;

public class GhostPatrol : EnemyAI
{
    // Start is called before the first frame update
    public override void Start()
    {
        //GetComponent
        agent = this.gameObject.GetComponent<NavMeshAgent>();
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
        player = GameObject.Find("Player1");
        distance = Vector3.Distance(player.transform.position,this.gameObject.transform.position);
        
        //追蹤玩家
        if(distance < 6f && OriginMode.isGod == false)
        {
            agent.SetDestination(player.transform.position);
        }
        //巡點
        else
        {
            GotoPoint();
        }

        //遊戲暫停
        if(Time.timeScale == 0 || Player.currentHP == 0)
        {
            audioSource.Stop();
        }
        else if(!audioSource.isPlaying)
        {
            audioSource.Play();
        }

        //玩家死亡
        if(Player.currentHP == 0)
        {
            gameObject.transform.GetChild(1).LookAt(player.transform);
        }
    }
}