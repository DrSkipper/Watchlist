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
        ContinueProgress
    }

    [System.Serializable]
    public struct Action
    {
        public ActionType Type;
        public string StringParameter;
    }

    public bool Locked = false;
    public List<Action> Actions;
    public Text Text;
    public Color LockedColor;
    public Color UnlockedColor;

    void Awake()
    {
        foreach (Action action in this.Actions)
        {
            handleAwakeForAction(action);
        }
    }

    void Start()
    {
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
        }
    }
}
