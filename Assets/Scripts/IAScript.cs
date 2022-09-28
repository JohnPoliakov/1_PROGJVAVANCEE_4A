using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class IAScript : MonoBehaviour
{
    public float movSpeed = 2f;


    private bool isWandering = false;
    private bool isWalkingLeft = false;
    private bool isWalkingRight = false;
    private bool isWalkingDown = false;
    private bool isWalkingUp = false;

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
            rb.AddRelativeForce(transform.right *  -movSpeed);
        }
        if (isWalkingRight == true)
        {
            rb.AddRelativeForce(transform.right *  movSpeed);
        }
        if (isWalkingDown == true)
        {
            rb.AddRelativeForce(transform.forward *  -movSpeed);
            
        }
        if (isWalkingUp == true)
        {
            rb.AddRelativeForce(transform.forward *  movSpeed);
        }
        
    }
    IEnumerator Wander()
    {
        int walkTime = Random.Range(1, 3);
        int walkwait = Random.Range(1, 3);
        int walkDirection = Random.Range(1, 5);

        isWandering = true;

        yield return new WaitForSeconds(walkwait);
        if (walkDirection == 1)
        {
            isWalkingUp = true;
            yield return new WaitForSeconds(walkTime);
            isWalkingDown = false;
            isWalkingLeft = false;
            isWalkingRight = false;
        }
        if (walkDirection == 2)
        {
            isWalkingDown = true;
            yield return new WaitForSeconds(walkTime);
            isWalkingUp = false;
            isWalkingLeft = false;
            isWalkingRight = false;
        }
        if (walkDirection == 3)
        {
            isWalkingLeft = true;
            yield return new WaitForSeconds(walkTime);
            isWalkingDown = false;
            isWalkingUp = false;
            isWalkingRight = false;
        }
        if (walkDirection == 4)
        {
            isWalkingRight = true;
            yield return new WaitForSeconds(walkTime);
            isWalkingDown = false;
            isWalkingLeft = false;
            isWalkingUp = false;
        }
        isWandering = false;
    }
}
