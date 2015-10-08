using UnityEngine;
using System;
using System.Collections.Generic;

public class CollisionManager : VoBehavior
{
    public const float RAYCAST_MAX_POSITION_INCREMENT = 1.0f;

    public struct RaycastResult
    {
        public bool Collided { get { return this.CollidedObjects != null && this.CollidedObjects.Length != 0; } }
        public GameObject[] CollidedObjects;
        public IntegerVector[] PointsBeforeCollisions;
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

    public List<IntegerRectCollider> GetCollidersInRange(LayerMask mask, IntegerRect range)
    {
        List<IntegerRectCollider> colliders = new List<IntegerRectCollider>();

        foreach (LayerMask key in _collidersByLayer.Keys)
        {
            if ((key & mask) != 0)
            {
                foreach (IntegerRectCollider collider in _collidersByLayer[key])
                {
                    if (collider.Bounds.Overlaps(range))
                        colliders.Add(collider);
                }
            }
        }

        return colliders;
    }

    public GameObject CollidePointFirst(IntegerVector point, int layerMask = Physics2D.DefaultRaycastLayers, string objectTag = null, List<IntegerRectCollider> potentialCollisions = null)
    {
        return null;
    }

    public RaycastResult RaycastFirst(IntegerVector origin, Vector2 direction, float range = 100000.0f, int layerMask = Physics2D.DefaultRaycastLayers, string objectTag = null)
    {
        Vector2 d = direction * range;
        IntegerVector rangeVector = new IntegerVector(Mathf.RoundToInt(d.x + 2.0f), Mathf.RoundToInt(d.y + 2.0f));
        IntegerVector halfwayPoint = (origin + rangeVector) / 2;
        List<IntegerRectCollider> possibleCollisions = this.GetCollidersInRange(layerMask, new IntegerRect(halfwayPoint, rangeVector));

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
                GameObject collision = this.CollidePointFirst(checkPos, layerMask, objectTag, possibleCollisions);
                if (collision)
                {
                    result.CollidedObjects = new GameObject[1];
                    result.PointsBeforeCollisions = new IntegerVector[1];
                    result.CollidedObjects[0] = collision;
                    result.PointsBeforeCollisions[0] = position;
                    break;
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
                GameObject collision = this.CollidePointFirst(checkPos, layerMask, objectTag, possibleCollisions);
                if (collision)
                {
                    result.CollidedObjects = new GameObject[1];
                    result.PointsBeforeCollisions = new IntegerVector[1];
                    result.CollidedObjects[0] = collision;
                    result.PointsBeforeCollisions[0] = position;
                    break;
                }

                position = checkPos;
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
