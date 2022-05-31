using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Items", menuName = "Items/Item")]
public class Item : ScriptableObject
{
    public Sprite icon;
    public string name;
    public List<Stat> stats = new List<Stat>();
}
