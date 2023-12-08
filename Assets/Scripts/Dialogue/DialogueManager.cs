using System;
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
    private const string EMOTION_TAG = "emotion";
    private const string LAYOUT_TAG = "layout";
    private const string ENCOUNTER_TAG = "encounter";
    private DialogueVariables dialogueVariables;

    [Header("Load Globals Json")]
    [SerializeField] private TextAsset loadGlobalsJson;

    #region Settings
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

    [Header("Audio")]
    [SerializeField] private List<AudioClip> dialogueSoundClips;
    private AudioSource audioSource;
    [SerializeField] private bool stopAudioSource;
    [Range(1,5)]
    [SerializeField] private int dialogueAudioFrequency;
    [Range(-3, 3)]
    [SerializeField] private float minPitch = 0.5f;
    [Range(-3, 3)]
    [SerializeField] private float maxPitch = 3f;

    [Header("DialogueControls")]
    [SerializeField, Range(0.1f, 1000f)] private float textSpeed = 40;
    private bool canContinueToNextLine = false;
    #endregion

    private Story currentStory;

    public bool DialogueIsPlaying { get; private set; }

    private Coroutine displayLineCoroutine;

    #region Start and Awake
    private void Awake()
    {
        if(Instance != null)
        {
            Debug.LogWarning(" DialogueManager instance already exists");
        }
        Instance = this;
        Debug.Log("Instance Set");

        dialogueVariables = new DialogueVariables(loadGlobalsJson);
        audioSource = gameObject.AddComponent<AudioSource>(); // convert to audio handler singleton in the future
    }

    private void Start()
    {
        layoutAnimator = dialoguePanel.GetComponent<Animator>();
        DialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
    }
    #endregion

    private void Update()
    {
        if(!DialogueIsPlaying) return;

        if (canContinueToNextLine && 
            currentStory.currentChoices.Count == 0 && 
            (Input.GetKeyDown(KeyCode.Space))) //change out for something proper
        {
            ContinueStory();
        }
    }

    #region Enter, Continue and Exit dialogue mode
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
    #endregion

    #region Dialogue functionality
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
            if(Input.GetKeyDown(KeyCode.Return)) // change out controls for something proper
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
                PlayDialogueSound(dialogueText.maxVisibleCharacters, dialogueText.text[dialogueText.maxVisibleCharacters]);
                dialogueText.maxVisibleCharacters++;
                
                yield return new WaitForSeconds(textSpeed / 1000);
            }

            
        }
        continueIcon.SetActive(true);
        DisplayChoices();
        canContinueToNextLine = true;
    }

    private void PlayDialogueSound(int currentDisplayedCharacterCount, char currentCharacter)
    {
        if(currentDisplayedCharacterCount % dialogueAudioFrequency == 0)
        {
            if (stopAudioSource)
            {
                audioSource.Stop();
            }
            AudioClip audioClip = null;
            int hashCode = currentCharacter.GetHashCode();
            int index = hashCode % dialogueSoundClips.Count;
            audioClip = dialogueSoundClips[index];

            int minPitchInt = (int)(minPitch * 100);
            int maxPitchInt = (int)(maxPitch * 100);
            int pitchRangeInt = maxPitchInt - minPitchInt;
            if (pitchRangeInt != 0)
            {
                int pitchInt = (hashCode % pitchRangeInt) + minPitchInt;
                float pitch = pitchInt / 100f;
            }
            else
            {
                audioSource.pitch = minPitch;
            }

            audioSource.PlayOneShot(audioClip);
        }
    }

    private void HandleTags(List<string> currentTags)
    {
        CharacterData speaker = CharacterLibrary.Instance.MainCharacter.data;
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
                //speaker code (set speaker specific variables)
                case SPEAKER_TAG:
                    speaker = CharacterLibrary.Instance.GetCharacter(tagValue);
                    SetEmotionVariables(speaker, "Neutral");
                    displayNameText.text = speaker.CharacterName;
                    break;


                //emotion code (set the portrait based on the speaker
                case EMOTION_TAG:
                    SetEmotionVariables(speaker, tagValue);
                    break;

                //UI layout code
                case LAYOUT_TAG:
                    layoutAnimator.Play(tagValue);
                    break;

                // custom tags, make sure to add the tag to the top of the script as a const and have the value be all lower case,
                // you can use lower and upper case in the ink file

                //encounter code
                case ENCOUNTER_TAG:
                    string[] splitId = tagValue.Split(",");
                    if (splitId.Length == 2)
                    {
                        EncounterManager.Instance.ActivateEncounter(splitId[0].Trim(), splitId[1].Trim());
                    }
                    break;


                default:
                    Debug.LogError("Tag came in but isn't currently handled: " + tag);
                    break;
            }
        }
    }

    void SetEmotionVariables(CharacterData speaker, string emotionText)
    {
        portraitAnimator.Play("Default");
        portraitAnimator.Play(speaker.GetPortrait(CharacterLibrary.Instance.GetEmotion(emotionText)));
        AudioProfile speakerProfile = speaker.GetAudioProfile(CharacterLibrary.Instance.GetEmotion(emotionText));
        minPitch = speakerProfile.MinPitch;
        maxPitch = speakerProfile.MaxPitch;
        dialogueAudioFrequency = speakerProfile.DialogueAudioFrequency;
        if(speakerProfile.clips.Count > 0)
        {
            dialogueSoundClips = speakerProfile.clips;
        }
        else
        {
            dialogueSoundClips = CharacterLibrary.Instance.GetCharacter("Default").GetAudioProfile(CharacterLibrary.Instance.GetEmotion(emotionText)).clips;
        }
    }

    #region Choice functionality
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
    #endregion

    #endregion

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
