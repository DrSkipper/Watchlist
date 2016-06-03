using UnityEngine;
using System.Collections.Generic;

public class BossUnlockDialog : MonoBehaviour, UIDialogHandler
{
    public bool AcceptingInput { get { return _acceptingInput; } set { _acceptingInput = value; } }
    //public 

    void Start()
    {
        UIDialog blah = this.GetComponent<UIDialog>();
    }

    /**
     * Private
     */
    private bool _acceptingInput = true;
}
