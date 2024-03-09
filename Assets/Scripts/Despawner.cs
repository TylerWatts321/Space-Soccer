using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Despawner : MonoBehaviour
{
    private AsteroidPoolManager asteroidPoolManager;

    private void Start()
    {
        asteroidPoolManager = AsteroidPoolManager.instance;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == 6)
        {
            if (other.TryGetComponent<Asteroid>(out Asteroid asteroid))
            {
                //print("Despawner found an asteroid! Releasing to pool...");
                asteroidPoolManager.RemoveAsteroid(asteroid.gameObject);
            }
        }
    }
}


