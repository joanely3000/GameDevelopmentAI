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
                    if (CheckIfEnemiesNear())
                    {
                        if (!CheckHasDestination())
                        {
                            SetEscapeDestination();
                        }
                        else
                        {
                            GoToDestination();
                        }
                    }
                    else
                    {
                        SetPlayerState(PlayerState.MOVING);
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
