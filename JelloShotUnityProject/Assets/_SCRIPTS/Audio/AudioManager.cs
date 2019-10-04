using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEditor;
// audio sources. audio mixers. audio snapshots. mixer is already setup. only switch if need arises. 
// snapshots are switched more often than mixer
// audio sources are switched out the most. 

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public bool _RunClipSetup = false;

    [SerializeField]
    public AudioStorage holderInstance;

    private AudioClip _Clip;

    #region Unity Callbacks
    private void OnEnable()
    {
        if (instance == null)
            instance = this;
    }
    private void OnDisable()
    {
        instance = null;
    }
    #endregion

    public void PlayAudioFromSource(ClipHolder _clipHolder)
    {
        _Clip = null;
        if (_clipHolder.audioUse == AudioUse.SFX)
        {
            for (int i = 0; i < holderInstance.sFXClips.Count; i++)
            {
                if (_clipHolder.clipName == holderInstance.sFXClips[i].name)
                {
                    _Clip = holderInstance.sFXClips[i];
                    break;
                }      
            }

        }
        if (_clipHolder.audioUse == AudioUse.Music)
        {
            for (int i = 0; i < holderInstance.musicClips.Count; i++)
            {
                if (_clipHolder.clipName == holderInstance.musicClips[i].name)
                {
                    _Clip = holderInstance.sFXClips[i];
                    break;
                }
            }
        }

        if (_clipHolder.isOneShot)
            _clipHolder.audioSource.PlayOneShot(_Clip);
        else _clipHolder.audioSource.Play();
    }
}

public class AudioStorage
{
    public List<AudioClip> sFXClips;
    public List<string> sfXNames;
    public List<AudioClip> musicClips;
    public List<string> musicNames;

    public void SetupNames()
    {
        for (int i = 0; i < sFXClips.Count - 1; i++)
        {
            sfXNames[i] = sFXClips[i].name;
            musicNames[i] = musicClips[i].name;
        }
    }
    public bool CheckIfNamesMatchClips()
    {
        for (int i = 0; i < sFXClips.Count - 1; i++)
        {
            if (sfXNames[i] != sFXClips[i].name)
            {
                return false;
            }
            if (musicNames[i] != musicClips[i].name)
            {
                return false;
            }
           
        }
        return true;
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(AudioManager))]
public class AudioSetup : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector(); // for other non-HideInInspector fields

        AudioManager _AudioManager = (AudioManager)target;

      
        if (_AudioManager._RunClipSetup && _AudioManager.holderInstance.CheckIfNamesMatchClips())
        {
            _AudioManager._RunClipSetup = false ;
        }
        else if (_AudioManager._RunClipSetup || !_AudioManager.holderInstance.CheckIfNamesMatchClips())
        {
            _AudioManager.holderInstance.SetupNames();
        }
    }
}
#endif


