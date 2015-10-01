using UnityEngine;
using System.Collections.Generic;

public class Actor2D : VoBehavior
{
    public Vector2 Velocity;
    public LayerMask HaltMovementMask;
    public LayerMask CollisionMask;

    void Update()
    {
        if (this.Velocity.x != 0.0f || this.Velocity.y != 0.0f)
        {
            RaycastHit2D? haltingHit = null;

            BoxCollider2D boxCollider = this.collider2D as BoxCollider2D;
            Vector2 position = new Vector2(this.transform.position.x, this.transform.position.y);
            Vector2 direction = this.Velocity.normalized;

            RaycastHit2D[] hits = Physics2D.BoxCastAll(position, boxCollider.size, 0.0f, direction, this.Velocity.magnitude * Time.deltaTime);

            foreach (RaycastHit2D hit in hits)
            {
                if ((hit.collider.gameObject.layer & this.HaltMovementMask) != 0)
                {
                    haltingHit = hit;
                    break;
                }
            }

            if (haltingHit.HasValue)
            {

            }
            else
            {
                this.transform.position = new Vector3(Mathf.Round(position.x + this.Velocity.x), Mathf.Round(position.y + this.Velocity.y), this.transform.position.z);
            }
        }
    }
}
