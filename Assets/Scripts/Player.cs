using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(SpaceshipController))]
[RequireComponent(typeof(SpaceshipVFX))]

public class Player : MonoBehaviour
{
    [Header("Couplings")]
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private SpaceshipController _playerController;
    [SerializeField] private SpaceshipVFX _playerVFX;

    public void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _playerController = GetComponent<SpaceshipController>();
        _playerVFX = GetComponent<SpaceshipVFX>();
    }
}
