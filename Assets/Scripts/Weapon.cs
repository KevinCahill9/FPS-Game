// Note: This script may include code or patterns modified from Unity tutorials.
// It has been modified and extended to suit the requirments of the project.
// Source: https://www.youtube.com/playlist?list=PLtLToKUhgzwm1rZnTeWSRAyx9tl8VbGUE

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Camera playerCamera;


    //Shooting
    public bool isShooting;
    public bool readyToShoot;
    public bool allowReset = true;
    public float shootingDelay = 0.2f;

    public static float totalShotsFired = 0;


    //Bullet
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public float bulletSpeed = 30;
    public float bulletPrefabLife = 3f;
    public int weaponDamage;

    public GameObject muzzleEffect; 


    private void Awake()
    {
        readyToShoot = true;
    }

    // Update is called once per frame
    void Update()
    {
    
        isShooting = Input.GetKey(KeyCode.Mouse0);

        if (readyToShoot && isShooting)
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        totalShotsFired++;

        muzzleEffect.GetComponent<ParticleSystem>().Play();
        readyToShoot = false;

        SoundManager.Instance.shottingSoundAk.Play();

        Vector3 shootingDirection = CalculateDirectionAndSpread().normalized;

        //Bullet is made 
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, Quaternion.identity);
        Bullet b = bullet.GetComponent<Bullet>();
        b.bulletDamage = weaponDamage;

        bullet.transform.forward = shootingDirection;


        //Shoot the bullet 
        bullet.GetComponent<Rigidbody>().AddForce(shootingDirection * bulletSpeed, ForceMode.Impulse);

        //Bullet life-time ends
        StartCoroutine(DestroyBulletAfterTime(bullet,bulletPrefabLife));


        //Checking if done shooting 
        if (allowReset)
        {
            Invoke("ResetShot", shootingDelay);
            allowReset = false;
        }


    }

    public Vector3 CalculateDirectionAndSpread()
    {
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.GetPoint(100);
        }

        Vector3 direction = targetPoint - bulletSpawn.position;

        return direction + new Vector3(0, 0, 0);
    }

    private void ResetShot()
    {
        readyToShoot = true;
        allowReset = true;
    }

    private IEnumerator DestroyBulletAfterTime(GameObject bullet, float bulletPrefabLife)
    {
        yield return new WaitForSeconds(bulletPrefabLife);
        Destroy(bullet);
    }
}
