using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectables : MonoBehaviour
{
    #region Singleton
    public static Collectables instance = null;
    void Awake()
    {
        if (instance == null) instance = this;
        else if (instance == this)
        {
            Debug.LogWarning("Multiple Collectables!");
            Destroy(gameObject);
        }
    }
    #endregion

    public List<GameObject> collectables = new List<GameObject>();
}
