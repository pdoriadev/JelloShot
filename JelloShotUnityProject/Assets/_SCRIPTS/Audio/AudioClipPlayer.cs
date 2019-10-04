using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class AudioClipPlayer : MonoBehaviour
{
    public ClipHolder clipHolder;
    public List<string> clipNames;
    

    private void OnEnable()
    {
        clipHolder.audioSource = GetComponent<AudioSource>();       
    }

    // sends clipholder object to audio manager to be read through and played. 
    public void PlayAudio()
    {
        AudioManager.instance.PlayAudioFromSource(clipHolder);
    }

}

public struct ClipHolder
{
    public AudioSource audioSource;
    public string clipName;
    public AudioUse audioUse;
    public bool isOneShot;
    public ClipHolder (AudioSource _audioSource, string _name, AudioUse _audioUse, bool _isOneshot)
    {
        audioSource = _audioSource;
        clipName = _name;
        audioUse = _audioUse;
        isOneShot = _isOneshot;
    }
}

public enum AudioUse
{
    SFX,
    Music,
    Dialogue
}

public enum PlayType
{
    Play,
    PlayOneShot,
    PlayScheduled,
    PlayOnDelay,
    Pause
}

public class ClipSetter
{
    
}

