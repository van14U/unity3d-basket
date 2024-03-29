﻿using UnityEngine;

public class PlayerController : MonoBehaviour
{
  public GameObject ball;
  public GameObject powerBar;
  public float throwStregth = 5.0f;
  public float ballDistance = 2.0f;
  public float speed = 3.0f;
  public float gravity = -5.0f;
  public float sensHorizontal = 5.0f;

  private CharacterController _characterController;
  private Camera _camera;
  private float _translation;
  private float _straffe;
  private bool _isPicked;
  private bool _canThrow;


  void Start()
  {
    _characterController = GetComponent<CharacterController>();
    _camera = GetComponentInChildren<Camera>();
    Cursor.lockState = CursorLockMode.Locked;
  }

  void Update()
  {
    // Move();
    // Rotate();

    if (_isPicked)
    {
      var ballPosition = _camera.transform.position + _camera.transform.forward * ballDistance;
      ball.GetComponent<Rigidbody>().useGravity = false;
      ball.GetComponent<Rigidbody>().velocity = Vector3.zero;
      ball.transform.position = ballPosition;
      ball.GetComponent<Collider>().enabled = false;

      Throw();
    }

    // if (Input.GetKey(KeyCode.F))
    // {
    //     _isPicked = true;
    //     _canThrow = false;
    // }

    // if (Input.GetKeyDown("escape"))
    // {
    //     Cursor.lockState = CursorLockMode.None;
    // }
  }

  private void Move()
  {
    _translation = Input.GetAxis("Vertical") * speed;
    _straffe = Input.GetAxis("Horizontal") * speed;

    Vector3 movement = new Vector3(_straffe, gravity, _translation);
    movement = Vector3.ClampMagnitude(movement, speed);
    movement *= Time.deltaTime;
    movement = transform.TransformDirection(movement);

    _characterController.Move(movement);
  }

  private void Rotate()
  {
    transform.Rotate(0, Input.GetAxis("Mouse X") * sensHorizontal, 0);
  }

  private void Throw()
  {
    if (Input.GetMouseButton(0))
    {
      _canThrow = true;
      if (throwStregth < 30)
      {
        throwStregth += 0.5f;
      }
    }
    else if (!Input.GetMouseButton(0) && _canThrow)
    {
      _isPicked = false;
      var rigidBody = ball.GetComponent<Rigidbody>();
      rigidBody.useGravity = true;
      ball.GetComponent<Collider>().enabled = true;
      rigidBody.AddForce(_camera.transform.forward * throwStregth);
      throwStregth = 5.0f;
    }

    powerBar.transform.position = new Vector3(powerBar.transform.position.x, throwStregth * 6);
  }
}