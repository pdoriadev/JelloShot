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
    public bool nextClipShouldLoop;
    public bool affectedByPitch;

    public List<string> sFXNames;
    public List<string> musicNames;

    public AudioUseCase useCase = AudioUseCase.SFX;
    [SerializeField]
    public AudioPlayType playType = AudioPlayType.PlayOneShot;

    public float delayTime;

    private AudioSource _Audio;
    
    private void OnEnable()
    {
        _Audio = GetComponent<AudioSource>();
    }

    /// <summary>
    /// sends clipholder object to audio manager to be read through and played. 
    /// NOTE: Only send clips of one type if sending multiple clips. AudioManager needs edit to check for mutliple AudioUse types. 
    /// </summary>
    public void PlayAudio()
    {
        if (nextClipShouldLoop)
            _Audio.loop = true;
        else _Audio.loop = false;
        if (affectedByPitch)
            clipHolder.affectedByPitch = true;
        else clipHolder.affectedByPitch = false;

        clipHolder.primaryClipName = audioFileName;
        if (stitchedFileName != null)
            clipHolder.stitchedClipName = stitchedFileName;
        clipHolder.audioUse = useCase;
        clipHolder.playType = playType;
        clipHolder.delayTime = delayTime;
        clipHolder.audioSource = _Audio;
        Debug.Log(clipHolder.audioSource);

        AudioManager.instance.PlayAudioFromSource(clipHolder);

        nextClipShouldLoop = false;
        clipHolder.affectedByPitch = false;
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
    public void SetNextClipShouldLoopToTrue()
    {
        nextClipShouldLoop = true;
    }
    public void SetAffectedByPitchToTrue()
    {
        affectedByPitch = true;
    }

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
    public bool shouldLoop;
    public bool affectedByPitch;
    public ClipHolder (AudioSource _audioSource, string _name, string _otherName, AudioUseCase _audioUse, AudioPlayType _playType, float _delayTime, bool _shouldLoop, bool _affectedByPitch)
    {
        audioSource = _audioSource;
        primaryClipName = _name;
        stitchedClipName = _otherName;
        audioUse = _audioUse;
        playType = _playType;
        delayTime = _delayTime;
        shouldLoop = _shouldLoop;
        affectedByPitch = _affectedByPitch;
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
