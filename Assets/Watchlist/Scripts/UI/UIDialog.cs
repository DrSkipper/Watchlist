using UnityEngine;

public interface UIDialogHandler
{
    bool AcceptingInput { set; }
}

[RequireComponent(typeof(TimedCallbacks))]
public class UIDialog : MonoBehaviour
{
    public bool ActiveAtStart = true;

    void Awake()
    {
        _handler = this.GetComponent<UIDialogHandler>();
        _timedCallbacks = this.GetComponent<TimedCallbacks>();
        if (UIDialogManager.Instance != null)
        {
            _usingManager = true;
            UIDialogManager.Instance.AddDialog(this);
            UIDialogManager.Instance.AddActiveDialogChangedCallback(activeDialogChanged);
        }
    }

    void Start()
    {
        if (this.ActiveAtStart)
        {
            if (_usingManager)
                UIDialogManager.Instance.ActiveDialog = this;
        }
        else
        {
            _handler.AcceptingInput = false;
        }
    }

    void OnDestroy()
    {
        if (UIDialogManager.Instance != null)
        {
            UIDialogManager.Instance.RemoveActiveDialogChangedCallback(activeDialogChanged);
            UIDialogManager.Instance.RemoveDialog(this);
        }
    }

    /**
     * Private
     */
    TimedCallbacks _timedCallbacks;
    UIDialogHandler _handler;
    private bool _usingManager;

    private const float BEGIN_ACCEPTING_INPUT_DELAY = 0.2f;

    private void activeDialogChanged(UIDialog activeDialog)
    {
        if (activeDialog == this)
        {
            _timedCallbacks.RemoveCallbacksForOwner(this);
            _timedCallbacks.AddCallback(this, beginAcceptingInput, BEGIN_ACCEPTING_INPUT_DELAY);
        }
    }

    private void beginAcceptingInput()
    {
        _handler.AcceptingInput = true;
    }
}
