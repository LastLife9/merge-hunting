using UnityEngine;

public class MergeManager : MonoBehaviour
{
    private MergeAnimal _holdedAnimal;
    private AnimalManager _animalManager;
    private GridHolder _gridHolder;
    private IInput _input;
    private Cell _selectedCell;
    private bool _enable = true;

    public MergeAnimal HoldedAnimal 
    { 
        get => _holdedAnimal; 
        set => _holdedAnimal = value; 
    }

    private void OnEnable()
    {
        EventBus.OnHuntState += Disable;
        EventBus.OnMergeState += Enable;
    }

    private void OnDisable()
    {
        EventBus.OnHuntState -= Disable;
        EventBus.OnMergeState -= Enable;
    }


    private void Awake()
    {
        _input = new MobileDesktopInput();
        _gridHolder = FindObjectOfType<GridHolder>();
        _animalManager = FindObjectOfType<AnimalManager>();
    }

    private void Update()
    {
        if (!_enable) return;

        if (_input.OnTouch())
        {
            if (HoldedAnimal == null) return;

            SelectSuitableCells();
        }

        if (_input.OnHold())
        {
            if (HoldedAnimal == null) return;

            Cell targetCell = _gridHolder.GetCellFromWorldPosition();
            if(_selectedCell != targetCell)
            {
                _selectedCell?.Deselect();
                SelectSuitableCells();
                targetCell?.Select();
                _selectedCell = targetCell;
            }

            HoldedAnimal.FollowMouse();
        }

        if (_input.OnRelease())
        {
            if (HoldedAnimal == null) return;

            Cell targetCell = _gridHolder.GetCellFromWorldPosition();
            Cell holdedCell = _gridHolder.GetCellFromAnimal(HoldedAnimal);
            MergeAnimal animalInCell = null;
            HunterType animalTypeInCell = HunterType.None;
            HunterType holdedAnimalType = HoldedAnimal.AnimalData.Type;
            ScriptableHunter upgradedAnimal = _animalManager.GetHunerByLevel((int)HoldedAnimal.AnimalData.Type + 1);
            Vector3 mergePosition = Vector3.zero;
            
            if(targetCell == null)
            {
                ReturnBackHoldedAnimal();
                ResetSelected();
                return;
            }

            animalInCell = _gridHolder.GetMergeAnimalFromCell(targetCell);
            mergePosition = targetCell.transform.position;

            if (animalInCell == null)
            {
                HoldedAnimal.ChangeParentCell(targetCell);
                holdedCell.HoldingScriptableHunter = null;
                _gridHolder.SaveGrid();
            }
            else if (upgradedAnimal == null)
            {
                ReturnBackHoldedAnimal();
            }
            else
            {
                animalTypeInCell = animalInCell.AnimalData.Type;
                if (animalInCell != null && 
                    animalTypeInCell == holdedAnimalType && 
                    targetCell != holdedCell)
                {
                    
                    HoldedAnimal.Merge(mergePosition);
                    animalInCell.Merge(mergePosition);
                    _animalManager.SpawnHunterInCell(targetCell, upgradedAnimal);
                    holdedCell.HoldingScriptableHunter = null;
                    _gridHolder.SaveGrid();
                }
                else
                {
                    ReturnBackHoldedAnimal();
                }
            }
            
            ResetSelected();
        }
    }

    private void SelectSuitableCells()
    {
        _gridHolder.SelectSuitableCells(HoldedAnimal);
    }

    private void ResetSelected()
    {
        HoldedAnimal = null;
        SelectSuitableCells();
    }

    private void ReturnBackHoldedAnimal()
    {
        HoldedAnimal.ReturnBack();
    }

    private void Enable()
    {
        _enable = true;
    }
    private void Disable()
    {
        _enable = false;
    }
}
