using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyPanal : MonoBehaviour
{
    private Image targetImage = null;
    private Text energyCountText;
    [SerializeField] private PropertyType currentProperty;

    private EnergyData currentEnergyData;

    private void Start()
    {
        targetImage = transform.GetChild(0).GetComponent<Image>();
        energyCountText = transform.GetChild(1).GetComponent<Text>();

        EventManager<PropertyType>.StartListening(ConstantManager.ADD_ENERGY, AddEnergy);

        currentEnergyData = DataManager.Instance.GetEnergyData(currentProperty);
        energyCountText.text = currentEnergyData.count.ToString();
        ChangeImageColor();
    }

    private void AddEnergy(PropertyType type)
    {
        Debug.Log(currentProperty);
        if (type != currentProperty) return;
        Debug.Log(currentProperty);
        currentEnergyData.count++;
        energyCountText.text = currentEnergyData.count.ToString();
        DataManager.Instance.SaveToJson();
    }

    private void ChangeImageColor()
    {
        switch(currentProperty)
        {
            case PropertyType.Fire:
                targetImage.color = Color.red;
                break;

            case PropertyType.Water:
                targetImage.color = Color.blue;
                break;

            case PropertyType.Terra:
                targetImage.color = Color.green;
                break;

            case PropertyType.Air:
                targetImage.color = Color.cyan;
                break;

            case PropertyType.Holy:
                targetImage.color = Color.yellow;
                break;

            case PropertyType.Void:
                targetImage.color = Color.magenta;
                break;

        }
    }
}
