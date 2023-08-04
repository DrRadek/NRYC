using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CatSpin : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 1.0f;

    Controls controls;
    Controls.PlayerActions playerControls;

    bool rotatingRight = false;
    bool rotatingLeft = false;

    private void Awake()
    {
        controls = new Controls();
        playerControls = controls.Player;
    }

    void Update()
    {
        rotationSpeed += (playerControls.ChangeCatSpeed.ReadValue<float>() + (rotatingRight ? 1 : 0) - (rotatingLeft ? 1 : 0))
            * Time.deltaTime * 50 * (Mathf.Abs(rotationSpeed) * 0.01f + 1);

        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }

    public void StartRotatingRight()
    {
        rotatingRight = true;
    }

    public void StopRotatingRight()
    {
        rotatingRight = false;
    }

    public void StartRotatingLeft()
    {
        rotatingLeft = true;
    }

    public void StopRotatingLeft()
    {
        rotatingLeft = false;
    }

    private void OnEnable() { controls.Enable(); }
    private void OnDisable() { controls.Disable(); }
}
