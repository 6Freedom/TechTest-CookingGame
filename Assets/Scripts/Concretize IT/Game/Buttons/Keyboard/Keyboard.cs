using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Keyboard : MonoBehaviour
{
    public Text PlayerName;
    public string DefaultName;
    public static Keyboard Instance;
    // Start is called before the first frame update
    void Start()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    //Ajout d'un caractère au pseudo
    public void AddCharacter(string character)
    {
        if(PlayerName.text.Length == 8)
        {
            SoundManager.Instance.PlayMalus();
        }
        else
        {
            PlayerName.text += character;
        }
    }

    //Retrait d'un caracère au pseudo
    public void RemoveCharacter()
    {
        if(PlayerName.text.Length != 0)
        PlayerName.text = PlayerName.text.Substring(0, PlayerName.text.Length - 1);
    }


}
