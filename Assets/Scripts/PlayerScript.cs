using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    private Rigidbody rb;

    public float speed = 10f;
    public Vector3 currentPosition;
    public Vector3 currentPlaceOccuped;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float xMove = Input.GetAxis("Horizontal");
        float yMove = Input.GetAxis("Vertical");
        rb.velocity = new Vector3(xMove, 0, yMove) * speed;

        currentPosition = gameObject.transform.position;
    }
    
    

    private void FixedUpdate()
    {
        
    }
}
