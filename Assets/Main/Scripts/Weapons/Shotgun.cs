using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.InputSystem;


public class Shotgun : MonoBehaviour
{
    public bool isEquipped = true;
    public bool isAutomatic = false;

    [SerializeField] Pellet pellet;
    [SerializeField] GameObject pelletStorage;
    [SerializeField] GameObject pelletSpawn; 
    [SerializeField] Transform trans;
    [SerializeField] float damage = 1.0f;
    [SerializeField] float pelletSpeed = 2.0f;
    [SerializeField] float pelletKnockback = 3.0f;
    [SerializeField] float pelletCount = 2;
    [SerializeField] float spread = 20.2f;
    [SerializeField] float fireRate = 0.5f;
    [SerializeField] float reloadSpeed = 0.7f;
    [SerializeField] int ammoLoaded = 8;
    [SerializeField] int maxAmmoLoaded = 8;
    // [SerializeField] int ammo = 40;

    float fireProgress = 0;

    Controls controls;
    Controls.PlayerActions playerControls;

    private void Awake()
    {
        controls = new Controls();
        playerControls = controls.Player;

        playerControls.Fire.performed += Fired;
    }

    private void Fired(InputAction.CallbackContext context)
    {
        if (isAutomatic || CurrentControls.type != CurrentControls.Types.Game)
            return;

        if (fireProgress <= 0)
            Shoot();
    }

    private void Update()
    {
        fireProgress = Mathf.Max(0, fireProgress - Time.deltaTime);

        if (!isAutomatic)
            return;

        if (CurrentControls.type != CurrentControls.Types.Game || playerControls.Fire.ReadValue<float>() > 0)
            return;

        if (fireProgress <= 0)
            Shoot();
    }
    
    void Shoot()
    {
        if (ammoLoaded == 0)
            return;

        fireProgress = fireRate;
        ammoLoaded--;

        trans.rotation = transform.rotation;
        trans.Rotate(Vector3.up, -spread/2);

        for(int i = 0; i < pelletCount; i++)
        {
            Pellet instance = Instantiate(pellet, pelletStorage.transform);
            instance.transform.SetPositionAndRotation(pelletSpawn.transform.position, trans.rotation);
            instance.damage = damage;
            instance.speed = pelletSpeed;
            instance.knockback = pelletKnockback;

            trans.Rotate(Vector3.up, spread/(pelletCount-1));
        }
    }

    private void OnEnable() { controls.Enable(); }
    private void OnDisable() { controls.Disable(); }
}
