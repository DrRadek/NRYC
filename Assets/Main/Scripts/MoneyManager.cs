using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MoneyManager : MonoBehaviour, IPrintable
{
    public static MoneyManager instance;

    private int money = 0;

    public int Money { get => money; set { money = value; onTextChanged.Invoke(gameObject, 0, money.ToString()); }  }

    readonly UnityEvent<GameObject, int, string> onTextChanged = new();
    public UnityEvent<GameObject, int, string> OnTextChanged { get => onTextChanged; }

    public void ManualPrintUpdate()
    {
        Money = money;
    }

    void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
            return;
        }

        instance = this;
    }
}
