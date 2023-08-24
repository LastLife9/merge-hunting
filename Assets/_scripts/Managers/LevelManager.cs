using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private SaveManager _saveManager;
    private List<ScriptableLevel> _levels;
    private int _currentLevel = 0;

    private void Awake()
    {
        AssembleResources();
        _saveManager = FindObjectOfType<SaveManager>();
    }

    private void Start()
    {
        _currentLevel = _saveManager.GetData().lvl;
    }

    public void LevelComplete()
    {
        _currentLevel++;
        if (_currentLevel >= _levels.Count) _currentLevel = 0;

        GameData gameData = _saveManager.GetData();
        gameData.lvl = _currentLevel;
        _saveManager.SetGameData(gameData);
    }

    public ScriptableLevel CurrentLevel => _levels[_currentLevel];

    private void AssembleResources()
    {
        _levels = Resources.LoadAll<ScriptableLevel>("Levels").ToList();
    }
}