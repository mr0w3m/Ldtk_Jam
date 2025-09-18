using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//A Displayer of conversation. Handles all things displaying conversation.

public class HUD_Conversation : MonoBehaviour
{
    public static HUD_Conversation i;
    private void Awake()
    {
        if (i != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            i = this;
        }
    }

    public event Action ConversationComplete;

    private void OnConversationComplete()
    {
        if (ConversationComplete != null)
        {
            ConversationComplete.Invoke();
        }
    }


    [SerializeField] private ConversationView _view;
    [SerializeField] private Transform _conversationHolder;
    [SerializeField] private A_Input _alternateInput;

    private ConversationComponent _currentConversation;
    private int _currentConversationStep;

    public void StartConversation(ConversationComponent convo)
    {
        Debug.Log("StartConv");
        ConversationComponent con = Instantiate(convo, _conversationHolder) as ConversationComponent;
        _currentConversation = con;
        _currentConversationStep = 0;
        _view.Show();

        if (Actor.i != null)
        {
            Actor.i.input.ADown += ProgressDialogue;
        }
        else
        {
            if (_alternateInput != null)
            {
                _alternateInput.ADown += ProgressDialogue;
            }
        }
        ProgressDialogue();
    }

    private void ProgressDialogue()
    {
        if (_currentConversationStep < _currentConversation.dialogues.Count)
        {
            Debug.Log("StepConversation: " + _currentConversationStep);
            LoadDialogue(_currentConversation.dialogues[_currentConversationStep]);
            _currentConversationStep++;

        }
        else
        {
            CloseDialogue();
            Debug.Log("current conversation step is less than the dialogues left");
        }
    }

    private void CloseDialogue()
    {
        if (Actor.i != null)
        {
            Actor.i.input.ADown -= ProgressDialogue;
        }
        else
        {
            _alternateInput.ADown -= ProgressDialogue;
        }
        _view.Hide();
        _currentConversationStep = 0;
        _currentConversation = null;
        OnConversationComplete();
        //Actor.i.paused = false;
        //Actor.i.movement.PauseMovement = true;
    }

    private void LoadDialogue(DialogueData data)
    {
        _view.SetTitleText(data.characterSpeaking.characterName);
        _view.SetDialogueText(data.dialogue);
    }
}