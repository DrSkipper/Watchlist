﻿using UnityEngine;

public class MenuController : VoBehavior, UIDialogHandler
{
    public GameObject[] MenuElements;
    public int CurrentElement = 0;
    public bool AllowSelection = true;
    public bool AcceptingInput { get { return _acceptingInput; } set { _acceptingInput = value; } }
    public bool ListenToPause = true;
    public int[] PrioritizedDefaults;
    public MenuControlType ControlType = MenuControlType.UpDown;

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
        }

        for (int i = 0; i < this.PrioritizedDefaults.Length; ++i)
        {
            int elementIndex = this.PrioritizedDefaults[i];
            if (!this.MenuElements[elementIndex].GetComponent<MenuElement>().Locked)
            {
                this.CurrentElement = elementIndex;
                break;
            }
        }

        highlightElement(this.CurrentElement);
    }

    void Update()
    {
        if ((!this.ListenToPause || !PauseController.IsPaused()) && 
            this.AcceptingInput && (!this.AllowSelection || !_animators[this.CurrentElement].GetCurrentAnimatorStateInfo(0).IsName(_elements[this.CurrentElement].Locked ? "Selected (Locked)" : "Selected (UnLocked)")))
        {
            if (highlightNextElement())
                highlightElement((this.CurrentElement + 1) % this.MenuElements.Length);
            else if (highlightPreviousElement())
                highlightElement(this.CurrentElement == 0 ? this.MenuElements.Length - 1 : this.CurrentElement - 1);
            else if (!otherHighlightOption() &&
                this.AllowSelection && MenuInput.SelectCurrentElement())
                selectCurrentElement();
        }
    }

    /**
     * Private
     */
    private bool _acceptingInput = true;
    private Animator[] _animators;
    private MenuElement[] _elements;

    private bool highlightNextElement()
    {
        switch (this.ControlType)
        {
            default:
            case MenuControlType.UpDown:
                return MenuInput.NavDown();
            case MenuControlType.LeftRight:
                return MenuInput.NavRight();
        }
    }

    private bool highlightPreviousElement()
    {
        switch (this.ControlType)
        {
            default:
            case MenuControlType.UpDown:
                return MenuInput.NavUp();
            case MenuControlType.LeftRight:
                return MenuInput.NavLeft();
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
        if (nextElement != this.CurrentElement)
            _animators[this.CurrentElement].SetTrigger("UnHighlighted");

        this.CurrentElement = nextElement;
        _animators[this.CurrentElement].SetTrigger("Highlighted");
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
}
