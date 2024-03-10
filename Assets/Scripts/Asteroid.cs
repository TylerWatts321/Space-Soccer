using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public Rigidbody2D rb2d;
    public float speed;
    public float rotateSpeed;
    public Vector3 destination;
    public AudioClip[] soundFiles;
    public AudioClip asteroidBreak;
    public int Score;

    public int health;

    public void Start()
    {
        transform.eulerAngles = new Vector3(0, 0, Random.Range(0, 360));
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
        Destroy(gameObject);
    }

    void Update()
    {
        if(destination == null || destination == Vector3.zero)
            transform.Rotate(0, 0, rotateSpeed * Time.deltaTime); //rotates 50 degrees per second around z axis
    }

    public void Damage(int damage)
    {

    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.name.Equals("Spaceship"))
            AudioManager.Instance.PlayCollisionSound(GetComponent<Collider2D>(), collision.collider, soundFiles[Random.Range(0, soundFiles.Length)]);
        else
            AudioManager.Instance.PlayCollisionSound(GetComponent<Collider2D>(), collision.collider, asteroidBreak);
    }

}
