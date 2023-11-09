using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class PopcornSpray : MonoBehaviour, ISprayable
{

    public FountainType FountainType = FountainType.POPCORN;
    public FountainType GetFountainType() { return FountainType; }


    [SerializeField] private PopcornPopper popcornPopper;
    //[SerializeField] private AudioClip waterSound = null;
    //[SerializeField] private AudioSource fountainSound = null;

    void Start()
    {
        popcornPopper.popEnabled = false;
    }


    public void Spray()
    {
        popcornPopper.popEnabled = true;
    }

    public async void Stop()
    {
        await Task.Delay(5000);
        popcornPopper.resetPopcorn();
    }

}
