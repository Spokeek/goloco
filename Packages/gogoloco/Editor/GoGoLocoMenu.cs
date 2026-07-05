#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using VRC.SDK3.Avatars.Components;

public class GoGoLocoMenu : EditorWindow
{
    const string GOGOLOCO_PATH = "Packages/gogoloco/Runtime/GoGo/GoLoco";

    const string GOGOLOCO_BEYOND_VRCFURY_PATH = GOGOLOCO_PATH + "/Prefabs/VRCFury/GogoLoco Beyond (VRCFury).prefab";
    const string GOGOLOCO_ALL_VRCFURY_PATH = GOGOLOCO_PATH + "/Prefabs/VRCFury/GogoLoco All (VRCFury).prefab";
    const string GOGOLOCO_BEYOND_MA_PATH = GOGOLOCO_PATH + "/Prefabs/Modular Avatar/GogoLoco Beyond (Modular Avatar).prefab";
    const string GOGOLOCO_ALL_MA_PATH = GOGOLOCO_PATH + "/Prefabs/Modular Avatar/GogoLoco All (Modular Avatar).prefab";

    private GameObject avatarTarget;

    private GameObject gogolocoBeyondVRCFuryPrefab;
    private GameObject gogolocoAllVRCFuryPrefab;

    private GameObject gogolocoBeyondMAPrefab;
    private GameObject gogolocoAllMAPrefab;

    private string errorLabel = "";

    private Texture2D headerImage;

    /**
    *  Load the GoGoLoco Prefab Window
    */
    [MenuItem("Tools/GoGoLoco/Add Prefabs")]
    public static void ShowWindow()
    {
        GetWindow<GoGoLocoMenu>("GoGoLoco Prefabs");
    }

    /**
    *  Load the GoGoLoco ressources
    */
    private void OnEnable()
    {
        headerImage = AssetDatabase.LoadAssetAtPath<Texture2D>(GOGOLOCO_PATH + "/Icons/icon_Go_Loco.png");

        gogolocoBeyondVRCFuryPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(GOGOLOCO_BEYOND_VRCFURY_PATH);
        gogolocoAllVRCFuryPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(GOGOLOCO_ALL_VRCFURY_PATH);

        gogolocoBeyondMAPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(GOGOLOCO_BEYOND_MA_PATH);
        gogolocoAllMAPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(GOGOLOCO_ALL_MA_PATH);

        avatarTarget = Selection.activeGameObject;
    }

    /**
    *  Flow of the GoGoLoco Prefab Window
    */
    private void OnGUI()
    {
        GUILayout.BeginHorizontal();
        // GoGoLoco Logo
        GUILayout.Label(headerImage, GUILayout.ExpandWidth(true), GUILayout.MaxHeight(headerImage.height));
        // GoGoLoco Title
        GUILayout.Label("GoGoLoco", GUILayout.ExpandWidth(true), GUILayout.MaxHeight(headerImage.height));
        GUILayout.EndHorizontal();

        // Help Box
        GUILayout.Label("Select your avatar in the hierarchy");
        // Avatar Selector 
        avatarTarget = EditorGUILayout.ObjectField(avatarTarget, typeof(GameObject), true) as GameObject;

        if (avatarTarget == null)
        {
            errorLabel = "Error: No object selected in the Hierarchy.";
            GUI.color = Color.red;
            GUILayout.Label(errorLabel);
            GUI.color = Color.white;
        }
        else if (avatarTarget.GetComponent<VRCAvatarDescriptor>() == null)
        {
            errorLabel = "Error: Selected object isn't an avatar (Doesn't have an AvatarDescriptor Component).";
            GUI.color = Color.red;
            GUILayout.Label(errorLabel);
            GUI.color = Color.white;
        }
        else
        {
            errorLabel = "";
        }

        // Disable buttons if wrong avatar selected
        GUI.enabled = errorLabel == "";

        GUILayout.BeginHorizontal();

        GUILayout.BeginVertical();
        GUILayout.Label("VRCFury Prefabs");
        if (GUILayout.Button("Add GoGoLoco All"))
        {
            AddPrefabToAvatar(gogolocoAllVRCFuryPrefab, avatarTarget, "Add GoGoLoco All (VRCFury)");
        }
        if (GUILayout.Button("Add GoGoLoco Beyond"))
        {
            AddPrefabToAvatar(gogolocoBeyondVRCFuryPrefab, avatarTarget, "Add GoGoLoco Beyond (VRCFury)");
        }
        GUILayout.EndVertical();

        GUILayout.BeginVertical();
        GUILayout.Label("Modular Avatar Prefabs");
        if (GUILayout.Button("Add GoGoLoco All"))
        {
            AddPrefabToAvatar(gogolocoAllMAPrefab, avatarTarget, "Add GoGoLoco All (Modular Avatar)");
        }
        if (GUILayout.Button("Add GoGoLoco Beyond"))
        {
            AddPrefabToAvatar(gogolocoBeyondMAPrefab, avatarTarget, "Add GoGoLoco Beyond (Modular Avatar)");
        }
        GUILayout.EndVertical();

        GUILayout.EndHorizontal();
        GUI.enabled = true;
    }

