using UnityEngine;
using System.Collections.Generic;

public class IntegerRectCollider : VoBehavior
{
    public IntegerVector Offset = IntegerVector.Zero;
    public IntegerVector Size = IntegerVector.Zero;
    public IntegerRect Bounds { get { return new IntegerRect(this.integerPosition + this.Offset, this.Size); } }

    public CollisionManager CollisionManager { get {
            if (_collisionManager == null)
                _collisionManager = FindObjectOfType<CollisionManager>();
            return _collisionManager;
    } }

    void Start()
    {
        this.CollisionManager.AddCollider(this.layerMask, this);
    }

    void OnDestroy()
    {
        if (this.CollisionManager)
            this.CollisionManager.RemoveCollider(this.layerMask, this);
    }

    void OnDrawGizmosSelected()
    {
        IntegerRect bounds = this.Bounds;
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(new Vector3(bounds.Center.X, bounds.Center.Y), new Vector3(this.Size.X, this.Size.Y));
    }

    public GameObject CollideFirst(int offsetX = 0, int offsetY = 0, int mask = Physics2D.DefaultRaycastLayers, string objectTag = null, List<IntegerRectCollider> potentialCollisions = null)
    {
        if (potentialCollisions == null)
            potentialCollisions = this.GetPotentialCollisions(0, 0, offsetX, offsetY, mask);
        
        if (potentialCollisions.Count == 0 || (potentialCollisions.Count == 1 && potentialCollisions[0] == this))
            return null;

        IntegerRect bounds = this.Bounds;
        bounds.Center = new IntegerVector(bounds.Center.X + offsetX, bounds.Center.Y + offsetY);

        foreach (IntegerRectCollider collider in potentialCollisions)
        {
            if (collider != this && (objectTag == null || collider.tag == objectTag))
            {
                if (bounds.Overlaps(collider.Bounds))
                    return collider.gameObject;
            }
        }

        return null;
    }

    public void Collide(List<GameObject> collisions, int offsetX = 0, int offsetY = 0, int mask = Physics2D.DefaultRaycastLayers, string objectTag = null, List<IntegerRectCollider> potentialCollisions = null)
    {
        if (potentialCollisions == null)
            potentialCollisions = this.GetPotentialCollisions(0, 0, offsetX, offsetY, mask);
        
        if (potentialCollisions.Count == 0 || (potentialCollisions.Count == 1 && potentialCollisions[0] == this))
            return;

        IntegerRect bounds = this.Bounds;
        bounds.Center = new IntegerVector(bounds.Center.X + offsetX, bounds.Center.Y + offsetY);

        foreach (IntegerRectCollider collider in potentialCollisions)
        {
            if (collider != this && (objectTag == null || collider.tag == objectTag))
            {
                if (bounds.Overlaps(collider.Bounds))
                    collisions.AddUnique(collider.gameObject);
            }
        }
    }

    public List<IntegerRectCollider> GetPotentialCollisions(float vx, float vy, int offsetX = 0, int offsetY = 0, int mask = Physics2D.DefaultRaycastLayers)
    {
        IntegerRect bounds = this.Bounds;
        IntegerRect range = new IntegerRect(bounds.Center.X + offsetX, bounds.Center.Y + offsetY, this.Size.X + Mathf.RoundToInt(Mathf.Abs(vx) + 0.55f), this.Size.Y + (Mathf.RoundToInt(Mathf.Abs(vy) + 0.55f)));
        return this.CollisionManager.GetCollidersInRange(range, mask);
    }

    public bool CollideCheck(GameObject checkObject, int offsetX = 0, int offsetY = 0)
    {
        IntegerRectCollider other = checkObject.GetComponent<IntegerRectCollider>();
        IntegerRect bounds = this.Bounds;
        bounds.Center.X += offsetX;
        bounds.Center.Y += offsetY;
        return other && this.Bounds.Overlaps(other.Bounds);
    }

    /**
     * Private
     */
    private CollisionManager _collisionManager = null;
}
