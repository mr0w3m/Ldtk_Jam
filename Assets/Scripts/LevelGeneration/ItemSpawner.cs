using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class Item
{
    public string id;
    public GameObject prefab;
    public int itemWeight;
}

public class ItemSpawner : MonoBehaviour
{
    [SerializeField] private List<Item> _itemsToSpawn;

    private void Start()
    {
        SpawnItem();
    }

    public void SpawnItem()
    {
        Item newItem = GetRandomItem();
        GameObject go = Instantiate(newItem.prefab, transform);
        go.transform.localPosition = Vector3.zero;
        //PlayerSaveManager.i.playerSaveData.specialItemsToSpawn.Remove(newItem.id);
    }

    private Item GetRandomItem()
    {
        



        int totalWeight = 0;
        foreach (Item f in _itemsToSpawn)
        {
            totalWeight += f.itemWeight;
        }
        Item chosenItem = null;
        int randomValue = Random.Range(1, totalWeight);
        foreach (Item f in _itemsToSpawn)
        {
            randomValue -= f.itemWeight;
            if (randomValue <= 0)
            {

                chosenItem = f;
                break;
            }
        }
        return chosenItem;
    }
}
