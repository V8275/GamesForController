using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.UI;

public class WebSocketManager : MonoBehaviour
{
    public event Action<MyDataModel> OnDataReceived;

    [SerializeField]
    private string WebSocketUrl = "ws://localhost:8080";
    [SerializeField]
    private GameObject WebSocketDisconnect;

    private Button connectButton;
    private InputField inputField;
    private GameObject exceptionPanel;
    private Text outputText;

    private ClientWebSocket _webSocket;
    private PlayerController _playerController;
    private bool connected = false;

    // Singleton instance
    private static WebSocketManager _instance;
    public static WebSocketManager Instance => _instance;

    private void Awake()
    {
        // Singleton pattern implementation
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        // Назначаем обработчик нажатия кнопки
        if (connectButton != null)
        {
            connectButton.onClick.AddListener(OnConnectButtonClicked);
        }
    }

    public void InitUI(Button conBut, InputField conField, GameObject excePanel, GameObject disconnectPanel, Text debugTxt = null)
    {
        WebSocketDisconnect = disconnectPanel;
        connectButton = conBut;
        inputField = conField;
        exceptionPanel = excePanel;
        if(debugTxt)
            outputText = debugTxt;

        if (connectButton != null)
        {
            connectButton.onClick.AddListener(OnConnectButtonClicked);
        }
    }

    // Обработчик нажатия кнопки
    public async void OnConnectButtonClicked()
    {
        WebSocketUrl = inputField.text;
        await ConnectToServer();
    }

    // Публичный метод для подключения к серверу
    public async Task ConnectToServer()
    {
        if (_webSocket == null || _webSocket.State != WebSocketState.Open)
        {
            try
            {
                _webSocket = new ClientWebSocket();
                await _webSocket.ConnectAsync(new Uri(WebSocketUrl), CancellationToken.None);
                Debug.Log("Connected to WebSocket server");
                connected = true;

                _ = ReceiveMessagesAsync(); // Запускаем прием сообщений

                // Можно обновить UI, например, изменить текст кнопки
                if (connectButton != null)
                {
                    connectButton.GetComponentInChildren<Text>().text = "Connected";
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Connection failed: {ex.Message}");
                connected = false;
                exceptionPanel.SetActive(true);
                // Обновляем UI при ошибке
                if (connectButton != null)
                {
                    connectButton.GetComponentInChildren<Text>().text = "Retry Connect";
                }
            }
        }
    }

    private async Task ReceiveMessagesAsync()
    {
        var buffer = new byte[1024 * 4];

        try
        {
            while (_webSocket != null && _webSocket.State == WebSocketState.Open)
            {
                var result = await _webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                if (result.MessageType == WebSocketMessageType.Text)
                {
                    var jsonString = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    Debug.Log($"Received JSON: {jsonString}");

                    try
                    {
                        var data = JsonUtility.FromJson<MyDataModel>(jsonString);
                        UpdateUIText(data);
                        OnDataReceived?.Invoke(data);
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError($"Error deserializing JSON: {ex.Message}");
                    }
                }
                else if (result.MessageType == WebSocketMessageType.Close)
                {
                    await _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
                    Debug.Log("WebSocket connection closed");
                    if (WebSocketDisconnect)
                        DisconnectMessage();
                    break;
                }
            }
        }
        catch (WebSocketException ex)
        {
            Debug.LogError($"WebSocket error: {ex.Message}");
            if (WebSocketDisconnect)
                DisconnectMessage();
        }
        catch (Exception ex)
        {
            Debug.LogError($"General error: {ex.Message}");
            if (WebSocketDisconnect)
                DisconnectMessage();
        }
        finally
        {
            connected = false;
            // Обновляем UI при отключении
            if (connectButton != null)
            {
                connectButton.GetComponentInChildren<Text>().text = "Connect";
            }
        }
    }


    private void DisconnectMessage()
    {
        WebSocketDisconnect.SetActive(true);
        Time.timeScale = 0;
    }

    private void UpdateUIText(MyDataModel data)
    {
        if (outputText)
            outputText.text = $"Property1: {data.Property1}\nProperty2: {data.Property2}";
    }

    private void OnApplicationQuit()
    {
        if (_webSocket != null)
        {
            _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Application closing", CancellationToken.None).Wait();
            _webSocket.Dispose();
        }
    }

    public bool isConnected()
    {
        return connected;
    }

    public void InitUI(GameObject Win)
    {
        WebSocketDisconnect = Win;
    }
}

[System.Serializable]
public class MyDataModel
{
    public string Property1;
    public int Property2;
}
