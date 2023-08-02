using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [SerializeField] float hp = 10.0f;
    [SerializeField] float maxHp = 10.0f;

    public bool destroyAfterDeath = true;

    public UnityEvent onDeath;
    public UnityEvent onHpChanged;
    public UnityEvent onMaxHpChanged;
    

    public float MaxHp { get => maxHp; 
        set { 
            maxHp = value;
            if(hp > maxHp)
                hp = maxHp;
            onMaxHpChanged.Invoke();
        } 
    }
    public float Hp
    {
        get => hp;
        set
        {
            hp = Mathf.Max(Mathf.Min(maxHp,value),0);
            onHpChanged.Invoke();
            if (hp == 0)
            {
                onDeath.Invoke();
                if (destroyAfterDeath)
                    Destroy(gameObject);
            }
        }
    }
    public void ChangeHp(float amount)
    {
        Hp += amount;
    }
}
