using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using JetBrains.Annotations;
using UnityEngine;

public class Word
{
    public string Writing;
    public string[] Grapheme;
    public string[] Phoneme;

    [CanBeNull] public readonly AudioClip Sound;
    [CanBeNull] public readonly Sprite Image;

    const string SoundsFolder = "Sounds/Mots/";
    const string ImagesFolder = "Images/Mots/";

    public Word(string writing, string grapheme, string phoneme)
    {
        this.Writing = writing;
        
        List<string> tempGrapheme = grapheme.Split('.').ToList();
        tempGrapheme.RemoveAt(0);
        this.Grapheme = tempGrapheme.ToArray();

        List<string> tempPhoneme = PhonemeToFileName(phoneme).Split('.').ToList();
        tempPhoneme.RemoveAt(0);
        this.Phoneme = tempPhoneme.ToArray();

        string writingNoAccent = RemoveDiacritics(writing);
        
        Sound = Resources.Load<AudioClip>($"{SoundsFolder}{writingNoAccent}");
        if (Sound == null) Debug.LogWarning($"A word sound is missing : {writingNoAccent}");
        Image = Resources.Load<Sprite>($"{ImagesFolder}{writingNoAccent}");
    }
    
    /// <summary>
    /// Remove accents from a word
    /// Used for example to get the right file name
    /// </summary>
    /// <param name="word">Word to be changed</param>
    /// <returns></returns>
    string RemoveDiacritics(string word)
    {
        string normalizedWord = word.Normalize(NormalizationForm.FormD);

        string regexPattern = @"\p{M}";
        return Regex.Replace(normalizedWord, regexPattern, "");
    }

    /// <summary>
    /// Replace some phoneme characters from the csv file to correspond with audio file's name
    /// </summary>
    /// <param name="phoneme"></param>
    /// <returns></returns>
    string PhonemeToFileName(string phoneme)
    {
        (string, string)[] matches = { ("e", "e_"), ("n", "n_"), ("s", "s_"), ("z", "z_"), ("ij", "j"), ("*", "~") };

        foreach ((string, string) tuple in matches)
        {
            phoneme = phoneme.Replace(tuple.Item1, tuple.Item2);
        }

        return phoneme;
    }
}
