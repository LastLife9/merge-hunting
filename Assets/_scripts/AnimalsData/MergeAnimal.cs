using DG.Tweening;
using UnityEngine;

public class MergeAnimal : HunterBase
{
    [SerializeField] private float _yOffset = 0.6f;
    [SerializeField] private float moveDuration = 0.1f;
    private Vector3 _startedPosition;
    private bool _enable = true;

    private void OnEnable()
    {
        _startedPosition = _transform.position;
    }

    private void OnMouseDown()
    {
        if (!_enable) return;
        if (Helpers.IsOverUI()) return;
        Punch();
        _transform.DOMoveY(_yOffset, moveDuration).Play();
        FindObjectOfType<MergeManager>().HoldedAnimal = this;
    }

    public void FollowMouse()
    {
        Ray ray = Helpers.Camera.ScreenPointToRay(Input.mousePosition);
        Vector3 rayPoint = ray.GetPoint(Helpers.DistanceToCamera(_transform));
        _transform.position = new Vector3(rayPoint.x, _transform.position.y, rayPoint.z);
    }

    public async void Merge(Vector3 toPosition)
    {
        _enable = false;
        _transform.DOKill(true);
        var destroySequence = DOTween.Sequence();
        destroySequence
            .Join(_transform.DOMove(toPosition, moveDuration))
            .Join(_transform.DOPunchScale(Vector3.one * _punchForce, _punchDuration));
        await destroySequence.Play().AsyncWaitForCompletion();
        Destroy(gameObject);
    }

    public void ChangeParentCell(Cell newCell)
    {
        newCell.HoldingScriptableHunter = this.AnimalData;
        _transform.SetParent(newCell.transform);
        _startedPosition = newCell.transform.position;
        ReturnBack();
    }

    public void ReturnBack()
    {
        Punch();
        _transform.DOMove(_startedPosition, moveDuration).Play();
    }
}