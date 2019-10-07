using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[RequireComponent(typeof(AudioSource))]
public class AudioClipPlayer : MonoBehaviour
{
    public ClipHolder clipHolder;
    public string audioFileName;
    public string stitchedFileName;
    public bool shouldResyncLists = false;

    public List<string> sFXNames;
    public List<string> musicNames;

    public AudioUseCase useCase = AudioUseCase.SFX;
    [SerializeField]
    public AudioPlayType playType = AudioPlayType.PlayOneShot;

    public float delayTime;
    

    private void OnEnable()
    {
        clipHolder.audioSource = GetComponent<AudioSource>();
        
    }


    /// <summary>
    /// sends clipholder object to audio manager to be read through and played. 
    /// NOTE: Only send clips of one type if sending multiple clips. AudioManager needs edit to check for mutliple AudioUse types. 
    /// </summary>
    public void PlayAudio()
    {
        clipHolder.audioUse = useCase;
        clipHolder.primaryClipName = audioFileName;
        clipHolder.playType = playType;
        clipHolder.stitchedClipName = stitchedFileName;
        clipHolder.delayTime = delayTime;

        AudioManager.instance.PlayAudioFromSource(clipHolder);
    }
    public void SetClipname(string _clipName)
    {
        audioFileName = _clipName;
    }
    public void SetPlayTypeAsPlay()  { playType = AudioPlayType.Play; }
    public void SetPlayTypeAsPlayOneShot() { playType = AudioPlayType.PlayOneShot; }
    public void SetPlayTypeAsPlayScheduled() { playType = AudioPlayType.PlayScheduled; }
    public void SetPlayTypeAsPlayDelayed() { playType = AudioPlayType.PlayDelayed; }
    public void SetPlayTypeAsPause() { playType = AudioPlayType.Pause; }
    public void SetPlayTypeAsUnpause() { playType = AudioPlayType.UnPause; }

    public bool CheckIfNamesMatch()
    {
        if (sFXNames != AudioManager.instance.storageInstance.sfXNames || musicNames != AudioManager.instance.storageInstance.musicNames)
        {
            return false;
        }
        return true;
    }
    public void SyncNames()
    {
        sFXNames.Clear();
        musicNames.Clear();
        sFXNames.AddRange(AudioManager.instance.storageInstance.sfXNames);
        musicNames.AddRange(AudioManager.instance.storageInstance.musicNames);
        //sFXNames = AudioManager.instance.storageInstance.sfXNames;
        //musicNames = AudioManager.instance.storageInstance.musicNames;
    }

}

public struct ClipHolder
{
    public AudioSource audioSource;
    public string primaryClipName;
    // use this if you want to stitch clips together using PlayScheduled
    public string stitchedClipName;
    public AudioUseCase audioUse;
    public AudioPlayType playType;
    public float delayTime;
    public ClipHolder (AudioSource _audioSource, string _name, string _otherName, AudioUseCase _audioUse, AudioPlayType _playType, float _delayTime)
    {
        audioSource = _audioSource;
        primaryClipName = _name;
        stitchedClipName = _otherName;
        audioUse = _audioUse;
        playType = _playType;
        delayTime = _delayTime;
    }
}

public enum AudioUseCase
{
    SFX,
    Music,
    Dialogue
}

public enum AudioPlayType
{
    Play,
    PlayOneShot,
    PlayScheduled,
    PlayDelayed,
    Pause,
    UnPause
}

#if UNITY_EDITOR
[CustomEditor(typeof(AudioClipPlayer))]
public class ClipSetter : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector(); // for other non-HideInInspector fields

        AudioClipPlayer _AudioClipPlayer = (AudioClipPlayer)target;
        AudioManager.instance = GameObject.FindGameObjectWithTag("audioManager").GetComponent<AudioManager>();
        if (AudioManager.instance.justRanSetup == true || _AudioClipPlayer.shouldResyncLists == true)
        {
            Debug.Log("Presync");
            _AudioClipPlayer.sFXNames.Clear();
            _AudioClipPlayer.musicNames.Clear();
            _AudioClipPlayer.sFXNames.AddRange(AudioManager.instance.storageInstance.sfXNames);
            _AudioClipPlayer.musicNames.AddRange(AudioManager.instance.storageInstance.musicNames);
            Debug.Log("Postsync");
            _AudioClipPlayer.shouldResyncLists = false;
        }
    }
}
#endif
