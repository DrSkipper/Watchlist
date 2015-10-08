using UnityEngine;
using System.Collections.Generic;
using System;

//TODO - Take into account rotation in Overlaps and Collides
public class IntegerRectCollider : IntegerCollider
{
    public IntegerVector Size;
    public override IntegerRect Bounds { get { return new IntegerRect(this.integerPosition + this.Offset, this.Size); } }

    void OnDrawGizmosSelected()
    {
        IntegerRect bounds = this.Bounds;
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(new Vector3(bounds.Center.X, bounds.Center.Y), new Vector3(this.Size.X, this.Size.Y));
    }
}
