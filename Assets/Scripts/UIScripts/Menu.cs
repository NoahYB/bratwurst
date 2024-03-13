using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField]
    private GameObject MENU;

    public Slider FOVSlider;

    private GameObject Player;

    private bool PlayerLoaded = false;
    public void SetPlayer() {
        Player = GameObject.FindGameObjectWithTag("Player");
        PlayerLoaded = true;
    }
    void Start() {
        FOVSlider.value = Camera.main.fieldOfView;
    }
    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape) && PlayerLoaded){ 
            MENU.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Player.GetComponent<PlayerController>().enabled = false;
        }
    }

    public void CloseMenu() {
        MENU.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Player.GetComponent<PlayerController>().enabled = true;
    }

    public void UpdateFOV() {
        Camera.main.fieldOfView = FOVSlider.value;
    }
}
