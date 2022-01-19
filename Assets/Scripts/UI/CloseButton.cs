using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CloseButton : MonoBehaviour
{
    private Button button;
    [SerializeField] private GameObject parentPanel;

    private void Start()
    {
        button = GetComponent<Button>();

        button.onClick.AddListener(() =>
        {
            GameManager.Instance.UIManager.RemoveCurrentPanels(parentPanel);
        });
    }
}
