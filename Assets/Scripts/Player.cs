using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    public int HitPoints = 100;

    public GameObject bloodOnScreen;

    public TextMeshProUGUI playerHPUI;

    public GameObject DeathScreen;

    public bool isDead;

    private void Start()
    {
        playerHPUI.text = $"Health: {HitPoints}";
    }

    public void TakeDamage(int damageAmount)
    {
        HitPoints -= damageAmount;

        if (HitPoints <= 0)
        {
            print("Player Dead");
            SoundManager.Instance.playerChannel.PlayOneShot(SoundManager.Instance.playerDead);
            PlayerDead();
            isDead = true;
            
        }
        else
        {
            print("Player Hit");
            StartCoroutine(BloodOnScreenEffect());
            playerHPUI.text = $"Health: {HitPoints}";
            SoundManager.Instance.playerChannel.PlayOneShot(SoundManager.Instance.playerHit);
        }
    }

    private void PlayerDead()
    {
        
        var outerPlayer = transform.parent?.gameObject;
        if (outerPlayer != null)
        {
            var playerMovement = outerPlayer.GetComponent<PlayerMovement>();
            if (playerMovement != null)
            {
                playerMovement.enabled = false;
            }
        }

        var gunObject = Camera.main.transform.Find("AK74"); 
        if (gunObject != null)
        {
            Destroy(gunObject.gameObject);
        }
        else
        {
            Debug.LogError("Gun object is missing!");
        }



        var mainCamera = Camera.main;
        if (mainCamera != null)
        {
            var cameraAnimator = mainCamera.GetComponent<Animator>();
            if (cameraAnimator != null)
            {
                cameraAnimator.enabled = true;
            }
            else
            {
                Debug.LogError("Animator component on MainCamera is missing!");
            }
        }
        else
        {
            Debug.LogError("Main Camera not found!");
        }

        playerHPUI.gameObject.SetActive(false);

        GetComponent<ScreenBlack>().StartFade();
        StartCoroutine(ShowDeathScreen());
    }

    private IEnumerator ShowDeathScreen()
    {
        yield return new WaitForSeconds(2f);
        DeathScreen.gameObject.SetActive(true);

    }

    private IEnumerator BloodOnScreenEffect()
    {
        if (bloodOnScreen.activeInHierarchy == false )
        {
            bloodOnScreen.SetActive(true);
        }

        var image = bloodOnScreen.GetComponentInChildren<Image>();

        // Set the initial alpha value to 1 (fully visible).
        Color startColor = image.color;
        startColor.a = 1f;
        image.color = startColor;

        float duration = 3f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / duration);

            
            Color newColor = image.color;
            newColor.a = alpha;
            image.color = newColor;

            
            elapsedTime += Time.deltaTime;

            yield return null; ; 
        }

        if (bloodOnScreen.activeInHierarchy == true)
        {
            bloodOnScreen.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("RobotHand"))
        {
            if (isDead == false)
            {
                TakeDamage(25);
            }
        }
    }
}
