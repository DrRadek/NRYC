using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShowCloseToPlayer : MonoBehaviour
{
    TextMeshProUGUI text;

    int sqrDistance = 40;

    private bool isVisible = false;

    public bool IsVisible { get => isVisible; private set => isVisible = value; }

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        Vector3 direction = GameManager.instance.player.transform.position - transform.position;

        IsVisible = (direction.sqrMagnitude < sqrDistance) ? true : false;

        text.enabled = IsVisible;
    }
}
