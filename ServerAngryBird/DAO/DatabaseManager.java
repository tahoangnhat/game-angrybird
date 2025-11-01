package DAO;

import java.io.FileInputStream;
import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.util.Properties;

import com.zaxxer.hikari.HikariConfig;
import com.zaxxer.hikari.HikariDataSource;

public class DatabaseManager {
    private static HikariDataSource dataSource;

    static {
        loadConfigAndInitPool();
    }

    private static void loadConfigAndInitPool() {
        Properties properties = new Properties();
        String filePath = "ServerAngryBird/DAO/config.properties";

        try (FileInputStream input = new FileInputStream(filePath)) {
            properties.load(input);

            HikariConfig config = new HikariConfig();

            // ⚙️ JDBC URL cho SQL Server
            config.setJdbcUrl(properties.getProperty("Url"));
            config.setUsername(properties.getProperty("User"));
            config.setPassword(properties.getProperty("Pass"));

            // ⚡ Cấu hình hiệu năng
            config.setMaximumPoolSize(50);
            config.setMinimumIdle(10);
            config.setIdleTimeout(60000);          // 60 giây
            config.setMaxLifetime(300000);         // 5 phút
            config.setConnectionTimeout(10000);    // 10 giây

            // ✅ Bắt buộc cho SQL Server: driver class
            config.setDriverClassName("com.microsoft.sqlserver.jdbc.SQLServerDriver");

            dataSource = new HikariDataSource(config);
            System.out.println("[INFO] ✅ HikariCP đã khởi tạo thành công!");

        } catch (Exception e) {
            e.printStackTrace();
            System.err.println("[ERROR] ❌ Lỗi khởi tạo HikariCP: " + e.getMessage());
        }
    }

    /** Lấy connection từ pool */
    public static Connection getConnection() throws SQLException {
        return dataSource.getConnection();
    }

    /** Đóng pool khi tắt ứng dụng */
    public static void shutdown() {
        if (dataSource != null && !dataSource.isClosed()) {
            dataSource.close();
            System.out.println("[INFO] 🔌 Đã ngắt kết nối HikariCP!");
        }
    }

    /** Thực thi câu lệnh SELECT */
    public static ResultSet query(String sql, Object... params) throws SQLException {
        Connection conn = getConnection();
        PreparedStatement stmt = conn.prepareStatement(sql);
        setParams(stmt, params);
        return stmt.executeQuery();
    }

    /** Thực thi INSERT/UPDATE/DELETE */
    public static int update(String sql, Object... params) throws SQLException {
        try (Connection conn = getConnection();
             PreparedStatement stmt = conn.prepareStatement(sql)) {
            setParams(stmt, params);
            return stmt.executeUpdate();
        }
    }

    /** Gán tham số cho PreparedStatement */
    private static void setParams(PreparedStatement stmt, Object... params) throws SQLException {
        for (int i = 0; i < params.length; i++) {
            stmt.setObject(i + 1, params[i]);
        }
    }
}