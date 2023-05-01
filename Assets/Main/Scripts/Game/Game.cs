
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
        characterManager.Init(2);
        stateMachine = new StateMachine<StateGame>();
        stateMachine.AddState(new StateInGame(stateMachine, this));
    }



    // Update is called once per frame
    void Update()
    {
        stateMachine.Update();
    }
    internal void OnExitTrigger()
    {
        var stats = CurrentPlaySingleton.GetInstance();
        stats.level++;
        var next = Resources.LoadAll<TextAsset>("Maps/Campaign/Book" + stats.book + "/Chapter" + stats.chapter + "/Level" + stats.level);
        if (next == null || next.Length == 0)
        {
            SaveData.GetInstance().SaveRam();
            EventManager.TriggerEvent(EventName.POPUP_OPEN, EventManager.Instance.GetEventData().SetString(PopupName.WIN));
        }
        else
        {
            foreach (var item in FindObjectsOfType<RecluitIconController>())
            {
                item.ForceKnocked();
            }
            var characterMain = FindObjectOfType<CharacterMain>();
            stats.SaveGamePlay(characterMain);
            characterMain.OnLevelEnd();
            LeanTween.color(fadeImage.rectTransform, Color.black, 2.5f).setOnComplete(ChangeScene);
        }
    }
    private void ChangeScene()
    {
        SceneManager.LoadScene("Game");
    }
    public void Reset()
    {
        CurrentPlaySingleton.GetInstance().Reset();
        LeanTween.color(fadeImage.rectTransform, Color.black, 2.5f).setOnComplete(ChangeScene);
        EventManager.TriggerEvent(EventName.POPUP_CLOSE_ALL);
    }
    private void OnDestroy()
    {
        EventManager.Instance.ClearAll();
    }
}
