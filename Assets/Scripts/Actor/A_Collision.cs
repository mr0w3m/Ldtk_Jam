using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_Collision : MonoBehaviour
{
    //[SerializeField] private A_AnimatorController _characterAnim;
    //[SerializeField] private A_AnimatorController _weaponAnim;

    [Header("Colliders")]
    [SerializeField] private Collider2D _rbC;
    [SerializeField] private Rigidbody2D _rb2D;

    [Header("Triggers")]
    [SerializeField] private BoxCollider2D _groundedT;
    [SerializeField] private BoxCollider2D _T;
    [SerializeField] private LayerMask _groundLayer;

    public bool checkGrounded;

    private bool _grounded;
    public bool Grounded
    {
        get { return _grounded; }
    }


    public BoxCollider2D GetTrigger
    {
        get { return _T; }
    }

    public event Action LeftGround;
    public void OnLeftGround()
    {
        if (LeftGround != null)
        {
            LeftGround.Invoke();
        }
    }

    public event Action ReturnedToGround;
    private void OnReturnedToGround()
    {
        if (ReturnedToGround != null)
        {
            ReturnedToGround.Invoke();
        }
    }

    public void SetCollisionState(bool state)
    {
        if (_rb2D != null)
        {
            _rb2D.simulated = false;
        }
        _rbC.enabled = state;
    }

    private void Start()
    {
        if (checkGrounded)
        {
            StartCoroutine(CheckLoop());
        }
    }

    private IEnumerator CheckLoop()
    {
        while (true)
        {
            GroundedCheck();
            yield return 0f;
        }
    }

    private void GroundedCheck()
    {
        bool prevGrounded = _grounded;

        Collider2D hitC = Physics2D.OverlapBox(_groundedT.transform.position, _groundedT.size, 0f, _groundLayer);
        _grounded = hitC != null ? true : false;

        if (!prevGrounded && _grounded)
        {
            OnReturnedToGround();
            SetAnim(true);
        }

        if (prevGrounded && !_grounded)
        {
            OnLeftGround();
            SetAnim(false);
        }
    }

    private void SetAnim(bool state)
    {
        //if (_characterAnim != null)
        //{
        //    _characterAnim.SetBool("Grounded", state);
        //}
        //if (_weaponAnim != null)
        //{
        //    _weaponAnim.SetBool("Grounded", state);
        //}
    }
}