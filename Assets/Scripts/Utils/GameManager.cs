using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Monster monster;
    [SerializeField] private List<Item> itemList;
    public static GameManager Inst = null;

    private void Awake()
    {
        Inst = this;
    }
    void Start()
    {
        //for(int i = 0; i < 1000; i++)
        //    monster.Damaged(1);
    }
    private void Update()
    {
        ClickObj();
        if(Input.GetKeyDown(KeyCode.A))
        {
            monster.Damaged(1);
        }
    }
    private void ClickObj()
    {
        Item item = null;
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit = new RaycastHit();


            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray.origin, ray.direction, out hit))
            {
                item = hit.transform.GetComponent<Item>();
                item?.Despawn();
            }

        }
    }

    public void EqualItem(string itemName)
    {
        Item item = itemList.Find((item) => item.itemData.itemName == itemName);

        if(item != null)
        {
            Instantiate(item, monster.transform.position, Quaternion.identity);
        }
    }
}
