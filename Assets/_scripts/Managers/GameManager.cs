using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    None,
    Initialize,
    Menu,
    Merge,
    Hunt
}

public class GameManager : MonoBehaviour
{
    private GameState _currentState;

    public GameState State
    { 
        get => _currentState; 
        set 
        {
            _currentState = value;
            UpdateState();
        }
    }

    private void Awake()
    {
        State = GameState.Initialize;
    }

    private void Start()
    {
        State = GameState.Menu;
    }

    public void StartMerge()
    {
        State = GameState.Merge;
    }

    public void StartHunt()
    {
        if(FindObjectOfType<GridHolder>().GetHuntingGroup().Length == 0)
        {
            Debug.Log("Hunting group is empty");
            return;
        }

        State = GameState.Hunt;
    }

    private void UpdateState()
    {
        switch (_currentState)
        {
            case GameState.None:
                break;
            case GameState.Initialize:
                Initialize();
                break;
            case GameState.Menu:
                EventBus.OnMenuState?.Invoke();
                break;
            case GameState.Merge:
                EventBus.OnMergeState?.Invoke();
                break;
            case GameState.Hunt:
                EventBus.OnHuntState?.Invoke();
                break;
            default:
                break;
        }
    }

    private void Initialize()
    {
        Application.targetFrameRate = 60;
    }
}