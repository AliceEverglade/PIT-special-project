using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using Ink.Runtime;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }
    private const string SPEAKER_TAG = "speaker";
    private const string PORTRAIT_TAG = "portrait";
    private const string LAYOUT_TAG = "layout";
    private DialogueVariables dialogueVariables;

    [Header("Load Globals Json")]
    [SerializeField] private TextAsset loadGlobalsJson;


    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private GameObject continueIcon;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextMeshProUGUI displayNameText;
    [SerializeField] private Animator portraitAnimator;
    private Animator layoutAnimator;

    [Header("Choices UI")]
    [SerializeField] private GameObject choiceHolder;
    [SerializeField] private GameObject choicePrefab;
    [SerializeField] private List<GameObject> choiceList;

    [Header("DialogueControls")]
    [SerializeField, Range(0.1f, 1000f)] private float textSpeed = 40;
    private bool canContinueToNextLine = false;


    private Story currentStory;

    public bool DialogueIsPlaying { get; private set; }

    private Coroutine displayLineCoroutine;

    private void Awake()
    {
        if(Instance != null)
        {
            Debug.LogWarning(" DialogueManager instance already exists");
        }
        Instance = this;
        Debug.Log("Instance Set");

        dialogueVariables = new DialogueVariables(loadGlobalsJson);
    }

    private void Start()
    {
        layoutAnimator = dialoguePanel.GetComponent<Animator>();
        DialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
    }

    private void Update()
    {
        if(!DialogueIsPlaying) return;

        if (canContinueToNextLine && 
            currentStory.currentChoices.Count == 0 && 
            (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))) //simplify
        {
            ContinueStory();
        }
    }

    public void EnterDialogueMode(TextAsset inkJson)
    {
        currentStory = new Story(inkJson.text);
        DialogueIsPlaying = true;
        dialoguePanel.SetActive(true);
        dialogueVariables.StartListening(currentStory);

        displayNameText.text = "???";
        portraitAnimator.Play("default");
        layoutAnimator.Play("right");

        ContinueStory();
    }

    private IEnumerator ExitDialogueMode()
    {
        yield return new WaitForSeconds(0.2f);
        dialogueVariables.StopListening(currentStory);
        DialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
    }

    private void ContinueStory()
    {
        if (currentStory.canContinue)
        {
            if(displayLineCoroutine != null)
            {
                StopCoroutine(displayLineCoroutine);
            }
            displayLineCoroutine = StartCoroutine(DisplayLine(currentStory.Continue()));
            HandleTags(currentStory.currentTags);
        }
        else
        {
            StartCoroutine(ExitDialogueMode());
        }
    }

    private IEnumerator DisplayLine(string line)
    {
        dialogueText.text = line;
        dialogueText.maxVisibleCharacters = 0;

        continueIcon.SetActive(false);
        HideChoices();
        canContinueToNextLine = false;
        bool isAddingRichTextTag = false;
        foreach(char letter in line)
        {
            if(Input.GetKeyDown(KeyCode.F) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return)) // simplify
            {
                dialogueText.maxVisibleCharacters = line.Length;
                break;
            }

            if(letter == '<' || isAddingRichTextTag)
            {
                isAddingRichTextTag = true;
                if (letter == '>')
                {
                    isAddingRichTextTag = false;
                }
            }
            else
            {
                dialogueText.maxVisibleCharacters++;
                yield return new WaitForSeconds(textSpeed / 1000);
            }

            
        }
        continueIcon.SetActive(true);
        DisplayChoices();
        canContinueToNextLine = true;
    }

    private void HandleTags(List<string> currentTags)
    {
        foreach (string tag in currentTags)
        {
            string[] splitTag = tag.Split(':');
            if(splitTag.Length != 2)
            {
                Debug.LogError("Tag could not be appropriately parsed: " + tag);
            }
            string tagKey = splitTag[0].Trim();
            string tagValue = splitTag[1].Trim();

            switch (tagKey)
            {
                case SPEAKER_TAG:
                    displayNameText.text = tagValue;
                    break;
                case PORTRAIT_TAG:
                    portraitAnimator.Play(tagValue);
                    break;
                case LAYOUT_TAG:
                    layoutAnimator.Play(tagValue);
                    break;
                default:
                    Debug.LogError("Tag came in but isn't currently handled: " + tag);
                    break;
            }
        }
    }

    private void DisplayChoices()
    {
        List<Choice> choices = currentStory.currentChoices;
        for (int i = 0; i < choices.Count; i++)
        {
            GameObject choiceObj = Instantiate(choicePrefab, choiceHolder.transform);
            choiceObj.GetComponent<ChoiceButton>().Init(choices[i].text, i);
            choiceList.Add(choiceObj.gameObject);
        }
        if(choiceList.Count > 0)
        {
            StartCoroutine(SelectFirstChoice(choiceList[0]));
        }
        
    }

    private void HideChoices()
    {
        foreach (GameObject choice in choiceList)
        {
            Destroy(choice);
        }
        choiceList.Clear();
    }

    public void MakeChoice(int choiceIndex)
    {
        if (canContinueToNextLine)
        {
            currentStory.ChooseChoiceIndex(choiceIndex);
            ContinueStory();
        }
    }

    private IEnumerator SelectFirstChoice(GameObject first) // might be redundant, tutorial told me to do it
    {
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(first);
    }

    public Ink.Runtime.Object GetVariableState(string variableName)
    {
        Ink.Runtime.Object variableValue = null;
        dialogueVariables.variables.TryGetValue(variableName, out variableValue);
        if(variableValue == null)
        {
            Debug.LogWarning("Ink variable was found to be null: " + variableName);
        }
        return variableValue;
    }
}
