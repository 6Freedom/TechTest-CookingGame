using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PlayButton : MonoBehaviour
{
    //Matériaux pour le bouton play
    public Material UnactiveMaterial;
    public Material ActiveMaterial;

    public Animator Anim;
    public GameObject Bouton;
    // Start is called before the first frame update
    void Start()
    {
        Anim = GetComponent<Animator>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        //On ne considère que les mains
        if (collision.gameObject.GetComponentInParent<XRRayInteractor>() == null)
            return;

        //Si le jeu n'est pas en cours, lance une partie
        if (!Anim.GetBool("Active"))
        {
            Anim.SetBool("Active", true);
            Bouton.GetComponent<Renderer>().material = ActiveMaterial;
            AppManager.Instance.StartGame();
        }
    }
}
