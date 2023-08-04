using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionKdjdToTaskbar : MonoBehaviour
{
    [SerializeField] Transform targetTransform;
    [SerializeField] Transform kdjdTransform;
    [SerializeField] Transform parent;
    [SerializeField] Transform target;

    private void Start()
    {
        enabled = false;
    }

    public void StartTransition()
    {
        StartCoroutine(Transition());
    }

    private void Update()
    {
        kdjdTransform.rotation = Quaternion.Slerp(kdjdTransform.rotation, targetTransform.rotation, Time.deltaTime * 4.0f);
        kdjdTransform.position = Vector3.Slerp(kdjdTransform.position, targetTransform.position, Time.deltaTime * 4.0f);
        kdjdTransform.localScale = Vector3.Slerp(kdjdTransform.localScale, targetTransform.localScale, Time.deltaTime * 4.0f);
    }

    IEnumerator Transition()
    {
        enabled = true;
        
        yield return new WaitForSeconds(1.5f);
        enabled = false;
        kdjdTransform.SetPositionAndRotation(targetTransform.position,targetTransform.rotation);
        kdjdTransform.localScale = targetTransform.localScale;
        parent.SetParent(target);

    }
}
