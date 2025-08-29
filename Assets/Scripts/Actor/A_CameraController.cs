using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A_CameraController : MonoBehaviour
{
    public static A_CameraController i;

    [SerializeField] private GameObject _cameraObject;
    [SerializeField] private GameObject _leader;
    [SerializeField] private Vector3 _offset;

    [SerializeField] private float _maxDistance;
    [SerializeField] private Vector2 _maxDistanceDelta;

    [SerializeField] private AnimationCurve _curve;

    private Vector2 _additiveOffset = Vector2.zero;

    private Vector2 _offsetDown = Vector2.down;

    //camer Shake
    [SerializeField] private float _cameraShakeIntensityDefault;


    private float _cameraShakeTime = 0;
    private float _currentCameraShakeIntensity;
    //private Vector3 _cachedCameraPosition = Vector3.zero;


    private void Awake()
    {
        if (i == null)
        {
            i = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void Init(GameObject go)
    {
        _leader = go;
    }

    public void AddCameraShake(float timeToShake, float intensity)
    {
        if (intensity > _currentCameraShakeIntensity)
        {
            _currentCameraShakeIntensity = intensity;
        }

        if (_cameraShakeTime < timeToShake) 
        {
            _cameraShakeTime = timeToShake;
        }
    }

    private void Update()
    {
        if (_cameraShakeTime > 0)
        {
            CameraShake();
        }
    }

    private void CameraShake()
    {
        _cameraShakeTime -= Time.deltaTime;

        float x = Random.Range(-1f, 1) * _currentCameraShakeIntensity;
        float y = Random.Range(-1f, 1) * _currentCameraShakeIntensity;

        _currentCameraShakeIntensity *= 0.5f;
        _cameraObject.transform.localPosition = Vector3.zero + new Vector3(x, y, 0);

        if (_cameraShakeTime <= 0 )
        {
            CameraShakeEnd();
        }
    }

    private void CameraShakeEnd()
    {
        //_currentCameraShakeIntensity = _cameraShakeIntensityDefault;
        _cameraObject.transform.localPosition = Vector3.zero;
    }

    private void LateUpdate()
    {
        if (_leader == null)
        {
            return;
        }

        Vector3 current = transform.position;
        Vector3 moveToPos = new Vector3((_leader.transform.position.x + _offset.x + _additiveOffset.x), (_leader.transform.position.y + _offset.y + _additiveOffset.y), _offset.z);
        Vector2 distance = (Vector2)moveToPos - (Vector2)transform.position;
        transform.position = Vector3.MoveTowards(current, moveToPos, Util.MapValue(Mathf.Abs(distance.magnitude), 0, _maxDistance, _maxDistanceDelta.x, _maxDistanceDelta.y));

        if (Actor.i != null)
        {
            if (Actor.i.input.LSY < -0.75f && Actor.i.collision.Grounded && !Actor.i.throwing.throwing)
            {
                AddOffset(_offsetDown);
            }
            else
            {
                AddOffset(Vector2.zero);
            }
        }
    }

    public void AddOffset(Vector2 offset)
    {
        _additiveOffset = offset;
    }
}
