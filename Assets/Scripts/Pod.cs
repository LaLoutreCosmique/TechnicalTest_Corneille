using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class Pod : MonoBehaviour
{
    string _word;
    string[] _grapheme;
    int _currentGraphemeIndex;

    [SerializeField] RectTransform closedPodMid; // To resize at animation
    const int MidPodWidth = 332;

    [SerializeField] GameObject podOpened;

    public void Init(string word, string[] grapheme)
    {
        this._word = word;
        this._grapheme = grapheme;
        _currentGraphemeIndex = 0;

        StartCoroutine(ResizePod());
    }

    /// <summary>
    /// Multiple steps :
    /// - 
    /// </summary>
    /// <returns></returns>
    IEnumerator ResizePod()
    {
        yield break;
    }
}
