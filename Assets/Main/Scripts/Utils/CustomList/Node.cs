using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node <T>
{
    T element;
    public Node<T> next;
    public T Element
    {
        get
        {
            return element;
        }
        set
        {
            element = value;
        }
    }
    public Node(T e)
    {
        element = e;
    }

}
