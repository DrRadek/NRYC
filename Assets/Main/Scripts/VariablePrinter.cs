using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.UI;
using TMPro;


public class VariablePrinter : MonoBehaviour
{
    [SerializeField] List<PrinterType> types;

    [Serializable]
    public class PrinterType
    {
        [SerializeField] public GameObject textTarget;
        [SerializeField] public TMP_Text printTarget;
        [SerializeField] public int index;
        [SerializeField] public string textBefore;
        [SerializeField] public string textAfter;
    }

    private void Start()
    {
        foreach(var type in types)
        {
            if (!type.textTarget.TryGetComponent(out IPrintable iTarget))
            {
                Debug.LogError($"{type.textTarget} is not {nameof(IPrintable)}");
                continue;
            }

            iTarget.OnTextChanged.AddListener(OnTextChanged);
            iTarget.ManualPrintUpdate(type.index);
        }
    }

    private void OnTextChanged(int index, string text)
    {
        var type = types[index];
        type.printTarget.text = type.textBefore + text + type.textAfter;
    }

}
