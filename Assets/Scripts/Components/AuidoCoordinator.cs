using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuidoCoordinator : MonoBehaviour
{
    [SerializeField] private AudioManager am = null;
    [SerializeField] private GameObject referenceObject = null;
    private ICallAudioEvents reference = null;

    private void Start()
    {
        if (am == null)
        {
            am = AudioManager.instance;
        }
        if (referenceObject != null)
        {
            reference = referenceObject.GetComponent<ICallAudioEvents>();
            SubscribeToICallAnimateEvents(reference);
        }
    }

    private void CallAudio(string command)
    {
        if (am != null)
        {
            am.Play(command);
        }
    }

    private void SubscribeToICallAnimateEvents(ICallAudioEvents reference)
    {
        if (reference != null)
        {
            reference.CallAudio += CallAudio;
        }
    }

    private void UnsubscribeToICallAnimateEvents(ICallAudioEvents reference)
    {
        if (reference != null)
        {
            reference.CallAudio -= CallAudio;
        }
    }

    private void OnEnable()
    {
        SubscribeToICallAnimateEvents(reference);
    }

    private void OnDisable()
    {
        UnsubscribeToICallAnimateEvents(reference);
    }
}
