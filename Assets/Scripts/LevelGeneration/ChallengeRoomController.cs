using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChallengeRoomController : MonoBehaviour
{
    [SerializeField] private HitBoxCheck _check;
    [SerializeField] private GameObject _gatesObject;
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private GameObject _enemySpawnPfx;
    [SerializeField] private GameObject _enemyStartSpawnPfx;
    [SerializeField] private float _maxTimeBetweenEnemies = 5f;
    [SerializeField] private float _minTimeBetweenEnemies = 1f;
    [SerializeField] private int _totalEnemiesCount;
    [SerializeField] private AnimationCurve _enemiesSpawnCurve;
    [SerializeField] private List<Transform> _enemySpawnPoints = new List<Transform>();
    [SerializeField] private AudioClip _openShutDoorsClip;

    private bool _spawnEnemies = false;
    private float _spawnEnemyTimer;
    private int _spawnedEnemiesCount;
    private int _defeatedEnemyCount = 0;
    


    void Start()
    {
        _check.EnterCollider += PlayerEnter;
    }

    private void PlayerEnter()
    {
        //drop the  gates!

        SetGatesState(true);
        //maybe ther's an animation on them whne they turn on

        //start spawning enemies!
        _spawnEnemies = true;
    }

    private void Update()
    {
        //if there are enemies left to spawn
        if (_spawnedEnemiesCount < _totalEnemiesCount)
        {
            if (_spawnEnemies)
            {
                if (_spawnEnemyTimer > 0)
                {
                    _spawnEnemyTimer -= Time.deltaTime;
                }
                else
                {
                    float newTimer = NewSpawnTime();
                    _spawnEnemyTimer = newTimer;
                    Debug.Log("TimeToNextEnemySpawn: " + newTimer);
                    SpawnEnemy();
                }
            }
        }
    }

    private float NewSpawnTime()
    {
        return Util.MapValue(_spawnedEnemiesCount, 0, _totalEnemiesCount, _maxTimeBetweenEnemies, _minTimeBetweenEnemies);
    }

    private void SpawnEnemy()
    {
        _spawnedEnemiesCount++;
        //spawn the fx
        Vector3 spawnPos = _enemySpawnPoints[Random.Range(0, _enemySpawnPoints.Count)].position;
        Instantiate(_enemyStartSpawnPfx, spawnPos, Quaternion.identity);
        IEnumerator spawnRoutine = SpawnEnemyRoutine(2f, spawnPos);
        StartCoroutine(spawnRoutine);
        //get reference to hp script and subscribe to dead
        //otherwise we should have a centralized component system for enemies to grab their hp regardless which enemy it is
    }

    private IEnumerator SpawnEnemyRoutine(float time, Vector3 pos)
    {
        Instantiate(_enemyStartSpawnPfx, pos, Quaternion.identity);
        yield return new WaitForSeconds(time);
        Instantiate(_enemySpawnPfx, pos, Quaternion.identity);
        GameObject go = Instantiate(_enemyPrefab, pos, Quaternion.identity);
        Enemy_HP hpRef = go.GetComponent<Enemy_HP>();
        if (hpRef != null)
        {
            hpRef.Died += EnemyDefeated;
        }
    }

    private void EnemyDefeated()
    {
        _defeatedEnemyCount++;
        if (_defeatedEnemyCount >= _totalEnemiesCount)
        {
            PlayerWinChallenge();
        }
    }
    
    private void PlayerWinChallenge()
    {
        Actor.i.paused = true;
        Actor.i.fader.FadeComplete += AfterFadeIn;
        Actor.i.fader.FadeIn(0, 0.666f);
    }

    private void AfterFadeIn()
    {
        Actor.i.fader.FadeComplete -= AfterFadeIn;
        Actor.i.upgrade.SelectedUpgrade += UpgradeSelected;
        Actor.i.upgrade.OpenUI();
    }

    private void UpgradeSelected()
    {
        Actor.i.upgrade.SelectedUpgrade -= UpgradeSelected;
        Actor.i.upgrade.CloseUI();
        Actor.i.fader.FadeComplete += ResumeGameplay;
        Actor.i.fader.FadeOut(0, 0.666f);
    }

    private void ResumeGameplay()
    {
        Actor.i.fader.FadeComplete -= ResumeGameplay;
        Actor.i.paused = false;
        SetGatesState(false);
    }

    private void SetGatesState(bool state)
    {
        _gatesObject.SetActive(state);
        if (_openShutDoorsClip != null)
        {
            AudioController.control.PlayClip(_openShutDoorsClip);
        }
    }
}
