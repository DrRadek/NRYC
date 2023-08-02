using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pellet : MonoBehaviour
{
    public float speed = 1.0f;
    public float damage = 1.0f;
    public float knockback = 1.0f; 

    void FixedUpdate()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.transform.TryGetComponent(out Enemy enemy))
            return;

        enemy.OnHit(this);
        Destroy(gameObject);
    }
}
