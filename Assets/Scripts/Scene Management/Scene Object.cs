using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SceneObjects", menuName = "Scene Object/Create Scene Object")]
public class SceneObject : ScriptableObject
{
    public string sceneName;
    public Vector3[] EnterPositions;
    public bool[] isFacingRight;
    public int zone;

}
