using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPot : MonoBehaviour
{
    //Variable de mouvement
    private bool dirRight = true;
    public float speed = 2.0f;
    public float maxX;
    public float minX;

    Vector3 initPosition;
    Quaternion initRotation;

    private void Start()
    {
        //Initialise les premières position pour la réinitialisation de la partie
        initPosition = transform.position;
        initRotation = transform.rotation;
        AppManager.Instance.Pot = this;
    }

    void Update()
    {
        if (AppManager.Instance.isPlaying)
        {
            Move();
        }
       
    }

    //Oscille entre les deux positions
    private void Move()
    {
        if (dirRight)
            transform.Translate(Vector2.right * speed * Time.deltaTime);
        else
            transform.Translate(-Vector2.right * speed * Time.deltaTime);

        if (transform.position.x >= maxX)
        {
            dirRight = false;
        }

        if (transform.position.x <= minX)
        {
            dirRight = true;
        }
    }

    //Retour à la position initiale
    public void ResetPosition()
    {
        transform.position = initPosition;
        transform.rotation = initRotation;
    }
}
