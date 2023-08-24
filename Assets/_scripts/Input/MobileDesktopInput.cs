using UnityEngine;

public class MobileDesktopInput : IInput
{
    private float _touchXDelta;
    private float _touchYDelta;
    private float _touchStartPositionX;
    private float _touchStartPositionY;

    public float GetHorizontalInput()
    {
        if (OnTouch())
        {
            _touchStartPositionX = Input.mousePosition.x;
        }

        if (OnHold())
        {
            _touchXDelta = Input.mousePosition.x - _touchStartPositionX;
        }

        if (OnRelease())
        {
            _touchXDelta = 0f;
        }

        return _touchXDelta;
}

    public float GetVerticalInput()
    {
        if (OnTouch())
        {
            _touchStartPositionY = Input.mousePosition.y;
        }

        if (OnHold())
        {
            _touchYDelta = Input.mousePosition.y - _touchStartPositionY;
        }

        if (OnRelease())
        {
            _touchYDelta = 0f;
        }

        return _touchYDelta;
    }

    public bool OnHold()
    {
        return Input.GetMouseButton(0);
    }

    public bool OnRelease()
    {
        return Input.GetMouseButtonUp(0);
    }

    public bool OnTouch()
    {
        return Input.GetMouseButtonDown(0);
    }
}