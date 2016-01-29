using UnityEngine;

public class Rotation : VoBehavior
{
    public Vector3 Axis = new Vector3(0, 0, -1);
    public float InitalRotation = 0.0f;
    public float RotationSpeed = 1.0f;
    public bool IsRotating = true;

    void Start()
    {
        _angle = this.InitalRotation;
	}
	
	void Update()
    {
        if (IsRotating)
        {
            this.transform.localRotation = Quaternion.AngleAxis(_angle, this.Axis);
            _angle += this.RotationSpeed * Time.deltaTime;
        }
	}

    public void ResetRotation(bool applyReset = true)
    {
        _angle = this.InitalRotation;
        if (applyReset)
            this.transform.localRotation = Quaternion.AngleAxis(_angle, this.Axis);
    }

    public float GetAngle()
    {
        return _angle;
    }

    /**
     * Private
     */
    private float _angle;
}
