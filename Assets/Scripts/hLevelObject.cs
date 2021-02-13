using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class hLevelObject : MonoBehaviour
{
    [System.Serializable]
    public class SerialData 
    {
        public string type;
        public hSerializedTransform serializedTransform;
        public virtual hLevelObject DeSerialize(Transform parent = null) => OnDeSerialize<hLevelObject>(parent);

        protected T OnDeSerialize<T>(Transform parent = null) where T : hLevelObject
        {
            hLevelObject prefab = hDatabase.current.GetLevelObject(System.Type.GetType(type));
            hLevelObject levelObject = Instantiate(prefab, parent);

            serializedTransform.DeSerialize(levelObject.transform);

            return (T)levelObject;
        }
    }


    public SerialData Serialize()
    {
        string type = GetType().FullName;
        hSerializedTransform serializedTransform = new hSerializedTransform();
        serializedTransform.Serialize(transform);
        return OnSerialize(type, serializedTransform);
    }

    protected virtual SerialData OnSerialize(string type, hSerializedTransform serializedTransform)
    {
        SerialData serialData = new SerialData();
        serialData.type = type;
        serialData.serializedTransform = serializedTransform;
        return serialData;
    }
}
