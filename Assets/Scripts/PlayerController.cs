using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class PlayerController : MonoBehaviour
{
    private Rigidbody _rigidbody = null;
    private InputController _input;

    private Vector3 _playerMoveInput = Vector3.zero;
    private Vector3 _playerBombInput = Vector3.zero;

    public PlayerType playerType;
    public GameObject bombPrefab;

    private float fireRate = 4f;
    private float nextFire;
    
    
    [Header("Movement")] [SerializeField] private float _movementMultiplier = 30.0f;
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void SetInputController(InputController controller)
    {
        _input = controller;
    }
    
    private void Update()
    {
        Assert.IsTrue(MapGenerator.Instance.IsGenerated);
        _playerMoveInput = GetMoveInput();
        PlayerMove();
        _rigidbody.AddRelativeForce(_playerMoveInput, ForceMode.Force);

        
 }

    private Vector3 GetMoveInput()
    {
        if(playerType == PlayerType.PLAYER_1)
            return new Vector3(_input.MoveInput.x, 0.0f, _input.MoveInput.y);
        else
        {
            return new Vector3(_input.MoveInput2.x, 0.0f, _input.MoveInput2.y);
        }
    }

    private void PlayerMove()
    {
        _playerMoveInput = (new Vector3(_playerMoveInput.x * _movementMultiplier * _rigidbody.mass,
            _playerMoveInput.y,
            _playerMoveInput.z * _movementMultiplier * _rigidbody.mass
        ));

    }
    
    public void SpawnBomb()
    {
        if (Time.time > nextFire)
        {
            if (playerType == PlayerType.PLAYER_1)
            {
                Instantiate(bombPrefab, MapGenerator.Instance.groundGrid[(int)(transform.position.x + 0.5f), (int)(transform.position.z + 0.5f)].attachedGameObject.transform.GetChild(0));
            }
            else
            {
                Instantiate(bombPrefab, MapGenerator.Instance.groundGrid[(int)(transform.position.x + 0.5f), (int)(transform.position.z + 0.5f)].attachedGameObject.transform.GetChild(0));

            }

            nextFire = Time.time + fireRate;
        }
        
        


    }
    public enum PlayerType
    {
        PLAYER_1,
        PLAYER_2
    }
    
}
