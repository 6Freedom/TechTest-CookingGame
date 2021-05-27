using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    // Start is called before the first frame update
    public int FoodId;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<TargetIngredient>())
        {

        }
    }
}
