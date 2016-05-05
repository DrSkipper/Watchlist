using UnityEngine;

public class Pathing : VoBehavior
{
    public Transform[] Nodes;
    public float Speed = 100.0f;
    public int CurrentIndex = 0;
    public int LoopIndex = 0;
    public LoopType Loop;
    public bool RunOnStart = true;

    [System.Serializable]
    public enum LoopType
    {
        NoLoop,
        Loop,
        PingPong
    }

    void Start()
    {
        if (this.RunOnStart)
            _running = true;
    }

    void Update()
    {
        if (_running)
        {
            float d = this.Speed * Time.deltaTime;

            while (d > 0.0f && _running)
            {
                d = move(d);
            }
        }
    }

    /**
     * Private
     */
    private bool _running;

    private float move(float d)
    {
        Vector2 target = this.Nodes[nextIndex].position;
        float distance = Vector2.Distance(this.transform.position, target);
        if (distance <= d)
        {
            this.transform.position = new Vector3(target.x, target.y, this.transform.position.z);
            handlePositioning();
            return d - distance;
        }

        Vector2 pos = Vector2.MoveTowards(this.transform.position, target, d);
        this.transform.position = new Vector3(pos.x, pos.y, this.transform.position.z);
        return -1;
    }

    private void handlePositioning()
    {
        if (this.CurrentIndex >= this.Nodes.Length - 2)
        {
            switch (this.Loop)
            {
                default:
                case LoopType.NoLoop:
                    _running = false;
                    this.transform.position = new Vector3(this.Nodes[this.LoopIndex].position.x, this.Nodes[this.LoopIndex].position.y, this.transform.position.z);
                    break;
                case LoopType.Loop:
                    this.transform.position = new Vector3(this.Nodes[this.LoopIndex].position.x, this.Nodes[this.LoopIndex].position.y, this.transform.position.z);
                    break;
                case LoopType.PingPong:
                    break;
            }
        }

        this.CurrentIndex = nextIndex;
    }

    private int nextIndex
    {
        get
        {
            if (this.CurrentIndex < this.Nodes.Length - 1)
                return this.CurrentIndex + 1;

            switch (this.Loop)
            {
                default:
                case LoopType.NoLoop:
                    return 0;
                case LoopType.Loop:
                    return this.LoopIndex;
                case LoopType.PingPong:
                    return Mathf.Max(0, this.Nodes.Length - 2);
            }
        }
    }
}
