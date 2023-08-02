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
        //Destroy(rb);
    }

    void Knockback(Rigidbody knockRb, Pellet pellet, float strength)
    {
        knockRb.AddForce((knockRb.worldCenterOfMass - pellet.transform.position).normalized * strength);
    }

    private void FixedUpdate()
    {
        if (isDestroyed) return;

        Vector3 direction = GameManager.instance.player.transform.position - rb.worldCenterOfMass;

        if (direction.sqrMagnitude > 50) // TODO: add check for player not being alive
            direction = GameManager.instance.computer.transform.position - rb.worldCenterOfMass;

        rb.AddForce(direction.normalized * speed, ForceMode.Acceleration);

        rb.velocity *= 0.9f;
    }

}
