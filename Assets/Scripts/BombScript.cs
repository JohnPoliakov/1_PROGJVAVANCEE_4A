using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BombScript : MonoBehaviour
{
    private MapGenerator.CubeData[,] dataTileMap;
    
    private Vector3 currentPosition;
    public int radiusExplosion = 2;
    
    private Vector2Int LEFT_NEIGHBOUR = new Vector2Int(-1, 0);
    private Vector2Int RIGHT_NEIGHBOUR = new Vector2Int(1, 0);
    private Vector2Int UP_NEIGHBOUR = new Vector2Int(0, 1);
    private Vector2Int DOWN_NEIGHBOUR = new Vector2Int(0, -1);
    // Start is called before the first frame update
    void Start()
    {
        currentPosition = transform.position;
        
        dataTileMap = MapGenerator.Instance.data;
    }

    private void Explode()
    {
        bool rightBool = true;
        bool leftBool = true;
        bool upBool = true;
        bool downBool = true;
        
        for (int i = 1; i <= radiusExplosion; i++)
        {
            
            if(upBool)
                CheckBlock(UP_NEIGHBOUR * i, out upBool);
            if(downBool)
                CheckBlock(DOWN_NEIGHBOUR * i, out downBool);
            if(rightBool)
                CheckBlock(RIGHT_NEIGHBOUR * i, out rightBool);
            if(leftBool)
                CheckBlock(LEFT_NEIGHBOUR * i, out leftBool);
        }
    }

    private void CheckBlock(Vector2Int neighbour,out bool isOK)
    {
        int xCoord = (int)(transform.position.x + 0.5f) + neighbour.x;
        int yCoord = (int)(transform.position.z + 0.5f) + neighbour.y;
        
        
        
        if (dataTileMap[xCoord, yCoord].type == 1)
        {
            Destroy(dataTileMap[xCoord, yCoord].attachedGameObject);
            dataTileMap[xCoord, yCoord].type = 0;
            isOK = false;
            return;
        }
        else if (dataTileMap[xCoord, yCoord].type == -1)
        {
            isOK = false;
            return;
        }
        else
        {
            //degats si joueur
        }

        isOK = true;

    }

    private void OnEnable()
    {
        StartCoroutine(WaitCoroutine());
        
    }

    IEnumerator WaitCoroutine()
    {
        yield return new WaitForSeconds(3);
        GetComponent<MeshRenderer>().enabled = false;
        
        Explode();
        
        Destroy(gameObject);
    }
}