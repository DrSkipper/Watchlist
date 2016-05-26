using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(TimedCallbacks))]
[RequireComponent(typeof(Rotation))]
[RequireComponent(typeof(LerpMovement))]
public class BossMasterMainBehavior : VoBehavior
{
    public List<Transform> Targets;
    public List<BossMasterGroupBehavior> BossGroups;
    public List<BossMasterSubBehavior> BossSubs;
    public List<Damagable> Shooters;
    public GameObject LeftEye;
    public GameObject RightEye;
    public WinCondition WinCondition;
    public float ReturnHomeTime = 0.2f;
    public float HomeStateDuration = 4.0f;
    public float TransitionFirstPartTime = 0.2f;
    public float TransitionSecondPartTime = 0.2f;
    public float EnterAttackTime = 0.2f;
    public float AttackStateDuration = 4.0f;
    public float BufferTime = 0.1f;
    public float AttackDelay = 0.1f;
    public float AngleOffset = 0.0f;
    public float MaxSpeed = 50.0f;
    public float TargetDistance = 100.0f;
    public float LeftEyePanStartRotation = 135.0f;
    public float RightEyePanStartRotation = 45.0f;
    public float LeftEyePanEndRotation = -100.0f;
    public float RightEyePanEndRotation = -80.0f;
    public float EyePanPrepareRotationSpeed = 360.0f;
    public float EyePanRotationSpeed = 90.0f;
    public float EyeShootDelay = 0.5f;
    public float EyePanDelay = 0.5f;
    public float InitialDelay = 1.5f;
    public float DeathDelay = 0.5f;
    public float SubBossDeathSpacing = 0.1f;
    public float EndLevelDelay = 1.0f;
    public Rotation EyeAttackRotation;
    public Transform LeftEyeHome;
    public Transform RightEyeHome;
    public Transform LeftEyeAttack;
    public Transform RightEyeAttack;
    public float LeftEyeAttackStartRotation = 180.0f;
    public float RightEyeAttackStartRotation = 0.0f;

    void Awake()
    {
        _timedCallbacks = this.GetComponent<TimedCallbacks>();
        _rotation = this.GetComponent<Rotation>();
        _actor = this.GetComponent<Actor2D>();
        GlobalEvents.Notifier.Listen(PlayerSpawnedEvent.NAME, this, playerSpawned);
    }

    void Start()
    {
        for (int i = 0; i < this.BossGroups.Count; ++i)
        {
            this.BossGroups[i].Shooter.GetComponent<Damagable>().OnDeathCallbacks.Add(shooterDeath);
        }

        for (int i = 0; i < this.BossSubs.Count; ++i)
        {
            this.BossSubs[i].GetComponent<Damagable>().OnDeathCallbacks.Add(subDeath);
        }

        this.GetComponent<BossHealth>().DeathCallbacks.Add(onDeath);

        _stateMachine = new FSMStateMachine();
        _stateMachine.AddState(HOME_STATE, updateHome, enterHome, exitHome);
        _stateMachine.AddState(TRANSITION_STATE, updateTransition, enterTransition, exitTransition);
        _stateMachine.AddState(ATTACK_STATE, updateAttack, enterAttack, exitAttack);
        _timedCallbacks.AddCallback(this, begin, this.InitialDelay);
    }

    void Update()
    {
        if (_began && !_dead)
            _stateMachine.Update();
    }

    /**
     * Private
     */
    private FSMStateMachine _stateMachine;
    private TimedCallbacks _timedCallbacks;
    private Actor2D _actor;
    private Rotation _rotation;
    private bool _switchState;
    private bool _seeking;
    private bool _began;
    private bool _dead;
    private bool _leftEyePrepared;
    private bool _rightEyePrepared;

    private const string HOME_STATE = "home";
    private const string TRANSITION_STATE = "transition";
    private const string ATTACK_STATE = "attack";
    private const float WIGGLE_ROOM = 10.0f;

    private void begin()
    {
        _began = true;
        enterHome();
        _stateMachine.BeginWithInitialState(HOME_STATE);
        _seeking = true;
    }
    
    private void onDeath(int hp)
    {
        _dead = true;
        while (this.Shooters.Count > 0)
        {
            this.Shooters[this.Shooters.Count - 1].Kill(0.0f);
        }
        for (int i = 0; i < this.BossSubs.Count; ++i)
        {
            this.BossSubs[i].GetComponent<LerpMovement>().HaltMovement();
        }
        _timedCallbacks.AddCallback(this, triggerDeaths, this.DeathDelay);
        _timedCallbacks.AddCallback(this, this.WinCondition.EndLevel, this.EndLevelDelay);
    }

    private void triggerDeaths()
    {
        for (int i = 0; i < this.BossSubs.Count; ++i)
        {
            _timedCallbacks.AddCallback(this, killSubBoss, i * this.SubBossDeathSpacing);
        }
    }

