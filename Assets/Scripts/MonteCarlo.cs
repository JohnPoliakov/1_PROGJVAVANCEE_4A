using System.Collections.Generic;
using UnityEngine;

/*
 *
 *
 * CRASH COMME MTCS_Algo, MTCS c'est pas ouf :/
 *
 * 
 */

public class MonteCarlo
{
    private MapGenerator.CubeData[,] grid;
    private List<Node> nodes;
    
    private int MAX_SIMULATION = 500;
    private int MAX_NOEUDS = 15;
    public static float TICK_RATE = 0.01f;

    public MonteCarlo(MapGenerator.CubeData[,] grid)
    {
        this.grid = grid;
        this.nodes = new();
    }
    
    public ActionType Compute(Vector3 positionPlayer, Vector3 positionIA)
    {

        int max = int.MinValue;
        ActionType bestAction = ActionType.BOMB;
        
        Node StartNode = new Node(null, grid, positionPlayer, positionIA, new List<ActionType>());
        
        
        for (int i = 0; i < MAX_NOEUDS; i++)
        {
            Node selectedNode = Selection(StartNode);
            Node newNode = Expand(selectedNode);
            
            int score = Simulate(newNode);
            BackPropagation(newNode, score, MAX_SIMULATION);
        }

        foreach (var child in StartNode.children)
        {
            if (max < child.ratio)
            {
                max = child.ratio;
                bestAction = child.selectedAction;
            }
        }

        return bestAction;
    }

    int Simulate(Node node)
    {

        int score = 0;
        
        for (int i = 0; i < MAX_SIMULATION; i++)
        {
            Node tmp = node;
            int limit = 0;
            
            while (tmp.status == -1 && limit <= 100000)
            {
                limit++;
                
                tmp.RandomAction();
                tmp.PlayAction();
                tmp.PlayerAction();
                tmp.HandleBombs();

            }
            
            if(limit >= 100000)
                Debug.Log("ENDLESS LOOP");

            score += tmp.status;
        }


        return score;
    }
    void BackPropagation(Node node, int score, int simulationCount)
    {

        while (node != null)
        {
            node.score += score;
            node.totalGame += simulationCount;
            node.ratio = node.score / node.totalGame;
            node = node.parent;
        }
        
    }

    Node Expand(Node node)
    {

        List<ActionType> blackList = new List<ActionType>();

        foreach (var child in node.children)
        {
            blackList.Add(child.selectedAction);
        }
        
        Node expandedNode = new Node(node, node.grid, node.positionPlayer, node.positionIA, blackList);
        
        expandedNode.RandomAction();

        expandedNode.UpdateState();
        
        return expandedNode;
    }
    
    
    
    Node Selection(Node StartNode)
    {
        // 80% explorer || 20% exploiter
        if (Random.Range(0, 100) < 80)
        {
            if (nodes.Count == 0)
                return StartNode;
            
            return nodes[Random.Range(0, nodes.Count)];
        }
        else
        {

            int bestRatio = int.MinValue;
            Node bestNode = null;
            
            foreach (var node in nodes)
            {
                if (node.ratio > bestRatio)
                {
                    bestRatio = node.ratio;
                    bestNode = node;
                }
            }

            return bestNode;
        }
    }
}

public class Node
{
    public List<Bomb> BombsList = new ();
    public Node parent;
    public HashSet<Node> children;
    public MapGenerator.CubeData[,] grid;
    public Vector3 positionPlayer;
    public Vector3 positionIA;
    public List<ActionType> actions;
    public ActionType selectedAction;
    public int score;
    public int totalGame;
    public int ratio;
    public int status = -1;

    public Node(Node parent, MapGenerator.CubeData[,] grid, Vector3 positionPlayer, Vector3 positionIA, List<ActionType> blackList)
    {
        this.parent = parent;
        this.grid = grid;
        this.positionPlayer = positionPlayer;
        this.positionIA = positionIA;
        children = new HashSet<Node>();
        actions = new List<ActionType>()
        {
            ActionType.BOMB
        };
        
        FilterActions(blackList);
    }

    public void UpdateState()
    {
        HandleBombs();
        PlayAction();
        PlayerAction();
    }

