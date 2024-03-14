using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerAnimationManager : NetworkBehaviour
{
    private PlayerController playerController;
    public Animator playerAnimator;
    
    public override void OnNetworkSpawn() {
        playerController = GetComponent<PlayerController>();
    }
    // Start is called before the first frame update
    void Start()
    {
        playerController = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!IsOwner) return;
        SetAllBoolsFalse();
        if (playerController.animationStates["Running"] == true) {
            playerAnimator.SetBool("Running", true);
        } 
        if (playerController.animationStates["Walking"] == true) {
            playerAnimator.SetBool("Walking", true);
        }
        if (playerController.animationStates["Shooting"] == true) {
            playerAnimator.SetBool("Shooting", true);
        }
        if (Input.GetKeyDown(KeyCode.T)) {
            Debug.Log("Taunting");
            playerAnimator.SetBool("Taunt", true);
        }
    }

    void SetAllBoolsFalse() {
        foreach(AnimatorControllerParameter parameter in playerAnimator.parameters) {            
            playerAnimator.SetBool(parameter.name, false);            
        }
    }

    
}
