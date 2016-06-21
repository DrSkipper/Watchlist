using UnityEngine;
using System.Collections.Generic;

public class LookAtPlayer : VoBehavior
{
    public float LookAtRange = 250.0f;
    public LayerMask LineOfSightBlockers = 0;
    public float RotationOffset = 0.0f;

    public bool IsLookingAt { get { return _isLookingAt; } }
    public Vector2 AimAxis { get { return _aimAxis; } }

    void Start()
    {
        _targets = PlayerTargetController.Targets;
        _rotationAxis = new Vector3(0, 0, 1);
    }

    void Update()
    {
        float distance = GenericEnemy.MAX_DISTANCE;

        // Find the closest visible target
        if (_targets.Count > 0)
        {
            Vector2 ourPosition = (Vector2)this.transform.position;

            for (int i = 0; i < _targets.Count; ++i)
            {
                Transform current = _targets[i];
                Vector2 theirPosition = current.position;
                float d = Vector2.Distance(ourPosition, theirPosition);
                if (d < distance && d <= this.LookAtRange)
                {
                    CollisionManager.RaycastResult result = CollisionManager.RaycastFirst(new IntegerVector(this.transform.position), (theirPosition - (Vector2)this.transform.position).normalized, d, this.LineOfSightBlockers);

                    if (!result.Collided)
                    {
                        distance = d;
                        _aimAxis = theirPosition - ourPosition;
                    }
                }
            }
        }

        // If close enough, turn to face the target
        if (distance < this.LookAtRange)
        {
            _isLookingAt = true;

            _currentAngle = Vector2.Angle(Vector2.up, _aimAxis);
            if (_aimAxis.x > 0.0f)
                _currentAngle = -_currentAngle;
            _currentAngle += this.RotationOffset;

            this.transform.localRotation = Quaternion.AngleAxis(_currentAngle, _rotationAxis);
        }
        else
        {
            _isLookingAt = false;
        }
    }

    private List<Transform> _targets;
    private Vector3 _rotationAxis;
    private Vector2 _aimAxis;
    private float _currentAngle;
    private bool _isLookingAt;
}
