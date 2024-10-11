
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Utils
{
    internal static long GetTimeNow()
    {
        return DateTimeOffset.Now.ToUnixTimeMilliseconds();
    }
    internal static float GetTimeWeek()
    {
        return 604800000f;
    }
    internal static float GetTimeMinute()
    {
        return 60000f;
    }


    internal static string ParseTimeToString(int v)
    {
        string ms = (v % 100).ToString("00");
        v /= 100;
        string s = (v % 60).ToString("00");
        int m = v / 60;
        return m + ":" + s + ":" + ms;
    }

    internal static void AdapteToResolution(RectTransform rectTransform, Transform transform, RectTransform canvasParent, bool onlyWidth = false, bool adjustPivot = true)
    {

        Transform[] childs = new Transform[transform.childCount];
        for (int i = 0; i < childs.Length; i++)
        {
            childs[i] = transform.GetChild(i);
        }

        var container = new GameObject().AddComponent<RectTransform>();
        container.transform.SetParent(rectTransform.transform);
        float scaleFactor = (transform.parent.GetComponent<RectTransform>().sizeDelta.x / 1080);
        container.sizeDelta = Vector2.right * 1080 + Vector2.up * 1920;
        container.anchoredPosition = Vector2.zero;
        container.transform.localScale = Vector3.one;
        container.gameObject.name = "container";
        for (int i = 0; i < childs.Length; i++)
        {
            childs[i].SetParent(container.transform);
        }
        float scale = canvasParent.rect.width / rectTransform.rect.width;
#if UNITY_STANDALONE || UNITY_WEBGL
        if (!onlyWidth)
            scale = 1;
#endif
        if (onlyWidth)
        {
            container.transform.localScale = Vector3.right * scale + Vector3.up + Vector3.forward;
        }
        else
        {
            container.transform.localScale = Vector3.one * scale;
        }
        if (adjustPivot)
            container.pivot = Vector2.right * (0.5f / scaleFactor) + Vector2.up * 0.5f;
        container.anchoredPosition = Vector2.zero;

    }

    public static Vector3 StearingVector(Vector3 position, Vector3 normalizedDirection, int mask)
    {
        Vector3 offset = Vector3.left * normalizedDirection.z + Vector3.forward * normalizedDirection.x;


        float dl = 3, dr = 3;
        RaycastHit hit;
        if (Physics.Raycast(position - offset, normalizedDirection, out hit, 3, mask))
        {
            dl = hit.distance;
        }
        if (Physics.Raycast(position + offset, normalizedDirection, out hit, 3, mask))
        {
            dr = hit.distance;
        }

        Debug.DrawRay(position + offset, CustomMath.XZNormalize(normalizedDirection) * dr, Color.green, 1);
        Debug.DrawRay(position - offset, CustomMath.XZNormalize(normalizedDirection) * dl, Color.green, 1);


        return offset * (dr - dl);
    }

    internal static int GetBinaryCount(int v)
    {
        int count = 0;
        for (int i = 0; i < 32; i++)
        {
            if (((1 << i) & v) != 0)
            {

                count++;
            }

        }

        return count;
    }

    internal static bool CanBeRecluited(Character.EnemyType canRecluit, Character.EnemyType enemyType)
    {
        return canRecluit == enemyType;
    }

    internal static int AddBinaryNumber(int insert, int into)
    {
        if (insert <= 0)
        {
            return into;
        }
        return into | 1 << (insert - 1);
    }
    internal static bool IsBinaryNumber(int code, int toCheck)
    {
        if (toCheck <= 0)
        {
            return false;
        }
        code = (code & 1 << (toCheck - 1));
        return code != 0;
    }
    static Vector4 vr = new Vector4(1, 00, 0, 0);
    static Vector4 vg = new Vector4(0, 1, 0, 0);
    static Vector4 vb = new Vector4(0, 00, 1, 0);
    static Vector4 va = new Vector4(0, 00, 0, 1);

    public static float ScreenWidth = 1080;
    public static float ScreenHeight = 1920;

    public static Color GetColor(float r, float g, float b, float a)
    {
        return r * vr + g * vg + b * vb + va * a;
    }

    public static int CreateSecret(List<string> list)
    {
        int secret = 762636;
        int len = 0;
        foreach (var str in list)
        {
            var lowStr = str.ToLower();

            var charArray = System.Text.Encoding.ASCII.GetBytes(lowStr);
            foreach (var item in charArray)
            {
                len++;
                // Debug.Log(item);
                secret += item;
            }

        }
        secret = secret % 100000;
        //Debug.Log("secret " + secret);
        //Debug.Log("list.Count " + list.Count);
        //Debug.Log("len " + len);

        return (secret * list.Count * len);
    }

    internal static Image CreateRedDot(Transform transform, Image redDot)
    {
        GameObject NewObj = new GameObject(); //Create the GameObject
        NewObj.name = "redDot";
        redDot = NewObj.AddComponent<Image>();
        redDot.color = Color.red;
        var rect = redDot.GetComponent<RectTransform>();
        rect.SetParent(transform);
        redDot.sprite = Resources.Load<Sprite>("Texture/circle");
        rect.anchoredPosition = Vector2.one * 50;
        rect.localScale = Vector2.one * 0.5f;
        return redDot;
    }

    internal static Vector4 GetCharacterTextColor(string v)
    {
        switch (v)
        {
            case "RedHairGirl":
                return CustomMath.Vector4NotNew(1f, 0.3f, 0f, 1);
            case "CharacterMain0":
                return CustomMath.Vector4NotNew(0.64f, 1f, 0.72f, 1);
            case "Succubus":
                return CustomMath.Vector4NotNew(0.75f, 0.0f, 1f, 1);
            case "LibraryGirl":
                return CustomMath.Vector4NotNew(0.85f, 3.0f, 1f, 1);
            default:
                return CustomMath.Vector4NotNew(0.7f, 0.7f, 0.4f, 1);
        }
    }
}
