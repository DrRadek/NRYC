using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IPrintable
{
    public void ManualPrintUpdate(int index);
    UnityEvent<int,string> OnTextChanged { get; }
}
