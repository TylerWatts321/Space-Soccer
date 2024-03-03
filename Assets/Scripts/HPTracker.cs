using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPTracker : MonoBehaviour
{
    public Image frontHealthBar;
    public Image backHealthBar;
    public Text healthText;

    [SerializeField] private float _chipSpeed = 1.75f;
    [SerializeField] private float endLimit = 0.002f;
    private float lerpTimer;
    float fillF;
    float fillB;

    public PlayerHealth PlayerHealth;

    float hFraction;
    void Awake()
    {
        float hFraction = 1f;
        frontHealthBar.fillAmount = hFraction;
        backHealthBar.fillAmount = hFraction;

        PlayerHealth.UpdateHealth += UpdateHealth;
    }

    public void OnDisable()
    {
        PlayerHealth.UpdateHealth -= UpdateHealth;
    }

    void UpdateHealth(float HP, float MAX_HP)
    {
        fillF = frontHealthBar.fillAmount;
        fillB = backHealthBar.fillAmount;
        hFraction = HP / MAX_HP;
        if (HP <= 0f)
            healthText.text = $"0 / {MAX_HP}";
        else if(HP < .5f)
            healthText.text = $"1 / {MAX_HP}";
        else
            healthText.text = $"{((int)HP).ToString()} / {MAX_HP}";

        if (fillB > hFraction)
        {
            frontHealthBar.fillAmount = hFraction;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / _chipSpeed;
            backHealthBar.fillAmount = Mathf.Lerp(fillB, hFraction, percentComplete);

            if (Mathf.Abs(fillB - hFraction) < endLimit)
                backHealthBar.fillAmount = hFraction;
        }
        else if (fillF < hFraction)
        {
            backHealthBar.fillAmount = hFraction;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / _chipSpeed;
            frontHealthBar.fillAmount = Mathf.Lerp(fillF, hFraction, percentComplete);

            if (Mathf.Abs(fillF - hFraction) < endLimit)
                frontHealthBar.fillAmount = hFraction;
        }

        lerpTimer /= 2;
    }

    private void Update()
    {
        fillF = frontHealthBar.fillAmount;
        fillB = backHealthBar.fillAmount;

        if (fillB > hFraction)
        {
            frontHealthBar.fillAmount = hFraction;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / _chipSpeed;
            backHealthBar.fillAmount = Mathf.Lerp(fillB, hFraction, percentComplete);

            if (Mathf.Abs(fillB - hFraction) < endLimit)
                backHealthBar.fillAmount = hFraction;
        }
        else if (fillF < hFraction)
        {
            backHealthBar.fillAmount = hFraction;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / _chipSpeed;
            frontHealthBar.fillAmount = Mathf.Lerp(fillF, hFraction, percentComplete);

            if (Mathf.Abs(fillF - hFraction) < endLimit)
                frontHealthBar.fillAmount = hFraction;
        }

        lerpTimer /= 2;
    }
}
