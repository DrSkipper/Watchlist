using UnityEngine;
using System.Collections.Generic;

public class BossUnlockDialog : MonoBehaviour, UIDialogHandler
{
    public bool AcceptingInput { get { return _acceptingInput; } set { _acceptingInput = value; } }

    /**
     * Private
     */
    private bool _acceptingInput = true;
}
