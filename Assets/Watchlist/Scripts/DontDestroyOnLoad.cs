using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour
{
    public bool MarkedForDestruction { get { return _markedForDestruction; } }

    void Awake()
    {
        string n = this.name;
        this.name = this.name + "_check";
        if (GameObject.Find(n) != null)
        {
            _markedForDestruction = true;
            Destroy(this.gameObject);
        }
        else
        {
            this.name = n;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    private bool _markedForDestruction;
}
