using UnityEngine;

public class SceneData : MonoBehaviour
{
    public static SceneData Instance { get; private set; }
    public Vector3 pos_from { get; set; } = Vector3.zero;
    public Vector3 pos_to { get; set; } = Vector3.zero;
    public Vector3 orient_from { get; set; } = Vector3.zero;
    public int target { get; set; } = 0;
    public Vector3 angle_to { get; set; } = Vector3.zero;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}