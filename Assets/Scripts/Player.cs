using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    public int HitPoints = 100;

    public void TakeDamage(int damageAmount)
    {
        HitPoints -= damageAmount;

        if (HitPoints <= 0)
        {
            print("Player Dead");
        }
        else
        {
            print("Player Hit");
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("RobotHand"))
        {
            TakeDamage(25);
        }
    }
}
