using UnityEngine;

public class CursorHider : MonoBehaviour
{
    public bool Visible = false;

	void Start()
    {
        Cursor.visible = this.Visible;
	}
}
