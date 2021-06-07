using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerButton : MonoBehaviour
{
    public FoodSpawner spawner;
    public Animator anim;
    public Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        spawner = GetComponentInParent<FoodSpawner>();
        anim = GetComponentInParent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        //On ne fait rien si une nourriture entre en contact avec le bouton, pour éviter une activation continue lors de l'apparition d'une première nourriture
        if(collision.gameObject.GetComponent<Food>() != null)
        {
            return;
        }
        
        //Apparition de la nourriture
        if (spawner != null && !anim.GetBool("Active"))
        {
            spawner.Spawn();
            anim.SetBool("Active", true);
        }
            
    }
}
