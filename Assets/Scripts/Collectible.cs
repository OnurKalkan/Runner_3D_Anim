using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    public CollectibleType cType;
    GameObject player;
    public bool collected, magneting;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public enum CollectibleType
    {
        Star,
        DoubleXP,
        Shield,
        Coin,
        Magnet
    }

    private void Update()
    {
        if (cType == CollectibleType.Coin && player.GetComponent<Player>().isMagnetActive == true && collected == false)
        {
            //if (Vector3.Distance(player.transform.position, transform.position) < 8)
            //{
            //    StartCoroutine(ToParent());
            //}
            if ((transform.position.z - player.transform.position.z) < 2)
            {
                magneting = true;
                StartCoroutine(ToParent());
            }
        }
    }

    public IEnumerator ToParent()
    {
        collected = true;
        transform.parent = player.transform.parent;
        transform.DOLocalMove(Vector3.zero + new Vector3(0, 1, 0), 0.2f).SetEase(Ease.Linear);
        yield return new WaitForSeconds(0.0f);
    }
}
