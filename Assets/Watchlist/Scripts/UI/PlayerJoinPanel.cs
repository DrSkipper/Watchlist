﻿using UnityEngine;
using UnityEngine.UI;
using Rewired;
using System.Collections.Generic;

public class PlayerJoinPanel : MonoBehaviour
{
    public int PlayerIndex = 0;
    public Text JoinText;
    public Text JoinedText;
    public ControllerAssigner Assigner;

    void Awake()
    {
        Assigner.AddAssignmentCallback(this.PlayerIndex, controllerAssigned);
        Assigner.AddUnassignmentCallback(this.PlayerIndex, controllerUnassigned);
    }

    void Start()
    {
        SessionPlayer p = DynamicData.GetSessionPlayer(this.PlayerIndex);
        if (p.HasJoined)
        {
            controllerAssigned(p);
        }
    }

    /**
     * Private
     */
    private void controllerAssigned(SessionPlayer player)
    {
        this.JoinedText.gameObject.SetActive(true);
        this.JoinText.gameObject.SetActive(false);

        string buttonText = "";
        Player rewiredPlayer = ReInput.players.GetPlayer(player.RewiredId);
        int categoryId = ReInput.mapping.GetMapCategoryId(MenuInput.MENU_CATEGORY);

        foreach (ControllerMap map in rewiredPlayer.controllers.maps.GetAllMaps())
        {
            if (map.categoryId == categoryId)
            {
                foreach (ActionElementMap actionMap in map.ButtonMapsWithAction(MenuInput.EXIT))
                {
                    buttonText = actionMap.elementIdentifierName;
                    break;
                }

                if (buttonText != "")
                    break;
            }
        }
        
        this.JoinedText.text = player.Name + " JOINED (HOLD '" + buttonText.ToUpper() + "' TO LEAVE)";
    }

    private void controllerUnassigned(SessionPlayer player)
    {
        this.JoinedText.gameObject.SetActive(false);
        this.JoinText.gameObject.SetActive(true);

        //TODO - Try to display correct button text with data from Rewired maps?
    }
}