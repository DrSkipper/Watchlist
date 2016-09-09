using UnityEngine;

public class MenuController : VoBehavior, UIDialogHandler
{
    public GameObject[] MenuElements;
    public int CurrentElement = 0;
    public bool AllowSelection = true;
    public bool AcceptingInput { get { return _acceptingInput; } set { _acceptingInput = value; } }
    public bool ListenToPause = true;
    public int[] PrioritizedDefaults;
    public MenuControlType ControlType = MenuControlType.UpDown;
    public int LimitToPlayerIndex = -1;
    public bool UsingController = false;
    public bool ShowSelectorsIfNoController = false;

    [System.Serializable]
    public enum MenuControlType
    {
        UpDown,
        LeftRight
    }
    
    void Start()
    {
        _animators = new Animator[this.MenuElements.Length];
        _elements = new MenuElement[this.MenuElements.Length];

        for (int i = 0; i < this.MenuElements.Length; ++i)
        {
            Animator animator = this.MenuElements[i].GetComponent<Animator>();
            MenuElement element = this.MenuElements[i].GetComponent<MenuElement>();
            _animators[i] = animator;
            _elements[i] = element;
            animator.SetBool("Locked", element.Locked);
            element.SelectedCallback = selectElement;
        }

        if (this.UsingController)
        {
            startUsingController();
        }
        else if (this.ShowSelectorsIfNoController)
        {
            for (int i = 0; i < this.MenuElements.Length; ++i)
            {
                _animators[i].SetTrigger("Highlighted");
            }
        }
    }

    void Update()
    {
        if ((!this.ListenToPause || !PauseController.IsPaused()) &&
            this.AcceptingInput)
        {
            if (!this.UsingController && MenuInput.ControllerUsed())
            {
                startUsingController();
            }

            else if (this.UsingController && (!this.AllowSelection || !_animators[this.CurrentElement].GetCurrentAnimatorStateInfo(0).IsName(_elements[this.CurrentElement].Locked ? "Selected (Locked)" : "Selected (UnLocked)")))
            {
                if (highlightNextElement())
                    highlightElement((this.CurrentElement + 1) % this.MenuElements.Length);
                else if (highlightPreviousElement())
                    highlightElement(this.CurrentElement == 0 ? this.MenuElements.Length - 1 : this.CurrentElement - 1);
                else if (!otherHighlightOption() &&
                    this.AllowSelection && MenuInput.SelectCurrentElement(this.LimitToPlayerIndex))
                    selectCurrentElement();
            }
        }
    }

    /**
     * Private
     */
    private bool _acceptingInput = true;
    private Animator[] _animators;
    private MenuElement[] _elements;

    private void startUsingController()
    {
        this.UsingController = true;
        for (int i = 0; i < this.PrioritizedDefaults.Length; ++i)
        {
            int elementIndex = this.PrioritizedDefaults[i];
            if (!this.MenuElements[elementIndex].GetComponent<MenuElement>().Locked)
            {
                this.CurrentElement = elementIndex;
                break;
            }
        }

        if (this.ShowSelectorsIfNoController)
        {
            for (int i = 0; i < this.MenuElements.Length; ++i)
            {
                _animators[i].SetTrigger("UnHighlighted");
            }
        }

        highlightElement(this.CurrentElement);
    }

    private bool highlightNextElement()
    {
        switch (this.ControlType)
        {
            default:
            case MenuControlType.UpDown:
                return MenuInput.NavDown(this.LimitToPlayerIndex);
            case MenuControlType.LeftRight:
                return MenuInput.NavRight(this.LimitToPlayerIndex);
        }
    }

    private bool highlightPreviousElement()
    {
        switch (this.ControlType)
        {
            default:
            case MenuControlType.UpDown:
                return MenuInput.NavUp(this.LimitToPlayerIndex);
            case MenuControlType.LeftRight:
                return MenuInput.NavLeft(this.LimitToPlayerIndex);
        }
    }

    private bool otherHighlightOption()
    {
        MenuElement.AdditionalNavOption[] options = _elements[this.CurrentElement].AdditionalNavOptions;

        if (options != null)
        {
            for (int i = 0; i < options.Length; ++i)
            {
                if (options[i].highlightNext())
                {
                    int index = findElement(options[i].Element);
                    if (index >= 0)
                    {
                        highlightElement(index);
                        return true;
                    }
                }
            }
        }
        return false;
    }

    private int findElement(MenuElement element)
    {
        for (int i = 0; i < _elements.Length; ++i)
        {
            if (_elements[i] == element)
                return i;
        }
        return -1;
    }

    private void highlightElement(int nextElement)
    {
        if (this.UsingController || !this.ShowSelectorsIfNoController)
        {
            if (nextElement != this.CurrentElement)
                _animators[this.CurrentElement].SetTrigger("UnHighlighted");
        }

        this.CurrentElement = nextElement;

        if (this.UsingController || !this.ShowSelectorsIfNoController)
        {
            _animators[this.CurrentElement].SetTrigger("Highlighted");
        }
    }

    private void selectCurrentElement()
    {
        _animators[this.CurrentElement].SetTrigger("Selected");

        if (!_elements[this.CurrentElement].Locked)
        {
            //TODO - fcole - Wait for some animation to be finished or something
            _elements[this.CurrentElement].Select();
        }
    }

    private void selectElement(MenuElement element)
    {
        if (!this.UsingController)
        {
            int e = 0;
            for (int i = 0; i < this.MenuElements.Length; ++i)
            {
                if (this.MenuElements[i] == element.gameObject)
                {
                    e = i;
                    break;
                }
            }

            _animators[e].SetTrigger("Selected");

            if (!_elements[e].Locked)
            {
                //TODO - fcole - Wait for some animation to be finished or something
                _elements[e].Select();
            }
        }
    }
}