    public void HandleBombs()
    {
        List<Bomb> garbage = new List<Bomb>();
        List<MapGenerator.CubeData> explosions = new ();
        
        foreach (var bomb in BombsList)
        {

            bomb.timer -= MonteCarlo.TICK_RATE;
            if (bomb.timer <= 0)
            {
                garbage.Add(bomb);

                bool rightBool = true;
                bool leftBool = true;
                bool upBool = true;
                bool downBool = true;
                
                for (int i = 1; i <= 6; i++)
                {
                    
                    if(upBool)
                        if(CheckBlock(new Vector2Int(0, 1) * i, new Vector2Int(bomb.x, bomb.z), out upBool))
                            explosions.Add(grid[(new Vector2Int(0, 1) * i).x + new Vector2Int(bomb.x, bomb.z).x, (new Vector2Int(0, 1) * i).y + new Vector2Int(bomb.x, bomb.z).y]);
                    if (downBool)
                        if(CheckBlock(new Vector2Int(0, -1) * i, new Vector2Int(bomb.x, bomb.z), out downBool))
                            explosions.Add(grid[(new Vector2Int(0, -1) * i).x + new Vector2Int(bomb.x, bomb.z).x, (new Vector2Int(0, -1) * i).y + new Vector2Int(bomb.x, bomb.z).y]);
                    if(rightBool)
                        if(CheckBlock(new Vector2Int(1, 0) * i, new Vector2Int(bomb.x, bomb.z), out rightBool))
                            explosions.Add(grid[(new Vector2Int(1, 0) * i).x + new Vector2Int(bomb.x, bomb.z).x, (new Vector2Int(1, 0) * i).y + new Vector2Int(bomb.x, bomb.z).y]);
                    if(leftBool)
                        if(CheckBlock(new Vector2Int(-1, 0) * i, new Vector2Int(bomb.x, bomb.z), out leftBool))
                            explosions.Add(grid[(new Vector2Int(-1, 0) * i).x + new Vector2Int(bomb.x, bomb.z).x, (new Vector2Int(-1, 0) * i).y + new Vector2Int(bomb.x, bomb.z).y]);
                }
            }

        }
        
        foreach (var bomb in garbage)
        {
            BombsList.Remove(bomb);
        }
        
        foreach (var data in explosions)
        {
            if ((int) positionIA.x == data.x && (int) positionIA.z == data.y)
                status = 0;
            if ((int)positionPlayer.x == data.x && (int)positionPlayer.z == data.y)
                status = 1;
        }
        status = -1;
    }

    private bool CheckBlock(Vector2Int neighbour, Vector2Int origin, out bool direction)
    {
        
        int xCoord = origin.x + neighbour.x;
        int yCoord = origin.y + neighbour.y;

        if (xCoord < 0 || xCoord >= MapGenerator.Instance.size.x || yCoord < 0 ||
            yCoord >= MapGenerator.Instance.size.y)
        {
            direction = false;
            return false;
        }
            
        
        
        if (grid[xCoord, yCoord].type == 1)
        {
            grid[xCoord, yCoord].type = 0;
            direction = false;
            return false;
        }

        if (grid[xCoord, yCoord].type == -1)
        {
            direction = false;
            return false;
        }

        direction = true;
        return true;
    }

    
    public void PlayAction()
    {
        switch (selectedAction)
        {
            case ActionType.BOMB:
            {
                BombsList.Add(new Bomb((int)positionIA.x, (int)positionIA.z));
                break;
            }

            case ActionType.MOVE_LEFT:
            {
                positionIA = new Vector3(positionIA.x - 1, positionIA.y, positionIA.z);
                break;
            }

            case ActionType.MOVE_RIGHT:
            {
                positionIA = new Vector3(positionIA.x + 1, positionIA.y, positionIA.z);
                break;
            }

            case ActionType.MOVE_UP:
            {
                positionIA = new Vector3(positionIA.x, positionIA.y, positionIA.z + 1);
                break;
            }

            case ActionType.MOVE_DOWN:
            {
                positionIA = new Vector3(positionIA.x, positionIA.y, positionIA.z - 1);
                break;
            }
        }
    }


