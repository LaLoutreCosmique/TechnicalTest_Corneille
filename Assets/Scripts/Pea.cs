using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Pea : MonoBehaviour
{
    Pod _pod;
    
    TMP_Text _peaText;
    Button _peaBtn;
    RectTransform _rect;
    RectTransform _parentRec;
    Animator _animator;

    string _grapheme;
    AudioClip _phonemeSound;
    
    bool _moveToCenter;
    
    const float MoveSpeed = 8f;
    readonly int _wrongAnim = Animator.StringToHash("Wrong");
    const string PhonemePath = "Sounds/Phonemes/";

    void Awake()
    {
        _peaBtn = GetComponent<Button>();
        _peaText = GetComponentInChildren<TMP_Text>();
        _parentRec = transform.parent.GetComponent<RectTransform>();
        _rect = GetComponent<RectTransform>();
        
        _peaBtn.onClick.AddListener(Perform);
    }

    void LateUpdate()
    {
        if(!_moveToCenter) return;
        
        if (Vector2.Distance(_rect.anchoredPosition, Vector2.zero) > 0.01f)
            _rect.anchoredPosition = Vector2.Lerp(_rect.anchoredPosition, Vector2.zero, Time.deltaTime * MoveSpeed);
        else
            _moveToCenter = false;
    }

    public void Init(Pod pod, string grapheme, string phoneme)
    {
        this._pod = pod;
        this._grapheme = grapheme;
        _phonemeSound = Resources.Load<AudioClip>($"{PhonemePath}{phoneme}");
        if (_phonemeSound == null && phoneme != "#") Debug.LogWarning($"A phoneme sound is missing : {phoneme}");
        
        _peaText.text = _grapheme.ToUpper();
        PlaceAtRandomLoc();
    }

    void Perform()
    {
        GameManager.Instance.audioSource.PlayOneShot(_phonemeSound);

        GameObject parentSlot = _pod.GuessPeaAndSlotMatch(_grapheme);
        if (parentSlot != null)
        {
            transform.parent = parentSlot.transform;
            _moveToCenter = true;
        }
        else
            _animator.SetTrigger(_wrongAnim);
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
        _animator = GetComponent<Animator>();
    }
}
