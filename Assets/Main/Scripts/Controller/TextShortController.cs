using System;
using TMPro;
using UnityEngine;

public class TextShortController : MonoBehaviour
{
    private const float X_RANDOM_OFFSET = 0.7F;
    private const float TEXT_TIME = 1.7F;
    private const float TEXT_START_Y = 6.5F;
    private const float TEXT_END_Y = 11F;
    public TextMeshPro[] text;
    private int currentIndex = 0;
    private void Awake()
    {
        text = GetComponentsInChildren<TextMeshPro>();
       
    }
    public void SetDialog(Vector3 position, string say, Color color = default(Color))
    {
        TextMeshPro text = GetCurrentText();
        text.transform.position = position + Vector3.up * TEXT_START_Y;
        text.text = say;
        text.color = color;
        text.transform.position += UnityEngine.Random.Range(-X_RANDOM_OFFSET, X_RANDOM_OFFSET) * Vector3.right;
        LeanTween.moveY(text.gameObject, TEXT_END_Y, TEXT_TIME).setEaseOutCubic().setOnComplete(() => { text.text = ""; });
    }

    private TextMeshPro GetCurrentText()
    {
        currentIndex = (currentIndex + 1) % text.Length;
        return text[currentIndex];
    }
    private void OnDestroy()
    {
        for (int i = 0; i < text.Length; i++)
        {
            LeanTween.cancel(text[i].gameObject);
        }
    }
}