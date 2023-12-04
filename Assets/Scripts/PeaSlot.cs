using UnityEngine;

public class PeaSlot : MonoBehaviour
{
    RectTransform _rectTransform;
    public GameObject _peaObject;
    
    void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    void Start()
    {
        //_peaObject.transform.position = transform.position;
    }
}
