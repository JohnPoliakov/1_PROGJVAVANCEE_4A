using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [Header("Pause")]
    [SerializeField]
    GameObject PauseMenu;
    [SerializeField]
    GameObject canvas;
    public bool IsPause;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void SetPause(bool state)
    {
        IsPause = state;

        if (state)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
        PauseMenu.SetActive(state);
    }

    public void StartGame()
    {
        MapGenerator.Instance.GeneratedMap();

        Camera.main.transform.position = new Vector3(MapGenerator.Instance.size.x / 2, MapGenerator.Instance.size.x, 0);
        
        canvas.SetActive(false);
    }
    
    

}
