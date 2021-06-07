using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class Key : MonoBehaviour
{

    public string Character;
    public Text KeyText;
    public Animator Anim;

    private void Start()
    {
        KeyText.text = Character;
        Anim = GetComponent<Animator>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        //On ne considère que les mains, pour ignorer les collisions avec l'environnement
        if (collision.gameObject.GetComponentInParent<XRRayInteractor>() == null)
            return;

        //Si l'animation n'est pas en cours, on poursuit l'action
        if (!Anim.GetBool("Active"))
        {
            Anim.SetBool("Active", true);
            //gestion de la suppresionn du dernier caractère ou ajout d'un nouveau caractère
            if (Character.Length != 1)
                Keyboard.Instance.RemoveCharacter();
            else
                Keyboard.Instance.AddCharacter(Character);
        }
    }

}
