 using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamagePopup : MonoBehaviour
{
    public static DamagePopup Create(Vector3 position, int damageAmount)
    {
        Transform damagePopupTransform = Instantiate(GameManager.instance.pfDamagePopup.transform, position, Quaternion.identity);

        DamagePopup damagePopup = damagePopupTransform.GetComponent<DamagePopup>();  
        damagePopup.Setup(damageAmount);
        return damagePopup;   
    }    
    private TextMeshPro textMesh;
    private float disappearTimer;
    private Color textColor;
    
    private void Awake () {        
        textMesh = transform.GetComponent<TextMeshPro>();   
    }    
    
    public void Setup (int damageAmount) {
        textMesh.SetText(damageAmount.ToString());
        textColor = textMesh.color;
        disappearTimer = 1f;

        Debug.Log("Popup created with text" + damageAmount.ToString());
    }

    private void Update()
    {
        float moveYSpeed = 1f;
        transform.position += new Vector3(0, moveYSpeed) * Time.deltaTime;

        disappearTimer -= Time.deltaTime;
        if (disappearTimer < 0)
        {
            float disappearSpeed = 3f;
            textColor.a -= disappearSpeed * Time.deltaTime;
            textMesh.color = textColor;
            if (textColor.a < 0)
            {
                Debug.Log("Popup destoryed");
                Destroy(gameObject); 
            }
        }
    }

}



