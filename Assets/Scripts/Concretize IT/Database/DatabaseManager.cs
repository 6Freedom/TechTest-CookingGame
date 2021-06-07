using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Mono.Data.Sqlite;
using UnityEngine;

public class DatabaseManager : MonoBehaviour
{
    public static DatabaseManager Instance;
    public IDbConnection dbConnection;
    public bool ResetDatabaseOnLaunch = false;

    //Classe Ingredient 
    public class Ingredient
    {
        public int Id;
        public string Ressource;
        public string Name;

        public Ingredient(string ressource, string name)
        {
            Id = DatabaseManager.Instance.GetAllIngredients().Count + 1;
            Ressource = ressource;
            Name = name;
        }

        public Ingredient(int id, string ressource, string name)
        {
            Id = id;
            Ressource = ressource;
            Name = name;
        }
    }

    //Classe Score
    public class Score
    {
        public int Id;
        public int Value;
        public string Name;

        public Score(int val, string name)
        {
            Id = DatabaseManager.Instance.GetAllScores().Count + 1;
            Value = val;
            Name = name;
        }

        public Score(int id, int val, string name)
        {
            Id = id;
            Value = val;
            Name = name;
        }
    }

    //Classe Recette
    public class Recette
    {
        public int Id;
        public string Name;
        public int Ingredient_Id_1;
        public int Ingredient_Id_2;
        public int Ingredient_Id_3;
        public int Ingredient_Id_4;

        public Recette(string name, int id1, int id2, int id3=0,int id4 = 0)
        {
            Name = name;
            Ingredient_Id_1 = id1;
            Ingredient_Id_2 = id2;
            Ingredient_Id_3 = id3;
            Ingredient_Id_4 = id4;
        }
        public Recette(int id, string name, int id1, int id2, int id3 = 0, int id4 = 0)
        {
            Id = id;
            Name = name;
            Ingredient_Id_1 = id1;
            Ingredient_Id_2 = id2;
            Ingredient_Id_3 = id3;
            Ingredient_Id_4 = id4;
        }

        public int IngredientCount()
        {
            return 2 + (Ingredient_Id_3 != 0 ? 1 : 0) + (Ingredient_Id_4 != 0 ? 1 : 0);
        }
    }

    //Initialisation du singleton et de la BDD
    private void Awake()
    {
        //Singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }

        //Connexion à la BDD
        string connectionString = "URI=file:" + Application.dataPath + "/Plugins/TestCookingBDD.db";
        dbConnection = new SqliteConnection(connectionString);
        dbConnection.Open();

        //Si l'option est choisie dans la scène, on supprime les données
        if (ResetDatabaseOnLaunch) ResetDatabase();

        //On initialise la BDD
        InitDb();

        
    }

    //Création BDD si elle n'existe pas
    private void InitDb()
    {
        //Lors d'un export, un BDD SQL lite est vide, il faut s'assurer qu'elle soit remplie lors d'un build
        var queryIngredient = "CREATE TABLE IF NOT EXISTS 'Ingredient' (" +
                                "'Id'    INTEGER," +
                                "'Name'  TEXT," +
                                "'Ressource' TEXT," +
                                "PRIMARY KEY('Id'))";

        var queryRecette = "CREATE TABLE IF NOT EXISTS 'Recette' (" +
                           "'Id'    INTEGER,"+
	                       "'Name'  TEXT,"+
	                       "'Ingredient_Id_1'   INTEGER,"+
	                       "'Ingredient_Id_2'   INTEGER,"+
	                       "'Ingredient_Id_3'   INTEGER,"+
	                       "'Ingredient_Id_4'   INTEGER,"+
	                       "FOREIGN KEY('Ingredient_Id_2') REFERENCES 'Ingredient'('Id'),"+
	                       "FOREIGN KEY('Ingredient_Id_3') REFERENCES 'Ingredient'('Id'),"+
	                       "FOREIGN KEY('Ingredient_Id_4') REFERENCES 'Ingredient'('Id'),"+
	                       "FOREIGN KEY('Ingredient_Id_1') REFERENCES 'Ingredient'('Id'),"+
                           "PRIMARY KEY('Id'))";
        
        var queryScore = "CREATE TABLE IF NOT EXISTS 'Score' (" +
                            "'Id'    INTEGER,"+
	                        "'Value' INTEGER,"+
	                        "'Name'  TEXT,"+
	                        "PRIMARY KEY('Id'))";

        IDbCommand cmnd = dbConnection.CreateCommand();
        cmnd.CommandText = queryIngredient;
        cmnd.ExecuteScalar();
        cmnd.CommandText = queryRecette;
        cmnd.ExecuteScalar();
        cmnd.CommandText = queryScore;
        cmnd.ExecuteScalar();

        FillDb();
    }

    //Remplissage BDD
    private void FillDb()
    {
        //Création des premiers scores
        InsertScore(new Score(1, 50, "Manu"));
        InsertScore(new Score(2, 110, "Fanfan"));
        InsertScore(new Score(3, 10, "Nico"));
        InsertScore(new Score(4, 75, "Jacquo"));
        InsertScore(new Score(5, 130, "Tonton"));

        //Création des premiers Ingredients
        InsertIngredient(new Ingredient(1, "Tomate", "Tomate"));
        InsertIngredient(new Ingredient(2, "Poivron", "Poivron"));
        InsertIngredient(new Ingredient(3, "Dinde", "Dinde"));
        InsertIngredient(new Ingredient(4, "Carotte", "Carotte"));
        InsertIngredient(new Ingredient(5, "Pomme", "Pomme"));

        //Création des premières recette
        InsertRecette(new Recette(1, "Compote",5,5,5));
        InsertRecette(new Recette(2, "Soupe",1,2,4));
        InsertRecette(new Recette(3, "Dinde provençale",1,2,3,4));
        InsertRecette(new Recette(4, "Dinde aux pommes",3,5));
        InsertRecette(new Recette(5, "Purée de tomate",1,1,1,1));
    }

