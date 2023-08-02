using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.InputSystem;


public class ChairManager : MonoBehaviour
{
    [SerializeField] Shotgun shotgun;
    [SerializeField] Collider shotgunCollider;
    [SerializeField] Collider playerCollider;
    [SerializeField] PlayerController playerController;
    [SerializeField] Transform chairPlace;
    [SerializeField] GameObject UI;
    [SerializeField] GameObject infoText;
    [SerializeField] Animator animator;
    [SerializeField] Transform playerCameraChairTrans;
    [SerializeField] Transform playerCameraMidTrans;
    [SerializeField] Transform playerCameraMapTrans;
    [SerializeField] MonitorScreen monitorScreen;


    [SerializeField] PlayerCamera playerCamera;

    Coroutine cameraCoroutine;


    Rigidbody playerRb;

    Controls controls;
    Controls.PlayerActions playerControls;

    bool isOnChair = false;

    private void Awake()
    {
        controls = new Controls();
        playerControls = controls.Player;

        playerControls.InteractWithChair.performed += UseChair;

    }

    private void Start()
    {
        playerRb = playerController.GetComponent<Rigidbody>();
        //TransitionToChair();
    }

    private void Update()
    {
        if(isOnChair)
            TransitionToChair();
    }

    private void UseChair(InputAction.CallbackContext callback)
    {
        if (cameraCoroutine != null)
            StopCoroutine(cameraCoroutine);

        isOnChair = !isOnChair;

        playerController.enabled = !isOnChair;
        playerCollider.enabled = !isOnChair;
        shotgunCollider.enabled = !isOnChair;
        shotgun.enabled = !isOnChair;
        shotgun.gameObject.SetActive(!isOnChair);
        UI.SetActive(!isOnChair);
        infoText.SetActive(!isOnChair);
        playerRb.isKinematic = isOnChair;

        if (!isOnChair)
        {
            var angles = playerController.rotationHelper.transform.eulerAngles;
            angles.x = 0;
            angles.z = 0;
            playerController.rotationHelper.transform.eulerAngles = angles;
            playerCamera.targetTransform = playerCameraMapTrans;
            monitorScreen.SetScreenType(MonitorScreen.ScreenType.camera);
        }
        else
        {
            playerCamera.targetTransform = playerCameraMidTrans;
            cameraCoroutine = StartCoroutine(SetRightCameraPos());
        }
            

        animator.SetBool("Sitting", isOnChair);

    }

    IEnumerator SetRightCameraPos()
    {
        yield return new WaitForSeconds(0.25f);
        playerCamera.targetTransform = playerCameraChairTrans;
        yield return new WaitForSeconds(0.25f);
        monitorScreen.SetScreenType(MonitorScreen.ScreenType.desktop);

    }

    private void TransitionToChair()
    {
        playerController.rotationHelper.transform.rotation = Quaternion.Slerp(playerController.rotationHelper.transform.rotation, chairPlace.rotation, Time.deltaTime * 4.0f);
        playerController.transform.position = Vector3.Slerp(playerController.transform.position, chairPlace.position, Time.deltaTime * 4.0f);
        
    }


    private void OnEnable() { controls.Enable(); }
    private void OnDisable() { controls.Disable(); }
}
