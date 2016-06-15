using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.UI;

public class MenuElement : MonoBehaviour
{
    public enum ActionType
    {
        SceneChange,
        ApplicationQuit,
        WipeProgress,
        ContinueProgress,
        SendEvent
    }

    [System.Serializable]
    public struct Action
    {
        public ActionType Type;
        public string StringParameter;
    }

    [System.Serializable]
    public struct AdditionalNavOption
    {
        public NavType Nav;
        public MenuElement Element;

        public bool highlightNext()
        {
            switch (this.Nav)
            {
                default:
                case NavType.Left:
                    return MenuInput.NavLeft();
                case NavType.Right:
                    return MenuInput.NavRight();
                case NavType.Up:
                    return MenuInput.NavUp();
                case NavType.Down:
                    return MenuInput.NavDown();
            }
        }
    }

    [System.Serializable]
    public enum NavType
    {
        Left,
        Right,
        Up,
        Down
    }

    public bool Locked = false;
    public List<Action> Actions;
    public Text Text;
    public Color LockedColor;
    public Color UnlockedColor;
    public AdditionalNavOption[] AdditionalNavOptions;

    void Awake()
    {
        foreach (Action action in this.Actions)
        {
            handleAwakeForAction(action);
        }
    }

    void Start()
    {
        if (this.Text != null)
            this.Text.color = this.Locked ? this.LockedColor : this.UnlockedColor;
    }

    public void Select()
    {
        foreach (Action action in this.Actions)
        {
            handleSelectForAction(action);
        }
    }

    /**
     * Private
     */
    private void handleAwakeForAction(Action action)
    {
        switch (action.Type)
        {
            default:
            case ActionType.SceneChange:
                break;
            case ActionType.ApplicationQuit:
                break;
            case ActionType.WipeProgress:
                break;
            case ActionType.ContinueProgress:
                if (ProgressData.CompletedTiles.Length == 0)
                    this.Locked = true;
                break;
            case ActionType.SendEvent:
                break;
        }
    }

    private void handleSelectForAction(Action action)
    {
        switch (action.Type)
        {
            default:
            case ActionType.SceneChange:
                SceneManager.LoadScene(action.StringParameter);
                break;
            case ActionType.ApplicationQuit:
                Application.Quit();
                break;
            case ActionType.WipeProgress:
                ProgressData.WipeData();
                break;
            case ActionType.ContinueProgress:
                break;
            case ActionType.SendEvent:
                GlobalEvents.Notifier.SendEvent(new MenuElementSelectedEvent(this, action.StringParameter));
                break;
        }
    }
}
