#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using VRC.SDK3.Avatars.Components;

public class GoGoLocoMenu : EditorWindow
{
    const string GOGOLOCO_PATH = "Packages/gogoloco/Runtime/GoGo/GoLoco";

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

        gogolocoBeyondVRCFuryPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(GOGOLOCO_PATH + "/Prefabs/VRCFury/GogoLoco Beyond (VRCFury).prefab");
        gogolocoAllVRCFuryPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(GOGOLOCO_PATH + "/Prefabs/VRCFury/GogoLoco All (VRCFury).prefab");

        gogolocoBeyondMAPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(GOGOLOCO_PATH + "/Prefabs/Modular Avatar/GogoLoco Beyond (Modular Avatar).prefab");
        gogolocoAllMAPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(GOGOLOCO_PATH + "/Prefabs/Modular Avatar/GogoLoco All (Modular Avatar).prefab");

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
            //childObject.transform.IsChildOf(parentObject.transform)
            GameObject instantiatedPrefab = PrefabUtility.InstantiatePrefab(gogolocoAllVRCFuryPrefab) as GameObject;
            instantiatedPrefab.transform.SetParent(avatarTarget.transform, false);
        }
        if (GUILayout.Button("Add GoGoLoco Beyond"))
        {
            GameObject instantiatedPrefab = PrefabUtility.InstantiatePrefab(gogolocoBeyondVRCFuryPrefab) as GameObject;
            instantiatedPrefab.transform.SetParent(avatarTarget.transform, false);
        }
        GUILayout.EndVertical();

        GUILayout.BeginVertical();
        GUILayout.Label("Modular Avatar Prefabs");
        if (GUILayout.Button("Add GoGoLoco All"))
        {
            GameObject instantiatedPrefab = PrefabUtility.InstantiatePrefab(gogolocoAllMAPrefab) as GameObject;
            instantiatedPrefab.transform.SetParent(avatarTarget.transform, false);
        }
        if (GUILayout.Button("Add GoGoLoco Beyond"))
        {
            GameObject instantiatedPrefab = PrefabUtility.InstantiatePrefab(gogolocoBeyondMAPrefab) as GameObject;
            instantiatedPrefab.transform.SetParent(avatarTarget.transform, false);
        }
        GUILayout.EndVertical();

        GUILayout.EndHorizontal();
        GUI.enabled = true;
    }
}

#endif

