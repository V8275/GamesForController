using UnityEngine;
using UnityEngine.UI;

public class ConnectUIController : MonoBehaviour
{
    [SerializeField]
    private InputField field;
    [SerializeField]
    private Button button;
    [SerializeField]
    private GameObject ErrorConnection;
    [SerializeField]
    private GameObject WebSocketDisc;
    [SerializeField]
    private Text TextText;

    private void Start()
    {
        if(WebSocketManager.Instance != null)
        {
            WebSocketManager.Instance.InitUI(button, field, ErrorConnection, WebSocketDisc, TextText);
        }
    }
}
