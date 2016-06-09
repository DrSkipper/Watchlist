using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(TimedCallbacks))]
public class CouncilChairController : VoBehavior
{
    public CouncilChair[] Chairs;
    public float ShakeDelay = 1.0f;
    public float FadeDelay = 2.0f;
    public float FadeLength = 5.0f;
    public float ShakeMagnitude = 70.0f;
    public float ShakeMagnitudeIncrease = 50.0f;
    public float ShakeCooldown = 0.3f;
    public float SceneChangeDelay = 11.0f;
    public string Destination = "Credits";
    public Image FadePanel;

    void Awake()
    {
        _timedCallbacks = this.GetComponent<TimedCallbacks>();
        _fadeColor = this.FadePanel.color;
    }
    
    void Update()
    {
        bool filled = true;
        for (int i = 0; i < this.Chairs.Length; ++i)
        {
            if (!this.Chairs[i].ChairFilled)
            {
                filled = false;
                break;
            }
        }

        if (filled && !_sequenceBegun)
        {
            _sequenceBegun = true;
            _timedCallbacks.AddCallback(this, beginShake, this.ShakeDelay);
            _timedCallbacks.AddCallback(this, beginFade, this.FadeDelay);
            _timedCallbacks.AddCallback(this, nextScene, this.SceneChangeDelay);
        }

        if (_shake)
        {
            _shakeCooldown -= Time.deltaTime;
            if (_shakeCooldown <= 0.0f)
            {
                _shakeCooldown = this.ShakeCooldown;
                Camera.main.GetComponent<ShakeHandler>().ApplyImpact(_shakeMagnitude);
                _shakeMagnitude += this.ShakeMagnitudeIncrease;
            }
        }

        if (_fade)
        {
            _fadeColor.a = Mathf.Min(_fadeColor.a += _fadeSpeed * Time.deltaTime, 1.0f);
        }
        else
        {
            _fadeColor.a = 0.0f;
        }

        this.FadePanel.color = _fadeColor;
    }

    /**
     * Private
     */
    private TimedCallbacks _timedCallbacks;
    private bool _sequenceBegun;
    private bool _shake;
    private float _shakeCooldown;
    private float _shakeMagnitude;
    private bool _fade;
    private Color _fadeColor;
    private float _fadeSpeed;

    private void beginShake()
    {
        _shakeMagnitude = this.ShakeMagnitude;
        _shake = true;
    }

    private void beginFade()
    {
        _fade = true;
        _fadeSpeed = 1.0f / this.FadeLength; 
    }

    private void nextScene()
    {
        SceneManager.LoadScene(this.Destination);
    }
}
