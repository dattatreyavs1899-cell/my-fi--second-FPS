using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    [Header("References")]
    public Camera playerCamera;
    public InputActionReference shootAction;

    [Header("UI")]
    public TextMeshProUGUI ammoText;

    [Header("Weapon Visuals & Animation")]
    public ParticleSystem muzzleFlash;
    public Transform gunModel;
    public Transform gunSlide;

    [Header("Recoil Settings")]
    public float recoilDistance = 0.1f;
    public float slideKickbackDistance = 0.15f;
    public float recoilDuration = 0.15f;
    public float recoilRotation = 5f;

    private Vector3 originalGunPos;
    private Vector3 originalSlidePos;
    private Vector3 originalGunRot;

    [Header("Weapon Stats")]
    public float damage = 50f;
    public float range = 100f;
    public float fireRate = 0.2f;
    private float nextTimeToFire = 0f;

    [Header("Ammo System")]
    public int maxAmmo = 60;
    public int currentAmmo;

    void Start()
    {
        currentAmmo = maxAmmo;
        UpdateAmmoUI();

        if (gunModel != null)
        {
            originalGunPos = gunModel.localPosition;
            originalGunRot = gunModel.localEulerAngles;
        }
        if (gunSlide != null)
        {
            originalSlidePos = gunSlide.localPosition;
        }
    }

    void OnEnable() { shootAction.action.Enable(); }
    void OnDisable() { shootAction.action.Disable(); }

    void Update()
    {
        if (shootAction.action.WasPressedThisFrame() && Time.time >= nextTimeToFire)
        {
            if (currentAmmo > 0)
            {
                nextTimeToFire = Time.time + fireRate;
                Shoot();
            }
        }
    }

    void Shoot()
    {
        currentAmmo--;
        UpdateAmmoUI();

        if (muzzleFlash != null) muzzleFlash.Play();
        AnimateGun();

        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, range))
        {
            EnemyHealth enemy = hit.transform.GetComponent<EnemyHealth>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }
    }

    void AnimateGun()
    {
        if (gunModel != null) gunModel.DOKill(true);
        if (gunSlide != null) gunSlide.DOKill(true);

        if (gunModel != null)
        {
            gunModel.localPosition = originalGunPos;
            gunModel.localEulerAngles = originalGunRot;

            gunModel.DOLocalMoveZ(originalGunPos.z - recoilDistance, recoilDuration / 2f)
                .SetLoops(2, LoopType.Yoyo)
                .SetEase(Ease.OutQuad);

            gunModel.DOLocalRotate(new Vector3(originalGunRot.x - recoilRotation, originalGunRot.y, originalGunRot.z), recoilDuration / 2f)
                .SetLoops(2, LoopType.Yoyo)
                .SetEase(Ease.OutQuad);
        }

        if (gunSlide != null)
        {
            gunSlide.localPosition = originalSlidePos;

            gunSlide.DOLocalMoveZ(originalSlidePos.z - slideKickbackDistance, recoilDuration / 2f)
                .SetLoops(2, LoopType.Yoyo)
                .SetEase(Ease.OutQuad);
        }
    }

    public void AddAmmo(int amount)
    {
        currentAmmo += amount;
        if (currentAmmo > maxAmmo) currentAmmo = maxAmmo;
        UpdateAmmoUI();
    }

    void UpdateAmmoUI()
    {
        if (ammoText != null)
        {
            ammoText.text = currentAmmo.ToString();
        }
    }
}