using UnityEngine;
using UnityEngine.UI;

public class TextAnimator : VoBehavior
{
    public string Text = "<INSERT TEXT>";
    public int CharactersToAnimate = 1;
    public int CharactersPerSecond = 10;
    public int SwapsPerSecond = 20;
    public bool RunOnStart = true;

    void Start()
    {
        if (this.RunOnStart)
            this.Begin();
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
                        char nextChar = this.Text[i];

                        if (shouldSkip(nextChar))
                        {
                            // Do nothing
                        }
                        else
                        {
                            string addition = "";

                            if (isAlphabetical(nextChar))
                            {
                                addition = ALPHABETICAL_CHARACTERS[Random.Range(0, ALPHABETICAL_CHARACTERS.Length)];
                            }
                            else // Special characters
                            {
                                addition = SPECIAL_CHARACTERS[Random.Range(0, SPECIAL_CHARACTERS.Length)];
                            }

                            newText += addition;
                        }
                    }

                    _textComponent.text = newText;
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
                }

                _timeSinceCharacter = 0;
                _timeSinceSwap = _swapDuration + 0.1f;
            }
        }
	}

    public void Begin()
    {
        _textComponent = this.GetComponent<Text>();
        _swapDuration = 1.0f / (float)this.SwapsPerSecond;
        _singleCharacterDuration = 1.0f / (float)this.CharactersPerSecond;
        _textSoFar = "";
        _currentIndex = 0;
        _running = true;
        _timeSinceCharacter = 0;
        _timeSinceSwap = _swapDuration + 0.1f;
    }

    /**
     * Private
     */
    private static string[] ALPHABETICAL_CHARACTERS = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "1", "2", "3", "4", "5", "6", "7", "8", "9", "0" };
    private static string[] SPECIAL_CHARACTERS = { ".", ",", ";", "!", "?", "<", ">", "*", "@", "#", "$", "%", "^", "&", "(", ")", "-", "_", "+", "=", "/", "'", "[", "]", "|", "\"" };

    private Text _textComponent;
    private string _textSoFar;
    private int _currentIndex;
    private float _swapDuration;
    private float _singleCharacterDuration;
    private float _timeSinceSwap;
    private float _timeSinceCharacter;
    private bool _running;

    private bool isAlphabetical(char check)
    {
        return (check >= 'A' && check <= 'Z') || (check >= 'a' && check <= 'z');
    }

    private bool shouldSkip(char check)
    {
        return check == ' ';
    }
}
