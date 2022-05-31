using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayMenu : MonoBehaviour
{
    public GameObject playMenuPanel;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        playMenuPanel.SetActive(false);
    }
    public void SetActive()
    {
        playMenuPanel.SetActive(!playMenuPanel.activeSelf);
    }
}
