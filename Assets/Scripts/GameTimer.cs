using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameTimer : MonoBehaviour
{
    public float gameDuration = 180f;  
    public float remainingTime;
    public TextMeshProUGUI timerText;

    private void Start()
    {
        remainingTime = gameDuration;
    }

    private void Update()
    {
        remainingTime -= Time.deltaTime;
        UpdateTimerUI();

        if (remainingTime <= 0)
        {
            EndGame();
        }
    }

    private void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    private void EndGame()
    {
        PlayerPrefs.SetInt(MainMenu.selectedVisualization + "_Kills", Robot.kills);
        PlayerPrefs.SetFloat(MainMenu.selectedVisualization + "_ShotsFired", Weapon.totalShotsFired);
        PlayerPrefs.SetFloat(MainMenu.selectedVisualization + "_ShotsHit", Bullet.totalHits);
        PlayerPrefs.SetFloat(MainMenu.selectedVisualization + "_HighlightedHits", Bullet.highlightedHits);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }
}
