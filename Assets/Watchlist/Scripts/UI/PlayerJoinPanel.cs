using UnityEngine;
using UnityEngine.UI;
using Rewired;

public class PlayerJoinPanel : MonoBehaviour
{
    public int PlayerIndex = 0;
    public Text JoinText;
    public Text JoinedText;
    public GameObject JoinedControllerIcon;
    public GameObject JoinedKeyboardIcon;
    public ControllerAssigner Assigner;

    void Awake()
    {
        _animator = this.GetComponent<Animator>();
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
        else
        {
            controllerUnassigned(p);
        }
    }

    /**
     * Private
     */
    private Animator _animator;

    private void controllerAssigned(SessionPlayer player)
    {
        if (_animator != null)
            _animator.SetTrigger("Joined");

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
        
        this.JoinedText.text = player.Name + " JOINED (PRESS '" + buttonText.ToUpper() + "' TO LEAVE)";
    }

    private void controllerUnassigned(SessionPlayer player)
    {
        if (_animator != null)
            _animator.SetTrigger("Unjoined");

        this.JoinedText.gameObject.SetActive(false);
        this.JoinText.gameObject.SetActive(true);

        //TODO - Try to display correct button text with data from Rewired maps?
    }
}
