using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{


    public int bulletDamage;

    // Start is called before the first frame update
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Target"))
        {
            print("hit" +  collision.gameObject.name + "!");

            CreateBulletImpact(collision);

            Destroy(gameObject);

        }
        
        if (collision.gameObject.CompareTag("Wall"))
        {
            print("You missed and hit a wall!!");

            CreateBulletImpact(collision);

            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("Robot"))
        {
            print("You hit a the robot!! " + bulletDamage + " damage taken");

            if (collision.gameObject.GetComponent<Robot>().isDead == false )
            {
                collision.gameObject.GetComponent<Robot>().TakeDamage(bulletDamage);
            }

            CreateRobotImpactEffect(collision);

            Destroy(gameObject);
        }
    }

    private void CreateRobotImpactEffect(Collision objectHit)
    {
        ContactPoint contact = objectHit.contacts[0];

        GameObject sparkEffectPrefab = Instantiate(
            GlobalReferences.Instance.robotImpact, contact.point, Quaternion.LookRotation(contact.normal)
            );

        sparkEffectPrefab.transform.SetParent(objectHit.gameObject.transform);
    }

    void CreateBulletImpact(Collision objectHit)
    {
        ContactPoint contact = objectHit.contacts[0];

        GameObject hole = Instantiate(
            GlobalReferences.Instance.bulletImpactPrefab, contact.point, Quaternion.LookRotation(contact.normal)
            );

        hole.transform.SetParent(objectHit.gameObject.transform);
    }

}
