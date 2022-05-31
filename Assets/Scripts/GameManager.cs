using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Singleton
    public static GameManager instance = null;
    public bool isCombatActive = false;
    public List<GameObject> enemyCombatUnits = new List<GameObject>();
    public List<GameObject> playerCombatUnits = new List<GameObject>();
    private CombatManager combatManager;
    public GameObject PlayerGO;
    [SerializeField]
    private GameObject combatUI;
    [SerializeField]
    private GameObject dialogueUI;
    [SerializeField]
    private GameObject shoppingUI;


    public GameObject pfDamagePopup;


    private void Awake()
    {
        combatManager = CombatManager.instance;

        if (instance == null) instance = this;
        else if (instance == this)
        {
            Debug.LogWarning("Multiple GameManagers!");
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void SetCombatUI(bool turnOn)
    {
        combatUI.SetActive(turnOn);
        shoppingUI.SetActive(!turnOn);
        dialogueUI.SetActive(!turnOn);
    }
    #endregion
}
