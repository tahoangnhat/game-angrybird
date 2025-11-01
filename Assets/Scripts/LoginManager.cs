using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Newtonsoft.Json.Linq;

public class LoginManager : MonoBehaviour
{
    [Header("UI References")]
    public TMP_InputField usernameInputField;
    public TMP_InputField passwordInputField;
    public Button loginButton;
    public GameObject MessagePanel;
    public TMP_Text MessageText;

    private void Start()
    {
        loginButton.onClick.AddListener(Login);
        MessagePanel.SetActive(false); // Ẩn bảng thông báo khi bắt đầu
    }

    public async void Login()
    {
        string username = usernameInputField.text.Trim();
        string password = passwordInputField.text.Trim();

        // 🧩 Kiểm tra nhập liệu
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            ShowMessage("Không được để trống tài khoản hoặc mật khẩu!");
            Invoke(nameof(HideMessage), 2f); // ẩn sau 2 giây

            return;
        }

        try
        {
            using (UdpClient client = new UdpClient())
            {
                // Kết nối tới server UDP
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Loopback, 12345);

                // Tạo JSON gửi lên server
                JObject json = new JObject
                {
                    ["mess"] = 1,
                    ["username"] = username,
                    ["password"] = password
                };

                byte[] data = Encoding.UTF8.GetBytes(json.ToString());
                await client.SendAsync(data, data.Length, endPoint);

                // ⏳ Chờ phản hồi (tối đa 5 giây)
                client.Client.ReceiveTimeout = 5000;
                var result = await client.ReceiveAsync();
                string response = Encoding.UTF8.GetString(result.Buffer);

                JObject res = JObject.Parse(response);
                string status = res["status"]?.ToString() ?? "ERROR";
                string message = res["message"]?.ToString() ?? "Phản hồi không hợp lệ";

                ShowMessage(message);

                // ✅ Nếu đăng nhập thành công
                if (status == "OK")
                {
                    Invoke(nameof(GoToMainMenu), 2f); // Chuyển scene sau 2 giây
                }
                else
                {
                    // ❌ Nếu thất bại -> tự ẩn thông báo
                    Invoke(nameof(HideMessage), 2f);
                }
            }
        }
        catch (Exception ex)
        {
            ShowMessage("🚫 Lỗi kết nối: " + ex.Message);
            Invoke(nameof(HideMessage), 3f);
        }
    }

    // 🧠 Hàm hiển thị thông báo
    private void ShowMessage(string message)
    {
        MessagePanel.SetActive(true);
        MessageText.text = message;
    }

    // 🕒 Tự ẩn bảng thông báo
    private void HideMessage()
    {
        MessagePanel.SetActive(false);
    }

    // 🚀 Chuyển sang scene MainMenu
    private void GoToMainMenu()
    {
        SceneManager.LoadScene("SceneMainMenu"); // ⚙️ Nhớ đúng tên scene
    }
}
