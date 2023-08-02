using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    [SerializeField] GameObject objectToFollow;
    private void FixedUpdate()
    {
        transform.position = objectToFollow.transform.position;
    }
}
