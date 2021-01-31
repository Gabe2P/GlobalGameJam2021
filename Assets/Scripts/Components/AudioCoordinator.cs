using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioCoordinator : MonoBehaviour
{
    [SerializeField] private Sound[] sounds;
    [SerializeField] private GameObject referenceObject = null;
    private ICallAudioEvents reference = null;
    private float timer = 0f;

    private void Start()
    {
        if (referenceObject != null)
        {
            reference = referenceObject.GetComponent<ICallAudioEvents>();
            SubscribeToICallAnimateEvents(reference);
        }

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    public void SetVolume(string name, float amount)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null && amount < 1 && amount > 0)
        {
            Debug.LogWarning("Sound: " + name + " not found or Amount not in Range!");
            return;
        }
        s.source.volume = amount;
    }

    public void SetPitch(string name, float amount)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null && amount < .1f && amount > 3f)
        {
            Debug.LogWarning("Sound: " + name + " not found or Amount not in Range!");
            return;
        }
        s.source.pitch = amount;
    }

    public void SetLoop(string name, bool active)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.loop = active;
    }

    public void Play(string name)
    {
        if (name.Equals("") || name == null)
        {
            return;
        }
        else
        {
            Sound s = Array.Find(sounds, sound => sound.name == name);
            if (s == null)
            {
                Debug.LogWarning("Sound: " + name + " not found!");
                return;
            }
            s.source.Play();
        }
    }

    public bool IsPlayingSound(string name)
    {
        if (name.Equals("") || name == null)
        {
            return false;
        }
        else
        {
            Sound s = Array.Find(sounds, sound => sound.name == name);
            if (s == null)
            {
                Debug.LogWarning("Sound: " + name + " not found!");
                return false;
            }
            return s.source.isPlaying;
        }
    }

    public void Stop(string name)
    {
        if (name.Equals("") || name == null)
        {
            return;
        }
        else
        {
            Sound s = Array.Find(sounds, sound => sound.name == name);
            if (s == null)
            {
                Debug.LogWarning("Sound: " + name + " not found!");
                return;
            }
            s.source.Stop();
        }
    }


    private void CallAudio(string command, float delay)
    {
        if (!IsPlayingSound(command))
        {
            if (timer >= delay)
            {
                Play(command);
                timer = 0;
            }
            else
            {
                timer += Time.deltaTime;
            }
        }
    }

    private void SubscribeToICallAnimateEvents(ICallAudioEvents reference)
    {
        if (reference != null)
        {
            reference.PlayAudio += CallAudio;
        }
    }

    private void UnsubscribeToICallAnimateEvents(ICallAudioEvents reference)
    {
        if (reference != null)
        {
            reference.PlayAudio -= CallAudio;
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
