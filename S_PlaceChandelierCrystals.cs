using UnityEditor;
using UnityEngine;

public class S_PlaceChandelierCrystals : EditorWindow
{
    GameObject crystalPrefab = null;            //public
    GameObject treeObject = null;

    float objectScale = 1f;

    private bool placerActivated = false;

    [MenuItem("Tools/Chandelier Crystal Placer")]
    public static void ShowWindow(){ GetWindow(typeof(S_PlaceChandelierCrystals)); }

    private void OnEnable()                         //event Handlers
    { SceneView.duringSceneGui += OnSceneGUI; }

    private void OnDisable(){ SceneView.duringSceneGui -= OnSceneGUI; }


    private void OnGUI()                                //GUI handlers
    {
        GUILayout.Space(10f);

        if (GUILayout.Button("Activate Tool"))
            placerActivated = !placerActivated;

        GUILayout.Label("Tool Activated: " + placerActivated);

        GUILayout.Space(10f);
        GUILayout.Width(10f);

        objectScale = EditorGUILayout.FloatField("Object Scale", objectScale);
        crystalPrefab = EditorGUILayout.ObjectField("Crystal Prefab", crystalPrefab, typeof(GameObject), false) as GameObject;
        treeObject = EditorGUILayout.ObjectField("Tree Piece", treeObject, typeof(GameObject), false) as GameObject;

        GUILayout.Space(10f);

        GUILayout.Label("Chandelier Pieces: " + treeObject.transform.childCount);
    }

    private void OnSceneGUI(SceneView view)
    {
        if (placerActivated)                            //can only work if the tool is activated, can be turned on or off at any given time
            checkMouseDown();
    }

    private void checkMouseDown()
    {
        if (Event.current.type == EventType.MouseDown)
        {
            Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 1000.0f))
            {
                if (hit.transform.tag == "Tree")            //check if mouse clicked on tree, else chandelier piece will not place
                    CreateCrystal(hit);

                else
                    Debug.LogWarning("Warning: Chandelier Pieces can only be placed on the tree.");
            }
        }
    }


    private void CreateCrystal(RaycastHit hit)          //GameObject Handlers
    {
        if (crystalPrefab == null || treeObject == null)
        {
            Debug.LogError("Error: One or more GameObjects not assigned.");
            return;
        }

        Vector3 newPos = new Vector3(hit.point.x, hit.point.y - 0.05f, hit.point.z);
        float randomY = Random.Range(0, 360);
        Quaternion rotation = new Quaternion(Quaternion.identity.x, randomY, Quaternion.identity.z, Quaternion.identity.w);

        GameObject newPiece = PrefabUtility.InstantiatePrefab(crystalPrefab) as GameObject;

        newPiece.transform.localPosition = newPos;
        newPiece.transform.localRotation = rotation;
        newPiece.transform.localScale = Vector3.one * objectScale;
    }
    
}
