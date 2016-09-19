using UnityEngine;

public class GibsBehavior : VoBehavior
{
    public GameObject[] Gibs;
    public AllegianceInfo AllegianceInfo;
    public AudioSource AudioSource;
    public Vector2 ImpactVector = Vector2.zero;
    public float Knockback = 250.0f;
    public float KnockbackModifier = 0.4f;
    public float RandomInfluence = 20.0f;
    public float OrientationInfluence = 50.0f;
    public float Lifetime = 1.0f;
    public string ObjectPoolKey = "gibs";

    void Start()
    {
        _startingPositions = new Vector2[this.Gibs.Length];
        for (int i = 0; i < this.Gibs.Length; ++i)
        {
            GameObject gib = this.Gibs[i];
            _startingPositions[i] = gib.transform.localPosition;
            setupGib(gib);
        }
        _lifetimeTimer = this.Lifetime;
    }

    void Update()
    {
        if (!_soundPlayed && this.AudioSource != null)
        {
            _soundPlayed = true;
            this.AudioSource.Play();
        }

        if (_lifetimeTimer <= 0.0f)
        {
            reset();
            ObjectPools.Instance.ReturnPooledObject(this.ObjectPoolKey, this.gameObject);
        }
        else
        {
            _lifetimeTimer -= Time.deltaTime;
        }
    }

    /**
     * Private
     */
    private float _lifetimeTimer;
    private Vector2[] _startingPositions;
    private bool _soundPlayed;

    private void setupGib(GameObject gib)
    {
        float randomAngle = Random.Range(0.0f, 2.0f * Mathf.PI);
        Vector2 random = new Vector2(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle));
        Vector2 orientation = (Vector2)(gib.transform.position - this.transform.position);
        if (orientation.magnitude < 0.5f)
            orientation = random;
        else
            orientation.Normalize();
        Vector2 final = (random * this.RandomInfluence) + (orientation * this.OrientationInfluence) + (this.ImpactVector.normalized * this.Knockback * this.KnockbackModifier);
        gib.GetComponent<Actor2D>().SetVelocityModifier("gib", new VelocityModifier(final, VelocityModifier.CollisionBehavior.bounce));
        gib.GetComponent<AllegianceColorizer>().AllegianceInfo = this.AllegianceInfo;
    }

    private void reset()
    {
        for (int i = 0; i < this.Gibs.Length; ++i)
        {
            GameObject gib = this.Gibs[i];
            gib.transform.localPosition = new Vector3(_startingPositions[i].x, _startingPositions[i].y, gib.transform.localPosition.z);
            setupGib(gib);
        }
        _lifetimeTimer = this.Lifetime;
        _soundPlayed = false;
    }
}
