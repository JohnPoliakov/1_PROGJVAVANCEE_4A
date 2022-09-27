using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class PlayerScript : MonoBehaviour
{
    private Rigidbody rb;

    public float speed = 10f;
    public Vector3 currentPosition;
    public GameObject prefabTest;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
        Assert.IsTrue(MapGenerator.Instance.IsGenerated);
        
        float xMove = Input.GetAxis("Horizontal");
        float yMove = Input.GetAxis("Vertical");
        rb.velocity = new Vector3(xMove, 0, yMove) * speed;

        currentPosition = transform.position;
        
        if (Input.GetKeyDown(KeyCode.E))
        {
            Instantiate(prefabTest, MapGenerator.Instance.groundGrid[(int)(transform.position.x + 0.5f), (int)(transform.position.z + 0.5f)].attachedGameObject.transform.GetChild(0));
        }
    }
}
