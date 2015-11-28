using UnityEngine;

public static class MenuInput
{
    public static bool HighlightNextElement()
    {
        return Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S);
    }

    public static bool HighlightPreviousElement()
    {
        return Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W);
    }

    public static bool SelectCurrentElement()
    {
        return Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space);
    }

    public static bool Pause()
    {
        return Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.P);
    }

    public static bool Exit()
    {
        return Input.GetKeyDown(KeyCode.Q);
    }
}
