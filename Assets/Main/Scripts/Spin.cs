using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Spin : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 1.0f;

    Controls controls;
    Controls.PlayerActions playerControls;

    private void Awake()
    {
        controls = new Controls();
        playerControls = controls.Player;
    }

    void Update()
    {
        rotationSpeed += playerControls.ChangeCatSpeed.ReadValue<float>() * Time.deltaTime * 50 * (Mathf.Abs(rotationSpeed) * 0.01f + 1);

        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }

    private void OnEnable() { controls.Enable(); }
    private void OnDisable() { controls.Disable(); }
}
