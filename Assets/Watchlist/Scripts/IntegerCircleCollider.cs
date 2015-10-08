using UnityEngine;
using System.Collections;
using System;

public class IntegerCircleCollider : IntegerCollider
{
    public int Radius = 1;
    public int Diameter { get { return this.Radius * 2; } }
    public override IntegerRect Bounds { get { return new IntegerRect(this.integerPosition + this.Offset, new IntegerVector(this.Diameter, this.Diameter)); } }

    void OnDrawGizmosSelected()
    {
        IntegerRect bounds = this.Bounds;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(new Vector3(bounds.Center.X, bounds.Center.Y), this.Radius);
    }
}
