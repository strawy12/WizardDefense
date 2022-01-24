using System.Collections.Generic;
using UnityEngine;

public class KeyManager : MonoBehaviour
{
    public static Dictionary<KeyAction, KeyCode> keySettings;
    public List<string> actionNames;


    private void Start()
    {
        keySettings = new Dictionary<KeyAction, KeyCode>();

        foreach (var keyInput in DataManager.Instance.PlayerData.keyInputDataList)
        {
            keySettings.Add(keyInput.keyAction, keyInput.keyCode);
        }
    }

    public void SetKeySetting(KeyAction action, KeyCode keycode)
    {
        keySettings[action] = keycode;
        DataManager.Instance.SetKeyInput(action, keycode);
    }
}
