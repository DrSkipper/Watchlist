using UnityEngine;

public class GibsBehavior : VoBehavior
{
    public GameObject[] Gibs;
    public Vector2 ImpactVector = Vector2.zero;
    public float Knockback = 250.0f;
    public float KnockbackModifier = 0.4f;
    public float RandomInfluence = 20.0f;
    public float OrientationInfluence = 50.0f;
    public float Lifetime = 1.0f;
    public bool DestroyWhenDone = true;

    void Start()
    {
        foreach (GameObject gib in this.Gibs)
        {
            float randomAngle = Random.Range(0.0f, 2.0f * Mathf.PI);
            Vector2 random = new Vector2(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle));
            Vector2 orientation = ((Vector2)(gib.transform.position - this.transform.position)).normalized;
            Vector2 final = (random * this.RandomInfluence) + (orientation * this.OrientationInfluence) + (this.ImpactVector.normalized * this.Knockback * this.KnockbackModifier);
            gib.GetComponent<Actor2D>().SetVelocityModifier("gib", new VelocityModifier(final, VelocityModifier.CollisionBehavior.bounce));
        }

        _lifetimeTimer = this.Lifetime;
    }

    void Update()
    {
        if (_lifetimeTimer <= 0.0f)
        {
            if (this.DestroyWhenDone)
            {
                Destroy(this.gameObject);
            }
            else
            {
                Destroy(this);
                foreach (GameObject gib in this.Gibs)
                {
                    Destroy(gib.GetComponent<Actor2D>());
                    Destroy(gib.GetComponent<ActorFriction>());
                }
            }
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
}
