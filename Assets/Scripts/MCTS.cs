using System;
using UnityEngine;

public class MCTS : MonoBehaviour
{
    public GameObject bombPrefab;
    public float moveSpeed = 20f;
    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        MCTS_Algo mcts = new MCTS_Algo(MapGenerator.Instance.data);
            
        mcts.Compute(GameObject.FindWithTag("Player_1").transform.position, transform.position);

        switch (mcts.ActionToPlay.GetActionType())
        {
            case ActionType.BOMB:
            {
                Instantiate(bombPrefab, MapGenerator.Instance.groundGrid[(int)(transform.position.x + 0.5f), (int)(transform.position.z + 0.5f)].attachedGameObject.transform.GetChild(0));
                break;
            }

            case ActionType.MOVE_LEFT:
            {
                rb.AddRelativeForce(transform.right * -moveSpeed);
                break;
            }

            case ActionType.MOVE_RIGHT:
            {
                rb.AddRelativeForce(transform.right * moveSpeed);
                break;
            }

            case ActionType.MOVE_UP:
            {
                rb.AddRelativeForce(transform.up * moveSpeed);
                break;
            }

            case ActionType.MOVE_DOWN:
            {
                rb.AddRelativeForce(transform.up * -moveSpeed);
                break;
            }
        }
    }
}