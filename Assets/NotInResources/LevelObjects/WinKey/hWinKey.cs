using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum KeyType
{ 
    GREEN,
    YELLOW,
}

public class hWinKey : hEffectObject
{
    #region Serialization
    [System.Serializable]
    public new class SerialData : hLevelObject.SerialData
    {
        public int keyType;

        public override hLevelObject DeSerialize(Transform parent)
        {
            var obj = OnDeSerialize<hWinKey>(parent);
            obj._keyType = (KeyType)keyType;
            switch (obj._keyType)
            {
                case KeyType.GREEN:
                    obj._meshRenderer.sharedMaterial = greenMat;
                    break;
                case KeyType.YELLOW:
                    obj._meshRenderer.sharedMaterial = yellowMat;
                    break;
            }
            return obj;
        }
    }

    protected override hLevelObject.SerialData OnSerialize(string type, hSerializedTransform serializedTransform)
    {
        OnAwake();
        SerialData serialData = new SerialData();
        serialData.type = type;
        serialData.serializedTransform = serializedTransform;
        serialData.keyType = (int)_keyType;
        return serialData;
    }
    #endregion
    private static Material greenMat, yellowMat;
    private KeyType _keyType;
    [SerializeField]
    private MeshRenderer _meshRenderer;

    public KeyType keyType => _keyType;

    private event DelVoid _getEvent;
    public event DelVoid getEvent { add => _getEvent += value;remove => _getEvent -= value; }

    protected override void OnAwake()
    {
        if(Application.isPlaying)
            hLevel.current.AddWinKey(this);
    }

    public override void DoEffect(GameObject inTarget)
    {
        if (inTarget.CompareTag("Player"))
        {
            _getEvent?.Invoke();
            gameObject.SetActive(false);
        }
    }
}
