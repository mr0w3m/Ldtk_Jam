using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thrown_Anchor : ThrowableObject
{
    [SerializeField] private GameObject _rockPrefab;
    
    [SerializeField] private GameObject _startPosObj;
    [SerializeField] private GameObject _endPosObj;

    [SerializeField] private HitBoxCheck _hitBoxCheck;
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private EdgeCollider2D _edgeColl;
    [SerializeField] private PlatformEffector2D _platformEffector;
    [SerializeField] private GameObject _spriteObj;
    [SerializeField] private GameObject _enemyDamager;



    public override void Throw()
    {
        base.Throw();
        _startPosObj.transform.SetParent(null);
        _endPosObj.transform.SetParent(null);
        _edgeColl.transform.SetParent(null);
        _edgeColl.transform.position = Vector3.zero;

        _startPosObj.transform.position = Actor.i.movement.Rb2D.transform.position;
        _endPosObj.transform.position = Actor.i.movement.Rb2D.transform.position;


        _hitBoxCheck.EnterCollider_Info += Land;
    }


    private void Land(CollisionInfo info)
    {
        _hitBoxCheck.EnterCollider_Info -= Land;
        _spriteObj.SetActive(false);
        Debug.Log("Land");

        rb.velocity = Vector2.zero;
        rb.isKinematic = true;
        _endPosObj.transform.position = info.pos;
        GameObject go = Instantiate(_rockPrefab, _startPosObj.transform.position, Quaternion.identity);
        //go.GetComponent<InteractableResource>().parentToDestroy = this.gameObject;
        GameObject go2 = Instantiate(_rockPrefab, _endPosObj.transform.position, Quaternion.identity);
        //go2.GetComponent<InteractableResource>().parentToDestroy = this.gameObject;


        _startPosObj.transform.SetParent(this.gameObject.transform);
        _endPosObj.transform.SetParent(this.gameObject.transform);
        go.transform.SetParent(this.gameObject.transform);
        go2.transform.SetParent(this.gameObject.transform);


        RerenderLine();
        _edgeColl.gameObject.SetActive(true);
        _edgeColl.gameObject.transform.SetParent(this.gameObject.transform);

        _enemyDamager.SetActive(false);

        float oa = Mathf.Abs(_startPosObj.transform.localPosition.y) / Mathf.Abs(_startPosObj.transform.localPosition.x);
        float angle = Mathf.Rad2Deg * Mathf.Atan(oa);
        _platformEffector.rotationalOffset = angle * (Actor.i.movement.Direction == Direction.right ? 1 : -1);
    }

    private void RerenderLine()
    {
        
        List<Vector2> _b = new List<Vector2>();


        _b.Add(_startPosObj.transform.position);
        _b.Add(_endPosObj.transform.position);
        _edgeColl.SetPoints(_b);
        Debug.Log("actual position: " + _startPosObj.transform.position);
        Debug.Log("actual position: " + _edgeColl.points[0]);
        Debug.Log("actual position: " + _endPosObj.transform.position);
        Debug.Log("actual position: " + _edgeColl.points[1]);
    }

    private void OnDestroy()
    {
        Destroy(_edgeColl.gameObject);
    }
}
