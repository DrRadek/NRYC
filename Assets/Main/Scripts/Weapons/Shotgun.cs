using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;


public class Shotgun : MonoBehaviour, IPrintable
{
    readonly UnityEvent<GameObject, int, string> onTextChanged = new();
    public UnityEvent<GameObject, int, string> OnTextChanged { get { return onTextChanged; } }

    public int AmmoLoaded { get => ammoLoaded; 
        private set 
        {
            ammoLoaded = value;
            onTextChanged.Invoke(gameObject,200, $"{value}/{maxAmmoLoaded}");
        } 
    }

    public void ManualPrintUpdate()
    {
        AmmoLoaded = ammoLoaded;

        int index = 0;
        foreach (var upgrade in upgrades)
        {
            PrintUpgrade(GetUpgradeAtIndex(index), index);
            index++;
        }

    }

    public bool isEquipped = true;
    public bool isAutomatic = false;

    [SerializeField] Pellet pellet;
    [SerializeField] GameObject pelletStorage;
    [SerializeField] GameObject pelletSpawn; 
    [SerializeField] Transform trans;
    [SerializeField] float damage = 1.0f;
    [SerializeField] float pelletSpeed = 2.0f;
    [SerializeField] float pelletKnockback = 3.0f;
    [SerializeField] int pelletCount = 2;
    [SerializeField] float spread = 20.0f;
    [SerializeField] float fireRate = 0.5f;
    [SerializeField] float reloadSpeed = 0.7f;
    [SerializeField] int ammoLoaded = 8;
    [SerializeField] int maxAmmoLoaded = 8;

    [SerializeField] AudioSource reloadSound;
    [SerializeField] AudioSource shootSound;


    [Serializable]
    private class WeaponUpgrade
    {
        public List<float> upgrades = new();
        public float max;
        public int upgradeIndex = 0;
        public bool isMax = false;
        public int upgradeCost = 6;
    }

    [SerializeField] List<WeaponUpgrade> upgrades = new();


    public void AutoUpgrade()
    {
        if (MoneyManager.instance.Money < 100)
            return;

        MoneyManager.instance.Money -= 100;
        isAutomatic = true;
    }

    public void UpgradeAtIndex(int index)
    {
        var weaponUpgrade = upgrades[index];

        if (weaponUpgrade.upgradeCost > MoneyManager.instance.Money || weaponUpgrade.isMax)
            return;

        MoneyManager.instance.Money -= weaponUpgrade.upgradeCost;

        var valueAfterUpgrade = GetValueAfterUpgrade(index);

        weaponUpgrade.upgradeCost = (int)(weaponUpgrade.upgradeCost * 1.5f);

        weaponUpgrade.upgradeIndex = Mathf.Min(weaponUpgrade.upgradeIndex + 1, weaponUpgrade.upgrades.Count-1);

        SetUpgradeAtIndex(index, valueAfterUpgrade);
    }

    private float GetValueAfterUpgrade(int index)
    {
        var weaponUpgrade = upgrades[index];

        var upgrade = weaponUpgrade.upgrades[weaponUpgrade.upgradeIndex];

        var value = GetUpgradeAtIndex(index) * upgrade;

        float newValue;

        if (upgrade > 1)
            newValue = Mathf.Min(value, weaponUpgrade.max);
        else
            newValue = Mathf.Max(value, weaponUpgrade.max);

        if (index == 3 || index == 7 || index == 8)
            return (int)newValue;
        else
            return newValue;
    }

    private void SetUpgradeAtIndex(int index, float newValue)
    {
        switch (index)
        {
            case 0:
                damage = newValue;
                break;
            case 1:
                pelletSpeed = newValue;
                break;
            case 2:
                pelletKnockback = newValue;
                break;
            case 3:
                pelletCount = (int)newValue;
                break;
            case 4:
                spread = newValue;
                break;
            case 5:
                fireRate = newValue;
                break;
            case 6:
                reloadSpeed = newValue;
                break;
            case 7:
                maxAmmoLoaded = (int)newValue;
                AmmoLoaded = maxAmmoLoaded;
                break;
        }

        PrintUpgrade(newValue, index);
    }

    void PrintUpgrade(float newValue, int index)
    {
        var valueAfterUpgrade = GetValueAfterUpgrade(index);
        var newValueOld = newValue;
        if (index == 5)
        {
            newValue = 60 / (float)newValue;
            valueAfterUpgrade = 60 / (float)valueAfterUpgrade;
        }

        var newValueString = string.Format("{0:0.##}", newValue);
        var valueAfterUpgradeString = string.Format("{0:0.##}", valueAfterUpgrade);

        if (newValueOld == upgrades[index].max)
        {
            upgrades[index].isMax = true;
            onTextChanged.Invoke(gameObject, index, $"{newValueString} [MAX]");
        }
        else
        {

            onTextChanged.Invoke(gameObject, index, $"{newValueString} => {valueAfterUpgradeString} ... Cost: {upgrades[index].upgradeCost}");
        }
    }

    private float GetUpgradeAtIndex(int index)
    {
        return index switch
        {
            0 => damage,
            1 => pelletSpeed,
            2 => pelletKnockback,
            3 => pelletCount,
            4 => spread,
            5 => fireRate,
            6 => reloadSpeed,
            7 => maxAmmoLoaded,
            _ => 0
        };
    }

    // [SerializeField] int ammo = 40;

    float fireProgress = 0;

    Controls controls;
    Controls.PlayerActions playerControls;

    bool shotDuringReloading = false;
    bool isReloading = false;


    private void Awake()
    {
        controls = new Controls();
        playerControls = controls.Player;

        playerControls.Fire.performed += Fired;
        playerControls.Reload.performed += Reloaded;
    }

    private void Reloaded(InputAction.CallbackContext context)
    {
        if(ammoLoaded < maxAmmoLoaded && !isReloading)
            StartCoroutine(Reloading());     
    }

    private void Fired(InputAction.CallbackContext context)
    {
        if (isAutomatic)
            return;

        if (fireProgress <= 0)
            Shoot();
    }

    private IEnumerator Reloading()
    {
        shotDuringReloading = false;
        isReloading = true;
        while (ammoLoaded < maxAmmoLoaded)
        {
            yield return new WaitForSeconds(reloadSpeed);
            if (shotDuringReloading || !isReloading)
            {
                isReloading = false;
                reloadSound.Stop();
                yield break;
            }
            AmmoLoaded++;

            if (!reloadSound.isPlaying)
                reloadSound.Play();

        }
        isReloading = false;
    }

    private void Update()
    {
        fireProgress = Mathf.Max(0, fireProgress - Time.deltaTime);

        if (!isAutomatic)
            return;

        if (playerControls.Fire.ReadValue<float>() == 0)
            return;

        if (fireProgress <= 0)
            Shoot();

    }
    
    void Shoot()
    {
        if (AmmoLoaded == 0)
            return;

        if (!shootSound.isPlaying)
            shootSound.Play();


        fireProgress = fireRate;
        AmmoLoaded--;
        shotDuringReloading = true;

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
    private void OnDisable() { controls.Disable(); isReloading = false; }


}
