using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hMovingGround : hGround
{
    #region Serialization
    [System.Serializable]
    public new class SerialData : hLevelObject.SerialData
    {
        //public hWayPoint.SerialData[] wayPoints;
        public int[] wayPoints;
        public bool isLoop;
        public float speed;

        public override hLevelObject DeSerialize(Transform parent)
        {
            var obj = OnDeSerialize<hMovingGround>(parent);
            obj._isLoop = isLoop;

            obj._wayPointsId = new int[wayPoints.Length];
            for (int i = 0; i < wayPoints.Length; ++i)
                obj._wayPointsId[i] = wayPoints[i];

            obj._speed = speed;

            return obj;
        }
    }

    protected override hLevelObject.SerialData OnSerialize(string type, hSerializedTransform serializedTransform)
    {
        SerialData serialData = new SerialData();
        serialData.type = type;
        serialData.serializedTransform = serializedTransform;
        serialData.wayPoints = new int[_wayPoints.Length];
        for (int i = 0; i < _wayPoints.Length; ++i)
            serialData.wayPoints[i] = _wayPoints[i].id;
        serialData.isLoop = _isLoop;
        serialData.speed = _speed;
        return serialData;
    }
    #endregion

    [SerializeField]
    public bool _isLoop;
    [SerializeField]
    private float _speed;
    [SerializeField]
    private hWayPoint[] _wayPoints;
    private int[] _wayPointsId;
    [SerializeField]
    private int _startIndex;
    private int _curWayIndex;
    private bool _isIncrease = true;
    private Rigidbody _rigidbody;

    protected override void OnAwake()
    {
        /*_wayPointsId = new int[_wayPoints.Length];
        for (int i = 0; i < _wayPoints.Length; ++i) 
            _wayPointsId[i] = _wayPoints[i].id;*/

        _rigidbody = GetComponent<Rigidbody>();
        if (_rigidbody == null)
            _rigidbody = gameObject.AddComponent<Rigidbody>();

        _curWayIndex = _startIndex;
    }

    private void FixedUpdate()
    {
        //var des = hLevel.current.GetWayPointPos(_wayPointsId[_curWayIndex]);
        var des = _wayPoints[_curWayIndex].transform.position;
        if (Vector3.Distance(_rigidbody.position, des) > .1f)
        {
            _rigidbody.position = Vector3.MoveTowards(_rigidbody.position, des, _speed * hTime.fixedDeltaTime);
            transform.position = _rigidbody.position;
            return;
        }

        _rigidbody.position = des;
        transform.position = _rigidbody.position;

        if (_isLoop)
        {
            _curWayIndex = ++_curWayIndex % _wayPoints.Length;
        }
        else
        {
            if (_isIncrease) ++_curWayIndex;
            else --_curWayIndex;

            if (_curWayIndex == 0 || _curWayIndex == _wayPoints.Length - 1)
                _isIncrease = !_isIncrease;
        }
    }
}
