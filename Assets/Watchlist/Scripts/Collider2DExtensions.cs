﻿using UnityEngine;
using System.Collections.Generic;

public static class Collider2DExtensions
{
    public const int MIN_OVERLAP_AREA_CHECK = 300;
    
    public static int LeftX(this BoxCollider2D self) { return Mathf.RoundToInt(self.bounds.min.x); }
    public static int RightX(this BoxCollider2D self) { return Mathf.RoundToInt(self.bounds.max.x); }
    public static int TopY(this BoxCollider2D self) { return Mathf.RoundToInt(self.bounds.max.y); }
    public static int BottomY(this BoxCollider2D self) { return Mathf.RoundToInt(self.bounds.min.y); }
    
    public static GameObject CollideFirst(this BoxCollider2D self, int offsetX = 0, int offsetY = 0, int layerMask = Physics2D.DefaultRaycastLayers, string objectTag = null, Collider2D[] potentialCollisions = null)
    {
        if (potentialCollisions == null)
            potentialCollisions = self.GetPotentialCollisions(0, 0, offsetX, offsetY, layerMask);

        // If there is only one collider, it is our collider, so there is nothing to collide with
        if (potentialCollisions.Length <= 1)
            return null;
        
        Bounds bounds = self.bounds;

        // Apply offset to our bounds and make sure we're using integer/pixel-perfect math
        bounds.center = new Vector3(Mathf.Round(bounds.center.x) + offsetX, Mathf.Round(bounds.center.y) + offsetY, Mathf.Round(bounds.center.z));

        // Account for bounds intersections marking true when colliders end at same point
        bounds.size = new Vector3(bounds.size.x - 2, bounds.size.y - 2, bounds.size.z);

        foreach (Collider2D collider in potentialCollisions)
        {
            if (collider != self && (objectTag == null || collider.tag == objectTag))
            {
                // Make sure we're using integer/pixel-perfect math
                Bounds otherBounds = collider.bounds;
                otherBounds.center = new Vector3(Mathf.Round(otherBounds.center.x), Mathf.Round(otherBounds.center.y), Mathf.Round(otherBounds.center.z));

                if (bounds.Intersects(otherBounds))
                    return collider.gameObject;
            }
        }

        return null;
    }

    public static void Collide(this BoxCollider2D self, List<GameObject> collisions, int offsetX = 0, int offsetY = 0, int layerMask = Physics2D.DefaultRaycastLayers, string objectTag = null, Collider2D[] potentialCollisions = null)
    {
        if (potentialCollisions == null)
            potentialCollisions = self.GetPotentialCollisions(0, 0, offsetX, offsetY, layerMask);

        // If there is only one collider, it is our collider, so there is nothing to collide with
        if (potentialCollisions.Length <= 1)
            return;

        Bounds bounds = self.bounds;

        // Apply offset to our bounds and make sure we're using integer/pixel-perfect math
        bounds.center = new Vector3(Mathf.Round(bounds.center.x) + offsetX, Mathf.Round(bounds.center.y) + offsetY, Mathf.Round(bounds.center.z));

        // Account for bounds intersections marking true when colliders end at same point
        bounds.size = new Vector3(Mathf.RoundToInt(bounds.size.x), Mathf.RoundToInt(bounds.size.y), bounds.size.z);

        foreach (Collider2D collider in potentialCollisions)
        {
            if (collider != self && (objectTag == null || collider.tag == objectTag))
            {
                // Make sure we're using integer/pixel-perfect math
                Bounds otherBounds = collider.bounds;
                otherBounds.center = new Vector3(Mathf.Round(otherBounds.center.x), Mathf.Round(otherBounds.center.y), Mathf.Round(otherBounds.center.z));

                if (bounds.Overlaps(otherBounds))
                {
                    GameObject collidedObject = collider.gameObject;
                    if (!collisions.Contains(collidedObject))
                        collisions.Add(collidedObject);
                }
            }
        }

        return;
    }

    public static Collider2D[] GetPotentialCollisions(this BoxCollider2D self, float vx, float vy, int offsetX = 0, int offsetY = 0, int layerMask = Physics2D.DefaultRaycastLayers)
    {
        // Overlap an area significantly larger than our bounding box so we can directly compare bounds with collision candidates
        // (Relying purely on OverlapAreaAll for collision seems to be inconsistent at times)
        //NOTE - There appears to be a bug with OverlapAreaAll when the area checked against is too small
        // http://answers.unity3d.com/questions/772941/physics2doverlapareaallnoalloc-fails-detecting-obj.html
        Bounds bounds = self.bounds;
        float overlapSizeX = Mathf.Max(bounds.size.x + Mathf.Abs(vx), MIN_OVERLAP_AREA_CHECK);
        float overlapSizeY = Mathf.Max(bounds.size.y + Mathf.Abs(vy), MIN_OVERLAP_AREA_CHECK);
        Vector2 corner1 = new Vector2(bounds.min.x - overlapSizeX + offsetX, bounds.min.y - overlapSizeY + offsetY);
        Vector2 corner2 = new Vector2(bounds.max.x + overlapSizeX + offsetX, bounds.max.y + overlapSizeY + offsetY);
        return Physics2D.OverlapAreaAll(corner1, corner2, layerMask);
    }

    public static bool CollideCheck(this BoxCollider2D self, GameObject checkObject, int offsetX = 0, int offsetY = 0)
    {
        BoxCollider2D other = checkObject.GetComponent<BoxCollider2D>();

        if (other)
        {
            // Apply offset to our bounds and make sure we're using integer/pixel-perfect math
            Bounds bounds = self.bounds;
            Bounds otherBounds = other.bounds;
            bounds.center = new Vector3(Mathf.Round(bounds.center.x) + offsetX, Mathf.Round(bounds.center.y) + offsetY, Mathf.Round(bounds.center.z));
            otherBounds.center = new Vector3(Mathf.Round(otherBounds.center.x), Mathf.Round(otherBounds.center.y), Mathf.Round(otherBounds.center.z));

            // Account for bounds intersections marking true when colliders end at same point
            bounds.size = new Vector3(bounds.size.x - 2, bounds.size.y - 2, bounds.size.z);
            
            return bounds.Intersects(otherBounds);
        }

        return false;
    }

    public static bool Overlaps(this Bounds self, Bounds other)
    {
        int selfMinX = Mathf.RoundToInt(self.min.x);
        int selfMaxX = Mathf.RoundToInt(self.max.x);
        int selfMinY = Mathf.RoundToInt(self.min.y);
        int selfMaxY = Mathf.RoundToInt(self.max.y);
        int otherMinX = Mathf.RoundToInt(other.min.x);
        int otherMaxX = Mathf.RoundToInt(other.max.x);
        int otherMinY = Mathf.RoundToInt(other.min.y);
        int otherMaxY = Mathf.RoundToInt(other.max.y);

        bool x = (otherMinX >= selfMinX && otherMinX < selfMaxX) || (otherMaxX > selfMinX && otherMaxX <= selfMaxX) || (selfMinX >= otherMinX && selfMinX < otherMaxX) || (selfMaxX > otherMinX && selfMaxX <= otherMaxX);
        bool y = (otherMinY >= selfMinY && otherMinY < selfMaxY) || (otherMaxY > selfMinY && otherMaxY <= selfMaxY) || (selfMinY >= otherMinY && selfMinY < otherMaxY) || (selfMaxY > otherMinY && selfMaxY <= otherMaxY);
        return x && y;
    }
}
