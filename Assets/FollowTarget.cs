using System;
using UnityEngine;


[ExecuteInEditMode]
public class FollowTarget : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0f, 7.5f, 0f);
    public bool DestroyIfTargetMissing;

    Rigidbody rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void SetTarget(Transform t, bool destroyIfMissing = false, Vector3? offset = null)
    {
        target = t;
        DestroyIfTargetMissing = destroyIfMissing;
        if (offset != null) this.offset = offset.Value;
    }

    public void Update()
    {
#if UNITY_EDITOR
        if (target != null)
        {
            if (Application.isPlaying)
            {
                if (rb) rb.MovePosition(target.position + offset);
                else transform.position = target.position + offset;
            }
            else transform.position = target.position + offset;
        }
        else if (Application.isPlaying && DestroyIfTargetMissing) Destroy(gameObject);
#else
            if (target != null)
            {
                if (rb) rb.MovePosition(target.position + offset);
                else transform.position = target.position + offset;
            }
            else if(DestroyIfTargetMissing) Destroy(gameObject);
#endif
    }
}

