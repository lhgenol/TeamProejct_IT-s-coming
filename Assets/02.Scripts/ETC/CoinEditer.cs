using UnityEditor;
using UnityEngine;

public class GameManagerWindow : EditorWindow
{
    [MenuItem("Custom Tools/GameManager Editor")]
    public static void ShowWindow()
    {
        GetWindow<GameManagerWindow>("Game Manager");
    }

    private void OnGUI()
    {
        if (GameManager.Instance == null)
        {
            EditorGUILayout.HelpBox("GameManager 인스턴스를 찾을 수 없습니다!", MessageType.Warning);
            return;
        }

        GameManager gm = GameManager.Instance;

        GUILayout.Label("GameManager Controller", EditorStyles.boldLabel);
        gm.Coin = EditorGUILayout.IntField("Coin", gm.Coin);

        if (GUILayout.Button("코인 증가 (+1)"))
        {
            gm.GetCoin();
        }
    }
}