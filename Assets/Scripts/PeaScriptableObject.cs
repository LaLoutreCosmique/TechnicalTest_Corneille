using UnityEngine;

[CreateAssetMenu(fileName = "PeaData", menuName = "PeaObjectData")]
public class PeaScriptableObject : ScriptableObject
{
    public string[] Grapheme;
    public AudioClip Phoneme;
    public char PhonemeChar;
}
