using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using ValueChecks;

public class AnimatedProgresBar : MonoBehaviour
{
    // objects
    [SerializeField] private GameObject rawImagePrefab;
    private TextMeshProUGUI text;

    
    [SerializeField] private List<strings> textStrings;
    // temp variables
    [SerializeField] private List<Value> values;
    [SerializeField] private List<BarInEditor> bars;
    

    // variables
    private List<AnimatedBar> animatedBar;
    private RectTransform rectTransform;
    private float width;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(GetComponent<RawImage>());
        Destroy(GetComponent<CanvasRenderer>());

        rectTransform = GetComponent<RectTransform>();
        text = GetComponentInChildren<TextMeshProUGUI>();
        animatedBar = new List<AnimatedBar>(bars.Count);

        for (int i = 0; i < bars.Count; i++)
        {
            AnimatedBar instance = new AnimatedBar();
            instance.gameObject = Instantiate(rawImagePrefab,transform);
            instance.rawImage = instance.gameObject.GetComponent<RawImage>();
            instance.rectTransform = instance.gameObject.GetComponent<RectTransform>();

            instance.rectTransform.sizeDelta = rectTransform.sizeDelta;
            instance.rawImage.color = bars[i].color;
            instance.rawImage.texture = bars[i].texture;
            instance.rawImage.material = bars[i].material;
            instance.valueIndex = bars[i].valueIndex;
            instance.isReverseValue = bars[i].isReverseValue;
            instance.startFromRight = bars[i].startFromRight;
            instance.currentValue = values[instance.valueIndex].value;

            animatedBar.Add(instance);
        }
        width = rectTransform.rect.xMax - rectTransform.rect.xMin;

        if (textStrings.Count != 0)
        {
            text.transform.SetAsLastSibling();
            text.enabled = true;
        }
        else
        {
            text.enabled = false;
        }

        bars.Clear();
    }

    private void Update()
    {
        for (int i = 0; i < values.Count; i++)
        {
            SetValue(i, values[i].value);
        }

        for (int i = 0; i < animatedBar.Count; i++)
        {
            UpdateBar(i);
        }

        if(textStrings.Count > 0)
        {
            string newText = "";
            foreach (strings strings in textStrings)
            {
                newText += strings.text;
                if (strings.valueIndex >= 0)
                {
                    float value = 0;
                    if (strings.inPercent)
                        value = returnValueNormalized(values[strings.valueIndex])*100;
                    else
                        value = values[strings.valueIndex].value;

                    if(strings.roundToInt)
                        value = Convert.ToInt32(value);
                    newText += value;
                }
            }
            text.text = newText;
        }
    }


    private class Bar
    {
        public int valueIndex = 0;
        public bool isReverseValue = false;
        public bool startFromRight = false;
    }

    private class AnimatedBar : Bar
    {
        public GameObject gameObject;
        public RectTransform rectTransform;
        public RawImage rawImage;
        public float currentValue = 0;
    }

    [Serializable]
    private class strings
    {
        public string text;
        public int valueIndex;
        public bool inPercent = false;
        public bool roundToInt = false;
    }

    [Serializable]
    private class BarInEditor : Bar
    {
        public Color color = Color.white;
        public Texture texture = null;
        public Material material = null;
    }

    [Serializable]
    private class Value
    {
        public float value = 0;
        public float maxValue = 0;
        public float minValue = 0;
    }

    public float GetValue(int index)
    {
        return values[index].value;
    }

    public ValueChangeResult SetValue(int index, float value)
    {
        ValueChangeResult result;
        if (value <= values[index].minValue)
        {
            values[index].value = values[index].minValue;
            result = ValueChangeResult.EMPTY;
        }
        else if (value >= values[index].maxValue)
        {
            values[index].value = values[index].maxValue;
            result = ValueChangeResult.FULL;
        }
        else
        {
            values[index].value = value;
            result = ValueChangeResult.OK;
        }

        return result;
    }

    public void SetLimits(int index, float min, float max)
    {
        values[index].minValue = min;
        values[index].maxValue = max;
    }

    public float GetMaxLimit(int index)
    {
        return values[index].maxValue;
    }

    public float GetMinLimit(int index)
    {
        return values[index].maxValue;
    }

    private void UpdateBar(int index)
    {
        AnimatedBar bar = animatedBar[index];
        float change = values[bar.valueIndex].value - bar.currentValue;
        bar.currentValue += change * 0.20f * Time.deltaTime * 60.0f;
        if(Mathf.Abs(change) < 0.03f)
            bar.currentValue = values[bar.valueIndex].value;

        float targetValue = width * (bar.currentValue - values[bar.valueIndex].minValue) / (values[bar.valueIndex].maxValue - values[bar.valueIndex].minValue);
        if (bar.isReverseValue && !bar.startFromRight || bar.startFromRight && !bar.isReverseValue)
            targetValue = width - targetValue;

        if (bar.startFromRight)
            bar.rectTransform.offsetMin = new Vector2(-width / 2 + targetValue, animatedBar[0].rectTransform.offsetMin.y);
        else
            bar.rectTransform.offsetMax = new Vector2(-width / 2 + targetValue, animatedBar[0].rectTransform.offsetMax.y);
    }

    private float returnValueNormalized(Value value)
    {
        return (value.value - value.minValue) / (value.maxValue - value.minValue);
    }
}

