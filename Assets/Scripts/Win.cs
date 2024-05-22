using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Win : MonoBehaviour
{

    [SerializeField] private HitBoxCheck _hitboxCheck;
    [SerializeField] private GameObject _devilAtFire;
    [SerializeField] private GameObject _playerPositionAtFire;

    [SerializeField] private ConversationComponent _convo;



    [SerializeField] private SceneController _sceneController;
    [SerializeField] private string _nextLevelString;

    private void Start()
    {
        _hitboxCheck.EnterCollider += BeatLevel;
    }

    private void BeatLevel()
    {
        Actor.i.paused = true;
        //fade to black,
        Actor.i.fader.FadeComplete += AfterFadeIn1;
        Actor.i.fader.FadeIn(0, 0.666f);
    }

    private void AfterFadeIn1()
    {
        Actor.i.fader.FadeComplete -= AfterFadeIn1;
        Actor.i.movement.SetPosition(_playerPositionAtFire.transform.position);

        Actor.i.fader.FadeComplete += AfterFadeOut1;
        Actor.i.fader.FadeOut(0, 0.666f);
    }

    private void AfterFadeOut1()
    {
        Actor.i.fader.FadeComplete -= AfterFadeOut1;
        Actor.i.conversation.StartConversation(_convo);
        Actor.i.conversation.ConversationComplete += AfterConversation;
    }


    private void AfterConversation()
    {
        Actor.i.conversation.ConversationComplete -= AfterConversation;
        //player goes into sleep mode
        Actor.i.sleeping = true;
        Actor.i.fader.FadeComplete += AfterFadeIn2;
        Actor.i.fader.FadeIn(0, 0.666f);
    }

    private void AfterFadeIn2()
    {
        Actor.i.fader.FadeComplete -= AfterFadeIn2;

        Actor.i.upgrade.OpenUI();
        Actor.i.upgrade.SelectedUpgrade += AfterSelectedUpgrade;
    }

    private void AfterSelectedUpgrade()
    {
        Actor.i.upgrade.SelectedUpgrade -= AfterSelectedUpgrade;
        Actor.i.upgrade.CloseUI();

        _devilAtFire.SetActive(false);

        PlayerSaveData playerSaveRef = PlayerSaveManager.i.playerSaveData;
        playerSaveRef.hunger = Actor.i.hunger.totalStartHunger;
        playerSaveRef.totalHunger = Actor.i.hunger.totalStartHunger;
        Actor.i.health.AddHP(100);
        playerSaveRef.hp = Actor.i.health.hp;
        playerSaveRef.totalHp = Actor.i.health.maxHP;
        playerSaveRef.totalItemSlots = Actor.i.inventory.totalInventoryCount;
        playerSaveRef.items = new List<string>(Actor.i.inventory.inventoryItemStrings);
        PlayerSaveManager.i.playerSaveData = playerSaveRef;
        Actor.i.fader.FadeComplete += AfterFadeOut2;
        Actor.i.fader.FadeOut(0, 1f);
        Actor.i.sleeping = false;
    }

    private void AfterFadeOut2()
    {
        Actor.i.fader.FadeComplete -= AfterFadeOut2;
        
        GoToNextLevel();
    }

    private void GoToNextLevel()
    {
        _sceneController.LoadScene(_nextLevelString);
    }
}
