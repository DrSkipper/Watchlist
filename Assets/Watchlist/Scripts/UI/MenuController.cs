using UnityEngine;

public class MenuController : VoBehavior
{
    public GameObject[] MenuElements;
    public int CurrentElement = 0;

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

        highlightElement(this.CurrentElement);
    }

    void Update()
    {
        if (!_animators[this.CurrentElement].GetCurrentAnimatorStateInfo(0).IsName(_elements[this.CurrentElement].Locked ? "Selected (Locked)" : "Selected (UnLocked"))
        {
            if (MenuInput.HighlightNextElement())
                highlightElement((this.CurrentElement + 1) % this.MenuElements.Length);
            else if (MenuInput.HighlightPreviousElement())
                highlightElement(this.CurrentElement == 0 ? this.MenuElements.Length - 1 : this.CurrentElement - 1);
            else if (MenuInput.SelectCurrentElement())
                selectCurrentElement();
        }
    }

    /**
     * Private
     */
    private Animator[] _animators;
    private MenuElement[] _elements;

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

        if (_elements[this.CurrentElement].Destination != "")
        {
            //TODO - fcole - Wait for some animation to be finished or something
            Application.LoadLevel(_elements[this.CurrentElement].Destination);
        }
    }
}
