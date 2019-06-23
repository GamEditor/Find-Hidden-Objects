using System;
using UnityEngine;

[Serializable]
public class LevelInfo
{
    public string BackgroundSprite;     // background sprite
    public string[] PictureNames;    // the name in resources folder (make a new HiddenObject and set the source sprite by using the name)
    public Vector2[] Scales;         // size of each sprite in the scene
    public Vector3[] Positions;     // positions on the screen
}