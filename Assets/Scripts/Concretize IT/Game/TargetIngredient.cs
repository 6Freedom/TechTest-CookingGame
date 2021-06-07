using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TargetIngredient : MonoBehaviour
{

    //Particules qui donneront une information visuelle au joueur
    public ParticleSystem WrongIngredient;
    public ParticleSystem CorrectIngredient;

    private void Start()
    {
        WrongIngredient.Stop();
        CorrectIngredient.Stop();
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Si un objet qui n'est pas à but d'interaction entre en contact avec la cible, alors on ne fait rien
        if (!collision.gameObject.GetComponent<XRGrabInteractable>())
        {
            return;
        }

        //Si le jeu est en cours, on comptabilise les points
        if (AppManager.Instance.isPlaying)
        {
            var food = collision.gameObject.GetComponent<Food>();
            var result = AppManager.Instance.AddIngredient(food != null ? food.FoodId : 0);


            if (result)
            {
                CorrectIngredient.Play();
            }
            else
            {
                WrongIngredient.Play();
            }

        }

        //On détruit l'objet source même si le jeu est en pause, pour éviter de remplir l'objectif avec des aliments
        Destroy(collision.gameObject);
    }
}
