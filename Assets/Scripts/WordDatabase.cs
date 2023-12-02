using System.Collections.Generic;
using UnityEngine;

public class WordDatabase : MonoBehaviour
{
    const string DataFile = "wordList";
    public Word[] WordList;
    
    void Awake()
    {
        TextAsset data = Resources.Load<TextAsset>(DataFile);
        InitWordList(data);
    }
    
    void InitWordList(TextAsset data)
    {
        string[] dataLine = data.text.Split('\n'); // Separate by lines
        WordList = new Word[dataLine.Length - 1];

        for (int i = 1; i < dataLine.Length; i++)
        {
            string[] dataField = dataLine[i].Split(';'); // Separate by columns
            WordList[i-1] = new Word(dataField[0], dataField[1], dataField[2]);
        }
    }
}
