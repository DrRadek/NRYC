using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MonitorTime : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI time;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(TimeUpdater());
    }

    IEnumerator TimeUpdater()
    {
        while (true)
        {
            time.text = DateTime.Now.ToString("HH:mm");
            yield return new WaitForSeconds(5f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
