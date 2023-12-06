using System;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    WordDatabase _database;
    public AudioSource audioSource;
    [SerializeField] Pod pod;
    
    const int ExtraPeasAmount = 2;
    
    void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
        
        _database = GetComponent<WordDatabase>();
        audioSource = GetComponent<AudioSource>();
    }

    public void Start()
    {
        int randIndex = DrawRandomWordIndex();

        (Word, int)[] extraPeas = new (Word, int)[ExtraPeasAmount]; // int is the index of the random grapheme in the word
        for (int i = 0; i < ExtraPeasAmount; i++)
        {
            extraPeas[i] = DrawRandomPea(_database.WordList[randIndex]);
        }
        
        pod.Init(_database.WordList[randIndex], extraPeas);
        //pod.Init(_database.WordList[177], extraPeas);
    }

    int DrawRandomWordIndex()
    {
        return Random.Range(0, _database.WordList.Length);
    }

    (Word, int) DrawRandomPea(Word wordToGuess)
    {
        Word randWord;
        int randGraphemeIndex;
        do
        {
            randWord = _database.WordList[DrawRandomWordIndex()];
            randGraphemeIndex = Random.Range(0, randWord.Grapheme.Length);
        } while (wordToGuess.Grapheme.Any(g => g == randWord.Grapheme[randGraphemeIndex]) || randWord.Grapheme[randGraphemeIndex] == "");
        
        return (randWord, randGraphemeIndex);
    }
}
