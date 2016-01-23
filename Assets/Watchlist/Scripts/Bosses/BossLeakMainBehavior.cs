using UnityEngine;
using System.Collections.Generic;

public class BossLeakMainBehavior : VoBehavior
{
    public Vector2[] StartingVelocities;
    public float StartingSpeed;
    public int[] PossibleRotationDirections = { 1, -1 };
    public float RotationSpeed = 180.0f;
    public float TimeLockedIn = 8.0f;
    public float TimeSpreadOut = 8.0f;
    public float ReturningLerpDistance = 10.0f;
    public float SubBossMinSpeed = 75.0f;
    public float SubBossMaxSpeed = 150.0f;
    public LayerMask LevelGeomLayerMask;
    public List<GameObject> SubBosses;
    public GameObject EndFlowObject;

    void Start()
    {
        _actor = this.GetComponent<Actor2D>();
        _timedCallbacks = this.GetComponent<TimedCallbacks>();
        _rotationAxis = new Vector3(0, 0, 1);
        _startingPositions = new Vector3[this.SubBosses.Count];
        _stateMachine = new FSMStateMachine();
        _stateMachine.AddState(LOCKED_IN_STATE, this.LockedInUpdate, this.EnterLockedIn, this.ExitLockedIn);
        _stateMachine.AddState(SPREAD_OUT_STATE, this.SpreadOutUpdate, this.EnterSpreadOut, this.ExitSpreadOut);
        _stateMachine.AddState(RETURNING_STATE, this.ReturningUpdate, this.EnterReturning, this.ExitReturning);

        this.EnterLockedIn();
        _stateMachine.BeginWithInitialState(LOCKED_IN_STATE);

        for(int i = 0; i < this.SubBosses.Count; ++i)
        {
            GameObject subBoss = this.SubBosses[i];
            subBoss.GetComponent<Damagable>().OnDeathCallbacks.Add(this.SubBossKilled);
            _startingPositions[i] = subBoss.transform.localPosition;
        }
    }

    void Update()
    {
        _stateMachine.Update();
    }

    /**
     * Private
     */
    private Actor2D _actor;
    private TimedCallbacks _timedCallbacks;
    private int _rotationDirection;
    private Vector3 _rotationAxis;
    private float _currentAngle;
    private FSMStateMachine _stateMachine;
    private bool _stateSwitch;
    private Vector3[] _startingPositions;

    private const string LOCKED_IN_STATE = "LockedIn";
    private const string SPREAD_OUT_STATE = "SpreadOut";
    private const string RETURNING_STATE = "Returning";
    private const string VELOCITY_KEY = "normal";

    private void TimeForStateSwitch()
    {
        _stateSwitch = true;
    }

    private void SubBossKilled(Damagable died)
    {
        this.SubBosses.Remove(died.gameObject);

        if (this.SubBosses.Count == 0)
        {
            this.EndFlowObject.GetComponent<WinCondition>().EndLevel();
        }
    }

    private string LockedInUpdate()
    {
        float additionalAngle = this.RotationSpeed * Time.deltaTime;
        _currentAngle = (_currentAngle + additionalAngle * _rotationDirection) % 360.0f;
        this.transform.localRotation = Quaternion.AngleAxis(_currentAngle, _rotationAxis);

        return !_stateSwitch ? LOCKED_IN_STATE : SPREAD_OUT_STATE;
    }

    private void EnterLockedIn()
    {
        _rotationDirection = this.PossibleRotationDirections[Random.Range(0, this.PossibleRotationDirections.Length)];
        Vector2 velocity = this.StartingVelocities[Random.Range(0, this.StartingVelocities.Length)];
        _actor.SetVelocityModifier(VELOCITY_KEY, new VelocityModifier(this.StartingSpeed * velocity.normalized, VelocityModifier.CollisionBehavior.bounce));
        _timedCallbacks.AddCallback(this, this.TimeForStateSwitch, this.TimeLockedIn);
    }

    private void ExitLockedIn()
    {
        _stateSwitch = false;
        _actor.Velocity = Vector2.zero;
        _actor.RemoveVelocityModifier(VELOCITY_KEY);
        _currentAngle = 0.0f;
        this.transform.localRotation = Quaternion.identity;
    }

    private string SpreadOutUpdate()
    {
        return !_stateSwitch ? SPREAD_OUT_STATE : RETURNING_STATE;
    }

    private void EnterSpreadOut()
    {
        foreach (GameObject subBoss in this.SubBosses)
        {
            Vector2 velocity = this.StartingVelocities[Random.Range(0, this.StartingVelocities.Length)];
            Actor2D actor = subBoss.GetComponent<Actor2D>();
            actor.HaltMovementMask |= this.LevelGeomLayerMask;
            actor.CollisionMask |= this.LevelGeomLayerMask;
            actor.SetVelocityModifier(VELOCITY_KEY, new VelocityModifier(Random.Range(this.SubBossMinSpeed, this.SubBossMaxSpeed) * velocity.normalized, VelocityModifier.CollisionBehavior.bounce));
            subBoss.GetComponent<Rotation>().IsRotating = true;
        }

        _timedCallbacks.AddCallback(this, this.TimeForStateSwitch, this.TimeSpreadOut);
    }

    private void ExitSpreadOut()
    {
        _stateSwitch = false;

        foreach (GameObject subBoss in this.SubBosses)
        {
            Vector2 velocity = this.StartingVelocities[Random.Range(0, this.StartingVelocities.Length)];
            Actor2D actor = subBoss.GetComponent<Actor2D>();
            actor.Velocity = Vector2.zero;
            actor.HaltMovementMask &= ~this.LevelGeomLayerMask;
            actor.CollisionMask &= ~this.LevelGeomLayerMask;
            actor.RemoveVelocityModifier(VELOCITY_KEY);
        }
    }

    private string ReturningUpdate()
    {
        bool done = true;
        for (int i = 0; i < this.SubBosses.Count; ++i)
        {
            GameObject subBoss = this.SubBosses[i];
            subBoss.transform.localPosition = Vector2.Lerp(subBoss.transform.localPosition, _startingPositions[i], (this.ReturningLerpDistance * Time.deltaTime) / Vector2.Distance(subBoss.transform.localPosition, _startingPositions[i]));
            done &= (Vector3.Distance(subBoss.transform.localPosition, _startingPositions[i]) < 0.1f);
        }
        return !done ? RETURNING_STATE : LOCKED_IN_STATE;
    }

    private void EnterReturning()
    {
    }

    private void ExitReturning()
    {
        foreach (GameObject subBoss in this.SubBosses)
        {
            Rotation rotation = subBoss.GetComponent<Rotation>();
            rotation.IsRotating = false;
            rotation.ResetRotation();
        }
    }
}
