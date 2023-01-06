using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Rigidbody2D rigidBody;
    public Vector2 movement;

    [SerializeField] private Weapon currentWeapon;
    [SerializeField] private WeaponParent weaponParent;
    [SerializeField] private float startTimeBetweenAttack;
    [SerializeField] private float timeBetweenAttack;

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

    public void SetWeapon(Weapon newWeapon, SpriteRenderer newWeaponSprite)
    {
        currentWeapon = newWeapon;
        weaponParent.SetNewWeapon(newWeaponSprite);
    }

    private void GetInput()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

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
        else
        {
            timeBetweenAttack -= Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        rigidBody.MovePosition(rigidBody.position + movement.normalized * moveSpeed * Time.fixedDeltaTime);
    }
}
