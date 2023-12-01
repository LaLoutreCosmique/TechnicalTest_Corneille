using UnityEngine;

public class Word
{
    public string Writing;
    public string[] Grapheme;
    public string[] Phoneme;

    public Word(string writing, string grapheme, string phoneme)
    {
        this.Writing = writing;
        this.Grapheme = grapheme.Split('.');
        this.Phoneme = phoneme.Split('.');
    }
}
