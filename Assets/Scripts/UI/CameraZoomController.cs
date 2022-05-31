using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoomController : MonoBehaviour
{
    private Camera cam;
    private float targetZoom;
    private float zoomFactor = 3f;
    [SerializeField]
    private float zoomLerpSpeed = 10;
    [SerializeField]
    private float combatZoom = 1.5f;



    #region Singleton
    public static CameraZoomController instance = null;

    void Awake()
    {
        if (instance == null) instance = this;
        else if (instance == this)
        {
            Debug.LogWarning("Multiple CameraZoomControllers!");
            Destroy(gameObject);
        }
    }
    #endregion

    private void Start()
    {
        cam = Camera.main;
        targetZoom = cam.orthographicSize;
    }

    private void Update()
    {

        float scrollData = Input.GetAxis("Mouse ScrollWheel");

        targetZoom -= scrollData * zoomFactor;
        targetZoom = Mathf.Clamp(targetZoom, 2f, 8f);

        if (CombatManager.IsCombatActive()) targetZoom = combatZoom;

        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetZoom, Time.deltaTime * zoomLerpSpeed);
            
    }   

    public void CombatZoomIn()
    {
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, combatZoom, Time.deltaTime * zoomLerpSpeed);
    }

}
