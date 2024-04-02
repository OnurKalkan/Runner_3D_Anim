using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    public CollectibleType cType;

    public enum CollectibleType
    {
        Star,
        DoubleXP,
        Shield,
        Coin,
        Magnet
    }
}