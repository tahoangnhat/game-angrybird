using UnityEngine;

public class CloudEffect : MonoBehaviour
{
    [SerializeField] private float speed = 0.2f;  // Tốc độ trôi ngang
    [SerializeField] private float resetX = -20f; // Vị trí mây quay lại bên phải
    [SerializeField] private float startX = 20f;  // Vị trí bắt đầu lại

    private void Update()
    {
        // Di chuyển tất cả mây con sang trái
        foreach (Transform cloud in transform)
        {
            cloud.Translate(Vector3.left * speed * Time.deltaTime);

            // Nếu mây ra khỏi màn hình bên trái, dịch lại qua bên phải
            if (cloud.position.x < resetX)
            {
                cloud.position = new Vector3(startX, cloud.position.y, cloud.position.z);
            }
        }
    }
}