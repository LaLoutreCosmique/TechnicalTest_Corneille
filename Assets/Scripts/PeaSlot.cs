using UnityEngine;

public class PeaSlot : MonoBehaviour
{
    RectTransform _rectTransform;
    string _grapheme;
    
    void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    void Start()
    {
        //_peaObject.transform.position = transform.position;
    }
}
