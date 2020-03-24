using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoanAI : BaseAI
{
    public override IEnumerator RunAI()
    {
        while (true)
        {
            if (!CheckHasDestination())
            {
                Debug.Log("Cojo un destino");
                SetDestination(new Vector3(0f, 0f, 0f));
                yield return null;
            }
            else
            {
                Debug.Log("Voy a mi destino");
                GoToDestination();
                yield return null;
            }
             /*
            yield return Ahead(2);
            yield return TurnLeft(180);
            yield return Left(4);
            yield return TurnRight(90);*/
        }
    }
}
