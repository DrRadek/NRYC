using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIHide : MonoBehaviour
{
    [SerializeField] ChairManager chair;
    [SerializeField] GameObject overlay;
    [SerializeField] GameObject infoText;

    Controls controls;
    Controls.PlayerActions playerControls;

    private bool isUiHidden = false;

    public bool IsUiHidden { get => isUiHidden; private set => isUiHidden = value; }

    private void Awake()
    {
        controls = new Controls();
        playerControls = controls.Player;
        playerControls.HideUI.performed += HideUI;
    }

    private void HideUI(InputAction.CallbackContext context)
    {
        if (!chair.IsOnChair)
        {
            IsUiHidden = !IsUiHidden;
            UpdateState();
        }
    }

    public void UpdateState()
    {
        overlay.SetActive(!IsUiHidden);
        infoText.SetActive(!IsUiHidden);
    }

    private void OnEnable() { controls.Enable(); }
    private void OnDisable() { controls.Disable(); }
}
