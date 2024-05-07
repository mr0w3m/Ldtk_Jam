using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thrown_Spear : ThrowableObject
{
    [SerializeField] private SpearPlatform _spearPlatform;
    [SerializeField] private Collider2D _collider;
    private bool _collided = false;

    public float angleOffset;

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
}
