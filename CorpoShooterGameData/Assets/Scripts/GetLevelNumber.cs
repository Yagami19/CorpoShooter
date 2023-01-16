using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.UI;


public class GetLevelNumber : MonoBehaviour
{
    [SerializeField] private InputField RoomCountInputField;
    private int val;
    // Awake as it is generated when scene is loaded
    void Awake()
    {
       // LevelNumber.roomCount = 5;
    }
   //loading scene that generates levels
   public void LoadScene()

    {
        LevelNumber.roomCount = TextToInt(RoomCountInputField.text);

        if (LevelNumber.roomCount > 0 && LevelNumber.roomCount < 51)
        {
            LevelNumber.roomCount = TextToInt(RoomCountInputField.text);
            SceneManager.LoadScene("SampleScene");
        }
        else
        {
            RoomCountInputField.text = "Podaj liczbe od 1 do 50";
        }
    }

    //Converting input field from string to int value
    public int TextToInt(string _input)
    {
        val = 0;
        string inputFieldText = _input;
        if (Int32.TryParse(inputFieldText, out val))
        {
            val = Convert.ToInt32(inputFieldText);
            return val;
        }
        else
        {
            return 0;
        }

    }

}