    private void killSubBoss()
    {
        if (this.BossSubs.Count > 0)
            this.BossSubs[Random.Range(0, this.BossSubs.Count)].GetComponent<Damagable>().Kill(0.0f);
    }

    private void switchState()
    {
        _switchState = true;
    }

    private void shooterDeath(Damagable shooter)
    {
        this.Shooters.Remove(shooter);
        this.BossGroups.Remove(shooter.transform.parent.GetComponent<BossMasterGroupBehavior>());
    }

    private void subDeath(Damagable sub)
    {
        this.BossSubs.Remove(sub.GetComponent<BossMasterSubBehavior>());
    }

    private void playerSpawned(LocalEventNotifier.Event e)
    {
        Transform playerTransform = (e as PlayerSpawnedEvent).PlayerObject.transform;
        this.Targets.Add(playerTransform);
        for (int i = 0; i < this.BossGroups.Count; ++i)
        {
            this.BossGroups[i].Targets.Add(playerTransform);
        }
    }

    private void enterHome()
    {
        _switchState = false;
        for (int i = 0; i < this.BossGroups.Count; ++i)
        {
            this.BossGroups[i].GoHome(this.ReturnHomeTime);
        }

        for (int i = 0; i < this.BossSubs.Count; ++i)
        {
            if (this.BossSubs[i] != null)
                this.BossSubs[i].GoHome(this.ReturnHomeTime);
        }

        this.LeftEye.GetComponent<LerpMovement>().BeginMovement(this.LeftEyeHome.transform.position, this.ReturnHomeTime);
        this.RightEye.GetComponent<LerpMovement>().BeginMovement(this.RightEyeHome.transform.position, this.ReturnHomeTime);

        _timedCallbacks.AddCallback(this, switchState, this.HomeStateDuration);
        _timedCallbacks.AddCallback(this, beginSeeking, this.ReturnHomeTime + this.BufferTime);
    }

    private void beginSeeking()
    {
        _seeking = true;
        LerpRotation leftRotation = this.LeftEye.GetComponent<LerpRotation>();
        LerpRotation rightRotation = this.RightEye.GetComponent<LerpRotation>();
        leftRotation.AddCallback(eyePrepared);
        rightRotation.AddCallback(eyePrepared);
        leftRotation.TargetRotation = this.LeftEyePanStartRotation;
        rightRotation.TargetRotation = this.RightEyePanStartRotation;
        leftRotation.RotationSpeed = this.EyePanPrepareRotationSpeed;
        rightRotation.RotationSpeed = this.EyePanPrepareRotationSpeed;
        leftRotation.LerpToTargetRotation();
        rightRotation.LerpToTargetRotation();
    }

    private void eyePrepared(GameObject go)
    {
        if (go == this.LeftEye)
            _leftEyePrepared = true;
        else
            _rightEyePrepared = true;
        go.GetComponent<LerpRotation>().ClearCallbacks();
    }

    private string updateHome()
    {
        if (_seeking)
        {
            // Find center of targets
            Vector2 avgPosition = Vector2.zero;
            int count = 0;
            for (int i = 0; i < this.Targets.Count; ++i)
            {
                if (this.Targets[i] != null)
                {
                    avgPosition += (Vector2)this.Targets[i].position;
                    ++count;
                }
            }

            if (count > 0)
                avgPosition /= count;
            
            // Lerp rotation to point at center of targets
            Vector2 aimAxis = avgPosition - (Vector2)this.transform.position;
            float distance = aimAxis.magnitude;
            aimAxis.Normalize();

            float targetAngle = Vector2.Angle(Vector2.up, aimAxis);
            if (aimAxis.x < 0.0f)
            {
                targetAngle = -targetAngle;
                targetAngle -= this.AngleOffset;
            }
            else
            {
                targetAngle += this.AngleOffset;
            }

            float currentAngle = _rotation.GetAngle();
            _rotation.SetAngle(Mathf.MoveTowardsAngle(currentAngle, targetAngle, _rotation.RotationSpeed * Time.deltaTime));

            // Lerp movement toward target distance from center of targets
            if (Mathf.Abs(distance - this.TargetDistance) > WIGGLE_ROOM)
            {
                if (distance < this.TargetDistance)
                    aimAxis *= -1;
                _actor.Velocity = aimAxis * this.MaxSpeed;
            }
            else
            {
                _actor.Velocity = Vector2.zero;
            }

            // Handle eyes
            if (_leftEyePrepared && _rightEyePrepared)
            {
                _leftEyePrepared = false;
                _rightEyePrepared = false;
                _timedCallbacks.AddCallback(this, beginEyeShooting, this.EyeShootDelay);
            }
        }

        return !_switchState ? HOME_STATE : TRANSITION_STATE;
    }

