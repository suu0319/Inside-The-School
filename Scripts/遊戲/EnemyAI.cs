using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;

namespace EnemyAInamespace
{
    public abstract class EnemyAI : MonoBehaviour
    {
        //巡點排序
        protected int destpoints;
        //與玩家的距離
        public float distance;
        //是否巡下一個點
        protected bool isGoNextPoint = false;
        //NavMeshAgent
        protected NavMeshAgent agent;
        //AI Animator
        protected Animator animator;
        //AI AudioSource
        public AudioSource audioSource;
        //玩家GameObject
        public GameObject player;
        //巡點座標
        public Transform[] points;

        //Start
        public abstract void Start();
        //EnemyAI控制
        public abstract void EnemyControl();

        //巡點
        public virtual void GotoPoint()
        {
            if(agent.remainingDistance < 0.5f && isGoNextPoint == false)
            {
                isGoNextPoint = true;
                GotoNextPoint();
                StartCoroutine(GotoNextPointCD());
            }   
        }

        //巡下個點
        public virtual void GotoNextPoint()
        {
            //AI目標 = 巡邏點陣列的座標
            agent.SetDestination(points[destpoints].position);
            //AI巡邏點+1
            destpoints = (destpoints + 1) % points.Length;
        }

        //巡下個點CD
        IEnumerator GotoNextPointCD()
        {
            yield return new WaitForSeconds(5f);
            isGoNextPoint = false;
        }
    }
}