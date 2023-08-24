using System.Collections.Generic;
using UnityEngine;

public class PathCreator : MonoBehaviour
{
    private Transform endPoint;
    private float verticalFactor = 2.0f;

    private LineRenderer _lineRenderer;
    private List<Vector3> _trajectoryPositions;
    [SerializeField, Range(1, 100)] private int _numPoints = 20;

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        endPoint = new GameObject().transform;
        endPoint.parent = transform;
        DisableLine();
    }

    public void DisableLine()
    {
        _lineRenderer.enabled = false;
    }

    public void EnableLine()
    {
        _lineRenderer.enabled = true;
    }

    public List<Vector3> GetPositions()
    {
        return _trajectoryPositions;
    }

    public void UpdateLine(Vector3 targetPos)
    {
        endPoint.position = targetPos;
        _trajectoryPositions = new List<Vector3>();
        _lineRenderer.positionCount = _numPoints;

        for (int i = 0; i < _numPoints; i++)
        {
            float t = i / (float)(_numPoints - 1);
            Vector3 position = CalculateCubicBezierPoint(t,
                transform.position,
                transform.position + transform.up * verticalFactor,
                endPoint.position + endPoint.up * verticalFactor,
                endPoint.position);
            _trajectoryPositions.Add(position);
            _lineRenderer.SetPosition(i, position);
        }
    }

    private Vector3 CalculateCubicBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;

        Vector3 point = uuu * p0 + 3 * uu * t * p1 + 3 * u * tt * p2 + ttt * p3;
        return point;
    }
}
