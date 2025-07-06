using UnityEditor;
using UnityEditor.Compilation;
using UnityEngine;

[InitializeOnLoad]
public static class ScriptRecompileDetector
{
    
    static ScriptRecompileDetector()
    {
        CompilationPipeline.compilationFinished -= OnCompilationFinished;
        CompilationPipeline.compilationFinished += OnCompilationFinished;
        OnCompilationFinished(null);
    }
    
    //only seems to work half the time.
    private static void OnCompilationFinished(object context)
    {

        var allItems = Resources.LoadAll<ItemSO>("");
        
        foreach (var item in allItems)
        {
            item.ResyncData();
        }
    }

}
