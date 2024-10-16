using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotHealth : MonoBehaviour
{
    public int maxHealth = 5;  // Robot will die after 5 hits
    private int currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // Destroy the robot
        Destroy(gameObject);
        // You can also trigger any death animation or effects here
        print(gameObject.name + " has been destroyed!");
    }
}
