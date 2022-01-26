using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyPanel : MonoBehaviour
{
    [SerializeField] private KeyAction currentKeyAction;
    //private Text keyActionText;
    private Text keyCodeText = null;
    private Button button;
    private bool isSelect = false;
    private int index;

    private void Awake()
    {
        EventManager.StartListening(ConstantManager.CLICK_KEYSETTINGBTN, ResetData);
    }

    private void Start()
    {
        currentKeyAction = (KeyAction)Enum.Parse(typeof(KeyAction), name);
        button = transform.GetChild(1).GetComponent<Button>();
        keyCodeText = button.transform.GetChild(0).GetComponent<Text>();
        button.onClick.AddListener(() => OnSelect());
        Initialize();
    }

    private void OnGUI()
    {
        Event curEvent = Event.current;

        if (curEvent.keyCode.ToString() == "None") return;
        if (KeyManager.keySettings.ContainsValue(curEvent.keyCode)) return;
        if (CheckMoveKeyEqual(curEvent.keyCode)) return;
        if (isSelect && curEvent.isKey)
        {
            if (curEvent.keyCode == KeyCode.Escape)
            {
                isSelect = false;
                return;
            }
            //if (curEvent.isKey)
            keyCodeText.text = curEvent.keyCode.ToString();
            isSelect = false;

            //else if (curEvent.isMouse)
            //{
            //    keyCodeText.text = Event.current.pointerType.ToString();
            //}
            GameManager.Instance.UIManager.ActiveKeySettingPanal(false);
            GameManager.Instance.KeyManager.SetKeySetting(currentKeyAction, Event.current.keyCode);
        }
    }

    private bool CheckMoveKeyEqual(KeyCode keycode)
    {
        if(keycode == KeyCode.W || keycode == KeyCode.A || keycode == KeyCode.S || keycode == KeyCode.D)
        {
            return true;
        }

        if (keycode == KeyCode.UpArrow || keycode == KeyCode.DownArrow || keycode == KeyCode.LeftArrow || keycode == KeyCode.RightArrow)
        {
            return true;
        }

        return false;
    }

    public void Initialize()
    {
        keyCodeText.text = KeyManager.keySettings[currentKeyAction].ToString();
    }

    private void OnSelect()
    {
        GameManager.Instance.UIManager.UiSound.PlaySound(1);
        GameManager.Instance.UIManager.ResetKeyPanel();
        GameManager.Instance.UIManager.ActiveKeySettingPanal(true);
        isSelect = true;
    }

    public void ResetData()
    {
        isSelect = false;
    }
}
