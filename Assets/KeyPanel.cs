using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyPanel : MonoBehaviour
{
    [SerializeField] private Text keyActionText;
    [SerializeField] private Text keyCodeText;
    private Button button;
    private bool isSelect = false;
    private int index;

    private void Start()
    {
        button = GetComponentInChildren<Button>();
        button.onClick.AddListener(() => OnSelect());
    }

    private void OnGUI()
    {
        if (isSelect && Event.current.isKey)
        {
            keyCodeText.text = Event.current.keyCode.ToString();
            GameManager.Instance.KeyManager.SetKeySetting((KeyAction)index, Event.current.keyCode);
        }
    }

    public void Initialize(int index)
    {
        keyActionText.text = ((KeyAction)index).ToString();
        keyCodeText.text = KeyManager.keySettings[(KeyAction)index].ToString();
        this.index = index;
    }

    private void OnSelect()
    {
        GameManager.Instance.UIManager.ResetKeyPanel();
        isSelect = true;
    }

    public void ResetData()
    {
        isSelect = false;
    }
}
