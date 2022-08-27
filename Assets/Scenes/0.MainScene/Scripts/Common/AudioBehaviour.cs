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
        player.mute = play;
    }
}
