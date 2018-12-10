package com.apple.apiforji.controller;

import com.mysql.jdbc.Connection;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

import java.io.File;
import java.sql.DriverManager;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.sql.Statement;

@Controller
public class TestLogin {

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

    @RequestMapping(value = "/login2")
    public Object login(String username, String password, String message, Model model) throws SQLException {
        Connection conn = null;
        Statement statement = null;
        try {
            // 获取数据库连接
            conn = getConn();

            // 获取语句statement对象
            statement = conn.createStatement();

            statement.execute("update user set comment = '" + message +"' where username='" + username + "'");

            // 用传入参数执行sql语句
            System.out.println("sql statement：" + "select * from user where username = '" + username + "' and password = '" + password + "'");

            statement.execute("select * from user where username = '" + username + "' and password = '" + password + "'");

            // 获取并解析结果
            ResultSet resultSet = statement.getResultSet();
            if (resultSet.next()) {
                // 按照给定的用户名和密码查询存在数据，说明用户信息验证通过
//                System.out.println("有用户数据，验证通过");
//                System.out.println("用户名 " + resultSet.getString(2));
//                return ("success, " + resultSet.getString(7) + "<br/>message is : " + message);
                model.addAttribute("comment", resultSet.getString(7));
                model.addAttribute("message", message);
                return "success.html";
            }
        } catch (Exception e) {
            e.printStackTrace();
            return e.getMessage();
        } finally {
            try {
                if (statement != null) statement.close();
                if (conn != null) conn.close();
            } catch (Exception e) {

            }
        }



        return "failed";
    }

    @RequestMapping(value = "/show2")
    public Object show(String username, String password, String message, Model model) throws SQLException {
        Connection conn = null;
        Statement statement = null;
        try {
            // 获取数据库连接
            conn = getConn();

            // 获取语句statement对象
            statement = conn.createStatement();

            // 用传入参数执行sql语句
            System.out.println("sql statement：" + "select * from user where username = '" + username + "' and password = '" + password + "'");

            statement.execute("select * from user where username = '" + "11@qq.com" + "' ");

            // 获取并解析结果
            ResultSet resultSet = statement.getResultSet();
            if (resultSet.next()) {
                // 按照给定的用户名和密码查询存在数据，说明用户信息验证通过
//                System.out.println("有用户数据，验证通过");
//                System.out.println("用户名 " + resultSet.getString(2));
//                return ("success, " + resultSet.getString(7) + "<br/>message is : " + message);
                model.addAttribute("comment", resultSet.getString(7));
                model.addAttribute("message", message);
                return "success.html";
            }
        } catch (Exception e) {
            e.printStackTrace();
            return e.getMessage();
        } finally {
            try {
                if (statement != null) statement.close();
                if (conn != null) conn.close();
            } catch (Exception e) {

            }
        }



        return "failed";
    }
}
