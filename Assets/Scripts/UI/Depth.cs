using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Depth : MonoBehaviour
{
    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!CombatManager.IsCombatActive())
            spriteRenderer.sortingOrder = (int)Camera.main.WorldToScreenPoint(transform.position).y * -1;
        else spriteRenderer.sortingOrder = 2;
        //if (this.gameObject.tag == "Player")
        //    spriteRenderer.sortingOrder = (int)Camera.main.WorldToScreenPoint(GetComponent<BoxCollider2D>().transform.localPosition).y * -1;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (tag == "NPC") return;

        if (other.tag == "PlayerBody" && other.GetComponent<Depth>().spriteRenderer.sortingOrder < this.spriteRenderer.sortingOrder)
        {
            spriteRenderer.color = new Color(1, 1, 1, 0.3f);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            spriteRenderer.color = new Color(1, 1, 1, 1);
        }
    }
}
