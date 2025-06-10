using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SceneList", menuName = "Config/Scene List")]
public class SceneList : ScriptableObject
{
    public List<string> scenes;
}