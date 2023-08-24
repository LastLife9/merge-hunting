using UnityEngine;

[System.Serializable]
public enum HunterType
{
    None,
    Fox,
    BlackFox,
    Boar
}

[System.Serializable, CreateAssetMenu(fileName = "New Animal", menuName = "New Animal", order = 51)]
public class ScriptableHunter : ScriptableAnimal
{
    public HunterType Type;
    public HunterBase MergePrefab;
    public HunterBase HuntPrefab;
    public float Damage;
}
