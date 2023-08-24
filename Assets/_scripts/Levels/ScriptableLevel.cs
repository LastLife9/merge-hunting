using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Level", menuName = "New Level", order = 53)]
public class ScriptableLevel : ScriptableObject
{
    public ScriptablePrey Prey;
    public LevelHolder LevelPrefab;
}
