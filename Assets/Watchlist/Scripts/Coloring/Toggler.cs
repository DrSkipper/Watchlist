using UnityEngine;

public class Toggler : MonoBehaviour
{
    public string ToggleInput;
    public string PlayerPrefKey;
    public Behaviour[] ToggleTargets;
    public bool UseMenuInput = false;
    public bool CheckInput = true;

    private delegate bool CheckDelegate();
    private CheckDelegate _check;
    private int _toggled;

    void Start()
    {
        if (this.PlayerPrefKey != null && this.PlayerPrefKey != "")
            _toggled = PlayerPrefs.GetInt(this.PlayerPrefKey, 1);
        else
            _toggled = 1;

        if (!this.CheckInput)
            _check = noCheck;
        else if (this.UseMenuInput)
            _check = menuCheck;
        else
            _check = gameplayCheck;

        toggle();
    }

    void Update()
    {
        if (_check())
            this.Toggle();
    }

    public void Toggle()
    {
        _toggled = _toggled == 1 ? 0 : 1;
        toggle();
        PlayerPrefs.SetInt(this.PlayerPrefKey, _toggled);
    }
    
    private void toggle()
    {
        for (int i = 0; i < this.ToggleTargets.Length; ++i)
        {
            this.ToggleTargets[i].enabled = _toggled == 1 ? true : false;
        }
    }

    private bool noCheck()
    {
        return false;
    }

    private bool menuCheck()
    {
        return MenuInput.AnyPlayerButtonPressed(this.ToggleInput);
    }

    private bool gameplayCheck()
    {
        return GameplayInput.AnyPlayerButtonPressed(this.ToggleInput);
    }
}
