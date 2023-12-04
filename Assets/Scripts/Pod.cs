using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Pod : MonoBehaviour
{
    string _word;
    string[] _grapheme;
    int _currentGraphemeIndex;
    const float MidPodWidth = 332; // Used for the smooth resize
    readonly int _playAnim = Animator.StringToHash("Play");

    [SerializeField] RectTransform closedPodMid; // To resize at animation
    [SerializeField] GameObject midSlotParent; // Parent of mid slots
    [SerializeField] GameObject midSlotPrefab;
    HorizontalLayoutGroup _midLayoutGroup; // Used to debug not updated layout

    const int TEMPSIZETEST = 4;
    Vector2 _nextMidSize;
    const float ResizeSpeed = 6f;

    bool _resizing;

    void Awake()
    {
        _midLayoutGroup = midSlotParent.GetComponent<HorizontalLayoutGroup>();
    }

    void Start()
    {
        GetComponent<Animator>().SetTrigger(_playAnim);
    }

    void LateUpdate()
    {
        if(!_resizing) return;
        
        if (Math.Abs(closedPodMid.sizeDelta.x - _nextMidSize.x) > 0.01)
            closedPodMid.sizeDelta = Vector2.Lerp(closedPodMid.sizeDelta, _nextMidSize, Time.deltaTime * ResizeSpeed);
        else
            _resizing = false;
    }

    public void Init(string word, string[] grapheme)
    {
        this._word = word;
        this._grapheme = grapheme;
        _currentGraphemeIndex = 0;
    }

    public void SmoothResize()
    {
        _nextMidSize = new Vector2((TEMPSIZETEST-2) * MidPodWidth, closedPodMid.sizeDelta.y);
        _resizing = true;
    }

    public void AddMidSlots()
    {
        for (int i = 0; i < TEMPSIZETEST-2; i++)
        {
            GameObject newSlot = Instantiate(midSlotPrefab, midSlotParent.transform);
        }
        
        // Debug not updated horizontal layout group
        StartCoroutine(DebugHorizontalLayout());
    }

    IEnumerator DebugHorizontalLayout()
    {
        yield return new WaitForSeconds(0.1f);
        _midLayoutGroup.spacing += 0.001f;
        _midLayoutGroup.spacing -= 0.001f;
    }
}
