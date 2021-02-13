using UnityEngine;
using UnityEngine.EventSystems;

public class hGameBall : hLevelObject
{
    #region Serialization
    [System.Serializable]
    public new class SerialData : hLevelObject.SerialData
    {
        public float[] rot;
        public float speed;
        public override hLevelObject DeSerialize(Transform parent)
        {
            var obj = OnDeSerialize<hGameBall>(parent);
            obj._rollComponent.center.transform.localRotation = Quaternion.Euler(new Vector3(rot[0], rot[1], rot[2]));
            obj._rollComponent.moveSpeed = speed;
            return obj;
        }
    }

    protected override hLevelObject.SerialData OnSerialize(string type, hSerializedTransform serializedTransform)
    {
        SerialData serialData = new SerialData();
        serialData.type = type;
        serialData.serializedTransform = serializedTransform;
        float[] rot = new float[] { 
            _rollComponent.center.transform.localEulerAngles.x,
            _rollComponent.center.transform.localEulerAngles.y,
            _rollComponent.center.transform.localEulerAngles.z 
        };
        serialData.rot = rot;
        serialData.speed = _rollComponent.moveSpeed;
        return serialData;
    }
    #endregion

    [SerializeField]
    private hRollComponent _rollComponent;
    private hAudioController _audioController;
    [SerializeField]
    private AudioClip _flyClip;

    private event DelVoid _deadEvent, _goalEvent;

    public event DelVoid deadEvent { add => _deadEvent += value; remove => _deadEvent -= value; }
    public event DelVoid goalEvent { add => _goalEvent += value; remove => _goalEvent -= value; }

    private static DelVoid _flyEvent, _landEvent;

    public static event DelVoid flyEvent { add => _flyEvent += value; remove => _flyEvent -= value; }
    public static event DelVoid landEvent { add => _landEvent += value; remove => _landEvent -= value; }

    private void Start()
    {
        _flyClip = hDatabase.current.ballFlyClip;
        _audioController = new hAudioController(gameObject);
        _rollComponent.flyEvent += _flyEvent;
        _rollComponent.landEvent += _landEvent;
    }

    private void OnDestroy()
    {
        _flyEvent = null;
        _landEvent = null;
    }

    private void GetInput()
    {
        if (hGameManager.current == null) return;
        if (hGameManager.current.isPause) return;
        if (EventSystem.current.IsPointerOverGameObject(0)) return;

#if UNITY_ANDROID
        if (!_rollComponent.isFly && Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            _rollComponent.isFly = true;
            _audioController.Play(_flyClip);
        }
#elif UNITY_STANDALONE_WIN
        if (!_rollComponent.isFly && Input.GetKeyDown(KeyCode.Space))
        {
            _rollComponent.isFly = true;
            _audioController.Play(_flyClip);
        }
#endif
#if UNITY_EDITOR
        if (!_rollComponent.isFly && Input.GetKeyDown(KeyCode.Space))
        {
            _rollComponent.isFly = true;
            _audioController.Play(_flyClip);
        }
#endif
    }

    private void Update()
    {
        GetInput();
    }

    public void Die()
    {
        _rollComponent.isStop = true;
        _deadEvent?.Invoke();
    }

    public void Goal()
    {
        _rollComponent.isStop = true;
        _goalEvent?.Invoke();
    }
}
