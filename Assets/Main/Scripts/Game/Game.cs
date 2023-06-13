
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    public GameUIController gameUIController;
    public Image fadeImage;
    public float time;
    public float waveTime;
   
    public CharacterManager CharacterManager { get { return characterManager; } }
    private CharacterManager characterManager;

    private StateMachine<StateGame> stateMachine;

    // Start is called before the first frame update
    void Start()
    {
       
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
        var stats = CurrentPlaySingleton.GetInstance();
        stats.level++;
        var next = Resources.LoadAll<TextAsset>("Maps/Campaign/Book" + stats.book + "/Chapter" + stats.chapter + "/Level" + stats.level);
        if (next == null || next.Length == 0)
        {
            SaveData.GetInstance().SaveRam();
            if (stats.chapter < 3)
            {
                stats.chapter++;
                int currentChapter= SaveData.GetInstance().GetValue(SaveDataKey.CURRENT_CHAPTER,1);
                if (stats.chapter > currentChapter)
                {
                    CurrentPlaySingleton.GetInstance().animateTransition = true;
                    SaveData.GetInstance().Save(SaveDataKey.CURRENT_CHAPTER, stats.chapter);
                }
            }
            else
            {
                stats.chapter = 1;
            }
            stats.SaveGamePlay(characterMain);
            stats.Reset();
            EventManager.TriggerEvent(EventName.POPUP_OPEN, EventManager.Instance.GetEventData().SetString(PopupName.WIN));
        }
        else
        {
            foreach (var item in FindObjectsOfType<RecluitIconController>())
            {
                item.ForceKnocked();
            }
           
            stats.SaveGamePlay(characterMain);
            
            LeanTween.color(fadeImage.rectTransform, Color.black, 2.5f).setOnComplete(NextLevelScene);
        }
        characterMain.OnLevelEnd(exitPosition);
    }
    private void NextLevelScene()
    {
        SceneManager.LoadScene("Game");
    }
    private void MainMenuScene()
    {
        SceneManager.LoadScene("Main Menu");
    }
    public void Reset()
    {
        CurrentPlaySingleton.GetInstance().Reset();
        Continue();
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
