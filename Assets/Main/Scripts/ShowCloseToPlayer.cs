using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShowCloseToPlayer : MonoBehaviour
{
    TextMeshProUGUI text;

    int sqrDistance = 30;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        Vector3 direction = GameManager.instance.player.transform.position - transform.position;

        if (direction.sqrMagnitude < sqrDistance)
            text.enabled = true;
        else
            text.enabled = false;

    }
}
