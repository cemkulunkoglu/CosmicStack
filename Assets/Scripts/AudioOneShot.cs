using UnityEngine;

public static class AudioOneShot
{
    public static void Play2D(AudioClip clip, float volume = 1f)
    {
        if (!clip) return;
        var pos = Camera.main ? Camera.main.transform.position : Vector3.zero;
        AudioSource.PlayClipAtPoint(clip, pos, Mathf.Clamp01(volume));
    }
}
