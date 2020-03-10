using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoanAI : BaseAI
{
    public override IEnumerator RunAI()
    {
        while (true)
        {
            yield return Ahead(5);
            yield return TurnRight(90);
            /*yield return Ahead(2);
            yield return TurnLeft(180);
            yield return Left(4);
            yield return TurnRight(90);*/
        }
    }
}
