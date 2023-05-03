using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PrefabSpreader))]
public class PrefabSpreaderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        PrefabSpreader prefabSpreader = (PrefabSpreader)target;

        if (GUILayout.Button("Update Seed and Regenerate Positions"))
        {
            prefabSpreader.seed = Random.Range(int.MinValue, int.MaxValue);
            prefabSpreader.GeneratePositions(prefabSpreader.seed);
            SceneView.RepaintAll();
        }
    }
}