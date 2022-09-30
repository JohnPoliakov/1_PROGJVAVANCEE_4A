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
        
        dataTileMap = MapGenerator.Instance.data;//récuperation tableau qui stocke les murs

        dataGroundMap = MapGenerator.Instance.groundGrid;//récuperation tableau qui stocke le sol
        
        
    }

    private void Explode()
    {
        // bool pour savoir s'il faut continuer a propager le souffle de la bombe
        bool rightBool = true;
        bool leftBool = true;
        bool upBool = true;
        bool downBool = true;

        
        IngredientsExplosion = new List<GameObject>();

        //recuperation des joueurs
        player1 = GameObject.FindWithTag("Player_1");
        player2 = GameObject.FindWithTag("Player_2");

        //variables correspondant aux coordonnées des 2 joueurs au moment de l'explosion
        xCoordPlayer1 = (int) (player1.transform.position.x + 0.5f);
        yCoordPlayer1 = (int) (player1.transform.position.z + 0.5f);
        xCoordPlayer2 = (int) (player2.transform.position.x + 0.5f);
        yCoordPlayer2 = (int) (player2.transform.position.z + 0.5f);
        
        //coordonnées de la bombe
        int xCoord = (int)(transform.position.x + 0.5f);
        int yCoord = (int)(transform.position.z + 0.5f);
        
        //si le joueur 1 a les memes coordonnées que la bombe alors on éxecute le script de fin de partie
        if (xCoordPlayer1 == xCoord && yCoordPlayer1 == yCoord)
        {
            EndGame();
        }
        
        //si le joueur 2 a les memes coordonnées que la bombe alors on éxecute le script de fin de partie
        if (xCoordPlayer2 == xCoord && yCoordPlayer2 == yCoord)
        {
            EndGame();

            IngredientsExplosion = new List<GameObject>();//liste des ingredients qui represente la portée de la bombe

        }

        //boucle qui va s'effectuer pour chaque direction selon le radius 
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
        
        if (dataTileMap[xCoord, yCoord].type == 1)
        {
            Destroy(dataTileMap[xCoord, yCoord].attachedGameObject);
            dataTileMap[xCoord, yCoord].type = 0;
            isOK = false;
            StartCoroutine(waitExplode(xCoord, yCoord));
            return;
        }
        if (dataTileMap[xCoord, yCoord].type == -1)
        {
            isOK = false;
            return;
        }
        if (xCoordPlayer1 == xCoord && yCoordPlayer1 == yCoord)
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