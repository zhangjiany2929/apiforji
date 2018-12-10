package com.apple.apiforji.controller;

import com.apple.apiforji.model.ResultModel;
import com.apple.apiforji.model.ReturnValue;
import com.mysql.jdbc.Connection;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

import java.io.File;
import java.sql.DriverManager;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.sql.Statement;

@RestController
public class TestAPI {

    private Boolean isOpen = false;


    @RequestMapping(value = "/openCurtain")
    public Object openCurtain () {
        ReturnValue returnValue = new ReturnValue();
        HttpUtil.get("http://192.168.123.81:8000/open-curtain");
        if (!isOpen) {
            System.out.println("OPEN CURTAIN...");
            returnValue.setReply("成功打开灯泡");
            isOpen = true;
        } else {
            returnValue.setReply("灯泡已经打开了");
        }
        returnValue.setResultType("RESULT");
        returnValue.setExecuteCode("SUCCESS");
        ResultModel resultModel = new ResultModel();
        resultModel.setReturnCode("0");
        resultModel.setReturnValue(returnValue);
        return resultModel;
    }

    @RequestMapping(value = "/closeCurtain")
    public Object closeCurtain () {
        ReturnValue returnValue = new ReturnValue();
        HttpUtil.get("http://192.168.123.81:8000/close-curtain");
        if (isOpen) {
            System.out.println("CLOSE CURTAIN...");
            returnValue.setReply("成功关闭灯泡");
            isOpen = false;
        } else {
            returnValue.setReply("灯泡已经关闭了");
        }
        returnValue.setResultType("RESULT");
        returnValue.setExecuteCode("SUCCESS");
        ResultModel resultModel = new ResultModel();
        resultModel.setReturnCode("0");
        resultModel.setReturnValue(returnValue);
        return resultModel;
    }

    @RequestMapping(value = "/test-api")
    public Object testAPI () {
        Person person = new Person();
        person.setAge(10);
        person.setGender("male");
        person.setName("zhangsan");
        return person;
    }

    class Person {
        private String name;
        private Integer age;
        private String gender;

        public String getName() {
            return name;
        }

        public void setName(String name) {
            this.name = name;
        }

        public Integer getAge() {
            return age;
        }

        public void setAge(Integer age) {
            this.age = age;
        }

        public String getGender() {
            return gender;
        }

        public void setGender(String gender) {
            this.gender = gender;
        }
    }

    public static void main(String[] args) {
        File[] files = new File(".").listFiles((f) -> !f.isHidden());
        System.out.println();
    }
    public interface Predicate<T> {

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

    @RequestMapping(value = "/login")
    public Object login(String username, String password, String comment) throws SQLException {
        Connection conn = null;
        Statement statement = null;
        try {
            // 获取数据库连接
            conn = getConn();

            // 获取语句statement对象
            statement = conn.createStatement();

            // 用传入参数执行sql语句
            System.out.println("sql statement：" + "select * from user where username = '" + username + "' and password = '" + password + "'");

            statement.execute("select * from user where username = '" + username + "' and password = '" + password + "'");

            // 获取并解析结果
            ResultSet resultSet = statement.getResultSet();
            if (resultSet.next()) {
                // 按照给定的用户名和密码查询存在数据，说明用户信息验证通过
//                System.out.println("有用户数据，验证通过");
//                System.out.println("用户名 " + resultSet.getString(2));
                return ("success, " + resultSet.getString(7) + "<br/>comment is : " + comment);
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

    @RequestMapping(value = "/show")
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
                return "<html xmlns=\"http://www.w3.org/1999/xhtml\" xmlns:th=\"http://www.thymeleaf.org\" >\n" +
                        "<head>\n" +
                        "    <title>查看留言</title>\n" +
                        "    <meta content=\"text/html;charset=UTF-8\"/>\n" +
                        "    <meta name=\"viewport\" content=\"width=device-width,initial-scale=1\"/>\n" +
                        "    <link th:href=\"@{/bootstrap/css/bootstrap.min.css}\" rel=\"stylesheet\"/>\n" +
                        "    <link th:href=\"@{/bootstrap/css/bootstrap-theme.min.css}\" rel=\"stylesheet\"/>\n" +
                        "</head>\n" +
                        "<body>\n" +
                        "success,<br/>\n" + resultSet.getString(7) +
                        "</body>\n" +
                        "</html>";
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
