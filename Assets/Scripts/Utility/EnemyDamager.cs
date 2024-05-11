using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamager : MonoBehaviour
{
    [SerializeField] private HitBoxCheck _enemyCheck;

    private bool _canDamage = true;


    private void Start()
    {
        _enemyCheck.EnterCollider_Info += HitEnemy;
    }

    private void HitEnemy(CollisionInfo info)
    {
        Debug.Log("HitEnemy!");
        if (!_canDamage)
        {
            Debug.Log("Can'tDamage!");
            return;
        }
        _enemyCheck.EnterCollider_Info -= HitEnemy;
        _canDamage = false;
        Enemy_HP hp = info.col.GetComponent<Enemy_HP>();
        if (hp != null)
        {
           hp.Hit();
        }
        else
        {
            Debug.Log("hpWasNull!");
        }
    }
}
