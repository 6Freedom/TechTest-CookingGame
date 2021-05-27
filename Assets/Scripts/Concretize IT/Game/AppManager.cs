using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class AppManager : MonoBehaviour
{
    public static AppManager Instance;
    Text MenuContent;
    Text ScoreContent;
    Text Timer;

    //Param�tres de jeu
    public float BaseDuration;//A d�finir en secondes
    float Duration;
    float ReceipeDuration;
    public int score = 0;
    public int scoreMalus = -10;
    public int scoreBonus = 10;
    public int ingredientTime = 10;//temps en secondes allou� � la recette par ingredient
    bool isPlaying = false;

    //Valeurs en base de donn�es
    public List<DatabaseManager.Recette> receipes;
    public List<DatabaseManager.Ingredient> ingredients;

    public int actualReceipe;
    public List<int> IngredientsId;

    //Before start
    void Awake()
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

    // Start is called before the first frame update
    void Start()
    {
        //R�cup�ration des �l�ments de la sc�ne
        MenuContent = GameObject.FindGameObjectWithTag("MenuBoard").GetComponent<Text>();
        ScoreContent = GameObject.FindGameObjectWithTag("ScoreBoard").GetComponent<Text>();
        Timer = GameObject.FindGameObjectWithTag("Timer").GetComponent<Text>();

        //initialisation des donn�es de jeu
        score = 0;
        Duration = BaseDuration;
        receipes = DatabaseManager.Instance.GetAllRecettes();
        ingredients = DatabaseManager.Instance.GetAllIngredients();

        UpdateScoringBoard();
        UpdateTimer();
        MenuContent.text = "";

    }

    // Update is called once per frame
    void Update()
    {
        if (isPlaying)
        {
            //Mise � jour du temps
            Duration -= Time.deltaTime;
            if (Duration <= 0)
            {
                isPlaying = false;
                Duration = 0;
                FinishGame();
            }
            UpdateTimer();
        }
        

    }

    void UpdateTimer()
    {
        if(Timer != null)
        {
            TimeSpan t = TimeSpan.FromSeconds(Duration);
            var timerContent = t.Minutes + " : " + t.Seconds + "\n";
            timerContent += "Score : " + score;
        }
        
    }

    public void UpdateScoringBoard()
    {
        if(ScoreContent != null)
        {
            var highScores = DatabaseManager.Instance.GetHighScores();

            string content = "";

            foreach(var score in highScores)
            {
                content += score.Name + " : " + score.Value + "\n";
            }

            ScoreContent.text = content;
        }
    }

    public void GetRandomReceipe()
    {
        if(MenuContent != null)
        {
            //R�cup�ration d'une nouvelle recette
            var actualReceipe = (int)UnityEngine.Random.Range(0, receipes.Count - 1);

            //Remplissage de la liste d'ingr�dients
            IngredientsId = new List<int>();
            IngredientsId.Add(receipes[actualReceipe].Ingredient_Id_1);
            IngredientsId.Add(receipes[actualReceipe].Ingredient_Id_2);
            if(receipes[actualReceipe].Ingredient_Id_3 != 0) IngredientsId.Add(receipes[actualReceipe].Ingredient_Id_3);
            if (receipes[actualReceipe].Ingredient_Id_4 != 0) IngredientsId.Add(receipes[actualReceipe].Ingredient_Id_4);

            //D�finition du temps allou� � la recette
            ReceipeDuration = ingredientTime * receipes[actualReceipe].IngredientCount();

            //R�cup�ration des ingr�dients
            var ingredient1 = ingredients.Where(i => i.Id == receipes[actualReceipe].Ingredient_Id_1).FirstOrDefault();
            var ingredient2 = ingredients.Where(i => i.Id == receipes[actualReceipe].Ingredient_Id_2).FirstOrDefault();
            var ingredient3 = ingredients.Where(i => i.Id == receipes[actualReceipe].Ingredient_Id_3).FirstOrDefault();
            var ingredient4 = ingredients.Where(i => i.Id == receipes[actualReceipe].Ingredient_Id_4).FirstOrDefault();

            //G�n�ration de la recette
            var receipeInstruction = receipes[actualReceipe].Name + "\n";
            receipeInstruction += ingredient1 != null ? ingredient1.Name + "\n" : "";
            receipeInstruction += ingredient2 != null ? ingredient2.Name + "\n" : "";
            receipeInstruction += ingredient3 != null ? ingredient3.Name + "\n" : "";
            receipeInstruction += ingredient4 != null ? ingredient4.Name + "\n" : "";

            MenuContent.text = receipeInstruction;
        }
    }

    //Appel� lorsqu'un ingr�dient rejoint la marmite
    public void AddIngredient(int foodId)
    {
        if (IngredientsId.Contains(foodId))
        {
            score += scoreBonus;
            IngredientsId.RemoveAt(IngredientsId.IndexOf(foodId));
            if (IngredientsId.Count == 0) ValidateReceipe();
            SoundManager.Instance.PlayBonus();
        }
        else
        {
            score += scoreMalus;
            SoundManager.Instance.PlayMalus();
        }
    }

    public void ValidateReceipe()
    {
        if(IngredientsId.Count == 0)
        {
            score += receipes[actualReceipe].IngredientCount() * scoreBonus;
        }
        GetRandomReceipe();
    }

    public void FinishGame()
    {

    }
}
