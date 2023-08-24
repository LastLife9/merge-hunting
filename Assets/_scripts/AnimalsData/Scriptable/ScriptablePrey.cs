using System.Collections;
using UnityEngine;

public enum PreyType
{
    None,
    Goat,
    MegaGoat
}

[System.Serializable, CreateAssetMenu(fileName = "New Prey", menuName = "New Prey", order = 52)]
public class ScriptablePrey : ScriptableAnimal
{
    public PreyType Type;
    public float Health;
    public int Reward;
    public PreyAnimal Prefab;
}