using System.Collections.Generic;
using UnityEngine;

public class KeyManager : MonoBehaviour
{
    public static Dictionary<KeyAction, KeyCode> keySettings;
    public List<KeyCode> defaultKeyCodes;
    public List<string> actionNames;

    private void Awake()
    {
        keySettings = new Dictionary<KeyAction, KeyCode>();
        defaultKeyCodes = new List<KeyCode>();

        defaultKeyCodes.Add(KeyCode.W);
        defaultKeyCodes.Add(KeyCode.S);
        defaultKeyCodes.Add(KeyCode.A);
        defaultKeyCodes.Add(KeyCode.D);
        defaultKeyCodes.Add(KeyCode.Space);
        defaultKeyCodes.Add(KeyCode.LeftShift);
        defaultKeyCodes.Add(KeyCode.Q);
        defaultKeyCodes.Add(KeyCode.E);
        defaultKeyCodes.Add(KeyCode.F);
        defaultKeyCodes.Add(KeyCode.F5);
        defaultKeyCodes.Add(KeyCode.R);
        defaultKeyCodes.Add(KeyCode.V);
        defaultKeyCodes.Add(KeyCode.Alpha1);
        defaultKeyCodes.Add(KeyCode.Alpha2);
        defaultKeyCodes.Add(KeyCode.Alpha3);
        defaultKeyCodes.Add(KeyCode.Alpha4);
    }

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
