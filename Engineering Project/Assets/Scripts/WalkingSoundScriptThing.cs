using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingSoundScriptThing : MonoBehaviour
{
    //public AudioClip footstep1, footstep2, footstep3;
    private AudioSource mainSource;
    //public AudioSource source1, source2, source3;

    //public Dictionary<int, AudioClip> soundDict = new Dictionary<int, AudioClip>();
    public List<AudioSource> soundList = new List<AudioSource>();

    private int sourceNumber = 0;
    public bool isStepping = false;

    // Start is called before the first frame update
    void Start()
    {
        /*source = gameObject.GetComponent<AudioSource>();
        soundList.Add(source1);
        soundList.Add(source2);
        soundList.Add(source3);*/
    }

    public void SoundCall()
    {
        sourceNumber = Random.Range(0, soundList.Count);
        mainSource = soundList[sourceNumber];
        mainSource.Play();
        isStepping = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isStepping)
        {
            SoundCall();
        }
    }
}
