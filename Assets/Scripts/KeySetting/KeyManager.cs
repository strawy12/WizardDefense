using System.Collections.Generic;
using UnityEngine;

public class KeyManager : MonoBehaviour
{
    public static Dictionary<KeyAction, KeyCode> keySettings = new Dictionary<KeyAction, KeyCode>();
    public List<KeyCode> defaultKeyCodes = new List<KeyCode>
        {
            KeyCode.Mouse0,
            KeyCode.Q,
            KeyCode.F,
            KeyCode.R,
            KeyCode.V
        };
    public List<string> actionNames;

    private void Start()
    {
        for (int i = 0; i < (int)KeyAction.Count; i++)
        {
            keySettings.Add((KeyAction)i, defaultKeyCodes[i]);
        }
    }

    public void SetKeySetting(KeyAction action, KeyCode keycode)
    {
        keySettings[action] = keycode;
    }
}
