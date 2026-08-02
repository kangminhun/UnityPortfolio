using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecibelScript : MonoBehaviour
{
    public AudioSource audioPlayer;
    public void AudioStop()
    {
        audioPlayer.Stop();
    }
}
