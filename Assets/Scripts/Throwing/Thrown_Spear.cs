using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thrown_Spear : ThrowableObject
{
    [SerializeField] private SpearPlatform _spearPlatform;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private Transform _startLinecastPos; 
    [SerializeField] private Transform _endLinecastPos;
    [SerializeField] private AudioClip _hitAudioClip;
    private bool _collided = false;

    public float angleOffset;


    public float _castRadius;
    public float _castDistance;


    private void Update()
    {
        if (_collided)
        {
            return;
        }
        Vector2 castDirection = ((Vector2)_startLinecastPos.position - (Vector2)_endLinecastPos.position);
        RaycastHit2D otherHitInfo = Physics2D.CircleCast(_startLinecastPos.position, _castRadius, castDirection.normalized, _castDistance, _groundLayer);
        RaycastHit2D hitInfo = Physics2D.Linecast(_startLinecastPos.position, ((Vector2)_endLinecastPos.position), _groundLayer);
        if (hitInfo.collider != null)
        {
            _collided = true;
            SpearPlatform spawnedSpear = Instantiate(_spearPlatform, hitInfo.point, Quaternion.identity);
            Vector3 _direction = new Vector3(-hitInfo.normal.x, -hitInfo.normal.y, 0);
            Debug.Log("Normal of hit surface: " + hitInfo.normal.ToString());

            if (hitInfo.normal.x < 0)
            {
                spawnedSpear.FlipPlatformDirection();
            }

            _direction = _direction.normalized;
            float angleDir = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
            angleDir += angleOffset;

            spawnedSpear.transform.eulerAngles = new Vector3(spawnedSpear.transform.eulerAngles.x, spawnedSpear.transform.eulerAngles.y, angleDir);

            AudioController.control.PlayClip(_hitAudioClip, Random.Range(0.5f, 1));


            Destroy(this.gameObject);
        }
    }
    
    /*
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!_collided)
        {
            _collided = true;
            ContactPoint2D contactPoint = collision.GetContact(0);
            SpearPlatform spawnedSpear = Instantiate(_spearPlatform, contactPoint.point, Quaternion.identity);
            Vector3 _direction = new Vector3(contactPoint.normal.x, contactPoint.normal.y, 0);
            Debug.Log("Normal of hit surface: " + contactPoint.normal.ToString());
            if (contactPoint.normal.x < 0)
            {
                spawnedSpear.FlipPlatformDirection();
            }


            _direction = _direction.normalized;
            float angleDir = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
            angleDir += angleOffset;

            spawnedSpear.transform.eulerAngles = new Vector3(spawnedSpear.transform.eulerAngles.x, spawnedSpear.transform.eulerAngles.y, angleDir);

            Destroy(this.gameObject);
        }
    }
    */
}
