using UnityEngine;

public class UnityDontDestroy : MonoBehaviour
{
    void Awake() { DontDestroyOnLoad(gameObject); }
}
