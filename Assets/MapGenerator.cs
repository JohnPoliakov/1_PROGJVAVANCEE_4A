using UnityEngine;

public class MapGenerator : MonoBehaviour
{

    public Texture2D[] mapPatterns;
    public CubeData[,] data;
    public CubeData[,] groundGrid;
    public bool IsGenerated;
    public GameObject groundPrefab;
    public static MapGenerator Instance;

    


    
    private Vector2[] neighbours = new[]
    {
        new Vector2(1, 0),
        new Vector2(-1, 0),
        new Vector2(0, 1),
        new Vector2(0, -1),
    };

    
    
    
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
        
        Texture2D mapPattern = mapPatterns[Random.Range(0, mapPatterns.Length)];

        data = new CubeData[mapPattern.width, mapPattern.height];
        groundGrid = new CubeData[mapPattern.width, mapPattern.height];

        GameObject ground = new GameObject();
        ground.name = "Ground";
        ground.transform.position = Vector3.zero;
        ground.transform.SetParent(transform);
        
        for (int x = 0; x < mapPattern.width; x++)
        {
            for (int y = 0; y < mapPattern.height; y++)
            {

                GameObject grass = Instantiate(groundPrefab, new Vector3(x, -1, y), Quaternion.identity,
                    ground.transform);
                grass.name = "Grass";

                GameObject go = null;
                if (IsBedrock(mapPattern.GetPixel(x, y)) || IsWall(mapPattern.GetPixel(x, y)))
                {
                    go = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    go.transform.position = new Vector3(x, 0, y);
                    go.transform.rotation = Quaternion.identity;
                    go.transform.SetParent(transform);
                    go.GetComponent<MeshRenderer>().material.color = mapPattern.GetPixel(x, y);
                }
                
                groundGrid[x, y] = new CubeData()
                {
                    x = x,
                    y = y,
                    attachedGameObject = grass
                };
                
                data[x, y] = new CubeData()
                {
                    x = x,
                    y = y,
                    type = IsBedrock(mapPattern.GetPixel(x, y)) ? -1 : IsWall(mapPattern.GetPixel(x, y)) ? 1 : 0,
                    attachedGameObject = go
                };

            }
        }

        IsGenerated = true;
    }

    bool IsBedrock(Color color)
    {
        return color.r == 0 && color.g == 0 &&  color.b == 0;
    }
    
    bool IsWall(Color color)
    {
        return color.r.Equals(1.0f) && color.g == 0 &&  color.b == 0;
    }

    public struct CubeData
    {
        public int x;
        public int y;
        public int type;
        public GameObject attachedGameObject;

        public void Hello()
        {
            
        }
    }

}
