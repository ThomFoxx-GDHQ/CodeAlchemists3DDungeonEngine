using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PartyManager))]
public class PartyManagerEditor: Editor
{
    private bool _showStats = false;
    private bool _showExtraInfo = false;
    
    
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        _showExtraInfo = EditorGUILayout.Toggle("Show party information", _showExtraInfo);
        
        if (!_showExtraInfo) return;

        var manger = (PartyManager)target;

        var row = manger.Party.GetLength(0);
        var cols =manger.Party.GetLength(1);

        for (var i = 0; i < row; i++)
        {
            for (var j = 0; j < cols; j++)
            {
                GUILayout.Space(20);
                if (manger.Party[i,j] == null)
                {
                    EditorGUILayout.LabelField("Empty");
                }
                else
                {
                    Element("Player Name:",manger.Party[i, j].Name);
                    _showStats = EditorGUILayout.BeginFoldoutHeaderGroup(_showStats, _showStats ? "Hide Stats" : "Show Stats");
                    if (_showStats)
                    {
                        Element("Race Type:",manger.Party[i, j].Race);
                        Element("Job Type:",manger.Party[i, j].JobType);
                        Element("Health Points:",manger.Party[i, j].HealthPoints);
                        Element("Magic Points:",manger.Party[i, j].MagicPoints);
                        Element("Strength:",manger.Party[i, j].Strength);
                        Element("Agility:",manger.Party[i, j].Agility);
                        Element("Constitution:",manger.Party[i, j].Constitution);
                        Element("Fortitude:",manger.Party[i, j].Fortitude);
                        Element("Wisdom:",manger.Party[i, j].Wisdom);
                        Element("Current Health:",manger.Party[i, j].CurrentHealth);
                        Element("Current Magic:",manger.Party[i, j].CurrentMagic);
                    }
                    EditorGUILayout.EndFoldoutHeaderGroup();
                }
                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            }
        }
        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }

    private static void Element(string field,object value)
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField($"{field}");
        EditorGUILayout.LabelField($"{value}");
        EditorGUILayout.EndHorizontal();
    }
}
