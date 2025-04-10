// Note: This script may include code or patterns modified from Unity tutorials.
// It has been modified and extended to suit the requirments of the project.
// Source: https://www.youtube.com/playlist?list=PLtLToKUhgzwm1rZnTeWSRAyx9tl8VbGUE

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public static float totalHits = 0;
    public static float highlightedHits = 0;

    public int bulletDamage;

    // Start is called before the first frame update
    private void OnCollisionEnter(Collision collision)
    {
        Robot robot = collision.gameObject.GetComponentInParent<Robot>();

        if (robot != null && !robot.isDead)
        {
            totalHits++;

            string hitTag = collision.gameObject.tag;  

            robot.TakeDamage(bulletDamage, hitTag);

            if (robot.IsLimbHighlighted(hitTag))
            {
                highlightedHits++;  
                
            }

            Debug.Log($"{hitTag} hit. Damage applied.");

            CreateRobotImpactEffect(collision);
            Destroy(gameObject);
            return;
        }

        if (collision.gameObject.CompareTag("Target"))
        {
            Debug.Log($"Hit {collision.gameObject.name}!");
            CreateBulletImpact(collision);
            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("Wall"))
        {
            Debug.Log("You missed and hit a wall!");
            CreateBulletImpact(collision);
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
