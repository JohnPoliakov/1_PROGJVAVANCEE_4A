using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [Header("Pause")]
    [SerializeField]
    GameObject PauseMenu;
    [SerializeField]
    GameObject canvas;
    [SerializeField]
    GameObject player_1;
    [SerializeField]
    GameObject player_2;
    [SerializeField]
    GameObject IARandom;
    public bool IsPause;
    public EnemyBehaviour EnemyBehaviour;
    public GameState GameState = GameState.MENU;
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
            GameState = GameState.PAUSE;
            Time.timeScale = 0;
        }
        else
        {
            GameState = GameState.RUNNING;
            Time.timeScale = 1;
        }
        PauseMenu.SetActive(state);
    }

    public void StartGame()
    {
        MapGenerator.Instance.GeneratedMap();

        Camera.main.transform.position = new Vector3(MapGenerator.Instance.size.x / 2, MapGenerator.Instance.size.x, 0);
        
        SpawnPlayers();
        
        canvas.SetActive(false);
    }

    void SpawnPlayers()
    {
        GameObject player = Instantiate(player_1, new Vector3(MapGenerator.Instance.spawns[0].x, 0, MapGenerator.Instance.spawns[0].y),
            Quaternion.identity);
        
        player.GetComponent<PlayerController>().SetInputController(GetComponent<InputController>());
        
        player = Instantiate(player_2, new Vector3(MapGenerator.Instance.spawns[1].x, 0, MapGenerator.Instance.spawns[1].y),
            Quaternion.identity);
        
        player.GetComponent<PlayerController>().SetInputController(GetComponent<InputController>());
    }

}

public enum GameState{
    MENU,
    RUNNING,
    PAUSE
}

public enum EnemyBehaviour
{
    PLAYER,
    RANDOM,
    MCTS
}