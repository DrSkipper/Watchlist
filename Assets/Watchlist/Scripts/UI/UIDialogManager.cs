using UnityEngine;
using System.Collections.Generic;

public class UIDialogManager : MonoBehaviour
{
    public static UIDialogManager Instance { get { return _manager; } }
    public delegate void ActiveDialogChangedDelegate(UIDialog activeDialog);

    public UIDialog ActiveDialog
    {
        get
        {
            if (_dialogStack.Count > 0)
                return _dialogStack[0];
            return null;
        }
        set
        {
            for (int i = 0; i < _dialogStack.Count; ++i)
            {
                if (_dialogStack[i] == value)
                {
                    _dialogStack.Move(i);
                    sendActiveDialogChangeNotif();
                    break;
                }
            }
        }
    }

    void Awake()
    {
        _manager = this;
        _dialogStack = new List<UIDialog>();
        _activeDialogChangedCallbacks = new List<ActiveDialogChangedDelegate>();
    }

    public void MoveActiveToBack()
    {
        _dialogStack.Move(0, _dialogStack.Count - 1);

        if (_dialogStack.Count > 0)
            sendActiveDialogChangeNotif();
    }

    public void AddDialog(UIDialog dialog)
    {
        _dialogStack.Add(dialog);

        if (_dialogStack.Count == 1)
            sendActiveDialogChangeNotif();
    }

    public void RemoveDialog(UIDialog dialog)
    {
        if (_dialogStack.Count > 0)
        {
            bool change = _dialogStack[0] == dialog;
            _dialogStack.Remove(dialog);
            if (change)
                sendActiveDialogChangeNotif();
        }
    }

    public void AddActiveDialogChangedCallback(ActiveDialogChangedDelegate callback)
    {
        _activeDialogChangedCallbacks.Add(callback);
    }

    public void RemoveActiveDialogChangedCallback(ActiveDialogChangedDelegate callback)
    {
        _activeDialogChangedCallbacks.Remove(callback);
    }

    /**
     * Private
     */
    private static UIDialogManager _manager;
    private List<UIDialog> _dialogStack;
    private List<ActiveDialogChangedDelegate> _activeDialogChangedCallbacks;

    private void sendActiveDialogChangeNotif()
    {
        UIDialog activeDialog = this.ActiveDialog;
        for (int i = 0; i < _activeDialogChangedCallbacks.Count; ++i)
        {
            _activeDialogChangedCallbacks[i](activeDialog);
        }
    }
}
