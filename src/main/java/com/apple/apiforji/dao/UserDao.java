package com.apple.apiforji.dao;

import java.sql.DriverManager;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.sql.Statement;

import com.mysql.jdbc.Connection;
import com.mysql.jdbc.PreparedStatement;

public class UserDao {

    public static void main(String[] args) throws SQLException {
        Connection conn = getConn();
        Statement statement = conn.createStatement();
        statement.execute("select * from user where username = '" + "11@qq.com" + "' and password = '" + "123456" + "'");
        ResultSet resultSet = statement.getResultSet();
        if (resultSet.next()) {
            System.out.println("有用户数据，验证通过");
            System.out.println("用户名 " + resultSet.getString(2));
        }
    }

    private static Connection getConn() {
        String driver = "com.mysql.jdbc.Driver";
        String url = "jdbc:mysql://localhost:3306/test";
        String username = "root";
        String password = "";
        Connection conn = null;
        try {
            Class.forName(driver);
            conn = (Connection) DriverManager.getConnection(url, username, password);
        } catch (ClassNotFoundException e) {
            e.printStackTrace();
        } catch (SQLException e) {
            e.printStackTrace();
        }
        return conn;
    }
}
