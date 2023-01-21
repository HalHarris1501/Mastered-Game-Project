using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Rigidbody2D rigidBody;
    public Vector2 movement;

    public Weapon currentWeapon;
    [SerializeField] private WeaponParent weaponParent;
    [SerializeField] private float startTimeBetweenAttack;
    [SerializeField] private float timeBetweenAttack;

    private WeaponStruct _currentWeapon;
    private GameObject collectableNearby;

    #region Singleton

    public static Player Instance;

    private void Awake()
    {
        Instance = this;
    }

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        weaponParent = GetComponentInChildren<WeaponParent>();
        if(weaponParent is null)
        {
            Debug.Log("Weapon Parent is missing");
            gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        if (!currentWeapon.isAttacking)
        {
            weaponParent.FaceMouse();
        }
    }

    public void SetWeapon(GameObject newWeapon)
    {
        if (currentWeapon.gameObject == newWeapon) return;
        currentWeapon.gameObject.SetActive(false);
        currentWeapon = newWeapon.GetComponent<Weapon>();
        currentWeapon.gameObject.SetActive(true);
        if (weaponParent != null)
        {
            weaponParent.SetNewWeapon(newWeapon.GetComponent<SpriteRenderer>());
        }
    }

    private void GetInput()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if(collectableNearby != null)
        {
            if(Input.GetKeyDown(KeyCode.F))
            {
                PickupCollectable();
            }
        }

        if (timeBetweenAttack <= 0)
        {
            if (Input.GetMouseButton(0))
            {
                currentWeapon.Attack(0);

                timeBetweenAttack = startTimeBetweenAttack;
            }
            else if (Input.GetMouseButton(1))
            {
                currentWeapon.Attack(1);

                timeBetweenAttack = startTimeBetweenAttack;
            }
        }
        else if (!currentWeapon.isAttacking)
        {
            timeBetweenAttack -= Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        rigidBody.MovePosition(rigidBody.position + movement.normalized * moveSpeed * Time.fixedDeltaTime);
    }

    public void SetStartingWeapon(WeaponType weaponType)
    {
        _currentWeapon = new WeaponStruct();
        _currentWeapon.Type = weaponType;
        _currentWeapon.WeaponObject = WeaponsLocker.Instance.GetWeaponObject(_currentWeapon.Type);
        WeaponManager.Instance.AddWeaponToInventory(_currentWeapon.Type, _currentWeapon.WeaponObject);
        SetWeapon(_currentWeapon.WeaponObject);  
    }

    public void PickupCollectable()
    {
        if(collectableNearby.GetComponent<ICollectable<WeaponType>>() != null)
        {
            WeaponStruct newWeapon = new WeaponStruct();
            newWeapon.Type = collectableNearby.GetComponent<ICollectable<WeaponType>>().Pickup();
            newWeapon.WeaponObject = WeaponsLocker.Instance.GetWeaponObject(newWeapon.Type);
            WeaponManager.Instance.AddWeaponToInventory(newWeapon.Type, newWeapon.WeaponObject);
        }
        else if(collectableNearby.GetComponent<ICollectable<PotionEnum>>() != null)
        {
            PotionStruct newPotion = new PotionStruct();
            newPotion.PotionType = collectableNearby.GetComponent<ICollectable<PotionEnum>>().Pickup();
            newPotion.Potion = PotionLocker.Instance.GetPotionObject(newPotion.PotionType);
            PotionManager.Instance.AddPotionToInventory(newPotion.PotionType, newPotion.Potion);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Collectable"))
        {
            WeaponCollectable weaponCollectable = collision.GetComponent<WeaponCollectable>();
            PotionCollectable potionCollectable = collision.GetComponent<PotionCollectable>();
            if (weaponCollectable != null)
            {
                collectableNearby = weaponCollectable.gameObject;                
            }
            else if (potionCollectable != null)
            {
                collectableNearby = potionCollectable.gameObject;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Collectable"))
        {
            WeaponCollectable weaponCollectable = collision.GetComponent<WeaponCollectable>();
            PotionCollectable potionCollectable = collision.GetComponent<PotionCollectable>();
            if (weaponCollectable != null)
            {
                collectableNearby = null;                
            }
            else if(potionCollectable != null)
            {
                collectableNearby = null;
            }
        }
    }

    public void SetTimeBetweenAttack(float timeToSet)
    {
        timeBetweenAttack = timeToSet;
    }
}
