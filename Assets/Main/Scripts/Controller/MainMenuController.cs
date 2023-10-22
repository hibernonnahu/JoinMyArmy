using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    public Image fadeImage;
    private void Start()
    {
        LeanTween.color(fadeImage.rectTransform, Color.clear, 1f);
        GetComponent<CollectTimeOffController>().Init();
    }
}

