using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamager : MonoBehaviour
{
    [SerializeField] private HitBoxCheck _enemyCheck;
    [SerializeField] private int _hitDmg = 1;

    private bool _canDamage = true;


    private void Start()
    {
        _enemyCheck.EnterCollider_Info += HitEnemy;
    }

    private void HitEnemy(CollisionInfo info)
    {
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
           hp.Hit(_hitDmg, this.gameObject);
        }
        else
        {
            Debug.Log("hpWasNull!");
        }
    }
}
