using UnityEngine;

public class PersistantSystem : MonoBehaviour
{
    void Awake()
    {
        if (FindObjectsOfType<PersistantSystem>().Length > 1) Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }
}
