using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConversationView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _titleText;
    [SerializeField] private TextMeshProUGUI _dialogueText;
    [SerializeField] private GameObject _holder;

    public void SetTitleText(string text)
    {
        _titleText.text = text;
    }

    public void SetDialogueText(string text)
    {
        _dialogueText.text = text;
    }

    public void Show()
    {
        _holder.SetActive(true);
    }

    public void Hide()
    {
        _holder.SetActive(false);
    }
}