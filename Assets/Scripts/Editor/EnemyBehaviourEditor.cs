using UnityEngine;
using UnityEditor;
using static UnityEditor.EditorGUILayout;

public class EnemyBehaviourEditor : Editor
{
    private EnemyBase _character = null;
    private bool _flip = false;


    private void OnEnable()
    {
        _character = (EnemyBase)target;
        _flip = _character.Flip;
    }


    public override void OnInspectorGUI()
    {
        BeginHorizontal();
        LabelField("Flip", GUILayout.MaxWidth(40.0f));
        EditorGUI.BeginChangeCheck();
        _flip = Toggle(_flip);
        if (EditorGUI.EndChangeCheck())
        {
            _character.Flip = _flip;
            EditorUtility.SetDirty(_character);
        }
        EndHorizontal();
        Space(10.0f);
        base.OnInspectorGUI();
    }
}



[CustomEditor(typeof(LineRendererAttacker)), CanEditMultipleObjects]
public class IntroSoldierEditor : EnemyBehaviourEditor { }
