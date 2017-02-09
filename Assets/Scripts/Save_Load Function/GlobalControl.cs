﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

// Game Master class, needs to be in every level.
// This keeps the saved data gotten from the save file and loads data from there.

public class GlobalControl : MonoBehaviour 
{
	public static GlobalControl Instance;

    //TUTORIAL
    public PlayerStatistics savedPlayerData = new PlayerStatistics();
	public bool IsSceneBeingLoaded = false;

	//Copy or our player, if we ever need it game-wide
	public GameObject Player;

	//Pseudo-singleton concept from Unity dev tutorial video:
    void Awake()
    {
        Application.targetFrameRate = 144;

        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

	// This is called when the player saves the game.
	public void SaveData()
	{
		string saveDirectory = Application.dataPath + "/Saves";

		// If there's no 'Saves' folder, the game creates one
		if(!Directory.Exists(saveDirectory))
			Directory.CreateDirectory(saveDirectory);

		// Creates the BinaryFormatter, which convets the player's save data into binary and opens the file to save it into.
		BinaryFormatter formatDataToFile = new BinaryFormatter();
		FileStream saveFile = File.Create(saveDirectory + "/save.bat");

		// Serializes the player's data into the saved file.
		savedPlayerData = PlayerState.Instance.localPlayerData;
		formatDataToFile.Serialize(saveFile,savedPlayerData);

		saveFile.Close();
	}

	// This is called when the player loads the game. If there's no save file, returns false
	public bool LoadData()
	{
		string filePath = Application.dataPath + "/Saves/save.bat";
		if(File.Exists(filePath) == true)
		{
			// Creates the BinaryFormatter that will deserialize the same data at the given path.
			BinaryFormatter convertData = new BinaryFormatter();
			FileStream saveFile = File.Open(filePath, FileMode.Open);

			// Loads the saved data into savedPlayerData
			savedPlayerData = (PlayerStatistics)convertData.Deserialize(saveFile);

			saveFile.Close();	
			return true;
		}
		else
		{
			print("No save file found!");
			return false;
		}
	}
}