using UnityEngine;
using System;
using System.Collections.Generic;

public class CollisionManager : VoBehavior
{
    public const float RAYCAST_MAX_POSITION_INCREMENT = 1.0f;

    public struct RaycastCollision
    {
        public GameObject CollidedObject;
        public IntegerVector CollisionPoint;
        public bool CollidedX;
        public bool CollidedY;
    }

    public struct RaycastResult
    {
        public bool Collided { get { return this.Collisions != null && this.Collisions.Length != 0; } }
        public RaycastCollision[] Collisions;
        public IntegerVector FarthestPointReached;
    }

    public void AddCollider(LayerMask layer, IntegerRectCollider collider)
    {
        if (!_collidersByLayer.ContainsKey(layer))
            _collidersByLayer.Add(layer, new List<IntegerRectCollider>());
        _collidersByLayer[layer].Add(collider);
    }

    public void RemoveCollider(LayerMask layer, IntegerRectCollider collider)
    {
        if (_collidersByLayer.ContainsKey(layer))
            _collidersByLayer[layer].Remove(collider);
    }

    public List<IntegerRectCollider> GetCollidersInRange(IntegerRect range, int mask = Physics2D.DefaultRaycastLayers, string objectTag = null)
    {
        List<IntegerRectCollider> colliders = new List<IntegerRectCollider>();

        foreach (LayerMask key in _collidersByLayer.Keys)
        {
            if ((key & mask) != 0)
            {
                foreach (IntegerRectCollider collider in _collidersByLayer[key])
                {
                    if ((objectTag == null || collider.tag == objectTag) && 
                        collider.Bounds.Overlaps(range))
                        colliders.Add(collider);
                }
            }
        }

        return colliders;
    }

    public GameObject CollidePointFirst(IntegerVector point, int mask = Physics2D.DefaultRaycastLayers, string objectTag = null)
    {
        foreach (LayerMask key in _collidersByLayer.Keys)
        {
            if ((key & mask) != 0)
            {
                foreach (IntegerRectCollider collider in _collidersByLayer[key])
                {
                    if ((objectTag == null || collider.tag == objectTag) && 
                        collider.Bounds.Contains(point))
                        return collider.gameObject;
                }
            }
        }
        return null;
    }

    public GameObject CollidePointFirst(IntegerVector point, List<IntegerRectCollider> potentialCollisions)
    {
        foreach (IntegerRectCollider collider in potentialCollisions)
        {
            if (collider.Bounds.Contains(point))
                return collider.gameObject;
        }
        return null;
    }
    
    public RaycastResult RaycastFirst(IntegerVector origin, Vector2 direction, float range = 100000.0f, int mask = Physics2D.DefaultRaycastLayers, string objectTag = null)
    {
        Vector2 d = direction * range;
        IntegerVector rangeVector = new IntegerVector(Mathf.RoundToInt(d.x + 2.0f), Mathf.RoundToInt(d.y + 2.0f));
        IntegerVector halfwayPoint = (origin + rangeVector) / 2;
        List<IntegerRectCollider> possibleCollisions = this.GetCollidersInRange(new IntegerRect(halfwayPoint, rangeVector), mask);

        Vector2 positionModifier = Vector2.zero;
        IntegerVector position = origin;

        float incX = d.x;
        float incY = d.y;

        if (Mathf.Abs(incX) > RAYCAST_MAX_POSITION_INCREMENT || Mathf.Abs(incY) > RAYCAST_MAX_POSITION_INCREMENT)
        {
            Vector2 dNormalized = d.normalized * RAYCAST_MAX_POSITION_INCREMENT;
            incX = dNormalized.x;
            incY = dNormalized.y;
        }
        
        Vector2 projected = Vector2.zero;
        Vector2 soFar = Vector2.zero;
        float dMagnitude = d.magnitude;
        RaycastResult result = new RaycastResult();
        bool endReached = false;

        while (true)
        {
            projected.x += incX;
            projected.y += incY;

            if (projected.magnitude > dMagnitude)
            {
                incX = d.x - soFar.x;
                incY = d.y - soFar.y;
                endReached = true;
            }

            positionModifier.x += incX;
            int move = (int)positionModifier.x;

            positionModifier.x -= move;
            int unitDir = Math.Sign(move);

            while (move > 0)
            {
                IntegerVector checkPos = new IntegerVector(position.X + unitDir, position.Y);
                GameObject collision = this.CollidePointFirst(checkPos, possibleCollisions);
                if (collision)
                {
                    result.Collisions = new RaycastCollision[1];
                    RaycastCollision hit = new RaycastCollision();
                    hit.CollidedObject = collision;
                    hit.CollisionPoint = position;
                    hit.CollidedX = true;
                }

                position = checkPos;
            }

            positionModifier.y += incY;
            move = (int)positionModifier.y;

            positionModifier.y -= move;
            unitDir = Math.Sign(move);

            while (move > 0)
            {
                IntegerVector checkPos = new IntegerVector(position.X, position.Y + unitDir);
                GameObject collision = this.CollidePointFirst(checkPos, possibleCollisions);
                if (collision)
                {
                    if (result.Collided)
                    {
                        result.Collisions[0].CollidedY = true;
                    }
                    else
                    {
                        result.Collisions = new RaycastCollision[1];
                        RaycastCollision hit = new RaycastCollision();
                        hit.CollidedObject = collision;
                        hit.CollisionPoint = position;
                        hit.CollidedY = true;
                    }
                }

                position = checkPos;

                if (result.Collided)
                {
                    endReached = true;
                    break;
                }
            }

            if (endReached)
                break;

            soFar.x = projected.x;
            soFar.y = projected.y;
        }

        result.FarthestPointReached = position;
        return result;
    }

    /**
     * Private
     */
    private Dictionary<LayerMask, List<IntegerRectCollider>> _collidersByLayer = new Dictionary<LayerMask, List<IntegerRectCollider>>();
}
