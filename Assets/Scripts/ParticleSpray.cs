using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSpray : MonoBehaviour, ISprayable
{
    public FountainType FountainType;
    public FountainType GetFountainType() { return FountainType; }

    [SerializeField] protected ParticleSystem spray = null;
    [SerializeField] protected AudioClip soundclip = null;
    [SerializeField] protected AudioSource fountainSound = null;

    protected virtual void Start()
    {
        if (spray == null) spray = GetComponent<ParticleSystem>();
        if (fountainSound == null) fountainSound = GetComponent<AudioSource>();
        spray.Pause();
    }

    void Update()
    {
        
    }

    public void Spray()
    {
        spray.Play();
        fountainSound.clip = soundclip;
        fountainSound.volume = 1;
        fountainSound.Play();
    }

    public virtual void Stop()
    {
        StartCoroutine(stopFountain());
    }


    IEnumerator stopFountain()
    {
        yield return new WaitForSeconds(5);
        spray.Stop();
        for (var i = 0; i < 10; i++)
        {
            fountainSound.volume -= (i * 0.1f);
            yield return new WaitForSeconds(0.15f);
        }
        fountainSound.Stop();
        fountainSound.volume = 1;

    }

   
}
