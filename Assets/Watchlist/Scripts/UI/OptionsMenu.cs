using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OptionsMenu : MonoBehaviour
{
    public const int ReturnIndex = 0;
    public const int FullscreenIndex = 1;
    public const int ResolutionIndex = 2;
    public const int ShaderIndex = 3;
    public const int VsyncIndex = 4;
    public const int DataEraseIndex = 5;

    public MenuController MenuController;
    public Text FullscreenText;
    public Text ResolutionText;
    public Text ShaderText;
    public Text VsyncText;
    public Text DataEraseText;
    public Toggler ShaderToggle;
    public string MainMenuScene = "MainMenu";

    void Start()
    {
        _fullscreenBaseText = this.FullscreenText.text;
        _resolutionBaseText = this.ResolutionText.text;
        _shaderBaseText = this.ShaderText.text;
        _vsyncBaseText = this.VsyncText.text;
        _dataEraseBaseText = this.DataEraseText.text;
        _fullscreenResolutions = Screen.resolutions;

        alignFullscreenText();
        alignResolutionText();
        alignShaderText();
        alignVsyncText();
    }

    void Update()
    {
        bool current = MenuInput.SelectCurrentElement();
        bool left = MenuInput.NavLeft();
        bool right = MenuInput.NavRight();

        if (current || left || right)
        {
            switch (MenuController.CurrentElement)
            {
                case ReturnIndex:
                    if (current)
                        SceneManager.LoadScene(this.MainMenuScene);
                    break;
                case FullscreenIndex:
                    toggleFullscreen();
                    break;
                case ResolutionIndex:
                    toggleResolution(left ? -1 : 1);
                    break;
                case ShaderIndex:
                    toggleShader();
                    break;
                case VsyncIndex:
                    toggleVsync();
                    break;
                case DataEraseIndex:
                    break;
            }
        }
    }

    /**
     * Private
     */
    private string _fullscreenBaseText;
    private string _resolutionBaseText;
    private string _shaderBaseText;
    private string _vsyncBaseText;
    private string _dataEraseBaseText;
    private Resolution[] _fullscreenResolutions;

    private const string ON = "ON";
    private const string OFF = "OFF";
    private const string SHADER_KEY = "shader_toggle";

    private void toggleFullscreen()
    {
        if (Screen.fullScreen)
        {
            int w = Screen.currentResolution.width;
            int h = Screen.currentResolution.height;

            if (h >= _fullscreenResolutions[_fullscreenResolutions.Length - 1].height)
            {
                int i = _fullscreenResolutions.Length > 1 ? _fullscreenResolutions.Length - 2 : 0;
                w = _fullscreenResolutions[i].width;
                h = _fullscreenResolutions[i].height;
                Screen.SetResolution(w, h, false);
            }
            else
            {
                Screen.fullScreen = false;
            }

            alignFullscreenText(false);
            alignResolutionText(w, h);
        }
        else
        {
            Resolution resolution = _fullscreenResolutions[_fullscreenResolutions.Length - 1];
            Screen.SetResolution(resolution.width, resolution.height, true);
            alignFullscreenText(true);
            alignResolutionText(resolution.width, resolution.height);
        }
    }

    private void toggleResolution(int dir)
    {
        int w = Screen.fullScreen ? Screen.currentResolution.width : Screen.width;
        int h = Screen.fullScreen ? Screen.currentResolution.height : Screen.height;
        int i = 0;

        for (; i < _fullscreenResolutions.Length; ++i)
        {
            if (_fullscreenResolutions[i].width == w && _fullscreenResolutions[i].height == h)
                break;
        }

        i += dir;
        int max = Screen.fullScreen ? _fullscreenResolutions.Length : _fullscreenResolutions.Length - 1;
        if (i >= max)
            i = 0;
        else if (i < 0)
            i = max - 1;

        w = _fullscreenResolutions[i].width;
        h = _fullscreenResolutions[i].height;
        Screen.SetResolution(w, h, Screen.fullScreen);
        alignResolutionText(w, h);
    }

    private void toggleShader()
    {
        this.ShaderToggle.Toggle();
        alignShaderText();
    }

    private void toggleVsync()
    {
        QualitySettings.vSyncCount = QualitySettings.vSyncCount == 0 ? 1 : 0;
        alignVsyncText();
    }

    private void alignFullscreenText()
    {
        alignFullscreenText(Screen.fullScreen);
    }

    private void alignFullscreenText(bool fullscreen)
    {
        this.FullscreenText.text = _fullscreenBaseText + (fullscreen ? ON : OFF);
    }

    private void alignResolutionText()
    {
        alignResolutionText(Screen.width, Screen.height);
    }

    private void alignResolutionText(int width, int height)
    {
        this.ResolutionText.text = _resolutionBaseText + width + "x" + height;
    }

    private void alignShaderText()
    {
        this.ShaderText.text = _shaderBaseText + (PlayerPrefs.GetInt(SHADER_KEY, 1) == 1 ? ON : OFF);
    }

    private void alignVsyncText()
    {
        this.VsyncText.text = _vsyncBaseText + (QualitySettings.vSyncCount > 0 ? ON : OFF);
    }
}
