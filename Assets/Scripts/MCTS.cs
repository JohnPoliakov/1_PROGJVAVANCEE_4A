using System;
using UnityEngine;

public class MCTS : MonoBehaviour
{
    public GameObject bombPrefab;
    float moveSpeed = 100f;
    Rigidbody rb;

    private float cooldown = 1.1f;
    private float lastDecision;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        
        if(Time.time < lastDecision)
            return;

        lastDecision = Time.time + cooldown; 
        
        MonteCarlo mcts = new MonteCarlo(MapGenerator.Instance.data);

        switch (mcts.Compute(GameObject.FindWithTag("Player_1").transform.position, transform.position))
        {
            case ActionType.BOMB:
            {
                Debug.Log("DROP BOMB");
                Instantiate(bombPrefab, MapGenerator.Instance.groundGrid[(int)(transform.position.x + 0.5f), (int)(transform.position.z + 0.5f)].attachedGameObject.transform.GetChild(0));
                break;
            }

            case ActionType.MOVE_LEFT:
            {
                Debug.Log("MOVE LEFT");
                rb.AddRelativeForce(transform.right * -moveSpeed);
                break;
            }

            case ActionType.MOVE_RIGHT:
            {
                Debug.Log("MOVE RIGHT");
                rb.AddRelativeForce(transform.right * moveSpeed);
                break;
            }

            case ActionType.MOVE_UP:
            {
                Debug.Log("MOVE UP");
                rb.AddRelativeForce(transform.forward * moveSpeed);
                break;
            }

            case ActionType.MOVE_DOWN:
            {
                Debug.Log("MOVE DOWN");
                rb.AddRelativeForce(transform.forward * -moveSpeed);
                break;
            }
        }
    }
}