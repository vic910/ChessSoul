using UnityEditor;
using UnityEngine;

public class NewBehaviourScript
{


    [MenuItem("GameObject/DeleteThisObject", true, 10)]
    static bool checkCanDelete()
    {
        return Selection.activeGameObject != null;
    }

    //[MenuItem("Assets/DeleteThisObject",true,10)]
    [MenuItem("GameObject/DeleteThisObject", false, 10)]
    public static void DeleteGameObject()
    {
        Undo.DestroyObjectImmediate(Selection.activeGameObject);
    }

}
