using System.Collections; using System.Collections.Generic; using UnityEngine;  public class BaseAI {     public PlayerController Player = null;      // Event     public IEnumerator Ahead(float distance)     {         yield return Player.__Ahead(distance);     }      public IEnumerator Back(float distance)     {         yield return Player.__Back(distance);     }      public IEnumerator Left(float distance)     {         yield return Player.__Left(distance);     }

    public IEnumerator Right(float distance)     {         yield return Player.__Right(distance);     }      public IEnumerator TurnLeft(float angle)     {         yield return Player.__TurnLeft(angle);     }      public IEnumerator TurnRight(float angle)     {         yield return Player.__TurnRight(angle);     }      public virtual IEnumerator RunAI()
    {
        yield return null;
    } }