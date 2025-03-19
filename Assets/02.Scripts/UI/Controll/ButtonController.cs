using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] UIState targetState;

    void Awake()
    {
        if (button == null)
            button = GetComponent<Button>();

        button.onClick.AddListener(OnButtonClick);
    }

    void OnButtonClick()
    {
        UIManager.Instance.ChangeState(targetState);
    }
}
