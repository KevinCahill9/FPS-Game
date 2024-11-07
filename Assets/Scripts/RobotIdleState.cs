using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotIdleState : StateMachineBehaviour
{


    float timer;
    public float IdleTime = 0f;

    Transform player;

    public float detectionRadius = 18f;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer = 0;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }


    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer += Time.deltaTime;

        if (timer > 0) 
        {
            animator.SetBool("isPatroling", true);
        }

        float distanceFromPlayer = Vector3.Distance(player.position, animator.transform.position);

        if (distanceFromPlayer < detectionRadius)
        {
            animator.SetBool("isChasing", true);
        }
    }

}