    //Insert ou met à jour un score
    public void InsertScore(Score score)
    {
        var query = score.Id != 0 ? "REPLACE INTO Score VALUES("+score.Id+"," : " INSERT INTO Score VALUES(";
        query += score.Value + ", '" + score.Name + "')";

        IDbCommand cmnd = dbConnection.CreateCommand();
        cmnd.CommandText = query;
        cmnd.ExecuteScalar();
    }

    //Insert ou met à jour un ingrédient
    public void InsertIngredient(Ingredient ingredient)
    {
        var query = ingredient.Id != 0 ? "REPLACE INTO Ingredient VALUES(" + ingredient.Id + "," : " INSERT INTO Ingredient VALUES(";
        query += "'" + ingredient.Name + "', '" + ingredient.Ressource + "')";

        IDbCommand cmnd = dbConnection.CreateCommand();
        cmnd.CommandText = query;
        cmnd.ExecuteScalar();
    }

    //Insert ou met un jour une recette
    public void InsertRecette(Recette recette)
    {
        var query = recette.Id != 0 ? "REPLACE INTO Recette VALUES(" + recette.Id + "," : " INSERT INTO Ingredient VALUES(";

        var id_3 = recette.Ingredient_Id_3 != 0 ? recette.Ingredient_Id_3.ToString() : "NULL";
        var id_4 = recette.Ingredient_Id_4 != 0 ? recette.Ingredient_Id_4.ToString() : "NULL";

        query += "'" + recette.Name + "', " + 
                recette.Ingredient_Id_1 + ", " +
                recette.Ingredient_Id_2 + ", " +
                id_3 + ", " +
                id_4 +")";

        IDbCommand cmnd = dbConnection.CreateCommand();
        cmnd.CommandText = query;
        cmnd.ExecuteScalar();
    }

    //Récupère les 5 meilleurs scores
    public Score[] GetHighScores()
    {
        var query = "SELECT * FROM Score ORDER BY Value DESC LIMIT 5 ";
        IDbCommand cmnd = dbConnection.CreateCommand();
        cmnd.CommandText = query;
        var reader = cmnd.ExecuteReader();

        Score[] scores = new Score[5];
        int arrayId = 0;
        while (reader.Read())
        {
            scores[arrayId] = new Score(reader.GetInt32(0), reader.GetInt32(1), reader.GetString(2));
            arrayId++;
        }

        return scores;
    }

    //Récupère la liste des recettes
    public List<Recette> GetAllRecettes()
    {
        var query = "SELECT * FROM Recette";
        IDbCommand cmnd = dbConnection.CreateCommand();
        cmnd.CommandText = query;
        var reader = cmnd.ExecuteReader();

        var recettes = new List<Recette>();
        while (reader.Read())
        {
            int id = reader.GetInt32(0);
            string name = reader.GetString(1);
            int id_1 = reader.GetInt32(2);
            int id_2 = reader.GetInt32(3);
            int id_3 = reader.IsDBNull(4) ? 0 : reader.GetInt32(4);
            int id_4 = reader.IsDBNull(5) ? 0 : reader.GetInt32(5);

            recettes.Add(new Recette(id, name, id_1, id_2, id_3, id_4));
        }

        return recettes;
    }

    //Récupère la liste des ingrédients
    public List<Ingredient> GetAllIngredients()
    {
        var query = "SELECT * FROM Ingredient";
        IDbCommand cmnd = dbConnection.CreateCommand();
        cmnd.CommandText = query;
        var reader = cmnd.ExecuteReader();

        var ingredients = new List<Ingredient>();
        while (reader.Read())
        {
            int id = reader.GetInt32(0);
            string name = reader.GetString(1);
            string ressource = reader.GetString(2);

            ingredients.Add(new Ingredient(id, name, ressource));
        }

        return ingredients;
    }

    //Récupère la liste des scores
    public List<Score> GetAllScores()
    {
        var query = "SELECT * FROM Score";
        IDbCommand cmnd = dbConnection.CreateCommand();
        cmnd.CommandText = query;
        var reader = cmnd.ExecuteReader();

        var ingredients = new List<Score>();
        while (reader.Read())
        {
            int id = reader.GetInt32(0);
            int value = reader.GetInt32(1);
            string name = reader.GetString(2);

            ingredients.Add(new Score(id, value, name));
        }

        return ingredients;
    }

    //Remise à 0 de la base de données
    public void ResetDatabase()
    {
        
        IDbCommand cmnd = dbConnection.CreateCommand();

        string deleteIngredient = "DELETE FROM Ingredient;";
        string deleteRecette = "DELETE FROM Recette;";
        string deleteScore = "DELETE FROM Score;";

        try
        {
            cmnd.CommandText = deleteIngredient;
            cmnd.ExecuteNonQuery();
            cmnd.CommandText = deleteRecette;
            cmnd.ExecuteNonQuery();
            cmnd.CommandText = deleteScore;
            cmnd.ExecuteNonQuery();
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }

        
    }
}
