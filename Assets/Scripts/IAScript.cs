using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class IAScript : MonoBehaviour
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
            CanDrop = false;
        }
        
    }
    IEnumerator Wander()
    {
        int walkTime = 1;
        float walkwait = 0.5f;
        int walkDirection = Random.Range(1, 5);
        

        int bombwait = Random.Range(1, 5);
        int bombDrop = Random.Range(5, 100);

        if (bombDrop <= 5)
        {
            CanDrop = true;
        } 
        
        isWandering = true;
        
        yield return new WaitForSeconds(walkwait);
        if (walkDirection == 1)
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
            
        }
        if (walkDirection == 2)
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
        }
        if (walkDirection == 3)
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
        }
        if (walkDirection == 4)
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
        }
        isWandering = false;
    }
}
