using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShopKeeper : MonoBehaviour
{
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private GameObject shopUI, inventoryUI;
    [SerializeField] private GameObject speechBubble;
    [SerializeField] private TMP_Text speechText;
    private bool interactable = false;
    private float speechBubbleTimer = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ManageUI();
        CheckBubbleOpen();
    }

    private void ManageUI()
    {
        if(!interactable)
        {
            shopUI.SetActive(false);
            inventoryUI.SetActive(false);
            return;
        }
        else
        {
            SetSpeechBubbleTimer();
            CheckForShopOpen();
        }        
    }

    private void CheckForShopOpen()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (shopUI.activeInHierarchy == false)
            {
                shopUI.SetActive(true);
            }
        }
    }

    private void SetSpeechBubbleTimer()
    {
        speechBubbleTimer = 5.0f;
    }

    private void CheckBubbleOpen()
    {
        if (speechBubbleTimer <= 0)
        {
            speechBubble.SetActive(false);
            return;
        }
        speechBubble.SetActive(true);
        speechBubbleTimer -= Time.deltaTime;
    }

    private void SetSpeechBubbleText(string text)
    {
        speechText.text = text;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(playerLayer == (playerLayer | (1 << collision.gameObject.layer)))
        {
            interactable = true;
            SetSpeechBubbleText("Greetings Traveller, do my wares interest you?");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (playerLayer == (playerLayer | (1 << collision.gameObject.layer)))
        {
            interactable = false;
            SetSpeechBubbleText("Thanks for your patronage!");
        }
    }
}
