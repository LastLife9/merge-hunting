using System;

public static class EventBus
{
    public static Action OnMenuState { get; set; }
    public static Action OnMergeState { get; set; }
    public static Action OnHuntState { get; set; }
}