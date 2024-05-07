using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticInfo : MonoBehaviour
{
    public static StaticInfo i;

    public GameObject playerGO;

    private void Awake()
    {
        if (i == null)
        {
            i = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void SetInfo(GameObject player)
    {
        playerGO = player;
    }
}