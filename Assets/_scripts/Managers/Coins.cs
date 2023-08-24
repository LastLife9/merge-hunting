using TMPro;
using UnityEngine;
using DG.Tweening;

public class Coins : MonoBehaviour
{
    private int _count = 0;
    private SaveManager _saveManager;
    [SerializeField] private TextMeshProUGUI[] _counters;
    [SerializeField] private float _warningInDuration = 0.3f;
    [SerializeField] private float _warningOutDuration = 0.5f;

    public int Count 
    { 
        get => _count; 
        set 
        {
            _count = value;
            UpdateCounters();
        }
    }

    private void Awake()
    {
        _saveManager = FindObjectOfType<SaveManager>();
    }

    private void Start()
    {
        Count = _saveManager.GetData().coins;
    }

    private void UpdateCounters()
    {
        GameData gameData = _saveManager.GetData();
        gameData.coins = Count;
        _saveManager.SetGameData(gameData);

        foreach (var counter in _counters)
            counter.text = Count.ToString();
    }

    public void NotEnoughWarning()
    {
        DOTween.Kill(true);

        foreach (var counter in _counters)
        {
            if (!counter.IsActive() || !counter.isActiveAndEnabled) return;

            var colSeq = DOTween.Sequence();
            colSeq.Join(counter.DOColor(Color.red, _warningInDuration))
                .Append(counter.DOColor(Color.white, _warningOutDuration))
                .Play();
        }
    }
}
