using UnityEngine;

public class Cell : MonoBehaviour
{
    [SerializeField] private GameObject _selectVisual;
    public ScriptableHunter HoldingScriptableHunter { get; set; }
    public bool Hunting { get; set; }

    public void Select()
    {
        _selectVisual.SetActive(true);
    }
    public void Deselect()
    {
        _selectVisual.SetActive(false);
    }
}