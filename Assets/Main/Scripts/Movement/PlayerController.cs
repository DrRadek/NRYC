using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] public GameObject rotationHelper;
    [SerializeField] GameObject shotgun;
    [SerializeField] float speed = 50.0f;

    Rigidbody rb;

    Vector3 dir = Vector3.zero;

    Controls controls;
    Controls.PlayerActions playerControls;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        controls = new Controls();
        playerControls = controls.Player;

    }

    
    void FixedUpdate()
    {
        Vector2 move = playerControls.Walk.ReadValue<Vector2>();
        dir.x = move.x;
        dir.z = move.y;

        rb.AddForce(transform.rotation * dir * speed);
        rb.velocity *= 0.9f;

        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (!Physics.Raycast(ray, out RaycastHit hit, 100, 1 << 3))
            return;

        Vector3 direction = hit.point - transform.position;
        Vector3 groundPerpendicular = Vector3.ProjectOnPlane(direction, -rotationHelper.transform.up).normalized;
        Quaternion lookOnLook = Quaternion.LookRotation(groundPerpendicular, rotationHelper.transform.up);
        rotationHelper.transform.rotation = Quaternion.Slerp(rotationHelper.transform.rotation, lookOnLook, Time.deltaTime * 4.0f);

        
        direction = hit.point - shotgun.transform.position;
        groundPerpendicular = Vector3.ProjectOnPlane(direction, -rotationHelper.transform.up).normalized;
        lookOnLook = Quaternion.LookRotation(groundPerpendicular, rotationHelper.transform.up);
        shotgun.transform.rotation = Quaternion.Slerp(shotgun.transform.rotation, lookOnLook, Time.deltaTime * 4.0f);

        if (transform.position.y < -1)
        {
            var pos = transform.position;
            pos.y = 3;
            transform.position = pos;
        }
            

    }

    private void OnEnable() { controls.Enable(); }
    private void OnDisable() { controls.Disable(); }
}
