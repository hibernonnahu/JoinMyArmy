using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectTimeOffController : MonoBehaviour
{
    private const float COINS_MULTIPLIER = 0.7f;
    public RectTransform totalCoinsText;
    public RectTransform totalCoinsGO;
    public Text coinsTempText;
    public RectTransform coinsTempContainer;
    private MiniPopup miniPopup;
    int coinsTemp;
    private static bool show = false;
    public void Init()
    {
        if (!show)
        {
            show = true;
            string minString = SaveData.GetInstance().GetMetric(SaveDataKey.MINUTES_SINCE_LAST_CONEXION, "");
            float minutesSinceLastConexion = 0;
            if (minString != "")
            {
                minutesSinceLastConexion = float.Parse(minString);
                minutesSinceLastConexion /= 60;
            }
            if (minutesSinceLastConexion > 60)//on hour
            {
                if (minutesSinceLastConexion > 1440)//day
                {
                    minutesSinceLastConexion = 1440;

                }
                coinsTemp = (int)(minutesSinceLastConexion * SaveData.GetInstance().GetValue(SaveDataKey.CURRENT_BOOK_CHAPTER + 1, CurrentPlaySingleton.GetInstance().GetInitialChapter(1)));
                coinsTemp = (int)(coinsTemp * COINS_MULTIPLIER);
                miniPopup = GameObject.FindObjectOfType<MiniPopup>();
                miniPopup.text.text = "Welcome back! have " + coinsTemp.ToString() + " coins!";
                miniPopup.SetImage("ads");

                miniPopup.SetAcceptAction(()=> { AdsController.instance.adHandler.RewardAd(AcceptReward, ()=> { miniPopup.OnCancel(); }); });
                miniPopup.Open();
            }
        }
    }

    private void AcceptReward()
    {
        string levelKey = SaveDataKey.REWARD_TIME_AWAY_AD + "_" + CurrentPlaySingleton.GetInstance().GameType() + "_" + CurrentPlaySingleton.GetInstance().book + "_" + CurrentPlaySingleton.GetInstance().chapter + "_" + CurrentPlaySingleton.GetInstance().level;
        SaveData.GetInstance().Save(levelKey, SaveData.GetInstance().GetValue(levelKey, 0) + 1);

        coinsTempContainer.gameObject.transform.position = miniPopup.transform.position;

        CurrentPlaySingleton.GetInstance().coins = coinsTemp;
        SaveData.GetInstance().SaveRam();
        var sql = GameObject.FindObjectOfType<SQLManager>();
        if (sql != null)
            sql.SaveUser();
        coinsTempText.text = (coinsTemp).ToString();
        coinsTempContainer.gameObject.SetActive(true);
        AnimateCoinsToBag(2);
    }
    private void AnimateCoinsToBag(float scale = 1)
    {
        LeanTween.scale(coinsTempContainer.gameObject, Vector3.one * 1.5f * scale, 0.3f).setDelay(0.5f).setEaseLinear().setOnComplete(
       () =>
       {
           LeanTween.scale(coinsTempContainer.gameObject, Vector3.one, 0.3f).setDelay(0.5f).setEaseOutBounce();
       }
       );
        LeanTween.move(coinsTempContainer.gameObject, Vector3.right * Screen.width * 0.5f + Vector3.up * Screen.height * 0.5f, 0.9f).setEaseOutCirc();
        LeanTween.moveY(coinsTempContainer.gameObject, totalCoinsGO.transform.position.y, 0.9f).setDelay(1).setEaseInOutCirc().setOnComplete(UpdateTotalCoins);
        LeanTween.moveX(coinsTempContainer.gameObject, totalCoinsGO.transform.position.x, 0.9f).setDelay(1).setEaseInCirc();
    }
    void UpdateTotalCoins()
    {
        EventManager.TriggerEvent(EventName.PLAY_FX, EventManager.Instance.GetEventData().SetString("coins"));

        EventManager.TriggerEvent(EventName.UPDATE_COINS_TEXT, EventManager.Instance.GetEventData().SetInt(0));
        coinsTempContainer.gameObject.SetActive(false);
        LeanTween.scale(totalCoinsText, Vector3.one * 1.5f, 0.3f).setEaseLinear().setOnComplete(
           () =>
           {
               LeanTween.scale(totalCoinsText, Vector3.one, 0.7f).setEaseOutBounce();
           }
           );
    }
}
