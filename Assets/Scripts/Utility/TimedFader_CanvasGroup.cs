using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TimedFader_CanvasGroup : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;

    public bool oneTime;
    [SerializeField] private int _savedIndexIfOneTime;
    public bool fadeInOnStart;
    public bool fadedInOnStart = false;
    [SerializeField] private float _waitForFadeIn;
    [SerializeField] private float _timeToFadeIn;

    public event Action FadeComplete;

    private void OnFadeComplete()
    {
        if (FadeComplete != null)
        {
            FadeComplete.Invoke();
        }
    }


    private void Awake()
    {
        if (oneTime)
        {
            int happen = 1;

            string version = PlayerPrefs.GetString("Version", "0");

            if (version == Application.version)
            {
                happen = PlayerPrefs.GetInt("happened" + _savedIndexIfOneTime.ToString(), 1);
            }

            if (happen == 1)
            {
                PlayerPrefs.SetInt("happened" + _savedIndexIfOneTime.ToString(), 0);
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
    }

    private void Start()
    {
        if (fadeInOnStart)
        {
            FadeIn(_waitForFadeIn, _timeToFadeIn);
        }
        else if (fadedInOnStart)
        {
            _canvasGroup.alpha = 1;
        }
        else
        {
            _canvasGroup.alpha = 0;
        }
    }

    public void FadeIn(float wait, float timeTo)
    {
        StartCoroutine(Fade(wait, timeTo, 0, 1));
    }

    public void FadeOut(float wait, float timeTo)
    {
        StartCoroutine(Fade(wait, timeTo, 1, 0));
    }

    private IEnumerator Fade(float wait, float timeTo, float startVal, float endVal)
    {
        float timer = timeTo;
        _canvasGroup.alpha = startVal;
        yield return new WaitForSeconds(wait);


        while (timer > 0)
        {
            timer -= Time.deltaTime;
            _canvasGroup.alpha = Mathf.Lerp(startVal, endVal, Util.MapValue(timer, timeTo, 0, 0, 1));
            yield return 0f;
        }
        OnFadeComplete();
    }
}