    public void PlayerAction()
    {

        List<ActionType> actions = new List<ActionType>()
        {
            ActionType.BOMB
        };
        
        // C'est moche, je sais mais j'ai paniqué
        
        int x = (int)(positionPlayer.x - 0.5f);
        int z = (int)(positionPlayer.z- 0.5f);

        Vector2Int LEFT = new Vector2Int(-1, 0);
        Vector2Int RIGHT = new Vector2Int(1, 0);
        Vector2Int UP = new Vector2Int(0, 1);
        Vector2Int DOWN = new Vector2Int(0, -1);

        if (x + LEFT.x >= 0 && x + LEFT.x < MapGenerator.Instance.size.x && z + LEFT.y >= 0 && z + LEFT.y < MapGenerator.Instance.size.y)
        {
            
            MapGenerator.CubeData neighbour = grid[x + LEFT.x, z + LEFT.y];
            if (neighbour.type == 0)
            {
                actions.Add(ActionType.MOVE_LEFT);
            }
        }
            
        if (x + RIGHT.x >= 0 && x + RIGHT.x < MapGenerator.Instance.size.x && z + RIGHT.y >= 0 && z + RIGHT.y < MapGenerator.Instance.size.y)
        {
            MapGenerator.CubeData neighbour = grid[x + RIGHT.x, z + RIGHT.y];
            if (neighbour.type != 0)
            {
                actions.Add(ActionType.MOVE_RIGHT);
            }
        }
            
        if (x + UP.x >= 0 && x + UP.x < MapGenerator.Instance.size.x && z + UP.y >= 0 && z + UP.y < MapGenerator.Instance.size.y)
        {
            MapGenerator.CubeData neighbour = grid[x + UP.x, z + UP.y];
            if (neighbour.type != 0)
            {
                actions.Add(ActionType.MOVE_UP);
            }
        }

        if (x + DOWN.x >= 0 && x + DOWN.x < MapGenerator.Instance.size.x && z + DOWN.y >= 0 && z + DOWN.y < MapGenerator.Instance.size.y)
        {
            MapGenerator.CubeData neighbour = grid[x + DOWN.x, z + DOWN.y];
            if (neighbour.type != 0)
            {
                actions.Add(ActionType.MOVE_DOWN);
            }
        }

        ActionType random = actions[Random.Range(0, actions.Count)];

        switch (random)
        {
            case ActionType.BOMB:
            {
                BombsList.Add(new Bomb((int)positionPlayer.x, (int)positionPlayer.z));
                break;
            }

            case ActionType.MOVE_LEFT:
            {
                positionPlayer = new Vector3(positionPlayer.x - 1, positionPlayer.y, positionPlayer.z);
                break;
            }

            case ActionType.MOVE_RIGHT:
            {
                positionPlayer = new Vector3(positionPlayer.x + 1, positionPlayer.y, positionPlayer.z);
                break;
            }

            case ActionType.MOVE_UP:
            {
                positionPlayer = new Vector3(positionPlayer.x, positionPlayer.y, positionPlayer.z + 1);
                break;
            }

            case ActionType.MOVE_DOWN:
            {
                positionPlayer = new Vector3(positionPlayer.x, positionPlayer.y, positionPlayer.z - 1);
                break;
            }
        }

    }
    
    
    public void RandomAction()
    {
        selectedAction = actions[Random.Range(0, actions.Count)];
    }
    
    public void FilterActions(List<ActionType> blackList)
    {
        int x = (int)(positionIA.x - 0.5f);
        int z = (int)(positionIA.z- 0.5f);

        Vector2Int LEFT = new Vector2Int(-1, 0);
        Vector2Int RIGHT = new Vector2Int(1, 0);
        Vector2Int UP = new Vector2Int(0, 1);
        Vector2Int DOWN = new Vector2Int(0, -1);

        if (x + LEFT.x >= 0 && x + LEFT.x < MapGenerator.Instance.size.x && z + LEFT.y >= 0 && z + LEFT.y < MapGenerator.Instance.size.y)
        {
            
            MapGenerator.CubeData neighbour = grid[x + LEFT.x, z + LEFT.y];
            if (neighbour.type == 0)
            {
                actions.Add(ActionType.MOVE_LEFT);
            }
        }
            
        if (x + RIGHT.x >= 0 && x + RIGHT.x < MapGenerator.Instance.size.x && z + RIGHT.y >= 0 && z + RIGHT.y < MapGenerator.Instance.size.y)
        {
            MapGenerator.CubeData neighbour = grid[x + RIGHT.x, z + RIGHT.y];
            if (neighbour.type != 0)
            {
                actions.Add(ActionType.MOVE_RIGHT);
            }
        }
            
        if (x + UP.x >= 0 && x + UP.x < MapGenerator.Instance.size.x && z + UP.y >= 0 && z + UP.y < MapGenerator.Instance.size.y)
        {
            MapGenerator.CubeData neighbour = grid[x + UP.x, z + UP.y];
            if (neighbour.type != 0)
            {
                actions.Add(ActionType.MOVE_UP);
            }
        }

        if (x + DOWN.x >= 0 && x + DOWN.x < MapGenerator.Instance.size.x && z + DOWN.y >= 0 && z + DOWN.y < MapGenerator.Instance.size.y)
        {
            MapGenerator.CubeData neighbour = grid[x + DOWN.x, z + DOWN.y];
            if (neighbour.type != 0)
            {
                actions.Add(ActionType.MOVE_DOWN);
            }
        }

        
        
        foreach (var action in blackList)
        {
            if (actions.Contains(action))
            {
                actions.Remove(action);
            }
        }

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