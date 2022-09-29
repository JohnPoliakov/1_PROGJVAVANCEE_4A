using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [Header("Pause")]
    [SerializeField]
    GameObject PauseMenu;
    [SerializeField]
    public GameObject canvas;
    [SerializeField]
    GameObject player_1;
    [SerializeField]
    GameObject player_2;
    [SerializeField]
    GameObject IARandom;
    public bool IsPause;

    [SerializeField] 
    private TMP_Dropdown TypeGame;
    
    public TMP_Dropdown ResolutionDropdown;
    private Resolution[] resolutions;
    
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
        
        resolutions = Screen.resolutions;
        ResolutionDropdown.ClearOptions();
        
        List<string> options = new List<string>();

        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
            {
                currentResolutionIndex = i;
            }
        }   
        ResolutionDropdown.AddOptions(options);
        ResolutionDropdown.value = currentResolutionIndex;
        ResolutionDropdown.RefreshShownValue();
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
        
        LaunchTypeGame();

        Camera.main.transform.position = new Vector3(MapGenerator.Instance.size.x / 2, MapGenerator.Instance.size.x, 0);

        
        canvas.SetActive(false);
        
    }

    void SpawnPlayers()
    {
        Debug.Log("spawn player");
        GameObject player = Instantiate(player_1, new Vector3(MapGenerator.Instance.spawns[0].x, 0, MapGenerator.Instance.spawns[0].y),
            Quaternion.identity);
        
        player.GetComponent<PlayerController>().SetInputController(GetComponent<InputController>());
        
        player = Instantiate(player_2, new Vector3(MapGenerator.Instance.spawns[1].x, 0, MapGenerator.Instance.spawns[1].y),
            Quaternion.identity);
        
        player.GetComponent<PlayerController>().SetInputController(GetComponent<InputController>());
    }

    private void LaunchTypeGame()
    {
        if (TypeGame.value == 0)
        {
            Debug.Log("bcsiludvbsd");
            SpawnPlayers();
        }
        else
        {
            Debug.Log("Come Soon");
        }
        
    }
    
    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

}

public enum ENEMY_BEHAVIOUR
{
     _n
}