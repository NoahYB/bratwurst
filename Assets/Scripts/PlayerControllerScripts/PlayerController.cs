using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public float gravity = 9.8f;
    public float jumpHeight = 3;
    public float speed = 2f;
    public float sprint = 2.0f;
    CharacterController controller;
    private Vector3 velocity;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        float sprintFactor = Input.GetKey(KeyCode.LeftShift) ? sprint : 1;
        velocity = new Vector3(0, velocity.y, 0);

        velocity += transform.forward * input.y * speed * sprintFactor;  
        velocity += transform.right   * input.x * speed * sprintFactor;  
        if (!controller.isGrounded) velocity += transform.up * -gravity * Time.deltaTime;
        else {
            velocity.y = -1f;
            JumpLogic();
        }
        controller.Move(velocity * Time.deltaTime);
    }

    private void JumpLogic() {
        if (Input.GetKeyDown(KeyCode.Space)) velocity.y = Mathf.Sqrt(jumpHeight * 2.0f * gravity);
    }
}
