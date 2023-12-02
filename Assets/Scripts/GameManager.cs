using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    WordDatabase _database;
    
    void Awake()
    {
        _database = GetComponent<WordDatabase>();
    }

    void Start()
    {
        
    }

    void DrawRandomWord()
    {
        
    }
}
