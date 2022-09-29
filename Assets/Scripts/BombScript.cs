using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Assertions;

public class BombScript : MonoBehaviour
{
    private MapGenerator.CubeData[,] dataTileMap;
    public List<GameObject> IngredientsSpreadList;
    private List<GameObject> IngredientsExplosion;
    private MapGenerator.CubeData[,] dataGroundMap;


    private GameObject player1;
    private GameObject player2;


    private Vector3 currentPosition;
    private int radiusExplosion = 6;
    private int bombCooldown = 3;
    
    
    private Vector2Int LEFT_NEIGHBOUR = new Vector2Int(-1, 0);
    private Vector2Int RIGHT_NEIGHBOUR = new Vector2Int(1, 0);
    private Vector2Int UP_NEIGHBOUR = new Vector2Int(0, 1);
    private Vector2Int DOWN_NEIGHBOUR = new Vector2Int(0, -1);
    
    private int xCoordPlayer1;
    private int yCoordPlayer1;
    private int xCoordPlayer2;
    private int yCoordPlayer2;
    

    //
    void Start()
    {
        currentPosition = transform.position;
        
        dataTileMap = MapGenerator.Instance.data;

        dataGroundMap = MapGenerator.Instance.groundGrid;
        
        
    }

    private void Explode()
    {
        bool rightBool = true;
        bool leftBool = true;
        bool upBool = true;
        bool downBool = true;

        
        IngredientsExplosion = new List<GameObject>();


        player1 = GameObject.FindWithTag("Player_1");
        player2 = GameObject.FindWithTag("Player_2");
        
        xCoordPlayer1 = (int) (player1.transform.position.x + 0.5f);
        yCoordPlayer1 = (int) (player1.transform.position.z + 0.5f);
        xCoordPlayer2 = (int) (player2.transform.position.x + 0.5f);
        yCoordPlayer2 = (int) (player2.transform.position.z + 0.5f);
        
        int xCoord = (int)(transform.position.x + 0.5f);
        int yCoord = (int)(transform.position.z + 0.5f);
        
        if (xCoordPlayer1 == xCoord && yCoordPlayer1 == yCoord)
        {
            Debug.Log("jhcbsuvebwsd");
            EndGame();
        }

        if (xCoordPlayer2 == xCoord && yCoordPlayer2 == yCoord)
        {
            Debug.Log("jhcbsuvebwsd");
            EndGame();

            IngredientsExplosion = new List<GameObject>();

        }

        for (int i = 1; i <= radiusExplosion; i++)
        {
            
            if(upBool)
                CheckBlock(UP_NEIGHBOUR * i, out upBool, player1, player2);
            if (downBool)
                CheckBlock(DOWN_NEIGHBOUR * i, out downBool, player1, player2);
            if(rightBool)
                CheckBlock(RIGHT_NEIGHBOUR * i, out rightBool, player1, player2);
            if(leftBool)
                CheckBlock(LEFT_NEIGHBOUR * i, out leftBool, player1, player2);
        }
    }

    private void CheckBlock(Vector2Int neighbour,out bool isOK, GameObject player1, GameObject player2)
    {
        int xCoord = (int)(transform.position.x + 0.5f) + neighbour.x;
        int yCoord = (int)(transform.position.z + 0.5f) + neighbour.y;


        Debug.Log("xCoord = " + xCoord);
        Debug.Log("yCoord = " + yCoord);

        Debug.Log("xCoordPlayer1 = " + xCoordPlayer1);
        Debug.Log("yCoordPlayer1 = " + yCoordPlayer1);
        Debug.Log("xCoordPlayer2 = " + xCoordPlayer2);
        Debug.Log("yCoordPlayer2 = " + yCoordPlayer2);
        
        
        if (dataTileMap[xCoord, yCoord].type == 1)
        {
            Destroy(dataTileMap[xCoord, yCoord].attachedGameObject);
            dataTileMap[xCoord, yCoord].type = 0;
            isOK = false;
            StartCoroutine(waitExplode(xCoord, yCoord));
            return;
        }
        else if (dataTileMap[xCoord, yCoord].type == -1)
        {
            isOK = false;
            return;
        }
        else if (xCoordPlayer1 == xCoord && yCoordPlayer1 == yCoord)
        {
            Debug.Log("jhcbsuvebwsd");
            EndGame();
        }
        else if (xCoordPlayer2 == xCoord && yCoordPlayer2 == yCoord)
        {
            Debug.Log("jhcbsuvebwsd");
            EndGame();
        }
            

            StartCoroutine(waitExplode(xCoord, yCoord));
        

        isOK = true;

    }

    private void OnEnable()
    {
        StartCoroutine(WaitCoroutine());
        
    }

    IEnumerator WaitCoroutine()
    {
        
        yield return new WaitForSeconds(bombCooldown);
        GetComponent<MeshRenderer>().enabled = false;
        
        Explode();
        
        
    }

    IEnumerator waitExplode(int xCoord, int yCoord)
    {
        GameObject ingredient = Instantiate(IngredientsSpreadList[Random.Range(0, IngredientsSpreadList.Count)], dataGroundMap[xCoord, yCoord].attachedGameObject.transform.GetChild(0));

        yield return new WaitForSeconds(1);
        Destroy(ingredient);
        Destroy(gameObject);
    }

    private void EndGame()
    {
        foreach (MapGenerator.CubeData data in dataGroundMap)
        {
            if(data.attachedGameObject != null)
                Destroy(data.attachedGameObject);
        }
        foreach (MapGenerator.CubeData data in dataTileMap)
        {
            if(data.attachedGameObject != null)
                Destroy(data.attachedGameObject);
        }
        Destroy(player1);
        Destroy(player2);
        GameManager.Instance.canvas.SetActive(true);
    }
}