using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CombatState { START, PLAYERTURN, ENEMYTURN, WON, LOST, NONE }

public class CombatManager : MonoBehaviour
{
    private CameraZoomController zoomController;
    public GameObject combatArena;
    public GameObject player;
    public GameObject enemy;
    public HealthBar playerHealthBar;
    public HealthBar enemyHealthBar;

    internal void RegisterHit()
    {
        switch (state)
        {
            case CombatState.PLAYERTURN:
                enemyCombatUnit.PlayDamagedAnim();
                enemyHealthBar.SetHealth(enemyCombatUnit.currentHp);
                DamagePopup.Create(enemyCombatUnit.transform.position, playerCombatUnit.damage);
                break;
            case CombatState.ENEMYTURN:
                playerCombatUnit.PlayDamagedAnim();
                playerHealthBar.SetHealth(playerCombatUnit.currentHp);
                DamagePopup.Create(playerCombatUnit.transform.position, enemyCombatUnit.damage);
                break;
        }
    }

    internal void CombatUnitIsDead()
    {
        switch (state)
        {
            case CombatState.PLAYERTURN:
                if (enemyCombatUnit.IsDead()) {
                    enemyCombatUnit.PlayDamagedFallAnim();
                }
                break;
            case CombatState.ENEMYTURN:
                if (playerCombatUnit.IsDead()) { 
                    playerCombatUnit.PlayDamagedFallAnim();
                }
                break;
        }
    }

    

    private CombatUnit playerCombatUnit;
    private CombatUnit enemyCombatUnit;
    public Transform playerPos;
    public Transform enemyPos;
    public CombatState state;
    public GameObject shade;
    private bool enemyHasAttacked = true;
    private Vector3 playerStartWorldPosition;
    private Vector3 enemyStartWorldPosition;

    #region Singleton
    public static CombatManager instance = null;
    void Awake()
    {
        if (instance == null) instance = this;
        else
        {
            Debug.LogWarning("Multiple CombatManagers!");
            Destroy(gameObject);
        }
    }
    #endregion

    private void Start()
    {
        zoomController = CameraZoomController.instance;
    }

    public static bool IsCombatActive()
    {
        return !instance.state.Equals(CombatState.NONE);
    }


    private void Update()
    {
        switch (state)
        {
            case CombatState.PLAYERTURN:
                break;
            case CombatState.ENEMYTURN:
                if (!enemyHasAttacked && !enemyCombatUnit.IsDead())
                {
                    CombatUnitAttack(enemyCombatUnit, playerCombatUnit);
                    enemyHasAttacked = true;
                }
                else if (enemyCombatUnit.IsDead())
                {
                    state = CombatState.WON;
                }
                break;
        }  
    }

    public void StartCombat()
    {
        state = CombatState.START;
        SetupCombat();
        GameManager.instance.isCombatActive = true;
    }

    private void SetupCombat()
    {
        playerStartWorldPosition = player.transform.position;
        enemyStartWorldPosition = enemy.transform.position;

        playerCombatUnit = player.GetComponent<CombatUnit>();
        enemyCombatUnit = enemy.GetComponent<CombatUnit>();

        playerHealthBar.SetHealth(playerCombatUnit.currentHp); 
        playerHealthBar.SetMaxHealth(playerCombatUnit.maxHp);
        enemyHealthBar.SetHealth(enemyCombatUnit.currentHp);
        enemyHealthBar.SetMaxHealth(enemyCombatUnit.maxHp);

        combatArena.SetActive(true);
        combatArena.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, 1);
        shade.transform.localScale = new Vector3(Screen.width, Screen.height);

        player.transform.SetParent(playerPos, false);
        enemy.transform.SetParent(enemyPos, false);
        player.transform.localPosition = enemy.transform.localPosition = new Vector3(0, 0, 1);

        player.GetComponent<CapsuleCollider2D>().enabled = player.GetComponent<BoxCollider2D>().enabled =
        enemy.GetComponent<CapsuleCollider2D>().enabled = enemy.GetComponent<BoxCollider2D>().enabled = false;

        player.GetComponentInChildren<BodyPartDisplay>().ChangeSortingLayerTo("CombatArena");
        Debug.Log(enemy);
        enemy.GetComponentInChildren<BodyPartDisplay>().ChangeSortingLayerTo("CombatArena");
        enemy.GetComponentInChildren<BodyPartDisplay>().FlipBodyPartsByX();
        GameManager.instance.SetCombatUI(true);

        playerCombatUnit.PlayCombatIdleAnim(true);
        enemyCombatUnit.PlayCombatIdleAnim(true);

        state = CombatState.PLAYERTURN;   
    }

    public void SetNextState()
    {
        if (state.Equals(CombatState.ENEMYTURN))
            if (playerCombatUnit.IsDead())
                LoseCombat();
            else
                state = CombatState.PLAYERTURN;
        else if (state.Equals(CombatState.PLAYERTURN))
            if (enemyCombatUnit.IsDead())
                WinCombat();
            else
                state = CombatState.ENEMYTURN;
    }
    public void Attack()
    {
        if (state.Equals(CombatState.PLAYERTURN) && enemyHasAttacked)
        {
            CombatUnitAttack(playerCombatUnit, enemyCombatUnit);
            enemyHasAttacked = false;
        }
    }

    private void CombatUnitAttack(CombatUnit attacker, CombatUnit target)
    {
        attacker.Attack(target);
        target.currentHp -= attacker.damage;
    }

    public void EndCombat()
    {
        player.transform.parent = enemy.transform.parent = null;
        combatArena.SetActive(false);
        state = CombatState.NONE;
        shade.transform.localScale = new Vector3(0, 0);
        GameManager.instance.SetCombatUI(false);
        player.GetComponentInChildren<BodyPartDisplay>().ChangeSortingLayerTo("Canal");
        enemy.GetComponentInChildren<BodyPartDisplay>().ChangeSortingLayerTo("Canal");
        player.transform.position = playerStartWorldPosition;
        enemy.transform.position = enemyStartWorldPosition;
        playerCombatUnit.PlayCombatIdleAnim(false);
        enemyCombatUnit.PlayCombatIdleAnim(false);
        player.GetComponent<CapsuleCollider2D>().enabled = player.GetComponent<BoxCollider2D>().enabled =
            enemy.GetComponent<CapsuleCollider2D>().enabled = enemy.GetComponent<BoxCollider2D>().enabled = true;
    }


    private void LoseCombat()
    {
        Debug.Log("Combat lost");
        EndCombat();
    }

    private void WinCombat()
    {
        Debug.Log("Combat won");
        EndCombat();
    }
}
