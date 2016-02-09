using UnityEngine;
using System.Collections.Generic;

public class BossYoungMainBehavior : VoBehavior
{
    public List<GameObject> SubBosses;
    public GameObject EndFlowObject;

    void Start()
    {
        for (int i = 0; i < this.SubBosses.Count; ++i)
        {
            GameObject subBoss = this.SubBosses[i];
            subBoss.GetComponent<Damagable>().OnDeathCallbacks.Add(this.SubBossKilled);
        }

        _stateMachine.AddState(ROTATION_STATE, updateRotation, enterRotation, exitRotation);
        _stateMachine.AddState(ATTACKING_STATE, updateAttacking, enterAttacking, exitAttacking);
        enterRotation();
        _stateMachine.BeginWithInitialState(ROTATION_STATE);
    }

    void Update()
    {
        _stateMachine.Update();
    }

    /**
     * Private
     */
    private TimedCallbacks _timedCallbacks;
    private FSMStateMachine _stateMachine;
    private bool _switchState;

    private const string ROTATION_STATE = "rotation";
    private const string ATTACKING_STATE = "path";

    private void switchState()
    {
        _switchState = true;
    }

    private void SubBossKilled(Damagable died)
    {
        this.SubBosses.Remove(died.gameObject);

        if (this.SubBosses.Count == 0)
        {
            this.EndFlowObject.GetComponent<WinCondition>().EndLevel();
        }
    }

    private string updateRotation()
    {
        return !_switchState ? ROTATION_STATE : ATTACKING_STATE;
    }

    private void enterRotation()
    {
        _switchState = false;
    }

    private void exitRotation()
    {
    }

    private string updateAttacking()
    {
        return !_switchState ? ATTACKING_STATE : ROTATION_STATE;
    }

    private void enterAttacking()
    {
        _switchState = false;
    }

    private void exitAttacking()
    {
    }
}
