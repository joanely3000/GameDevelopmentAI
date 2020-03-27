using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JesusAI : BaseAI
{
    private float stopSpeed = 0.1f;

    private float minDistChasing = 2f;

    public override IEnumerator RunAI()
    {
        SetFocus();
        while (true)
        {
            switch (CheckPlayerState())
            {
                case PlayerState.CHASING: //Done
                    Debug.Log("CHASING");
                    if (GetAgentRemainingDistance() < minDistChasing)
                    {
                        SetAgentVelocity(Vector3.zero);
                    }

                    if (ChaseTargetExists())
                    {
                        SetDestination(GetFocusPositionIfNear());
                    }
                    else
                    {
                        SetPlayerState(PlayerState.MOVING);
                        SetDestination(GetRandomDestination());
                    }
                    break;

                case PlayerState.ESCAPING: //Done
                    if (GetAgentVelocity().magnitude < stopSpeed)
                    {
                        SetHasDestination(false);
                    }

                    if (CheckIfFocusNear())
                    {
                        SetPlayerState(PlayerState.CHASING);
                        Debug.Log(GetFocusPositionIfNear());
                        SetDestination(GetFocusPositionIfNear());           
                    }
                    else
                    {
                        if (CheckIfEnemiesNear())
                        {
                            SetDestination(GetEscapeDestination());
                        }
                        else
                        {
                            SetPlayerState(PlayerState.MOVING);
                            SetDestination(GetRandomDestination());
                        }
                    }
                    break;

                case PlayerState.MOVING:

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
                    break;

                case PlayerState.INLIGHT: //Done
                    if (CheckIfEnemyIsMyFocus())
                    {
                        SetPlayerState(PlayerState.CHASING);
                        SetDestination(GetFocusPositionIfNear());
                    }
                    else
                    {
                        SetPlayerState(PlayerState.ESCAPING);
                        SetDestination(GetEscapeDestination());
                    }             
                    break;
            }

            yield return null;
        }
    }
}
