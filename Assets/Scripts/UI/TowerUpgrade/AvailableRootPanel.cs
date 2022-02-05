using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AvailableRootPanel : PanelBase
{
    private int index;
    protected TowerRoot root;
    [SerializeField] private Image image;

    private void Start()
    {
        if (transform.GetSiblingIndex() == 1)
            isSelected = true;

        GetComponent<Button>().onClick.AddListener(() => OnSelect());
    }

    public override void UpdateData()
    {
        TowerBase tower = GameManager.Instance.censorTower.towerBase;
        index = transform.GetSiblingIndex() - 1;
        if (index == 0) OnSelect();

        //아직 아무 루트도 선택하지 않았을 때
        if (tower.currentRoot.rootIndex == 0)
        {
            if (index < tower.availableRootIndexes.Count)
            {
                UpdateUI();
                return;
            }
        }

        //루트 하나를 선택했을 때
        else
        {
            //첫번째만 뜨게 한다
            if (index == 0)
            {
                UpdateUI();
                return;
            }
        }

        gameObject.SetActive(false);
        root = null;
    }

    private void UpdateUI()
    {
        gameObject.SetActive(true);
        TowerBase tower = GameManager.Instance.censorTower.towerBase;
        if (tower.currentRoot.rootIndex != 0 && tower.currentRoot.index >= GameManager.Instance.Data.GetRootsCount(tower.currentRoot.rootIndex) - 1)
        {
            gameObject.SetActive(false);
        }
        else
        {
            root = GameManager.Instance.Data.GetTowerRoot(tower.availableRootIndexes[index], tower.currentRoot.index + 1);

            nameText.text = root.name;
            infoText.text = root.info;
            priceText.text = string.Format("{0}MP", root.price);
            productImage.sprite = root.rootImage;
        }
    }

    public override void OnSelect()
    {
        GameManager.Instance.UIManager.DeselectAvailablePanels();
        image.color = Color.gray;
        isSelected = true;
    }

    public override void Deselect()
    {
        isSelected = false;
        image.color = Color.white;
    }

    public override TowerRoot GetRoot()
    {
        return root;
    }
}

