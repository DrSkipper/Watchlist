using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class TextAnimator : VoBehavior
{
    [TextArea(3, 10)]
    [Multiline]
    public string Text = "<INSERT TEXT>";
    public int CharactersToAnimate = 1;
    public int CharactersPerSecond = 10;
    public int SwapsPerSecond = 20;
    public bool ClearOnStart = true;
    public bool RunOnStart = true;
    public bool EnableCursorBlink = false;
    public string CursorText = "_";
    public float CursorBlinkOnDuration = 0.5f;
    public float CursorBlinkOffDuration = 0.2f;
    public bool Running { get { return _running; } }

    void Start()
    {
        _textComponent = this.GetComponent<Text>();

        if (this.RunOnStart)
            this.Begin();
        else if (this.ClearOnStart)
            _textComponent.text = "";
    }

	void Update()
    {
        if (_running)
        {
            bool shouldProgress = false;

            if (_timeSinceCharacter < _singleCharacterDuration && _currentIndex < this.Text.Length)
            {
                _timeSinceCharacter += Time.deltaTime;

                if (_timeSinceSwap >= _swapDuration)
                {
                    _timeSinceSwap = 0;

                    string newText = _textSoFar;
                    int charsToAnimate = this.CharactersToAnimate;
                    if (charsToAnimate + _currentIndex > this.Text.Length)
                        charsToAnimate = this.Text.Length - _currentIndex;

                    string animatingChars = this.Text.Substring(_currentIndex, charsToAnimate);

                    for (int i = 0; i < animatingChars.Length; ++i)
                    {
                        char nextChar = animatingChars[i];

                        if (shouldSkip(nextChar))
                        {
                            // Do nothing
                        }
                        else
                        {
                            char addition = ' ';

                            if (isAlphabetical(nextChar))
                            {
                                addition = ALPHABETICAL_CHARACTERS[Random.Range(0, ALPHABETICAL_CHARACTERS.Length)];
                            }
                            else if (isPunctuation(nextChar))
                            {
                                addition = PUNCTUATION_CHARACTERS[Random.Range(0, PUNCTUATION_CHARACTERS.Length)];
                            }
                            else // Special characters
                            {
                                addition = SPECIAL_CHARACTERS[Random.Range(0, SPECIAL_CHARACTERS.Length)];
                            }

                            newText += addition;
                        }
                    }

                    _textComponent.text = this.EnableCursorBlink ? newText + this.CursorText : newText;
                }
                else
                {
                    _timeSinceSwap += Time.deltaTime;
                }
            }
            else
            {
                shouldProgress = true;
            }

            if (shouldProgress)
            {
                while (_timeSinceCharacter > 0.0f)
                {
                    _timeSinceCharacter -= _singleCharacterDuration;

                    _textSoFar += this.Text[_currentIndex];
                    _textComponent.text = _textSoFar;
                    ++_currentIndex;

                    if (_currentIndex >= this.Text.Length)
                    {
                        _running = false;
                        break;
                    }
                    else if (this.EnableCursorBlink)
                    {
                        _textComponent.text += this.CursorText;
                    }
                }

                _timeSinceCharacter = 0;
                _timeSinceSwap = _swapDuration + 0.1f;
            }
        }

        // If not running, blink the cursor at the end of the text
        else if (this.EnableCursorBlink)
        {
            if (_blinking)
            {
                if (_timeSinceBlink >= this.CursorBlinkOnDuration)
                {
                    _timeSinceBlink = 0.0f;
                    _textComponent.text = _textComponent.text.Remove(_textComponent.text.Length - this.CursorText.Length);
                    _blinking = false;
                }
                else
                {
                    _timeSinceBlink += Time.deltaTime;
                }
            }
            else
            {
                if (_timeSinceBlink >= this.CursorBlinkOffDuration)
                {
                    _timeSinceBlink = 0.0f;
                    _textComponent.text += this.CursorText;
                    _blinking = true;
                }
                else
                {
                    _timeSinceBlink += Time.deltaTime;
                }
            }
        }
	}

    public void Begin()
    {
        if (_blinking)
        {
            _textComponent.text.Remove(_textComponent.text.Length - this.CursorText.Length);
            _blinking = false;
        }

        _swapDuration = 1.0f / (float)this.SwapsPerSecond;
        _singleCharacterDuration = 1.0f / (float)this.CharactersPerSecond;
        _textSoFar = "";
        _currentIndex = 0;
        _running = true;
        _timeSinceCharacter = 0;
        _timeSinceSwap = _swapDuration + 0.1f;
        _timeSinceBlink = 0.0f;
    }

    /**
     * Private
     */
    private static char[] ALPHABETICAL_CHARACTERS = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0' };
    private static char[] PUNCTUATION_CHARACTERS = { '.', ',', ';', '-', '\'', ':' };
    private static char[] SPECIAL_CHARACTERS = { '!', '?', '<', '>', '*', '@', '#', '$', '%', '^', '&', '(', ')', '_', '+', '=', '/', '[', ']', '|', '"' };

    private Text _textComponent;
    private string _textSoFar;
    private int _currentIndex;
    private float _swapDuration;
    private float _singleCharacterDuration;
    private float _timeSinceSwap;
    private float _timeSinceCharacter;
    private bool _running;
    private bool _blinking;
    private float _timeSinceBlink;

    private bool isAlphabetical(char check)
    {
        return (check >= 'A' && check <= 'Z') || (check >= 'a' && check <= 'z') || (check >= '0' && check <= '9');
    }

    private bool isPunctuation(char check)
    {
        foreach (char c in PUNCTUATION_CHARACTERS)
        {
            if (c == check)
                return true;
        }
        return false;
    }

    private bool shouldSkip(char check)
    {
        return check == ' ';
    }
}
