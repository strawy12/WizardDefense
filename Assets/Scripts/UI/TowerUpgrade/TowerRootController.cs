using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerRootController : MonoBehaviour
{
    [Header("Selected Element")]
    [SerializeField] private Text elementNameText;
    [SerializeField] private Text elementInfoText;
    [SerializeField] private Text elementPriceText;
    [SerializeField] private Image elementImage;

    [Header("Control Root Panels")]
    private List<RootElementPanel[]> rootPanels = new List<RootElementPanel[]>();
    private List<RectTransform> lines = new List<RectTransform>();

    [SerializeField] private List<Transform> rootParentTransforms;
    [SerializeField] private GameObject rootView;

    void Awake()
    {
        for (int i = 0; i < rootParentTransforms.Count; i++)
        {
            rootPanels.Add(rootParentTransforms[i].GetComponentsInChildren<RootElementPanel>());
            lines.Add(rootParentTransforms[i].parent.GetChild(0).GetComponent<RectTransform>());
        }

        InitializeRoots();
    }

    private void InitializeRoots()
    {
        for (int i = 0; i < rootPanels.Count; i++)
        {
            for (int j = 0; j < rootPanels[i].Length; j++)
            {
                rootPanels[i][j].Initialize(i, j);
            }
        }
    }

    public void UpdateRoots()
    {
        TowerBase tower = GameManager.Instance.censorTower.towerBase;
        for (int i = 0; i < rootPanels.Count; i++)
        {
            rootParentTransforms[i].parent.gameObject.SetActive(i < tower.availableRootIndexes.Count);

            if (i < tower.availableRootIndexes.Count)
            {
                int maxCount = GameManager.Instance.Data.GetRootsCount(tower.availableRootIndexes[i]);
                lines[i].sizeDelta = new Vector2(120f * maxCount, lines[i].sizeDelta.y);
            }

            for (int j = 0; j < rootPanels[i].Length; j++)
            {
                rootPanels[i][j].UpdateRoot(tower);
            }
        }

        FirstSelect(tower);
    }

    public void UpdateSelectedRoot(TowerRoot root)
    {
        Deselect();
        elementNameText.text = root.name;
        elementInfoText.text = root.info;
        elementPriceText.text = string.Format("{0}MP �Ҹ�", root.price);
        elementImage.sprite = root.rootImage;
    }

    private void Deselect()
    {
        for (int i = 0; i < rootPanels.Count; i++)
        {
            for (int j = 0; j < rootPanels[i].Length; j++)
            {
                rootPanels[i][j].Deselect();
            }
        }
    }

    public void ShowRootView()
    {
        rootView.SetActive(true);
        GameManager.Instance.UIManager.currentUIPanels.Add(rootView);
        UpdateRoots();
    }

    public void OnEnforcement()
    {
        TowerRoot root = GameManager.Instance.UIManager.GetCurrentSelectedRoot();
        TowerBase tower = GameManager.Instance.censorTower.towerBase;
        string[] datas = GameManager.Instance.Data.GetTowerRootData(root).Split(',');

        if (GameManager.Instance.Data.GetRootsCount(int.Parse(datas[0])) < tower.currentRoot.index + 2) return;
        if (datas.Length == 0) return;

        tower.currentRoot.rootIndex = int.Parse(datas[0]);
        tower.currentRoot.index = int.Parse(datas[1]);

        GameManager.Instance.UIManager.UpdateAvailablePanels();
        Debug.Log("��ȭ");
    }

    private void FirstSelect(TowerBase tower)
    {
        if (tower.currentRoot.rootIndex == 0)
        {
            rootPanels[0][0].OnSelected();
        }
        else
        {
            int index = tower.availableRootIndexes.FindIndex(x => x == tower.currentRoot.rootIndex);
            rootPanels[index][tower.currentRoot.index].OnSelected();
        }
    }
}
