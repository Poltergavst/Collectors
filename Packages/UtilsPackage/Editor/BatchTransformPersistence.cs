using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public static class BatchTransformPersistence
{
    private static List<Snapshot> _snapshots = new List<Snapshot>();

    private struct Snapshot
    {
        public string path;
        public string json;
    }

    [MenuItem("Tools/Batch Transform/Copy (Selected + Children)")]
    private static void Copy()
    {
        _snapshots.Clear();
        var selected = Selection.gameObjects;
        if (selected.Length == 0)
        {
            Debug.LogWarning("BatchTransform: Select at least one GameObject.");
            return;
        }

        foreach (var root in selected)
        {
            foreach (var t in root.GetComponentsInChildren<Transform>(true))
            {
                _snapshots.Add(new Snapshot
                {
                    path = GetPath(t),
                    json = EditorJsonUtility.ToJson(t)
                });
            }
        }

        Debug.Log($"BatchTransform: Copied {_snapshots.Count} transform(s) " +
                  $"from {selected.Length} root object(s) + children");
    }

    [MenuItem("Tools/Batch Transform/Paste (Restore by Path)")]
    private static void Paste()
    {
        if (_snapshots.Count == 0)
        {
            Debug.LogWarning("BatchTransform: Nothing to paste. " +
                             "Run Copy first while in Play Mode.");
            return;
        }

        int restored = 0;
        foreach (var snap in _snapshots)
        {
            var go = GameObject.Find(snap.path);
            if (go == null) continue;

            EditorJsonUtility.FromJsonOverwrite(snap.json, go.transform);
            EditorUtility.SetDirty(go.transform);
            restored++;
        }

        Debug.Log($"BatchTransform: Restored {restored}/{_snapshots.Count} transform(s)");
    }

    private static string GetPath(Transform t)
    {
        if (t.parent == null) return t.name;
        return GetPath(t.parent) + "/" + t.name;
    }
}
