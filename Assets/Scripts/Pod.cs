using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pod : MonoBehaviour
{
    Word word;
    (Word, int)[] _extraPeas;
    int _currentGuessSlotIndex;
    readonly int _playAnim = Animator.StringToHash("Play");

    [SerializeField] RectTransform closedPodMid; // To resize at animation
    [SerializeField] GameObject midSlotParent; // Parent where instantiate mid slots
    [SerializeField] GameObject midSlotPrefab;
    HorizontalLayoutGroup _midLayoutGroup; // Used to debug not updated layout

    [SerializeField] GameObject firstSlot;
    [SerializeField] GameObject lastSlot;
    GameObject[] _slots;
    
    [SerializeField] GameObject peaPrefab;
    [SerializeField] GameObject peaContainer;
    public List<Pea> peas;

    [SerializeField] SoundBtn _soundBtn;
    
    Vector2 _nextMidSize;
    const float ResizeSpeed = 6f;
    const float MidPodWidth = 332f; // Used for the smooth resize

    bool _resizing;

    void Awake()
    {
        _midLayoutGroup = midSlotParent.GetComponent<HorizontalLayoutGroup>();
    }

    void LateUpdate()
    {
        if(!_resizing) return;
        
        if (Math.Abs(closedPodMid.sizeDelta.x - _nextMidSize.x) > 0.01)
            closedPodMid.sizeDelta = Vector2.Lerp(closedPodMid.sizeDelta, _nextMidSize, Time.deltaTime * ResizeSpeed);
        else
            _resizing = false;
    }

    public void Init(Word word, (Word, int)[] extraPeas)
    {
        this.word = word;
        this._extraPeas = extraPeas;
        _currentGuessSlotIndex = 0;
        
        GetComponent<Animator>().SetTrigger(_playAnim);
        Debug.Log(word.Writing);
    }

    public void SmoothResize()
    {
        _nextMidSize = new Vector2((word.Grapheme.Length-2) * MidPodWidth, closedPodMid.sizeDelta.y);
        _resizing = true;
    }

    public void SetupPeaSlots()
    {
        _slots = new GameObject[word.Grapheme.Length];

        _slots[0] = firstSlot;
        for (int i = 0; i < word.Grapheme.Length-2; i++)
        {
            GameObject newSlotParent = Instantiate(midSlotPrefab, midSlotParent.transform);
            _slots[i + 1] = newSlotParent.transform.GetChild(0).gameObject;
        }
        _slots[word.Grapheme.Length - 1] = lastSlot;
        
        // Debug not updated horizontal layout group
        StartCoroutine(DebugHorizontalLayout());
    }

    IEnumerator DebugHorizontalLayout()
    {
        yield return new WaitForSeconds(0.001f);
        _midLayoutGroup.spacing += 0.001f;
        _midLayoutGroup.spacing -= 0.001f;
    }

    public void SpawnPeas()
    {
        Pea newPea;
        
        // Instantiate right peas
        for (int i = 0; i < word.Grapheme.Length; i++)
        {
            newPea = Instantiate(peaPrefab, peaContainer.transform).GetComponent<Pea>();
            newPea.Init(this, word.Grapheme[i], word.Phoneme[i]);
            peas.Add(newPea);
        }

        // Instantiate wrong peas
        for (int i = 0; i < _extraPeas.Length; i++)
        {
            newPea = Instantiate(peaPrefab, peaContainer.transform).GetComponent<Pea>();
            int extraPeaIndex = _extraPeas[i].Item2;
            newPea.Init(this, _extraPeas[i].Item1.Grapheme[extraPeaIndex], _extraPeas[i].Item1.Phoneme[extraPeaIndex]);
            peas.Add(newPea);
        }
        
        if (word.Sound is not null)
            GameManager.Instance.audioSource.PlayOneShot(word.Sound);
        
        ShowImageButton();
    }

    public GameObject GuessPeaAndSlotMatch(string guessedGrapheme)
    {
        if (_currentGuessSlotIndex < word.Grapheme.Length && word.Grapheme[_currentGuessSlotIndex] == guessedGrapheme)
        {
            _currentGuessSlotIndex++;
            
            // Check for end of level
            if (_currentGuessSlotIndex == _slots.Length)
                StartCoroutine(EndLevel());
            
            return _slots[_currentGuessSlotIndex-1];
        }

        return null;
    }

    void Cleanup()
    {
        if (peas.Count == 0) return;
        
        // Destroy pod mid slots
        for (int i = 1; i < _slots.Length-1; i++)
        { 
            Destroy(_slots[i].transform.parent.gameObject);
        }

        StartCoroutine(DebugHorizontalLayout());
        
        // Destroy peas
        Destroy(_slots[0].GetComponentInChildren<Pea>().gameObject);
        Destroy(_slots[^1].GetComponentInChildren<Pea>().gameObject);
        foreach (Pea pea in peas)
        {
            Destroy(pea.gameObject);
        }

        peas = new List<Pea>();
    }

    IEnumerator EndLevel()
    {
        yield return new WaitForSeconds(1);
        if (word.Sound is not null)
            GameManager.Instance.audioSource.PlayOneShot(word.Sound);
        GameManager.Instance.Start();
    }

    void ShowImageButton()
    {
        _soundBtn.Show(word.Image, word.Sound);
    }

    void HideImageButton()
    {
        _soundBtn.Hide();
    }
}
