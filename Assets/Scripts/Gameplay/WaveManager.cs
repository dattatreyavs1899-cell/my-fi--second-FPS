using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance;

    [Header("Object Pooling")]
    public GameObject enemyPrefab;
    public int poolSize = 20;
    private List<GameObject> enemyPool;

    [Header("Dynamic Spawn Settings")]
    public float timeBetweenSpawns = 1f;
    public float minSpawnDistance = 10f;
    public float maxSpawnRadius = 25f;

    [Header("Rising Animation Settings")]
    public float spawnDepth = 2.5f;
    public float riseSpeed = 1.5f;

    [Header("Wave Stats")]
    public int currentWave = 0;
    public int maxWaves = 3;
    public int baseEnemies = 5;
    public float breakTime = 15f;

    [Header("Supply Crates")]
    public GameObject crateGroup;

    [Header("UI")]
    public TextMeshProUGUI waveInfoText;

    private int enemiesToSpawnThisWave;
    private int enemiesAlive;
    private bool isWaveActive;

    private Transform playerTransform;

    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) playerTransform = playerObj.transform;

        InitializePool();
        StartCoroutine(StartNextWave());
    }

    void InitializePool()
    {
        enemyPool = new List<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(enemyPrefab);

            obj.transform.SetParent(this.transform);

            obj.SetActive(false);
            enemyPool.Add(obj);
        }
    }

    GameObject GetPooledEnemy()
    {
        foreach (GameObject obj in enemyPool)
        {
            if (!obj.activeInHierarchy) return obj;
        }
        return null;
    }

    IEnumerator StartNextWave()
    {
        float timer = (currentWave == 0) ? 3f : breakTime;

        while (timer > 0)
        {
            if (waveInfoText != null)
            {
                if (currentWave == 0)
                    waveInfoText.text = "Starting in: " + Mathf.Ceil(timer).ToString();
                else
                    waveInfoText.text = "Break: " + Mathf.Ceil(timer).ToString() + "s";
            }

            timer -= Time.deltaTime;
            yield return null;
        }

        currentWave++;

        if (currentWave == 1 && GameOverManager.Instance != null)
        {
            GameOverManager.Instance.StartTimer();
        }

        if (waveInfoText != null) waveInfoText.text = "Wave " + currentWave;

        enemiesToSpawnThisWave = baseEnemies + ((currentWave - 1) * 2);
        enemiesAlive = enemiesToSpawnThisWave;
        isWaveActive = true;

        StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies()
    {
        int spawned = 0;
        while (spawned < enemiesToSpawnThisWave)
        {
            GameObject zombie = GetPooledEnemy();

            if (zombie != null)
            {
                Vector3 validSpot = GetValidSpawnPoint();
                StartCoroutine(RiseZombieFromGround(zombie, validSpot));
                spawned++;
            }

            yield return new WaitForSeconds(timeBetweenSpawns);
        }
    }

    Vector3 GetValidSpawnPoint()
    {
        if (playerTransform == null) return Vector3.zero;

        for (int i = 0; i < 30; i++)
        {
            Vector2 randomDir = Random.insideUnitCircle.normalized;

            float randomDist = Random.Range(minSpawnDistance, maxSpawnRadius);

            Vector3 potentialPoint = playerTransform.position + new Vector3(randomDir.x, 0, randomDir.y) * randomDist;

            NavMeshHit hit;
            if (NavMesh.SamplePosition(potentialPoint, out hit, 2f, NavMesh.AllAreas))
            {
                return hit.position;
            }
        }

        return playerTransform.position + (Vector3.forward * 5f);
    }

    IEnumerator RiseZombieFromGround(GameObject zombie, Vector3 finalSurfacePos)
    {
        NavMeshAgent agent = zombie.GetComponent<NavMeshAgent>();
        EnemyAI ai = zombie.GetComponent<EnemyAI>();

        if (agent != null) agent.enabled = false;
        if (ai != null) ai.enabled = false;

        zombie.transform.position = finalSurfacePos + (Vector3.down * spawnDepth);
        zombie.GetComponent<EnemyHealth>().ResetHealth();
        zombie.SetActive(true);

        Vector3 startPos = zombie.transform.position;
        float percent = 0f;

        while (percent < 1f)
        {
            percent += Time.deltaTime * riseSpeed;
            zombie.transform.position = Vector3.Lerp(startPos, finalSurfacePos, percent);
            yield return null;
        }

        zombie.transform.position = finalSurfacePos;

        if (agent != null) agent.enabled = true;
        if (ai != null) ai.enabled = true;
    }

    public void OnEnemyDied()
    {
        enemiesAlive--;

        if (enemiesAlive <= 0 && isWaveActive)
        {
            isWaveActive = false;

            if (currentWave >= maxWaves)
            {
                if (waveInfoText != null) waveInfoText.text = "YOU SURVIVED!";

                HideCrates();

                if (GameOverManager.Instance != null)
                {
                    GameOverManager.Instance.ShowGameOver(true);
                }
            }
            else
            {
                if (crateGroup != null) crateGroup.SetActive(true);
                StartCoroutine(StartNextWave());
            }
        }
    }
    public void HideCrates()
    {
        if (crateGroup != null) crateGroup.SetActive(false);
    }
}