using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

public class Shotgun : MonoBehaviour
{
    public bool isEquipped = true;

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
    [SerializeField] int ammoLoaded = 1;
    [SerializeField] int maxAmmoLoaded = 8;
    [SerializeField] int ammo = 40;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("shoot", 0.5f, 0.5f);
    }

    void shoot()
    {
        if (ammoLoaded == 0)
            return;

        ammoLoaded--;

        trans.rotation = transform.rotation;
        trans.Rotate(Vector3.up, -spread/2);

        for(int i = 0; i < pelletCount; i++)
        {
            Pellet instance = Instantiate(pellet, pelletStorage.transform);
            instance.transform.position = pelletSpawn.transform.position;
            instance.transform.rotation = trans.rotation;
            instance.damage = damage;
            instance.speed = pelletSpeed;
            instance.knockback = pelletKnockback;

            trans.Rotate(Vector3.up, spread/(pelletCount-1));
        }
    }
}
