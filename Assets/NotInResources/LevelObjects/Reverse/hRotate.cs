using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hRotate : hEffectObject
{
    [SerializeField]
    private Vector3 _angleVector;
    [SerializeField]
    private bool _isPermanent;

    public override void DoEffect(GameObject inTarget)
    {
        var myCollider = GetComponent<Collider>();
        var targetCollider = inTarget.GetComponent<Collider>();

        var between = Vector3.Distance(myCollider.bounds.center, targetCollider.bounds.center);
        var radiusSum = myCollider.bounds.extents.z + targetCollider.bounds.extents.z;
        var sub = radiusSum - between;
        var offset = sub + .05f;

        inTarget.transform.position -= inTarget.transform.forward * offset;

        inTarget.GetComponent<hRollComponent>().center.transform.Rotate(_angleVector);

        if(!_isPermanent)
            gameObject.SetActive(false);
    }
}
