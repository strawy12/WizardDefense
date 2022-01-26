using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropertyItemObject : MonoBehaviour
{
    public PropertyType currentPropertyType;
    private Outline outline;
    private float timer = 0f;
    private bool isActive = false;
    private void Start()
    {
        outline = GetComponent<Outline>();
    }
    private void Update()
    {
        if (isActive)
        {
            timer += Time.deltaTime;

            if (timer >= 3f)
            {
                ShowOutLine(false);
            }
        }
    }

    public void AddPropertyEnergy()
    {
        Debug.Log(currentPropertyType);
        EventManager<PropertyType>.TriggerEvent(ConstantManager.ADD_ENERGY, currentPropertyType);
        Destroy(gameObject);
    }

    public void ShowOutLine(bool isShow)
    {
        if (isShow)
        {
            outline.OutlineWidth = outline.thisOutLine;
            isActive = true;
            timer = 0f;
        }
        else
        {
            outline.OutlineWidth = 0f;
            isActive = false;
            timer = 0f;
        }
    }
}
