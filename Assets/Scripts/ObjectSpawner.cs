using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObjectSpawner : MonoBehaviour
{
    public enum CameraSide { Left, Right, Up, Down};
    public CameraSide side;
    public BoxCollider2D spawnBox;
    public GameObject AsteroidPrefab;

    public float SpawnTimeMultiplier = 6f;

    // Kaleb Code
    private AsteroidPoolManager asteroidPoolManager;

    private void Start()
    {
        asteroidPoolManager = AsteroidPoolManager.instance;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (BountyManager.instance.currentWave.AsteroidGoals.Contains(this) 
            && other.gameObject.layer == 6 
            && other.name.Contains("(Launched)"))
        {

            Asteroid asteroid = other.gameObject.GetComponent<Asteroid>();
            BountyManager.instance.UpdatePlayerScore(asteroid.Score);
        }
    }

    public void StartSpawningAsteroids(float minSpawnTime, float maxSpawnTime)
    {
        InvokeRepeating(nameof(SpawnAsteroid), 0, Random.Range(minSpawnTime * SpawnTimeMultiplier, maxSpawnTime * SpawnTimeMultiplier));
    }

    public void StopSpawningAsteroids()
    {
        CancelInvoke(nameof(SpawnAsteroid));
    }

    public void SpawnAsteroid()
    {
        Bounds colliderBounds = spawnBox.bounds;
        Vector3 colliderCenter = colliderBounds.center;

        GameObject spawnedAsteroid = null;
        if (side.Equals(CameraSide.Up) || side.Equals(CameraSide.Down))
        {
            // Object pooling
            spawnedAsteroid = asteroidPoolManager.GetAsteroid();
            
            spawnedAsteroid.transform.position = new Vector2(DetermineSpawnPointX(colliderBounds, colliderCenter), colliderCenter.y);
            
            Asteroid asteroidComponent = spawnedAsteroid.GetComponent<Asteroid>();
            asteroidComponent.destination = new Vector2(DetermineSpawnPointX(colliderBounds, colliderCenter), -colliderCenter.y);
            asteroidComponent.SendFlying();
        }
        else if (side.Equals(CameraSide.Right) || side.Equals(CameraSide.Left))
        {
            // Object Pooling
            spawnedAsteroid = asteroidPoolManager.GetAsteroid();
            
            spawnedAsteroid.transform.position = new Vector2(colliderCenter.x, DetermineSpawnPointY(colliderBounds, colliderCenter));

            Asteroid asteroidComponent = spawnedAsteroid.GetComponent<Asteroid>();
            asteroidComponent.destination = new Vector2(-colliderCenter.x, DetermineSpawnPointY(colliderBounds, colliderCenter));
            asteroidComponent.SendFlying();
        }
    }

    public void SpawnLevelObject(GameObject prefab)
    {
        Bounds colliderBounds = spawnBox.bounds;
        Vector3 colliderCenter = colliderBounds.center;

        Instantiate(prefab, new Vector2(DetermineSpawnPointX(colliderBounds, colliderCenter),
                    DetermineSpawnPointY(colliderBounds, colliderCenter)), Quaternion.identity);
    }

    public float DetermineSpawnPointX(Bounds colliderBounds, Vector3 colliderCenter)
    {
        float randomX = Random.Range(colliderCenter.x - colliderBounds.extents.x, colliderCenter.x + colliderBounds.extents.x);

        return randomX;
    }

    public float DetermineSpawnPointY(Bounds colliderBounds, Vector3 colliderCenter)
    {
        float randomY = Random.Range(colliderCenter.y - colliderBounds.extents.y, colliderCenter.y + colliderBounds.extents.y);

        return randomY;
    }
}
