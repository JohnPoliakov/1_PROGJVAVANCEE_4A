using System.Collections.Generic;
using UnityEngine;

public static class MCTS
{
    private static int MAX_SIMULATION = 20;
    
    public static void Compute(Vector3 position)
    {
        int max = int.MinValue;
        Action bestAction;
        State state = new State()
        {
            position = position,
            actions = new List<ActionType>()
            {
                ActionType.MOVE_UP,
                ActionType.MOVE_DOWN,
                ActionType.MOVE_LEFT,
                ActionType.MOVE_RIGHT,
                ActionType.BOMB
            },
            grid = MapGenerator.Instance.data
        };
            
        state.FilterActions();

        foreach (var action in state.actions)
        {
            int winNumber = Simulate(new Action(action));
        }


    }

    static int Simulate(Action action)
    {
        
        
        
        return 0;
    }
        
}

struct State
{
    public Vector3 position;
    public List<ActionType> actions;
    public MapGenerator.CubeData[,] grid;

    public void FilterActions()
    {
        int x = (int)position.x;
        int z = (int)position.z;

        Vector2Int LEFT = new Vector2Int(-1, 0);
        Vector2Int RIGHT = new Vector2Int(1, 0);
        Vector2Int UP = new Vector2Int(0, 1);
        Vector2Int DOWN = new Vector2Int(0, -1);

        if (x + LEFT.x >= 0 && x + LEFT.x < MapGenerator.Instance.size.x && z + LEFT.y >= 0 && z + LEFT.y < MapGenerator.Instance.size.y)
        {
            MapGenerator.CubeData neighbour = grid[x + LEFT.x, z + LEFT.y];
            if (neighbour.type != 0)
            {
                actions.Remove(ActionType.MOVE_LEFT);
            }
        }else
            actions.Remove(ActionType.MOVE_LEFT);
            
        if (x + RIGHT.x >= 0 && x + RIGHT.x < MapGenerator.Instance.size.x && z + RIGHT.y >= 0 && z + RIGHT.y < MapGenerator.Instance.size.y)
        {
            MapGenerator.CubeData neighbour = grid[x + RIGHT.x, z + RIGHT.y];
            if (neighbour.type != 0)
            {
                actions.Remove(ActionType.MOVE_RIGHT);
            }
        }else
            actions.Remove(ActionType.MOVE_RIGHT);
            
        if (x + UP.x >= 0 && x + UP.x < MapGenerator.Instance.size.x && z + UP.y >= 0 && z + UP.y < MapGenerator.Instance.size.y)
        {
            MapGenerator.CubeData neighbour = grid[x + UP.x, z + UP.y];
            if (neighbour.type != 0)
            {
                actions.Remove(ActionType.MOVE_UP);
            }
        }else
            actions.Remove(ActionType.MOVE_UP);
            
        if (x + DOWN.x >= 0 && x + DOWN.x < MapGenerator.Instance.size.x && z + DOWN.y >= 0 && z + DOWN.y < MapGenerator.Instance.size.y)
        {
            MapGenerator.CubeData neighbour = grid[x + DOWN.x, z + DOWN.y];
            if (neighbour.type != 0)
            {
                actions.Remove(ActionType.MOVE_DOWN);
            }
        }else
            actions.Remove(ActionType.MOVE_DOWN);

    }
}

class Action
{
    private int value;
    private ActionType type;

    public Action(ActionType type)
    {
        this.type = type;
    }


    public void SetValue(int value)
    {
        this.value = value;
    }
}

enum ActionType
{
    MOVE_UP,
    MOVE_DOWN,
    MOVE_LEFT,
    MOVE_RIGHT,
    BOMB
}