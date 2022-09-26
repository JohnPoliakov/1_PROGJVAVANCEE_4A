using UnityEngine;

public class MapGenerator : MonoBehaviour
{

    public Texture2D[] mapPatterns;
    private CubeData[,] data;
    public GameObject groundPrefab;

    private Vector2 LEFT_NEIGHBOUR = new Vector2(-1, 0);
    private Vector2 RIGHT_NEIGHBOUR = new Vector2(1, 0);
    private Vector2 UP_NEIGHBOUR = new Vector2(0, 1);
    private Vector2 DOWN_NEIGHBOUR = new Vector2(0, -1);
    
    private Vector2[] neighbours = new[]
    {
        new Vector2(1, 0),
        new Vector2(-1, 0),
        new Vector2(0, 1),
        new Vector2(0, -1),
    };

    void Explode(int x, int y)
    {
        
        for (int i = 1; i < 5; i++)
        {

            CubeData dat = data[Mathf.FloorToInt(i * LEFT_NEIGHBOUR.x), Mathf.FloorToInt(i * LEFT_NEIGHBOUR.y)];
            
            
        }

        Vector2 input;
        
        
        
        

    }
    
    private void Awake()
    {
        Texture2D mapPattern = mapPatterns[Random.Range(0, mapPatterns.Length)];

        data = new CubeData[mapPattern.width, mapPattern.height];

        GameObject ground = new GameObject();
        ground.transform.position = Vector3.zero;
        ground.transform.SetParent(transform);
        
        for (int x = 0; x < mapPattern.width; x++)
        {
            for (int y = 0; y < mapPattern.height; y++)
            {

                Instantiate(groundPrefab, new Vector3(x, -1, y), Quaternion.identity,
                    ground.transform);


                if (isBedrock(mapPattern.GetPixel(x, y)) || isWall(mapPattern.GetPixel(x, y)))
                {
                    GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    go.transform.position = new Vector3(x, 0, y);
                    go.transform.rotation = Quaternion.identity;
                    go.transform.SetParent(transform);
                    go.GetComponent<MeshRenderer>().material.color = mapPattern.GetPixel(x, y);
                }

                data[x, y] = new CubeData()
                {
                    x = x,
                    y = y,
                    type = isBedrock(mapPattern.GetPixel(x, y)) ? -1 : isWall(mapPattern.GetPixel(x, y)) ? 1 : 0
                };
                
            }
        }
    }

    bool isBedrock(Color color)
    {
        return color.r == 0 && color.g == 0 &&  color.b == 0;
    }
    
    bool isWall(Color color)
    {
        return color.r.Equals(1.0f) && color.g == 0 &&  color.b == 0;
    }

    private struct CubeData
    {
        public int x;
        public int y;
        public int type;

        public void Hello()
        {
            
        }
    }

}
