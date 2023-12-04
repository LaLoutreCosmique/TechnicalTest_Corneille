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
    (Word, int)[] _extraPeas; // int is the index of the random grapheme in the word
    
    void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
        
        _database = GetComponent<WordDatabase>();
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        int randIndex = DrawRandomWordIndex();
        
        // DON'T KEEP IT
        foreach (var graph in _database.WordList[randIndex].Grapheme)
        {
            Debug.Log(graph);
        }
        // RIGHT ?

        _extraPeas = new (Word, int)[ExtraPeasAmount];
        for (int i = 0; i < ExtraPeasAmount; i++)
        {
            _extraPeas[i] = DrawRandomPea(_database.WordList[randIndex]);
        }
        
        pod.Init(_database.WordList[randIndex]);
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
        } while (wordToGuess.Grapheme.Any(g => g == randWord.Grapheme[randGraphemeIndex]));
        
        Debug.Log($"OK CHECK YES GOOD WORKED : {randWord.Grapheme[randGraphemeIndex]}");
        return (randWord, randGraphemeIndex);
    }
}