// Considerations from https://johnleonardfrench.com/articles/10-unity-audio-tips-that-you-wont-find-in-the-tutorials/
/// <summary>
/// // PlayOneShot vs Play
/// PlayOneShot doesnot interrupt clips already playing. Great for repeating sounds
/// PlayOneShot requires specifying the clip you want to play. Easier to use different clips for variation
/// Can set clip volume when calling PlayOneShot. 
/// 
/// Best for short, non-looping clips. Useful for triggering multiple sounds from same audio source 
///         w/o interrupting each other. 
/// Play function is great for anything that loops. 
/// 
/// // Big AudioMixer vs. multiple Mixers
/// Simple game, simple soundscape, go for Big AudioMixer.
/// Otherwise, multiple mixers can be more performant. 
/// 
/// // Pausing all audio
/// AudioListener.pause = true
/// If you want a few audio sources to ignore the pause, use AudioSource.ignoreListenerPause=true;
/// 
/// // Soften 3D sounds with Audio Source Spread setting
/// First, you need to increase the spatial blend value before Spread takes effect.
/// Spread takes a value between 0 and 360.
///     0 degrees (defualt). 3D sounds hardpan to their direction of origin. Sound from the left only goes through left speaker.
///     360 degrees is the opposite of 0. A sound on the left will be heard only through the right speaker.
///     At 180 degrees, sound is spread evenly between speakers. 
/// Look at the article for more info on how to use spread to achieve a 3D soundscape
///     Also notes on Spread's differences to Spatial Blend. 
///     
/// // Create audio zones only using primitive colliders
/// Combine multiple colliders to create a single audio zone in Unity
/// 1. Whenever the player enters or exits a collider, keep count using a simple script (see article for example).
/// 2. Every time an enter trigger is fired, add one to the total and for an exit, subtract one.
/// 3. If the total is 1 or higher, the player is inside the (in this case for the example) the saloon. If it's 0, they're outside. 
/// 
/// Solves the problem without using a mesh collider. 
/// 
/// // Stitch audio clips together seamlessly : PlayScheduled
/// For anything that needs sound played at ultra-precise times (i.e. dynamic music, beat matching), PlayScheduled is a must. 
/// audioSource.PlayScheduled(double)
/// the DSP time value you need to pass in is a point in time, not a delay. Need to either record or reference the current DSP time in order to use PlayScheduled.
/// For example: audioSource.PlayScheduled(AudioSettings.dspTime + 3);
/// 
/// However, the real power of PlayScheduled is in stitching together audioclips seamlessly. 
/// 
/// 1. Work out the length of the first clip as a double value. Get this by casting the clip's total samples as a double
///     and then divide it by the sample rate. This gives you a super accurate duration value.
///     double clipDuration = (double)firstAudioSource.clip.samples / firstAudioSource.clip.frequency;
/// 2. Setup two audio sources. You must use 2 audio sources, as it's impossible to schedule an audio source to play what's already playing,
///     or that's already scheduled to play. However, if you intend to trigger more than two clips in a row, siply toggle between
///     the two audio sources as you schedule clips.
/// 3. Schedule audio clips. Start the first clip using PlayScheduled. This way you can use the exact start time in your calculation.
///     Schedule the first clip to play at the current Audio DSP time, plus a small delay ( to make sure it plays at the correct time. 
///     Next, schedule the secondd audio source to play at the current DSP time plus the small delay plus the first clip's duration.
///     firstAudioSource.PlayScheduled(AudioSettings.dspTime + 0.1);
///     secondAudioSource.PlayScheduled(AudioSettings.dspTime + 0.1 + clipDuration);
///  
/// For more info on PlayScheduled
///     ultimate guide to Play Scheduled in Unity: https://johnleonardfrench.com/articles/ultimate-guide-to-playscheduled-in-unity/
/// 
/// // Prevent audio popping in unity when using AudioMixer Snapshots
/// audio popping when witching between snapshots. Happens when changing group volume using snapshots, specifically
///     when the default snapshot includes a group that's set to exactly 0db (default volume).
///     Setting the volume of that group to a slightly lower vale value, such as -0.05db, instantly fixes the problem.
///     
/// // Fade multiple audio effects by blending snapshots in real time
/// TransitionToSnapshots function allows you to blend two or more snapshots continuously by using different float weightings
///     and an array of snapshots to blend. 
///     Great for connecting multiple audio mixer effects to in-game values, without having to individually expose their parameters to scripting. 
///     Example in article: creating a low pas filter that gradually blends in as the player starts to lose health.
///     
/// See article for how it works. 
/// 
/// // Make user volume controls (that actually work)
/// Problem:  UI slider value range is linear while an Audio Mixer Group's volume is measured using  a logarithmic scale.
/// Solution: 
///     1. Make slider as usual, but use a slider value of 0.0001 - 1 (exactly)
///     2. Expose the Audio Mixer Group fader to scripting (select the group and right click on the value label in the inspector)
///     3. Name the exposed parameter in the Audio Mixer window.
///     4. Create a script to set the exposed parameter, using the calculation below to convert the value:
///         AudioMixer.SetFloat("exposedVolumeParam", Mathf.Log10(sliderValueGoesHere) * 20);
///      5. Connect the slider  to the xcript, using the OnValueChanged event. 
///  For a full guide, go here: https://johnleonardfrench.com/articles/the-right-way-to-make-a-volume-slider-in-unity-using-logarithmic-conversion/
///  
/// When you don’t need to do this! -->  Setting the Audio Source volume directly
///  
/// // Crossfade aaudio in unity (without a dip in the middle)
/// </summary>
