using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomMath
{

    static Vector4 one = new Vector4(1, 0, 0, 0);
    static Vector4 two = new Vector4(0, 1, 0, 0);
    static Vector4 three = new Vector4(0, 0, 1, 0);
    static Vector4 four = new Vector4(0, 0, 0, 1);
    private static float InverseSqr(float x)
    {
        float xhalf = 0.5f * x;
        int i = BitConverter.ToInt32(BitConverter.GetBytes(x), 0);
        i = 0x5f3759df - (i >> 1);
        x = BitConverter.ToSingle(BitConverter.GetBytes(i), 0);
        x = x * (1.5f - xhalf * x * x);
        return x;
    }

    internal static float Average(List<int> values)
    {
        float sum = 0;
        float count = 0;
        foreach (var n in values)
        {
            sum += n;
            count++;
        }
        if (count == 0)
        {
            return 0;
        }
        return sum / count;
    }

    internal static Vector3 Vector3NotNew(float x, float y, float z)
    {
        return Vector3.right * x + Vector3.up * y + Vector3.forward * z;
    }

    public static Vector3 XZNormalize(Vector3 vec)
    {
        //Debug.Log("initial "+vec);
        //Debug.Log("normaliz "+vec.normalized);
        if (vec.x == 0 && 0 == vec.z)
            return Vector3.zero;
        float isqr = InverseSqr(vec.x * vec.x + vec.z * vec.z);



        return (Vector3.right * vec.x * isqr + Vector3.forward * vec.z * isqr);
    }

    internal static float SqrDistance2(float x, float z)
    {
        return x * x + z * z;
    }

    public static Vector3 Normalize(Vector3 vec)
    {
        //Debug.Log("initial "+vec);
        //Debug.Log("normaliz "+vec.normalized);
        if (vec == Vector3.zero)
            return Vector3.zero;
        float isqr = InverseSqr(vec.x * vec.x + vec.y * vec.y + vec.z * vec.z);



        return (vec * isqr);
    }

    internal static int CountDigits(int i)
    {
        int count = 0;
        do
        {
            i = i / 10;
            ++count;
        } while (i != 0);
        return count;
    }

    public static Color Vector4NotNew(float v1, float v2, float v3, float v4)
    {
        return one * v1 + two * v2 + three * v3 + four * v4;
    }
}
