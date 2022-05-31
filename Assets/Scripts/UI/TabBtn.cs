using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Image))]
public class TabBtn : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    public TabGroup tabGroup;
    [SerializeField]
    private bool isEnabled = true;
    public Image background;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!isEnabled) return;
        tabGroup.OnTabSelected(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isEnabled) {
            Tooltip.ShowTooltip_Static("Locked!", "");
            return;
        }
        tabGroup.OnTabEnter(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isEnabled)
        {
            Tooltip.HideTooltip_Static();
            return;
        }
        tabGroup.OnTabExit(this);
    }

    public void Enable()
    {
        isEnabled = true;
    }

    public void Disable()
    {
        isEnabled = false;
    }

    void Start()
    {
        background = GetComponent<Image>();
        tabGroup.Subscribe(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
