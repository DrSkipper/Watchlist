using UnityEngine;

public class MoveOrderOnAnimEvent : VoBehavior
{
    public void MoveToFront()
    {
        this.transform.SetAsLastSibling();
    }

    public void MoveToBack()
    {
        this.transform.SetAsFirstSibling();
    }
}
