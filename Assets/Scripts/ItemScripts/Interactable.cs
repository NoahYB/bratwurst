using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class Interactable : MonoBehaviour
{
    [SerializeField] private string textOnHover = "e";
    [SerializeField] private bool holdable = true;
    private TextMeshPro textObject;
    private bool inView = false;
    private PlayerController interactedController;
    private bool held = false;
    private void Start() {
        textObject = GetComponentInChildren<TextMeshPro>();
        gameObject.layer = LayerMask.NameToLayer("Interactable");
        textObject.enabled = false;
        textObject.text = textOnHover;
    }
    private void Update() {
        if(!inView) textObject.enabled = false;
        if(textObject.gameObject == null) return;
        textObject.gameObject.transform.LookAt(Camera.main.transform);
        textObject.gameObject.transform.Rotate(Vector3.up - new Vector3(0,180,0));
        if (Input.GetKeyDown(textOnHover) && inView) {
            Interact(interactedController);
        }
        if (Input.GetKeyDown(KeyCode.X) && held) Drop();
        inView = false;
    }
    public void DisplayOnHover(PlayerController playerController) {
        if (held) return;
        interactedController = playerController;
        textObject.enabled = true;
        inView = true;
    }
    private void Interact(PlayerController playerController) {
        if (holdable) {
            if (gameObject.GetComponent<Rigidbody>() != null) Destroy(gameObject.GetComponent<Rigidbody>());
            gameObject.transform.position = playerController.GetHoldingHand().position;
            gameObject.transform.parent = playerController.GetHoldingHand();
            gameObject.transform.LookAt(Camera.main.transform.forward * -1000f);
            gameObject.transform.position += Camera.main.transform.forward * .5f;
            held = true;
        }
    }
    private void Drop() {
        gameObject.transform.parent = null;
        gameObject.AddComponent<Rigidbody>();
        held = false;
    }

}
