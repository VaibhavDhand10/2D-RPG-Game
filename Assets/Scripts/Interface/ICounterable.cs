using UnityEngine;

public interface ICounterable
{
    public void HandleCounter();
    public bool CanBeCountered { get;}
}