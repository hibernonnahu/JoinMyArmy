
using Febucci.UI;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CoinsCollectUIController : MonoBehaviour
{
    public RectTransform topReference;

    public Text coinsTempText;
    public RectTransform coinsTempContainer;
    public Text coinstToDuplicateText;
    public RectTransform rewardContainer;
    public RectTransform coinstToDuplicateContainer;
    public RectTransform totalCoinsText;
    public RectTransform totalCoinsGO;
    public GameObject extraCoins;
    private bool win;
    private int coinsTemp = 0;
#if UNITY_EDITOR
    public bool forceReward = false;
#endif
    private void Awake()
    {

        rewardContainer.gameObject.SetActive(false);
        EventManager.TriggerEvent(EventName.UPDATE_COINS_TEXT);
        coinsTemp = CurrentPlaySingleton.GetInstance().coins;
        coinsTempText.text = coinsTemp.ToString();

    }
    private void Start()
    {
        totalCoinsText.gameObject.SetActive(false);
        Utils.AdapteToResolution(GetComponent<RectTransform>(), transform, GetComponentInParent<Canvas>().GetComponent<RectTransform>());

    }

    public void StartAnimation(bool win)
    {
        this.win = win;
        Vector3 pos = totalCoinsText.transform.position;
        float walletDelay = 1;
#if UNITY_EDITOR
        if ((SaveData.GetInstance().GetValue("tutorial4") != 0 && coinsTemp > 25) || forceReward)
#else
        if (SaveData.GetInstance().GetValue("tutorial4") != 0)
#endif
        {

            LeanTween.move(coinsTempContainer.gameObject, coinstToDuplicateText.transform.position, 1).setEaseInCubic().setOnComplete(InitToDuplicate);
            rewardContainer.gameObject.SetActive(true);
            Vector3 pos2 = rewardContainer.transform.position;
            rewardContainer.transform.position -= Vector3.right * Screen.width;
            LeanTween.move(rewardContainer.gameObject, pos2, 0.5f).setEaseInElastic();
            walletDelay = 1.5f;

        }
        else
        {
            //LeanTween.moveX(coinsTempContainer.gameObject,totalCoinsGO.transform.position.x-Screen.width*0.7f, 1f).setEaseOutCirc();
            AnimateCoinsToBag();
        }
        totalCoinsText.transform.position -= Vector3.right * Screen.width;
        LeanTween.move(totalCoinsText.gameObject, pos, 0.5f).setDelay(walletDelay).setEaseInOutElastic();
        totalCoinsText.gameObject.SetActive(true);

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
        LeanTween.moveX(coinsTempContainer.gameObject, totalCoinsGO.transform.position.x, 0.9f).setDelay(1).setEaseInCirc().setOnComplete(ShowUpgrades);
    }

    private void ShowMiniPopup()
    {
        var miniPopup = FindObjectOfType<MiniPopup>();
        miniPopup.text.text = "Get extra " + coinsTemp.ToString() + " coins!";
        miniPopup.SetImage("ads");
        miniPopup.SetCancelAction(MoveReward);
        miniPopup.SetAcceptAction(AcceptReward);
        miniPopup.Open();
    }
    
    public void AcceptRewardLate()
    {
        string levelKey = SaveDataKey.REWARD_AD_LATE + "_" + CurrentPlaySingleton.GetInstance().book + "_" + CurrentPlaySingleton.GetInstance().chapter + "_" + CurrentPlaySingleton.GetInstance().level;
        SaveData.GetInstance().Save(levelKey, SaveData.GetInstance().GetValue(levelKey, 0) + 1);
        AcceptReward();
    }
    private void AcceptReward()
    {
        string levelKey = SaveDataKey.REWARD_AD + "_" + CurrentPlaySingleton.GetInstance().book + "_" + CurrentPlaySingleton.GetInstance().chapter + "_" + CurrentPlaySingleton.GetInstance().level;
        SaveData.GetInstance().Save(levelKey, SaveData.GetInstance().GetValue(levelKey, 0) + 1);

        coinsTempContainer.gameObject.transform.position = coinstToDuplicateText.transform.position;
        rewardContainer.gameObject.SetActive(false);
        CurrentPlaySingleton.GetInstance().coins = coinsTemp;
        SaveData.GetInstance().SaveRam();
        coinsTempText.text = (coinsTemp * 2).ToString();
        coinsTempContainer.gameObject.SetActive(true);
        AnimateCoinsToBag(2);

    }

    void InitToDuplicate()
    {
        coinstToDuplicateText.text = coinsTempText.text;
        coinsTempContainer.gameObject.SetActive(false);
        rewardContainer.GetComponentInChildren<ButtonBehaviour>().GoWiggle(8);
        LeanTween.scale(coinstToDuplicateContainer, Vector3.one * 1.5f, 0.3f).setEaseLinear().setOnComplete(
            () =>
            {
                EventManager.TriggerEvent(EventName.PLAY_FX, EventManager.Instance.GetEventData().SetString("pop"));

                LeanTween.scale(coinstToDuplicateContainer, Vector3.one, 0.3f).setEaseOutBounce().setOnComplete(ShowMiniPopup);
            }
            );
    }
    void UpdateTotalCoinsVisual()
    {
        coinsTempContainer.gameObject.SetActive(true);

        totalCoinsText.gameObject.SetActive(true);
        LeanTween.moveX(coinsTempContainer.gameObject, totalCoinsGO.transform.position.x, 0.8f).setEaseInCirc().setEaseOutSine();

        LeanTween.moveY(coinsTempContainer.gameObject, totalCoinsGO.transform.position.y, 1.4F).setEaseInBack().setOnComplete(UpdateTotalCoins);
    }


    void UpdateTotalCoins()
    {
        EventManager.TriggerEvent(EventName.PLAY_FX, EventManager.Instance.GetEventData().SetString("coins"));

        EventManager.TriggerEvent(EventName.UPDATE_COINS_TEXT);
        coinsTempContainer.gameObject.SetActive(false);
        LeanTween.scale(totalCoinsText, Vector3.one * 1.5f, 0.3f).setEaseLinear().setOnComplete(
           () =>
           {
               LeanTween.scale(totalCoinsText, Vector3.one, 0.7f).setEaseOutBounce();
           }
           );
    }
    void MoveReward()
    {
        LeanTween.scale(totalCoinsText, Vector3.one, 0.7f).setEaseOutBounce();
        LeanTween.scale(rewardContainer.gameObject, Vector3.one * 0.6f, 1);
        LeanTween.move(rewardContainer.gameObject, topReference.transform.position, 0.4f).setEaseInCubic().setOnComplete(ShowUpgrades);
    }

    void ShowUpgrades()
    {
        if (win)
        {
            EventManager.TriggerEvent(EventName.POPUP_OPEN, EventManager.Instance.GetEventData().SetString(PopupName.WIN));
        }
        else
        {
            EventManager.TriggerEvent(EventName.POPUP_OPEN, EventManager.Instance.GetEventData().SetString(PopupName.LOSE));

        }
    }
}
