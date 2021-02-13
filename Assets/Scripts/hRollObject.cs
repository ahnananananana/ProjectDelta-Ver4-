using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class hRollObject : MonoBehaviour
{
    [SerializeField]
    protected Transform _mesh;
    [SerializeField]
    protected float _rotSpeed;
    [SerializeField]
    protected float _moveSpeed, _groundDistance;
    protected Vector3 _prePosition;
    [SerializeField]
    protected Transform _center, _centerRayPoint, _frontRayPoint, _backRayPoint;
    [SerializeField]
    protected LayerMask _groundLayer;
    [SerializeField]
    protected float _rayDis;
    protected bool _isStop, _isFly;
    protected Transform _groundOn;
    protected Rigidbody _rigidbody;

    public Transform center => _center;

    protected static DelVoid _flyEvent, _landEvent;
    public static event DelVoid flyEvent { add => _flyEvent += value; remove => _flyEvent -= value; }
    public static event DelVoid landEvent { add => _landEvent += value; remove => _landEvent -= value; }
    public bool isFly { get => _isFly; set => _isFly = value; }

    protected virtual void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnDestroy()
    {
        _flyEvent = null;
        _landEvent = null;
    }

    public void TransfromObject(Vector3 inPosition, Quaternion inRotation)
    {
        transform.SetParent(null);
        transform.position = inPosition;
        _center.transform.rotation = inRotation;
        _rigidbody.position = inPosition;
    }

    protected void MoveForward()
    {
        if (_isFly) return;

        _prePosition = _rigidbody.position;
        //transform.Translate(_center.forward * _moveSpeed * Time.fixedDeltaTime);
        _rigidbody.position += _center.forward * _moveSpeed * hTime.fixedDeltaTime;
        transform.position = _rigidbody.position;

        Ray bottomRay = new Ray(_centerRayPoint.position, -_centerRayPoint.up);
        Ray forwardRay = new Ray(_centerRayPoint.position, _centerRayPoint.forward);
        Ray frontRay = new Ray(_frontRayPoint.position, -_frontRayPoint.up);
        Ray backRay = new Ray(_backRayPoint.position, -_backRayPoint.up);

        if (Physics.Raycast(bottomRay, out var groundHit, _rayDis, _groundLayer))
            if (_groundOn != groundHit.transform)
                transform.SetParent(groundHit.transform);

        if (Physics.Raycast(forwardRay, _rayDis, _groundLayer))
        {
            //transform.Rotate(new Vector3(-90f, 0, 0), Space.World);
            _center.transform.Rotate(-90f, 0f, 0f);
            forwardRay = new Ray(_centerRayPoint.position, -_centerRayPoint.up);
            Physics.Raycast(forwardRay, out var hit, _rayDis, _groundLayer);
            _rigidbody.position = hit.point + (-forwardRay.direction * _groundDistance);
            transform.position = _rigidbody.position;
            return;
        }

        if (Physics.Raycast(frontRay, out var frontHit, _rayDis, _groundLayer))
        {
            _rigidbody.position = frontHit.point + (-frontRay.direction * _groundDistance);
            transform.position = _rigidbody.position;
            _rigidbody.position += _rigidbody.position - _frontRayPoint.position;
            transform.position = _rigidbody.position;
        }
        else if (Physics.Raycast(backRay, out var backHit, _rayDis, _groundLayer))
        {
            _rigidbody.position = backHit.point + (-backRay.direction * _groundDistance);
            transform.position = _rigidbody.position;
            _rigidbody.position += _rigidbody.position - _backRayPoint.position;
            transform.position = _rigidbody.position;
        }
        else
        {
            _rigidbody.position = _prePosition;
            transform.position = _rigidbody.position;
            //transform.Rotate(90f, 0f, 0f, Space.World);
            _center.transform.Rotate(90f, 0f, 0f);

            frontRay = new Ray(_frontRayPoint.position, -_frontRayPoint.up);
            Physics.Raycast(frontRay, out var hit, _rayDis, _groundLayer);
            _rigidbody.position = hit.point + (-frontRay.direction * _groundDistance);
            transform.position = _rigidbody.position;
            _rigidbody.position += _rigidbody.position - _frontRayPoint.position;
            transform.position = _rigidbody.position;

            _rigidbody.position += _center.forward * _moveSpeed * hTime.fixedDeltaTime;
            transform.position = _rigidbody.position;
        }

        Debug.DrawRay(_centerRayPoint.position, _centerRayPoint.forward, Color.red);
        Debug.DrawRay(_frontRayPoint.position, -_frontRayPoint.up, Color.red);
        Debug.DrawRay(_backRayPoint.position, -_backRayPoint.up, Color.red);
    }

    protected void Fly()
    {
        if (!_isFly) return;

        _prePosition = _rigidbody.position;
        //transform.Translate(_center.up * _moveSpeed * Time.fixedDeltaTime);
        _rigidbody.position += _center.up * _moveSpeed * hTime.fixedDeltaTime;
        transform.position = _rigidbody.position;

        Ray backRay = new Ray(_backRayPoint.position, _backRayPoint.up);
        Ray frontRay = new Ray(_frontRayPoint.position, _frontRayPoint.up);

        bool isBack = Physics.Raycast(backRay, out var backHit, _groundDistance, _groundLayer);
        bool isFront = Physics.Raycast(frontRay, out var frontHit, _groundDistance, _groundLayer);

        if (isBack || isFront)
        {
            _center.transform.Rotate(0, 180f, 180f);

            if (isBack) transform.SetParent(backHit.transform);
            else transform.SetParent(frontHit.transform);

            _isFly = false;
            _landEvent?.Invoke();
        }

        Debug.DrawRay(_frontRayPoint.position, -_frontRayPoint.up, Color.red);
        Debug.DrawRay(_backRayPoint.position, -_backRayPoint.up, Color.red);
    }

    protected abstract void FixedUpdate();
}
