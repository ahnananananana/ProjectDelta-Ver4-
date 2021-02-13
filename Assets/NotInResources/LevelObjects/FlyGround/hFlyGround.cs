using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hFlyGround : hGround
{
    [SerializeField]
    private Material _normalMat, _translucentMat;
    private MeshRenderer _meshRenderer;

    protected override void OnAwake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _meshRenderer.sharedMaterial = _normalMat;
        hGameBall.flyEvent += () => { SetActive(false); };
        hGameBall.landEvent += () => { SetActive(true); };
    }

    private void SetActive(bool set)
    {
        GetComponent<Collider>().enabled = set;
        if(set) _meshRenderer.sharedMaterial = _normalMat;
        else _meshRenderer.sharedMaterial = _translucentMat;
    }
}
