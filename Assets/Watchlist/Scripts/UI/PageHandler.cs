using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[RequireComponent(typeof(TextAnimator))]
public class PageHandler : VoBehavior
{
    public delegate void FlippedLastPageCallback();
    public int CurrentPage { get { return _currentPage; } }
    public FlippedLastPageCallback OnFlippedLastPage;

    void Awake()
    {
        _textAnimator = this.GetComponent<TextAnimator>();
    }

    public void AddPage(string pageText)
    {
        _pages.Add(pageText);
    }

    public void IncrementPage()
    {
        if (!_textAnimator.Running)
        {
            ++_currentPage;

            if (_currentPage < _pages.Count)
            {
                _textAnimator.Text = _pages[_currentPage];
                _textAnimator.Begin();
            }
            else if (this.OnFlippedLastPage != null)
            {
                this.OnFlippedLastPage();
            }
        }
    }

    public bool PageDone { get { return !_textAnimator.Running; } }

    /**
     * Private
     */
    private TextAnimator _textAnimator;
    private List<string> _pages = new List<string>();
    private int _currentPage = -1;
}
