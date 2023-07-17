using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class ViewerController : MonoBehaviour
{
    float nextMessageTime = 0.5f;
    [field: SerializeField] int testCounter;
    public float viewerCounter;
    [field: SerializeField] public TextMeshProUGUI ViewerCounterLabel { get; private set; }
    // Start is called before the first frame update
    private float _lastViewerTriggerTime;
    private float _timeDelay;
    void Awake()
    {
    }
    void Start()
    {
        viewerCounter = 1;
        testCounter = 0;
        SubscribeEvents();
    }

    private void OnGameStateChange(GameState oldState, GameState newState)
    {
        if (newState == GameState.StreamerPlaying)
        {
            _lastViewerTriggerTime = Time.time;
            _timeDelay = Random.Range(0.5f, 1.5f);
            viewerCounter = 0;
            ViewerCounterLabel.text = "<color=\"red\">" + (int)viewerCounter;
        }
    }

    void FixedUpdate()
    {
        if (Time.time > _lastViewerTriggerTime + _timeDelay)
        {
            _lastViewerTriggerTime = Time.time;
            _timeDelay = Random.Range(0.5f, 1.5f);
            viewerCounter += Random.Range(4f, 11f);
            ViewerCounterLabel.text = "<color=\"red\">" + (int)viewerCounter;
        }
    }

    public bool CheckProgress()
    {
        return true;
    }

    public bool CheckHype()
    {
        return false;
    }

    public bool CheckFall()
    {
        return false;
    }
    private void SubscribeEvents()
    {
        GameManager.OnGameStateChange += OnGameStateChange;
    }
    private void UnsubscribeEvents()
    {
        GameManager.OnGameStateChange -= OnGameStateChange;
    }
    private void OnDestroy()
    {
        UnsubscribeEvents();
    }
    IEnumerator Loop()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(1);
            testCounter += 1;
            if (CheckProgress())
            {
                viewerCounter += Random.Range(4f, 11f);
            }
            if (CheckHype())
            {
                viewerCounter *= 1.05f;
            }
            if (CheckFall())
            {
                viewerCounter *= 1.5f;
            }
            ViewerCounterLabel.text = "<color=\"red\">" + (int)viewerCounter;
        }
    }
}
