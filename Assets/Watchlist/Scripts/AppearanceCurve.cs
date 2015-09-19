using UnityEngine;

public class AppearanceCurve : VoBehavior
{
    public float AnimationDuration = 0.5f;
    public float StartAngle = 90.0f;
    public float EndAngle = 0.0f;
    public float StartScale = 0.1f;
    public float EndScale = 1.0f;
    public bool RunOnStart = true;

    void Start()
    {
        if (this.RunOnStart)
            this.Begin();
    }

    void Update()
    {
        if (!_running)
            return; //TODO - Remove and destory this component?

        if (_timeElapsed >= this.AnimationDuration)
        {
            _timeElapsed = this.AnimationDuration;
            _running = false;
        }

        float angle = Mathf.Lerp(this.StartAngle, this.EndAngle, _timeElapsed / this.AnimationDuration);
        this.transform.localRotation = Quaternion.AngleAxis(angle, new Vector3(1, 1, 0));
        float scale = Mathf.Lerp(this.StartScale, this.EndScale, _timeElapsed / this.AnimationDuration);
        this.transform.localScale = new Vector3(scale, scale, scale);
        _timeElapsed += Time.deltaTime;
    }

    public void Begin()
    {
        _running = true;
        _timeElapsed = 0.0f;
        _totalDistance = Mathf.Abs(this.EndAngle - this.StartAngle);
        this.transform.localRotation = Quaternion.AngleAxis(this.StartAngle, new Vector3(1, 1, 0));
    }

    /**
     * Private
     */
    private float _timeElapsed;
    private float _totalDistance;
    private bool _running;
}
