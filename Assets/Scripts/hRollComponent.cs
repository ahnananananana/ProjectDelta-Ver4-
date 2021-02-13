using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hRollComponent : MonoBehaviour
{
    [SerializeField]
    private Transform _attachedTransform, _mesh, _center, _centerRayPoint, _frontRayPoint, _backRayPoint;
    [SerializeField]
    private float _rotSpeed, _moveSpeed, _groundDistance, _rayDis;
    [SerializeField]
    private LayerMask _groundLayer;
    private Vector3 _prePosition;
    private bool _isStop, _isFly;
    private Transform _groundOn;
    [SerializeField]
    private Rigidbody _rigidbody;
    private Transform _air;

    public Transform center => _center;

    private DelVoid _flyEvent, _landEvent;

    public event DelVoid flyEvent { add => _flyEvent += value; remove => _flyEvent -= value; }
    public event DelVoid landEvent { add => _landEvent += value; remove => _landEvent -= value; }

    public bool isFly
    {
        get => _isFly;
        set
        {
            _isFly = value;
            if (_isFly)
                _flyEvent?.Invoke();
            else
                _landEvent?.Invoke();
        }
    }
    public bool isStop { get => _isStop; set => _isStop = value; }
    public float moveSpeed { get => _moveSpeed; set => _moveSpeed = value; }

    private void Start()
    {
        if(hLevel.current != null)
            _air = hLevel.current.transform;
    }

    private void FixedUpdate()
    {
        if (_isStop) return;
        _mesh.Rotate(_moveSpeed * _rotSpeed * hTime.fixedDeltaTime, 0, 0);
        Fly();
        MoveForward();
    }

    public void TransfromObject(Vector3 inPosition, Quaternion inRotation)
    {
        _attachedTransform.SetParent(_air);
        _attachedTransform.position = inPosition;
        _center.transform.rotation = inRotation;
        _rigidbody.position = inPosition;
    }

    public void MoveForward()
    {
        if (_isFly) return;

        _prePosition = _rigidbody.position;
        //transform.Translate(_center.forward * _moveSpeed * Time.fixedDeltaTime);
        _rigidbody.position += _center.forward * _moveSpeed * hTime.fixedDeltaTime;
        _attachedTransform.position = _rigidbody.position;

        Ray bottomRay = new Ray(_centerRayPoint.position, -_centerRayPoint.up);
        Ray forwardRay = new Ray(_centerRayPoint.position, _centerRayPoint.forward);
        Ray frontRay = new Ray(_frontRayPoint.position, -_frontRayPoint.up);
        Ray backRay = new Ray(_backRayPoint.position, -_backRayPoint.up);

        if (Physics.Raycast(bottomRay, out var groundHit, _rayDis, _groundLayer))
            if (_groundOn != groundHit.transform)
                _attachedTransform.SetParent(groundHit.transform);

        if (Physics.Raycast(forwardRay, _rayDis, _groundLayer))
        {
            //transform.Rotate(new Vector3(-90f, 0, 0), Space.World);
            _center.transform.Rotate(-90f, 0f, 0f);
            forwardRay = new Ray(_centerRayPoint.position, -_centerRayPoint.up);
            Physics.Raycast(forwardRay, out var hit, _rayDis, _groundLayer);
            _rigidbody.position = hit.point + (-forwardRay.direction * _groundDistance);
            _attachedTransform.position = _rigidbody.position;
            return;
        }

        if (Physics.Raycast(frontRay, out var frontHit, _rayDis, _groundLayer))
        {
            _rigidbody.position = frontHit.point + (-frontRay.direction * _groundDistance);
            _attachedTransform.position = _rigidbody.position;
            _rigidbody.position += _rigidbody.position - _frontRayPoint.position;
            _attachedTransform.position = _rigidbody.position;
        }
        else if(Physics.Raycast(bottomRay, out var bottomHit, _rayDis, _groundLayer))
        {
            _rigidbody.position = bottomHit.point + (-bottomRay.direction * _groundDistance);
            _attachedTransform.position = _rigidbody.position;
            _rigidbody.position += _rigidbody.position - _centerRayPoint.position;
            _attachedTransform.position = _rigidbody.position;
        }
        else if (Physics.Raycast(backRay, out var backHit, _rayDis, _groundLayer))
        {
            _rigidbody.position = backHit.point + (-backRay.direction * _groundDistance);
            _attachedTransform.position = _rigidbody.position;
            _rigidbody.position += _rigidbody.position - _backRayPoint.position;
            _attachedTransform.position = _rigidbody.position;
        }
        else
        {
            _rigidbody.position = _prePosition;
            _attachedTransform.position = _rigidbody.position;
            //transform.Rotate(90f, 0f, 0f, Space.World);
            _center.transform.Rotate(90f, 0f, 0f);

            frontRay = new Ray(_frontRayPoint.position, -_frontRayPoint.up);
            Physics.Raycast(frontRay, out var hit, _rayDis, _groundLayer);
            _rigidbody.position = hit.point + (-frontRay.direction * _groundDistance);
            _attachedTransform.position = _rigidbody.position;
            _rigidbody.position += _rigidbody.position - _frontRayPoint.position;
            _attachedTransform.position = _rigidbody.position;

            _rigidbody.position += _center.forward * _moveSpeed * hTime.fixedDeltaTime;
            _attachedTransform.position = _rigidbody.position;
        }

        Debug.DrawRay(_centerRayPoint.position, _centerRayPoint.forward, Color.red);
        Debug.DrawRay(_frontRayPoint.position, -_frontRayPoint.up, Color.red);
        Debug.DrawRay(_backRayPoint.position, -_backRayPoint.up, Color.red);
    }

    public void Fly()
    {
        if (!_isFly) return;
        if (transform.parent != _air)
            transform.SetParent(_air);

        _prePosition = _rigidbody.position;
        //transform.Translate(_center.up * _moveSpeed * Time.fixedDeltaTime);
        _rigidbody.position += _center.up * _moveSpeed * hTime.fixedDeltaTime;
        _attachedTransform.position = _rigidbody.position;

        Ray backRay = new Ray(_backRayPoint.position, _backRayPoint.up);
        Ray frontRay = new Ray(_frontRayPoint.position, _frontRayPoint.up);

        bool isBack = Physics.Raycast(backRay, out var backHit, _groundDistance, _groundLayer);
        bool isFront = Physics.Raycast(frontRay, out var frontHit, _groundDistance, _groundLayer);

        if (isBack || isFront)
        {
            _center.transform.Rotate(0, 180f, 180f);

            if (isBack) _attachedTransform.SetParent(backHit.transform);
            else _attachedTransform.SetParent(frontHit.transform);

            _isFly = false;
            _landEvent?.Invoke();
        }

        Debug.DrawRay(_frontRayPoint.position, -_frontRayPoint.up, Color.red);
        Debug.DrawRay(_backRayPoint.position, -_backRayPoint.up, Color.red);
    }
}
