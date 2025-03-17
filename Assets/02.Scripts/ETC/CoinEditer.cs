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
            EditorGUILayout.HelpBox("GameManager �ν��Ͻ��� ã�� �� �����ϴ�!", MessageType.Warning);
            return;
        }

        GameManager gm = GameManager.Instance;
        PlayerManager pm = PlayerManager.Instance;

        GUILayout.Label("GameManager Controller", EditorStyles.boldLabel);
        gm.Coin = EditorGUILayout.IntField("Coin", gm.Coin);

        if (GUILayout.Button("���� ���� (+1)"))
        {
            gm.GetCoin();
        }

        if (GUILayout.Button("�÷��̾� ü������"))
        {
            pm.Player.health = 100;
        }
    }
}