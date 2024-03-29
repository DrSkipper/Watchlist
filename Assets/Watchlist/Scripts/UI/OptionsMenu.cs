﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OptionsMenu : MonoBehaviour
{
    public const int ReturnIndex = 0;
    public const int FullscreenIndex = 1;
    public const int ResolutionIndex = 2;
    public const int ShaderIndex = 3;
    public const int VsyncIndex = 4;
    public const int CameraAimingImpactIndex = 5;
    public const int ControllerAimSpeedIndex = 6;
    public const int ControllerAimLastDirectionIndex = 7;
    public const int DataEraseIndex = 8;

    public MenuController MenuController;
    public Text FullscreenText;
    public Text ResolutionText;
    public Text ShaderText;
    public Text VsyncText;
    public Text CameraAimingImpactText;
    public Text ControllerAimSpeedText;
    public Text ControllerAimLastDirText;
    public Text DataEraseText;
    public Toggler ShaderToggle;
    public string MainMenuScene = "MainMenu";
    public float EraseDataHoldTime = 5.0f;

    void Start()
    {
        _fullscreenBaseText = this.FullscreenText.text;
        _resolutionBaseText = this.ResolutionText.text;
        _shaderBaseText = this.ShaderText.text;
        _vsyncBaseText = this.VsyncText.text;
        _dataEraseBaseText = this.DataEraseText.text;
        _cameraAimImpactBaseText = this.CameraAimingImpactText.text;
        _controllerAimSpeedBaseText = this.ControllerAimSpeedText.text;
        _controllerAimLastDirBaseText = this.ControllerAimLastDirText.text;
        _fullscreenResolutions = Screen.resolutions;

        alignFullscreenText();
        alignResolutionText();
        alignShaderText();
        alignVsyncText();
        alignCameraAimImpactText();
        alignControllerAimSpeedText();
        alignControllerAimLastDirText();
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
                case CameraAimingImpactIndex:
                    if (left)
                        changeCameraAimImpact(-1);
                    else if (right)
                        changeCameraAimImpact(1);
                    break;
                case ControllerAimSpeedIndex:
                    if (left)
                        changeControllerAimSpeed(-1);
                    else if (right)
                        changeControllerAimSpeed(1);
                    break;
                case ControllerAimLastDirectionIndex:
                    toggleControllerAimLastDir();
                    break;
                default:
                case DataEraseIndex:
                    break;
            }
        }

        updateDataErase(this.MenuController.CurrentElement == DataEraseIndex && MenuInput.HoldingConfirm());
    }

    /**
     * Private
     */
    private string _fullscreenBaseText;
    private string _resolutionBaseText;
    private string _shaderBaseText;
    private string _vsyncBaseText;
    private string _dataEraseBaseText;
    private string _cameraAimImpactBaseText;
    private string _controllerAimSpeedBaseText;
    private string _controllerAimLastDirBaseText;
    private Resolution[] _fullscreenResolutions;
    private float _eraseDataHoldTime;
    private bool _erasedData;

    public const string CAMERA_AIM_IMPACT_KEY = "cam_aim_impact";
    public const int DEFAULT_CAMERA_AIM_IMPACT = 50;
    private const int MIN_CAMERA_AIM_IMPACT = 0;
    private const int MAX_CAMERA_AIM_IMPACT = 100;
    private const int CAMERA_AIM_IMACT_INTERVAL = 10;
    private const int CAMERA_AIM_IMPACT_DISPLAY_FACTOR = 10;

    public const string CONTROLLER_AIM_SPEED_KEY = "con_aim_speed";
    public const float DEFAULT_CONTROLLER_AIM_SPEED = 7;
    private const float MIN_CONTROLLER_AIM_SPEED = 5;
    private const float MAX_CONTROLLER_AIM_SPEED = 9;
    private const float CONTROLLER_AIM_SPEED_INTERVAL = 0.5f;
    private const int CONTROLLER_AIM_SPEED_DISPLAY_OFFSET = -4;
    private const int CONTROLLER_AIM_SPEED_DISPLAY_MULT = 2;

    public const string CONTROLLER_AIM_LAST_DIR_KEY = "con_aim_prev_dir";
    public const int DEFAULT_CONTROLLER_AIM_LAST_DIR = 1;

    private const string ON = "ON";
    private const string OFF = "OFF";
    private const string SHADER_KEY = "shader_toggle";

    private void updateDataErase(bool holding)
    {
        if (_erasedData)
            return;

        if (holding)
        {
            if (_eraseDataHoldTime > this.EraseDataHoldTime)
            {
                _erasedData = true;
                ProgressData.WipeData();
                ProgressData.SaveToDisk();
                PersistentData.EraseLocalData();
                this.DataEraseText.text = _dataEraseBaseText + " (Erased!)";
            }
            else
            {
                _eraseDataHoldTime += Time.deltaTime;
                this.DataEraseText.text = _dataEraseBaseText + " (Hold " + Mathf.RoundToInt(Mathf.Clamp(this.EraseDataHoldTime - _eraseDataHoldTime, 0.0f, this.EraseDataHoldTime)) + ")";
            }

        }
        else if (_eraseDataHoldTime > 0.0f)
        {
            _eraseDataHoldTime = 0.0f;
            this.DataEraseText.text = _dataEraseBaseText;
        }
    }

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

    private void changeCameraAimImpact(int dir)
    {
        int current = PlayerPrefs.GetInt(CAMERA_AIM_IMPACT_KEY, DEFAULT_CAMERA_AIM_IMPACT);
        current = Mathf.Clamp(current + dir * CAMERA_AIM_IMACT_INTERVAL, MIN_CAMERA_AIM_IMPACT, MAX_CAMERA_AIM_IMPACT);
        PlayerPrefs.SetInt(CAMERA_AIM_IMPACT_KEY, current);
        alignCameraAimImpactText();
    }

    private void changeControllerAimSpeed(int dir)
    {
        float current = PlayerPrefs.GetFloat(CONTROLLER_AIM_SPEED_KEY, DEFAULT_CONTROLLER_AIM_SPEED);
        current = Mathf.Clamp(current + dir * CONTROLLER_AIM_SPEED_INTERVAL, MIN_CONTROLLER_AIM_SPEED, MAX_CONTROLLER_AIM_SPEED);
        PlayerPrefs.SetFloat(CONTROLLER_AIM_SPEED_KEY, current);
        alignControllerAimSpeedText();
    }

    private void toggleControllerAimLastDir()
    {
        int current = PlayerPrefs.GetInt(CONTROLLER_AIM_LAST_DIR_KEY, DEFAULT_CONTROLLER_AIM_LAST_DIR);
        current = current == 1 ? 0 : 1;
        PlayerPrefs.SetInt(CONTROLLER_AIM_LAST_DIR_KEY, current);
        alignControllerAimLastDirText();
    }

    private void alignCameraAimImpactText()
    {
        this.CameraAimingImpactText.text = _cameraAimImpactBaseText + (PlayerPrefs.GetInt(CAMERA_AIM_IMPACT_KEY, DEFAULT_CAMERA_AIM_IMPACT) / CAMERA_AIM_IMPACT_DISPLAY_FACTOR);
    }

    private void alignControllerAimSpeedText()
    {
        this.ControllerAimSpeedText.text = _controllerAimSpeedBaseText + Mathf.RoundToInt(((PlayerPrefs.GetFloat(CONTROLLER_AIM_SPEED_KEY, DEFAULT_CONTROLLER_AIM_SPEED) + CONTROLLER_AIM_SPEED_DISPLAY_OFFSET) * CONTROLLER_AIM_SPEED_DISPLAY_MULT));
    }

    private void alignControllerAimLastDirText()
    {
        this.ControllerAimLastDirText.text = _controllerAimLastDirBaseText + (PlayerPrefs.GetInt(CONTROLLER_AIM_LAST_DIR_KEY, DEFAULT_CONTROLLER_AIM_LAST_DIR) == 1 ? ON : OFF);
    }
}
