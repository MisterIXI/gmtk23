using UnityEngine;

public class SoundController : MonoBehaviour
{
    private AudioSource _audio;

    private void Awake()
    {

    }
    private void SubscribeEvents()
    {

    }

    private void UnSubscribeEvents()
    {

    }

    private void OnDestroy()
    {
        UnSubscribeEvents();
    }
}