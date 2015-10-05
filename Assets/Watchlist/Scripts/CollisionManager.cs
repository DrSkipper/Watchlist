using UnityEngine;
using System.Collections.Generic;

public class CollisionManager : VoBehavior
{
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

    /**
     * Private
     */
    private Dictionary<LayerMask, List<IntegerRectCollider>> _collidersByLayer = new Dictionary<LayerMask, List<IntegerRectCollider>>();
}
