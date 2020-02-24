using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof (PlayerLight))]
public class PlayerLightEditor : Editor
{
    private void OnSceneGUI()
    {
        PlayerLight pl = (PlayerLight)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(pl.transform.position, Vector3.up, Vector3.forward, 360, pl.viewRadius);

        Vector3 viewAngleA = pl.DirFromAngle(-pl.viewAngle / 2, false);
        Vector3 viewAngleB = pl.DirFromAngle(pl.viewAngle / 2, false);

        Handles.DrawLine(pl.transform.position, pl.transform.position + viewAngleA * pl.viewRadius);
        Handles.DrawLine(pl.transform.position, pl.transform.position + viewAngleB * pl.viewRadius);
    }
}
