using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Pod : MonoBehaviour
{
    string _word;
    string[] _grapheme;
    int _currentGuessSlotIndex;
    readonly int _playAnim = Animator.StringToHash("Play");

    [SerializeField] RectTransform closedPodMid; // To resize at animation
    [SerializeField] GameObject midSlotParent; // Parent of mid slots
    [SerializeField] GameObject midSlotPrefab;
    HorizontalLayoutGroup _midLayoutGroup; // Used to debug not updated layout

    [SerializeField] GameObject firstSlot;
    [SerializeField] GameObject lastSlot;
    GameObject[] _slots;

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

    public void Init(Word word)
    {
        this._word = word.Writing;
        this._grapheme = word.Grapheme;
        _currentGuessSlotIndex = 0;
        
        GetComponent<Animator>().SetTrigger(_playAnim);
    }

    public void SmoothResize()
    {
        _nextMidSize = new Vector2((_grapheme.Length-2) * MidPodWidth, closedPodMid.sizeDelta.y);
        _resizing = true;
    }

    public void SetupPeaSlots()
    {
        _slots = new GameObject[_grapheme.Length];

        _slots[0] = firstSlot;
        for (int i = 0; i < _grapheme.Length-2; i++)
        {
            GameObject newSlotParent = Instantiate(midSlotPrefab, midSlotParent.transform);
            _slots[i + 1] = newSlotParent.transform.GetChild(0).gameObject;
        }
        _slots[_grapheme.Length - 1] = lastSlot;
        
        // Debug not updated horizontal layout group
        StartCoroutine(DebugHorizontalLayout());
    }

    IEnumerator DebugHorizontalLayout()
    {
        yield return new WaitForSeconds(0.1f);
        _midLayoutGroup.spacing += 0.001f;
        _midLayoutGroup.spacing -= 0.001f;
    }

    public void SpawnPeas()
    {
        
    }
}
