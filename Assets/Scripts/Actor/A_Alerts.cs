using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StringSprite
{
    public string s;
    public Sprite sprite;

    public StringSprite(string s, Sprite sprite)
    {
        this.s = s;
        this.sprite = sprite;
    }
}

public class A_Alerts : MonoBehaviour
{
    [SerializeField] private GameObject _alertCanvasGO;

    [SerializeField] private AlertUI_Module _alertModulePrefab;

    private List<StringSprite> _queuedAlerts = new List<StringSprite>();

    private float _timer;

    public void Alert(string s, Sprite icon)
    {
        _queuedAlerts.Add(new StringSprite(s, icon));
    }

    private void Update()
    {
        if (_timer > 0)
        {
            _timer -= Time.deltaTime;
        }
        else
        {
            TriggerNextAlert();
        }
    }

    private void TriggerNextAlert()
    {
        if (_queuedAlerts.Count > 0)
        {
            _timer = 1f;
            SpawnAlert(_queuedAlerts[0]);
            _queuedAlerts.RemoveAt(0);
        }
    }

    private void SpawnAlert(StringSprite data)
    {
        AlertUI_Module m = Instantiate(_alertModulePrefab, _alertCanvasGO.transform);
        m.Init(data.s, data.sprite);
    }
}
