using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public static string selectedVisualization = "None";  // Default

    public TextMeshProUGUI visualizationText;
    public TextMeshProUGUI resultsText;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        DisplayResults();
        UpdateVisualizationText();
    }

    public void StartGame()
    {
        Robot.kills = 0;
        Weapon.totalShotsFired = 0f;
        Bullet.totalHits = 0f;
        Bullet.highlightedHits = 0f;


        SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
    }

    public void ExitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;  
#endif
    }

    public void SetVisualizationNone()
    {
        selectedVisualization = "None";
        UpdateVisualizationText();
        Debug.Log("Visualization set to None");
    }

    public void SetVisualizationGlow()
    {
        selectedVisualization = "Glow";
        UpdateVisualizationText();
        Debug.Log("Visualization set to Glow");
    }

    public void SetVisualizationMatte()
    {
        selectedVisualization = "Matte";
        UpdateVisualizationText();
        Debug.Log("Visualization set to Matte");
    }

    public void SetVisualizationOutline()
    {
        selectedVisualization = "Outline";
        UpdateVisualizationText();
        Debug.Log("Visualization set to Outline");
    }

    public void SetVisualizationPopup()
    {
        selectedVisualization = "Popup";
        UpdateVisualizationText();
        Debug.Log("Visualization set to Popup");
    }

    public void SetVisualizationFlashing()
    {
        selectedVisualization = "Flashing";
        UpdateVisualizationText();
        Debug.Log("Visualization set to Flashing");
    }

    public void SetVisualizationCracked()
    {
        selectedVisualization = "Cracked";
        UpdateVisualizationText();
        Debug.Log("Visualization set to Cracked");
    }

    private void UpdateVisualizationText()
    {
        if (visualizationText != null)
        {
            visualizationText.text = "Selected Visualization: " + selectedVisualization;
        }
    }

    private void DisplayResults()
    {
        string visualizationKey = selectedVisualization;

        if (PlayerPrefs.HasKey(visualizationKey + "_Kills"))
        {
            int kills = PlayerPrefs.GetInt(visualizationKey + "_Kills");
            float shotsFired = PlayerPrefs.GetFloat(visualizationKey + "_ShotsFired");
            float shotsHit = PlayerPrefs.GetFloat(visualizationKey + "_ShotsHit");
            float highlightedHits = PlayerPrefs.GetFloat(visualizationKey + "_HighlightedHits");

            float overallAccuracy = shotsFired > 0 ? Mathf.Round((shotsHit / shotsFired) * 10000f) / 100f : 0f;
            float highlightedAccuracyToShotsFired = shotsFired > 0 ? Mathf.Round((highlightedHits / shotsFired) * 10000f) / 100f : 0f;
            float highlightedAccuracyToShotsHit = shotsHit > 0 ? Mathf.Round((highlightedHits / shotsHit) * 10000f) / 100f : 0f;

            resultsText.text = $"Results:\n" +
                               $"Kills: {kills}\n" +
                               $"Shots Fired: {shotsFired:F0}\n" +
                               $"Shots Hit: {shotsHit:F0}\n" +
                               $"Overall Accuracy: {overallAccuracy:F2}%\n" +
                               $"Highlighted Limb Accuracy (to Shots Fired): {highlightedAccuracyToShotsFired:F2}%\n" +
                               $"Highlighted Limb Accuracy (to Shots Hit): {highlightedAccuracyToShotsHit:F2}%";
        }
        else
        {
            resultsText.text = "No results yet.";
        }
    }
}