using System.Collections; using System.Collections.Generic; using UnityEngine;  public class BaseAI {     public PlayerController Player = null;      // Event      #region Coroutines     public IEnumerator Ahead(float distance)     {         yield return Player.__Ahead(distance);     }      public IEnumerator Back(float distance)     {         yield return Player.__Back(distance);     }      public IEnumerator Left(float distance)     {         yield return Player.__Left(distance);     }

    public IEnumerator Right(float distance)     {         yield return Player.__Right(distance);     }      public IEnumerator TurnLeft(float angle)     {         yield return Player.__TurnLeft(angle);     }      public IEnumerator TurnRight(float angle)     {         yield return Player.__TurnRight(angle);     }      public virtual IEnumerator RunAI()
    {
        yield return null;
    }
    #endregion

    #region Functions 
    public bool CheckHasDestination()
    {
        return Player.getHasDestination();
    }      public void SetDestination(Vector3 destination)
    {
        Player.setDestination(destination);
    } 
    public void GoToDestination()
    {
        Player.GoToDestination();
    }

    public Transform GetRandomDestination()
    {
        return Graph.Instance.returnRandomNode();
    }

    public bool CheckIfVisibleEnemiesNear()
    {
        return Player.CheckIfThereAreVisibleEnemies();
    }

    public bool CheckIfEnemiesNear()
    {
        return Player.CheckIfEnemiesNear();
    }

    public bool CheckIfEnemiesAreStronger()
    {
        return Player.AnyEnemyIsStronger();
    }

    public void SetEscapeDestination()
    {
        Debug.Log("Soy " + Player.gameObject.name + ", busco destino de escape");
        Player.SetEscapeDestination();
    }

    public PlayerState CheckPlayerState()
    {
        return Player.CheckState();
    }

    public void SetPlayerState(PlayerState state)
    {
        Player.SetPlayerState(state);
    }
    #endregion }