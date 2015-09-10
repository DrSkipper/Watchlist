using UnityEngine;

public class BulletController : VoBehavior
{
    public Vector2 Velocity = new Vector2();
    public Vector3 Up = Vector3.up;
    public float Duration = 5.0f;
    
    public void Update()
    {
        this.transform.position = new Vector3(this.transform.position.x + this.Velocity.x * Time.deltaTime, 
                                              this.transform.position.y, 
                                              this.transform.position.z) + this.Up * (this.Velocity.y * Time.deltaTime);
        _lifetime += Time.deltaTime;
        if (_lifetime >= this.Duration)
            Destroy(this.gameObject);
    }

    /**
     * Private
     */
    private float _lifetime = 0.0f;
}
