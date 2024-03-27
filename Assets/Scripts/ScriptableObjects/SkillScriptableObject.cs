using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Skill", menuName = "ScriptableObjects/Skill")]
public class SkillScriptableObject : ScriptableObject
{
    public string skillName;
    public string skillDescription;
    public Sprite skillIcon;
}
