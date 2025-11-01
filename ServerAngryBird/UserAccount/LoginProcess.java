package UserAccount;

import DAO.DatabaseManager;
import java.sql.*;
import org.json.JSONObject;

public class LoginProcess {

    public static JSONObject ProcessLogin(String Username, String Password) {
        JSONObject req = new JSONObject();

        try (Connection conn = DatabaseManager.getConnection()) {

            String q = "SELECT id FROM accounts WHERE username = ? AND password = ?";
            PreparedStatement stmt = conn.prepareStatement(q);
            stmt.setString(1, Username);
            stmt.setString(2, Password);

            ResultSet rs = stmt.executeQuery();

            if (rs.next()) {
                int accountId = rs.getInt("id");
                req.put("status", "OK");
                req.put("accountId", accountId);
            } else {
                req.put("status", "ERROR"); // 🔹 thêm dòng này
                req.put("message", "Sai tài khoản hoặc mật khẩu");
            }

        } catch (Exception e) {
            req.put("error", "Lỗi server khi đăng nhập: " + e.getMessage());
            e.printStackTrace();
        }

        return req;
    }
}
