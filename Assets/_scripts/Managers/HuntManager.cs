using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class HuntManager : MonoBehaviour
{
    private ScriptableHunter[] _hunters;
    private ScriptablePrey _prey;
    private ScriptableLevel _level;
    private List<HuntAnimal> _huntAnimals;
    private PreyAnimal _preyAnimal;

    private float _distOfset;
    private bool _end = false;
    [SerializeField] private float _spacing = 0.8f;
    [SerializeField, Range(0f, 1f)] private float _endScreensFadeTo = 0.5f;

    private void OnEnable()
    {
        EventBus.OnHuntState += StartHunt;
        EventBus.OnMergeState += ResetValue;
    }

    private void OnDisable()
    {
        EventBus.OnHuntState -= StartHunt;
        EventBus.OnMergeState -= ResetValue;
    }

    private async void StartHunt()
    {
        AssignAnimals();
        UIManager uiCurrentScene = FindObjectOfType<UIManager>();

        await uiCurrentScene.FadePanel(PanelType.Hunt);

        SceneLoader sceneLoader = new SceneLoader();
        sceneLoader.LoadHuntScene();

        await Task.Yield();

        SpawnObjects();
        StartMove();
        UIManager uiNextScene = FindObjectOfType<UIManager>();
        await uiNextScene.FadePanel(PanelType.Hunt, 0);
    }

    private void AssignAnimals()
    {
        AnimalManager animalManager = FindObjectOfType<AnimalManager>();
        GridHolder grid = FindObjectOfType<GridHolder>();
        ScriptableHunter[] huntingGroup = grid.GetHuntingGroup();

        if (huntingGroup.Length == 0)
        {
            Debug.Log("Hunting group is empty!");
            return;
        }

        _hunters = new ScriptableHunter[huntingGroup.Length];
        for (int i = 0; i < huntingGroup.Length; i++)
        {
            ScriptableHunter huntAnimal = animalManager.GetHuner(huntingGroup[i].Type);
            _hunters[i] = huntAnimal;
        }

        _level = FindObjectOfType<LevelManager>().CurrentLevel;
        _prey = _level.Prey;
    }

    private void SpawnObjects()
    {
        Quaternion identity = Quaternion.identity;

        LevelHolder level = Instantiate(_level.LevelPrefab, Vector3.zero, identity);
        _distOfset = Vector3.Distance(Vector3.zero, level.PreySpawn.position);
        PreyAnimal prey = Instantiate(_prey.Prefab, level.PreySpawn.position, identity);
        prey.Health = _prey.Health;
        _preyAnimal = prey;

        int huntersCount = _hunters.Length;
        float totalLength = (huntersCount - 1) * _spacing;

        Vector3 startPosition = transform.position - new Vector3(totalLength / 2, 0, 0);
        HuntAnimal[] hunters = new HuntAnimal[huntersCount];

        for (int i = 0; i < huntersCount; i++)
        {
            Vector3 spawnPosition = startPosition + new Vector3(i * _spacing, 0, 0);

            hunters[i] = Instantiate(_hunters[i].HuntPrefab, spawnPosition, identity) as HuntAnimal;
            hunters[i].AnimalData = _hunters[i];
        }
        
        _huntAnimals.AddRange(hunters);
    }

    private void StartMove()
    {
        _preyAnimal.Move();
        foreach (var hunter in _huntAnimals)
        {
            hunter.Move();
            hunter.Deactivate();
        }

        _huntAnimals[0].Activate();
    }

    public void ChangeHunter(HuntAnimal hunterToChange)
    {
        if (_end) return;

        hunterToChange.Deactivate();
        _huntAnimals.Remove(hunterToChange);
        if (_huntAnimals.Count > 0) _huntAnimals[0].Activate();
        else
        {
            _end = true;

            GameOver();
        }
    }

    public void PreyDie()
    {
        if (_end) return;
        _end = true;

        FindObjectOfType<Coins>().Count += _prey.Reward;
        LevelComplete();
    }

    private void GameOver()
    {
        FindObjectOfType<UIManager>().FadePanel(PanelType.GameOver, _endScreensFadeTo);
    }

    private void LevelComplete()
    {
        FindObjectOfType<LevelManager>().LevelComplete();
        FindObjectOfType<UIManager>().FadePanel(PanelType.LevelComplete, _endScreensFadeTo);
    }

    private void ResetValue()
    {
        _end = false;
        _hunters = null;
        _prey = null;
        _level = null;

        _preyAnimal = null;
        _huntAnimals = new List<HuntAnimal>();
    }

    public float DistanceOfset => _distOfset;
}