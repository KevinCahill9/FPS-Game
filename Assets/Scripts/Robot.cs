using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Robot : MonoBehaviour
{
    [SerializeField] private int HitPoints = 100;
    private int maxHitPoints;
    private Animator animator;

    private NavMeshAgent agent;


    [SerializeField] private Slider healthBar;

    private void Start()
    {
        maxHitPoints = HitPoints;
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    public void TakeDamage(int damageAmount)
    {
       HitPoints -= damageAmount;

        if (healthBar != null)
        {
            healthBar.value = HitPoints;
        }

        if (HitPoints <= 0)
        {
            if (healthBar != null)
            {
                Destroy(healthBar.gameObject);
            }

            int randInt = Random.Range(0, 2);

            if (randInt == 0) 
            {
                animator.SetTrigger("DIE1");
            }
            else
            {
                animator.SetTrigger("DIE2");
            }
        }
        else
        {
            animator.SetTrigger("DAMAGE");
        }
    }
}
