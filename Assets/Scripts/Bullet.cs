using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Target"))
        {
            print("hit" +  collision.gameObject.name + "!");

            CreateBulletImpact(collision);

            RobotHealth robotHealth = collision.gameObject.GetComponent<RobotHealth>();
            if (robotHealth != null)
            {
                robotHealth.TakeDamage(1);  // Each bullet deals 1 damage
            }

            Destroy(gameObject);

        }
        
        if (collision.gameObject.CompareTag("Wall"))
        {
            print("You missed and hit a wall!!");

            CreateBulletImpact(collision);

            Destroy(gameObject);
        }
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
