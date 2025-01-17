using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class RobotSpawnController : MonoBehaviour
{
    public int startRobotsPerWave = 1;
    public int currentRobotsPerWave;

    public float spawnDelay = 0.5f; //delay between spawning of each robot, not delay between waves;

    public int currentWave = 0;
    public float waveDelay = 10.0f;

    public bool inCooldown;
    public float cooldownCount = 0;

    public List<Robot> currentRobotsAlive;

    public GameObject robotPrefab;

    public TextMeshProUGUI waveOver;
    public TextMeshProUGUI prepTime;
    public TextMeshProUGUI WaveRound;

    private void Start()
    {
        currentRobotsPerWave = startRobotsPerWave;

        StartNextWave();
    }

    private void StartNextWave()
    {
        currentRobotsAlive.Clear();

        currentWave++;
        WaveRound.text = "Wave: " + currentWave.ToString();

        StartCoroutine(SpawnWave());
    }

    private IEnumerator SpawnWave()
    {
        for (int i = 0; i < currentRobotsPerWave; i++)
        {
            Vector3 spawnOffset = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
            Vector3 spawnPosition = transform.position + spawnOffset;

            var robot = Instantiate(robotPrefab, spawnPosition, Quaternion.identity);

            Robot robotScript = robot.GetComponent<Robot>();

            currentRobotsAlive.Add(robotScript);

            yield return new WaitForSeconds(spawnDelay);
        }
    }

    private void Update()
    {
        List<Robot> robotsToRemove = new List<Robot>();

        foreach (Robot robot in currentRobotsAlive)
        {
            if (robot.isDead)
            {
                robotsToRemove.Add(robot);
            }
        }

        foreach(Robot robot in robotsToRemove)
        {
            currentRobotsAlive.Remove(robot);
        }

        robotsToRemove.Clear();

        if (currentRobotsAlive.Count == 0 && inCooldown == false)
        {
            StartCoroutine(WaveCoolDown());
        }

        if (inCooldown)
        {
            cooldownCount -= Time.deltaTime;
        }
        else 
        {
            cooldownCount = waveDelay;
        }

        prepTime.text = cooldownCount.ToString("F0");
    }

    private IEnumerator WaveCoolDown()
    {
        inCooldown = true;
        waveOver.gameObject.SetActive(true);

        yield return new WaitForSeconds(waveDelay);

        inCooldown = false;
        waveOver.gameObject.SetActive(false);

        currentRobotsPerWave += 1;
        
        StartNextWave();
    }
}
