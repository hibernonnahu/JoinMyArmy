using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconUIControllerMain : IconUIController
{
    public int overrideIndex;
    public override int IndexPosition { get { return overrideIndex; } }
}
