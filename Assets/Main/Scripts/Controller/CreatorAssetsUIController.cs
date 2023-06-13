using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreatorAssetsUIController : MonoBehaviour
{
    public RectTransform enemies;
    public RectTransform obstacles;
    public Button buttonEnemies;
    public Button buttonObstacles;
    public Button buttonSave;
    public Button buttonLoad;

    // Start is called before the first frame update


    private void Start()
    {
        enemies.gameObject.SetActive(false);
        obstacles.gameObject.SetActive(false);
    }

    public void OnObstaclesButton()
    {
        enemies.gameObject.SetActive(false);
        obstacles.gameObject.SetActive(!obstacles.gameObject.activeSelf);
        buttonSave.gameObject.SetActive(!obstacles.gameObject.activeSelf);
        buttonLoad.gameObject.SetActive(!obstacles.gameObject.activeSelf);

    }
    public void OnEnemiesButton()
    {
        obstacles.gameObject.SetActive(false);
        enemies.gameObject.SetActive(!enemies.gameObject.activeSelf);
        buttonSave.gameObject.SetActive(!enemies.gameObject.activeSelf);
        buttonLoad.gameObject.SetActive(!enemies.gameObject.activeSelf);
    }
}
