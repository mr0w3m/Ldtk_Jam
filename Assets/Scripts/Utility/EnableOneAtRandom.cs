using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableOneAtRandom : MonoBehaviour
{
    [SerializeField] private bool _activateOnStart = true;
    [SerializeField] private bool _destroyOthers;
    [SerializeField] private List<GameObject> _objects;

    private void Start()
    {
        if(_activateOnStart)
        {
            Randomize();
        }
    }

    public void Randomize()
    {
        int randomChoice = Random.Range(0, _objects.Count);
        
        for(int i = 0; i < _objects.Count; i++)
        {
            if(i == randomChoice)
            {
                _objects[i].SetActive(true);
            }
            else
            {
                if(_destroyOthers)
                {
                    Destroy(_objects[i]);
                }
                else
                {
                    _objects[i].SetActive(false);
                }
            }
        }
    }
}
