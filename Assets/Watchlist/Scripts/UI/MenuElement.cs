using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

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

    void Awake()
    {
        foreach (Action action in this.Actions)
        {
            handleStartForAction(action);
        }
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
    private void handleStartForAction(Action action)
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
