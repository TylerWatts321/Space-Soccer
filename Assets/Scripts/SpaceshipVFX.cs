using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipVFX : MonoBehaviour
{
    private Animator _playerAnimator;

    private void Awake()
    {
        GetComponent<PlayerInput>().Move += ThrustAnim;
        GetComponent<PlayerInput>().Stop += StopAnim;
        _playerAnimator = GetComponentInChildren<Animator>();
    }
    private void ThrustAnim(Vector2 direction)
    {
        //Change animation
        switch (direction.y)
        {
            case 0:
                {
                    if (direction.x == 0)
                        _playerAnimator.SetInteger("moveState", 0);

                    if (direction.x == 1)
                        _playerAnimator.SetInteger("moveState", 2);

                    if (direction.x == -1)
                        _playerAnimator.SetInteger("moveState", 1);
                    break;
                }
            case 1:
                {

                    if (direction.x == 0)
                        _playerAnimator.SetInteger("moveState", 3);
                    break;
                }
            case -1:
                {
                    if (direction.x == 0)
                        _playerAnimator.SetInteger("moveState", 6);

                    break;
                }
        }
        if (direction.x > 0 && direction.y > 0)
        {
                _playerAnimator.SetInteger("moveState", 4);
        }

        if (direction.x < 0 && direction.y > 0)
        {
                _playerAnimator.SetInteger("moveState", 5);
        }

        if (direction.x > 0 && direction.y < 0)
        {
            _playerAnimator.SetInteger("moveState", 7);
        }

        if (direction.x < 0 && direction.y < 0)
        {
            _playerAnimator.SetInteger("moveState", 8);
        }
    }

    private void StopAnim()
    {
        _playerAnimator.SetInteger("moveState", 0);
    }
}
