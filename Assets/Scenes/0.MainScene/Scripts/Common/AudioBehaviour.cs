using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioBehaviour : MonoBehaviour
{
    public AudioSource player;
    private bool play = false;

    public void TogglePlay()
    {
        play = !play;

        if (play)
            player.Play();
        else
            player.Stop();
    }
}
