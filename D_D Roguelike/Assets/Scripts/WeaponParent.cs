using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponParent : MonoBehaviour
{
    [SerializeField] private SpriteRenderer characterRenderer, weaponRenderer;
    public RangedObjectPooler rangedObjectPooler;

    private void Start()
    {
        Initialise();
    }

    private void Initialise()
    {
        rangedObjectPooler = GetComponentInChildren<RangedObjectPooler>();
        if (rangedObjectPooler is null)
        {
            Debug.Log("Ranged Object Pooler is missing");
            gameObject.SetActive(false);
        }
    }

    public void FaceMouse()
    {        
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector2 direction = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y).normalized;

        transform.right = direction;

        //Vector2 scale = transform.localScale;
        //if (direction.x < 0)
        //{
        //    scale.y = -1;
        //}
        //else if(direction.x > 0)
        //{
        //    scale.y = 1;
        //}
        //transform.localScale = scale;
        
        if (transform.eulerAngles.z > 0 && transform.eulerAngles.z < 180) //makes the weapon sprite go to a lower layer when held above the player's head 
        {
            weaponRenderer.sortingOrder = characterRenderer.sortingOrder - 1;
        }
        else
        {
            weaponRenderer.sortingOrder = characterRenderer.sortingOrder + 1;
        }        
    }

    public void SetNewWeapon(SpriteRenderer newWeaponSprite, ProjectileDataPack currentProjectile)
    {
        weaponRenderer = newWeaponSprite;
        if(!(currentProjectile is null)) rangedObjectPooler.SetDataPack(currentProjectile);
    }

    private void OnDrawGizmos()
    {
        float spread = 0.3f;
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector2 direction = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y).normalized;
        //calculate spread for projectile
        //float angle = Mathf.Atan2(mousePosition.y, mousePosition.x) * Mathf.Rad2Deg;
        //transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        //Vector2 dir = transform.rotation * Vector2.up;
        direction = direction * 10f;
        Vector2 pdir = Vector2.Perpendicular(direction);

        Vector2 minDirection = direction + (pdir * -spread);
        Vector2 maxDirection = direction + (pdir * spread);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, (new Vector2( transform.position.x, transform.position.y) + minDirection));
        Gizmos.DrawLine(transform.position, (new Vector2(transform.position.x, transform.position.y) + maxDirection));        
    }
}
