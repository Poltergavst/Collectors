#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class ShowGlobalTransform
{
    [MenuItem("CONTEXT/Transform/Show Global Position")]
    static void ShowGlobalPosition(MenuCommand command)
    {
        Transform t = (Transform)command.context;
        Debug.Log($"[{t.name}] Global Position: {t.position}, Global Rotation: {t.eulerAngles}, Global Scale: {t.lossyScale}");
    }

#endif
}
