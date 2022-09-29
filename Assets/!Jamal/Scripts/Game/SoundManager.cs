using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(Instance);
        }
    }

    [Header("Effects")]

    public AudioClip button_Menu;
    public AudioClip button_DragonSelect;

    [Space]

    [Header("BG")]

    public AudioClip[] BackGround;

    [Space]

    [Header("Audio Source")]

    public AudioSource source_OneShot;
    public AudioSource source_BG;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void PlayBG(int indexBG)
    {
        source_BG.clip = BackGround[indexBG];
        source_BG.Play();
    }

    public void PlayOneShot(AudioClip clip)
    {
        source_OneShot.PlayOneShot(clip);
    }
}
