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
    public bool runClipSetup = false;
    public bool justRanSetup = false;

    public AudioStorage storageInstance;

    private AudioClip _Clip;
    // in the event playscheduled is used
    private AudioClip _StitchedClip;
    private float _ChangedPitch = 1f;

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
        _StitchedClip = null;

        if (_clipHolder.audioUse == AudioUseCase.SFX)
        {
            bool primClipFound = false;
            bool stitchedClipFound = false;
            for (int i = 0; i < storageInstance.sFXClips.Count; i++)
            {
                if (primClipFound == false && _clipHolder.primaryClipName == storageInstance.sFXClips[i].name)
                {
                    _Clip = storageInstance.sFXClips[i];
                    primClipFound = true;
                }
                if (_clipHolder.stitchedClipName != null)
                {
                    if (stitchedClipFound == false && _clipHolder.stitchedClipName == storageInstance.sFXClips[i].name)
                    {
                        _StitchedClip = storageInstance.sFXClips[i];
                        stitchedClipFound = true;
                    }
                }
                else stitchedClipFound = true;

                if (primClipFound && stitchedClipFound)
                    break;
               // else Debug.LogWarning("No clip found for " + _clipHolder.audioSource.gameObject);
            }
        }
        if (_clipHolder.audioUse == AudioUseCase.Music)
        {
            bool primClipFound = false;
            bool stitchedClipFound = false;
            for (int i = 0; i < storageInstance.musicClips.Count; i++)
            {
                if (primClipFound == false && _clipHolder.primaryClipName == storageInstance.musicClips[i].name)
                {
                    _Clip = storageInstance.musicClips[i];
                    primClipFound = true;
                }
                if (_clipHolder.stitchedClipName != null)
                {
                    if (stitchedClipFound == false && _clipHolder.stitchedClipName == storageInstance.musicClips[i].name)
                    {
                        _StitchedClip = storageInstance.musicClips[i];
                        stitchedClipFound = true;
                    }
                }
                // if stitchedClipName == null and primClipFound
                else if (primClipFound)
                    break;
                if (primClipFound && stitchedClipFound)
                    break;
                //else Debug.LogWarning("No clip found for " + _clipHolder.audioSource.gameObject);
            }
        }

        if (_clipHolder.playType == AudioPlayType.Play)
        {
            _clipHolder.audioSource.clip = _Clip;
            _clipHolder.audioSource.Play();
        }
        else if (_clipHolder.playType == AudioPlayType.PlayDelayed)
        {
            _clipHolder.audioSource.PlayDelayed(_clipHolder.delayTime);
        }
        else if (_clipHolder.playType == AudioPlayType.PlayOneShot)
        {
            if (_clipHolder.shouldAffectPitch)
            {

                if (_ChangedPitch > 3f)
                {
                    _ChangedPitch = 1;
                }
                else
                {
                    _ChangedPitch += 0.1f;
                    _clipHolder.audioSource.pitch = _ChangedPitch;
                }
            }

            _clipHolder.audioSource.PlayOneShot(_Clip);
        }

        // How PlayScheduled works OR how to stitch together clips seamlsessly  
        else if (_clipHolder.playType == AudioPlayType.PlayScheduled)
        {
            //1.Work out the length of the first clip as a double value. Get this by casting the clip's total samples as a double
            /// and then divide it by the sample rate.This gives you a super accurate duration value.
            double clipDuration = (double)_clipHolder.audioSource.clip.samples / _clipHolder.audioSource.clip.frequency;

            /// 2.Setup two audio sources.You must use 2 audio sources, as it's impossible to schedule an audio source to play what's already playing,
            /// or that's already scheduled to play. However, if you intend to trigger more than two clips in a row, siply toggle between
            /// the two audio sources as you schedule clips.
            AudioSource otherAudioSource;
            AudioSource[] sourcesOnGO = _clipHolder.audioSource.gameObject.GetComponents<AudioSource>();
            if (sourcesOnGO.Length < 2)
            {
                otherAudioSource = _clipHolder.audioSource.gameObject.AddComponent<AudioSource>();
                otherAudioSource.clip = _Clip;

                _clipHolder.audioSource.PlayScheduled(AudioSettings.dspTime + 0.1f);
                otherAudioSource.PlayScheduled(AudioSettings.dspTime + 0.1f + clipDuration);
            }
            else
            {
                for (int i = 0; i < _clipHolder.audioSource.gameObject.GetComponents<AudioSource>().Length; i++)
                {
                    if (_clipHolder.audioSource != sourcesOnGO[i])
                    {
                        otherAudioSource = sourcesOnGO[i];
                        otherAudioSource.PlayScheduled(clipDuration);
                        break;
                    }
                    else Debug.LogWarning("Couldn't find another audio source on " + _clipHolder.audioSource.gameObject.name);
                }    
            }
            /// 3. (Look in the above if statements) Schedule audio clips. Start the first clip using PlayScheduled.
            /// This way you can use the exact start time in your calculation.
            /// Schedule the first clip to play at the current Audio DSP time, pl-us a small delay(to make sure it plays at the correct time.
            /// Next, schedule the second audio source to play at the current DSP time plus the small delay plus the first clip's duration.
        }
        else if (_clipHolder.playType == AudioPlayType.Pause)
        {
            _clipHolder.audioSource.Pause();
        }     
        else if (_clipHolder.playType == AudioPlayType.UnPause)
        {
            _clipHolder.audioSource.UnPause();
        }
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

        if (_AudioManager.storageInstance == null)
            _AudioManager.storageInstance = _AudioManager.gameObject.GetComponent<AudioStorage>();
        Debug.Log("Pre ifs");
        // Checks if names and audio files need to be resynced. 
        if (_AudioManager.justRanSetup)
        {
            _AudioManager.justRanSetup = false;
            Debug.Log("just ran setup setting to false");
        }

        else if (_AudioManager.runClipSetup || !_AudioManager.storageInstance.CheckIfNamesMatchClips())
        {
            Debug.Log("pre-setup setup");
            _AudioManager.storageInstance.SetupNames();
            _AudioManager.runClipSetup = false;

            Debug.Log("post setup");

            // AudioClipPlayers check if this is true to know when to subscribe to know when to resync their clipName lists. 
            _AudioManager.justRanSetup = true;
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
