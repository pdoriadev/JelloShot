using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioStorage : MonoBehaviour
{
    public List<AudioClip> sFXClips;
    public List<string> sfXNames;
    public List<AudioClip> musicClips;
    public List<string> musicNames;

    public void SetupNames()
    {
        sfXNames.Clear();
        musicNames.Clear();
        for (int i = 0; i < sFXClips.Count; i++)
        {
            if (sFXClips[i] != null)
            {
                sfXNames.Add(sFXClips[i].name);               
            }
            else Debug.Log("sfx clip at index " + i + " of " + sFXClips.ToString() + " is null" );

        }
        for (int i = 0; i < musicClips.Count; i++)
        {
            if (musicClips[i] != null)
            {
                musicNames.Add(musicClips[i].name);
            }
             else Debug.Log("sfx clip at index " + i + " of " + musicClips.ToString() + " is null");
        }
        //sfXNames.RemoveRange(sFXClips.Count, sfXNames.Count - sFXClips.Count);
        //musicNames.RemoveRange(musicClips.Count, musicNames.Count - musicClips.Count);
        Debug.Log('r');
    }
    public bool CheckIfNamesMatchClips()
    {
        Debug.Log("pre-check");
        for (int i = 0; i < sFXClips.Count - 1; i++)
        {
            if (sfXNames[i] != sFXClips[i].name)
            {
                return false;
            }
        }
        for (int i = 0; i < musicClips.Count - 1; i++)
        {
            if (musicNames[i] != musicClips[i].name)
            {
                return false;
            }
        }
        return true;
    }
}
