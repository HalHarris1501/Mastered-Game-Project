using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Rigidbody2D rigidBody;
    public Vector2 movement;

    public WeaponHolder playerWeapon;
    [SerializeField] private WeaponParent weaponParent;
    [SerializeField] private int critMinRoll = 20;
    [SerializeField] private float startTimeBetweenAttack;
    [SerializeField] private float timeBetweenAttack;
    [SerializeField] private CreatureAttributes playerAttributes;

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
        InitialisePlayer();
    }

    private void InitialisePlayer()
    {
        weaponParent = GetComponentInChildren<WeaponParent>();
        if (weaponParent is null)
        {
            Debug.Log("Weapon Parent is missing");
            gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        if (playerWeapon.state != WeaponHolder.State.Attacking)
        {
            weaponParent.FaceMouse();
        }
    }

    public void SetWeapon(BaseWeapon newWeapon)
    {
        playerWeapon.SetVariables(newWeapon);
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
                AttackRoll(0);

                timeBetweenAttack = startTimeBetweenAttack;
            }
            else if (Input.GetMouseButton(1))
            {
                AttackRoll(1);

                timeBetweenAttack = startTimeBetweenAttack;
            }
        }
        else if (playerWeapon.state != WeaponHolder.State.Attacking)
        {
            timeBetweenAttack -= Time.deltaTime;
        }
    }

    private void AttackRoll(int attackType)
    {
        bool isCritical = false;

        int roll = DiceRoller.D20Check();
        if (roll >= critMinRoll) isCritical = true;

        playerWeapon.Attack(attackType, isCritical);
    }

    private void FixedUpdate()
    {
        rigidBody.MovePosition(rigidBody.position + movement.normalized * moveSpeed * Time.fixedDeltaTime);
    }

    public void SetStartingWeapon(WeaponType weaponType)
    {
        Debug.Log("setting starting weapon");
        _currentWeapon = new WeaponStruct();
        _currentWeapon.Type = weaponType;
        _currentWeapon.weaponObject = WeaponsLocker.Instance.GetWeaponObject(_currentWeapon.Type);
        WeaponManager.Instance.AddWeaponToInventory(_currentWeapon.Type, _currentWeapon.weaponObject);
        SetWeapon(_currentWeapon.weaponObject);  
    }

    public void PickupCollectable()
    {
        if(collectableNearby.GetComponent<ICollectable<WeaponType>>() != null)
        {
            WeaponStruct newWeapon = new WeaponStruct();
            newWeapon.Type = collectableNearby.GetComponent<ICollectable<WeaponType>>().Pickup();
            newWeapon.weaponObject = WeaponsLocker.Instance.GetWeaponObject(newWeapon.Type);
            WeaponManager.Instance.AddWeaponToInventory(newWeapon.Type, newWeapon.weaponObject);
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
            ICollectable<WeaponType> weaponCollectable = collision.GetComponent<ICollectable<WeaponType>>();
            ICollectable<PotionEnum> potionCollectable = collision.GetComponent<ICollectable<PotionEnum>>();
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

    private void OnDrawGizmos()
    {
        if (playerWeapon.currentWeapon != null)
        {            
            Matrix4x4 rotationMatrix = Matrix4x4.TRS(transform.position, weaponParent.transform.rotation, transform.localScale);
            Gizmos.matrix = rotationMatrix;
            Gizmos.color = Color.red;
            Vector2 inverseBox = new Vector2(playerWeapon.currentWeapon.damageBox.y, playerWeapon.currentWeapon.damageBox.x);
            Gizmos.DrawWireCube(new Vector3(0.455f, 0.02f, 0f), inverseBox);
        }
    }
}
