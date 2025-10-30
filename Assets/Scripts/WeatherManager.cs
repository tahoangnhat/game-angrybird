using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

#region Data Models
[Serializable]
public class WeatherInfo
{
    public Weather[] weather;
    public Main main;
}

[Serializable]
public class Weather
{
    public string main;
    public string description;
}

[Serializable]
public class Main
{
    public float temp;
}
#endregion

public class WeatherManager : MonoBehaviour
{
    public static WeatherManager Instance; // ✅ Singleton

    [Header("API Settings")]
    [SerializeField] private string apiKey = "YOUR_API_KEY";  // ⚠️ Nhập key thật
    [SerializeField] private string city = "Ho Chi Minh";
    private string apiUrl = "https://api.openweathermap.org/data/2.5/weather?q={0}&appid={1}&units=metric";

    [Header("Weather Effects")]
    public GameObject sunEffect;    // ☀️ Prefab mặt trời
    public GameObject rainEffect;   // 🌧 Prefab mưa
    public GameObject cloudEffect;  // ☁️ Prefab mây

    [Header("Optional")]
    public float checkInterval = 300f; // 5 phút

    private string currentWeather = "";

    private void Awake()
    {
        // ✅ Đảm bảo chỉ có 1 WeatherManager tồn tại xuyên suốt game
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        StartCoroutine(GetWeatherLoop());
    }

    IEnumerator GetWeatherLoop()
    {
        while (true)
        {
            yield return GetWeather();
            yield return new WaitForSeconds(checkInterval);
        }
    }

    IEnumerator GetWeather()
    {
        if (string.IsNullOrEmpty(apiKey) || apiKey == "YOUR_API_KEY")
        {
            Debug.LogError("❌ Bạn chưa nhập API Key trong WeatherManager!");
            yield break;
        }

        string finalUrl = string.Format(apiUrl, city, apiKey);
        UnityWebRequest request = UnityWebRequest.Get(finalUrl);
        Debug.Log($"🌍 Fetching weather: {finalUrl}");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string json = request.downloadHandler.text;
            WeatherInfo info = JsonUtility.FromJson<WeatherInfo>(json);

            if (info != null && info.weather != null && info.weather.Length > 0)
            {
                currentWeather = info.weather[0].main;
                Debug.Log($"☁ Weather: {currentWeather}, 🌡 Temp: {info.main.temp}°C");
                ChangeEnvironment(currentWeather);
            }
            else
            {
                Debug.LogWarning("⚠ Không thể đọc dữ liệu thời tiết.");
            }
        }
        else
        {
            Debug.LogError($"❌ Lỗi khi gọi API: {request.error}");
        }
    }

    public void ChangeEnvironment(string weather)
    {
        if (sunEffect) sunEffect.SetActive(false);
        if (rainEffect) rainEffect.SetActive(false);
        if (cloudEffect) cloudEffect.SetActive(false);

        switch (weather)
        {
            case "Clear":
            case "Sun":
            case "Sunny":
                if (sunEffect) sunEffect.SetActive(true);
                Debug.Log("☀️ Trời nắng");
                break;

            case "Rain":
            case "Drizzle":
                if (rainEffect) rainEffect.SetActive(true);
                Debug.Log("🌧 Trời mưa");
                break;

            case "Clouds":
            default:
                if (cloudEffect) cloudEffect.SetActive(true);
                Debug.Log("☁️ Trời mây");
                break;
        }
    }

    // ✅ Dành cho scene khác để cập nhật object hiệu ứng
    public void SetEffects(GameObject sun, GameObject rain, GameObject cloud)
    {
        sunEffect = sun;
        rainEffect = rain;
        cloudEffect = cloud;

        if (!string.IsNullOrEmpty(currentWeather))
            ChangeEnvironment(currentWeather);
    }

    // ✅ Nếu muốn thay thành phố khác khi đang chạy
    public void SetCity(string newCity)
    {
        city = newCity;
        StartCoroutine(GetWeather());
    }
}
