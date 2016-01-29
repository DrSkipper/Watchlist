using UnityEngine;

public class BossOtherSubBehavior : VoBehavior
{
    [System.Serializable]
    public struct ActionParameters
    {
        public float TargetRotation;
        public Transform TargetPosition;
    }

    public ActionParameters[] ActionData;

    void Awake()
    {
        _normalPosition = this.transform.position;
        _lerpRotation = this.GetComponent<LerpRotation>();
        _lerpMovement = this.GetComponent<LerpMovement>();
    }

    void Start()
    {
        _lerpMovement.AddCallback(moveLerpFinished);
    }

    public void BeginAction(int actionNum)
    {
        _currentAction = this.ActionData[actionNum];
        _lerpRotation.TargetRotation = _currentAction.Value.TargetRotation;
        _lerpRotation.LerpToTargetRotation();
    }

    public void BeginMovement()
    {
        _movingOut = true;
        _lerpMovement.BeginMovement(_currentAction.Value.TargetPosition.position);
    }

    /**
     * Private
     */
    private Vector2 _normalPosition;
    private LerpRotation _lerpRotation;
    private LerpMovement _lerpMovement;
    private ActionParameters? _currentAction = null;
    private bool _movingOut;
    private bool _movingBack;

    private void moveLerpFinished(GameObject gameObject)
    {
        if (_movingOut)
        {
            _movingOut = false;
            _movingBack = true;
            _lerpMovement.BeginMovement(_normalPosition);
        }
        else if (_movingBack)
        {
            _movingBack = false;
        }
    }
}
