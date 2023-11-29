using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] private GameObject popup;
    private bool playerInRange;

    [SerializeField] private TextAsset inkJson;

    private void Awake()
    {
        playerInRange = false;
        popup.SetActive(false);
    }

    private void Update()
    {
        
        if(playerInRange && !DialogueManager.Instance.DialogueIsPlaying)
        {
            popup.SetActive(true);
            if (Input.GetKeyDown(KeyCode.F))
            {
                DialogueManager.Instance.EnterDialogueMode(inkJson);
            }
        }
        else
        {
            popup.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = true;
        }
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}
