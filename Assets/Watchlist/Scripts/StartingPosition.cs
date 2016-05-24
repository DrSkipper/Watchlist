using UnityEngine;

public class StartingPosition : MonoBehaviour
{
    public Vector3 Local() { return _local; }
    public Vector3 Global() { return _global; }
    
    void Awake()
    {
        _local = this.transform.localPosition;
        _global = this.transform.position;
    }

    private Vector3 _local;
    private Vector3 _global;
}

