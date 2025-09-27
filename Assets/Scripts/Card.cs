using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : ScriptableObject
{
    public new string name;
    public Sprite art;
    public int cost;
    public string[] type;
    public string effect = "None";
    public int[] effectType;
    public int[] effectCode;
    public int[] args;
    public string[] sargs;
}
