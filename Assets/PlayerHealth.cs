using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public event Action<float, float> UpdateHealth = delegate { };

    public float CollisionMultiplier = 15f;
    public float ImpactBaseValue = 3f;

    public float HP;
    public float MAX_HP;

    public float RegenRate;
    public float HP_PerSec;

    private SpriteRenderer PlayerSprite;

    private float t = 0.35f;
    private bool iFrames = false;

    private void Awake()
    {
        PlayerSprite = GetComponentInChildren<SpriteRenderer>();
    }

    public void Start()
    {
        BountyManager.instance.WaveEnded += Regen;
        BountyManager.instance.WaveStarted += EndRegen;
        BountyManager.instance.TurnOff += Disable;
        UpdateHealth(HP, MAX_HP);
    }

    public void Disable()
    {
        BountyManager.instance.WaveEnded -= Regen;
        BountyManager.instance.WaveStarted -= EndRegen;
        BountyManager.instance.TurnOff -= Disable;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Damage calculation
        float amount = Mathf.Abs(collision.relativeVelocity.magnitude
        * CollisionMultiplier);
        amount = Mathf.Log(amount, ImpactBaseValue);
        amount = amount > 1 ? amount : 0;

        Damage(amount);
    }

    public void Damage(float amount)
    {
        amount = (int)amount;

        if (!iFrames)
        {
            HP = Mathf.Clamp(HP - amount, 0f, MAX_HP);
            UpdateHealth(HP, MAX_HP);
            t = 0.2f;
            StartCoroutine(PlayerInvuln());
        }

        if (HP <= 0 && !BountyManager.instance.IsGameOver)
        {
            BountyManager.instance.GameOver();
        }
    }
    void EndRegen(BountyManager.Wave wave)
    {
        if (wave.WaveNum % 5 != 0) { return; }
        CancelInvoke(nameof(Heal));
    }

    public void Regen(BountyManager.Wave wave)
    {
        if(wave.WaveNum % 5 != 0) { return; }
        InvokeRepeating(nameof(Heal), 0f, RegenRate);
    }

    private void Heal()
    {
        HP = Mathf.Clamp(HP + HP_PerSec, 0f, MAX_HP);
        UpdateHealth(HP, MAX_HP);
    }

    IEnumerator PlayerInvuln()
    {
        iFrames = true;

        PlayerSprite.enabled = !PlayerSprite.enabled;

        yield return new WaitForSeconds(t);

        if (t >= 0.01f)
        {
            t -= 0.03f;
            StartCoroutine(PlayerInvuln());
            yield break;
        }
        else
        {
            PlayerSprite.enabled = true;
            iFrames = false;
        }
        yield break;
    }
}
