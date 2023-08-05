
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    public GameUIController gameUIController;
    public CoinsCollectUIController coinsCollectUIController;
    public Image fadeImage;
    public float time;
    public float waveTime;

    public CharacterManager CharacterManager { get { return characterManager; } }
    private CharacterManager characterManager;

    private StateMachine<StateGame> stateMachine;

    public ToRecluitManager toRecluitManager;
    // Start is called before the first frame update
    void Start()
    {
        toRecluitManager = gameObject.AddComponent<ToRecluitManager>();
        var loader = GetComponent<LevelJsonLoader>();
        loader.book = CurrentPlaySingleton.GetInstance().book;
        loader.chapter = CurrentPlaySingleton.GetInstance().chapter;
        loader.level = CurrentPlaySingleton.GetInstance().level;
        characterManager = GetComponent<CharacterManager>();
        characterManager.Init();
        stateMachine = new StateMachine<StateGame>();
        stateMachine.AddState(new StateWaitGame(stateMachine, this));
        stateMachine.AddState(new StateInGame(stateMachine, this));

        //#if UNITY_ANDROID
        //        AndroidJavaClass unityPlayerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        //        AndroidJavaObject activity = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity");
        //        AndroidJavaObject window = activity.Call<AndroidJavaObject>("getWindow");

        //        int flags = 0x00080000 | 0x4000000; // Fullscreen and Immersive flags
        //        window.Call("setFlags", flags, flags);
        //#endif
    }



    // Update is called once per frame
    void Update()
    {
      
        stateMachine.Update();
    }
    internal void OnExitTrigger(Vector3 exitPosition)
    {
        var characterMain = FindObjectOfType<CharacterMain>();
        stateMachine.AddState(new StateEndGame(stateMachine, this, characterMain));
        stateMachine.AddState(new StateGameChapterFinish(stateMachine, this, true, "chapter complete"));
        stateMachine.ChangeState(typeof(StateEndGame));
        characterMain.OnLevelEnd(exitPosition);
    }
    public void OnDead()
    {
        coinsCollectUIController.gameObject.SetActive(true);
        SaveData.GetInstance().SaveRam(false);
        EventManager.TriggerEvent(EventName.MAIN_TEXT, EventManager.Instance.GetEventData().SetString("You died!"));

        stateMachine.AddState(new StateGameChapterFinish(stateMachine, this, false, "lose", 2));
        stateMachine.ChangeState(typeof(StateGameChapterFinish));
    }
    private void MainMenuScene()
    {
        SceneManager.LoadScene("Main Menu");
    }
    public void Reset()
    {
        //CurrentPlaySingleton.GetInstance().ResetStats();
        CurrentPlaySingleton.GetInstance().AddSkill<SkillExtraDamage>(typeof(SkillExtraDamage));
        CurrentPlaySingleton.GetInstance().AddSkill<SkillExtraDefense>(typeof(SkillExtraDefense));
        CurrentPlaySingleton.GetInstance().RefillHP();
        var mm = FindObjectOfType<MusicManager>();
        if (mm != null)
        {
            mm.PlayDefault();
        }
        Time.timeScale = 1;
        SceneManager.LoadScene("Game");

    }
    public void ResetAndMain()
    {
        CurrentPlaySingleton.GetInstance().Reset();
        MainMenuScene();

    }
    public void Continue()
    {
        LeanTween.color(fadeImage.rectTransform, Color.black, 2.5f).setOnComplete(MainMenuScene);
        EventManager.TriggerEvent(EventName.POPUP_CLOSE_ALL);
    }
    private void OnDestroy()
    {
        EventManager.Instance.ClearAll();
    }
}
