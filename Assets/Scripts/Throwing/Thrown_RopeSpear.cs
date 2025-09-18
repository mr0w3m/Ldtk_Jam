using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thrown_RopeSpear : ThrowableObject
{
    [SerializeField] private RopeSpearPlatform _ropeSpearPlatform;
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
            RopeSpearPlatform spawnedSpear = Instantiate(_ropeSpearPlatform, hitInfo.point, Quaternion.identity);
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
}
