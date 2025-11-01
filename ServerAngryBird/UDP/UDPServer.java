package UDP;

import java.net.DatagramPacket;
import java.net.DatagramSocket;
import java.net.InetAddress;
import java.nio.charset.StandardCharsets;
import org.json.JSONObject;
import UserAccount.LoginProcess;

public class UDPServer {

    private static final int PORT = 12345;
    private static DatagramSocket socket;

    public static void main(String[] args) {
        try {
            // 🔹 Bắt buộc để đảm bảo console in đúng UTF-8
            System.setOut(new java.io.PrintStream(System.out, true, StandardCharsets.UTF_8));
            System.setErr(new java.io.PrintStream(System.err, true, StandardCharsets.UTF_8));
            System.setProperty("file.encoding", "UTF-8");

            socket = new DatagramSocket(PORT);
            System.out.println("✅ UDP Server đang chạy trên cổng " + PORT);

            byte[] buffer = new byte[4096];

            while (true) {
                // 📨 Nhận dữ liệu từ client
                DatagramPacket packet = new DatagramPacket(buffer, buffer.length);
                socket.receive(packet);

                String received = new String(packet.getData(), 0, packet.getLength(), StandardCharsets.UTF_8);
                InetAddress address = packet.getAddress();
                int port = packet.getPort();

                System.out.println("📩 Nhận từ client: " + received);

                JSONObject req = new JSONObject(received);
                JSONObject res = new JSONObject();

                try {
                    int messType = req.optInt("mess", -1);

                    switch (messType) {
                        case 1: // 🔑 Đăng nhập
                            if (!req.has("username") || !req.has("password")) {
                                res.put("status", "ERROR");
                                res.put("message", "Thiếu username hoặc password!");
                                break;
                            }

                            String username = req.getString("username");
                            String password = req.getString("password");

                            JSONObject loginResult = LoginProcess.ProcessLogin(username, password);

                            if (loginResult.has("status") && loginResult.getString("status").equals("OK")) {
                                int accountId = loginResult.getInt("accountId");
                                res.put("status", "OK");
                                res.put("message", "Đăng nhập thành công!");
                                res.put("player", accountId);
                            } else {
                                res.put("status", "ERROR");
                                res.put("message", loginResult.optString("error", "Sai tài khoản hoặc mật khẩu!"));
                            }
                            break;

                        default:
                            res.put("status", "ERROR");
                            res.put("message", "Yêu cầu không hợp lệ (thiếu messType)!");
                            break;
                    }

                } catch (Exception e) {
                    System.err.println("❌ [SERVER] Lỗi xử lý yêu cầu: " + e.getMessage());
                    e.printStackTrace();
                    res.put("status", "ERROR");
                    res.put("message", "Server gặp lỗi xử lý yêu cầu!");
                }

                // 📤 Gửi phản hồi lại client (đảm bảo gửi UTF-8)
                byte[] responseData = res.toString().getBytes(StandardCharsets.UTF_8);
                DatagramPacket responsePacket = new DatagramPacket(responseData, responseData.length, address, port);
                socket.send(responsePacket);

                System.out.println("📤 Đã gửi phản hồi: " + res.toString());
            }

        } catch (Exception e) {
            System.err.println("🚨 Lỗi khởi chạy UDP Server: " + e.getMessage());
            e.printStackTrace();
        }
    }
}
