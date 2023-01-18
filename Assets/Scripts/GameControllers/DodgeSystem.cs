using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using static UnityEngine.DodgeLayers;

[Serializable]
[Obsolete]
public class DodgeSystem //: DodgeLayers
{
    public bool jumping;
    public bool rolling;
    public bool dodging;
    public bool hitted;
    public int dodgeDirection;
    //public DodgeLayers layer;
    //public DodgingLayer layer;
    [Tooltip("-1 = Down. 0 = Both. 1 = Up")]
    public int layer;

    public DodgeSystem(int layer)
    {
        jumping = false;
        rolling = false;
        dodging = false;
        dodgeDirection = 1;

        if (layer >= -1 && layer <= 1) this.layer = layer;
        else Debug.LogError("A Dodge Layer can only be from -1 to 1");
    }

    /*public DodgingLayer GetLayer(GameObject gameObject)
    {
        DodgeLayers layerObject = gameObject.GetComponent<DodgeLayers>();
        if (gameObject.layer == 8)
        {
            return layerObject.layer;
        }
        else
        {
            return DodgingLayer.Both;
        }
    }*/

    public bool CompareLayer(int incomingLayer)
    {
        if(layer == incomingLayer)
        {
            return true;
        }
        else if(layer == 0 || incomingLayer == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public IEnumerator Jump(float cooldown, GameObject gameObject, Vector3 direction)
    {
        layer = 1;
        gameObject.transform.position = gameObject.transform.position + direction;
        Debug.Log("Jumping! Layer: " + layer);
        yield return new WaitForSeconds(cooldown);
        layer = 0;
        Debug.Log("Jumping Over! Layer: " + layer);
    }

    public IEnumerator Roll(float cooldown)
    {
        layer = -1;
        Debug.Log("Rolling! Layer: " + layer);
        yield return new WaitForSeconds(cooldown);
        layer = 0;
        Debug.Log("Rolling Over! Layer: " + layer);
    }
}