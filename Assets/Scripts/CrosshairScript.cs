using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.DefaultInputActions;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CrosshairScript : MonoBehaviour
{
    public Sprite crosshairImage;

    public bool crosshair = true;

    public PlayerInput PlayerInput;

    private Image CrosshairReticle;

    // Start is called before the first frame update
    void OnEnable()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
        PlayerInput.Rotate += UpdateCrosshair;
        CrosshairReticle = GetComponent<Image>();
        CrosshairReticle.sprite = crosshairImage;
        CrosshairReticle.rectTransform.pivot = new Vector2(0.5f, 0.5f);
    }

    public void OnDisable()
    {
        PlayerInput.Rotate -= UpdateCrosshair;
    }

    void UpdateCrosshair(Vector2 mousePos)
    {
        CrosshairReticle.rectTransform.position = mousePos;
        
    }
}
