using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Pea : MonoBehaviour
{
    Pod _pod;
    [SerializeField] float volumeScale = 1f;
    TMP_Text _peaText;
    Button _peaBtn;

    string _grapheme;
    AudioClip _phonemeSound;

    const string PhonemePath = "Sounds/Phonèmes/";

    void Awake()
    {
        _peaBtn = GetComponent<Button>();
        _peaText = GetComponentInChildren<TMP_Text>();
        
        _peaBtn.onClick.AddListener(Perform);
    }

    public void Init(Pod pod, string grapheme, string phoneme)
    {
        this._pod = pod;
        this._grapheme = grapheme;
        _phonemeSound = Resources.Load<AudioClip>($"{PhonemePath}{phoneme}");
        if (_phonemeSound == null) Debug.LogWarning($"A phoneme sound is missing : {phoneme}");
        
        _peaText.text = _grapheme.ToUpper();
    }

    void Perform()
    {
        Debug.Log(_peaText.text);
        GameManager.Instance.audioSource.PlayOneShot(_phonemeSound, volumeScale);
    }
}
