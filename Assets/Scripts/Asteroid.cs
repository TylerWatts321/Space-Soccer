using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public Rigidbody2D rb2d;
    public float speed;
    public float rotateSpeed;
    public Vector3 destination;
    public AudioClip soundFile;

    public int Score;

    public int health;

    // Kaleb Code
    private AsteroidPoolManager asteroidPoolManager;

    public void Start()
    {
        transform.eulerAngles = new Vector3(0, 0, Random.Range(0, 360));
        asteroidPoolManager = AsteroidPoolManager.instance;
    }
    public void SendFlying()
    {
        Vector3 dir = destination - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
        rb2d.velocity = transform.TransformDirection(Vector3.right * speed);
        StartCoroutine(KilltheAsteroid(20));
    }

    IEnumerator KilltheAsteroid(int time)
    {
        yield return new WaitForSecondsRealtime(time);

        // Release back to pool
        asteroidPoolManager.RemoveAsteroid(this.gameObject);
    }

    private void FixedUpdate()
    {
        if (destination == null || destination == Vector3.zero)
            transform.Rotate(0, 0, rotateSpeed * 5.0f); //rotates 50 degrees per second around z axis
    }

    public void Damage(int damage)
    {

    }

}
