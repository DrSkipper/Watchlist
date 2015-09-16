using UnityEngine;

public static class VectorExtensions
{
    public static int IntX(this Vector2 self) { return Mathf.RoundToInt(self.x); }
    public static int IntY(this Vector2 self) { return Mathf.RoundToInt(self.y); }
}
