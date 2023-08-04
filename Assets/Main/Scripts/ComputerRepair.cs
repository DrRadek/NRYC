using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class ComputerRepair : MonoBehaviour
{
    [SerializeField] ShowCloseToPlayer computerCloseToPlayer;
    [SerializeField] GameObject computerPos;
    [SerializeField] List<Rigidbody> computerParts = new();

    Vector3 targetPos = Vector3.zero;

    Controls controls;
    Controls.PlayerActions playerControls;

    bool repairInProgress = false;

    private void Awake()
    {
        controls = new Controls();
        playerControls = controls.Player;

        playerControls.Repair.performed += OnRepairStart;
    }

    private void FixedUpdate()
    {
        if (repairInProgress)
        {
            transform.position = Vector3.Slerp(transform.position, targetPos, Time.deltaTime * 6.0f);
            foreach (var part in computerParts)
            {
                part.transform.localRotation = Quaternion.Slerp(part.transform.localRotation, Quaternion.identity, Time.deltaTime * 6.0f);
                part.transform.localPosition = Vector3.Slerp(part.transform.localPosition, Vector3.zero, Time.deltaTime * 6.0f);
                part.velocity *= 0.7f;
                part.angularVelocity *= 0.7f;
            }
        }

        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, -37, 37);
        pos.z = Mathf.Clamp(pos.z, -37, 37);
        if (pos.y < -0.1f)
            pos.y = 2;
        transform.position = pos;
        if (transform.position != pos)
        {
            targetPos = pos;
            StartRepair();

        }

    }

    private void StartRepair()
    {
        if (!computerCloseToPlayer.IsVisible || repairInProgress)
            return;

        repairInProgress = true;
        targetPos = computerPos.transform.position;
        targetPos.y = 0;
        StartCoroutine(WaitForRepair());
    }


    private void OnRepairStart(InputAction.CallbackContext callback)
    {
        StartRepair();
    }

    private void OnRepairEnd()
    {
        repairInProgress = false;

        transform.position = targetPos;

        foreach (var part in computerParts)
        {
            part.transform.localRotation = Quaternion.identity;
            part.transform.localPosition = Vector3.zero;
            part.velocity = Vector3.zero;
            part.angularVelocity = Vector3.zero;
        }

    }

    private IEnumerator WaitForRepair()
    {
        yield return new WaitForSeconds(0.8f);
        OnRepairEnd();
    }

    private void OnEnable() { controls.Enable(); }
    private void OnDisable() { controls.Disable(); }
}
