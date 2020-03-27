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
    }     public void SetHasDestination(bool value)
    {
        Player.setHasDestination(value);
    }      public void SetDestination(Vector3 destination)
    {
        Player.setDestination(destination);
    }

    public Vector3 GetRandomDestination()
    {
        return Graph.Instance.returnRandomNode().position;
    }

    public Vector3 GetFarestPoint(Vector3 enemyPos)
    {
        return Graph.Instance.GetFarestPoint(enemyPos);
    }

    public bool CheckIfVisibleEnemiesNear()
    {
        return Player.CheckIfThereAreVisibleEnemies();
    }

    public bool CheckIfEnemiesAreStrongerNear()
    {
        return Player.AnyEnemyIsStrongerNear();
    }

    public bool CheckIfEnemiesNear()
    {
        return Player.CheckIfEnemiesNear();
    }

    public bool CheckIfEnemiesAreStronger()
    {
        return Player.AnyEnemyIsStronger();
    }

    public Vector3 GetEscapeDestination()
    {
        return Player.GetEscapeDirection();
    }

    public PlayerState CheckPlayerState()
    {
        return Player.CheckState();
    }

    public void SetPlayerState(PlayerState state)
    {
        Player.SetPlayerState(state);
    }

    public Vector3 GetChasePosition()
    {
        return Player.GetChasePosition();
    }

    public Vector3 GetAgentVelocity()
    {
        return Player.GetAgentVelocity();
    }

    public void SetAgentVelocity(Vector3 velocity)
    {
        Player.SetAgentVelocity(velocity);
    }

    public float GetAgentRemainingDistance()
    {
        return Player.GetAgentRemainingDistance();
    }

    public bool ChaseTargetExists()
    {
        return Player.ChaseTargetExists();
    }

    public Vector3 GetStrongestEnemyPosition()
    {
        return Player.GetStrongestEnemyPosition();
    }

    public void SetFocus()
    {
        Player.SetFocus();
    }
    public GameObject GetFocus()
    {
        return Player.GetFocus();
    }
    public Vector3 GetFocusPositionIfNear()
    {
        return Player.GetFocusPositionIfNear();
    }
    public bool CheckIfFocused()
    {
        return Player.CheckIfFocused();
    }
    public bool CheckIfEnemyIsMyFocus()
    {
        return Player.CheckIfEnemyIsMyFocus();
    }
    public bool CheckIfFocusNear()
    {
        return Player.CheckIfFocusNear();
    }

    #endregion }