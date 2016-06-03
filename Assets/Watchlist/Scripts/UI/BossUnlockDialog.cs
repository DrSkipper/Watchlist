using UnityEngine;
using System.Collections.Generic;

public class BossUnlockDialog : MonoBehaviour, UIDialog
{
    public bool AcceptingInput { get { return _acceptingInput; } set { _acceptingInput = value; } }
    //public 

    /**
     * Private
     */
    private bool _acceptingInput = true;
}
