using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hWayPoint : hLevelObject
{
    #region Serialization
    [System.Serializable]
    public new class SerialData : hLevelObject.SerialData
    {
        public int id;
        public bool isCollider;

        public override hLevelObject DeSerialize(Transform parent)
        {
            var obj = OnDeSerialize<hWayPoint>(parent);
            obj._id = id;
            if (!isCollider)
                DestroyImmediate(obj.GetComponent<Collider>());
            return obj;
        }
    }
    protected override hLevelObject.SerialData OnSerialize(string type, hSerializedTransform serializedTransform)
    {
        SerialData serialData = new SerialData();
        serialData.type = type;
        serialData.serializedTransform = serializedTransform;
        serialData.id = id;
        if (GetComponent<Collider>() == null)
            serialData.isCollider = false;
        else
            serialData.isCollider = true;
        return serialData;
    }
    #endregion

    private static int s_totalNum = 0;
    private int _id = -1;

    public static int totalNum { get => s_totalNum; set => s_totalNum = value; }

    public int id
    {
        get
        {
            if(!Application.isPlaying && _id < 0)
                _id = s_totalNum++;

            return _id;
        }
    }

    private void Awake()
    {
        hLevel.current.AddWayPoint(this);
    }
}
