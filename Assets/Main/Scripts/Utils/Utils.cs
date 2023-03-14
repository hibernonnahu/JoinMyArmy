
using System;

using UnityEngine;


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

    internal static Vector3 Normalize(Vector3 vector3)
    {
        return vector3.normalized;
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
    public static Color GetColor(float r, float g, float b, float a)
    {
        return r * vr + g * vg + b * vb + va * a;
    }
    
}
