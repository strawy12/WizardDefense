using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Monster monster;
    [SerializeField] private List<Item> itemList;
    [SerializeField] private WayPoints wayPoints;
    [SerializeField] private List<GameObject> prefabs;
    [SerializeField] private Transform enemySpawnPoint;
    public static GameManager Inst = null;
    public WayPoints WayPoints {  get { return wayPoints; } }

    private void Awake()
    {
        Inst = this;
    }
    void Start()
    {
        StartCoroutine(GenerateMonsters());
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

    private IEnumerator GenerateMonsters()
    {
        for(int j = 0; j < 5; j++)
        {
            int generateCount = Random.Range(3, 10);

            for (int i = 0; i < generateCount; i++)
            {
                SpawnMonster();

                yield return new WaitForSeconds(2f);
            }
            yield return new WaitForSeconds(5f);
        }
        
    }

    private void SpawnMonster()
    {
        int randIndex = Random.Range(0, prefabs.Count);

        Instantiate(prefabs[randIndex], enemySpawnPoint.position, Quaternion.identity);
    }
}
