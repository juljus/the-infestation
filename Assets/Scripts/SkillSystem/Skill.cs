using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : ScriptableObject
{
    // editor variables
    public new string name;
    public string skillDescription;
    public Sprite skillIcon;
    public string id;
    
    public float cooldownTime;
    public float activeTime;
    public bool isPassive;
    public float castRange;


    public virtual void Activate(GameObject player, SkillHelper skillHelper = null) {}

    public virtual void Deactivate(GameObject player) {}
}
