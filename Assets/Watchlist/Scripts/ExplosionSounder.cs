using UnityEngine;

public class ExplosionSounder : VoBehavior
{
    public float Cooldown = 0.1f;

    void Awake()
    {
        _sounder = this;
        _audio = this.GetComponent<AudioSource>();
    }

    public static void Detonate()
    {
        if (_sounder._cooldown <= 0.0f)
            _sounder.detonate();
    }

    void Update()
    {
        if (_cooldown >= 0.0f)
            _cooldown -= Time.deltaTime;
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        _sounder = null;
    }

    /**
     * Private
     */
    private static ExplosionSounder _sounder;
    private float _cooldown;
    private AudioSource _audio;

    private void detonate()
    {
        _cooldown = this.Cooldown;
        _audio.Play();
    }
}
