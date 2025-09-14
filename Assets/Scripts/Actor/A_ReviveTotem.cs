using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class A_ReviveTotem : MonoBehaviour
{
    [SerializeField] private HitBoxCheck _hitboxCheck_Enemy;
    [SerializeField] private HitBoxCheck _hitboxCheck_Spikes;


    public bool _holdingReviveTotem;
    public Vector2 LastSafeLocation
    {
        get { return _lastSafeLocation; }
    }

    private bool _inSpikes = false;
    private bool _usedTotem = false;
    private Vector2 _lastSafeLocation;
    private float _timer;

    private void Start()
    {
        Actor.i.inventory.ItemAdded += CheckAdd;
        Actor.i.inventory.ItemRemoved += CheckRemove;

        List<string> saveItemList = PlayerSaveManager.i.playerSaveData.items.Where(i => i == "revivetotem").ToList();
        if (saveItemList.Count > 0)
        {
            Debug.Log("We Have A revive Totem");
            CheckAdd(saveItemList[0]);
        }
        else
        {
            Debug.Log("We do not have a revive Totem");
        }
    }

    private void Update()
    {
        if (!_holdingReviveTotem)
        {
            return;
        }

        if (_timer > 0)
        {
            _timer -= Time.deltaTime;
        }
        else
        {
            CacheSafePosition();
        }
    }

    private void CacheSafePosition()
    {
        _timer = 2f;
        
        if (!_hitboxCheck_Enemy.colliding && !_inSpikes && Actor.i.collision.Grounded)
        {
            
            _lastSafeLocation = Actor.i.transform.position;
        }
    }

    private void InSpikes()
    {
        _inSpikes = true;
    }

    private void LeaveSpikes()
    {
        _inSpikes = false;
    }


    private void CheckAdd(string id)
    {
        if (!_holdingReviveTotem && id == "revivetotem")
        {
            _holdingReviveTotem = true;
            Actor.i.death.playerDied += UseTotem;
            _hitboxCheck_Enemy.disabled = false;
            Debug.Log("Added Totem");

            //check for spikes
            _hitboxCheck_Spikes.disabled = true;
            _hitboxCheck_Spikes.EnterCollider += InSpikes;
            _hitboxCheck_Spikes.LeftCollider += LeaveSpikes;
        }
    }

    private void CheckRemove(string id)
    {
        if (_holdingReviveTotem && id == "revivetotem")
        {
            Actor.i.death.playerDied -= UseTotem;
            _holdingReviveTotem = false;
            _hitboxCheck_Enemy.disabled = true;
            Debug.Log("Removed Totem");

            //remove spike check

            _hitboxCheck_Spikes.disabled = false;
            _hitboxCheck_Spikes.EnterCollider -= InSpikes;
            _hitboxCheck_Spikes.LeftCollider -= LeaveSpikes;
        }
    }

    private void UseTotem()
    {
        //Use the totem, revive the player in their place
        Actor.i.hunger.EatFood(Actor.i.hunger.totalStartHunger);
        Actor.i.death.Revive();
        Actor.i.inventory.RemoveItem("revivetotem");
        _holdingReviveTotem = false;
    }
}
