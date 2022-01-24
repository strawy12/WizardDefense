using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensitvitySlider : MonoBehaviour
{
    private UnityEngine.UI.Slider currentSlider;

    private void Awake()
    {
        currentSlider = GetComponent<UnityEngine.UI.Slider>();
    }

    private void Start()
    {
        EventManager<float>.StartListening(ConstantManager.CHANGE_SENSITVITY, ChangeValue);
    }

    public void ChangeSensitivity(float value)
    {
        EventManager<float>.TriggerEvent(ConstantManager.CHANGE_SENSITVITY, value);
    }

    private void ChangeValue(float value)
    {
        currentSlider.value = value;
    }
}
