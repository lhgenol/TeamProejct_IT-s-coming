using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] UIState targetState; // 예: 변경할 상태 enum

    void Awake()
    {
        // button 컴포넌트 직접 참조, 혹은 인스펙터에서 할당
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
