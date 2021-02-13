using UnityEngine;

[System.Serializable]
public struct hSerializedTransform
{
    public float[] pos, rot, scale;

    public void Serialize(Transform transform)
    {
        pos = new float[3];
        rot = new float[3];
        scale = new float[3];

        pos[0] = transform.localPosition.x;
        pos[1] = transform.localPosition.y;
        pos[2] = transform.localPosition.z;

        rot[0] = transform.localRotation.eulerAngles.x;
        rot[1] = transform.localRotation.eulerAngles.y;
        rot[2] = transform.localRotation.eulerAngles.z;

        scale[0] = transform.localScale.x;
        scale[1] = transform.localScale.y;
        scale[2] = transform.localScale.z;
    }

    public void DeSerialize(Transform transform)
    {
        Vector3 pos = new Vector3(this.pos[0], this.pos[1], this.pos[2]);
        Vector3 rot = new Vector3(this.rot[0], this.rot[1], this.rot[2]);
        Vector3 scale = new Vector3(this.scale[0], this.scale[1], this.scale[2]);

        transform.localPosition = pos;
        transform.localRotation = Quaternion.Euler(rot);
        transform.localScale = scale;

    }
}
