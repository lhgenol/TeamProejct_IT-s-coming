using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] UIState targetState; // ��: ������ ���� enum

    void Awake()
    {
        // button ������Ʈ ���� ����, Ȥ�� �ν����Ϳ��� �Ҵ�
        if (button == null)
            button = GetComponent<Button>();

        button.onClick.AddListener(OnButtonClick);
    }

    void OnButtonClick()
    {
        Debug.Log(targetState);
        UIManager.Instance.ChangeState(targetState);
    }
}
