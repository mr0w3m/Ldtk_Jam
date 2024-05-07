using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "FSM/ChaseState")]
public class ChaseState : AbstractFSMState
{

    [SerializeField] private Vector2 _chaseTimeRange;

    public override void OnEnable()
    {
        base.OnEnable();
        stateType = FSMStateType.Chase;

        //Should probably create an init function;
        StartChase();
    }

    public override bool EnterState()
    {
        base.EnterState();
        return true;
    }

    public override void UpdateLogic()
    {

    }

    public override bool ExitState()
    {
        base.ExitState();
        return true;
    }

    private void StartChase()
    {
        float timeToChase = Random.Range(_chaseTimeRange.x, _chaseTimeRange.y);
        // CoroutineRunner.instance.RunCoroutine(FollowPlayer(timeToChase));
    }

    private IEnumerator FollowPlayer(float time)
    {
        float timer = time;

        while (timer > 0)
        {
            timer -= Time.deltaTime;
            yield return 0f;
        }

        ExitState();

    }
}