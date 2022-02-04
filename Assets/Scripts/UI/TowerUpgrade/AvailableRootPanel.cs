using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvailableRootPanel : PanelBase
{
    private int index;

    private void Start()
    {
        index = transform.GetSiblingIndex() - 1;
    }
    public override void UpdateData()
    {
        TowerBase tower = GameManager.Instance.censorTower.towerBase;

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
    }

    private void UpdateUI()
    {
        gameObject.SetActive(true);
        TowerBase tower = GameManager.Instance.censorTower.towerBase;

        TowerRoot root = GameManager.Instance.Data.GetTowerRoots(tower.availableRootIndexes[index], tower.currentRoot.index);

        nameText.text = root.name;
        infoText.text = root.info;
        priceText.text = string.Format("{0}MP", root.price);
        productImage.sprite = root.rootImage;
    }
}

