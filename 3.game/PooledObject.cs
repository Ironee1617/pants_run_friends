using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PooledObject : MonoBehaviour {
    public int poolItemID;
    public string poolItemName = string.Empty;
    public GameObject itemPrefab;

    public int itemCount = 0;

    [SerializeField]
    private List<GameObject> itemList = new List<GameObject>();

    public void Initialize()
    {
        for (int i = 0; i < itemCount; ++i)
            itemList.Add(CreateItem());
    }

    //몬스터 생성 및 리스트에 저장
    public void PushToPool(GameObject item)
    {
        item.transform.SetParent(transform);
        item.SetActive(false);
        itemList.Add(item);
    }

    //0번 몬스터 뽑아내기
    public GameObject PopFromPool()
    {
        if (itemList.Count == 0)
            itemList.Add(CreateItem());

        GameObject item = itemList[0];
        itemList.RemoveAt(0);
        return item;
    }

    private GameObject CreateItem()
    {
        GameObject item = Instantiate(itemPrefab) as GameObject;
        item.name = poolItemName;
        item.transform.SetParent(transform);
        item.SetActive(false);

        return item;
    }
}
