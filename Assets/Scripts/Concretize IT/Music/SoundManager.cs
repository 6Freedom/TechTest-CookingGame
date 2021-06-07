using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    //Paramètres musique
    public AudioSource musicSource;
    public AudioSource effectSource;

    //Clip musicaux
    public AudioClip malusSound;
    public AudioClip bonusSound;

    //Vitesse de musique
    public float BaseAudioSpeed = 1;
    float AudioSpeed;

    //Singleton
    public static SoundManager Instance;

    private void Awake()
    {
        //Singleton
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        musicSource.Play();
        musicSource.pitch = 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    //Joue le son de validation
    public void PlayBonus()
    {
        effectSource.PlayOneShot(bonusSound);
    }

    //Joue le son d'erreur
    public void PlayMalus()
    {
        effectSource.PlayOneShot(malusSound);
    }

    /*Ce script devait abriter une fonctionnalité jouant sur la bande son, 
     pour rendre le jeu plus stressant et entraînant. A la vue du manque de temps,
    cette fonctionnalité a été abandonné.*/
}
