using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class BountyManager : MonoBehaviour
{
    public static BountyManager instance;
    public bool IsGameOver;

    [Header("Spawners")]
    public List<ObjectSpawner> AsteroidSpawners = new List<ObjectSpawner>();

    public event Action<Wave> WaveStarted = delegate { };
    public event Action<Wave> WaveEnded = delegate { };
    public event Action<int> UpdateScore = delegate { };
    public event Action GameOverEvent = delegate { };
    public event Action GameWon = delegate { };
    public event Action DisableInput = delegate { };
    public event Action EnableInput = delegate { };
    public event Action TurnOff = delegate { };


    [Range(0f, 20f)]
    public float timeBetweenWaves;
    [Range(.25f, 1f)]
    public float maxWaveLength;

    public AnimationCurve MaxTimeToSpawnAsteroid;
    public AnimationCurve MinTimeToSpawnAsteroid;
    public int WaveCap = 50;

    private int score = 0;

    [System.Serializable]
    public struct Wave
    {
        public List<ObjectSpawner> AsteroidSpawners;
        public List<ObjectSpawner> AsteroidGoals;
        public float WaveStartTime;
        public int WaveNum;
    }
    public Wave currentWave;

    public enum SpawnState {WAITING, SPAWNING };
    public SpawnState state = SpawnState.SPAWNING;

    private int waveCount = 0;

    // Kaleb Code
    public float handleWaveEvery = 0.1f;
    
    public void Initialize()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        instance = null;
        Initialize();
    }

    public void Start()
    {
        UpdateScore(score);
        StartCoroutine(HandleWavesCoroutine());
    }

    public void UpdatePlayerScore(int num)
    {
        score += num;
        UpdateScore(score);
    }

    private float waveCountdown; // Used for keeping track of wave timings.
    private void HandleWaves()
    {
        if (state == SpawnState.WAITING)
        {
            if(WaveOver())
            {
                WaveCompleted();
            }

            return;
        }
        else if (waveCountdown <= 0) { CreateWave(); }
        else { waveCountdown -= Time.deltaTime;  }
    }

    private IEnumerator HandleWavesCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(handleWaveEvery);

            if (state == SpawnState.WAITING)
            {
                if (WaveOver())
                {
                    WaveCompleted();
                }

                continue;
            }
            else if (waveCountdown <= 0) { CreateWave(); }
            else { waveCountdown -= handleWaveEvery; }
        }
    }

    public bool WaveOver()
    {
        if ((Time.time - currentWave.WaveStartTime) >= maxWaveLength * 60) { return true; }

        else { return false; }
    }

    private List<ObjectSpawner> GetRandomSpawnBoxes()
    {
        List<ObjectSpawner> spawners = new List<ObjectSpawner>();
        System.Random a = new System.Random();

        ObjectSpawner spawner = AsteroidSpawners[a.Next(0, 4)];
        ObjectSpawner siblingSpawner = null;

        switch(spawner.side)
        {
            case ObjectSpawner.CameraSide.Left:
                siblingSpawner = AsteroidSpawners.Find(x => x.side == ObjectSpawner.CameraSide.Right);
                break;
            case ObjectSpawner.CameraSide.Right:
                siblingSpawner = AsteroidSpawners.Find(x => x.side == ObjectSpawner.CameraSide.Left);
                break;
            case ObjectSpawner.CameraSide.Up:
                siblingSpawner = AsteroidSpawners.Find(x => x.side == ObjectSpawner.CameraSide.Down);
                break;
            case ObjectSpawner.CameraSide.Down:
                siblingSpawner = AsteroidSpawners.Find(x => x.side == ObjectSpawner.CameraSide.Up);
                break;
        }

        spawners.Add(spawner);
        spawners.Add(siblingSpawner);
        
        return spawners;
    }

    private List<ObjectSpawner> GetAsteroidGoals()
    {
        List<ObjectSpawner> spawners = new List<ObjectSpawner>();

        foreach (ObjectSpawner objectSpawner in AsteroidSpawners)
        {
            if (currentWave.AsteroidSpawners.Contains(objectSpawner))
                continue;
            else
                spawners.Add(objectSpawner);
        }

        return spawners;
    }
    private void CreateWave()
    {
        waveCount++;

        if (waveCount >= WaveCap)
        {
            GameWon();
            return;
        }

        currentWave = new Wave();
        currentWave.AsteroidSpawners = GetRandomSpawnBoxes();
        currentWave.AsteroidGoals = GetAsteroidGoals();
        currentWave.WaveStartTime = Time.time;
        currentWave.WaveNum = waveCount;

        SpawnAsteroids(MinTimeToSpawnAsteroid.Evaluate(waveCount / WaveCap),
                       MaxTimeToSpawnAsteroid.Evaluate(waveCount / WaveCap));

        state = SpawnState.WAITING;
        WaveStarted(currentWave);
    }

    private void WaveCompleted()
    {
        WaveEnded(currentWave);
        waveCountdown = timeBetweenWaves;
        StopAsteroids();
        state = SpawnState.SPAWNING;
    }

    public void SpawnAsteroids(float minSpawnTime, float maxSpawnTime)
    {
        foreach (ObjectSpawner spawner in currentWave.AsteroidSpawners)
        {
            spawner.StartSpawningAsteroids(minSpawnTime, maxSpawnTime);
        }
    }

    public void StopAsteroids()
    {
        foreach (ObjectSpawner spawner in currentWave.AsteroidSpawners)
        {
            spawner.StopSpawningAsteroids();
        }
    }

    public void GameOver()
    {
        Debug.LogError("Game Over");
        DisableInput();
        GameOverEvent();
    }

    public void RestartGame()
    {
        Debug.LogError("Restarting Game");
        Time.timeScale = 1f;
        TurnOff();
        EnableInput();
        SceneManager.LoadScene("AsteroidBelt");
    }
}
