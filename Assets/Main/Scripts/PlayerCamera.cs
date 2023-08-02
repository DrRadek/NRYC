using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] public Transform targetTransform;

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, targetTransform.rotation, Time.deltaTime * 5.0f);
        transform.position = Vector3.Slerp(transform.position, targetTransform.position, Time.deltaTime * 5.0f);
    }
}
