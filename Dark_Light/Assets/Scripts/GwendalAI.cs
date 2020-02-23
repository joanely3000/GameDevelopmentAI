using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GwendalAI : BaseAI
{
    public override IEnumerator RunAI()
    {
        while (true)
        {
            yield return Ahead(10);
            yield return Back(10);
            yield return Left(1);
            yield return Right(1);
            yield return TurnRight(180);
            yield return TurnLeft(180);
        }
    }
}
