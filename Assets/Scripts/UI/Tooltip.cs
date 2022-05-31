using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Tooltip : MonoBehaviour
{
    //[SerializeField]
    //public TextMeshProUGUI tooltipText;
    //public RectTransform backgroundRectTransform;

    #region Singleton
    public static Tooltip instance = null;

    private void Awake()
    {
        if (instance == null) instance = this;
        else if (instance == this)
        {
            Debug.LogWarning("Multiple Tooltips!");
            Destroy(gameObject);
        }

        rectTransform = GetComponent<RectTransform>();
        layoutElement = GetComponent<LayoutElement>();

        HideToolTip();
    }
    #endregion


    public TextMeshProUGUI headerField;
    public TextMeshProUGUI contentField;
    private LayoutElement layoutElement;
    public int characterWrapLimit;
    private RectTransform rectTransform;
    public float tooltipOffsetX;
    public float tooltipOffsetY;

    private void Update()
    {
        if (headerField == null) return;
        int headerLength = headerField.text.Length;
        int contentLength = contentField.text.Length;

        layoutElement.enabled = (headerLength > characterWrapLimit || contentLength > characterWrapLimit);
        

        Vector2 position = Input.mousePosition;

        float pivotX = position.x / Screen.width;
        float pivotY = position.y / Screen.height;

        rectTransform.pivot = new Vector2(pivotX + tooltipOffsetX, pivotY + tooltipOffsetY);
        transform.position = position;       
    }

    private void ShowToolTip(string header, string content)
    {
        headerField.text = header;
        contentField.text = content;

        gameObject.SetActive(true);
    }

    private void HideToolTip()
    {
        gameObject.SetActive(false);
    }

    public static void ShowTooltip_Static(string header, string content)
    {
        instance.ShowToolTip(header, content);
    }

    public static void HideTooltip_Static()
    {
        instance.HideToolTip();
    }

}
