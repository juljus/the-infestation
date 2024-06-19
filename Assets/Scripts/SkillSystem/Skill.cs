using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : ScriptableObject
{
    public new string name;
    public string skillDescription;
    public Sprite skillIcon;
    public string id;
    
    public float cooldownTime;
    public float activeTime;
    public bool isPassive;

    public virtual void Activate(GameObject player) {}

    public virtual void Deactivate(GameObject player) {}
}
