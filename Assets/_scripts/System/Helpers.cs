using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public static class Helpers
{
    private static RectTransform canvasRectTransform;
    private static PointerEventData _eventDataCurrentPosition;
    private static List<RaycastResult> _results;
    private static Camera _camera;

    public static Camera Camera
    {
        get
        {
            if (_camera == null) _camera = Camera.main;
            return _camera;
        }
    }

    public static bool IsOverUI()
    {
        _eventDataCurrentPosition = new PointerEventData(EventSystem.current) { position = Input.mousePosition };
        _results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(_eventDataCurrentPosition, _results);
        return _results.Count > 0;
    }

    public static Vector2 GetPositionOnCanvas(Vector3 worldPosition)
    {
        Vector2 canvasPosition;

        if(canvasRectTransform == null)
        {
            canvasRectTransform = GameObject.FindObjectOfType<Canvas>().GetComponent<RectTransform>();
        }
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform,
            Camera.WorldToScreenPoint(worldPosition), null, out canvasPosition);

        return canvasPosition;
    }

    public static float DistanceToCamera(Transform from) => (Helpers.Camera.transform.position - from.position).magnitude;
}