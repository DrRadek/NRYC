using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyLater : MonoBehaviour
{
    [SerializeField] bool startAtStart = false;
    [SerializeField] float time = 4.0f;

    private void Start()
    {
        if (startAtStart)
            StartDestruction(time);
    }

    public void StartDestruction(float time)
    {
        StartCoroutine(DestryLater(time));
    }

    IEnumerator DestryLater(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }

}
