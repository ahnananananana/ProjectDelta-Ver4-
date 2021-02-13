using UnityEngine;

public class hGate : hGround
{
    #region Serialization
    [System.Serializable]
    public new class SerialData : hLevelObject.SerialData
    {
        public int keyType;

        public override hLevelObject DeSerialize(Transform parent)
        {
            var obj = OnDeSerialize<hGate>(parent);
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
    [SerializeField]
    private hWinKey[] _keys;
    private int _leftKeyNum;

    protected override void OnAwake()
    {
       /* if(greenMat == null)
            greenMat = Resources.Load("GameObjects/LevelObjects/Gate/Gate_Green") as Material;
        if (yellowMat == null)
            yellowMat = Resources.Load("GameObjects/LevelObjects/Gate/Gate_Yellow") as Material;
        if (_meshRenderer.sharedMaterial == greenMat)
            _keyType = KeyType.GREEN;
        else if (_meshRenderer.sharedMaterial == yellowMat)
            _keyType = KeyType.YELLOW;*/
    }

    private void Start()
    {
        var winKeys = hLevel.current.winKeyList;
        _leftKeyNum = 0;
        for (int i = 0; i < _keys.Length; ++i)
        {
            ++_leftKeyNum;
            _keys[i].getEvent += () => { --_leftKeyNum; if (_leftKeyNum == 0) Open(); };
        } 
        /*for (int i = 0; i < winKeys.Count; ++i)
        {
            if(winKeys[i].keyType == _keyType)
            {
                ++_leftKeyNum;
                winKeys[i].getEvent += () => { --_leftKeyNum; if (_leftKeyNum == 0) Open(); };
            }
        }*/
    }

    private void Open()
    {
        gameObject.SetActive(false);
    }
}
