using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Rotation))]
public class LerpRotation : VoBehavior
{
    public float TargetRotation = 0.0f;
    public float RotationSpeed = 180.0f;

    public delegate void LerpFinishedCallback(GameObject gameObject);

    void Awake()
    {
        _rotation = this.GetComponent<Rotation>();
        _callbacks = new List<LerpFinishedCallback>();
    }

    public void LerpToTargetRotation()
    {
        _angle = _rotation.GetAngle() % 360.0f;
        _rotation.ResetRotation(false);
        _rotation.IsRotating = false;
        _isLerping = true;
    }

    public void AddCallback(LerpFinishedCallback callback)
    {
        _callbacks.Add(callback);
    }

    public void ClearCallbacks()
    {
        _callbacks.Clear();
    }

    void Update()
    {
        if (_isLerping)
        {
            _angle = Mathf.MoveTowardsAngle(_angle, this.TargetRotation, this.RotationSpeed * Time.deltaTime);
            
            if (Mathf.Abs(Mathf.DeltaAngle(this.TargetRotation, _angle)) < 0.1f)
            {
                _angle = this.TargetRotation;
                _isLerping = false;
                _rotation.InitalRotation = _angle;
                _rotation.ResetRotation(false);
            }

            this.transform.localRotation = Quaternion.AngleAxis(_angle, _rotation.Axis);

            if (!_isLerping)
            {
                foreach (LerpFinishedCallback callback in _callbacks)
                {
                    callback(this.gameObject);
                }
            }
        }
    }

    /**
     * Private
     */
    private Rotation _rotation;
    private bool _isLerping;
    private float _angle;
    private List<LerpFinishedCallback> _callbacks;
}
