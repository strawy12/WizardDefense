using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensitivityInputField : MonoBehaviour
{
    private UnityEngine.UI.InputField currentInputField;

    private void Awake()
    {
        currentInputField = GetComponent<UnityEngine.UI.InputField>();
        EventManager<float>.StartListening(ConstantManager.CHANGE_SENSITVITY, ChangeValue);
    }

    private void Start()
    {
        ChangeValue(DataManager.Instance.PlayerData.sensitivityValue);
    }

    public void ChangeSensitivity(string valueStr)
    {
        float value = float.Parse(valueStr);
        EventManager<float>.TriggerEvent(ConstantManager.CHANGE_SENSITVITY, value);
    }

    private void ChangeValue(float value)
    {
        if(value < 0f)
        {
            value = 0f;
        }

        if(value > 100f)
        {
            value = 100f;
        }

        currentInputField.text = string.Format("{0:0.0}", value);
    }
}
