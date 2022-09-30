using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class MCTS_Algo
{
    private int MAX_SIMULATION = 500;
    private float TICK_RATE = 0.01f;
    private List<Bomb> BombsList = new ();
    private GameState GameState = GameState.RUNNING;
    private MapGenerator.CubeData[,] grid;
    private MapGenerator.CubeData[,] gridTemp;

    public static bool check;

    public Action ActionToPlay;
    
    public MCTS_Algo(MapGenerator.CubeData[,] grid)
    {
        this.grid = grid;
        gridTemp = grid;
    }
    
    public void Compute(Vector3 positionPlayer, Vector3 positionIA)
    {
        int max = int.MinValue;
        
        State stateIA = new State()
        {
            position = positionIA,
            actions = new List<ActionType>()
            {
                ActionType.BOMB
            },
            grid = gridTemp
        };
        
        State statePlayer = new State()
        {
            position = positionPlayer,
            actions = new List<ActionType>()
            {
                ActionType.BOMB
            },
            grid = gridTemp
        };
        
        statePlayer.FilterActions();
        stateIA.FilterActions();

        foreach (var actionType in stateIA.actions)
        {

            Action action = new Action(actionType);
            int winNumber = 0;
            
            for (int i = 0; i < MAX_SIMULATION; i++)
            {
                Debug.Log("SIMULATION : "+i);
             
                gridTemp = grid;

                int rlt = Simulate(action, stateIA, statePlayer);

                Debug.Log("RESULT : "+(rlt == 0 ? "DEFEAT" : "VICTORY"));
                
                winNumber += rlt;
                action.AddValue(winNumber);
            }

            if (max < action.GetValue())
            {
                max = action.GetValue();
                ActionToPlay = action;
            }

            Debug.Log("NEED TO PLAY : " +ActionToPlay.GetActionType());

        }


    }
    int Simulate(Action action, State stateIA, State statePlayer)
    {
        BombsList = new();
        int status = -1;

        int limit = 0;
        Debug.Log("SIMULATE");
        
        while (status == -1 && limit <= 100000)
        {

            limit++;
            
            status = HandleBombs(stateIA, statePlayer);
            PlayAction(action, ref stateIA);
            PlayerMove(ref statePlayer);
            Update(ref stateIA, ref statePlayer);
            stateIA.RandomAction();
            action = new Action(stateIA.selectedAction);
        }
        
        if(limit >= 100000)
            Debug.Log("ENDLESS LOOP");
        
        
        return status;
    }

    void PlayerMove(ref State state)
    {
        state.RandomAction();
    }
    
    void PlayAction(Action action, ref State state)
    {
        state.selectedAction = action.GetActionType();
    }
    
    void Update(ref State IA, ref State Player)
    {

        switch (IA.selectedAction)
        {
            case ActionType.BOMB:
                BombsList.Add(new Bomb((int)IA.position.x, (int)IA.position.z));
                break;
            case ActionType.MOVE_LEFT:
                IA.position = new Vector3(IA.position.x - 1, IA.position.y, IA.position.z);
                break;
            case ActionType.MOVE_RIGHT:
                IA.position = new Vector3(IA.position.x + 1, IA.position.y, IA.position.z);
                break;
            case ActionType.MOVE_UP:
                IA.position = new Vector3(IA.position.x, IA.position.y, IA.position.z + 1);
                break;
            case ActionType.MOVE_DOWN:
                IA.position = new Vector3(IA.position.x, IA.position.y, IA.position.z - 1);
                break;
        }
        
        switch (Player.selectedAction)
        {
            case ActionType.BOMB:
                BombsList.Add(new Bomb((int)Player.position.x, (int)Player.position.z));
                break;
            case ActionType.MOVE_LEFT:
                Player.position = new Vector3(Player.position.x - 1, Player.position.y, Player.position.z);
                break;
            case ActionType.MOVE_RIGHT:
                Player.position = new Vector3(Player.position.x + 1, Player.position.y, Player.position.z);
                break;
            case ActionType.MOVE_UP:
                Player.position = new Vector3(Player.position.x, Player.position.y, Player.position.z + 1);
                break;
            case ActionType.MOVE_DOWN:
                Player.position = new Vector3(Player.position.x, Player.position.y, Player.position.z - 1);
                break;
        }
        
        IA.ResetActions();
        Player.ResetActions();
    }

    int HandleBombs(State IA, State Player)
    {
        
        List<Bomb> garbage = new List<Bomb>();
        List<MapGenerator.CubeData> explosions = new ();

        foreach (var bomb in BombsList)
        {

            bomb.timer -= TICK_RATE;
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
                            explosions.Add(gridTemp[(new Vector2Int(0, 1) * i).x + new Vector2Int(bomb.x, bomb.z).x, (new Vector2Int(0, 1) * i).y + new Vector2Int(bomb.x, bomb.z).y]);
                    if (downBool)
                        if(CheckBlock(new Vector2Int(0, -1) * i, new Vector2Int(bomb.x, bomb.z), out downBool))
                            explosions.Add(gridTemp[(new Vector2Int(0, -1) * i).x + new Vector2Int(bomb.x, bomb.z).x, (new Vector2Int(0, -1) * i).y + new Vector2Int(bomb.x, bomb.z).y]);
                    if(rightBool)
                        if(CheckBlock(new Vector2Int(1, 0) * i, new Vector2Int(bomb.x, bomb.z), out rightBool))
                            explosions.Add(gridTemp[(new Vector2Int(1, 0) * i).x + new Vector2Int(bomb.x, bomb.z).x, (new Vector2Int(1, 0) * i).y + new Vector2Int(bomb.x, bomb.z).y]);
                    if(leftBool)
                        if(CheckBlock(new Vector2Int(-1, 0) * i, new Vector2Int(bomb.x, bomb.z), out leftBool))
                            explosions.Add(gridTemp[(new Vector2Int(-1, 0) * i).x + new Vector2Int(bomb.x, bomb.z).x, (new Vector2Int(-1, 0) * i).y + new Vector2Int(bomb.x, bomb.z).y]);
                }
            }

        }

        
        
        foreach (var bomb in garbage)
        {
            BombsList.Remove(bomb);
        }

        foreach (var data in explosions)
        {
            if ((int)IA.position.x == data.x && (int)IA.position.z == data.y)
                return 0;
            if ((int)Player.position.x == data.x && (int)Player.position.z == data.y)
                return 1;
        }
        return -1;
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
            
        
        
        if (gridTemp[xCoord, yCoord].type == 1)
        {
            gridTemp[xCoord, yCoord].type = 0;
            direction = false;
            return false;
        }

        if (gridTemp[xCoord, yCoord].type == -1)
        {
            direction = false;
            return false;
        }

        direction = true;
        return true;
    }
}

class Bomb
{
    public int x;
    public int z;
    public float timer;

    public Bomb(int x, int z)
    {
        this.x = x;
        this.z = z;
        this.timer = 3;
    }
}

struct State
{
    public Vector3 position;
    public List<ActionType> actions;
    public ActionType selectedAction;
    public MapGenerator.CubeData[,] grid;

    public void ResetActions()
    {
        actions = new List<ActionType>()
        {
            ActionType.BOMB
        };
        
        FilterActions();
    }

    public void RandomAction()
    {
        selectedAction = actions[Random.Range(0, actions.Count)];
    }
    
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

        if (MCTS_Algo.check)
        {
            foreach (var VARIABLE in actions)
            {
                Debug.Log(VARIABLE);
            }

            MCTS_Algo.check = true;
        }
        

    }
}

public class Action
{
    private int value = 0;
    private ActionType type;

    public Action(ActionType type)
    {
        this.type = type;
    }

    public int GetValue()
    {
        return value;
    }

    public ActionType GetActionType()
    {
        return type;
    }


    public void AddValue(int value)
    {
        this.value += value;
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