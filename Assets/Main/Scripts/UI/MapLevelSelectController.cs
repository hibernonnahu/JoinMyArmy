
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MapLevelSelectController : MonoBehaviour
{
    public enum GameType
    {
        Campaign, CastleDefense
    }
    public GameType gameTypeEnum;
    public Text levelText;

    public Button button;

    public Image image;
    public GameObject commingSoon;
    public int id;
    public int book = 1;


    // Start is called before the first frame update
    void Start()
    {
        image.sprite = Resources.Load<Sprite>("LevelThumbNail/" + id);

        levelText.text = "Chapter " + id;
        commingSoon.SetActive(id > GameConfig.GetInstance().maxChapterEnable[book - 1]);
        button.gameObject.SetActive(id <= GameConfig.GetInstance().maxChapterEnable[book - 1]);
    }

    public void OnClick()
    {
        CurrentPlaySingleton.GetInstance().book = book;
        CurrentPlaySingleton.GetInstance().chapter = id;
        CurrentPlaySingleton.GetInstance().level = 1;
        SaveData.GetInstance().SaveMetric(SaveDataKey.GAME_TYPE, gameTypeEnum.ToString());
        
        SceneManager.LoadScene("Game");
    }

    internal void UnlockAnimation(Action onComplete)
    {
        BounceEnableButton(onComplete);
    }
    private void BounceEnableButton(Action onComplete)
    {
        var rectButton = button.GetComponent<RectTransform>();
        LeanTween.cancel(rectButton);
        LeanTween.scale(rectButton, Vector3.one * 1.5f, 0.7f).setEaseInBounce().setOnComplete(
            () =>
            {
                button.interactable = true;
                LeanTween.scale(rectButton, Vector3.one, 0.7f).setEaseOutBounce().setOnComplete(onComplete);
            }
            );
    }
}
