using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GwendalAI : BaseAI
{
    private float stopSpeed = 0.1f;

    private float minDistChasing = 2f;

    public override IEnumerator RunAI()
    {
        while (true)
        {
            Debug.Log(CheckPlayerState());
            switch (CheckPlayerState())
            {
                case PlayerState.CHASING:
                    if (CheckIfEnemiesNear())
                        SetDestination(GetChasePosition());
                    else
                        SetPlayerState(PlayerState.MOVING);
                    break;
                case PlayerState.ESCAPING:
                    if (CheckIfEnemiesNear())
                    {
                        if (CheckIfEnemiesAreStrongerNear())
                        {
                            SetDestination(GetEscapeDestination());
                        }
                        else
                        {
                            SetPlayerState(PlayerState.CHASING);
                        }

                    }
                    break;
                case PlayerState.INLIGHT:
                    if (CheckIfEnemiesNear())
                    {
                        if (CheckIfEnemiesAreStrongerNear())
                        {
                            SetPlayerState(PlayerState.ESCAPING);
                        }
                        else
                        {
                            SetPlayerState(PlayerState.CHASING);
                        }
                    }
                    else
                        SetPlayerState(PlayerState.MOVING);
                    break;
                case PlayerState.MOVING:
                    if (CheckIfEnemiesNear())
                    {
                        SetPlayerState(PlayerState.CHASING);
                    }
                    else
                    {
                        if (!CheckHasDestination())
                        {
                            SetDestination(GetRandomDestination());
                        }
                        else
                        {
                            if (GetAgentVelocity().magnitude < stopSpeed)
                            {
                                SetHasDestination(false);
                            }
                        }
                    }
                    break;
            }
            yield return null;
        }
    }
}
