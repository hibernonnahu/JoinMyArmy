using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BulletPool : MonoBehaviour
{
    [SerializeField]
    private Bullet[] bullets;
    public float scale = 1;
    private int currentIndex = 0;

    private void Start()
    {
        foreach (var item in bullets)
        {
            item.transform.localScale = Vector3.one * scale;
            item.gameObject.SetActive(false);
            item.transform.SetParent(null);
        }
    }
    public Bullet GetBullet(Character character)
    {
        currentIndex = (currentIndex + 1) % bullets.Length;
        bullets[currentIndex].character = character;
        bullets[currentIndex].gameObject.SetActive(true);
        return bullets[currentIndex];
    }

    internal void SetLayer(int layer)
    {
        foreach (var item in bullets)
        {
            item.gameObject.layer = layer;
        }
    }
}
