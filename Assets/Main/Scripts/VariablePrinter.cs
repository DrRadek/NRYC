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
    class PrinterType
    {
        [SerializeField] public GameObject textSource;
        public List<PrinterInfo> info;
        //[SerializeField] public TMP_Text printTarget;
        //[SerializeField] public int index;
        //[SerializeField] public string textBefore;
        //[SerializeField] public string textAfter;
    }

    [Serializable]
    class PrinterInfo
    {
        [SerializeField] public TMP_Text printTarget;
        [SerializeField] public int index;
        [SerializeField] public string textBefore;
        [SerializeField] public string textAfter;
    }

    Dictionary<GameObject, List<PrinterInfo>> printers = new();
    Dictionary<GameObject, Dictionary<int,int>> indexes = new();


    private void Start()
    {
        foreach(var type in types)
        {
            if (!type.textSource.TryGetComponent(out IPrintable iTarget))
            {
                Debug.LogError($"{type.textSource} is not {nameof(IPrintable)}");
                continue;
            }

            printers.Add(type.textSource, type.info);
            indexes.Add(type.textSource, new());

            int index = 0;
            foreach(var info in type.info)
            {
                indexes[type.textSource].Add(info.index, index);
                
                index++;
            }

            iTarget.OnTextChanged.AddListener(OnTextChanged);
            iTarget.ManualPrintUpdate();
        }
    }

    private void OnTextChanged(GameObject obj,int index, string text)
    {
        if (!indexes[obj].TryGetValue(index, out int i))
            return;

        var type = printers[obj][i];
        type.printTarget.text = type.textBefore + text + type.textAfter;
    }

}
