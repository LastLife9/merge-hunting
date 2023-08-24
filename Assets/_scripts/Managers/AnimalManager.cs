using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AnimalManager : MonoBehaviour
{
    private Coins _coins;
    private GridHolder _gridHolder;
    private List<ScriptableHunter> _hunters;
    private List<ScriptablePrey> _prey;
    private Dictionary<HunterType, ScriptableHunter> _huntersDict;
    private Dictionary<PreyType, ScriptablePrey> _preyDict;
    private Dictionary<int, HunterType> _huntersLevelsDict = new Dictionary<int, HunterType>()
    {
        {1, HunterType.Fox},
        {2, HunterType.BlackFox},
        {3, HunterType.Boar},
    };
    [SerializeField] private int _huntersPrice = 35;
    [SerializeField] private Button _buyButton;
    [SerializeField] private TextMeshProUGUI _buyBtnLabel;
    [SerializeField] private float _punchAnimForce = 0.5f;
    [SerializeField] private float _punchAnimDuration = 0.2f;

    private void OnEnable()
    {
        _buyButton.onClick.RemoveAllListeners();
        _buyButton.onClick.AddListener(BuyHunter);
    }

    private void Awake()
    {
        AssembleResources();
        _coins = FindObjectOfType<Coins>();
        _gridHolder = FindObjectOfType<GridHolder>();
    }

    private void Start()
    {
        UpdateBuyBtn();
    }

    public void BuyHunter()
    {
        if(_coins.Count < _huntersPrice)
        {
            _coins.NotEnoughWarning();
            return;
        }
        Cell freeCell = _gridHolder.GetRandomFreeCell();
        if (freeCell == null)
        {
            Debug.Log("All cells are busy");
            return;
        }

        _coins.Count -= _huntersPrice;
        UpdateBuyBtn();
        ScriptableHunter newAnimalData = GetHunerByLevel(1);
        SpawnHunterInCell(freeCell, newAnimalData);
    }

    public void SpawnHunterInCell(Cell cell, ScriptableHunter animal)
    {
        HunterBase animalPrefab = animal.MergePrefab;
        HunterBase newAnimal = Instantiate(animalPrefab, cell.transform);
        MergeAnimal mergeAnimal = newAnimal as MergeAnimal;
        mergeAnimal.Punch();
        newAnimal.AnimalData = animal;
        cell.HoldingScriptableHunter = mergeAnimal.AnimalData;

        _gridHolder.SaveGrid();
    }

    private void UpdateBuyBtn()
    {
        DOTween.Kill(true);
        _buyBtnLabel.text = $"{_huntersPrice}";
        _buyButton.transform
            .DOPunchScale(Vector3.one * _punchAnimForce, _punchAnimDuration)
            .Play();
    }

    private void AssembleResources()
    {
        _hunters = Resources.LoadAll<ScriptableHunter>("Hunters").ToList();
        _prey = Resources.LoadAll<ScriptablePrey>("Prey").ToList();
        _huntersDict = _hunters.ToDictionary(r => r.Type, r => r);
        _preyDict = _prey.ToDictionary(r => r.Type, r => r);
    }

    public ScriptablePrey GetPrey(PreyType type) => 
        _preyDict[type];
    public ScriptableHunter GetHuner(HunterType type) => 
        _huntersDict[type];
    public ScriptableHunter GetHunerByLevel(int level) => 
        _huntersLevelsDict.ContainsKey(level) ? GetHuner(_huntersLevelsDict[level]) : null;
}
