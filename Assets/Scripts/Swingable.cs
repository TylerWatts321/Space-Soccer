using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swingable : MonoBehaviour
{
    public float radius;

    [SerializeField] public GameObject origin;
    private Rigidbody2D originRb;

    [Range(0.0f, 2.0f)]
    [SerializeField] private float turnSensitivity;

    [Range(0.0f, 2.0f)]
    [SerializeField] private float reboundIntensity;

    [Range(-50.0f, 50.0f)]
    [SerializeField] private float initialForce;

    [Range(0.0f, 50.0f)]
    [SerializeField] public float angleLimit;
    
    public float lastRotation;

    public float acceleration;

    public float currentAngle;
    public float maxAngle;

    void Start()
    {
        originRb = origin.GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        CalculateAngle();

        // Enforce new position
        transform.localEulerAngles.Set(0, 0, currentAngle);

        float x = radius * Mathf.Cos(Mathf.Deg2Rad * currentAngle);
        float y = radius * Mathf.Sin(Mathf.Deg2Rad * currentAngle);

        transform.localPosition = new Vector2(x, y);
    }

    void CalculateAngle()
    {
        float currentRotation = originRb.rotation;

        // Calculate angle change
        float angularVel = -originRb.angularVelocity * Time.fixedDeltaTime;

        acceleration += (angularVel * turnSensitivity) * 0.01f;
        maxAngle = Mathf.Abs(angleLimit * (acceleration)) * 0.5f;

        float diff = Mathf.Abs(currentRotation - lastRotation);
        if (diff < 2) acceleration *= reboundIntensity;

        currentAngle += acceleration;

        if (currentAngle >= maxAngle)
        {
            currentAngle = maxAngle;
        }

        if (currentAngle <= -maxAngle)
        {
            currentAngle = -maxAngle;
        }

        acceleration = Mathf.Min(acceleration, 3);
        acceleration = Mathf.Max(acceleration, -3);

        lastRotation = currentRotation;
    }
}
