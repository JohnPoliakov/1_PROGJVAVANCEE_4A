using System.Collections.Generic;
using UnityEngine;

public class MonteCarlo
{
    private MapGenerator.CubeData[,] grid;

    public MonteCarlo(MapGenerator.CubeData[,] grid)
    {
        this.grid = grid;
    }
    
    public void Compute(Vector3 positionPlayer, Vector3 positionIA)
    {
        
        
        
    }
}

public class Node
{
    private ActionType type;
    private HashSet<Node> childs;
    private int score;

    public Node(ActionType type)
    {
        this.type = type;
        childs = new HashSet<Node>();
    }
    
    
}
public enum ActionType
{
    MOVE_UP,
    MOVE_DOWN,
    MOVE_LEFT,
    MOVE_RIGHT,
    BOMB
}