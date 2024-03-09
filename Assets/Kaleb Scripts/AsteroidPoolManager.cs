using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidPoolManager : MonoBehaviour
{
    public static AsteroidPoolManager instance;

    public GameObject asteroidPrefab;

    public List<GameObject> asteroids = new List<GameObject>();

    private void Awake()
    {
        if (instance != null && AsteroidPoolManager.instance != this)
        {
            Destroy(this);
        }
        else
        {
            AsteroidPoolManager.instance = this;
        }
    }

    public GameObject GetAsteroid()
    {
        GameObject returnObj = null;

        if (asteroids.Count > 0)
        {
           // print("Grabbing from pool!");
            returnObj = asteroids[0];
            returnObj.SetActive(true);
            asteroids.RemoveAt(0);
            return returnObj;
        }
        else
        {
            //print("Pool was empty, instantiating new object...");
            returnObj = Instantiate(asteroidPrefab);
            return returnObj;
        }
    }

    public void RemoveAsteroid(GameObject asteroid)
    {
        if (asteroids.Count < 10)
        {
            //print("Removing asteroid from the world and adding it to pool!");
            asteroid.SetActive(false);
            asteroid.transform.position = new Vector3(-1000, -1000, 1000);
            asteroids.Add(asteroid);
        }
        else
        {
            //print("Pool is too large! Removing object from scene...");
            Destroy(asteroid);
        }
    }
}
