using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Whippable : MonoBehaviour
{
    public Player p;
    public Rigidbody2D playerRb;

    float diff;
    float lastRotation;

    void Update()
    {
        if (p == null)
            return;

        diff = playerRb.rotation - lastRotation;
        lastRotation = playerRb.rotation;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Damage calculation
        float amount = Mathf.Abs(collision.relativeVelocity.magnitude * diff);

        Asteroid asteroid = collision.gameObject.GetComponent<Asteroid>();

        if (asteroid != null)
            asteroid.Damage((int) amount);
    }
}
