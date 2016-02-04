using UnityEngine;

[RequireComponent(typeof(LerpMovement))]
public class BossEarthSubBehavior : VoBehavior
{
    [HideInInspector]
    public Transform[] PathPoints;
    [HideInInspector]
    public int CurrentPathPoint = 0;

    void Awake()
    {
        _lerpMovement = this.GetComponent<LerpMovement>();
    }

    public void PathToNextPoint()
    {
        this.CurrentPathPoint += 1;
        if (this.CurrentPathPoint >= this.PathPoints.Length)
            this.CurrentPathPoint = 0;

        _lerpMovement.TargetPosition = this.PathPoints[this.CurrentPathPoint].position;
        _lerpMovement.IsMoving = true;
    }

    /**
     * Private
     */
    private LerpMovement _lerpMovement;
}
