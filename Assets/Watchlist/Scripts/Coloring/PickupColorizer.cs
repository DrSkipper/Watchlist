using UnityEngine;

public class PickupColorizer : VoBehavior
{
    public string ColorCategory = "pickup";
    public float HalfCycleTime = 0.4f;

    void Start()
    {
        _previousHSV = GameplayPalette.GetColorFromTag(this.ColorCategory, 0).GetHSV();
        _nextHSV = GameplayPalette.GetColorFromTag(this.ColorCategory, 1).GetHSV();
        updateColor(_previousHSV.h, _previousHSV.s, _previousHSV.v);
    }

    void Update()
    {
        _t += Time.deltaTime;

        if (_t < this.HalfCycleTime)
        {
            float t = _t / this.HalfCycleTime;
            updateColor(Mathf.Lerp(_previousHSV.h, _nextHSV.h, t), Mathf.Lerp(_previousHSV.s, _nextHSV.s, t), Mathf.Lerp(_previousHSV.v, _nextHSV.v, t));
        }
        else
        {
            updateColor(_nextHSV.h, _nextHSV.s, _nextHSV.v);
            _t = 0.0f;
            ColorExtensions.HSV temp = _nextHSV;
            _nextHSV = _previousHSV;
            _previousHSV = temp;
        }
    }

    /**
     * Private
     */
    private float _t;
    private ColorExtensions.HSV _previousHSV;
    private ColorExtensions.HSV _nextHSV;

    private void updateColor(float h, float s, float v)
    {
        this.spriteRenderer.color = ColorExtensions.ColorWithHSV(h, s, v);
    }
}
