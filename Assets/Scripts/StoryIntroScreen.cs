using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryIntroScreen : MonoBehaviour
{
    [SerializeField] private A_Input _input;
    [SerializeField] private SceneController _sceneController;
    [SerializeField] private string _sceneToLoad;
    [SerializeField] private TimedFader_CanvasGroup _fader;
    [SerializeField] private ConversationComponent _convoAtFire;
    [SerializeField] private ConversationComponent _convoBlackScreen;

    private void Start()
    {
        _fader.FadeComplete += StartConvoAtFire;
        _fader.FadeOut(2f, 2f);
    }

    private void StartConvoAtFire()
    {
        _fader.FadeComplete -= StartConvoAtFire;
        HUD_Conversation.i.ConversationComplete += FadeOut;
        HUD_Conversation.i.StartConversation(_convoAtFire);
    }

    private void FadeOut()
    {
        HUD_Conversation.i.ConversationComplete -= FadeOut;
        _fader.FadeComplete += StartConvoBlackScreen;
        _fader.FadeIn(1f, 3f);
    }

    private void StartConvoBlackScreen()
    {
        _fader.FadeComplete -= StartConvoBlackScreen;
        HUD_Conversation.i.ConversationComplete += NextScene;
        HUD_Conversation.i.StartConversation(_convoBlackScreen);
    }

    private void NextScene()
    {
        HUD_Conversation.i.ConversationComplete -= NextScene;
        _sceneController.LoadScene(_sceneToLoad);
    }
}
