using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Health))]
public class Enemy : MonoBehaviour
{
    [SerializeField] List<GameObject> enemyParts;
    [SerializeField] SphereCollider computerCollider;

    [SerializeField] float speed = 20.0f;

    [SerializeField] List<AudioSource> sounds = new();

    Rigidbody rb;
    Health health;

    bool isDestroyed = false;
    
    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        health = GetComponent<Health>();
    }

    public void OnHit(Pellet pellet)
    {
        health.ChangeHp(-pellet.damage);

        Knockback(rb, pellet, pellet.knockback);

        if (health.Hp == 0 && !isDestroyed)
            OnDeath(pellet);

    }

    void OnDeath(Pellet pellet)
    {
        isDestroyed = true;

        if(Random.Range(0, 25) == 13)
            sounds[1].Play();
        else
            sounds[0].Play();

        MoneyManager.instance.Money += (int)(health.MaxHp * (1 + Mathf.Pow(GameManager.instance.CurrentWave, 2) * 0.2f));

        GameManager.instance.EnemiesLeft--;
        GameManager.instance.EnemiesAlive--;

        var pos = pellet.transform.position;
        pos.y -= 0.3f;
        pellet.transform.position = pos;

        foreach (var part in enemyParts)
        {
            foreach(var child in part.transform.GetComponentsInChildren<Transform>())
            {
                child.gameObject.layer = (int)Layer.Destroyed;
            }

            var partRb = part.AddComponent<Rigidbody>();

            Knockback(partRb, pellet, pellet.knockback * 150);
        }

        var destroyLater = this.AddComponent<DestroyLater>();
        destroyLater.StartDestruction(3.0f);

        computerCollider.radius = 0;
        computerCollider.transform.position = new Vector3(10000, 10000, 100000);

        Destroy(this);
    }

    void Knockback(Rigidbody knockRb, Pellet pellet, float strength)
    {
        knockRb.AddForce((knockRb.worldCenterOfMass - pellet.transform.position).normalized * strength);
    }

    private void FixedUpdate()
    {
        if (isDestroyed) return;

        Vector3 direction = GameManager.instance.player.transform.position - rb.worldCenterOfMass;

        float sqrMagnitude = direction.sqrMagnitude;

        if (sqrMagnitude > 50)
            direction = GameManager.instance.computer.transform.position - rb.worldCenterOfMass;
        else if (sqrMagnitude < 30)
            GameManager.instance.player.speedReduction += 0.003f * Time.deltaTime * (50 - direction.sqrMagnitude);

        rb.AddForce(direction.normalized * speed, ForceMode.Acceleration);

        rb.velocity *= 0.9f;
    }

}
