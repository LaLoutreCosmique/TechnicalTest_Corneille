using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct CharConversion
{
    public string baseChar;
    public string newChar;

    public override string ToString()
    {
        return newChar;
    }
}

public class WordDatabase : MonoBehaviour
{
    const string DataFile = "wordList";
    [SerializeField] CharConversion[] diacriticCharConversion; // Diacritics (accents) are only used for the word to guess
    [SerializeField] CharConversion[] phonemeCharConversion; // Used to convert for example "e" to "e_" to fit with sound files name
    public readonly List<Word> WordList = new List<Word>();
    
    void Awake()
    {
        TextAsset data = Resources.Load<TextAsset>(DataFile);
        InitWordList(data);
    }

    void InitWordList(TextAsset data)
    {
        string[] splitedData = data.text.Split('\n');
        Debug.Log(splitedData.Length);
    }
}
