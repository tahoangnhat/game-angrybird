import java.sql.Connection;
import DAO.DatabaseManager;

public class TestConnection {
    public static void main(String[] args) {
        try (Connection conn = DatabaseManager.getConnection()) {
            if (conn != null && !conn.isClosed()) {
                System.out.println("✅ Kết nối SQL Server thành công!");
            } else {
                System.out.println("❌ Không thể kết nối SQL Server!");
            }
        } catch (Exception e) {
            e.printStackTrace();
        } finally {
            DatabaseManager.shutdown();
        }
    }
}