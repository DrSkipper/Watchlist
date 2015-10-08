using UnityEngine;

[System.Serializable]
public struct IntegerRect
{
    public IntegerVector Center;
    public IntegerVector Size;
    public IntegerVector Extents { get { return this.Size / 2; } }

    public IntegerVector Min
    {
        get { return this.Center - this.Extents; }
        set
        {
            IntegerVector newCenter = (value + this.Max) / 2;
            this.Size = this.Max - value;
            this.Center = newCenter;
        }
    }

    public IntegerVector Max
    {
        get { return this.Center + this.Extents; }
        set
        {
            IntegerVector newCenter = (this.Min + value) / 2;
            this.Size = value - this.Min;
            this.Center = newCenter;
        }
    }

    public IntegerRect(int centerX = 0, int centerY = 0, int sizeX = 0, int sizeY = 0)
    {
        this.Center = new IntegerVector(centerX, centerY);
        this.Size = new IntegerVector(sizeX, sizeY);
    }

    public IntegerRect(IntegerVector center, IntegerVector size)
    {
        this.Center = center;
        this.Size = size;
    }

    public bool Overlaps(IntegerRect other)
    {
        IntegerVector selfMin = this.Min;
        IntegerVector selfMax = this.Max;
        IntegerVector otherMin = other.Min;
        IntegerVector otherMax = other.Max;
        
        return ((otherMin.X >= selfMin.X && otherMin.X < selfMax.X) || (otherMax.X > selfMin.X && otherMax.X <= selfMax.X) || 
                (selfMin.X >= otherMin.X && selfMin.X < otherMax.X) || (selfMax.X > otherMin.X && selfMax.X <= otherMax.X)) && 
               ((otherMin.Y >= selfMin.Y && otherMin.Y < selfMax.Y) || (otherMax.Y > selfMin.Y && otherMax.Y <= selfMax.Y) || 
                (selfMin.Y >= otherMin.Y && selfMin.Y < otherMax.Y) || (selfMax.Y > otherMin.Y && selfMax.Y <= otherMax.Y));
    }

    public bool Contains(IntegerVector point)
    {
        IntegerVector selfMin = this.Min;
        IntegerVector selfMax = this.Max;
        return point.X >= selfMin.X && point.X <= selfMax.X && point.Y >= selfMin.Y && point.Y <= selfMax.Y;
    }
}

[System.Serializable]
public struct IntegerVector
{
    public int X;
    public int Y;

    public IntegerVector(int x = 0, int y = 0)
    {
        this.X = x;
        this.Y = y;
    }

    public IntegerVector(Vector2 v)
    {
        this.X = Mathf.RoundToInt(v.x);
        this.Y = Mathf.RoundToInt(v.y);
    }

    public static IntegerVector operator +(IntegerVector v1, IntegerVector v2)
    {
        return new IntegerVector(v1.X + v2.X, v1.Y + v2.Y);
    }

    public static IntegerVector operator -(IntegerVector v1, IntegerVector v2)
    {
        return new IntegerVector(v1.X - v2.X, v1.Y - v2.Y);
    }

    public static IntegerVector operator *(IntegerVector v, int i)
    {
        return new IntegerVector(v.X * i, v.Y * i);
    }

    public static IntegerVector operator /(IntegerVector v, int i)
    {
        return new IntegerVector(v.X / i, v.Y / i);
    }

    public static IntegerVector Zero { get { return new IntegerVector(); } }
}
