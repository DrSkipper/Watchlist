using UnityEngine;

public class IntegerRectCollider : VoBehavior
{
    public IntegerVector Offset = IntegerVector.Zero;
    public IntegerVector Size = IntegerVector.Zero;

    public IntegerRect Bounds { get { return new IntegerRect(this.integerPosition + this.Offset, this.Size); } }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(new Vector3(this.Bounds.Center.X, this.Bounds.Center.Y), new Vector3(this.Size.X, this.Size.Y));
    }
}
