using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Pea : MonoBehaviour
{
    Pod _pod;
    [SerializeField] float volumeScale = 1f;
    TMP_Text _peaText;
    Button _peaBtn;
    RectTransform _rect;
    RectTransform _parentRec;

    string _grapheme;
    AudioClip _phonemeSound;

    const string PhonemePath = "Sounds/Phonèmes/";

    void Awake()
    {
        _peaBtn = GetComponent<Button>();
        _peaText = GetComponentInChildren<TMP_Text>();
        _parentRec = transform.parent.GetComponent<RectTransform>();
        _rect = GetComponent<RectTransform>();
        
        _peaBtn.onClick.AddListener(Perform);
    }

    public void Init(Pod pod, string grapheme, string phoneme)
    {
        this._pod = pod;
        this._grapheme = grapheme;
        _phonemeSound = Resources.Load<AudioClip>($"{PhonemePath}{phoneme}");
        if (_phonemeSound == null) Debug.LogWarning($"A phoneme sound is missing : {phoneme}");
        
        _peaText.text = _grapheme.ToUpper();
        PlaceAtRandomLoc();
    }

    void Perform()
    {
        Debug.Log(_grapheme);
        GameManager.Instance.audioSource.PlayOneShot(_phonemeSound, volumeScale);
    }

    void PlaceAtRandomLoc()
    {
        Vector2 sizeDelta = _parentRec.sizeDelta;
        int x, y;
        bool stackedPea;
        do
        {
            stackedPea = false;
            
            x = Random.Range(-(int)sizeDelta.x / 2, (int)sizeDelta.x / 2);
            y = Random.Range(-(int)sizeDelta.y / 2, (int)sizeDelta.y / 2);
            Vector2 newPosition = new Vector2(x, y);

            foreach (Pea pea in _pod.peas)
            {
                if (Vector2.Distance(newPosition, pea._rect.anchoredPosition) < 80)
                {
                    stackedPea = true;
                    break;
                }
            }
        } while (stackedPea); // Avoid superposition

        _rect.anchoredPosition = new Vector2(x, y);
    }
}