    private void beginEyeShooting()
    {
        this.LeftEye.GetComponent<WeaponAutoFire>().Paused = false;
        this.RightEye.GetComponent<WeaponAutoFire>().Paused = false;
        _timedCallbacks.AddCallback(this, beginEyePan, this.EyePanDelay);
    }

    private void beginEyePan()
    {
        LerpRotation leftRotation = this.LeftEye.GetComponent<LerpRotation>();
        LerpRotation rightRotation = this.RightEye.GetComponent<LerpRotation>();
        leftRotation.AddCallback(eyePanDone);
        rightRotation.AddCallback(eyePanDone);
        leftRotation.TargetRotation = this.LeftEyePanEndRotation;
        rightRotation.TargetRotation = this.RightEyePanEndRotation;
        leftRotation.RotationSpeed = this.EyePanRotationSpeed;
        rightRotation.RotationSpeed = this.EyePanRotationSpeed;
        leftRotation.LerpToTargetRotation();
        rightRotation.LerpToTargetRotation();
    }

    private void eyePanDone(GameObject go)
    {
        go.GetComponent<LerpRotation>().ClearCallbacks();
        go.GetComponent<WeaponAutoFire>().Paused = true;
    }

    private void exitHome()
    {
        _seeking = false;
        _actor.Velocity = Vector2.zero;
    }

    private void enterTransition()
    {
        _switchState = false;
        this.EyeAttackRotation.SetAngle(0.0f);

        for (int i = 0; i < this.BossGroups.Count; ++i)
        {
            this.BossGroups[i].EnterTransitionPattern(this.TransitionFirstPartTime);
        }

        _timedCallbacks.AddCallback(this, secondStageTransition, this.TransitionFirstPartTime + this.BufferTime);
    }

    private void secondStageTransition()
    {
        for (int i = 0; i < this.BossSubs.Count; ++i)
        {
            if (this.BossSubs[i] != null)
                this.BossSubs[i].GoToAttackPosition(this.TransitionSecondPartTime);
        }

        this.LeftEye.transform.parent = this.EyeAttackRotation.transform;
        this.RightEye.transform.parent = this.EyeAttackRotation.transform;
        this.LeftEye.GetComponent<LerpMovement>().BeginMovement(this.LeftEyeAttack.transform.position, this.TransitionSecondPartTime);
        this.RightEye.GetComponent<LerpMovement>().BeginMovement(this.RightEyeAttack.transform.position, this.TransitionSecondPartTime);
        LerpRotation leftRotation = this.LeftEye.GetComponent<LerpRotation>();
        LerpRotation rightRotation = this.RightEye.GetComponent<LerpRotation>();
        leftRotation.TargetRotation = this.LeftEyeAttackStartRotation;
        rightRotation.TargetRotation = this.RightEyeAttackStartRotation;
        leftRotation.RotationSpeed = this.EyePanPrepareRotationSpeed;
        rightRotation.RotationSpeed = this.EyePanPrepareRotationSpeed;
        leftRotation.LerpToTargetRotation();
        rightRotation.LerpToTargetRotation();

        _timedCallbacks.AddCallback(this, switchState, this.TransitionSecondPartTime + this.BufferTime);
    }

    private string updateTransition()
    {
        return !_switchState ? TRANSITION_STATE : ATTACK_STATE;
    }

    private void exitTransition()
    {
    }

    private void enterAttack()
    {
        _switchState = false;

        for (int i = 0; i < this.BossGroups.Count; ++i)
        {
            this.BossGroups[i].EnterAttackPattern(this.EnterAttackTime, this.AttackDelay);
        }
        
        _timedCallbacks.AddCallback(this, beginEyeFiring, this.EnterAttackTime);
        _timedCallbacks.AddCallback(this, beginEyeRotating, this.EnterAttackTime * 2.0f);
        _timedCallbacks.AddCallback(this, switchState, this.AttackStateDuration);
    }

    private void beginEyeFiring()
    {
        this.LeftEye.GetComponent<WeaponAutoFire>().Paused = false;
        this.RightEye.GetComponent<WeaponAutoFire>().Paused = false;
    }

    private void beginEyeRotating()
    {
        this.EyeAttackRotation.RotationSpeed = -this.EyeAttackRotation.RotationSpeed;
        this.EyeAttackRotation.IsRotating = true;
    }

    private string updateAttack()
    {
        return !_switchState ? ATTACK_STATE : HOME_STATE;
    }

    private void exitAttack()
    {
        this.EyeAttackRotation.IsRotating = false;
        this.LeftEye.transform.parent = this.transform;
        this.RightEye.transform.parent = this.transform;
        this.LeftEye.GetComponent<WeaponAutoFire>().Paused = true;
        this.RightEye.GetComponent<WeaponAutoFire>().Paused = true;
    }
}
