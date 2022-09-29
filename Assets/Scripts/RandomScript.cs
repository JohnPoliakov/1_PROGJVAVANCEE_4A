using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomScript : MonoBehaviour
{
    public float moveSpeed = 20f;


    private bool isWandering = false;
    private bool isWalkingLeft = false;
    private bool isWalkingRight = false;
    private bool isWalkingDown = false;
    private bool isWalkingUp = false;
    private bool isBombing = false;
    private bool CanDrop = false;
    
    public GameObject bombPrefab;
    Rigidbody rb;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        if (isWandering == false)
        {
            StartCoroutine(Wander());
        }
        if (isWalkingLeft == true)
        {
            rb.AddRelativeForce(transform.right *  -moveSpeed);
        }
        if (isWalkingRight == true)
        {
            rb.AddRelativeForce(transform.right *  moveSpeed);
        }
        if (isWalkingDown == true)
        {
            rb.AddRelativeForce(transform.forward *  -moveSpeed);
            
        }
        if (isWalkingUp == true)
        {
            rb.AddRelativeForce(transform.forward *  moveSpeed);
        }
        if (isBombing == true)
        {
            Instantiate(bombPrefab, MapGenerator.Instance.groundGrid[(int)(transform.position.x + 0.5f), (int)(transform.position.z + 0.5f)].attachedGameObject.transform.GetChild(0));
            isBombing = false;
        }

    }
    IEnumerator Wander()
    {
        int walkTime = 1;
        float walkwait = 0.5f;
        MoveDirection direction = (MoveDirection) Enum.GetValues(typeof(MoveDirection)).GetValue(Random.Range(0, 4));

        int bombDrop = Random.Range(1, 100);

        if (bombDrop <= 5)
        {
            CanDrop = true;
        } 
        
        isWandering = true;
        
        yield return new WaitForSeconds(walkwait);

        switch (direction)
        {
            case MoveDirection.UP:
            {
                
                isWalkingUp = true;
                yield return new WaitForSeconds(walkTime);
                isWalkingDown = false;
                isWalkingLeft = false;
                isWalkingRight = false;

                if (CanDrop == true)
                { 
                    isBombing = true;
                }
                break;
            }

            case MoveDirection.DOWN:
            {
                
                isWalkingDown = true;
                yield return new WaitForSeconds(walkTime);
                isWalkingUp = false;
                isWalkingLeft = false;
                isWalkingRight = false;
                if (CanDrop == true)
                { 
                    isBombing = true;
                }
                break;
            }

            case MoveDirection.LEFT:
            {
                isWalkingLeft = true;
                yield return new WaitForSeconds(walkTime);
                isWalkingDown = false;
                isWalkingUp = false;
                isWalkingRight = false;
                if (CanDrop == true)
                { 
                    isBombing = true;
                }

                break;
            }

            case MoveDirection.RIGHT:
            {
                isWalkingRight = true;
                yield return new WaitForSeconds(walkTime);
                isWalkingDown = false;
                isWalkingLeft = false;
                isWalkingUp = false;
                if (CanDrop == true)
                { 
                    isBombing = true;
                }

                break;
            }
            
        }
        isWandering = false;
    }
    
}
public enum MoveDirection
{
    LEFT,
    RIGHT,
    UP,
    DOWN
}
