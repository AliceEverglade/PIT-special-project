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

    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;

    [Header("Choices UI")]
    [SerializeField] private GameObject choiceHolder;
    [SerializeField] private GameObject choicePrefab;
    [SerializeField] private List<GameObject> choiceList;


    private Story currentStory;

    public bool DialogueIsPlaying { get; private set; }

    private void Awake()
    {
        if(Instance != null)
        {
            Debug.LogWarning(" DialogueManager instance already exists");
        }
        Instance = this;
        Debug.Log("Instance Set");
    }

    private void Start()
    {
        DialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
    }

    private void Update()
    {
        if(!DialogueIsPlaying) return;

        if (currentStory.currentChoices.Count == 0 && Input.GetKeyDown(KeyCode.Return))
        {
            ContinueStory();
        }
    }

    public void EnterDialogueMode(TextAsset inkJson)
    {
        currentStory = new Story(inkJson.text);
        DialogueIsPlaying = true;
        dialoguePanel.SetActive(true);

        ContinueStory();
    }

    private IEnumerator ExitDialogueMode()
    {
        yield return new WaitForSeconds(0.2f);
        DialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
    }

    private void ContinueStory()
    {
        if (currentStory.canContinue)
        {
            dialogueText.text = currentStory.Continue();
            DisplayChoices();
        }
        else
        {
            StartCoroutine(ExitDialogueMode());
        }
    }

    private void DisplayChoices()
    {
        foreach (GameObject choice in choiceList)
        {
            Destroy(choice);
        }
        choiceList.Clear();
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

    public void MakeChoice(int choiceIndex)
    {
        currentStory.ChooseChoiceIndex(choiceIndex);
        ContinueStory();
    }

    private IEnumerator SelectFirstChoice(GameObject first) // might be redundant, tutorial told me to do it
    {
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(first);
    }
}
