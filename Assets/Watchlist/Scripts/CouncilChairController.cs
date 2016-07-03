using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(TimedCallbacks))]
public class CouncilChairController : VoBehavior
{
    public CouncilChair[] Chairs;
    public float ShakeDelay = 1.0f;
    public float FadeDelay = 2.0f;
    public float SoundsDelay = 1.5f;
    public float FadeLength = 5.0f;
    public float ShakeMagnitude = 70.0f;
    public float ShakeMagnitudeIncrease = 50.0f;
    public float ShakeCooldown = 0.3f;
    public float SceneChangeDelay = 11.0f;
    public float SoundCooldownMax = 1.0f;
    public float SoundCooldownMinStart = 0.5f;
    public float SoundCooldownMinEnd = 0.1f;
    public float SoundCooldownMinDecrease = 0.1f;
    public float SoundCooldownMaxDecrease = 0.05f;
    public float SoundCooldownMaxEnd = 6.0f;
    public float SoundEndDelay = 9.5f;
    public string Destination = "Credits";
    public Image FadePanel;

    void Awake()
    {
        _audio = this.GetComponent<AudioSource>();
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
            GlobalEvents.Notifier.SendEvent(new PlayMusicEvent());
            _timedCallbacks.AddCallback(this, beginShake, this.ShakeDelay);
            _timedCallbacks.AddCallback(this, beginFade, this.FadeDelay);
            _timedCallbacks.AddCallback(this, nextScene, this.SceneChangeDelay);
            _timedCallbacks.AddCallback(this, beginSound, this.SoundsDelay);
            _timedCallbacks.AddCallback(this, endSound, this.SoundEndDelay);
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

        if (_sound)
        {
            _soundCooldown -= Time.deltaTime;
            if (_soundCooldown <= 0.0f)
            {
                _audio.Play();
                _soundCooldown = Random.Range(_soundMinCooldown, this.SoundCooldownMax);
                _soundMinCooldown = Mathf.Max(this.SoundCooldownMinEnd, _soundMinCooldown - this.SoundCooldownMinDecrease);
                _soundMaxCooldown = Mathf.Max(this.SoundCooldownMaxEnd, _soundMaxCooldown - this.SoundCooldownMinDecrease);
            }
        }
    }

    /**
     * Private
     */
    private TimedCallbacks _timedCallbacks;
    private AudioSource _audio;
    private bool _sequenceBegun;
    private bool _shake;
    private float _shakeCooldown;
    private float _shakeMagnitude;
    private bool _fade;
    private Color _fadeColor;
    private float _fadeSpeed;
    private float _soundCooldown;
    private bool _sound;
    private float _soundMinCooldown;
    private float _soundMaxCooldown;

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

    private void beginSound()
    {
        _sound = true;
        _soundMinCooldown = this.SoundCooldownMinStart;
        _soundMaxCooldown = this.SoundCooldownMax;
    }

    private void endSound()
    {
        _sound = false;
    }

    private void nextScene()
    {
        SceneManager.LoadScene(this.Destination);
    }
}
