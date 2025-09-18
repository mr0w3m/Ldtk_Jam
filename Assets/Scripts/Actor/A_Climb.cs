using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_Climb : MonoBehaviour
{
    [SerializeField] private GameObject _playerCenterRef;
    [SerializeField] private float _climbSpeed;
    [SerializeField] private LayerMask _targetLayer;

    private bool _canClimb;


    private void EnableClimb()
    {
        _canClimb = true;
    }

    private void DisableClimb()
    {
        _canClimb = false;
    }

    private void FixedUpdate()
    {
        //check climbable

        Collider2D hitObj = Physics2D.OverlapCircle((Vector2)_playerCenterRef.transform.position, 0.75f, _targetLayer);
        if (hitObj != null)
        {
            //we hit a trigger
            ClimbableObject potentialClimbable = hitObj.GetComponent<ClimbableObject>();
            if (potentialClimbable != null)
            {
                //then we hit a climbable surface
                EnableClimb();
            }
            else
            {
                DisableClimb();
            }
        }
        else
        {
            DisableClimb();
        }


        if (_canClimb)
        {
            ReadClimbInput();
        }
    }

    private void ReadClimbInput()
    {
        if (Actor.i.input.LSY > 0.1f)
        {
            Vector2 direction = new Vector2(0, Actor.i.input.LSY);
            Actor.i.movement.MoveUp(direction * _climbSpeed);
        }
    }
}
