using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyPanel : MonoBehaviour
{
    private Text keyActionText;
    private Text keyCodeText;
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
        Event curEvent = Event.current;

        if (curEvent.keyCode.ToString() == "None") return;
        if (KeyManager.keySettings.ContainsValue(curEvent.keyCode)) return;
        if (isSelect && curEvent.isKey)
        {
            //if (curEvent.isKey)
            {
                keyCodeText.text = Event.current.keyCode.ToString();
            }

            //else if (curEvent.isMouse)
            //{
            //    keyCodeText.text = Event.current.pointerType.ToString();
            //}

            GameManager.Instance.KeyManager.SetKeySetting((KeyAction)index, Event.current.keyCode);
        }
    }

    public void Initialize(int index)
    {
        keyActionText.text = GameManager.Instance.KeyManager.actionNames[index];
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
