using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IPrintable
{
    public void ManualPrintUpdate();
    UnityEvent<GameObject,int,string> OnTextChanged { get; }
}
