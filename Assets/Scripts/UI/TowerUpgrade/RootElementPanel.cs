using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RootElementPanel : MonoBehaviour
{
    private int rootIndex;
    private int index;

    [SerializeField] private Text nameText;
    [SerializeField] private Image image;

    private TowerRoot root;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(() => OnSelected());
    }

    public void Initialize(int rootIndex, int index)
    {
        this.rootIndex = rootIndex;
        this.index = index;
    }

    public void UpdateRoot(TowerBase tower)
    {
        if (rootIndex < tower.availableRootIndexes.Count)
        {
            if (index < GameManager.Instance.Data.GetRootsCount(tower.availableRootIndexes[rootIndex]))
            {
                root = GameManager.Instance.Data.GetTowerRoot(tower.availableRootIndexes[rootIndex], index);
                UpdateUI();
                return;
            }
        }

        gameObject.SetActive(false);
    }

    private void UpdateUI()
    {
        gameObject.SetActive(true);
        nameText.text = root.name;
        image.sprite = root.rootImage;
    }

    private void OnSelected()
    {
        GameManager.Instance.UIManager.RootView.UpdateSelectedRoot(root);
    }
}
