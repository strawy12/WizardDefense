using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerSelect : MonoBehaviour
{
    [Header("타워 이름 텍스트")] [SerializeField] private Text towerName;
    [Header("설치에 필요한 룬 텍스트")] [SerializeField] private Text needText;
    [Header("타워 이미지")] [SerializeField] private Image towerImage;
    [Header("설치 버튼")] [SerializeField] private Button buildBtn;
    [Header("프리뷰 나가는 버튼")] [SerializeField] private GameObject previewOutBtn;

    [HideInInspector] public int curRune;


    [Header("타워 정보 리스트")] public List<Tower> towerList = new List<Tower>();

    [Header("타워들 스프라이트")] [SerializeField] private Sprite[] towerSprite;
    [Header("타워들 오브젝트 배열")] [SerializeField] private GameObject[] tower;

    [HideInInspector] public int needMax;

    [Header("프리뷰 카메라")] [SerializeField] private Camera previewCamera = null;
    [Header("타워 부모 위치")] [SerializeField] private Transform towerMom;

    public static Transform buildTrn;
    public static GameObject buildObj;

    private int selectTower;
    private bool isOutPreView = false;

    public TpsController tpsController;

    private void Start()
    {
        OnClickTower1();
    }

    public void OnClickTower1()
    {
        SetValue(0);
    }

    public void OnClickTower2()
    {
        SetValue(1);
    }

    public void OnClickTower3()
    {
        SetValue(2);
    }

    private void UpdateUI()
    {
        needText.text = $"{curRune} / {needMax}";
        CheckCanBuild();
    }

    private void SetValue(int num)
    {
        selectTower = num;
        needMax = towerList[num].energy;
        towerImage.sprite = towerSprite[num];

        UpdateUI();
        towerName.text = towerList[num].name;
    }

    public void AddRune()
    {
        curRune++;
        UpdateUI();
    }

    public void MinusRune()
    {
        if (needMax > 0)
        {
            curRune--;
            UpdateUI();
        }
        else
        {
            return;
        }
    }

    public void OnClickBuild()
    {
        if (curRune >= needMax)
        {
            curRune -= needMax;
            GameManager.Instance.UIManager.OnClickOutChang();
            GameManager.Instance.UIManager.FMarkFalse();
            Debug.Log("설치성공");
            UpdateUI();
            SelectTower();
        }
        else
        {
            Debug.Log("룬이 부족하자나;;");
        }
    }

    private void SelectTower()
    {
        GameObject a = null;

        TowerNum(a);
        Destroy(buildObj);
    }

    private void CheckCanBuild()
    {
        if (curRune >= needMax)
        {
            buildBtn.interactable = true;
        }
        else
        {
            buildBtn.interactable = false;
        }
    }

    public void OnClickPreviewOut()
    {
        isOutPreView = true;
    }

    public void OnClickPreView()
    {

    }

    private void TowerNum(GameObject _tower)
    {
        switch (selectTower)
        {
            case 0:
                _tower = Instantiate(tower[0], buildTrn);
                break;

            case 1:
                _tower = Instantiate(tower[1], buildTrn);
                break;

            case 2:
                _tower = Instantiate(tower[2], buildTrn);
                break;

            default:
                break;
        }

        _tower.transform.SetParent(towerMom);
    }
}
