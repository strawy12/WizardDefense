using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensitvitySlider : MonoBehaviour
{
    private UnityEngine.UI.Slider currentSlider;

    private void Awake()
    {
        currentSlider = GetComponent<UnityEngine.UI.Slider>();
        EventManager<float>.StartListening(ConstantManager.CHANGE_SENSITVITY, ChangeValue);
    }

    private void Start()
    {
        ChangeValue(DataManager.Instance.PlayerData.sensitivityValue);
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
