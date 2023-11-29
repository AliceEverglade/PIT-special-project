using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceButton : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private TMP_Text txt;
    private int index;

    public Action<int> OnButtonPressed;
    public void Init(string text, int index)
    {
        this.index = index;
        txt.text = text;
        OnButtonPressed += DialogueManager.Instance.MakeChoice;
    }
    private void OnEnable() => button.onClick.AddListener(OnPressed);
    public void OnPressed() => OnButtonPressed?.Invoke(index);
}
