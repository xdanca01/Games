using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSound : MonoBehaviour
{
    // Start is called before the first frame update
    private AudioSource source;
    public AudioClip clip;
    void Start()
    {
        source = GameObject.FindGameObjectWithTag("Music").GetComponent<AudioSource>();
    }

    public void ClickSound()
    {
        source.PlayOneShot(clip);
    }
}
