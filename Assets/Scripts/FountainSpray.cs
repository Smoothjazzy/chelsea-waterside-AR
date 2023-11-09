using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using Random = UnityEngine.Random;

public interface ISprayable
{
    void Spray();
    void Stop();
    FountainType GetFountainType();

}


public class FountainSpray : MonoBehaviour {

    public ISprayable currentFountain;
    
    public ParticleSpray waterSpray;
    public PopcornSpray popcornPopper;
    public ParticleSpray bubbleSpray;
    public ParticleSpray sprinkleSpray;
    public ParticleSpray confettiSpray;

    private ISprayable[] sprays;

    private AudioSource fountainSound;
    public AudioClip waterSound;
    public AudioClip bubbleSound;
    public AudioClip sprinkleSound;
    public AudioClip confettiSound;

	void OnMouseDown () {
        StartCoroutine(FountainTouchResponse());
        currentFountain.Spray();
    }

	void OnMouseUp () {
        currentFountain.Stop();
    } 

	void Awake () {

        if (fountainSound == null) fountainSound = GetComponent<AudioSource>();
        sprays = new ISprayable[] { waterSpray, popcornPopper, bubbleSpray, sprinkleSpray, confettiSpray }; // { WATER, POPCORN, BUBBLES, SPRINKLES, CONFETTI}
        
    }

    private void Start()
    {
        currentFountain = waterSpray;
        ManagerScript.instance.FountainSelected += FountainSelected;
    }

  
    private async void FountainSelected(FountainType fountain)
    {

        foreach (ISprayable spray in sprays)
        {
            if (fountain == spray.GetFountainType())
            {
                currentFountain = spray;
                break;
            }
        }

        await Task.Delay(ManagerScript.instance.selectionDuration*1000);
        currentFountain = waterSpray;
    }


    IEnumerator FountainTouchResponse()
    {
        for (int i = 0; i < 10; i++)
        {
            Vector3 scaleChange = new Vector3(0, 0, 1f);
            this.gameObject.transform.localScale -= scaleChange;
            yield return new WaitForSeconds(0.05f);
        }
        for (int i = 0; i < 10; i++)
        {
            Vector3 scaleChange = new Vector3(0, 0, 1f);
            this.gameObject.transform.localScale += scaleChange;
            yield return new WaitForSeconds(0.05f);
        }
    }
}
