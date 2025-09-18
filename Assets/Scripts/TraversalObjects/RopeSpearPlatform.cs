using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeSpearPlatform : MonoBehaviour
{
    [SerializeField] PlatformEffector2D _platformEffector;

    //place rope
    [SerializeField] private GameObject _ropeStartPosition;
    [SerializeField] private GameObject _ropeEndPosition;

    [SerializeField] private BoxCollider2D _climbCollider;

    [SerializeField] private InteractableResource _spearPickup;
    [SerializeField] private GameObject _ropePickupPrefab;

    [SerializeField] private float _ropeRadius = 0.25f;
    [SerializeField] private LayerMask groundLayer;

    public void FlipPlatformDirection()
    {
        _platformEffector.rotationalOffset = 0;
    }

    private void Start()
    {
        //when placed? maybe we should have the spear inject init?
        PlaceRope();
        _spearPickup.ResourceAcquired += SpawnRope;

        if (((Mathf.Abs(transform.rotation.eulerAngles.z)) > 170 && (Mathf.Abs(transform.rotation.eulerAngles.z)) < 190) || (Mathf.Abs(transform.rotation.eulerAngles.z) < 10))
        {
            _climbCollider.gameObject.transform.localEulerAngles = new Vector3(0, 0, 0);
        }
        else
        {
            _climbCollider.gameObject.transform.localEulerAngles = new Vector3(0, 0, 90);
        }
    }

    private void PlaceRope()
    {
        RaycastHit2D[] hitInfo = Physics2D.CircleCastAll(_ropeStartPosition.transform.position, _ropeRadius, Vector2.down, 32f, groundLayer);
        if (hitInfo.Length > 0)
        {
            foreach(RaycastHit2D hit in hitInfo)
            {
                if (hit.collider.GetComponent<RopeSpearPlatform>() == null)
                {
                    //we hit the ground
                    //put the line renderer positions in place
                    _ropeEndPosition.transform.position = hit.point;
                    //collision
                    float distanceDownward = _ropeStartPosition.transform.position.y - hit.point.y;
                    _climbCollider.transform.position = new Vector2(_ropeStartPosition.transform.position.x, _ropeStartPosition.transform.position.y - distanceDownward / 2);
                    _climbCollider.size = new Vector2(_climbCollider.size.x, distanceDownward);
                    Debug.Log("Didn'tHitSelf,Placed.");
                    return;
                }
                else
                {
                    Debug.Log("Hit Self");
                }
            }
            Debug.Log("FinishedCastLoop1");
        }
    }

    private void SpawnRope()
    {
        Debug.Log("SpawnRope");
        Instantiate(_ropePickupPrefab, _ropeEndPosition.transform.position, Quaternion.identity).gameObject.transform.SetParent(null);
    }
}
