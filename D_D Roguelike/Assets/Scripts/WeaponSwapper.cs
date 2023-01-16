using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSwapper : MonoBehaviour, IObserver
{
    [SerializeField] private RectTransform _weaponContainer; //The container that holds all the weapon buttons and has a grid layout for automatically ordering children
    [SerializeField] private Button _weaponButtonTemplate; //template button that will be instantiate for each weapon in player's inventory

    // Start is called before the first frame update
    void Start()
    {
        //register as an observer of the weaponManager
        WeaponManager.Instance.RegisterObserver(this);
        //deactivate the template button
        _weaponButtonTemplate.gameObject.SetActive(false);
        Player.Instance.SetStartingWeapon(WeaponType.Dagger);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NewWeaponAdded(WeaponType weaponType)
    {
        //create new button for the weapon
        Button newButton = Instantiate(_weaponButtonTemplate, _weaponContainer);
        //get icon from the weapons locker which is a singleton
        newButton.GetComponentInChildren<Image>().sprite = WeaponsLocker.Instance.GetWeaponIcon(weaponType);
        //create an event for when the button is clicked
        newButton.onClick.AddListener(() => OnWeaponSelected(weaponType));
        newButton.gameObject.SetActive(true);
    }

    //called when player clicks on the weapon button in the UI
    void OnWeaponSelected(WeaponType weaponType)
    {
        //gets the weapon from the weapon manager
        GameObject weaponObject = WeaponManager.Instance.GetWeapon(weaponType);
        //Equips the weapon
        Player.Instance.SetWeapon(weaponObject);
        Debug.Log("bingul");
    }
}
