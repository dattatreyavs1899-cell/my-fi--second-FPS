using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerCombat : MonoBehaviour
{
    [Header("References")]
    public Camera playerCamera;
    public InputActionReference shootAction;

    [Header("UI")]
    public TextMeshProUGUI ammoText;

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