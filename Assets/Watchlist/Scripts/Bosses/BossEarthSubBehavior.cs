using UnityEngine;

[RequireComponent(typeof(LerpMovement))]
public class BossEarthSubBehavior : VoBehavior
{
    [HideInInspector]
    public Transform[] PathPoints;
    [HideInInspector]
    public int CurrentPathPoint = 0;
    public float TimeToReachPoint = 1.0f;

    void Awake()
    {
        _lerpMovement = this.GetComponent<LerpMovement>();
    }

    public void PathToNextPoint()
    {
        this.CurrentPathPoint += 1;
        if (this.CurrentPathPoint >= this.PathPoints.Length)
            this.CurrentPathPoint = 0;
        
        _lerpMovement.BeginMovement(this.PathPoints[this.CurrentPathPoint].position, this.TimeToReachPoint);
    }

    /**
     * Private
     */
    private LerpMovement _lerpMovement;
}
