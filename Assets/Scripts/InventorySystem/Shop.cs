using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    [SerializeField]
    private GameObject shoppingPanel;
    public Sprite imageOff;
    public Sprite imageOn;
    public bool canShop;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && canShop)
        {
            StartShopping();
        }
    }

    private void StartShopping()
    {
        shoppingPanel.SetActive(true);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            GetComponent<SpriteRenderer>().sprite = imageOn;
            canShop = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            GetComponent<SpriteRenderer>().sprite = imageOff;
            canShop = false;
        }
    }
}
 