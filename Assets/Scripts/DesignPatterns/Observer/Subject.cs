using System.Collections.Generic;
using UnityEngine;

public abstract class Subject : MonoBehaviour
{
    private List<Observer> observers = new List<Observer>();

    public void Attach(Observer observer)
    {
        observers.Add(observer);
    }
    public void Detach(Observer observer)
    {
        observers.Remove(observer);
    }
    public void Notify()
    {
        for(int i = observers.Count - 1; i >= 1; i--)
        {
            if (observers[i] == null)
                observers.RemoveAt(i);
            else
                observers[i].OnNotify(this);
        }
    }
}
