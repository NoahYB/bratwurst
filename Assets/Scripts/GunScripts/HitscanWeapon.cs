using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

[RequireComponent(typeof(Interactable))]
[RequireComponent(typeof(BoxCollider))]
public class HitscanWeapon : MonoBehaviour
{
    [SerializeField] private bool automatic;
    [SerializeField] private float fireCooldown;
    [SerializeField] private float coolDownTimer;
    [SerializeField] private bool held = false;
    [SerializeField] private Transform bulletOrigin;
    private AudioSource fireSound;
    public AnimationCurve recoilCurve;
    private PlayerController playerController;
    // Start is called before the first frame update
    void Start()
    {
        fireSound = GetComponent<AudioSource>();
    }
    public void SetPlayer(PlayerController pc) {
        playerController = pc;
        held = true;
    }
    // Update is called once per frame
    void Update()
    {
        if (!held) return;
        if (automatic) {
            if (Input.GetMouseButton(0)) {
                if (coolDownTimer <= 0)  {
                    ShootVFX();
                    Shoot();
                    coolDownTimer = fireCooldown;
                }
            }
        }
        else if (Input.GetMouseButtonDown(0)) {
            if (coolDownTimer <= 0)  {
                ShootVFX();
                Shoot();
                coolDownTimer = fireCooldown;
            }
        }
        coolDownTimer -= Time.deltaTime;
    }
    void ShootVFX() {
        fireSound.Play();
    }

    public void Shoot() {
        Ray gunRay = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        if( Physics.Raycast(gunRay, out RaycastHit hitInfo)){
            if (hitInfo.collider.gameObject.tag == "Hittable") {
            }
        }
    }

}
