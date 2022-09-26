using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    private Rigidbody rb;

    public float speed = 10f;
    public Vector3 currentPosition;
    public GameObject currentPlaceOccuped;
    public GameObject prefabTest;
    
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
        
        if (Input.GetKeyDown(KeyCode.E))
        {
            GameObject objInstantiate = Instantiate(prefabTest, currentPlaceOccuped.transform.GetChild(0));
            List<GameObject> explosionBlock = new List<GameObject>();
            Vector3 positionExplosionRight = objInstantiate.transform.position + new Vector3(1,0);
            Destroy(objInstantiate, 3);
        }
    }
    
    

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Grass")
        {
            currentPlaceOccuped = collision.gameObject; 
        }
        
    }

    private void FixedUpdate()
    {
        
    }
}
