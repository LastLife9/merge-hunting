using System.Collections.Generic;
using UnityEngine;

public class GridHolder : MonoBehaviour
{
    [SerializeField] private Transform _gridParent;
    [SerializeField] private Cell _cellPrefab;
    [SerializeField] private int _height = 3;
    [SerializeField] private int _width = 5;
    [SerializeField] private float _cellSize = 1f;
    [SerializeField] private float _huntingGroupOffset = 1f;
    private int _cellLayer = 1 << 6;
    private SaveManager _saveManager;
    private Cell[,] _cells;

    private void OnEnable()
    {
        EventBus.OnMergeState += Initialize;
    }

    private void OnDisable()
    {
        EventBus.OnMergeState -= Initialize;
    }

    public void Initialize()
    {
        _saveManager = FindObjectOfType<SaveManager>();
        CreateGrid();
        SetHuntingCells();
        LoadGrid();
    }

    public void SaveGrid()
    {
        GameData gameData = _saveManager.GetData();
        int[] save = new int[_width * _height];
        int index = 0;

        for (int x = 0; x < _width; x++)
        {
            for (int z = 0; z < _height; z++)
            {
                ScriptableHunter scriptableHunter = _cells[x, z].HoldingScriptableHunter;
                if(scriptableHunter == null)
                {
                    save[index] = 0;
                }
                else
                {
                    save[index] = (int)scriptableHunter.Type;
                }

                index++;
            }
        }

        gameData.hunters = save;
        _saveManager.SetGameData(gameData);
    }

    public void LoadGrid()
    {
        AnimalManager animalManager = FindObjectOfType<AnimalManager>();
        GameData gameData = _saveManager.GetData();
        if (gameData.hunters == null) return;
        int[] huntersToLoad = gameData.hunters;
        int[,] arrayToLoad = new int[_width, _height];
        int index = 0;

        for (int x = 0; x < _width; x++)
        {
            for (int z = 0; z < _height; z++)
            {
                arrayToLoad[x, z] = huntersToLoad[index];
                if (arrayToLoad[x, z] != 0)
                {
                    ScriptableHunter hunter = animalManager.GetHuner((HunterType)arrayToLoad[x, z]);
                    animalManager.SpawnHunterInCell(_cells[x, z], hunter);
                }

                index++;
            }
        }
    }

    private void CreateGrid()
    {
        _cells = new Cell[_width, _height];
        Vector3 gridCenter = _gridParent.position;
        float xOffset = (_height - 1) * _cellSize * 0.5f;
        float zOffset = (_width - 1) * _cellSize * 0.5f;

        for (int x = 0; x < _width; x++)
        {
            for (int z = 0; z < _height; z++)
            {
                Vector3 cellPosition = gridCenter + new Vector3(
                    -x * _cellSize + zOffset, 
                    0f, 
                    z * _cellSize - xOffset);

                Cell newCell = Instantiate(_cellPrefab, _gridParent);
                newCell.transform.position = cellPosition;
                _cells[x, z] = newCell;
            }
        }
    }

    private void SetHuntingCells()
    {
        for (int x = 0; x < _width; x++)
        {
            for (int z = 0; z < _height; z++)
            {
                Cell currentCell = _cells[x, z];
                currentCell.transform.position += Vector3.forward * (z == 0 ? -_huntingGroupOffset : 0f);
                currentCell.Hunting = z == 0;
            }
        }
    }

    public ScriptableHunter[] GetHuntingGroup()
    {
        List<ScriptableHunter> animalsInGroup = new List<ScriptableHunter>();

        for (int x = 0; x < _width; x++)
        {
            for (int z = 0; z < _height; z++)
            {
                Cell currentCell = _cells[x, z];
                if (currentCell.Hunting && currentCell.HoldingScriptableHunter != null)
                {
                    animalsInGroup.Add(currentCell.HoldingScriptableHunter);
                }
            }
        }

        return animalsInGroup.ToArray();
    }

    public void HideGrid()
    {
        _gridParent.gameObject.SetActive(false);
    }

    public void SelectSuitableCells(MergeAnimal holdingAnimal)
    {
        for (int x = 0; x < _width; x++)
        {
            for (int z = 0; z < _height; z++)
            {
                Cell currentCell = _cells[x, z];

                if (holdingAnimal != null && 
                    currentCell.HoldingScriptableHunter != null &&
                    currentCell != GetCellFromAnimal(holdingAnimal) && 
                    currentCell.HoldingScriptableHunter.Type == holdingAnimal.AnimalData.Type)
                {
                    currentCell.Select();
                }
                else
                {
                    currentCell.Deselect();
                }
            }
        }
    }

    public Cell GetRandomFreeCell()
    {
        List<Cell> freeCells = new List<Cell>();

        for (int x = 0; x < _width; x++)
        {
            for (int z = 0; z < _height; z++)
            {
                Cell currentCell = _cells[x, z];

                if (currentCell.HoldingScriptableHunter == null)
                {
                    freeCells.Add(currentCell);
                }
            }
        }

        if (freeCells.Count > 0) return freeCells[Random.Range(0, freeCells.Count)];
        else return null;
    }

    public Cell GetCellFromWorldPosition()
    {
        Ray ray = Helpers.Camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, float.MaxValue, _cellLayer))
        {
            Cell targetCell = hit.collider.GetComponent<Cell>();

            if (targetCell != null)
            {
                return targetCell;
            }
        }

        return null;
    }

    public Cell GetCellFromAnimal(MergeAnimal animal)
    {
        return animal.GetComponentInParent<Cell>();
    }

    public MergeAnimal GetMergeAnimalFromCell(Cell cell)
    {
        return cell.GetComponentInChildren<MergeAnimal>();
    }
}
