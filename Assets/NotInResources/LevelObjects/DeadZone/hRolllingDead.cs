using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hRolllingDead : hDeadZone
{
    #region Serialization
    [System.Serializable]
    public new class SerialData : hLevelObject.SerialData
    {
        public float[] rot;
        public float speed;
        public override hLevelObject DeSerialize(Transform parent)
        {
            var obj = OnDeSerialize<hRolllingDead>(parent);
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
}