    /*
    Instantiate the given prefab under the target avatar, register undo, mark the avatar dirty, and ping the new object in the Hierarchy.
    This is important to ensure the Scene knows it's been modified.
    */
    private static void AddPrefabToAvatar(GameObject prefab, GameObject avatar, string undoLabel)
    {
        if (prefab == null || avatar == null) return;

        GameObject instantiatedPrefab = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
        instantiatedPrefab.transform.SetParent(avatar.transform, false);

        Undo.RegisterCreatedObjectUndo(instantiatedPrefab, undoLabel);
        EditorUtility.SetDirty(avatar);

        Selection.activeObject = instantiatedPrefab;
        EditorGUIUtility.PingObject(instantiatedPrefab);
    }

    /*
    Option to apply prefab from GameObject/GoGoLoco MenuItem
    */
    private static void AddPrefabFromMenu(string prefabPath, string undoLabel)
    {
        GameObject selectedObject = Selection.activeGameObject;

        if (selectedObject == null || selectedObject.GetComponent<VRCAvatarDescriptor>() == null)
        {
            EditorUtility.DisplayDialog("Error", "Please first select a VRChat Avatar in the current Scene, then try again. Make sure it has the VRC Avatar Descriptor Component on it.", "OK");
            return;
        }

        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);

        if (prefab == null)
        {
            EditorUtility.DisplayDialog("Error", "Could not load the GoGoLoco prefab at path: " + prefabPath, "OK");
            return;
        }

        AddPrefabToAvatar(prefab, selectedObject, undoLabel);
    }

    private static bool ValidateSelectedAvatar()
    {
        GameObject selectedObject = Selection.activeGameObject;
        return selectedObject != null && selectedObject.GetComponent<VRCAvatarDescriptor>() != null;
    }

    // VRCFury
    [MenuItem("GameObject/GoGoLoco/Add GoGoLoco All (VRCFury)", false, 10)]
    public static void AddGoGoLocoAllVRCFuryMenu()
    {
        AddPrefabFromMenu(GOGOLOCO_ALL_VRCFURY_PATH, "Add GoGoLoco All (VRCFury)");
    }

    [MenuItem("GameObject/GoGoLoco/Add GoGoLoco All (VRCFury)", true)]
    public static bool ValidateAddGoGoLocoAllVRCFuryMenu() => ValidateSelectedAvatar();

    [MenuItem("GameObject/GoGoLoco/Add GoGoLoco Beyond (VRCFury)", false, 10)]
    public static void AddGoGoLocoBeyondVRCFuryMenu()
    {
        AddPrefabFromMenu(GOGOLOCO_BEYOND_VRCFURY_PATH, "Add GoGoLoco Beyond (VRCFury)");
    }

    [MenuItem("GameObject/GoGoLoco/Add GoGoLoco Beyond (VRCFury)", true)]
    public static bool ValidateAddGoGoLocoBeyondVRCFuryMenu() => ValidateSelectedAvatar();

    // Modular Avatar
    [MenuItem("GameObject/GoGoLoco/Add GoGoLoco All (Modular Avatar)", false, 10)]
    public static void AddGoGoLocoAllMAMenu()
    {
        AddPrefabFromMenu(GOGOLOCO_ALL_MA_PATH, "Add GoGoLoco All (Modular Avatar)");
    }

    [MenuItem("GameObject/GoGoLoco/Add GoGoLoco All (Modular Avatar)", true)]
    public static bool ValidateAddGoGoLocoAllMAMenu() => ValidateSelectedAvatar();

    [MenuItem("GameObject/GoGoLoco/Add GoGoLoco Beyond (Modular Avatar)", false, 10)]
    public static void AddGoGoLocoBeyondMAMenu()
    {
        AddPrefabFromMenu(GOGOLOCO_BEYOND_MA_PATH, "Add GoGoLoco Beyond (Modular Avatar)");
    }

    [MenuItem("GameObject/GoGoLoco/Add GoGoLoco Beyond (Modular Avatar)", true)]
    public static bool ValidateAddGoGoLocoBeyondMAMenu() => ValidateSelectedAvatar();
}

#endif
