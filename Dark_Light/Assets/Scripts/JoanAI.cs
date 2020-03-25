using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoanAI : BaseAI
{
    public override IEnumerator RunAI()
    {
        while (true)
        {
            switch (CheckPlayerState())
            {
                case PlayerState.CHASING:
                    break;

                case PlayerState.ESCAPING:
                    Debug.Log("Estoy escapando");
                    if (CheckIfEnemiesNear())
                    {
                        SetEscapeDestination();
                        GoToDestination();
                    }
                    else
                    {
                        SetPlayerState(PlayerState.MOVING);
                        SetDestination(GetRandomDestination().position);
                    }
                    break;

                case PlayerState.MOVING:
                    if (!CheckHasDestination())
                    {
                        SetDestination(GetRandomDestination().position);
                    }
                    else
                    {
                        GoToDestination();
                    }
                    break;

                case PlayerState.INLIGHT:
                    Debug.Log("Estoy en la luuus");
                    SetPlayerState(PlayerState.ESCAPING);
                    SetEscapeDestination();
                    break;
            }

            yield return null;

            /*
           yield return Ahead(2);
           yield return TurnLeft(180);
           yield return Left(4);
           yield return TurnRight(90);*/
        }
    }
}
