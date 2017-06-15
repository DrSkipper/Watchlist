using UnityEngine;

public class Rotator : MonoBehaviour
{
    public float RotationSpeed = 5.0f;
    public float MaxAngle = 30.0f;

    void Update()
    {
        if (!PauseController.IsPaused())
            rotate();
    }

    private float _currentAngle;
    private int _angleDirection = 1;

    private void rotate()
    {
        _currentAngle += this.RotationSpeed * Time.deltaTime * _angleDirection;
        if (Mathf.Abs(_currentAngle) > this.MaxAngle)
            _angleDirection = -_angleDirection;
        this.transform.localRotation = Quaternion.AngleAxis(_currentAngle, Vector3.forward);
    }
}
