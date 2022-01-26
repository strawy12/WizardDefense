using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMSoundManage : MonoBehaviour
{
    private ObjectSound bgmSound;

    private void Awake()
    {
        bgmSound = GetComponent<ObjectSound>();

        EventManager.StartListening(ConstantManager.START_BREAKTIME, StartBreakTime);
        EventManager.StartListening(ConstantManager.START_DEFENSETIME, StartDefenseTime);
    }

    private void StartBreakTime()
    {
        bgmSound.PlaySound(0);
    }

    private void StartDefenseTime()
    {
        bgmSound.PlaySound(1);
    }

}
