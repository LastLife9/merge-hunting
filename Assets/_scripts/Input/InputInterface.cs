public interface IInput
{
    float GetHorizontalInput();
    float GetVerticalInput();
    bool OnTouch();
    bool OnHold();
    bool OnRelease();
}