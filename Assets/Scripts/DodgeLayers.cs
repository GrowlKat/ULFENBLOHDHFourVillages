using System;
using UnityEngine;

//[Serializable]
/*namespace UnityEngine
{
    public class DodgeLayers
    {
        public DodgingLayer layer;
        public enum DodgingLayer
        {
            Up = 0,
            Down = 1,
            Both = 2
        }
    }
}*/

[Obsolete]
public class DodgeLayers : MonoBehaviour
{
    public int layer = 0;
}
