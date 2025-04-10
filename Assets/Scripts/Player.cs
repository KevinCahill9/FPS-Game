// Note: This script may include code or patterns modified from Unity tutorials.
// It has been modified and extended to suit the requirments of the project.
// Source: https://www.youtube.com/playlist?list=PLtLToKUhgzwm1rZnTeWSRAyx9tl8VbGUE

using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public int HitPoints = 100;
    private int maxHitPoints;

    public GameObject bloodOnScreen;
    public TextMeshProUGUI playerHPUI;
    public GameObject DeathScreen;

    public bool isDead;
    public Transform respawnPoint; 

    private GameObject outerPlayer;  
    private CharacterController characterController;
    private PlayerMovement playerMovement;

    private void Start()
    {
        maxHitPoints = HitPoints;
        playerHPUI.text = $"Health: {HitPoints}";

        outerPlayer = transform.parent?.gameObject;  
        if (outerPlayer != null)
        {
            characterController = outerPlayer.GetComponent<CharacterController>();
            playerMovement = outerPlayer.GetComponent<PlayerMovement>();
        }

        if (respawnPoint == null)
        {
            respawnPoint = outerPlayer != null ? outerPlayer.transform : transform; 
        }
    }

    public void TakeDamage(int damageAmount)
    {
        if (isDead) return;

        HitPoints -= damageAmount;
        playerHPUI.text = $"Health: {HitPoints}";

        if (HitPoints <= 0)
        {
            print("Player Dead");
            SoundManager.Instance.playerChannel.PlayOneShot(SoundManager.Instance.playerDead);
            isDead = true;
            PlayerDead();
        }
        else
        {
            print("Player Hit");
            StartCoroutine(BloodOnScreenEffect());
            SoundManager.Instance.playerChannel.PlayOneShot(SoundManager.Instance.playerHit);
        }
    }

    private void PlayerDead()
    {
        if (playerMovement != null)
        {
            playerMovement.enabled = false;
        }

        var gunObject = Camera.main.transform.Find("AK74");
        if (gunObject != null)
        {
            gunObject.gameObject.SetActive(false); 
        }

        var mainCamera = Camera.main;
        if (mainCamera != null)
        {
            var cameraAnimator = mainCamera.GetComponent<Animator>();
            if (cameraAnimator != null)
            {
                cameraAnimator.enabled = true;
                cameraAnimator.Play("PlayerDeath");
            }
        }

        playerHPUI.gameObject.SetActive(false);
        GetComponent<ScreenBlack>().StartFade();

        StartCoroutine(ShowDeathScreen());
        StartCoroutine(RespawnPlayer(4f));
    }

    private IEnumerator ShowDeathScreen()
    {
        yield return new WaitForSeconds(2f);
        DeathScreen.gameObject.SetActive(true);
    }

    private IEnumerator RespawnPlayer(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (DeathScreen != null)
        {
            DeathScreen.SetActive(false);
            CanvasGroup deathCanvas = DeathScreen.GetComponent<CanvasGroup>();
            if (deathCanvas != null)
            {
                deathCanvas.alpha = 0f;
                deathCanvas.interactable = false;
                deathCanvas.blocksRaycasts = false;
            }
            Debug.Log("Death screen fully hidden.");
        }

        ScreenBlack screenBlack = GetComponent<ScreenBlack>();
        if (screenBlack != null)
        {
            screenBlack.ResetFade();
            Debug.Log("Black screen reset.");
        }

        HitPoints = maxHitPoints;
        playerHPUI.text = $"Health: {HitPoints}";
        playerHPUI.gameObject.SetActive(true);

        
        if (outerPlayer != null)
        {
            if (characterController != null)
            {
                characterController.enabled = false;
                outerPlayer.transform.position = respawnPoint.position;
                characterController.enabled = true;
            }
            else
            {
                outerPlayer.transform.position = respawnPoint.position;
            }
        }

        
        ResetPlayerAnimation();

        
        if (playerMovement != null)
        {
            playerMovement.enabled = true;
        }

        
        var gunObject = Camera.main.transform.Find("AK74");
        if (gunObject != null)
        {
            gunObject.gameObject.SetActive(true); 
        }

        isDead = false;

       
    }



    private void ResetPlayerAnimation()
    {
        var mainCamera = Camera.main;
        if (mainCamera != null)
        {
            Animator cameraAnimator = mainCamera.GetComponent<Animator>();
            if (cameraAnimator != null)
            {
                Debug.Log("Resetting camera animation...");

                cameraAnimator.Rebind(); 
                cameraAnimator.Update(0f);
                cameraAnimator.enabled = false; 

                Debug.Log("Camera animation fully reset and disabled.");
            }
        }
    }



    private IEnumerator BloodOnScreenEffect()
    {
        if (!bloodOnScreen.activeInHierarchy)
        {
            bloodOnScreen.SetActive(true);
        }

        var image = bloodOnScreen.GetComponentInChildren<Image>();
        Color startColor = image.color;
        startColor.a = 1f;
        image.color = startColor;

        float duration = 3f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / duration);
            image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        if (bloodOnScreen.activeInHierarchy)
        {
            bloodOnScreen.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("RobotHand") && !isDead)
        {
            TakeDamage(25);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GoToMainMenu();
        }
    }

    private void GoToMainMenu()
    {
        ResetResults();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }

    private void ResetResults()
    {
        PlayerPrefs.SetInt(MainMenu.selectedVisualization + "_Kills", 0);
        PlayerPrefs.SetFloat(MainMenu.selectedVisualization + "_ShotsFired", 0f);
        PlayerPrefs.SetFloat(MainMenu.selectedVisualization + "_ShotsHit", 0f);
        PlayerPrefs.SetFloat(MainMenu.selectedVisualization + "_HighlightedHits", 0f);
        PlayerPrefs.Save();
    }
}
