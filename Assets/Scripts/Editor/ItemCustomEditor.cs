using UnityEditor;

//Making a custom editor for the ItemSO class so we can display it differently when selected
[CustomEditor(typeof(ItemSO), true)]
public class ItemCustomEditor : Editor
{
    public override void OnInspectorGUI()
    {
        //save the value of the target (what is selected) into a value so we can use it later.
        var value = (ItemSO)target;
        
        //Makes it so we can't edit what is below this
        EditorGUI.BeginDisabledGroup(true);
        EditorGUILayout.Space();
        //Add in a label to show the ID value,
        //without it being editable because of above line. 
        EditorGUILayout.LabelField("ID", value.ItemId.ToString());
        
        EditorGUILayout.Space();
        // ReEnables editing of values
        EditorGUI.EndDisabledGroup();
        
        //normal inspector values
        base.OnInspectorGUI();
    }

 
    // Feel free to remove the comments, just thought it would help explain things a little :3
}
