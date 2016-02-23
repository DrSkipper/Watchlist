using UnityEngine;

public class BossYoungSubBehavior : VoBehavior
{
    public LerpMovement.LerpFinishedCallback ReturnHomeCallback;
    public float PreferredMinDistance = 200.0f;
    public int AttemptsAtPreferred = 3;

    void Awake()
    {
        _lerpMovement = this.GetComponent<LerpMovement>();
    }

    void Start()
    {
        _home = new IntegerVector(this.transform.position);
        _lerpMovement.AddCallback(movementFinished);
    }

    public void BeginAttackPathing(IntegerVector lowerLeft, IntegerVector upperRight)
    {
        _attacking = true;
        _lowerLeft = lowerLeft;
        _upperRight = upperRight;
        seekAttackPoint();
    }

    public void StopAttackingAndReturnHome()
    {
        _attacking = false;
        _lerpMovement.BeginMovement(_home);
    }

    /**
     * Private
     */
    private IntegerVector _lowerLeft;
    private IntegerVector _upperRight;
    private IntegerVector _home;
    private LerpMovement _lerpMovement;
    private bool _attacking;

    private void seekAttackPoint()
    {
        IntegerVector target = new IntegerVector(Random.Range(_lowerLeft.X, _upperRight.X), Random.Range(_lowerLeft.Y, _upperRight.Y));

        int i = 0;
        while (i < this.AttemptsAtPreferred && Vector2.Distance(this.transform.position, target) < this.PreferredMinDistance)
        {
            target = new IntegerVector(Random.Range(_lowerLeft.X, _upperRight.X), Random.Range(_lowerLeft.Y, _upperRight.Y));
            ++i;
        }
        
        _lerpMovement.BeginMovement(target);
    }

    private void movementFinished(GameObject go)
    {
        if (_attacking)
            seekAttackPoint();
        else if (this.ReturnHomeCallback != null)
            this.ReturnHomeCallback(go);
    }
}
