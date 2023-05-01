using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomList<T> : IEnumerable<T>
{
    public Node<T> first;
    public Node<T> last;
    int count = 0;

    public T this[int index]
    {
        get
        {
            int count = 0;
            var current = first;
            while (count != index && current != null)
            {
                current = current.next;
                count++;
            }
            if (current == null) { return default(T); }
            return current.Element;
        }
        set
        {
            int count = 0;
            var current = first;
            while (count != index)
            {
                current = current.next;
                count++;
            }
            current.Element = value;
        }
    }
    public int Count
    {
        get
        {
            return count;
        }

    }
    public void Add(T element)
    {
        var node = new Node<T>(element);
        if (last == null)
        {
            first = last = node;
        }
        else
        {
            last.next = node;
            last = node;
        }
        count++;
    }
    public void AddInPosition(T element, int position)
    {
        if (position >= count)//
        {
            Add(element);
        }
        else
        {
            int currentPosition = 0;
            var node = new Node<T>(element);
            {
                Node<T> current = first;
                Node<T> b4 = null;
                while (currentPosition <= position)
                {
                    if (currentPosition == position)
                    {
                        if (b4 != null)
                        {
                            node.next = b4.next;
                            b4.next = node;
                        }
                        else
                        {
                            node.next = first;
                            first = node;
                        }
                        current = node.next;
                    }
                    else
                    {
                        b4 = current;
                        current = current.next;
                    }
                    currentPosition++;
                }

            }
            count++;
        }
        if (first != null && first.next == null && last != null && !first.Equals(last))
        {
            Debug.Log("here");
        }
    }

    public IEnumerator<T> GetEnumerator()
    {
        var current = first;
        while (current != null)
        {
            yield return current.Element;
            current = current.next;
        }
    }
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
    public void IterateAndErase(Func<T, bool> condition)
    {
        Node<T> current, prev; current = prev = first;
        RecurtionIterateAndErase(condition, current, prev);
    }
    private void RecurtionIterateAndErase(Func<T, bool> condition, Node<T> current, Node<T> prev)
    {
        if (current != null)
        {
            if (condition(current.Element))//si es verdadero elimino el nodo
            {
                if (current == first)//si es el primer nodo
                {
                    current = first = prev = current.next;
                }
                else
                {
                    prev.next = current.next;
                    current = prev;
                }
                count--;
            }
            if (current != first)
            {
                prev = current;

            }
            current = current.next;
            RecurtionIterateAndErase(condition, current, prev);
        }
    }
    public void Clear()
    {
        first = last = null;
        count = 0;
    }
    public void Remove(T element)
    {
        if (first != null)
        {
            if (first.Element.Equals(element))
            {
                if (first.Equals(last))
                {
                    last = null;
                }
               
                var old = first;
                first = first.next;
               
                count--;
                
            }
            else
            {
                var current = first;
                while (current.next != null)
                {
                    if (current.next.Element.Equals(element))
                    {
                        if (element.Equals(last.Element))
                        {
                            last = current;
                        }
                        current.next = current.next.next;
                        count--;
                        break;
                    }
                    else
                    {
                        current = current.next;
                    }
                }
               
            }
        }
    }


}
