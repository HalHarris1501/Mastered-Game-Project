using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponParent : MonoBehaviour
{
    [SerializeField] private SpriteRenderer characterRenderer, weaponRenderer;

    private void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FaceMouse()
    {        
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector2 direction = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y).normalized;

        transform.right = direction;

        Vector2 scale = transform.localScale;
        if (direction.x < 0)
        {
            scale.y = -1;
        }
        else if(direction.x > 0)
        {
            scale.y = 1;
        }
        transform.localScale = scale;
        
        if (transform.eulerAngles.z > 0 && transform.eulerAngles.z < 180) //makes the weapon sprite go to a lower layer when held above the player's head 
        {
            weaponRenderer.sortingOrder = characterRenderer.sortingOrder - 1;
        }
        else
        {
            weaponRenderer.sortingOrder = characterRenderer.sortingOrder + 1;
        }        
    }

    public void SetNewWeapon(SpriteRenderer newWeaponSprite)
    {
        weaponRenderer = newWeaponSprite;
    }
}
