using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class ChairManager : MonoBehaviour
{
    [SerializeField] Shotgun shotgun;
    [SerializeField] Collider shotgunCollider;
    [SerializeField] Collider playerCollider;
    [SerializeField] PlayerController playerController;
    [SerializeField] Transform chairPlace;
    [SerializeField] GameObject ui;
    [SerializeField] GameObject infoText;
    [SerializeField] Animator animator;
    [SerializeField] Transform playerCameraChairTrans;
    [SerializeField] Transform playerCameraMidTrans;
    [SerializeField] Transform playerCameraMapTrans;
    [SerializeField] MonitorScreen monitorScreen;
    [SerializeField] PlayerCamera playerCamera;
    [SerializeField] ShowCloseToPlayer chairCloseToPlayer;
    [SerializeField] UIHide uiHide;

    MonitorScreen.ScreenType lastScreenType;

    Coroutine cameraCoroutine;


    Rigidbody playerRb;

    Controls controls;
    Controls.PlayerActions playerControls;

    bool isOnChair = false;

    public bool IsOnChair { get => isOnChair; set => isOnChair = value; }

    private void Awake()
    {
        controls = new Controls();
        playerControls = controls.Player;

        playerControls.InteractWithChair.performed += UseChairCallback;

    }

    private void Start()
    {
        playerRb = playerController.GetComponent<Rigidbody>();
        UseChair();
    }

    private void Update()
    {
        if(IsOnChair)
            TransitionToChair();

    }

    private void UseChairCallback(InputAction.CallbackContext callback)
    {
        if ((chairCloseToPlayer.IsVisible || IsOnChair) && StoryManager.instance.StoryProgression >= StoryManager.Story.wave)
            UseChair();
    }

    private void UseChair()
    {
        if (cameraCoroutine != null)
            StopCoroutine(cameraCoroutine);

        IsOnChair = !IsOnChair;

        playerController.enabled = !IsOnChair;
        playerCollider.enabled = !IsOnChair;
        shotgunCollider.enabled = !IsOnChair;
        shotgun.enabled = !IsOnChair;
        shotgun.gameObject.SetActive(!IsOnChair);

        ui.SetActive(!IsOnChair);
        infoText.SetActive(!IsOnChair);

        if(!IsOnChair)
            uiHide.UpdateState();


        playerRb.isKinematic = IsOnChair;

        if (!IsOnChair)
        {
            var angles = playerController.rotationHelper.transform.eulerAngles;
            angles.x = 0;
            angles.z = 0;
            playerController.rotationHelper.transform.eulerAngles = angles;
            playerCamera.targetTransform = playerCameraMapTrans;

            if (monitorScreen.activeScreen != MonitorScreen.ScreenType.camera)
                lastScreenType = monitorScreen.activeScreen;

            if (monitorScreen.activeScreen == MonitorScreen.ScreenType.camera)
                AudioManager.instance.StopPurr();

            monitorScreen.SetScreenType(MonitorScreen.ScreenType.camera);
        }
        else
        {
            playerCamera.targetTransform = playerCameraMidTrans;
            cameraCoroutine = StartCoroutine(SetRightCameraPos());
        }
            

        animator.SetBool("Sitting", IsOnChair);

    }

    IEnumerator SetRightCameraPos()
    {
        yield return new WaitForSeconds(0.25f);
        playerCamera.targetTransform = playerCameraChairTrans;
        yield return new WaitForSeconds(0.25f);
        
        if (StoryManager.instance.StoryProgression <= StoryManager.Story.introStart)
            monitorScreen.SetScreenType(MonitorScreen.ScreenType.cat);
        else
        {
            monitorScreen.SetScreenType(lastScreenType);
        }
    }

    private void TransitionToChair()
    {
        playerController.rotationHelper.transform.rotation = Quaternion.Slerp(playerController.rotationHelper.transform.rotation, chairPlace.rotation, Time.deltaTime * 4.0f);
        playerController.transform.position = Vector3.Slerp(playerController.transform.position, chairPlace.position, Time.deltaTime * 4.0f);
        
    }


    private void OnEnable() { controls.Enable(); }
    private void OnDisable() { controls.Disable(); }
}
