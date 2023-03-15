using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHolder : MonoBehaviour, IObserver<WeaponType>
{
    public BaseWeapon currentWeapon;
    public State state;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Animator animator;

    private void Start()
    {
        WeaponManager.Instance.RegisterObserver(this);
    }

    public enum State
    {
        Attacking,
        Ready
    }

    public void SetVariables(BaseWeapon newWeapon)
    {
        currentWeapon = newWeapon;
    }

    public void Attack(int attackType, bool isCrit)
    {
        animator.SetTrigger("Attack");
        currentWeapon.Attack(this.gameObject, attackType, isCrit);
    }

    public void NewItemAdded(WeaponType type)
    {
        //throw new System.NotImplementedException();
    }

    public void ItemRemoved(WeaponType type)
    {
        //throw new System.NotImplementedException();
    }

    public void ItemCountAltered(WeaponType type, int count)
    {
        if (type != currentWeapon.weaponData.weaponType) return;

        if(count > 0)
        {
            spriteRenderer.enabled = true;
        }
        else
        {
            spriteRenderer.enabled = false;
        }
    }
}
