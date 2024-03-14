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

    [SerializeField] GameObject armature;
    [SerializeField] GameObject fullModel;
    [SerializeField] private Transform rightHand;

    public LayerMask interactableLayer;


    public Dictionary<string, bool> animationStates = new Dictionary <string,bool> ();
    public override void OnNetworkSpawn() {
        if (!IsOwner) {
            Destroy(gameObject.GetComponentInChildren<UniversalAdditionalCameraData>());
            Destroy(gameObject.GetComponentInChildren<Camera>());
            int invisLayer = LayerMask.NameToLayer("Invisible");
            SetLayerAllChildren(armature.transform, invisLayer);
            int layer = LayerMask.NameToLayer("Default");
            SetLayerAllChildren(fullModel.transform, layer);
        }
    }

    void Awake() {
        InitializeAnimationStates();
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
        if(!IsOwner) return;
        SeeObject();
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
        SetAnimationState(input, sprintFactor);
    }

    private void JumpLogic() {
        if (Input.GetKeyDown(KeyCode.Space)) velocity.y = Mathf.Sqrt(JumpHeight * 2.0f * Gravity);
    }

    private void SeeObject() {
        Ray sightRay = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        if( Physics.Raycast(sightRay, out RaycastHit hitInfo, 10.0f, interactableLayer)){
            if (hitInfo.collider.gameObject.GetComponentInChildren<Interactable>() != null) {
                hitInfo.collider.gameObject.GetComponentInChildren<Interactable>().DisplayOnHover(this);
            }
        }
    }

    private void SetAnimationState(Vector3 input, float sprintFactor) {
        if (Input.GetMouseButton(0)) {
            animationStates["Shooting"] = true;
        } else animationStates["Shooting"] = false;
        if (sprintFactor != 1) animationStates["Running"] = true;
        else animationStates["Running"] = false;
        if (input.y != 0)  animationStates["Walking"] = true;
        else animationStates["Walking"] = false;
    }
    void SetLayerAllChildren(Transform root, int layer)
    {
        var children = root.GetComponentsInChildren<Transform>(includeInactive: true);
        foreach (var child in children)
        {
//            Debug.Log(child.name);
            child.gameObject.layer = layer;
        }
    }
    void InitializeAnimationStates() {
        animationStates["Running"] = false;
        animationStates["Idle"] = true;
        animationStates["Shooting"] = false;
        // animationStates["Dance"] = false;
    }
    public Transform GetHoldingHand() {
        return rightHand;
    }
}
