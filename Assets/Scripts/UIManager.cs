using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    #region Tower UI Various
    [Header("Ÿ�� UI")]
    [SerializeField] private Image towerUI;

    [SerializeField] private Image towerStatBar;
    private Text towerStatText;

    [SerializeField] private Transform towerButtons;

    [SerializeField] private Image skillImage;
    [SerializeField] private Image skillCoolTimeImage;

    private EquipmentButton currentEquipButton;
    #endregion

    #region Panels Various
    [Header("�г� UI")]

    #endregion

    [Header("����â")]
    [SerializeField] private GameObject settingPanel;

    [Header("��ž��ġ����ǥ��")] [SerializeField] private GameObject FMark;
    [Header("��ž��ġâ")] [SerializeField] private GameObject buildChang;

    [Header("�ð� �ؽ�Ʈ")]
    [SerializeField] private GameObject breakTimeUI;
    [SerializeField] private Text timeText;
    [SerializeField] private Text skipKeyText;

    [Header("����� ���� Ű ����")]
    [SerializeField] private GameObject keySettingPanal;
    [SerializeField] private InventoryUIManager inventoryUIManager;

    private List<GameObject> currentUIPanels = new List<GameObject>();

    private bool isArea;
    [HideInInspector] public bool isTarget;

    private bool turnOnInventory;

    public void Awake()
    {
        towerStatText = towerStatBar.GetComponentInChildren<Text>();

        //InstantiatePanel();

        //for (int i = 0; i < settingButtonsParent.childCount; i++)
        //{
        //    Button button = settingButtonsParent.GetChild(i).GetComponent<Button>();
        //    button.onClick.AddListener(() => OnClickSettingButton(button.transform.GetSiblingIndex()));
        //}
    }

    private void Start()
    {
        EventManager.StartListening(ConstantManager.TURNOFF_INVENTORY, () => turnOnInventory = false);
    }

    private void Update()
    {
        //ShowSkillUI(GameManager.Instance.selectedTower);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SetCurrentPanels();
        }

        if(Input.GetKeyDown(KeyManager.keySettings[KeyAction.Inventory]))
        {
            if (settingPanel.activeSelf) return;

            turnOnInventory = !turnOnInventory;
            TurnOnInventory(turnOnInventory);
        }
    }

    public void SetTimer(float time)
    {
        timeText.text = string.Format("{0} : {1}", (int)time / 60, (time % 60).ToString("F1"));
    }

    public void ActiveBreakTimeUI(bool isActive)
    {

        breakTimeUI.SetActive(isActive);
        skipKeyText.text = KeyManager.keySettings[KeyAction.Skip].ToString();
    }

    private void SetCurrentPanels()
    {
        if (currentUIPanels.Count > 0)
        {
            currentUIPanels[currentUIPanels.Count - 1].gameObject.SetActive(false);
            currentUIPanels.RemoveAt(currentUIPanels.Count - 1);

            if (currentUIPanels.Count == 0)
            {
                ActiveUIPanalState(false);
                CursorLocked(true);
            }
        }
        else
        {
            ActiveSettingPanel();
        }
    }

    #region Setting Panel
    //private void OnClickSettingButton(int index)
    //{
    //    for (int i = 0; i < settingPanelsParent.childCount; i++)
    //    {
    //        settingPanelsParent.GetChild(i).gameObject.SetActive(i == index);
    //    }
    //}

    //private void InstantiatePanel()
    //{
    //    for (int i = 0; i < (int)KeyAction.Count; i++)
    //    {
    //        GameObject panel = Instantiate(keyPanelTemplate, keyPanelTemplate.transform.parent);
    //        KeyPanel keyPanel = panel.GetComponent<KeyPanel>();
    //        keyPanel.Initialize(i);
    //        keyPanels.Add(keyPanel);
    //    }

    //    keyPanelTemplate.SetActive(false);
    //}

    public void ResetKeyPanel()
    {
        EventManager.TriggerEvent(ConstantManager.CLICK_KEYSETTINGBTN);
        //foreach (KeyPanel panel in keyPanels)
        //{
        //    panel.ResetData();
        //}
    }

    public void ActiveSettingPanel()
    {
        CursorLocked(settingPanel.activeSelf);
        if (settingPanel.activeSelf)
        {
            ActiveUIPanalState(false);
        }
        else
        {
            ActiveUIPanalState(true);
        }

        settingPanel.SetActive(!settingPanel.activeSelf);
    }
    #endregion

    #region TowerUI
    public void ShowSkillUI(TowerAttack tower, bool isActive)
    {
        if (!isActive)
        {
            towerUI.gameObject.SetActive(false);
            skillCoolTimeImage.fillAmount = 0f;
            currentUIPanels.Remove(towerUI.gameObject);
        }

        else
        {
            if(towerUI.gameObject.activeSelf)
            {
                towerUI.gameObject.SetActive(false);
                SetGameState(GameState.Playing);
                currentUIPanels.Remove(towerUI.gameObject);
                return;
            }

            towerUI.gameObject.SetActive(true);
            towerButtons.gameObject.SetActive(GameManager.Instance.inGameState == InGameState.BreakTime);
            currentUIPanels.Add(towerUI.gameObject);

            CursorLocked(false);
            SetGameState(GameState.InGameSetting);

            if (!tower.CheckSkillCoolTime())
            {
                skillCoolTimeImage.fillAmount = (tower.skill.coolTime - tower.useSkillTime) / tower.skill.coolTime;
            }
        }
    }

    public void ShowTowerStatBar(bool isShow, int attack = 0, float speed = 0)
    {
        towerStatBar.gameObject.SetActive(isShow);
        towerStatText.text = string.Format("���ݷ� {0}\n���ݼӵ� {1}", attack, speed);
    }
    #endregion

    #region Tower Build UI
    public void ActivePanal(GameObject panal)
    {
        panal.SetActive(true);
        currentUIPanels.Add(panal);
        panal.transform.DOKill();
        panal.transform.DOScaleY(1f, 0.3f).SetUpdate(true);
    }

    public void ActiveKeySettingPanal(bool isActive)
    {
        if (isActive)
        {
            ActivePanal(keySettingPanal);
        }

        else
        {
            UnActivePanal(keySettingPanal);
        }
    }

    public void UnActivePanal(GameObject panal)
    {
        currentUIPanels.Remove(panal);
        panal.transform.DOKill();
        panal.transform.DOScaleY(0f, 0.2f).SetUpdate(true).OnComplete(() => panal.SetActive(false));
    }

    public void Chang()
    {
        if (settingPanel.activeSelf) return;

        CursorLocked(false);

        isArea = !isArea;
        if (isArea)
        {
            FMark.SetActive(false);
            buildChang.SetActive(true);
            currentUIPanels.Add(buildChang);

            SetGameState(GameState.InGameSetting);
        }
        else
        {
            FMark.SetActive(false);
            buildChang.SetActive(false);
            currentUIPanels.Remove(buildChang);

            SetGameState(GameState.Playing);
        }

    }

    public void OnClickOutChang()
    {
        isArea = !isArea;
        FMark.SetActive(false);
        buildChang.SetActive(false);
        ActiveUIPanalState(false);
        CursorLocked(true);
    }
    #endregion

    public void ActiveUIPanalState(bool isActive)
    {
        if (isActive)
        {
            SetGameState(GameState.InGameSetting);
            GameManager.Instance.gameState = GameState.Setting;
        }

        else
        {
            SetGameState(GameState.Playing);
            GameManager.Instance.gameState = GameState.Playing;
        }
    }

    public void CursorLocked(bool isLocked)
    {
        if (isLocked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    #region Set
    public void AreaCheack()
    {
        isArea = false;
    }

    public void FMarkTrue()
    {
        FMark.SetActive(true);
        isTarget = true;
    }

    public void FMarkFalse()
    {
        FMark.SetActive(false);
        isTarget = false;
    }

    public void RemoveCurrentPanels(GameObject panel)
    {
        currentUIPanels.Remove(panel);
    }

    public void SetCurEquipBtn(EquipmentButton button)
    {
        currentEquipButton = button;
    }
    #endregion

    public bool IsFMarkActive()
    {
        return FMark.activeSelf;
    }

    private void SetGameState(GameState gameState)
    {
        GameManager.Instance.gameState = gameState;
    }
    private void TurnOnInventory(bool turnOn)
    {
        if(turnOn)
        {
            GameManager.Instance.gameState = GameState.Setting;
            CursorLocked(false);
            inventoryUIManager.gameObject.SetActive(true);
            inventoryUIManager.canvasGroup.DOKill();
            inventoryUIManager.canvasGroup.DOFade(1f, 0.25f).SetUpdate(true);
            currentUIPanels.Add(inventoryUIManager.gameObject);
            EventManager.TriggerEvent(ConstantManager.TURNON_INVENTORY);
        }

        else
        {
            GameManager.Instance.gameState = GameState.Playing;
            CursorLocked(true);
            inventoryUIManager.canvasGroup.DOKill();
            inventoryUIManager.canvasGroup.DOFade(0f, 0.25f).SetUpdate(true).OnComplete(() => inventoryUIManager.gameObject.SetActive(false));
            currentUIPanels.Remove(inventoryUIManager.gameObject);
        }
    }
}
