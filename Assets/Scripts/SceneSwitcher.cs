using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    public GameObject player;

    #region Singleton
    public static SceneSwitcher instance = null;
    void Awake()
    {
        DontDestroyOnLoad(this);

        if (instance == null) instance = this;
        else if (instance == this)
        {
            Debug.LogWarning("Multiple SceneSwitchers!");
            Destroy(gameObject);
        }
    }
    #endregion

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }


    public void StartCombat()
    {
        SceneManager.LoadScene("CombatScene");
    }

    public static void EndCombat()
    {
        SceneManager.LoadScene("SampleScene");

    }
}
