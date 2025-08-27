using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Food 
{
    public GameObject prefab;
    public int itemWeight;
}


public class FoodSpawner : MonoBehaviour
{
    [SerializeField] private List<Food> _foodsToSpawn;

    void Start()
    {
        SpawnFood();
    }

    private void SpawnFood()
    {
        Instantiate(GetRandomFood().prefab, transform);
    }

    private Food GetRandomFood()
    {
        int totalWeight = 0;
        foreach(Food f in _foodsToSpawn)
        {
            totalWeight += f.itemWeight;
        }
        Food chosenFood = null;
        int randomValue = Random.Range(1, totalWeight);
        foreach(Food f in _foodsToSpawn)
        {
            randomValue -= f.itemWeight;
            if (randomValue <= 0)
            {
                Debug.Log("FoodChosen: " + f.prefab.name);
                chosenFood = f;
                break;
            }
        }
        return chosenFood;
    }
}
