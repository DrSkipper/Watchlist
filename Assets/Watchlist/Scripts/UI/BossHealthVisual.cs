using UnityEngine;

[RequireComponent(typeof(TimedCallbacks))]
public class BossHealthVisual : MonoBehaviour
{
    public BossHealth Health;
    public int TargetWidth = 400;
    public int LargeHealthChangeCutoff = 7;
    public float SmallHealthChangeLerpSpeed = 20.0f;
    public float LargeHealthChangeLerpSpeed = 120.0f;
    public float InitialDelay = 0.0f;

    void Awake()
    {
        _rectTransform = this.GetComponent<RectTransform>();
        _targetWidth = this.TargetWidth;
    }

    void Start()
    {
        this.Health.Callbacks.Add(updateHealth);
        this.GetComponent<TimedCallbacks>().AddCallback(this, begin, this.InitialDelay);
    }

    void Update()
    {
        if (_begun)
        {
            float currentWidth = _rectTransform.sizeDelta.x;
            float speed = Mathf.Abs(currentWidth - _targetWidth) > this.LargeHealthChangeCutoff ? this.LargeHealthChangeLerpSpeed : this.SmallHealthChangeLerpSpeed;
            currentWidth = Mathf.MoveTowards(currentWidth, _targetWidth, speed * Time.deltaTime);
            _rectTransform.sizeDelta = new Vector2(currentWidth, _rectTransform.sizeDelta.y);
        }
    }

    /**
     * Private
     */
    private RectTransform _rectTransform;
    private int _targetWidth;
    private bool _begun;

    private void begin()
    {
        _begun = true;
    }

    private void updateHealth(int health)
    {
        float percentHealth = (float)health / (float)this.Health.MaxHealth;
        _targetWidth = Mathf.RoundToInt((float)this.TargetWidth * percentHealth);
    }
}
