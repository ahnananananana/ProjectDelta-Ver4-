using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hPortal : hEffectObject
{
    #region Serialization
    [System.Serializable]
    public new class SerialData : hLevelObject.SerialData
    {
        public int id, nextPortalId, matId;

        public override hLevelObject DeSerialize(Transform parent)
        {
            var obj = OnDeSerialize<hPortal>(parent);
            obj._id = id;
            obj._nextPortalId = nextPortalId;
            obj._meshRenderer.sharedMaterial = obj._materials[matId];
            return obj;
        }
    }

    protected override hLevelObject.SerialData OnSerialize(string type, hSerializedTransform serializedTransform)
    {
        SerialData serialData = new SerialData();
        serialData.type = type;
        serialData.serializedTransform = serializedTransform;
        serialData.id = _id;
        serialData.nextPortalId = (_nextPortal != null) ? _nextPortal._id : -1;
        serialData.matId = System.Array.FindIndex(_materials, (mat) => _meshRenderer.sharedMaterial == mat);

        return serialData;
    }
    #endregion
    private static int s_totalNum = 0;
    [SerializeField]
    private Material[] _materials;
    [SerializeField]
    private MeshRenderer _meshRenderer;
    [SerializeField]
    private hPortal _nextPortal;
    private int _id = -1, _nextPortalId;
    private List<hRollComponent> _goOutList;
    [SerializeField]
    private bool _isFly;

    public int id
    {
        get
        {
            if(!Application.isPlaying && _id < 0)
            {
                _id = s_totalNum++;
            }
            return _id;
        }
        set => _id = value; 
    }
    public int nextPortalId { get => _nextPortalId; set => _nextPortalId = value; }
    public static int totalNum { get => s_totalNum; set => s_totalNum = value; }
    public hPortal nextPortal { get => _nextPortal; set => _nextPortal = value; }

    //public Color _color;

    protected override void OnAwake()
    {
        if (_id < 0)
            _id = s_totalNum++;
        _goOutList = new List<hRollComponent>();
        //SetColor(_color);
    }

    protected override void FixedUpdate()
    {
        _meshRenderer.transform.Rotate(_rotVec * hTime.fixedDeltaTime);
    }

    public void SetColor(Color inColor)
    {
        var mpb = new MaterialPropertyBlock();
        mpb.SetColor(Shader.PropertyToID("_Color"), inColor);
        GetComponent<MeshRenderer>().SetPropertyBlock(mpb);
        //GetComponent<MeshRenderer>().materials[0].SetColor("_Color", inColor);
    }

    public override void DoEffect(GameObject inTarget)
    {
        if (_nextPortal == null) return;
        var rollObject = inTarget.GetComponent<hRollComponent>();
        if (rollObject == null || _goOutList.Contains(rollObject)) return;
        rollObject.TransfromObject(_nextPortal.transform.position, _nextPortal.transform.rotation);

        if(_nextPortal._isFly)
        {
            rollObject.isFly = true;
            rollObject.center.transform.Rotate(90f, 0, 0);
        }
        else
        {
            rollObject.isFly = false;
        }

        _nextPortal._goOutList.Add(rollObject);
    }

    private void OnTriggerExit(Collider other)
    {
        var rollObject = other.GetComponent<hRollComponent>();
        if (rollObject == null || !_goOutList.Contains(rollObject)) return;
        _goOutList.Remove(rollObject);
    }
}
