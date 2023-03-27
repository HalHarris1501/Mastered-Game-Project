using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PotionUI : MonoBehaviour, IObserver<PotionEnum>
{
    [SerializeField] private RectTransform _potionContainer; //The container that holds all the weapon buttons and has a grid layout for automatically ordering children
    [SerializeField] private Button _potionButtonTemplate; //template button that will be instantiate for each weapon in player's inventory

    private Dictionary<PotionEnum, Button> _buttons = new Dictionary<PotionEnum, Button>();

    // Start is called before the first frame update
    void Start()
    {
        //register as an observer of the weaponManager
        PotionManager.Instance.RegisterObserver(this);
        //deactivate the template button
        _potionButtonTemplate.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void NewItemAdded(PotionEnum potionType)
    {
        //create new button for the weapon
        Button newButton = Instantiate(_potionButtonTemplate, _potionContainer);
        newButton.name = potionType.ToString() + " button";
        //get icon from the weapons locker which is a singleton
        newButton.GetComponent<PotionButton>().SetIcon(potionType);
        //create an event for when the button is clicked
        newButton.onClick.AddListener(() => OnPotionSelected(potionType));
        newButton.gameObject.SetActive(true);
        _buttons.Add(potionType, newButton);
    }

    public void ItemRemoved(PotionEnum potionType)
    {
        if (_buttons.ContainsKey(potionType))
        {
            GameObject tempButton = _buttons[potionType].gameObject;
            _buttons.Remove(potionType);
            Destroy(tempButton);
        }
    }

    //called when player clicks on the weapon button in the UI
    void OnPotionSelected(PotionEnum potionType)
    {
        PotionManager.Instance.UsePotion(potionType);
    }

    public void ItemAltered(PotionEnum type, int count)
    {
        //throw new System.NotImplementedException();
    }
}
