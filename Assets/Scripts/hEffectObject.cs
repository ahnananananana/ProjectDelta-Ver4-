using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void DelEffect(hEffectObject inEffect);

public abstract class hEffectObject : hLevelObject
{
    protected Collider _collider;
    protected hAudioController _audioController;
    [SerializeField]
    protected AudioClip _sfxClip;

    [SerializeField]
    protected Vector3 _rotVec;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
        _collider.isTrigger = true;
        gameObject.layer = LayerMask.NameToLayer("Effect");
        OnAwake();
    }

    protected virtual void OnAwake() { }

    private void Start()
    {
        #region Binary serial data
       /* Ray ray = new Ray(transform.position, transform.up);
        if(Physics.Raycast(ray, out var hit, 1f))
        {
            if(hit.transform.GetComponent<hMovingGround>() != null)
                transform.SetParent(hit.transform);
        }

        ray.direction = -transform.up;
        if (Physics.Raycast(ray, out hit, 1f))
        {
            if (hit.transform.GetComponent<hMovingGround>() != null)
                transform.SetParent(hit.transform);
        }

        ray.direction = transform.forward;
        if (Physics.Raycast(ray, out hit, 1f))
        {
            if (hit.transform.GetComponent<hMovingGround>() != null)
                transform.SetParent(hit.transform);
        }

        ray.direction = -transform.forward;
        if (Physics.Raycast(ray, out hit, 1f))
        {
            if (hit.transform.GetComponent<hMovingGround>() != null)
                transform.SetParent(hit.transform);
        }*/
        #endregion

        OnStart();
    }

    protected virtual void OnStart() { }

    protected virtual void FixedUpdate()
    {
        transform.Rotate(_rotVec * hTime.fixedDeltaTime);
    }

    public abstract void DoEffect(GameObject inTarget);

    private void OnTriggerEnter(Collider other) => DoEffect(other.gameObject);
}
