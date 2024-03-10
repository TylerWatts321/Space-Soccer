using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(UnityEngine.InputSystem.PlayerInput))]
public class PlayerInput : MonoBehaviour
{
    private Vector2 moveDir;
    public event Action<Vector2> Move = delegate { };
    public event Action Stop = delegate { };
    public event Action<Vector2> Rotate = delegate { };
    public event Action OnFireStarted = delegate { };
    public event Action OnFireCanceled = delegate { };

    public AudioClip sideThrusterClip;
    public AudioClip thrusterClip;

    private Camera MainCamera;
    bool MoveHeld;
    private void Start()
    {
        MainCamera = Camera.main;
        GetComponent<UnityEngine.InputSystem.PlayerInput>().enabled = false;
        GetComponent<UnityEngine.InputSystem.PlayerInput>().enabled = true;
        BountyManager.instance.DisableInput += SwitchToUI;
        BountyManager.instance.TurnOff += OnDisable;
    }

    private void OnDisable()
    {
        BountyManager.instance.DisableInput -= SwitchToUI;
        BountyManager.instance.TurnOff -= OnDisable;
        SwitchToGame();
        GetComponent<UnityEngine.InputSystem.PlayerInput>().currentActionMap.actions[0].Disable();
        GetComponent<UnityEngine.InputSystem.PlayerInput>().currentActionMap.actions[1].Disable();
        GetComponent<UnityEngine.InputSystem.PlayerInput>().currentActionMap.actions[2].Disable();

    }

    public void SwitchToUI()
    {
        GetComponent<UnityEngine.InputSystem.PlayerInput>().SwitchCurrentActionMap("UI");
        Debug.Log(GetComponent<UnityEngine.InputSystem.PlayerInput>().currentActionMap.name);
    }
    public void SwitchToGame()
    {
        GetComponent<UnityEngine.InputSystem.PlayerInput>().SwitchCurrentActionMap("Player");
        Debug.Log(GetComponent<UnityEngine.InputSystem.PlayerInput>().currentActionMap.name);
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.performed || MoveHeld == true) { MoveHeld = true; moveDir = context.ReadValue<Vector2>(); }

        if (context.ReadValue<Vector2>().x != 0)
        {
            AudioManager.Instance.PlaySound(AudioManagerChannels.ThrusterChannel, thrusterClip);
        }
        else
        {
            AudioManager.Instance.StopSound(AudioManagerChannels.ThrusterChannel);
        }
        if (context.ReadValue<Vector2>().y != 0)
        {
            AudioManager.Instance.PlaySound(AudioManagerChannels.SideThrusterChannel, sideThrusterClip);
        }
        else
        {
            AudioManager.Instance.StopSound(AudioManagerChannels.SideThrusterChannel);
        }
        
        if (context.canceled) { MoveHeld = false; Stop(); }
    }

    public void OnAim(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.performed)
            OnFireStarted();
        else if (context.canceled)
            OnFireCanceled();
    }

    public void Update()
    {
        if (MoveHeld) { Move(moveDir); }
    }

    public void LateUpdate()
    {
        Rotate(Mouse.current.position.ReadValue());
    }
}
