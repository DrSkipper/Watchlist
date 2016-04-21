using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour {
    void Awake()
    {
        string n = this.name;
        this.name = this.name + "_check";
        if (GameObject.Find(n) != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            this.name = n;
            DontDestroyOnLoad(this.gameObject);
        }
    }
}
