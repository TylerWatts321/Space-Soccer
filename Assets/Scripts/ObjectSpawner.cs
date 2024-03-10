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

    public AudioClip goalSound;

    public float SpawnTimeMultiplier = 6f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (BountyManager.instance.currentWave.AsteroidGoals.Contains(this)     
            && other.gameObject.layer == 6 
            && other.name.Contains("(Launched)"))
        {

            Asteroid asteroid = other.gameObject.GetComponent<Asteroid>();
            BountyManager.instance.UpdatePlayerScore(asteroid.Score);
            AudioManager.Instance.PlaySound(AudioManagerChannels.SoundEffectChannel, goalSound);
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
            spawnedAsteroid = Instantiate(AsteroidPrefab, new Vector2(DetermineSpawnPointX(colliderBounds, colliderCenter), colliderCenter.y), Quaternion.identity);
            spawnedAsteroid.GetComponent<Asteroid>().destination = new Vector2(DetermineSpawnPointX(colliderBounds, colliderCenter), -colliderCenter.y);
            spawnedAsteroid.GetComponent<Asteroid>().SendFlying();
        }
        else if (side.Equals(CameraSide.Right) || side.Equals(CameraSide.Left))
        {
            spawnedAsteroid = Instantiate(AsteroidPrefab, new Vector2(colliderCenter.x, DetermineSpawnPointY(colliderBounds, colliderCenter)), Quaternion.identity);
            spawnedAsteroid.GetComponent<Asteroid>().destination = new Vector2(-colliderCenter.x, DetermineSpawnPointY(colliderBounds, colliderCenter));
            spawnedAsteroid.GetComponent<Asteroid>().SendFlying();
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
