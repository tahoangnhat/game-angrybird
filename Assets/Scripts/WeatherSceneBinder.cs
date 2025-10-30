using UnityEngine;

public class WeatherSceneBinder : MonoBehaviour
{
    void Start()
    {
        // ✅ Tìm WeatherManager đang sống xuyên suốt game
        WeatherManager wm = WeatherManager.Instance;
        if (wm == null)
        {
            wm = FindFirstObjectByType<WeatherManager>();
        }

        if (wm != null)
        {
            GameObject sun = GameObject.Find("SunEffect");
            GameObject rain = GameObject.Find("RainEffect");
            GameObject cloud = GameObject.Find("CloudEffect");

            wm.SetEffects(sun, rain, cloud);
            Debug.Log("🔗 WeatherSceneBinder: Đã liên kết lại hiệu ứng cho scene mới");
        }
        else
        {
            Debug.LogWarning("⚠ Không tìm thấy WeatherManager trong scene!");
        }
    }
}
