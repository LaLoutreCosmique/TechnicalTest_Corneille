using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class SoundBtn : MonoBehaviour
{
    Button _btn;
    [SerializeField] Image imageContainer;
    RectTransform _rectTransform;
    RectTransform _imgRectTransform;
    
    AudioClip _wordSound;
    [SerializeField] Sprite baseImage;

    bool hiding, showing;
    const float AnimSpeed = 8f;
    Vector2 maxSize;

    void Awake()
    {
        _btn = GetComponent<Button>();
        _rectTransform = GetComponent<RectTransform>();
        _imgRectTransform = imageContainer.GetComponent<RectTransform>();

        maxSize = _rectTransform.sizeDelta;
        
        _btn.onClick.AddListener(PlaySound);
    }

    void Start()
    {
        _rectTransform.sizeDelta = Vector2.zero;
        _imgRectTransform.sizeDelta = Vector2.zero;
    }

    void LateUpdate()
    {
        if (showing)
        {
            Debug.Log(Math.Abs(_rectTransform.sizeDelta.x - maxSize.x));
            if (Math.Abs(_rectTransform.sizeDelta.x - maxSize.x) > 0.1)
            {
                _imgRectTransform.sizeDelta = Vector2.Lerp(_imgRectTransform.sizeDelta, maxSize, Time.deltaTime * AnimSpeed);
                _rectTransform.sizeDelta = Vector2.Lerp(_rectTransform.sizeDelta, maxSize, Time.deltaTime * AnimSpeed);
            }
            else
                showing = false;
        }
        
        if (hiding)
        {
            if (_rectTransform.sizeDelta.x > 0.1)
            {
                _rectTransform.sizeDelta = Vector2.Lerp(_rectTransform.sizeDelta, Vector2.zero, Time.deltaTime * AnimSpeed);
                _imgRectTransform.sizeDelta = Vector2.Lerp(_imgRectTransform.sizeDelta, Vector2.zero, Time.deltaTime * AnimSpeed);
            }
            else
                hiding = false;
        }
        
    }

    public void Show([CanBeNull] Sprite newImage, AudioClip newSound)
    {
        _wordSound = newSound;
        imageContainer.sprite = newImage ? newImage : baseImage;

        showing = true;
    }

    public void Hide()
    {
        hiding = true;
    }

    void PlaySound()
    {
        if (_wordSound is not null)
            GameManager.Instance.audioSource.PlayOneShot(_wordSound);
    }
}
