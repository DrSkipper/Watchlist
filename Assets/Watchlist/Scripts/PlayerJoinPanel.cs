using UnityEngine;
using UnityEngine.UI;
using Rewired;
using System.Collections.Generic;

public class PlayerJoinPanel : MonoBehaviour
{
    public int PlayerId = 0;
    public Text JoinText;
    public Text JoinedText;
    public ControllerAssigner Assigner;

    void Awake()
    {
        Assigner.AddAssignmentCallback(this.PlayerId, controllerAssigned);
        Assigner.AddUnassignmentCallback(this.PlayerId, controllerUnassigned);
    }

    /**
     * Private
     */
    private void controllerAssigned(Player player, Controller controller)
    {
        this.JoinedText.gameObject.SetActive(true);
        this.JoinText.gameObject.SetActive(false);

        string buttonText = "";
        foreach (ControllerMap map in player.controllers.maps.GetMaps(controller.type, controller.id))
        {
            foreach (ActionElementMap actionMap in map.ButtonMapsWithAction("Exit"))
            {
                buttonText = actionMap.elementIdentifierName;
                break;
            }
            break;
        }

        this.JoinedText.text = player.descriptiveName + " JOINED (HOLD '" + buttonText.ToUpper() + "' TO LEAVE)";
    }

    private void controllerUnassigned()
    {
        this.JoinedText.gameObject.SetActive(false);
        this.JoinText.gameObject.SetActive(true);
    }
}
