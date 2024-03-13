using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.VFX;


[RequireComponent(typeof(CharacterController))]
public class PlayerController : NetworkBehaviour
{
    
    // Tunable Variables
    public float Speed { get; set; }
    public float Sprint { get; set; }
    public float Gravity { get; set; }
    public float JumpHeight { get; set; }
    CharacterController controller;
    private Vector3 velocity;
    public VisualEffect flash;
    public GameObject gun;
    public override void OnNetworkSpawn() {
        if (!IsOwner) {
            Destroy(gameObject.GetComponentInChildren<UniversalAdditionalCameraData>());
            Destroy(gameObject.GetComponentInChildren<Camera>());
            Destroy(gameObject.GetComponentInChildren<Gun>());
        }
        if(!IsOwner) Destroy(this);
    }
    void Awake() {
    }
    // Start is called before the first frame update
    void Start()
    {
        Gravity = 9.8f;
        JumpHeight = 3f;
        Speed = 4f;
        Sprint = 2f;
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        float sprintFactor = Input.GetKey(KeyCode.LeftShift) ? Sprint : 1;
        velocity = new Vector3(0, velocity.y, 0);

        velocity += input.y * Speed * sprintFactor * transform.forward;  
        velocity += input.x * Speed * sprintFactor * transform.right;  
        if (!controller.isGrounded) velocity += -Gravity * Time.deltaTime * transform.up;
        else {
            velocity.y = -1f;
            JumpLogic();
        }
        controller.Move(velocity * Time.deltaTime);

    }

    private void JumpLogic() {
        Debug.Log(JumpHeight);
        if (Input.GetKeyDown(KeyCode.Space)) velocity.y = Mathf.Sqrt(JumpHeight * 2.0f * Gravity);
    }
}
