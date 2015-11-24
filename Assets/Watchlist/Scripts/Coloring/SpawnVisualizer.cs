using UnityEngine;

public class SpawnVisualizer : VoBehavior
{
    public float AnimationDuration = 1.0f;
    public float RotationSpeed = 720.0f;
    public float MaxScale = 5.0f;

    void Start()
    {
        this.transform.localScale = Vector3.zero;
        _scalePerSecond = (this.MaxScale / this.AnimationDuration) * 2.0f;
    }

    void Update()
    {
        _duration += Time.deltaTime;
        _angle += this.RotationSpeed * Time.deltaTime;

        this.transform.localRotation = Quaternion.AngleAxis(_angle, new Vector3(0, 0, 1));
        this.transform.localScale = new Vector3(_scale, _scale, _scale);

        if (_duration < this.AnimationDuration / 2.0f)
        {
            _scale += _scalePerSecond * Time.deltaTime;
            _scale = Mathf.Min(_scale, this.MaxScale);
        }
        else
        {
            _scale -= _scalePerSecond * Time.deltaTime;
            _scale = Mathf.Max(0.0f, _scale);

            if (_duration >= this.AnimationDuration)
                Destroy(this.gameObject);
        }
    }

    /**
     * Private
     */
    private float _duration;
    private float _angle;
    private float _scale;
    private float _scalePerSecond;
}
