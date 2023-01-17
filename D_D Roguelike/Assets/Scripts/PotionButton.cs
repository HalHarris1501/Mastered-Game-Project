using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PotionButton : MonoBehaviour
{
    [SerializeField] private Image potionImage;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetIcon(PotionEnum potionType)
    {
        potionImage.sprite = PotionLocker.Instance.GetPotionIcon(potionType);
    }
}
