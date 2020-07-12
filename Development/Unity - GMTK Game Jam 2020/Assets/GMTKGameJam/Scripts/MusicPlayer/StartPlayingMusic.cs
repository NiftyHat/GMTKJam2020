using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPlayingMusic : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        MusicPlayer m = FindObjectOfType<MusicPlayer>();
		  m?.Play();
    }
}
