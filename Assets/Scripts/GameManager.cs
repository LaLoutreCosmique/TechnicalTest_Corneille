using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    WordDatabase _database;
    public AudioSource audioSource;
    
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
        
    }

    void DrawRandomWord()
    {
        
    }
}
