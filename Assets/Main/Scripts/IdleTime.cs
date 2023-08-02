using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ValueChecks;

public class IdleTime : MonoBehaviour
{
    [SerializeField] AnimatedProgresBar bar;
    [SerializeField] TriggerCounter trigger;
    [SerializeField] Canvas canvas;

    public float fillSpeed;

    // Update is called once per frame
    void Update()
    {
        var oldValue = bar.GetValue(0);

        if (trigger.isEmpty)
        {
            canvas.enabled = false;
            oldValue = 0;
        }
        else
        {
            canvas.enabled = true;
            oldValue += fillSpeed * trigger.Count * Time.deltaTime;
        }
        
        if (bar.SetValue(0, oldValue) == ValueChangeResult.FULL)
            Debug.Log("Game over");

    }
}
