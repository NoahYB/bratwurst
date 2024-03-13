using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.VFX;

public class Gun : MonoBehaviour
{
    public bool automatic;

    public float fireCooldown;
    private float coolDownTimer;
    
    private bool held = true;
    public VisualEffect flash;
    private AudioSource fireSound;

    public float rotationVelocity;
    public float timeToRecoil;
    public RawImage hitMarkerImage;
    private float recoilTimer = 0f;
    private bool finishRot = false;
    public Transform bulletOrigin;
    
    public AnimationCurve recoilCurve;

    private PlayerController playerController;
    // Start is called before the first frame update
    void Start()
    {
        fireSound = GetComponent<AudioSource>();
        hitMarkerImage = GameObject.FindGameObjectWithTag("HitMarker").GetComponent<RawImage>();
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerController.enabled) return;
        if (automatic) {
            if (Input.GetMouseButton(0)) {
                if (coolDownTimer <= 0)  {
                    ShootVFX();
                    Shoot();
                    Recoil();
                    coolDownTimer = fireCooldown;
                }
            }
        }
        else if (Input.GetMouseButtonDown(0)) {
            if (coolDownTimer <= 0)  {
                ShootVFX();
                Shoot();
                Recoil();
                coolDownTimer = fireCooldown;
            }
        }
        if (recoilTimer > 0) {
            ApplyRecoil();
        } else if (finishRot) {
            ResetGunRotation();
        }
        coolDownTimer -= Time.deltaTime;
    }
    void Recoil() {
        recoilTimer = timeToRecoil;
        finishRot = true;

    }
    void ShootVFX() {
        flash.Play();
        fireSound.Play();
    }

    public void Shoot() {
        Ray gunRay = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        if( Physics.Raycast(gunRay, out RaycastHit hitInfo)){
            if (hitInfo.collider.gameObject.tag == "Hittable") {
                hitMarkerImage.GetComponent<FadeImage>().Flash();
            }
        }
    }

    private IEnumerator SpawnTrail(TrailRenderer trail, Vector3 destination)
    {
        float time = 0; 
        Vector3 startPosition = trail.transform.position;
        while (time < 1){
            trail.transform.position = Vector3.Lerp(startPosition, destination, time);
            time += Time.deltaTime / trail.time;
            yield return null;
        }
        trail.transform.position = destination;

        Destroy(trail.gameObject, trail.time );

    }

    void ApplyRecoil() {
        recoilTimer -= Time.deltaTime;
        // rotationVelocity = recoilTimer > (timeToRecoil/ 2) ?  Math.Abs(rotationVelocity) * -1 : Math.Abs(rotationVelocity);
        transform.Rotate(Vector3.forward, recoilCurve.Evaluate(rotationVelocity * ((timeToRecoil - recoilTimer)/timeToRecoil)));
        // Debug.Log(recoilCurve.Evaluate(timeToRecoil - recoilTimer));
    }

    void ResetGunRotation() {
        transform.localRotation = Quaternion.Euler(
            0, 85.287f, -3.65f
        );
        finishRot = false;
    }

}
