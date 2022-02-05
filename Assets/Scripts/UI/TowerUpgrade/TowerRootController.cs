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
    [SerializeField] List<Transform> rootParentTransforms;

    void Awake()
    {
        for (int i = 0; i < rootParentTransforms.Count; i++)
        {
            rootPanels.Add(rootParentTransforms[i].GetComponentsInChildren<RootElementPanel>());
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
        elementPriceText.text = string.Format("{0}MP 소모", root.price);
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
        gameObject.SetActive(true);
        GameManager.Instance.UIManager.currentUIPanels.Add(gameObject);
        UpdateRoots();
        //UpdateSelectedRoot(GameManager.Instance.Data.GetTowerRoots(GameManager.Instance.censorTower.towerBase.availableRootIndexes[0], 0));
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
        Debug.Log("강화");
    }

    private void FirstSelect(TowerBase tower)
    {
        if(tower.currentRoot.rootIndex == 0)
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
