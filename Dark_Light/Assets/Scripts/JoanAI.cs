using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoanAI : BaseAI
{
    private float stopSpeed = 0.1f;

    private float minDistChasing = 2f;

    public override IEnumerator RunAI()
    {
        while (true)
        {
            switch (CheckPlayerState())
            {
                case PlayerState.CHASING:
                    if (GetAgentRemainingDistance() < minDistChasing)
                    {
                        SetAgentVelocity(Vector3.zero);
                    }

                    if (CheckIfEnemiesAreStronger())
                    {
                        SetPlayerState(PlayerState.ESCAPING);
                        SetDestination(GetEscapeDestination());
                    }
                    break;

                case PlayerState.ESCAPING:
                    if (GetAgentVelocity().magnitude < stopSpeed)
                    {
                        SetHasDestination(false);
                    }

                    if (CheckIfEnemiesNear())
                    {
                        if (CheckIfEnemiesAreStronger())
                        {
                            SetDestination(GetEscapeDestination());
                        }
                        else
                        {
                            SetPlayerState(PlayerState.CHASING);
                            SetDestination(GetChasePosition());
                        }
                        
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
                        if (GetAgentVelocity().magnitude < stopSpeed)
                        {
                            SetHasDestination(false);
                        }
                    }
                    break;

                case PlayerState.INLIGHT:
                    if (CheckIfEnemiesAreStronger())
                    {
                        SetPlayerState(PlayerState.ESCAPING);
                        SetDestination(GetEscapeDestination());
                    }
                    else
                    {
                        SetDestination(GetChasePosition());
                    }
                    
                    break;
            }

            yield return null;
        }
    }
}
