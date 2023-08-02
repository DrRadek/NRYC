using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerCounter : MonoBehaviour
{
    [System.NonSerialized] public bool isEmpty = true;

    int count = 0;

    public int Count { 
        get => count;
        set 
        {
            count = value;
            isEmpty = count == 0;
        } 
    }

    void OnTriggerEnter()
    {
        Count++;
    }
    void OnTriggerExit()
    {
        Count--;
    }
    private void OnDisable()
    {
        Count = 0;
    }
}
