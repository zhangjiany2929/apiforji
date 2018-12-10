Imports System.Net.Sockets
Imports System.Net
Imports System.Threading

Public Class Form2
    Public isdataloaded As Boolean = False '确定数据是否已经加载
    Public isdataloaded2 As Boolean = False
    Public isdataloaded3 As Boolean = False
    Public datalen1 As Integer 'A1桥对应的数据数量
    Public datalen2 As Integer 'A2桥对应的数据数量
    Public datalen3 As Integer 'A3桥对应的数据数量

    Public warning1 As Boolean = False 'SP4-SP7桥数据是否引发报警
    Public warning2 As Boolean = False 'SP7-SP10桥数据是否引发报警
    Public warning3 As Boolean = False 'SP10-SP12桥数据是否引发报警
    Public thwarn1 As New System.Threading.Thread(AddressOf warnLight1) 'SP4-SP7桥数据报警
    Public thwarn2 As New System.Threading.Thread(AddressOf warnLight2) 'SP7-SP10桥数据报警
    Public thwarn3 As New System.Threading.Thread(AddressOf warnLight3) 'SP10-SP12桥数据报警
    'Public dataquene1 As New Concurrent.ConcurrentQueue(Of Single) '第1个数据队列
    'Public dataquene2 As New Concurrent.ConcurrentQueue(Of Single) '第2个数据队列
    'Public dataquene3 As New Concurrent.ConcurrentQueue(Of Single) '第3个数据队列

    Public dataquene1 As New Queue(Of Single) '第1个加速度数据队列
    Public dataquene2 As New Queue(Of Single) '第2个加速度数据队列
    Public dataquene3 As New Queue(Of Single) '第3个加速度数据队列

    Public tempquene1 As New Queue(Of Single) '第一个温度队列
    Public humquene1 As New Queue(Of Single) '第一个湿度队列

    Public tempquene2 As New Queue(Of Single) '第二个温度队列
    Public humquene2 As New Queue(Of Single) '第二个湿度队列

    Public isPlayingAlarm As Boolean = False '当前是否正在播放报警
    '主界面下方三个图表显示数据的传感器编号
    Public ch1num As Integer = 11002
    Public ch2num As Integer = 11011
    Public ch3num As Integer = 11014

    'Public allChNums() As Integer = {11002, 11011, 11014, 11004, 11005, 11006, 11007} '待测试时调整
    Public allChNums() As Integer = {11002, 11010, 11014, 11031, 11042, 11048, 11020, 11022, 11036} '待测试时调整

    '当前3个窗口显示的3个桥的编号
    Public curBridgeNums(3) As Integer

    '当前读取的最新传感器数据
    Public accFile01 As String = ""
    Public accFile02 As String = ""
    Public accFile03 As String = ""
    Public tempFile01 As String = ""
    Public tempFile02 As String = ""
    Public tempFile03 As String = ""
    Public tempFile04 As String = ""

    Public accData01() As Single
    Public accData02() As Single
    Public accData03() As Single
    Public tempData01() As Single
    Public tempData02() As Single
    Public tempData03() As Single
    Public tempData04() As Single

    Public dataPoint() As Integer = {0, 0, 0, 0, 0, 0, 0} '当前数据指针
    Public hasNewData() As Boolean = {False, False, False, False, False, False, False} '是否有新的数据







    Private Sub Button1_Click(sender As Object, e As EventArgs)
        Me.Hide()
        Form1.Show()
    End Sub







    Private Sub Form2_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        If MsgBox("是否退出系统？", MsgBoxStyle.OkCancel) = MsgBoxResult.Cancel Then
            e.Cancel = True
            Exit Sub
        End If
        Try
            demoData.Abort()
            demoData2.Abort()
            demoData3.Abort()
            thShowGroundAcc.Abort()
            thShowHum.Abort()
            thShowTemp.Abort()
            thShowWeight.Abort()

        Catch ex As Exception

        End Try

        'Environment.Exit(0)
        Shell("TASKKILL /F /IM SHM.exe")
        System.Threading.Thread.Sleep(10000)

    End Sub


    Private Sub Form2_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.FixedSingle
        Control.CheckForIllegalCrossThreadCalls = False

        curBridgeNums = {0, 1, 2}

        '设置主界面传感子系统listbox默认值，载入默认图片
        'ListBox2.SelectedIndex = 0
        PictureBox2.Image = Image.FromFile("mainimage\zixitong\0.jpg")



        '加载主界面健康状况图片



        '载入gif图片
        PictureBox1.Image = Image.FromFile("mainimage\hk\ratio.gif")

        '新建线程，用于每天进行模态参数识别
        'Dim mi As New System.Threading.Thread(AddressOf midaily)
        'mi.Start()

        '新建线程，用于绘制振型图
        'zhenxinggif = New System.Threading.Thread(AddressOf zhenxing)
        'ComboBox3.SelectedIndex = 0

        '主界面音量条熄灭
        'Label7.Visible = False
        'Label8.Visible = False
        'Label9.Visible = False
        'Label10.Visible = False
        'Label11.Visible = False
        'Label12.Visible = False
        'Label13.Visible = False
        'Label14.Visible = False
        'Label15.Visible = False
        'Label16.Visible = False
        'Label17.Visible = False
        'Label18.Visible = False
        'Label19.Visible = False
        'Label20.Visible = False


        '以下用于深圳演示：
        Dim demoShenzhen = True


        PictureBox17.Parent = PictureBox4
        PictureBox18.Parent = PictureBox4
        PictureBox19.Parent = PictureBox4
        PictureBox20.Parent = PictureBox1

        cleargraph(ZedGraphControl4, 40)
        cleargraph(ZedGraphControl3, 40)
        cleargraph(ZedGraphControl5, 40)
        cleargraph(ZedGraphControl6, 40)
        cleargraph(ZedGraphControl1, 25)
        cleargraph(ZedGraphControl2, 25)
        cleargraph(ZedGraphControl7, 25)

        'ZedGraphControl7.GraphPane.XAxis.IsVisible = False

        '播放视频
        AxWindowsMediaPlayer2.URL = "./" + Profile.ReadOneString("main", "video", "./config/A0.ini")
        AxWindowsMediaPlayer2.Ctlcontrols.play()
        AxWindowsMediaPlayer2.uiMode = "none"
        AxWindowsMediaPlayer2.enableContextMenu = False
        AxWindowsMediaPlayer2.settings.setMode("loop", True)
        AxWindowsMediaPlayer2.Ctlenabled = False
        AxWindowsMediaPlayer2.fullScreen = False
    End Sub

    Private Sub cleargraph(zgc As ZedGraph.ZedGraphControl, fontsize As Integer)

        '设置鼠标指针指向时显示数据的格式为两位小数
        zgc.PointValueFormat = "F"

        Dim myPane = zgc.GraphPane


        zgc.IsShowPointValues = False

        '设置图标标题和x、y轴标题
        'myPane.Title.Text = "A1桥"
        'myPane.XAxis.Title.Text = "时间(s)"
        'myPane.YAxis.Title.Text = "加速度(m/s^2)"

        myPane.Title.Text = ""
        myPane.XAxis.Title.Text = ""
        myPane.YAxis.Title.Text = ""

        'myPane.XAxis.MajorTic.IsOpposite = False
        'myPane.XAxis.IsVisible = False

        '更改标题的字体
        'Dim myFont As ZedGraph.FontSpec = New ZedGraph.FontSpec("Arial", 20, Color.Red, False, False, False)
        'myPane.Title.FontSpec = myFont
        'myPane.XAxis.Title.FontSpec = myFont
        'myPane.YAxis.Title.FontSpec = myFont
        Dim myFont As ZedGraph.FontSpec = New ZedGraph.FontSpec("Times New Roman", 20, Color.Black, False, False, False)
        'myPane.Title.FontSpec = myFont
        myPane.XAxis.Scale.FontSpec.Size = fontsize
        myPane.YAxis.Scale.FontSpec.Size = fontsize
        ' myPane.XAxis.Scale.FontSpec = myFont
        'myPane.XAxis.Title.FontSpec = myFont
        'myPane.YAxis.Title.FontSpec = myFont
        myPane.YAxis.Title.FontSpec.Size = fontsize
        myPane.XAxis.Title.FontSpec.Size = fontsize
        myPane.YAxis.Title.FontSpec.Family = "Times New Roman"
        myPane.XAxis.Scale.FontSpec.Family = "Times New Roman"
        myPane.XAxis.Scale.FontSpec.Size = fontsize
        myPane.YAxis.Scale.FontSpec.Family = "Times New Roman"
        myPane.YAxis.Scale.FontSpec.Size = fontsize


        '显示网格
        'myPane.XAxis.MinorGrid.IsVisible = True
        myPane.XAxis.MajorGrid.IsVisible = True
        'myPane.XAxis.MajorGrid.DashOff = 0
        'myPane.XAxis.MinorGrid.DashOff = 0
        'myPane.YAxis.MinorGrid.IsVisible = True
        myPane.YAxis.MajorGrid.IsVisible = True
        'myPane.YAxis.MajorGrid.DashOff = 0
        'myPane.YAxis.MinorGrid.DashOff = 0
    End Sub

    Private Sub showTCPData1()


        Dim myPane As ZedGraph.GraphPane = ZedGraphControl7.GraphPane


        '去除之前的数据，并且记得要用axischange()和refresh()重画
        ZedGraphControl7.GraphPane.CurveList.Clear()
        ZedGraphControl7.GraphPane.GraphObjList.Clear()
        ZedGraphControl7.AxisChange()
        ZedGraphControl7.Refresh()


        '设置图标标题和x、y轴标题
        'myPane.Title.Text = "A1桥"
        'myPane.XAxis.Title.Text = "时间(s)"
        'myPane.YAxis.Title.Text = "加速度(m/s^2)"

        myPane.Title.Text = ""
        myPane.XAxis.Title.Text = ""
        myPane.YAxis.Title.Text = ""

        '更改标题的字体
        'Dim myFont As ZedGraph.FontSpec = New ZedGraph.FontSpec("Arial", 20, Color.Red, False, False, False)
        'myPane.Title.FontSpec = myFont
        'myPane.XAxis.Title.FontSpec = myFont
        'myPane.YAxis.Title.FontSpec = myFont
        Dim myFont As ZedGraph.FontSpec = New ZedGraph.FontSpec("黑体", 50, Color.Red, False, False, False)
        'myPane.Title.FontSpec = myFont
        'myPane.XAxis.Title.FontSpec = myFont
        'myPane.YAxis.Title.FontSpec = myFont

        '生成数据
        Dim list1 As ZedGraph.PointPairList = New ZedGraph.PointPairList()
        Dim warnList As New ZedGraph.PointPairList '设置预警红线
        Dim warnList2 As New ZedGraph.PointPairList

        Dim myCurve As ZedGraph.LineItem = myPane.AddCurve("", list1, Color.Blue, ZedGraph.SymbolType.None)
        Dim warnCurve As ZedGraph.LineItem = myPane.AddCurve("", warnList, Color.Red, ZedGraph.SymbolType.Circle) '添加预警线
        Dim warnCurve2 As ZedGraph.LineItem = myPane.AddCurve("", warnList2, Color.Red, ZedGraph.SymbolType.Circle) '添加预警线


        Dim i As Integer = 0 '循环变量

        Dim x As Double
        x = 0
        'Dim y1 As Single

        myPane.XAxis.Scale.Max = 5
        myPane.XAxis.Scale.Min = 0

        Dim quenelength As Integer = 0
        While True
            While dataquene1.Count > 100
                Try

                    quenelength = dataquene1.Count
                    Dim datanow As Single
                    For j = 0 To dataquene1.Count - 1
                        datanow = dataquene1.Dequeue()
                        If j Mod 2 = 0 Then
                            list1.Add(x, datanow)
                            If i Mod 10 = 0 Then
                                'warnList.Add(x, 0.04)
                                'warnList2.Add(x, -0.04)
                            End If

                            '控制主界面音量条开关
                            If (i Mod 80 = 0) Then
                                Dim dataAbs As Single = System.Math.Abs(datanow)

                                '以下为开灯
                                If dataAbs > 0 Then

                                    Label7.BackColor = Color.LimeGreen
                                    Label8.BackColor = Color.LimeGreen
                                End If

                                If dataAbs > 0.001 Then
                                    Label9.BackColor = Color.LimeGreen
                                End If

                                If dataAbs > 0.0021 Then
                                    Label10.BackColor = Color.LimeGreen
                                End If

                                If dataAbs > 0.0031 Then
                                    Label11.BackColor = Color.LimeGreen
                                End If

                                If dataAbs > 0.0042 Then
                                    Label12.BackColor = Color.LimeGreen
                                End If

                                If dataAbs > 0.0053 Then
                                    Label13.BackColor = Color.LimeGreen
                                End If

                                If dataAbs > 0.0065 Then

                                    Label14.BackColor = Color.LimeGreen
                                End If

                                If dataAbs > 0.0379 Then
                                    Label15.BackColor = Color.LimeGreen
                                End If

                                If dataAbs > 0.0399 Then

                                    Label16.BackColor = Color.Gold
                                End If

                                If dataAbs > 0.0432 Then
                                    Label17.BackColor = Color.Gold
                                End If

                                If dataAbs > 0.0498 Then
                                    Label18.BackColor = Color.Gold
                                End If

                                If dataAbs > 0.4385 Then
                                    'warning1 = True
                                    'Label19.BackColor = Color.Red
                                End If

                                '以下为关灯

                                If dataAbs < 0.0001 Then
                                    Label7.BackColor = Color.Gray
                                    Label8.BackColor = Color.Gray
                                End If

                                If dataAbs < 0.001 Then
                                    Label9.BackColor = Color.Gray
                                End If

                                If dataAbs < 0.0021 Then
                                    Label10.BackColor = Color.Gray
                                End If

                                If dataAbs < 0.0031 Then
                                    Label11.BackColor = Color.Gray
                                End If

                                If dataAbs < 0.0042 Then
                                    Label12.BackColor = Color.Gray
                                End If

                                If dataAbs < 0.0053 Then
                                    Label13.BackColor = Color.Gray
                                End If

                                If dataAbs < 0.0065 Then
                                    Label14.BackColor = Color.Gray
                                End If

                                If dataAbs < 0.0379 Then
                                    Label15.BackColor = Color.Gray
                                End If

                                If dataAbs < 0.0399 Then

                                    Label16.BackColor = Color.Gray
                                End If

                                If dataAbs < 0.0432 Then
                                    Label17.BackColor = Color.Gray
                                End If

                                If dataAbs < 0.0498 Then
                                    Label18.BackColor = Color.Gray
                                End If

                                If dataAbs < 0.4385 Then

                                    Label19.BackColor = Color.Gray
                                End If



                                '预警灯开关
                                If Not warning1 Then
                                    If dataAbs < 0.3399 Then
                                        PictureBox6.BackgroundImage = Image.FromFile("mainimage\hk\light1.png")
                                        PictureBox5.BackgroundImage = Image.FromFile("mainimage\hk\light5.png")
                                        PictureBox3.BackgroundImage = Image.FromFile("mainimage\hk\light6.png")
                                    ElseIf dataAbs < 4.385 Then
                                        PictureBox6.BackgroundImage = Image.FromFile("mainimage\hk\light4.png")
                                        PictureBox5.BackgroundImage = Image.FromFile("mainimage\hk\light2.png")
                                        PictureBox3.BackgroundImage = Image.FromFile("mainimage\hk\light6.png")
                                    ElseIf dataAbs > 100.4385 Then
                                        PictureBox6.BackgroundImage = Image.FromFile("mainimage\hk\light4.png")
                                        PictureBox5.BackgroundImage = Image.FromFile("mainimage\hk\light5.png")
                                        PictureBox3.BackgroundImage = Image.FromFile("mainimage\hk\light3.png")
                                    End If
                                Else
                                    If Not thwarn1.IsAlive Then
                                        thwarn1 = New System.Threading.Thread(AddressOf warnLight1)
                                        thwarn1.Start()
                                    End If
                                    'Try
                                    '    thwarn1.Abort()
                                    'Catch ex As Exception

                                    'End Try
                                    'thwarn1 = New System.Threading.Thread(AddressOf warnLight1)
                                    'thwarn1.Start()
                                End If


                            End If

                            If i Mod 50 = 0 Then
                                ZedGraphControl7.AxisChange()
                                ZedGraphControl7.Refresh()
                                System.Threading.Thread.Sleep(300)

                            End If

                            x = x + 0.01
                            If x > 5 Then
                                ZedGraphControl7.AxisChange()
                                ZedGraphControl7.Refresh()
                                x = 0
                                list1.Clear()
                                warnList.Clear()
                                warnList2.Clear()
                            End If

                            i = i + 1
                            'System.Threading.Thread.Sleep(1)
                        End If

                    Next




                Catch ex As Exception

                End Try
            End While
            Thread.Sleep(100)
        End While

    End Sub



    'Private Sub showTCPData11()


    '    Dim myPane As ZedGraph.GraphPane = ZedGraphControl7.GraphPane


    '    '去除之前的数据，并且记得要用axischange()和refresh()重画
    '    ZedGraphControl7.GraphPane.CurveList.Clear()
    '    ZedGraphControl7.GraphPane.GraphObjList.Clear()
    '    ZedGraphControl7.AxisChange()
    '    ZedGraphControl7.Refresh()


    '    '设置图标标题和x、y轴标题
    '    'myPane.Title.Text = "A1桥"
    '    'myPane.XAxis.Title.Text = "时间(s)"
    '    'myPane.YAxis.Title.Text = "加速度(m/s^2)"

    '    myPane.Title.Text = ""
    '    myPane.XAxis.Title.Text = ""
    '    myPane.YAxis.Title.Text = ""

    '    '更改标题的字体
    '    'Dim myFont As ZedGraph.FontSpec = New ZedGraph.FontSpec("Arial", 20, Color.Red, False, False, False)
    '    'myPane.Title.FontSpec = myFont
    '    'myPane.XAxis.Title.FontSpec = myFont
    '    'myPane.YAxis.Title.FontSpec = myFont
    '    Dim myFont As ZedGraph.FontSpec = New ZedGraph.FontSpec("黑体", 50, Color.Red, False, False, False)
    '    'myPane.Title.FontSpec = myFont
    '    'myPane.XAxis.Title.FontSpec = myFont
    '    'myPane.YAxis.Title.FontSpec = myFont

    '    '生成数据
    '    Dim list1 As ZedGraph.PointPairList = New ZedGraph.PointPairList()
    '    Dim warnList As New ZedGraph.PointPairList '设置预警红线
    '    Dim warnList2 As New ZedGraph.PointPairList

    '    Dim myCurve As ZedGraph.LineItem = myPane.AddCurve("", list1, Color.Blue, ZedGraph.SymbolType.None)
    '    Dim warnCurve As ZedGraph.LineItem = myPane.AddCurve("", warnList, Color.Red, ZedGraph.SymbolType.Circle) '添加预警线
    '    Dim warnCurve2 As ZedGraph.LineItem = myPane.AddCurve("", warnList2, Color.Red, ZedGraph.SymbolType.Circle) '添加预警线


    '    Dim i As Integer = 0 '循环变量

    '    Dim x As Double
    '    x = 0
    '    'Dim y1 As Single

    '    myPane.XAxis.Scale.Max = 5
    '    myPane.XAxis.Scale.Min = 0

    '    Dim quenelength As Integer = 0
    '    While True
    '        While dataquene1.Count > 100
    '            quenelength = dataquene1.Count
    '            Dim datanow As Single
    '            For j = 0 To dataquene1.Count - 1
    '                dataquene1.TryDequeue(datanow)
    '                If j Mod 1 = 0 Then
    '                    list1.Add(x, datanow)
    '                    If i Mod 10 = 0 Then
    '                        'warnList.Add(x, 0.04)
    '                        'warnList2.Add(x, -0.04)
    '                    End If

    '                    '控制主界面音量条开关
    '                    If (i Mod 80 = 0) Then
    '                        Dim dataAbs As Single = System.Math.Abs(datanow)

    '                        '以下为开灯
    '                        If dataAbs > 0 Then

    '                            Label7.BackColor = Color.LimeGreen
    '                            Label8.BackColor = Color.LimeGreen
    '                        End If

    '                        If dataAbs > 0.001 Then
    '                            Label9.BackColor = Color.LimeGreen
    '                        End If

    '                        If dataAbs > 0.0021 Then
    '                            Label10.BackColor = Color.LimeGreen
    '                        End If

    '                        If dataAbs > 0.0031 Then
    '                            Label11.BackColor = Color.LimeGreen
    '                        End If

    '                        If dataAbs > 0.0042 Then
    '                            Label12.BackColor = Color.LimeGreen
    '                        End If

    '                        If dataAbs > 0.0053 Then
    '                            Label13.BackColor = Color.LimeGreen
    '                        End If

    '                        If dataAbs > 0.0065 Then

    '                            Label14.BackColor = Color.LimeGreen
    '                        End If

    '                        If dataAbs > 0.0079 Then
    '                            Label15.BackColor = Color.LimeGreen
    '                        End If

    '                        If dataAbs > 0.0099 Then

    '                            Label16.BackColor = Color.Gold
    '                        End If

    '                        If dataAbs > 0.0132 Then
    '                            Label17.BackColor = Color.Gold
    '                        End If

    '                        If dataAbs > 0.0198 Then
    '                            Label18.BackColor = Color.Gold
    '                        End If

    '                        If dataAbs > 0.4385 Then
    '                            warning1 = True
    '                            Label19.BackColor = Color.Red
    '                        End If

    '                        '以下为关灯

    '                        If dataAbs < 0.0001 Then
    '                            Label7.BackColor = Color.Gray
    '                            Label8.BackColor = Color.Gray
    '                        End If

    '                        If dataAbs < 0.001 Then
    '                            Label9.BackColor = Color.Gray
    '                        End If

    '                        If dataAbs < 0.0021 Then
    '                            Label10.BackColor = Color.Gray
    '                        End If

    '                        If dataAbs < 0.0031 Then
    '                            Label11.BackColor = Color.Gray
    '                        End If

    '                        If dataAbs < 0.0042 Then
    '                            Label12.BackColor = Color.Gray
    '                        End If

    '                        If dataAbs < 0.0053 Then
    '                            Label13.BackColor = Color.Gray
    '                        End If

    '                        If dataAbs < 0.0065 Then
    '                            Label14.BackColor = Color.Gray
    '                        End If

    '                        If dataAbs < 0.0079 Then
    '                            Label15.BackColor = Color.Gray
    '                        End If

    '                        If dataAbs < 0.0099 Then

    '                            Label16.BackColor = Color.Gray
    '                        End If

    '                        If dataAbs < 0.0132 Then
    '                            Label17.BackColor = Color.Gray
    '                        End If

    '                        If dataAbs < 0.0198 Then
    '                            Label18.BackColor = Color.Gray
    '                        End If

    '                        If dataAbs < 0.4385 Then

    '                            Label19.BackColor = Color.Gray
    '                        End If



    '                        '预警灯开关
    '                        If Not warning1 Then
    '                            If dataAbs < 0.0099 Then
    '                                PictureBox6.BackgroundImage = Image.FromFile("mainimage\hk\light1.png")
    '                                PictureBox5.BackgroundImage = Image.FromFile("mainimage\hk\light5.png")
    '                                PictureBox3.BackgroundImage = Image.FromFile("mainimage\hk\light6.png")
    '                            ElseIf dataAbs < 0.4385 Then
    '                                PictureBox6.BackgroundImage = Image.FromFile("mainimage\hk\light4.png")
    '                                PictureBox5.BackgroundImage = Image.FromFile("mainimage\hk\light2.png")
    '                                PictureBox3.BackgroundImage = Image.FromFile("mainimage\hk\light6.png")
    '                            ElseIf dataAbs > 0.4385 Then
    '                                PictureBox6.BackgroundImage = Image.FromFile("mainimage\hk\light4.png")
    '                                PictureBox5.BackgroundImage = Image.FromFile("mainimage\hk\light5.png")
    '                                PictureBox3.BackgroundImage = Image.FromFile("mainimage\hk\light3.png")
    '                            End If
    '                        Else
    '                            If Not thwarn1.IsAlive Then
    '                                thwarn1 = New System.Threading.Thread(AddressOf warnLight1)
    '                                thwarn1.Start()
    '                            End If
    '                            'Try
    '                            '    thwarn1.Abort()
    '                            'Catch ex As Exception

    '                            'End Try
    '                            'thwarn1 = New System.Threading.Thread(AddressOf warnLight1)
    '                            'thwarn1.Start()
    '                        End If


    '                    End If

    '                    If i Mod 100 = 0 Then
    '                        ZedGraphControl7.AxisChange()
    '                        ZedGraphControl7.Refresh()

    '                    End If

    '                    x = x + 0.01
    '                    If x > 5 Then
    '                        x = 0
    '                        list1.Clear()
    '                        warnList.Clear()
    '                        warnList2.Clear()
    '                    End If

    '                    i = i + 1
    '                    'System.Threading.Thread.Sleep(30)
    '                End If

    '            Next





    '        End While
    '        Thread.Sleep(1000)
    '    End While

    'End Sub

    Private Sub showTCPData2()





        Dim myPane As ZedGraph.GraphPane = ZedGraphControl1.GraphPane


        '去除之前的数据，并且记得要用axischange()和refresh()重画
        ZedGraphControl1.GraphPane.CurveList.Clear()
        ZedGraphControl1.GraphPane.GraphObjList.Clear()
        ZedGraphControl1.AxisChange()
        ZedGraphControl1.Refresh()




        myPane.Title.Text = ""
        myPane.XAxis.Title.Text = ""
        myPane.YAxis.Title.Text = ""

        '更改标题的字体
        'Dim myFont As ZedGraph.FontSpec = New ZedGraph.FontSpec("Arial", 20, Color.Red, False, False, False)
        'myPane.Title.FontSpec = myFont
        'myPane.XAxis.Title.FontSpec = myFont
        'myPane.YAxis.Title.FontSpec = myFont
        Dim myFont As ZedGraph.FontSpec = New ZedGraph.FontSpec("黑体", 50, Color.Red, False, False, False)
        'myPane.Title.FontSpec = myFont
        'myPane.XAxis.Title.FontSpec = myFont
        'myPane.YAxis.Title.FontSpec = myFont

        '生成数据
        Dim list1 As ZedGraph.PointPairList = New ZedGraph.PointPairList()
        Dim warnList As New ZedGraph.PointPairList '设置预警红线
        Dim warnList2 As New ZedGraph.PointPairList

        Dim myCurve As ZedGraph.LineItem = myPane.AddCurve("", list1, Color.Blue, ZedGraph.SymbolType.None)
        Dim warnCurve As ZedGraph.LineItem = myPane.AddCurve("", warnList, Color.Red, ZedGraph.SymbolType.Circle) '添加预警线
        Dim warnCurve2 As ZedGraph.LineItem = myPane.AddCurve("", warnList2, Color.Red, ZedGraph.SymbolType.Circle) '添加预警线

        Dim i As Integer '循环变量

        Dim x As Double
        x = 0
        'Dim y1 As Single

        'myPane.XAxis.Scale.Max = 60
        myPane.XAxis.Scale.Max = 5

        myPane.XAxis.Scale.Min = 0

        Dim quenelength As Integer = 0
        While True
            While dataquene2.Count > 100
                Try



                    quenelength = dataquene2.Count
                    Dim datanow As Single
                    For j = 0 To dataquene2.Count - 1
                        datanow = dataquene2.Dequeue()
                        If j Mod 2 = 0 Then
                            list1.Add(x, datanow)
                            If i Mod 10 = 0 Then
                                'warnList.Add(x, 0.04)
                                'warnList2.Add(x, -0.04)
                            End If




                            '控制主界面音量条开关
                            If i Mod 80 = 0 Then
                                Dim dataAbs As Single = System.Math.Abs(datanow)

                                '以下为开灯
                                If dataAbs > 0.001 Then
                                    Label37.BackColor = Color.LimeGreen
                                    Label36.BackColor = Color.LimeGreen
                                End If

                                If dataAbs > 0.0021 Then
                                    Label35.BackColor = Color.LimeGreen
                                End If

                                If dataAbs > 0.0031 Then
                                    Label34.BackColor = Color.LimeGreen
                                End If

                                If dataAbs > 0.0042 Then
                                    Label33.BackColor = Color.LimeGreen
                                End If

                                If dataAbs > 0.0053 Then
                                    Label32.BackColor = Color.LimeGreen
                                End If

                                If dataAbs > 0.0065 Then
                                    Label31.BackColor = Color.LimeGreen
                                End If

                                If dataAbs > 0.0079 Then
                                    Label30.BackColor = Color.LimeGreen
                                End If

                                If dataAbs > 0.0099 Then
                                    Label29.BackColor = Color.LimeGreen
                                End If

                                If dataAbs > 0.132 Then

                                    Label28.BackColor = Color.Gold
                                End If

                                If dataAbs > 0.198 Then

                                    Label27.BackColor = Color.Gold
                                End If

                                If dataAbs > 4.385 Then
                                    'warning2 = True '演示
                                    Label26.BackColor = Color.Gold
                                    'Label25.BackColor = Color.Red '演示
                                End If

                                'If dataAbs > 0.4385 Then
                                'warning2 = True
                                'Label25.BackColor = Color.Red
                                'End If

                                '以下为关灯

                                If dataAbs < 0.001 Then
                                    Label37.BackColor = Color.Gray
                                    Label36.BackColor = Color.Gray
                                End If

                                If dataAbs < 0.0021 Then
                                    Label35.BackColor = Color.Gray
                                End If

                                If dataAbs < 0.0031 Then
                                    Label34.BackColor = Color.Gray
                                End If

                                If dataAbs < 0.0042 Then
                                    Label33.BackColor = Color.Gray
                                End If

                                If dataAbs < 0.0053 Then
                                    Label32.BackColor = Color.Gray
                                End If

                                If dataAbs < 0.0065 Then
                                    Label31.BackColor = Color.Gray
                                End If

                                If dataAbs < 0.0079 Then
                                    Label30.BackColor = Color.Gray
                                End If

                                If dataAbs < 0.0099 Then
                                    Label29.BackColor = Color.Gray
                                End If

                                If dataAbs < 0.132 Then
                                    Label28.BackColor = Color.Gray
                                End If

                                If dataAbs < 0.198 Then
                                    Label27.BackColor = Color.Gray
                                End If

                                If dataAbs < 4.385 Then
                                    Label26.BackColor = Color.Gray
                                    Label25.BackColor = Color.Gray '演示
                                End If

                                If dataAbs < 0.4385 Then
                                    'Label25.BackColor = Color.Gray
                                End If

                                '预警灯开关
                                If Not warning2 Then
                                    If dataAbs < 0.3399 Then
                                        PictureBox9.BackgroundImage = Image.FromFile("mainimage\hk\light1.png")
                                        PictureBox8.BackgroundImage = Image.FromFile("mainimage\hk\light5.png")
                                        PictureBox7.BackgroundImage = Image.FromFile("mainimage\hk\light6.png")
                                    ElseIf dataAbs < 4.385 Then
                                        PictureBox9.BackgroundImage = Image.FromFile("mainimage\hk\light4.png")
                                        PictureBox8.BackgroundImage = Image.FromFile("mainimage\hk\light2.png")
                                        PictureBox7.BackgroundImage = Image.FromFile("mainimage\hk\light6.png")
                                    ElseIf dataAbs > 100.4385 Then
                                        PictureBox9.BackgroundImage = Image.FromFile("mainimage\hk\light4.png")
                                        PictureBox8.BackgroundImage = Image.FromFile("mainimage\hk\light5.png")
                                        'PictureBox7.BackgroundImage = Image.FromFile("mainimage\hk\light3.png")
                                    End If
                                Else
                                    If Not thwarn2.IsAlive Then
                                        thwarn2 = New System.Threading.Thread(AddressOf warnLight2)
                                        thwarn2.Start()
                                    End If
                                End If


                            End If

                            If i Mod 50 = 0 Then

                                ZedGraphControl1.AxisChange()
                                ZedGraphControl1.Refresh()
                                System.Threading.Thread.Sleep(300)


                            End If



                            x = x + 0.01
                            'If x > 60 Then
                            If x > 5 Then
                                ZedGraphControl7.AxisChange()
                                ZedGraphControl7.Refresh()
                                x = 0
                                list1.Clear()
                                warnList.Clear()
                                warnList2.Clear()
                            End If
                            i = i + 1

                            'System.Threading.Thread.Sleep(1)
                        End If
                    Next
                Catch ex As Exception

                End Try
            End While
            Thread.Sleep(100)
        End While
    End Sub

    Private Sub showTCPData3()



        Dim myPane As ZedGraph.GraphPane = ZedGraphControl2.GraphPane


        '去除之前的数据，并且记得要用axischange()和refresh()重画
        ZedGraphControl2.GraphPane.CurveList.Clear()
        ZedGraphControl2.GraphPane.GraphObjList.Clear()
        ZedGraphControl2.AxisChange()
        ZedGraphControl2.Refresh()


        '设置图标标题和x、y轴标题
        'myPane.Title.Text = "A1桥"
        'myPane.XAxis.Title.Text = "时间(s)"
        'myPane.YAxis.Title.Text = "加速度(m/s^2)"

        myPane.Title.Text = ""
        myPane.XAxis.Title.Text = ""
        myPane.YAxis.Title.Text = ""

        '更改标题的字体
        'Dim myFont As ZedGraph.FontSpec = New ZedGraph.FontSpec("Arial", 20, Color.Red, False, False, False)
        'myPane.Title.FontSpec = myFont
        'myPane.XAxis.Title.FontSpec = myFont
        'myPane.YAxis.Title.FontSpec = myFont
        Dim myFont As ZedGraph.FontSpec = New ZedGraph.FontSpec("黑体", 50, Color.Red, False, False, False)
        'myPane.Title.FontSpec = myFont
        'myPane.XAxis.Title.FontSpec = myFont
        'myPane.YAxis.Title.FontSpec = myFont

        '生成数据
        Dim list1 As ZedGraph.PointPairList = New ZedGraph.PointPairList()
        Dim warnList As New ZedGraph.PointPairList '设置预警红线
        Dim warnList2 As New ZedGraph.PointPairList

        Dim myCurve As ZedGraph.LineItem = myPane.AddCurve("", list1, Color.Blue, ZedGraph.SymbolType.None)
        Dim warnCurve As ZedGraph.LineItem = myPane.AddCurve("", warnList, Color.Red, ZedGraph.SymbolType.Circle) '添加预警线
        Dim warnCurve2 As ZedGraph.LineItem = myPane.AddCurve("", warnList2, Color.Red, ZedGraph.SymbolType.Circle) '添加预警线

        Dim i As Integer '循环变量

        Dim x As Double
        x = 0
        'Dim y1 As Single

        'myPane.XAxis.Scale.Max = 60
        myPane.XAxis.Scale.Max = 5

        myPane.XAxis.Scale.Min = 0

        Dim quenelength As Integer = 0
        While True
            While dataquene3.Count > 100
                Try
                    quenelength = dataquene3.Count
                    Dim datanow As Single
                    For j = 0 To dataquene3.Count - 1
                        datanow = dataquene3.Dequeue()
                        If j Mod 2 = 0 Then
                            list1.Add(x, datanow)
                            If i Mod 10 = 0 Then
                                'warnList.Add(x, 0.04)
                                'warnList2.Add(x, -0.04)
                            End If



                            '控制主界面音量条开关
                            If i Mod 80 = 0 Then
                                Dim dataAbs As Single = System.Math.Abs(datanow)

                                '以下为开灯
                                If dataAbs > 0.0001 Then
                                    Label54.BackColor = Color.LimeGreen
                                    Label53.BackColor = Color.LimeGreen
                                End If

                                If dataAbs > 0.001 Then
                                    Label52.BackColor = Color.LimeGreen
                                End If

                                If dataAbs > 0.0021 Then
                                    Label51.BackColor = Color.LimeGreen
                                End If

                                If dataAbs > 0.0031 Then
                                    Label50.BackColor = Color.LimeGreen
                                End If

                                If dataAbs > 0.042 Then

                                    Label49.BackColor = Color.LimeGreen
                                End If

                                If dataAbs > 0.053 Then
                                    Label48.BackColor = Color.LimeGreen
                                End If

                                If dataAbs > 0.065 Then

                                    Label47.BackColor = Color.LimeGreen
                                End If

                                If dataAbs > 0.079 Then
                                    Label46.BackColor = Color.LimeGreen
                                End If

                                If dataAbs > 0.99 Then
                                    Label45.BackColor = Color.Gold
                                End If

                                If dataAbs > 1.32 Then
                                    Label44.BackColor = Color.Gold
                                End If

                                If dataAbs > 1.98 Then
                                    Label43.BackColor = Color.Gold
                                End If

                                If dataAbs > 4.385 Then
                                    'warning3 = True
                                    'Label42.BackColor = Color.Red
                                End If

                                '以下为关灯

                                If dataAbs < 0.0001 Then
                                    Label54.BackColor = Color.Gray
                                    Label53.BackColor = Color.Gray
                                End If

                                If dataAbs < 0.001 Then
                                    Label52.BackColor = Color.Gray
                                End If

                                If dataAbs < 0.0021 Then
                                    Label51.BackColor = Color.Gray
                                End If

                                If dataAbs < 0.0031 Then
                                    Label50.BackColor = Color.Gray
                                End If

                                If dataAbs < 0.042 Then
                                    Label49.BackColor = Color.Gray
                                End If

                                If dataAbs < 0.053 Then
                                    Label48.BackColor = Color.Gray
                                End If

                                If dataAbs < 0.065 Then
                                    Label47.BackColor = Color.Gray
                                End If

                                If dataAbs < 0.079 Then
                                    Label46.BackColor = Color.Gray
                                End If

                                If dataAbs < 0.99 Then
                                    Label45.BackColor = Color.Gray
                                End If

                                If dataAbs < 1.32 Then
                                    Label44.BackColor = Color.Gray
                                End If

                                If dataAbs < 1.98 Then
                                    Label43.BackColor = Color.Gray
                                End If

                                If dataAbs < 4.385 Then
                                    Label42.BackColor = Color.Gray
                                End If

                                '预警灯开关
                                If Not warning3 Then
                                    If dataAbs < 0.3399 Then
                                        PictureBox12.BackgroundImage = Image.FromFile("mainimage\hk\light1.png")
                                        PictureBox11.BackgroundImage = Image.FromFile("mainimage\hk\light5.png")
                                        PictureBox10.BackgroundImage = Image.FromFile("mainimage\hk\light6.png")
                                    ElseIf dataAbs < 4.385 Then
                                        PictureBox12.BackgroundImage = Image.FromFile("mainimage\hk\light4.png")
                                        PictureBox11.BackgroundImage = Image.FromFile("mainimage\hk\light2.png")
                                        PictureBox10.BackgroundImage = Image.FromFile("mainimage\hk\light6.png")
                                    ElseIf dataAbs > 100.4385 Then
                                        PictureBox12.BackgroundImage = Image.FromFile("mainimage\hk\light4.png")
                                        PictureBox11.BackgroundImage = Image.FromFile("mainimage\hk\light5.png")
                                        'PictureBox10.BackgroundImage = Image.FromFile("mainimage\hk\light3.png")
                                    End If
                                Else
                                    If Not thwarn3.IsAlive Then
                                        thwarn3 = New System.Threading.Thread(AddressOf warnLight3)
                                        thwarn3.Start()
                                    End If
                                End If


                            End If

                            If i Mod 50 = 0 Then
                                ZedGraphControl2.AxisChange()
                                ZedGraphControl2.Refresh()
                                System.Threading.Thread.Sleep(300)
                            End If

                            x = x + 0.01
                            'If x > 60 Then
                            If x > 5 Then
                                ZedGraphControl7.AxisChange()
                                ZedGraphControl7.Refresh()
                                x = 0
                                list1.Clear()
                                warnList.Clear()
                                warnList2.Clear()
                            End If

                            i = i + 1

                            'System.Threading.Thread.Sleep(30)
                        End If
                    Next
                Catch ex As Exception

                End Try
            End While
            Thread.Sleep(100)
        End While
    End Sub






    Private Sub showDemoData1()


        While True
            Dim dirwhole = "Storage Data\shenzhen\test3.bin"
            Label3.Text = "加载文件..."
            isdataloaded = False
            Dim dataShenzhen1(370000) As Single



            Try
                Dim fs2 As New System.IO.FileStream(dirwhole, IO.FileMode.Open)
                'Dim fs3 As New System.IO.FileStream("F:\byte2txt.txt", IO.FileMode.Create)
                Dim bw2 As New System.IO.BinaryReader(fs2)
                Dim numnow(3) As Byte
                'MsgBox(fs2.Length)
                For j = 0 To (fs2.Length / 4 - 1)
                    For k = 0 To 3
                        numnow(k) = fs2.ReadByte
                    Next
                    dataShenzhen1(j) = BitConverter.ToSingle(numnow, 0)
                Next

                datalen1 = fs2.Length / 4 - 1 'datalen的值最终为数组的最大下标，而数据的数量为datalen+1


                'For j = 0 To 100
                '    For i = 0 To 3
                '        numnow(i) = fs2.ReadByte
                '    Next
                '    MsgBox(BitConverter.ToSingle(numnow, 0))
                'Next

                bw2.Close()
                fs2.Close()

                isdataloaded = True
                Label3.Text = "就绪"



            Catch ex As Exception
                Label3.Text = "加载失败，文件不存在。请检查日期是否有误！"
            End Try

            'Dim zgc = ZedGraphControl7

            If Not isdataloaded Then Exit Sub

            Dim myPane As ZedGraph.GraphPane = ZedGraphControl7.GraphPane


            '去除之前的数据，并且记得要用axischange()和refresh()重画
            ZedGraphControl7.GraphPane.CurveList.Clear()
            ZedGraphControl7.GraphPane.GraphObjList.Clear()
            ZedGraphControl7.AxisChange()
            ZedGraphControl7.Refresh()


            '设置图标标题和x、y轴标题
            'myPane.Title.Text = "A1桥"
            'myPane.XAxis.Title.Text = "时间(s)"
            'myPane.YAxis.Title.Text = "加速度(m/s^2)"

            myPane.Title.Text = ""
            myPane.XAxis.Title.Text = ""
            myPane.YAxis.Title.Text = ""

            '更改标题的字体
            'Dim myFont As ZedGraph.FontSpec = New ZedGraph.FontSpec("Arial", 20, Color.Red, False, False, False)
            'myPane.Title.FontSpec = myFont
            'myPane.XAxis.Title.FontSpec = myFont
            'myPane.YAxis.Title.FontSpec = myFont
            Dim myFont As ZedGraph.FontSpec = New ZedGraph.FontSpec("黑体", 50, Color.Red, False, False, False)
            myPane.Title.FontSpec = myFont
            myPane.XAxis.Title.FontSpec = myFont
            myPane.YAxis.Title.FontSpec = myFont

            '生成数据
            Dim list1 As ZedGraph.PointPairList = New ZedGraph.PointPairList()
            Dim warnList As New ZedGraph.PointPairList '设置预警红线
            Dim warnList2 As New ZedGraph.PointPairList

            Dim myCurve As ZedGraph.LineItem = myPane.AddCurve("", list1, Color.Blue, ZedGraph.SymbolType.None)
            Dim warnCurve As ZedGraph.LineItem = myPane.AddCurve("", warnList, Color.Red, ZedGraph.SymbolType.Circle) '添加预警线
            Dim warnCurve2 As ZedGraph.LineItem = myPane.AddCurve("", warnList2, Color.Red, ZedGraph.SymbolType.Circle) '添加预警线


            Dim i As Integer '循环变量

            Dim x As Double
            x = 0
            Dim y1 As Single

            myPane.XAxis.Scale.Max = 60
            myPane.XAxis.Scale.Min = 0

            For i = 0 To datalen1 Step 3
                'x = CSng(i) / CSng(100) Mod CSng(60)

                'Dim x As Double = CSng(i) / CSng(100) Mod CSng(60)
                'Dim y1 As Single
                y1 = dataShenzhen1(i)
                list1.Add(x, y1)
                If i Mod 10 = 0 Then
                    warnList.Add(x, 0.04)
                    warnList2.Add(x, -0.04)
                End If


                'warnList.Add(x, 20) '预警线的y值设置为20

                'myPane.XAxis.Scale.Max = 60
                'myPane.XAxis.Scale.Min = 0



                'If list1.Count > 2000 Then
                '    list1.Clear()
                'End If

                '控制主界面音量条开关
                If (i Mod 80 = 0) Then
                    Dim dataAbs As Single = System.Math.Abs(dataShenzhen1(i))

                    '以下为开灯
                    If dataAbs > 0 Then

                        Label7.BackColor = Color.LimeGreen
                        Label8.BackColor = Color.LimeGreen
                    End If

                    If dataAbs > 0.001 Then
                        Label9.BackColor = Color.LimeGreen
                    End If

                    If dataAbs > 0.0021 Then
                        Label10.BackColor = Color.LimeGreen
                    End If

                    If dataAbs > 0.0031 Then
                        Label11.BackColor = Color.LimeGreen
                    End If

                    If dataAbs > 0.0042 Then
                        Label12.BackColor = Color.LimeGreen
                    End If

                    If dataAbs > 0.0053 Then
                        Label13.BackColor = Color.LimeGreen
                    End If

                    If dataAbs > 0.0065 Then

                        Label14.BackColor = Color.LimeGreen
                    End If

                    If dataAbs > 0.0079 Then
                        Label15.BackColor = Color.LimeGreen
                    End If

                    If dataAbs > 0.0099 Then

                        Label16.BackColor = Color.Gold
                    End If

                    If dataAbs > 0.0132 Then
                        Label17.BackColor = Color.Gold
                    End If

                    If dataAbs > 0.0198 Then
                        Label18.BackColor = Color.Gold
                    End If

                    If dataAbs > 0.4385 Then
                        warning1 = True
                        Label19.BackColor = Color.Red
                    End If

                    '以下为关灯

                    If dataAbs < 0.0001 Then
                        Label7.BackColor = Color.Gray
                        Label8.BackColor = Color.Gray
                    End If

                    If dataAbs < 0.001 Then
                        Label9.BackColor = Color.Gray
                    End If

                    If dataAbs < 0.0021 Then
                        Label10.BackColor = Color.Gray
                    End If

                    If dataAbs < 0.0031 Then
                        Label11.BackColor = Color.Gray
                    End If

                    If dataAbs < 0.0042 Then
                        Label12.BackColor = Color.Gray
                    End If

                    If dataAbs < 0.0053 Then
                        Label13.BackColor = Color.Gray
                    End If

                    If dataAbs < 0.0065 Then
                        Label14.BackColor = Color.Gray
                    End If

                    If dataAbs < 0.0079 Then
                        Label15.BackColor = Color.Gray
                    End If

                    If dataAbs < 0.0099 Then

                        Label16.BackColor = Color.Gray
                    End If

                    If dataAbs < 0.0132 Then
                        Label17.BackColor = Color.Gray
                    End If

                    If dataAbs < 0.0198 Then
                        Label18.BackColor = Color.Gray
                    End If

                    If dataAbs < 0.4385 Then

                        Label19.BackColor = Color.Gray
                    End If



                    '预警灯开关
                    If Not warning1 Then
                        If dataAbs < 0.0099 Then
                            PictureBox6.BackgroundImage = Image.FromFile("mainimage\hk\light1.png")
                            PictureBox5.BackgroundImage = Image.FromFile("mainimage\hk\light5.png")
                            PictureBox3.BackgroundImage = Image.FromFile("mainimage\hk\light6.png")
                        ElseIf dataAbs < 0.4385 Then
                            PictureBox6.BackgroundImage = Image.FromFile("mainimage\hk\light4.png")
                            PictureBox5.BackgroundImage = Image.FromFile("mainimage\hk\light2.png")
                            PictureBox3.BackgroundImage = Image.FromFile("mainimage\hk\light6.png")
                        ElseIf dataAbs > 0.4385 Then
                            PictureBox6.BackgroundImage = Image.FromFile("mainimage\hk\light4.png")
                            PictureBox5.BackgroundImage = Image.FromFile("mainimage\hk\light5.png")
                            PictureBox3.BackgroundImage = Image.FromFile("mainimage\hk\light3.png")
                        End If
                    Else
                        If Not thwarn1.IsAlive Then
                            thwarn1 = New System.Threading.Thread(AddressOf warnLight1)
                            thwarn1.Start()
                        End If
                        'Try
                        '    thwarn1.Abort()
                        'Catch ex As Exception

                        'End Try
                        'thwarn1 = New System.Threading.Thread(AddressOf warnLight1)
                        'thwarn1.Start()
                    End If


                End If

                If i Mod 10 = 0 Then
                    ZedGraphControl7.AxisChange()
                    ZedGraphControl7.Refresh()

                End If

                x = x + 0.03
                If x > 60 Then
                    x = 0
                    list1.Clear()
                    warnList.Clear()
                    warnList2.Clear()
                End If

                System.Threading.Thread.Sleep(30)

                'System.Windows.Forms.Application.DoEvents()
            Next

            '用list1生产一条曲线，标注是第一个参数中的字符串（此处为空）
            'Dim myCurve As ZedGraph.LineItem = myPane.AddCurve("", list1, Color.Blue, ZedGraph.SymbolType.None)
            'dim warnCurve As ZedGraph.LineItem = myPane.AddCurve("", warnList, Color.Red, ZedGraph.SymbolType.None) '添加预警线

            '填充图表颜色
            'myPane.Fill = New ZedGraph.Fill(Color.White, Color.FromArgb(200, 200, 255), 45.0F)



            'myPane.XAxis.Scale.Max = i / 100 '设置横坐标最大值为数据数量/100（即秒数）
            'myPane.YAxis.Scale.Max = absmax * 1.1 '设置纵坐标最大值为数据绝对值的最大值*1.1
            'myPane.YAxis.Scale.Min = 0 - absmax * 1.1 '设置纵坐标最大值为负的数据绝对值的最大值*1.1

            'myPane.YAxis.Scale.MaxAuto = True
            'myPane.YAxis.Scale.MinAuto = True
            'myPane.XAxis.Scale.MaxAuto = True
            'myPane.XAxis.Scale.MinAuto = True

            '画到zedGraphControl1控件中，此句必加
            'ZedGraphControl7.AxisChange()

            'ZedGraphControl7.Refresh()


        End While
    End Sub

    Private Sub playAlarm()
        'While True
        For i = 0 To 2
            My.Computer.Audio.Play("mainimage\alarm3.wav", AudioPlayMode.WaitToComplete)
        Next

        System.Threading.Thread.Sleep(10000)
        isPlayingAlarm = False
        'End While
        'My.Computer.Audio.Play("mainimage\alarm.wav", AudioPlayMode.BackgroundLoop)
    End Sub

    Private Sub warnLight1()
        PictureBox6.BackgroundImage = Image.FromFile("mainimage\hk\light4.png")
        PictureBox5.BackgroundImage = Image.FromFile("mainimage\hk\light5.png")
        PictureBox3.BackgroundImage = Image.FromFile("mainimage\hk\light3.png")

        '开始预警操作
        If Not isPlayingAlarm Then
            Dim alarmSound As New System.Threading.Thread(AddressOf playAlarm)
            alarmSound.Start()
            isPlayingAlarm = True
        End If

        While True
            PictureBox3.BackgroundImage = Image.FromFile("mainimage\hk\light3.png")
            System.Threading.Thread.Sleep(500)
            PictureBox3.BackgroundImage = Image.FromFile("mainimage\hk\light6.png")
            System.Threading.Thread.Sleep(500)
        End While


        'For i = 0 To 9

        '    PictureBox3.BackgroundImage = Image.FromFile("mainimage\hk\light3.png")
        '    System.Threading.Thread.Sleep(500)
        '    PictureBox3.BackgroundImage = Image.FromFile("mainimage\hk\light6.png")
        '    System.Threading.Thread.Sleep(500)
        'Next
        'warning1 = False
    End Sub

    Private Sub warnLight2()
        PictureBox9.BackgroundImage = Image.FromFile("mainimage\hk\light4.png")
        PictureBox8.BackgroundImage = Image.FromFile("mainimage\hk\light5.png")
        PictureBox7.BackgroundImage = Image.FromFile("mainimage\hk\light3.png")

        '开始预警操作
        If Not isPlayingAlarm Then
            Dim alarmSound As New System.Threading.Thread(AddressOf playAlarm)
            alarmSound.Start()
            isPlayingAlarm = True
        End If
        While True
            PictureBox7.BackgroundImage = Image.FromFile("mainimage\hk\light3.png")
            System.Threading.Thread.Sleep(500)
            PictureBox7.BackgroundImage = Image.FromFile("mainimage\hk\light6.png")
            System.Threading.Thread.Sleep(500)
        End While


        'For i = 0 To 9

        '    PictureBox7.BackgroundImage = Image.FromFile("mainimage\hk\light3.png")
        '    System.Threading.Thread.Sleep(500)
        '    PictureBox7.BackgroundImage = Image.FromFile("mainimage\hk\light6.png")
        '    System.Threading.Thread.Sleep(500)
        'Next
        'warning2 = False
    End Sub

    Private Sub warnLight3()
        PictureBox12.BackgroundImage = Image.FromFile("mainimage\hk\light4.png")
        PictureBox11.BackgroundImage = Image.FromFile("mainimage\hk\light5.png")
        PictureBox10.BackgroundImage = Image.FromFile("mainimage\hk\light3.png")

        '开始预警操作
        If Not isPlayingAlarm Then
            Dim alarmSound As New System.Threading.Thread(AddressOf playAlarm)
            alarmSound.Start()
            isPlayingAlarm = True
        End If
        While True
            PictureBox10.BackgroundImage = Image.FromFile("mainimage\hk\light3.png")
            System.Threading.Thread.Sleep(500)
            PictureBox10.BackgroundImage = Image.FromFile("mainimage\hk\light6.png")
            System.Threading.Thread.Sleep(500)
        End While


        'For i = 0 To 9

        '    PictureBox10.BackgroundImage = Image.FromFile("mainimage\hk\light3.png")
        '    System.Threading.Thread.Sleep(500)
        '    PictureBox10.BackgroundImage = Image.FromFile("mainimage\hk\light6.png")
        '    System.Threading.Thread.Sleep(500)
        'Next
        'warning3 = False
    End Sub


    Private Sub showDemoData2()
        While True
            Dim dirwhole = "Storage Data\shenzhen\test2.bin"
            'Label3.Text = "加载文件..."
            isdataloaded2 = False
            Dim dataShenzhen2(370000) As Single



            Try
                Dim fs2 As New System.IO.FileStream(dirwhole, IO.FileMode.Open)
                'Dim fs3 As New System.IO.FileStream("F:\byte2txt.txt", IO.FileMode.Create)
                Dim bw2 As New System.IO.BinaryReader(fs2)
                Dim numnow(3) As Byte
                'MsgBox(fs2.Length)
                For j = 0 To (fs2.Length / 4 - 1)
                    For k = 0 To 3
                        numnow(k) = fs2.ReadByte
                    Next
                    dataShenzhen2(j) = BitConverter.ToSingle(numnow, 0)
                Next

                datalen2 = fs2.Length / 4 - 1 'datalen的值最终为数组的最大下标，而数据的数量为datalen+1


                'For j = 0 To 100
                '    For i = 0 To 3
                '        numnow(i) = fs2.ReadByte
                '    Next
                '    MsgBox(BitConverter.ToSingle(numnow, 0))
                'Next

                bw2.Close()
                fs2.Close()

                isdataloaded2 = True
                'Label3.Text = "就绪"



            Catch ex As Exception
                'Label3.Text = "加载失败，文件不存在。请检查日期是否有误！"
            End Try

            'Dim zgc = ZedGraphControl7

            If Not isdataloaded2 Then Exit Sub

            Dim myPane As ZedGraph.GraphPane = ZedGraphControl1.GraphPane


            '去除之前的数据，并且记得要用axischange()和refresh()重画
            ZedGraphControl1.GraphPane.CurveList.Clear()
            ZedGraphControl1.GraphPane.GraphObjList.Clear()
            ZedGraphControl1.AxisChange()
            ZedGraphControl1.Refresh()


            '设置图标标题和x、y轴标题
            'myPane.Title.Text = "A1桥"
            'myPane.XAxis.Title.Text = "时间(s)"
            'myPane.YAxis.Title.Text = "加速度(m/s^2)"

            myPane.Title.Text = ""
            myPane.XAxis.Title.Text = ""
            myPane.YAxis.Title.Text = ""

            '更改标题的字体
            'Dim myFont As ZedGraph.FontSpec = New ZedGraph.FontSpec("Arial", 20, Color.Red, False, False, False)
            'myPane.Title.FontSpec = myFont
            'myPane.XAxis.Title.FontSpec = myFont
            'myPane.YAxis.Title.FontSpec = myFont
            Dim myFont As ZedGraph.FontSpec = New ZedGraph.FontSpec("黑体", 50, Color.Red, False, False, False)
            myPane.Title.FontSpec = myFont
            myPane.XAxis.Title.FontSpec = myFont
            myPane.YAxis.Title.FontSpec = myFont

            '生成数据
            Dim list1 As ZedGraph.PointPairList = New ZedGraph.PointPairList()
            Dim warnList As New ZedGraph.PointPairList '设置预警红线
            Dim warnList2 As New ZedGraph.PointPairList

            Dim myCurve As ZedGraph.LineItem = myPane.AddCurve("", list1, Color.Blue, ZedGraph.SymbolType.None)
            Dim warnCurve As ZedGraph.LineItem = myPane.AddCurve("", warnList, Color.Red, ZedGraph.SymbolType.Circle) '添加预警线
            Dim warnCurve2 As ZedGraph.LineItem = myPane.AddCurve("", warnList2, Color.Red, ZedGraph.SymbolType.Circle) '添加预警线

            Dim i As Integer '循环变量

            Dim x As Double
            x = 0
            Dim y1 As Single

            myPane.XAxis.Scale.Max = 60
            myPane.XAxis.Scale.Min = 0

            For i = 0 To datalen2 Step 3
                'Dim x As Double = CSng(i) / CSng(100) Mod CSng(60)
                'x = CSng(i) / CSng(100) Mod CSng(60)
                'Dim y1 As Single
                y1 = dataShenzhen2(i)
                list1.Add(x, y1)

                If i Mod 10 = 0 Then
                    warnList.Add(x, 0.04)
                    warnList2.Add(x, -0.04)
                End If

                'warnList.Add(x, 0.04) '预警线的y值设置为20
                'warnList2.Add(x, -0.04)

                'myPane.XAxis.Scale.Max = 60
                'myPane.XAxis.Scale.Min = 0

                'If list1.Count > 2000 Then
                '    list1.Clear()
                'End If

                '控制主界面音量条开关
                If i Mod 80 = 0 Then
                    Dim dataAbs As Single = System.Math.Abs(dataShenzhen2(i))

                    '以下为开灯
                    If dataAbs > 0.0001 Then
                        Label37.BackColor = Color.LimeGreen
                        Label36.BackColor = Color.LimeGreen
                    End If

                    If dataAbs > 0.001 Then
                        Label35.BackColor = Color.LimeGreen
                    End If

                    If dataAbs > 0.0021 Then
                        Label34.BackColor = Color.LimeGreen
                    End If

                    If dataAbs > 0.0031 Then
                        Label33.BackColor = Color.LimeGreen
                    End If

                    If dataAbs > 0.0042 Then
                        Label32.BackColor = Color.LimeGreen
                    End If

                    If dataAbs > 0.0053 Then
                        Label31.BackColor = Color.LimeGreen
                    End If

                    If dataAbs > 0.0065 Then
                        Label30.BackColor = Color.LimeGreen
                    End If

                    If dataAbs > 0.0079 Then
                        Label29.BackColor = Color.LimeGreen
                    End If

                    If dataAbs > 0.0099 Then

                        Label28.BackColor = Color.Gold
                    End If

                    If dataAbs > 0.0132 Then

                        Label27.BackColor = Color.Gold
                    End If

                    If dataAbs > 0.0198 Then
                        warning2 = True '演示
                        Label26.BackColor = Color.Gold
                        Label25.BackColor = Color.Red '演示
                    End If

                    If dataAbs > 0.4385 Then
                        warning2 = True
                        Label25.BackColor = Color.Red
                    End If

                    '以下为关灯

                    If dataAbs < 0.0001 Then
                        Label37.BackColor = Color.Gray
                        Label36.BackColor = Color.Gray
                    End If

                    If dataAbs < 0.001 Then
                        Label35.BackColor = Color.Gray
                    End If

                    If dataAbs < 0.0021 Then
                        Label34.BackColor = Color.Gray
                    End If

                    If dataAbs < 0.0031 Then
                        Label33.BackColor = Color.Gray
                    End If

                    If dataAbs < 0.0042 Then
                        Label32.BackColor = Color.Gray
                    End If

                    If dataAbs < 0.0053 Then
                        Label31.BackColor = Color.Gray
                    End If

                    If dataAbs < 0.0065 Then
                        Label30.BackColor = Color.Gray
                    End If

                    If dataAbs < 0.0079 Then
                        Label29.BackColor = Color.Gray
                    End If

                    If dataAbs < 0.0099 Then
                        Label28.BackColor = Color.Gray
                    End If

                    If dataAbs < 0.0132 Then
                        Label27.BackColor = Color.Gray
                    End If

                    If dataAbs < 0.0198 Then
                        Label26.BackColor = Color.Gray
                        Label25.BackColor = Color.Gray '演示
                    End If

                    If dataAbs < 0.4385 Then
                        'Label25.BackColor = Color.Gray
                    End If

                    '预警灯开关
                    If Not warning2 Then
                        If dataAbs < 0.0099 Then
                            PictureBox9.BackgroundImage = Image.FromFile("mainimage\hk\light1.png")
                            PictureBox8.BackgroundImage = Image.FromFile("mainimage\hk\light5.png")
                            PictureBox7.BackgroundImage = Image.FromFile("mainimage\hk\light6.png")
                        ElseIf dataAbs < 0.4385 Then
                            PictureBox9.BackgroundImage = Image.FromFile("mainimage\hk\light4.png")
                            PictureBox8.BackgroundImage = Image.FromFile("mainimage\hk\light2.png")
                            PictureBox7.BackgroundImage = Image.FromFile("mainimage\hk\light6.png")
                        ElseIf dataAbs > 0.4385 Then
                            PictureBox9.BackgroundImage = Image.FromFile("mainimage\hk\light4.png")
                            PictureBox8.BackgroundImage = Image.FromFile("mainimage\hk\light5.png")
                            PictureBox7.BackgroundImage = Image.FromFile("mainimage\hk\light3.png")
                        End If
                    Else
                        If Not thwarn2.IsAlive Then
                            thwarn2 = New System.Threading.Thread(AddressOf warnLight2)
                            thwarn2.Start()
                        End If
                    End If


                End If

                If i Mod 10 = 0 Then

                    ZedGraphControl1.AxisChange()
                    ZedGraphControl1.Refresh()


                End If



                x = x + 0.03
                If x > 60 Then
                    x = 0
                    list1.Clear()
                    warnList.Clear()
                    warnList2.Clear()
                End If

                System.Threading.Thread.Sleep(30)

                'System.Windows.Forms.Application.DoEvents()
            Next

            '用list1生产一条曲线，标注是第一个参数中的字符串（此处为空）
            'Dim myCurve As ZedGraph.LineItem = myPane.AddCurve("", list1, Color.Blue, ZedGraph.SymbolType.None)
            'dim warnCurve As ZedGraph.LineItem = myPane.AddCurve("", warnList, Color.Red, ZedGraph.SymbolType.None) '添加预警线

            '填充图表颜色
            'myPane.Fill = New ZedGraph.Fill(Color.White, Color.FromArgb(200, 200, 255), 45.0F)



            'myPane.XAxis.Scale.Max = i / 100 '设置横坐标最大值为数据数量/100（即秒数）
            'myPane.YAxis.Scale.Max = absmax * 1.1 '设置纵坐标最大值为数据绝对值的最大值*1.1
            'myPane.YAxis.Scale.Min = 0 - absmax * 1.1 '设置纵坐标最大值为负的数据绝对值的最大值*1.1

            'myPane.YAxis.Scale.MaxAuto = True
            'myPane.YAxis.Scale.MinAuto = True
            'myPane.XAxis.Scale.MaxAuto = True
            'myPane.XAxis.Scale.MinAuto = True

            '画到zedGraphControl1控件中，此句必加
            'ZedGraphControl1.AxisChange()

            'ZedGraphControl1.Refresh()


        End While
    End Sub

    Private Sub showDemoData3()
        While True
            Dim dirwhole = "Storage Data\shenzhen\test.bin"
            'Label3.Text = "加载文件..."
            isdataloaded3 = False
            Dim dataShenzhen3(370000) As Single



            Try
                Dim fs2 As New System.IO.FileStream(dirwhole, IO.FileMode.Open)
                'Dim fs3 As New System.IO.FileStream("F:\byte2txt.txt", IO.FileMode.Create)
                Dim bw2 As New System.IO.BinaryReader(fs2)
                Dim numnow(3) As Byte
                'MsgBox(fs2.Length)
                For j = 0 To (fs2.Length / 4 - 1)
                    For k = 0 To 3
                        numnow(k) = fs2.ReadByte
                    Next
                    dataShenzhen3(j) = BitConverter.ToSingle(numnow, 0)
                Next

                datalen3 = fs2.Length / 4 - 1 'datalen的值最终为数组的最大下标，而数据的数量为datalen+1


                'For j = 0 To 100
                '    For i = 0 To 3
                '        numnow(i) = fs2.ReadByte
                '    Next
                '    MsgBox(BitConverter.ToSingle(numnow, 0))
                'Next

                bw2.Close()
                fs2.Close()

                isdataloaded3 = True
                'Label3.Text = "就绪"



            Catch ex As Exception
                'Label3.Text = "加载失败，文件不存在。请检查日期是否有误！"
            End Try

            'Dim zgc = ZedGraphControl7

            If Not isdataloaded3 Then Exit Sub

            Dim myPane As ZedGraph.GraphPane = ZedGraphControl2.GraphPane


            '去除之前的数据，并且记得要用axischange()和refresh()重画
            ZedGraphControl2.GraphPane.CurveList.Clear()
            ZedGraphControl2.GraphPane.GraphObjList.Clear()
            ZedGraphControl2.AxisChange()
            ZedGraphControl2.Refresh()


            '设置图标标题和x、y轴标题
            'myPane.Title.Text = "A1桥"
            'myPane.XAxis.Title.Text = "时间(s)"
            'myPane.YAxis.Title.Text = "加速度(m/s^2)"

            myPane.Title.Text = ""
            myPane.XAxis.Title.Text = ""
            myPane.YAxis.Title.Text = ""

            '更改标题的字体
            'Dim myFont As ZedGraph.FontSpec = New ZedGraph.FontSpec("Arial", 20, Color.Red, False, False, False)
            'myPane.Title.FontSpec = myFont
            'myPane.XAxis.Title.FontSpec = myFont
            'myPane.YAxis.Title.FontSpec = myFont
            Dim myFont As ZedGraph.FontSpec = New ZedGraph.FontSpec("黑体", 50, Color.Red, False, False, False)
            myPane.Title.FontSpec = myFont
            myPane.XAxis.Title.FontSpec = myFont
            myPane.YAxis.Title.FontSpec = myFont

            '生成数据
            Dim list1 As ZedGraph.PointPairList = New ZedGraph.PointPairList()
            Dim warnList As New ZedGraph.PointPairList '设置预警红线
            Dim warnList2 As New ZedGraph.PointPairList

            Dim myCurve As ZedGraph.LineItem = myPane.AddCurve("", list1, Color.Blue, ZedGraph.SymbolType.None)
            Dim warnCurve As ZedGraph.LineItem = myPane.AddCurve("", warnList, Color.Red, ZedGraph.SymbolType.Circle) '添加预警线
            Dim warnCurve2 As ZedGraph.LineItem = myPane.AddCurve("", warnList2, Color.Red, ZedGraph.SymbolType.Circle) '添加预警线


            Dim x As Double
            x = 0
            Dim y1 As Single

            myPane.XAxis.Scale.Max = 60
            myPane.XAxis.Scale.Min = 0

            Dim i As Integer '循环变量
            For i = 36000 To datalen3 Step 3
                'Dim x As Double = CSng(i) / CSng(100) Mod CSng(60)
                'x = CSng(i) / CSng(100) Mod CSng(60)
                'Dim y1 As Single
                y1 = dataShenzhen3(i)
                list1.Add(x, y1)

                If i Mod 10 = 0 Then
                    warnList.Add(x, 0.04)
                    warnList2.Add(x, -0.04)
                End If

                'warnList.Add(x, 0.04) '预警线的y值设置为20
                'warnList2.Add(x, -0.04)

                'myPane.XAxis.Scale.Max = 60
                'myPane.XAxis.Scale.Min = 0

                'If list1.Count > 2000 Then
                '    list1.Clear()
                'End If

                '控制主界面音量条开关
                If i Mod 80 = 0 Then
                    Dim dataAbs As Single = System.Math.Abs(dataShenzhen3(i))

                    '以下为开灯
                    If dataAbs > 0.0001 Then
                        Label54.BackColor = Color.LimeGreen
                        Label53.BackColor = Color.LimeGreen
                    End If

                    If dataAbs > 0.001 Then
                        Label52.BackColor = Color.LimeGreen
                    End If

                    If dataAbs > 0.0021 Then
                        Label51.BackColor = Color.LimeGreen
                    End If

                    If dataAbs > 0.0031 Then
                        Label50.BackColor = Color.LimeGreen
                    End If

                    If dataAbs > 0.0042 Then

                        Label49.BackColor = Color.LimeGreen
                    End If

                    If dataAbs > 0.0053 Then
                        Label48.BackColor = Color.LimeGreen
                    End If

                    If dataAbs > 0.0065 Then

                        Label47.BackColor = Color.LimeGreen
                    End If

                    If dataAbs > 0.0079 Then
                        Label46.BackColor = Color.LimeGreen
                    End If

                    If dataAbs > 0.0099 Then
                        Label45.BackColor = Color.Gold
                    End If

                    If dataAbs > 0.0132 Then
                        Label44.BackColor = Color.Gold
                    End If

                    If dataAbs > 0.0198 Then
                        Label43.BackColor = Color.Gold
                    End If

                    If dataAbs > 0.4385 Then
                        warning3 = True
                        Label42.BackColor = Color.Red
                    End If

                    '以下为关灯

                    If dataAbs < 0.0001 Then
                        Label54.BackColor = Color.Gray
                        Label53.BackColor = Color.Gray
                    End If

                    If dataAbs < 0.001 Then
                        Label52.BackColor = Color.Gray
                    End If

                    If dataAbs < 0.0021 Then
                        Label51.BackColor = Color.Gray
                    End If

                    If dataAbs < 0.0031 Then
                        Label50.BackColor = Color.Gray
                    End If

                    If dataAbs < 0.0042 Then
                        Label49.BackColor = Color.Gray
                    End If

                    If dataAbs < 0.0053 Then
                        Label48.BackColor = Color.Gray
                    End If

                    If dataAbs < 0.0065 Then
                        Label47.BackColor = Color.Gray
                    End If

                    If dataAbs < 0.0079 Then
                        Label46.BackColor = Color.Gray
                    End If

                    If dataAbs < 0.0099 Then
                        Label45.BackColor = Color.Gray
                    End If

                    If dataAbs < 0.0132 Then
                        Label44.BackColor = Color.Gray
                    End If

                    If dataAbs < 0.0198 Then
                        Label43.BackColor = Color.Gray
                    End If

                    If dataAbs < 0.4385 Then
                        Label42.BackColor = Color.Gray
                    End If

                    '预警灯开关
                    If Not warning3 Then
                        If dataAbs < 0.0099 Then
                            PictureBox12.BackgroundImage = Image.FromFile("mainimage\hk\light1.png")
                            PictureBox11.BackgroundImage = Image.FromFile("mainimage\hk\light5.png")
                            PictureBox10.BackgroundImage = Image.FromFile("mainimage\hk\light6.png")
                        ElseIf dataAbs < 0.4385 Then
                            PictureBox12.BackgroundImage = Image.FromFile("mainimage\hk\light4.png")
                            PictureBox11.BackgroundImage = Image.FromFile("mainimage\hk\light2.png")
                            PictureBox10.BackgroundImage = Image.FromFile("mainimage\hk\light6.png")
                        ElseIf dataAbs > 0.4385 Then
                            PictureBox12.BackgroundImage = Image.FromFile("mainimage\hk\light4.png")
                            PictureBox11.BackgroundImage = Image.FromFile("mainimage\hk\light5.png")
                            PictureBox10.BackgroundImage = Image.FromFile("mainimage\hk\light3.png")
                        End If
                    Else
                        If Not thwarn3.IsAlive Then
                            thwarn3 = New System.Threading.Thread(AddressOf warnLight3)
                            thwarn3.Start()
                        End If
                    End If


                End If

                If i Mod 10 = 0 Then
                    ZedGraphControl2.AxisChange()
                    ZedGraphControl2.Refresh()
                End If

                x = x + 0.03
                If x > 60 Then
                    x = 0
                    list1.Clear()
                    warnList.Clear()
                    warnList2.Clear()
                End If

                System.Threading.Thread.Sleep(30)

                'System.Windows.Forms.Application.DoEvents()
            Next

            '用list1生产一条曲线，标注是第一个参数中的字符串（此处为空）
            'Dim myCurve As ZedGraph.LineItem = myPane.AddCurve("", list1, Color.Blue, ZedGraph.SymbolType.None)
            'dim warnCurve As ZedGraph.LineItem = myPane.AddCurve("", warnList, Color.Red, ZedGraph.SymbolType.None) '添加预警线

            '填充图表颜色
            'myPane.Fill = New ZedGraph.Fill(Color.White, Color.FromArgb(200, 200, 255), 45.0F)



            'myPane.XAxis.Scale.Max = i / 100 '设置横坐标最大值为数据数量/100（即秒数）
            'myPane.YAxis.Scale.Max = absmax * 1.1 '设置纵坐标最大值为数据绝对值的最大值*1.1
            'myPane.YAxis.Scale.Min = 0 - absmax * 1.1 '设置纵坐标最大值为负的数据绝对值的最大值*1.1

            'myPane.YAxis.Scale.MaxAuto = True
            'myPane.YAxis.Scale.MinAuto = True
            'myPane.XAxis.Scale.MaxAuto = True
            'myPane.XAxis.Scale.MinAuto = True

            '画到zedGraphControl1控件中，此句必加
            'ZedGraphControl2.AxisChange()

            'ZedGraphControl2.Refresh()


        End While
    End Sub



    Private Sub Label1_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub PictureBox3_Click_1(sender As Object, e As EventArgs)
        Me.Hide()
        Try
            demoData.Suspend()
            demoData2.Suspend()
            demoData3.Suspend()
        Catch ex As Exception

        End Try

        Form1.Show()
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs)
        'Dim th1 As System.Threading.Thread = New System.Threading.Thread(AddressOf loadimagemain)
        'th1.Start()
    End Sub

    Private Sub loadimagemain()
        PictureBox2.Image = Image.FromFile("mainimage\zixitong\" + ComboBox1.SelectedIndex.ToString + ".jpg")
    End Sub

 
    Private Sub PictureBox5_Click(sender As Object, e As EventArgs)
        Me.Close()
    End Sub

    Private Sub Label55_Click(sender As Object, e As EventArgs)
        Me.Close()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs)
        Form5.Show()
        Form5.PictureBox1.Image = Image.FromFile("mainimage\sys.jpg")
        Form5.Text = "监测系统组成图"
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs)
        Form5.Show()
        Form5.PictureBox1.Image = Image.FromFile("mainimage\sys2.jpg")
        Form5.Text = "监测系统网络组成图"
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs)
        Form5.Show()
        Form5.PictureBox1.Image = Image.FromFile("mainimage\sensornum.jpg")
        Form5.Text = "系统传感器数目表"
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs)
        demoData = New System.Threading.Thread(AddressOf showDemoData1)
        demoData.Start()
        demoData2 = New System.Threading.Thread(AddressOf showDemoData2)
        demoData2.Start()
        demoData3 = New System.Threading.Thread(AddressOf showDemoData3)
        demoData3.Start()
    End Sub

    Private Sub Label7_Click(sender As Object, e As EventArgs) Handles Label7.Click

    End Sub

    Private Sub Label8_Click(sender As Object, e As EventArgs) Handles Label8.Click

    End Sub

    Private Sub Label9_Click(sender As Object, e As EventArgs) Handles Label9.Click

    End Sub

    Private Sub Label10_Click(sender As Object, e As EventArgs) Handles Label10.Click

    End Sub

    Private Sub Label11_Click(sender As Object, e As EventArgs) Handles Label11.Click

    End Sub

    Private Sub Label12_Click(sender As Object, e As EventArgs) Handles Label12.Click

    End Sub

    Private Sub Label13_Click(sender As Object, e As EventArgs) Handles Label13.Click

    End Sub

    Private Sub Label14_Click(sender As Object, e As EventArgs) Handles Label14.Click

    End Sub

    Private Sub Label15_Click(sender As Object, e As EventArgs) Handles Label15.Click

    End Sub

    Private Sub Label16_Click(sender As Object, e As EventArgs) Handles Label16.Click

    End Sub

    Private Sub Label17_Click(sender As Object, e As EventArgs) Handles Label17.Click

    End Sub

    Private Sub Label18_Click(sender As Object, e As EventArgs) Handles Label18.Click

    End Sub

    Private Sub Label19_Click(sender As Object, e As EventArgs) Handles Label19.Click

    End Sub

    Private Sub Label20_Click(sender As Object, e As EventArgs) Handles Label20.Click

    End Sub

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs)

    End Sub

    Private Sub Label55_Click_1(sender As Object, e As EventArgs) Handles Label55.Click

    End Sub

    Private Sub Label56_Click(sender As Object, e As EventArgs) Handles Label56.Click

    End Sub

    Private Sub Label58_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub PictureBox19_Click(sender As Object, e As EventArgs) Handles PictureBox19.Click
        'Me.Close()
        Try
            demoData.Abort()
            demoData2.Abort()
            demoData3.Abort()
            thShowGroundAcc.Abort()
            thShowHum.Abort()
            thShowTemp.Abort()
            thShowWeight.Abort()

        Catch ex As Exception

        End Try

        'Environment.Exit(0)
        Shell("TASKKILL /F /IM SHM.exe")
        System.Threading.Thread.Sleep(10000)
    End Sub

    Private Sub PictureBox17_Click(sender As Object, e As EventArgs) Handles PictureBox17.Click
        Me.Hide()
        Try

            demoData.Suspend()
            demoData2.Suspend()
            demoData3.Suspend()
            thShowGroundAcc.Suspend()
            thShowHum.Suspend()
            thShowTemp.Suspend()
            thShowWeight.Suspend()
        Catch ex As Exception

        End Try

        Form1.Show()
    End Sub



    Private Sub PictureBox17_MouseDown(sender As Object, e As MouseEventArgs) Handles PictureBox17.MouseDown
        'PictureBox4.BackgroundImage = Image.FromFile("mainimage\hk\title2.png")
    End Sub

    Private Sub PictureBox17_MouseUp(sender As Object, e As MouseEventArgs) Handles PictureBox17.MouseUp
        'PictureBox4.BackgroundImage = Image.FromFile("mainimage\hk\title1.png")
    End Sub

    '加载离线数据
    Private Sub loadOfflineData()
        Dim chongcaiyang As Integer = 2 '定义重采样频率
        While True
            '获取传感器编号
            Dim acc01 As String = (Profile.ReadOneString("main", "acc", "./config/A0.ini").Split(","))(0)
            Dim acc02 As String = (Profile.ReadOneString("main", "acc", "./config/A0.ini").Split(","))(1)
            Dim acc03 As String = (Profile.ReadOneString("main", "acc", "./config/A0.ini").Split(","))(2)
            Dim temp01 As String = (Profile.ReadOneString("main", "temp", "./config/A0.ini").Split(","))(0)
            Dim temp02 As String = (Profile.ReadOneString("main", "temp", "./config/A0.ini").Split(","))(1)
            Dim temp03 As String = (Profile.ReadOneString("main", "temp", "./config/A0.ini").Split(","))(2)
            Dim temp04 As String = (Profile.ReadOneString("main", "temp", "./config/A0.ini").Split(","))(3)

            Try
                '获取全路径
                Dim dirmid As String = "\" + Date.Now.Year.ToString + "-" + Date.Now.Month.ToString.PadLeft(2, "0") + "\" + Date.Now.Day.ToString.PadLeft(2, "0") + "\"
                Dim dirAcc01 = (Profile.ReadOneString("main", "dataDir", "./config/A0.ini")) + dirmid + acc01 + "\"
                Dim accFiles01 = My.Computer.FileSystem.GetFiles(dirAcc01) '列目录
                Dim accFileNow01 As String = accFiles01(accFiles01.Count - 1) '取最后一个文件
                If accFile01 <> accFileNow01 Then
                    ReDim accData01(0)
                    ReDim accData01(1000000)
                    '加载数据
                    Dim fs2 As New System.IO.FileStream(accFileNow01, IO.FileMode.Open)
                    Dim bw2 As New System.IO.BinaryReader(fs2)
                    Dim numnow(3) As Byte
                    Dim dataxiabiao As Integer = 0 '重采样数据下标
                    For j = 0 To (fs2.Length / 4 - 1) Step chongcaiyang
                        dataxiabiao = j / chongcaiyang
                        For i = 0 To 3
                            numnow(i) = fs2.ReadByte
                        Next
                        fs2.ReadByte()
                        fs2.ReadByte()
                        fs2.ReadByte()
                        fs2.ReadByte()
                        accData01(dataxiabiao) = BitConverter.ToSingle(numnow, 0)
                    Next
                    accFile01 = accFileNow01
                    hasNewData(0) = True
                End If
            Catch ex As Exception
            End Try

            Try
                '获取全路径
                Dim dirmid As String = "\" + Date.Now.Year.ToString + "-" + Date.Now.Month.ToString.PadLeft(2, "0") + "\" + Date.Now.Day.ToString.PadLeft(2, "0") + "\"
                Dim dirAcc = (Profile.ReadOneString("main", "dataDir", "./config/A0.ini")) + dirmid + acc02 + "\"
                Dim accFiles = My.Computer.FileSystem.GetFiles(dirAcc) '列目录
                Dim accFileNow As String = accFiles(accFiles.Count - 1) '取最后一个文件
                If accFile02 <> accFileNow Then
                    ReDim accData02(0)
                    ReDim accData02(1000000)
                    '加载数据
                    Dim fs2 As New System.IO.FileStream(accFileNow, IO.FileMode.Open)
                    Dim bw2 As New System.IO.BinaryReader(fs2)
                    Dim numnow(3) As Byte
                    Dim dataxiabiao As Integer = 0 '重采样数据下标
                    For j = 0 To (fs2.Length / 4 - 1) Step chongcaiyang
                        dataxiabiao = j / chongcaiyang
                        For i = 0 To 3
                            numnow(i) = fs2.ReadByte
                        Next
                        fs2.ReadByte()
                        fs2.ReadByte()
                        fs2.ReadByte()
                        fs2.ReadByte()
                        accData02(dataxiabiao) = BitConverter.ToSingle(numnow, 0)
                    Next
                    accFile02 = accFileNow
                    hasNewData(1) = True
                End If
            Catch ex As Exception
            End Try

            Try
                '获取全路径
                Dim dirmid As String = "\" + Date.Now.Year.ToString + "-" + Date.Now.Month.ToString.PadLeft(2, "0") + "\" + Date.Now.Day.ToString.PadLeft(2, "0") + "\"
                Dim dirAcc = (Profile.ReadOneString("main", "dataDir", "./config/A0.ini")) + dirmid + acc03 + "\"
                Dim accFiles = My.Computer.FileSystem.GetFiles(dirAcc) '列目录
                Dim accFileNow As String = accFiles(accFiles.Count - 1) '取最后一个文件
                If accFile03 <> accFileNow Then
                    ReDim accData03(0)
                    ReDim accData03(1000000)
                    '加载数据
                    Dim fs2 As New System.IO.FileStream(accFileNow, IO.FileMode.Open)
                    Dim bw2 As New System.IO.BinaryReader(fs2)
                    Dim numnow(3) As Byte
                    Dim dataxiabiao As Integer = 0 '重采样数据下标
                    For j = 0 To (fs2.Length / 4 - 1) Step chongcaiyang
                        dataxiabiao = j / chongcaiyang
                        For i = 0 To 3
                            numnow(i) = fs2.ReadByte
                        Next
                        fs2.ReadByte()
                        fs2.ReadByte()
                        fs2.ReadByte()
                        fs2.ReadByte()
                        accData03(dataxiabiao) = BitConverter.ToSingle(numnow, 0)
                    Next
                    accFile03 = accFileNow
                    hasNewData(2) = True
                End If
            Catch ex As Exception
            End Try

            Try
                '获取全路径
                Dim dirmid As String = "\" + Date.Now.Year.ToString + "-" + Date.Now.Month.ToString.PadLeft(2, "0") + "\" + Date.Now.Day.ToString.PadLeft(2, "0") + "\"
                Dim dirAcc = (Profile.ReadOneString("main", "dataDir", "./config/A0.ini")) + dirmid + temp01 + "\"
                Dim accFiles = My.Computer.FileSystem.GetFiles(dirAcc) '列目录
                Dim accFileNow As String = accFiles(accFiles.Count - 1) '取最后一个文件
                If tempFile01 <> accFileNow Then
                    ReDim tempData01(0)
                    ReDim tempData01(1000000)
                    '加载数据
                    Dim fs2 As New System.IO.FileStream(accFileNow, IO.FileMode.Open)
                    Dim bw2 As New System.IO.BinaryReader(fs2)
                    Dim numnow(3) As Byte
                    Dim dataxiabiao As Integer = 0 '重采样数据下标
                    For j = 0 To (fs2.Length / 4 - 1) Step chongcaiyang
                        dataxiabiao = j / chongcaiyang
                        For i = 0 To 3
                            numnow(i) = fs2.ReadByte
                        Next
                        fs2.ReadByte()
                        fs2.ReadByte()
                        fs2.ReadByte()
                        fs2.ReadByte()
                        tempData01(dataxiabiao) = BitConverter.ToSingle(numnow, 0)
                    Next
                    tempFile01 = accFileNow
                    hasNewData(3) = True
                End If
            Catch ex As Exception
            End Try

            Try
                '获取全路径
                Dim dirmid As String = "\" + Date.Now.Year.ToString + "-" + Date.Now.Month.ToString.PadLeft(2, "0") + "\" + Date.Now.Day.ToString.PadLeft(2, "0") + "\"
                Dim dirAcc = (Profile.ReadOneString("main", "dataDir", "./config/A0.ini")) + dirmid + temp02 + "\"
                Dim accFiles = My.Computer.FileSystem.GetFiles(dirAcc) '列目录
                Dim accFileNow As String = accFiles(accFiles.Count - 1) '取最后一个文件
                If tempFile02 <> accFileNow Then
                    ReDim tempData02(0)
                    ReDim tempData02(1000000)
                    '加载数据
                    Dim fs2 As New System.IO.FileStream(accFileNow, IO.FileMode.Open)
                    Dim bw2 As New System.IO.BinaryReader(fs2)
                    Dim numnow(3) As Byte
                    Dim dataxiabiao As Integer = 0 '重采样数据下标
                    For j = 0 To (fs2.Length / 4 - 1) Step chongcaiyang
                        dataxiabiao = j / chongcaiyang
                        For i = 0 To 3
                            numnow(i) = fs2.ReadByte
                        Next
                        fs2.ReadByte()
                        fs2.ReadByte()
                        fs2.ReadByte()
                        fs2.ReadByte()
                        tempData02(dataxiabiao) = BitConverter.ToSingle(numnow, 0)
                    Next
                    tempFile02 = accFileNow
                    hasNewData(4) = True
                End If
            Catch ex As Exception
            End Try

            Try
                '获取全路径
                Dim dirmid As String = "\" + Date.Now.Year.ToString + "-" + Date.Now.Month.ToString.PadLeft(2, "0") + "\" + Date.Now.Day.ToString.PadLeft(2, "0") + "\"
                Dim dirAcc = (Profile.ReadOneString("main", "dataDir", "./config/A0.ini")) + dirmid + temp03 + "\"
                Dim accFiles = My.Computer.FileSystem.GetFiles(dirAcc) '列目录
                Dim accFileNow As String = accFiles(accFiles.Count - 1) '取最后一个文件
                If tempFile03 <> accFileNow Then
                    ReDim tempData03(0)
                    ReDim tempData03(1000000)
                    '加载数据
                    Dim fs2 As New System.IO.FileStream(accFileNow, IO.FileMode.Open)
                    Dim bw2 As New System.IO.BinaryReader(fs2)
                    Dim numnow(3) As Byte
                    Dim dataxiabiao As Integer = 0 '重采样数据下标
                    For j = 0 To (fs2.Length / 4 - 1) Step chongcaiyang
                        dataxiabiao = j / chongcaiyang
                        For i = 0 To 3
                            numnow(i) = fs2.ReadByte
                        Next
                        fs2.ReadByte()
                        fs2.ReadByte()
                        fs2.ReadByte()
                        fs2.ReadByte()
                        tempData03(dataxiabiao) = BitConverter.ToSingle(numnow, 0)
                    Next
                    tempFile03 = accFileNow
                    hasNewData(5) = True
                End If
            Catch ex As Exception
            End Try

            Try
                '获取全路径
                Dim dirmid As String = "\" + Date.Now.Year.ToString + "-" + Date.Now.Month.ToString.PadLeft(2, "0") + "\" + Date.Now.Day.ToString.PadLeft(2, "0") + "\"
                Dim dirAcc = (Profile.ReadOneString("main", "dataDir", "./config/A0.ini")) + dirmid + temp04 + "\"
                Dim accFiles = My.Computer.FileSystem.GetFiles(dirAcc) '列目录
                Dim accFileNow As String = accFiles(accFiles.Count - 1) '取最后一个文件
                If tempFile04 <> accFileNow Then
                    ReDim tempData04(0)
                    ReDim tempData04(1000000)
                    '加载数据
                    Dim fs2 As New System.IO.FileStream(accFileNow, IO.FileMode.Open)
                    Dim bw2 As New System.IO.BinaryReader(fs2)
                    Dim numnow(3) As Byte
                    Dim dataxiabiao As Integer = 0 '重采样数据下标
                    For j = 0 To (fs2.Length / 4 - 1) Step chongcaiyang
                        dataxiabiao = j / chongcaiyang
                        For i = 0 To 3
                            numnow(i) = fs2.ReadByte
                        Next
                        fs2.ReadByte()
                        fs2.ReadByte()
                        fs2.ReadByte()
                        fs2.ReadByte()
                        tempData04(dataxiabiao) = BitConverter.ToSingle(numnow, 0)
                    Next
                    tempFile04 = accFileNow
                    hasNewData(6) = True
                End If
            Catch ex As Exception
            End Try


            System.Threading.Thread.Sleep(60000) '一分钟后再次检查
        End While
    End Sub

    '将离线数据读入缓存
    Private Sub readDataArr()
        Dim eachNum = 1000
        While True
            For i = 0 To 6
                If hasNewData(i) = True Then
                    dataPoint(i) = 0
                    hasNewData(i) = False
                End If
            Next

            Try
                For i = 1 To eachNum
                    dataquene1.Enqueue(accData01(dataPoint(0)))
                    dataPoint(0) = dataPoint(0) + 1
                Next
                '如果队列过长，则清空数据
                If dataquene1.Count > (eachNum * 3) Then
                    dataquene1.Clear()
                End If
            Catch ex As Exception

            End Try

            Try
                For i = 1 To eachNum
                    dataquene2.Enqueue(accData02(dataPoint(1)))
                    dataPoint(1) = dataPoint(1) + 1
                Next
                '如果队列过长，则清空数据
                If dataquene2.Count > (eachNum * 3) Then
                    dataquene2.Clear()
                End If
            Catch ex As Exception

            End Try

            Try
                For i = 1 To eachNum
                    dataquene3.Enqueue(accData03(dataPoint(2)))
                    dataPoint(2) = dataPoint(2) + 1
                Next
                '如果队列过长，则清空数据
                If dataquene3.Count > (eachNum * 3) Then
                    dataquene3.Clear()
                End If
            Catch ex As Exception

            End Try

            Try
                For i = 1 To eachNum
                    tempquene1.Enqueue(tempData01(dataPoint(3)))
                    dataPoint(3) = dataPoint(3) + 1
                Next
                '如果队列过长，则清空数据
                If tempquene1.Count > (eachNum * 3) Then
                    tempquene1.Clear()
                End If
            Catch ex As Exception

            End Try

            Try
                For i = 1 To eachNum
                    humquene1.Enqueue(tempData02(dataPoint(4)))
                    dataPoint(4) = dataPoint(4) + 1
                Next
                '如果队列过长，则清空数据
                If humquene1.Count > (eachNum * 3) Then
                    humquene1.Clear()
                End If
            Catch ex As Exception

            End Try

            Try
                For i = 1 To eachNum
                    tempquene2.Enqueue(tempData03(dataPoint(5)))
                    dataPoint(5) = dataPoint(5) + 1
                Next
                '如果队列过长，则清空数据
                If tempquene2.Count > (eachNum * 3) Then
                    tempquene2.Clear()
                End If
            Catch ex As Exception

            End Try

            Try
                For i = 1 To eachNum
                    humquene2.Enqueue(tempData04(dataPoint(6)))
                    dataPoint(6) = dataPoint(6) + 1
                Next
                '如果队列过长，则清空数据
                If humquene2.Count > (eachNum * 3) Then
                    humquene2.Clear()
                End If
            Catch ex As Exception

            End Try

            System.Threading.Thread.Sleep((1000 * eachNum / 200))
        End While

    End Sub

    Public Sub PictureBox18_Click(sender As Object, e As EventArgs) Handles PictureBox18.Click

        Try
            If demoData.IsAlive Then
                Exit Sub
            End If
        Catch ex As Exception

        End Try


        '载入离线数据并模拟实时接收
        Dim loadOfflineDataTh = New System.Threading.Thread(AddressOf loadOfflineData)
        loadOfflineDataTh.Start()

        Dim readDatath = New System.Threading.Thread(AddressOf readDataArr)
        readDatath.Start()

        '使用TCP方式接收实时数据
        'thTCPReceive = New System.Threading.Thread(AddressOf TCPReceive2)
        'thTCPReceive2 = New System.Threading.Thread(AddressOf TCPReceiveSystem2)
        'thTCPReceive3 = New System.Threading.Thread(AddressOf TCPReceiveSystem3)
        'thTCPReceive.Priority = 4
        'thTCPReceive2.Priority = 4
        'thTCPReceive3.Priority = 4
        'thTCPReceive.Start()
        'thTCPReceive2.Start()
        'thTCPReceive3.Start()


        demoData = New System.Threading.Thread(AddressOf showTCPData1)
        demoData.Start()
        demoData2 = New System.Threading.Thread(AddressOf showTCPData2)
        demoData2.Start()
        demoData3 = New System.Threading.Thread(AddressOf showTCPData3)
        demoData3.Start()

        thShowTemp = New System.Threading.Thread(AddressOf showTemp1)
        thShowTemp.Start()
        thShowGroundAcc = New System.Threading.Thread(AddressOf showTemp2)
        thShowGroundAcc.Start()
        thShowHum = New System.Threading.Thread(AddressOf showhum1)
        thShowHum.Start()
        thShowWeight = New System.Threading.Thread(AddressOf showhum2)
        thShowWeight.Start()


        Dim positonth = New System.Threading.Thread(AddressOf showposition)
        positonth.Start()

        'Dim videoTh = New System.Threading.Thread(AddressOf playVideo)
        'videoTh.Start()

    End Sub

    Private Sub playVideo()
        'While True
        '    Try
        '        'Dim Uri = New Uri("e:\test.rmvb")
        '        'Dim Uri = New Uri("rtsp://wowzaec2demo.streamlock.net/vod/mp4:BigBuckBunny_115k.mov")
        '        Dim Uri = New Uri("rtsp://192.168.1.3:554/")
        '        Dim convertedURI = Uri.AbsoluteUri
        '        AxVLCPlugin21.playlist.clear()
        '        For i = 0 To 10
        '            AxVLCPlugin21.playlist.add(convertedURI)
        '        Next

        '        AxVLCPlugin21.playlist.play()
        '        While True
        '            System.Threading.Thread.Sleep(5000)
        '            If AxVLCPlugin21.playlist.isPlaying = False Then
        '                AxVLCPlugin21.playlist.stop()
        '                Throw New Exception("播放终止")
        '            End If
        '        End While
        '    Catch ex As Exception

        '    End Try

        'End While

    End Sub


    Private Sub showTemp1()
        'Dim tempValueS As String
        'tempValueS = My.Computer.FileSystem.ReadAllText("D:\Storage Data\shenzhen\Temp.txt")
        'Dim tempValue = tempValueS.Split(Chr(9))

        Dim zgc = ZedGraphControl4
        Dim myPane = ZedGraphControl4.GraphPane
        '生成数据
        Dim list1 As ZedGraph.PointPairList = New ZedGraph.PointPairList()
        'Dim warnList As New ZedGraph.PointPairList '设置预警红线

        myPane.XAxis.Scale.Max = 60
        myPane.XAxis.Scale.Min = 0
        Dim x As Integer = 0
        Dim i As Integer '循环变量
        myPane.CurveList.Clear()

        '用list1生产一条曲线，标注是第一个参数中的字符串（此处为空）
        Dim myCurve As ZedGraph.LineItem = myPane.AddCurve("", list1, Color.LimeGreen, ZedGraph.SymbolType.None)

        '线条粗细调整
        myCurve.Line.Width = 3
        While True





            While tempquene1.Count > 0
                Try


                    Dim blue As Single
                    blue = tempquene1.Dequeue
                    myPane.YAxis.Scale.Max = blue + 2
                    myPane.YAxis.Scale.Min = blue - 2
                    list1.Add(x, blue)

                    '画到zedGraphControl1控件中，此句必加
                    zgc.AxisChange()

                    zgc.Refresh()
                    System.Threading.Thread.Sleep(750)
                    x = x + 1



                    If x = 60 Then
                        x = 0

                        '去除之前的数据
                        list1.Clear()
                    End If
                Catch ex As Exception

                End Try
            End While
            System.Threading.Thread.Sleep(500)

            'For i = 0 To tempValue.Length - 1
            '    Dim blue As Single
            '    blue = Val(tempValue(i))
            '    list1.Add(x, blue)





            '    '画到zedGraphControl1控件中，此句必加
            '    zgc.AxisChange()

            '    zgc.Refresh()
            '    System.Threading.Thread.Sleep(1000)

            '    x = x + 1



            '    If x = 36 Then
            '        x = 0

            '        '去除之前的数据
            '        list1.Clear()
            '    End If
            '    System.Windows.Forms.Application.DoEvents()
            'Next

        End While
    End Sub

    Private Sub showTemp2()
        'Dim tempValueS As String
        'tempValueS = My.Computer.FileSystem.ReadAllText("D:\Storage Data\shenzhen\Temp.txt")
        'Dim tempValue = tempValueS.Split(Chr(9))

        Dim zgc = ZedGraphControl6
        Dim myPane = ZedGraphControl6.GraphPane
        '生成数据
        Dim list1 As ZedGraph.PointPairList = New ZedGraph.PointPairList()
        'Dim warnList As New ZedGraph.PointPairList '设置预警红线

        myPane.XAxis.Scale.Max = 60
        myPane.XAxis.Scale.Min = 0
        Dim x As Integer = 0
        Dim i As Integer '循环变量
        myPane.CurveList.Clear()

        '用list1生产一条曲线，标注是第一个参数中的字符串（此处为空）
        Dim myCurve As ZedGraph.LineItem = myPane.AddCurve("", list1, Color.LimeGreen, ZedGraph.SymbolType.None)

        '线条粗细调整
        myCurve.Line.Width = 3
        While True





            While tempquene2.Count > 0
                Try


                    Dim blue As Single
                    blue = tempquene2.Dequeue
                    myPane.YAxis.Scale.Max = blue + 2
                    myPane.YAxis.Scale.Min = blue - 2
                    list1.Add(x, blue)

                    '画到zedGraphControl1控件中，此句必加
                    zgc.AxisChange()

                    zgc.Refresh()
                    System.Threading.Thread.Sleep(750)
                    x = x + 1



                    If x = 60 Then
                        x = 0

                        '去除之前的数据
                        list1.Clear()
                    End If
                Catch ex As Exception

                End Try
            End While
            System.Threading.Thread.Sleep(500)

            'For i = 0 To tempValue.Length - 1
            '    Dim blue As Single
            '    blue = Val(tempValue(i))
            '    list1.Add(x, blue)





            '    '画到zedGraphControl1控件中，此句必加
            '    zgc.AxisChange()

            '    zgc.Refresh()
            '    System.Threading.Thread.Sleep(1000)

            '    x = x + 1



            '    If x = 36 Then
            '        x = 0

            '        '去除之前的数据
            '        list1.Clear()
            '    End If
            '    System.Windows.Forms.Application.DoEvents()
            'Next

        End While
    End Sub


    Private Sub showhum1()
        'Dim tempValueS As String
        'tempValueS = My.Computer.FileSystem.ReadAllText("D:\Storage Data\shenzhen\Temp.txt")
        'Dim tempValue = tempValueS.Split(Chr(9))

        Dim zgc = ZedGraphControl3
        Dim myPane = ZedGraphControl3.GraphPane
        '生成数据
        Dim list1 As ZedGraph.PointPairList = New ZedGraph.PointPairList()
        'Dim warnList As New ZedGraph.PointPairList '设置预警红线

        myPane.XAxis.Scale.Max = 60
        myPane.XAxis.Scale.Min = 0
        Dim x As Integer = 0
        Dim i As Integer '循环变量
        myPane.CurveList.Clear()

        '用list1生产一条曲线，标注是第一个参数中的字符串（此处为空）
        Dim myCurve As ZedGraph.LineItem = myPane.AddCurve("", list1, Color.LimeGreen, ZedGraph.SymbolType.None)

        '线条粗细调整
        myCurve.Line.Width = 3
        While True





            While humquene1.Count > 0
                Try


                    Dim blue As Single
                    blue = humquene1.Dequeue * 0.6
                    myPane.YAxis.Scale.Max = blue + 2
                    myPane.YAxis.Scale.Min = blue - 2
                    list1.Add(x, blue)

                    '画到zedGraphControl1控件中，此句必加
                    zgc.AxisChange()

                    zgc.Refresh()
                    System.Threading.Thread.Sleep(750)
                    x = x + 1



                    If x = 60 Then
                        x = 0

                        '去除之前的数据
                        list1.Clear()
                    End If
                Catch ex As Exception

                End Try
            End While
            System.Threading.Thread.Sleep(500)

            'For i = 0 To tempValue.Length - 1
            '    Dim blue As Single
            '    blue = Val(tempValue(i))
            '    list1.Add(x, blue)





            '    '画到zedGraphControl1控件中，此句必加
            '    zgc.AxisChange()

            '    zgc.Refresh()
            '    System.Threading.Thread.Sleep(1000)

            '    x = x + 1



            '    If x = 36 Then
            '        x = 0

            '        '去除之前的数据
            '        list1.Clear()
            '    End If
            '    System.Windows.Forms.Application.DoEvents()
            'Next

        End While
    End Sub

    Private Sub showhum2()
        'Dim tempValueS As String
        'tempValueS = My.Computer.FileSystem.ReadAllText("D:\Storage Data\shenzhen\Temp.txt")
        'Dim tempValue = tempValueS.Split(Chr(9))

        Dim zgc = ZedGraphControl5
        Dim myPane = ZedGraphControl5.GraphPane
        '生成数据
        Dim list1 As ZedGraph.PointPairList = New ZedGraph.PointPairList()
        'Dim warnList As New ZedGraph.PointPairList '设置预警红线

        myPane.XAxis.Scale.Max = 60
        myPane.XAxis.Scale.Min = 0
        Dim x As Integer = 0
        Dim i As Integer '循环变量
        myPane.CurveList.Clear()

        '用list1生产一条曲线，标注是第一个参数中的字符串（此处为空）
        Dim myCurve As ZedGraph.LineItem = myPane.AddCurve("", list1, Color.LimeGreen, ZedGraph.SymbolType.None)

        '线条粗细调整
        myCurve.Line.Width = 3
        While True





            While humquene2.Count > 0
                Try


                    Dim blue As Single
                    blue = humquene2.Dequeue
                    myPane.YAxis.Scale.Max = blue + 2
                    myPane.YAxis.Scale.Min = blue - 2
                    list1.Add(x, blue)

                    '画到zedGraphControl1控件中，此句必加
                    zgc.AxisChange()

                    zgc.Refresh()
                    System.Threading.Thread.Sleep(750)
                    x = x + 1



                    If x = 60 Then
                        x = 0

                        '去除之前的数据
                        list1.Clear()
                    End If
                Catch ex As Exception

                End Try
            End While
            System.Threading.Thread.Sleep(500)

            'For i = 0 To tempValue.Length - 1
            '    Dim blue As Single
            '    blue = Val(tempValue(i))
            '    list1.Add(x, blue)





            '    '画到zedGraphControl1控件中，此句必加
            '    zgc.AxisChange()

            '    zgc.Refresh()
            '    System.Threading.Thread.Sleep(1000)

            '    x = x + 1



            '    If x = 36 Then
            '        x = 0

            '        '去除之前的数据
            '        list1.Clear()
            '    End If
            '    System.Windows.Forms.Application.DoEvents()
            'Next

        End While
    End Sub



    Private Sub showHum()
        On Error Resume Next
        While True


            Dim tempValueS As String
            tempValueS = My.Computer.FileSystem.ReadAllText("D:\Storage Data\shenzhen\Hum.txt")
            Dim tempValue = tempValueS.Split(Chr(9))

            Dim zgc = ZedGraphControl3
            Dim myPane = zgc.GraphPane
            '生成数据
            Dim list1 As ZedGraph.PointPairList = New ZedGraph.PointPairList()
            'Dim warnList As New ZedGraph.PointPairList '设置预警红线

            myPane.XAxis.Scale.Max = 36
            myPane.XAxis.Scale.Min = 0
            Dim x As Integer = 0
            Dim i As Integer '循环变量

            myPane.CurveList.Clear()

            '用list1生产一条曲线，标注是第一个参数中的字符串（此处为空）
            Dim myCurve As ZedGraph.LineItem = myPane.AddCurve("", list1, Color.LimeGreen, ZedGraph.SymbolType.None)


            '线条粗细调整
            myCurve.Line.Width = 3

            For i = 0 To tempValue.Length - 1
                Dim blue As Single
                blue = Val(tempValue(i))
                list1.Add(x, blue)






                '画到zedGraphControl1控件中，此句必加
                zgc.AxisChange()

                zgc.Refresh()
                System.Threading.Thread.Sleep(1000)

                x = x + 1
                If x = 36 Then
                    x = 0

                    '去除之前的数据
                    list1.Clear()
                End If
                System.Windows.Forms.Application.DoEvents()
            Next

        End While
    End Sub

    Private Sub showGroundAcc()
        On Error Resume Next
        While True



            Dim tempValueS As String
            tempValueS = My.Computer.FileSystem.ReadAllText("D:\Storage Data\shenzhen\groundacc.txt")
            Dim sep(2) As Char


            Dim tempValue = tempValueS.Split(vbCrLf)


            Dim zgc = ZedGraphControl6
            Dim myPane = zgc.GraphPane

            myPane.CurveList.Clear()

            '生成数据
            Dim list1 As ZedGraph.PointPairList = New ZedGraph.PointPairList()
            'Dim warnList As New ZedGraph.PointPairList '设置预警红线

            myPane.XAxis.Scale.Max = 60
            myPane.XAxis.Scale.Min = 0
            Dim x As Single = 0
            Dim i As Integer '循环变量
            '用list1生产一条曲线，标注是第一个参数中的字符串（此处为空）
            Dim myCurve As ZedGraph.LineItem = myPane.AddCurve("", list1, Color.LimeGreen, ZedGraph.SymbolType.None)

            ''线条粗细调整
            'myCurve.Line.Width = 3

            Dim blue As Single

            For i = 0 To tempValue.Length - 1 Step 2
                'x = i / 50

                blue = Val(tempValue(i))
                list1.Add(x, blue)





                If (i Mod 30 = 0) And x > 0 Then
                    '画到zedGraphControl1控件中，此句必加
                    zgc.AxisChange()

                    zgc.Refresh()

                End If

                System.Threading.Thread.Sleep(200)

                x = x + 0.2
                If x > 60 Then
                    x = 0

                    '去除之前的数据
                    list1.Clear()
                End If

                'System.Windows.Forms.Application.DoEvents()
            Next

        End While
    End Sub

    Private Sub showWeight()
        On Error Resume Next

        While True



            Dim tempValueS, timeS As String
            tempValueS = My.Computer.FileSystem.ReadAllText("D:\Storage Data\shenzhen\Weight.txt")
            timeS = My.Computer.FileSystem.ReadAllText("D:\Storage Data\shenzhen\Weight_time.txt")
            Dim tempValue = tempValueS.Split(Chr(9))
            Dim time = timeS.Split(Chr(9))


            Dim zgc = ZedGraphControl5
            Dim myPane = zgc.GraphPane

            myPane.CurveList.Clear()

            '生成数据
            Dim list1 As ZedGraph.PointPairList = New ZedGraph.PointPairList()
            'Dim warnList As New ZedGraph.PointPairList '设置预警红线

            myPane.XAxis.Scale.Max = 600
            myPane.XAxis.Scale.Min = 0
            Dim x As Integer = 0
            Dim i As Integer '循环变量
            Dim timenow, timesub As Integer
            timenow = 0
            timesub = 0

            '用list1生产一条曲线，标注是第一个参数中的字符串（此处为空）
            'Dim mybar As ZedGraph.BarItem = myPane.AddBar("", list1, Color.Blue)
            'mybar.Bar.Border.Width = 0
            Dim myStick As ZedGraph.StickItem = myPane.AddStick("", list1, Color.LimeGreen)
            myStick.Line.Width = 3
            'Dim myCurve As ZedGraph.LineItem = myPane.AddCurve("", list1, Color.Blue, ZedGraph.SymbolType.None)

            '线条粗细调整
            'myCurve.Line.Width = 3

            For i = 0 To tempValue.Length - 1
                Dim blue As Single
                timenow = Val(time(i)) - timesub
                blue = Val(tempValue(i)) / 1000
                list1.Add(timenow, blue)





                '画到zedGraphControl1控件中，此句必加
                zgc.AxisChange()

                zgc.Refresh()
                If i > 0 Then
                    System.Threading.Thread.Sleep((Val(time(i)) - Val(time(i - 1))) * 1000)
                End If


                x = x + 1

                If timenow > 600 Then
                    timesub = timesub + 600
                    list1.Clear()
                End If

                'If x = 60 Then
                '    x = 0

                '    '去除之前的数据
                '    list1.Clear()
                'End If

                System.Windows.Forms.Application.DoEvents()
            Next

        End While
    End Sub

    Private Sub showposition()
        While True

            For i = 0 To 4
                PictureBox20.BackgroundImage = Image.FromFile("mainimage\hk\space5.png")

                System.Threading.Thread.Sleep(500)
                PictureBox20.BackgroundImage = Image.FromFile("mainimage\hk\space4.png")
                System.Threading.Thread.Sleep(500)
            Next

            For i = 0 To 4
                PictureBox20.BackgroundImage = Image.FromFile("mainimage\hk\space_2nd_red.png")

                System.Threading.Thread.Sleep(500)
                PictureBox20.BackgroundImage = Image.FromFile("mainimage\hk\space_2nd.png")
                System.Threading.Thread.Sleep(500)
            Next

        End While

    End Sub



    Private Sub PictureBox18_MouseDown(sender As Object, e As MouseEventArgs) Handles PictureBox18.MouseDown
        'PictureBox4.BackgroundImage = Image.FromFile("mainimage\hk\title3.png")
    End Sub

    Private Sub PictureBox18_MouseUp(sender As Object, e As MouseEventArgs) Handles PictureBox18.MouseUp
        'PictureBox4.BackgroundImage = Image.FromFile("mainimage\hk\title1.png")
    End Sub

    Private Sub PictureBox19_MouseDown(sender As Object, e As MouseEventArgs) Handles PictureBox19.MouseDown
        'PictureBox4.BackgroundImage = Image.FromFile("mainimage\hk\title4.png")
    End Sub



    Private Sub PictureBox19_MouseUp(sender As Object, e As MouseEventArgs) Handles PictureBox19.MouseUp
        'PictureBox4.BackgroundImage = Image.FromFile("mainimage\hk\title1.png")
    End Sub

    Private Sub PictureBox13_Click(sender As Object, e As EventArgs) Handles PictureBox13.Click
        Form5.Show()
        Form5.filenames = {"mainimage\sys.jpg", "mainimage\sys2nd.png"}
        Form5.PictureBox1.Image = Image.FromFile("mainimage\sys.jpg")
        Form5.Text = "监测系统组成图"
    End Sub

    Private Sub PictureBox13_MouseDown(sender As Object, e As MouseEventArgs) Handles PictureBox13.MouseDown
        PictureBox13.BackgroundImage = Image.FromFile("mainimage\hk\consist2.png")
    End Sub

    Private Sub PictureBox13_MouseUp(sender As Object, e As MouseEventArgs) Handles PictureBox13.MouseUp
        PictureBox13.BackgroundImage = Image.FromFile("mainimage\hk\consist1.png")
    End Sub



    Private Sub PictureBox15_MouseDown(sender As Object, e As MouseEventArgs) Handles PictureBox15.MouseDown
        PictureBox15.BackgroundImage = Image.FromFile("mainimage\hk\consist2.png")
    End Sub

    Private Sub PictureBox15_MouseUp(sender As Object, e As MouseEventArgs) Handles PictureBox15.MouseUp
        PictureBox15.BackgroundImage = Image.FromFile("mainimage\hk\consist1.png")
    End Sub



    Private Sub PictureBox16_MouseDown(sender As Object, e As MouseEventArgs) Handles PictureBox16.MouseDown
        PictureBox16.BackgroundImage = Image.FromFile("mainimage\hk\consist2.png")
    End Sub

    Private Sub PictureBox16_MouseUp(sender As Object, e As MouseEventArgs) Handles PictureBox16.MouseUp
        PictureBox16.BackgroundImage = Image.FromFile("mainimage\hk\consist1.png")
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged_1(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        Dim th1 As System.Threading.Thread = New System.Threading.Thread(AddressOf loadimagemain)
        th1.Start()
    End Sub

    Private Sub PictureBox15_Click(sender As Object, e As EventArgs) Handles PictureBox15.Click
        Form5.Show()
        Form5.filenames = {"mainimage\sys2.jpg", "mainimage\sys2.jpg"}
        Form5.PictureBox1.Image = Image.FromFile("mainimage\sys2.jpg")
        Form5.Text = "监测系统网络组成图"
    End Sub

    Private Sub PictureBox16_Click(sender As Object, e As EventArgs) Handles PictureBox16.Click
        Form5.Show()
        Form5.filenames = {"mainimage\sensornum.jpg", "mainimage\sensornum2.png"}
        Form5.PictureBox1.Image = Image.FromFile("mainimage\sensornum.jpg")
        Form5.Text = "系统传感器数目表"
    End Sub




    Private Sub ToolStripMenuItem4_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem4.Click
        PictureBox18_Click(sender, e)

    End Sub

    Private Sub 复制CToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 复制CToolStripMenuItem.Click
        GraphSel.ShowDialog()
        Select Case GraphSel.ComboBox1.SelectedIndex
            Case 0
                ZedGraphControl4.Copy(True)
            Case 1
                ZedGraphControl3.Copy(True)
            Case 2
                ZedGraphControl6.Copy(True)
            Case 3
                ZedGraphControl5.Copy(True)
            Case 4
                ZedGraphControl7.Copy(True)
            Case 5
                ZedGraphControl1.Copy(True)
            Case 6
                ZedGraphControl2.Copy(True)

        End Select

    End Sub

    Private Sub 另存AToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 另存AToolStripMenuItem.Click
        GraphSel.ShowDialog()
        Select Case GraphSel.ComboBox1.SelectedIndex
            Case 0
                ZedGraphControl4.SaveAs()
            Case 1
                ZedGraphControl3.SaveAs()
            Case 2
                ZedGraphControl6.SaveAs()
            Case 3
                ZedGraphControl5.SaveAs()
            Case 4
                ZedGraphControl7.SaveAs()
            Case 5
                ZedGraphControl1.SaveAs()
            Case 6
                ZedGraphControl2.SaveAs()

        End Select
    End Sub

    Private Sub 打印ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 打印ToolStripMenuItem.Click
        GraphSel.ShowDialog()
        Select Case GraphSel.ComboBox1.SelectedIndex
            Case 0
                ZedGraphControl4.DoPrint()
            Case 1
                ZedGraphControl3.DoPrint()
            Case 2
                ZedGraphControl6.DoPrint()
            Case 3
                ZedGraphControl5.DoPrint()
            Case 4
                ZedGraphControl7.DoPrint()
            Case 5
                ZedGraphControl1.DoPrint()
            Case 6
                ZedGraphControl2.DoPrint()

        End Select
    End Sub

    Private Sub ToolStripMenuItem3_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem3.Click
        GraphSel.ShowDialog()
        Select Case GraphSel.ComboBox1.SelectedIndex
            Case 0
                ZedGraphControl4.DoPrintPreview()

            Case 1
                ZedGraphControl3.DoPrintPreview()
            Case 2
                ZedGraphControl6.DoPrintPreview()
            Case 3
                ZedGraphControl5.DoPrintPreview()
            Case 4
                ZedGraphControl7.DoPrintPreview()
            Case 5
                ZedGraphControl1.DoPrintPreview()
            Case 6
                ZedGraphControl2.DoPrintPreview()

        End Select
    End Sub

    Private Sub 恢复默认大小ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 恢复默认大小ToolStripMenuItem.Click
        GraphSel.ShowDialog()
        Select Case GraphSel.ComboBox1.SelectedIndex
            Case 0
                ZedGraphControl4.ZoomOutAll(ZedGraphControl4.GraphPane)
            Case 1
                ZedGraphControl3.ZoomOutAll(ZedGraphControl3.GraphPane)
            Case 2
                ZedGraphControl6.ZoomOutAll(ZedGraphControl6.GraphPane)
            Case 3
                ZedGraphControl5.ZoomOutAll(ZedGraphControl5.GraphPane)
            Case 4
                ZedGraphControl7.ZoomOutAll(ZedGraphControl7.GraphPane)
            Case 5
                ZedGraphControl1.ZoomOutAll(ZedGraphControl1.GraphPane)
            Case 6
                ZedGraphControl2.ZoomOutAll(ZedGraphControl2.GraphPane)

        End Select
    End Sub

    Private Sub 返回ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 返回ToolStripMenuItem.Click
        PictureBox17_Click(sender, e)
    End Sub

    Private Sub 退出ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 退出ToolStripMenuItem.Click
        'Me.Close()
        Try
            demoData.Abort()
            demoData2.Abort()
            demoData3.Abort()
            thShowGroundAcc.Abort()
            thShowHum.Abort()
            thShowTemp.Abort()
            thShowWeight.Abort()

        Catch ex As Exception

        End Try

        'Environment.Exit(0)
        Shell("TASKKILL /F /IM SHM.exe")
        System.Threading.Thread.Sleep(10000)
    End Sub

    Private Sub 联系ToolStripMenuItem_Click(sender As Object, e As EventArgs)
        System.Diagnostics.Process.Start("mailto:ly7628@hit.edu.cn")
    End Sub

    Private Sub 打开ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 打开ToolStripMenuItem.Click

        If OpenFileDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
            Dim fileSel = OpenFileDialog1.FileName
            Form3.Show()
            Dim th1 As New System.Threading.Thread(New System.Threading.ParameterizedThreadStart(AddressOf Form3.readfile3))
            th1.Start(fileSel)
            Me.Hide()
            Form3.Text = "SP10-SP12桥 - 英雄山路立交桥梁集群安全运营监测与预警系统"
            bridgeSelected = 3
            Form3.Label6.Text = ""
            Form3.TextBox1.Text = ""
            Form3.RadioButton4.Enabled = False
            Form3.RadioButton2.Checked = True
            '调整传感器位置偏移量
            Dim pyX As Integer = 68
            Dim pyY As Integer = 52

            Form3.PictureBox23.Location = New System.Drawing.Point(113 + pyX, 369 + pyY)
            Form3.PictureBox26.Location = New System.Drawing.Point(161 + pyX, 369 + pyY)
            Form3.PictureBox18.Location = New System.Drawing.Point(251 + pyX, 366 + pyY)
            Form3.PictureBox27.Location = New System.Drawing.Point(318 + pyX, 369 + pyY)
            Form3.PictureBox24.Location = New System.Drawing.Point(368 + pyX, 369 + pyY)
            Form3.PictureBox31.Location = New System.Drawing.Point(113 + pyX, 337 + pyY)
            Form3.PictureBox29.Location = New System.Drawing.Point(162 + pyX, 337 + pyY)
            Form3.PictureBox32.Location = New System.Drawing.Point(240 + pyX, 336 + pyY)
            Form3.PictureBox28.Location = New System.Drawing.Point(318 + pyX, 337 + pyY)
            Form3.PictureBox30.Location = New System.Drawing.Point(368 + pyX, 337 + pyY)

            Form3.PictureBox33.Visible = True

            '设置加速度路径名
            ReDim Form3.accfolders(6)
            Form3.accfolders = {"11013", "11014", "11015", "11016", "11017", "11018"}

            '设置位移路径名
            ReDim Form3.dispfolders(12)
            Form3.dispfolders = {"16028", "16025", "16029", "16026", "16030", "16027", "16034", "16031", "16035", "16032", "16036", "16033"}

            '设置应变传感器编号
            ReDim Form3.sensornum(25)
            Form3.sensornum = {"088822", "084780", "088813", "084784", "084791", "092427", "092428", "084785", "088819", "092436", "088815", "088838", "088839", "088840", "088841"}
            ReDim Form3.strainfolders(15)
            Form3.strainfolders = {"13022", "13021", "13020", "13019", "13018", "13027", "13026", "13025", "13024", "13023", "13032", "13031", "13030", "13029", "13028"}


            '设置桩号
            Form3.Label7.Text = "SP10"
            Form3.Label8.Text = "SP11"
            Form3.Label9.Text = "SP12"

            '更改立体图中测点显示位置
            Form3.PictureBox25.Location = New Drawing.Point(463, 118)
            Form3.PictureBox2.Location = New Drawing.Point(401, 144)

            Form3.PictureBox22.Location = New Drawing.Point(335, 170)
            Form3.PictureBox20.Location = New Drawing.Point(301, 184)
            Form3.PictureBox21.Location = New Drawing.Point(266, 199)

            Form3.PictureBox16.Location = New Drawing.Point(195, 227)
            Form3.PictureBox19.Location = New Drawing.Point(161, 241)
            Form3.PictureBox17.Location = New Drawing.Point(126, 256)
        End If

    End Sub








    Private Sub 查看帮助VToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 查看帮助VToolStripMenuItem.Click
        System.Diagnostics.Process.Start("readme.pdf")
    End Sub

    Private Sub 关于AToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 关于AToolStripMenuItem.Click
        关于.Show()
    End Sub

    Private Sub 编辑EToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 编辑EToolStripMenuItem.Click

    End Sub

    Private Sub 设置SToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 设置SToolStripMenuItem.Click
        PictureBox13_Click(sender, e)
    End Sub

    Private Sub 网络组成NToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 网络组成NToolStripMenuItem.Click
        PictureBox15_Click(sender, e)
    End Sub

    Private Sub 传感器数目EToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 传感器数目EToolStripMenuItem.Click
        PictureBox16_Click(sender, e)
    End Sub


    Private Sub TCPReceive2()
        Dim serverIP As IPAddress = IPAddress.Parse(My.Settings.ServerIP)
        'Dim serverIP As IPAddress = IPAddress.Parse("192.168.1.66")
        Dim serverPort As Integer = My.Settings.ServerPort
        'Dim serverPort As Integer = 26546
        Dim ip As String = My.Settings.ServerIP
        'Dim ip As String = "192.168.1.66"


        Dim tcpClient As TcpClient = Nothing
        Dim networkStream As NetworkStream = Nothing
        Dim strbuf() As Byte
        Dim length As Integer = 0
        Dim cnt As Integer = 0
        Dim lenbytes() As Byte = {0, 0, 0, 0}
        Dim buffersize As Integer = 10240
        While True
            Try
                '与目标主机建立连接
                tcpClient = New TcpClient(ip, serverPort)
                Console.WriteLine("Connected to " + ip.ToString)
                tcpClient.ReceiveBufferSize = buffersize

                '获得用于网络访问的数据流
                networkStream = tcpClient.GetStream
                networkStream.ReadTimeout = 2000

                '等候并读取回应信息
                While True
                    Try
                        cnt = networkStream.Read(lenbytes, 0, 4)


                    Catch ex As Exception
                        '出现IOException，为超时
                        Exit While
                        'Continue While
                    End Try

                    If cnt = 0 Then
                        '未出现 IOException 但 cnt 为 0, 为网络连接异常
                        Console.WriteLine("网络连接异常")
                        Exit While
                    End If

                    length = BitConverter.ToUInt32(lenbytes, 0)


                    If (length < 4000 Or length > 60000) Then
                        Console.WriteLine("length")
                        '标记：待测试时调整
                        'Exit While
                    End If
                    ReDim strbuf(length)
                    Try
                        cnt = 0
                        Dim cntthis As Integer = 0
                        While (cnt < length)
                            If (length - cnt <= buffersize) Then
                                cntthis = networkStream.Read(strbuf, cnt, length - cnt)
                                cnt += cntthis
                            Else
                                cntthis = networkStream.Read(strbuf, cnt, buffersize)
                                cnt += cntthis
                            End If
                            If cntthis = 0 Then
                                Console.WriteLine("CNT THIS is zero")
                                networkStream.Close()
                                tcpClient.Close()
                            End If
                        End While

                        Label75.Text = CInt(Label75.Text) + 1

                        'msgQ.Enqueue(bytes)

                        '获取子包个数
                        Dim subpackets As UInt16 = BitConverter.ToUInt16(strbuf, 0)



                        '当前子包开始位置（在strbuf中的索引号）
                        Dim subpacstart As Integer = 2

                        '循环读取子包
                        For subpacnow = 0 To subpackets - 1

                            '获取子包长度
                            Dim subpaclen As UInt16 = BitConverter.ToUInt16(strbuf, subpacstart)

                            '该子包内包含的浮点数个数
                            Dim datalen As Integer = (subpaclen - 18) / 4

                            '获取测点名
                            Dim pointnum As UInt16 = BitConverter.ToUInt16(strbuf, subpacstart + 2)

                            If pointnum = allChNums(curBridgeNums(0)) Then
                                '如果测点名为指定测点，则读取其中数据
                                For i = 0 To datalen - 1
                                    'data2(i) = BitConverter.ToSingle(strbuf, subpacstart + 20 + 4 * i)
                                    'dataquene2.Enqueue(data2(i))
                                    dataquene1.Enqueue(BitConverter.ToSingle(strbuf, subpacstart + 20 + 4 * i))
                                Next

                                '如果队列过长，则清空数据
                                If dataquene1.Count > 600 Then
                                    dataquene1.Clear()
                                End If





                                'Dim j As Integer = 0
                                'Dim datamin As Single
                                'Dim datamax As Single
                                'For i = 0 To datalen - 1
                                '    If j = 0 Then
                                '        datamin = BitConverter.ToSingle(strbuf, subpacstart + 20 + 4 * i)
                                '        datamax = datamin
                                '    Else
                                '        Dim datanow As Single = BitConverter.ToSingle(strbuf, subpacstart + 20 + 4 * i)
                                '        datamax = IIf(datanow > datamax, datanow, datamax)
                                '        datamin = IIf(datanow < datamin, datanow, datamin)
                                '    End If

                                '    j = j + 1

                                '    If j = 4 Then
                                '        j = 0
                                '        dataquene1.Enqueue(datamax)
                                '        dataquene1.Enqueue(datamin)
                                '    End If

                                '    'data1(i) = BitConverter.ToSingle(strbuf, subpacstart + 20 + 4 * i)
                                '    'dataquene1.Enqueue(data1(i))
                                '    'dataquene1.Enqueue(BitConverter.ToSingle(strbuf, subpacstart + 20 + 4 * i))
                                'Next
                            End If

                            If pointnum = allChNums(curBridgeNums(1)) Then
                                '如果测点名为指定测点，则读取其中数据
                                'Dim data2(datalen - 1) As Single
                                For i = 0 To datalen - 1
                                    'data2(i) = BitConverter.ToSingle(strbuf, subpacstart + 20 + 4 * i)
                                    'dataquene2.Enqueue(data2(i))
                                    dataquene2.Enqueue(BitConverter.ToSingle(strbuf, subpacstart + 20 + 4 * i))
                                Next

                                '如果队列过长，则清空数据
                                If dataquene2.Count > 600 Then
                                    dataquene2.Clear()
                                End If

                            End If

                            If pointnum = allChNums(curBridgeNums(2)) Then
                                '如果测点名为指定测点，则读取其中数据
                                'Dim data3(datalen - 1) As Single
                                For i = 0 To datalen - 1
                                    'data3(i) = BitConverter.ToSingle(strbuf, subpacstart + 20 + 4 * i)
                                    'dataquene3.Enqueue(data3(i))
                                    dataquene3.Enqueue(BitConverter.ToSingle(strbuf, subpacstart + 20 + 4 * i))
                                Next

                                '如果队列过长，则清空数据
                                If dataquene3.Count > 600 Then
                                    dataquene3.Clear()
                                End If

                            End If

                            If pointnum = 15001 Then
                                '如果测点名为指定测点，则读取其中数据
                                'Dim data3(datalen - 1) As Single
                                'data3(i) = BitConverter.ToSingle(strbuf, subpacstart + 20 + 4 * i)
                                'dataquene3.Enqueue(data3(i))

                                humquene1.Enqueue(BitConverter.ToSingle(strbuf, subpacstart + 20))
                                '如果队列过长，则清空数据
                                If tempquene1.Count > 3 Then
                                    tempquene1.Clear()
                                End If

                            End If

                            If pointnum = 15002 Then
                                '如果测点名为指定测点，则读取其中数据
                                'Dim data3(datalen - 1) As Single
                                'data3(i) = BitConverter.ToSingle(strbuf, subpacstart + 20 + 4 * i)
                                'dataquene3.Enqueue(data3(i))
                                tempquene1.Enqueue(BitConverter.ToSingle(strbuf, subpacstart + 20))

                                '如果队列过长，则清空数据
                                If humquene1.Count > 2 Then
                                    humquene1.Clear()
                                End If

                            End If


                            If pointnum = 15003 Then
                                '如果测点名为指定测点，则读取其中数据
                                'Dim data3(datalen - 1) As Single
                                'data3(i) = BitConverter.ToSingle(strbuf, subpacstart + 20 + 4 * i)
                                'dataquene3.Enqueue(data3(i))
                                humquene2.Enqueue(BitConverter.ToSingle(strbuf, subpacstart + 20))

                                '如果队列过长，则清空数据
                                If tempquene2.Count > 2 Then
                                    tempquene2.Clear()
                                End If

                            End If

                            If pointnum = 15004 Then
                                '如果测点名为指定测点，则读取其中数据
                                'Dim data3(datalen - 1) As Single
                                'data3(i) = BitConverter.ToSingle(strbuf, subpacstart + 20 + 4 * i)
                                'dataquene3.Enqueue(data3(i))

                                tempquene2.Enqueue(BitConverter.ToSingle(strbuf, subpacstart + 20))
                                '如果队列过长，则清空数据
                                If humquene2.Count > 2 Then
                                    humquene2.Clear()
                                End If

                            End If

                            subpacstart = subpacstart + 20 + datalen * 4

                        Next







                    Catch ex As Exception
                        Console.WriteLine(">没有收到完整数据, 断开连接")
                        Exit While
                    End Try


                End While
            Catch ex As Exception
                Console.WriteLine(ex.Message)
            Finally
                '关闭连接
                If (Not networkStream Is Nothing) Then
                    networkStream.Close()
                    If (Not tcpClient Is Nothing) Then
                        tcpClient.Close()
                    End If
                End If
                System.Threading.Thread.Sleep(1000)
            End Try
        End While

        Console.WriteLine("STOPPP!")





    End Sub

    Private Sub TCPReceiveSystem3()
        Dim serverIP As IPAddress = IPAddress.Parse(My.Settings.ServerIP)
        'Dim serverIP As IPAddress = IPAddress.Parse("192.168.1.66")
        Dim serverPort As Integer = My.Settings.ServerPort3
        'Dim serverPort As Integer = 26546
        Dim ip As String = My.Settings.ServerIP
        'Dim ip As String = "192.168.1.66"


        Dim tcpClient As TcpClient = Nothing
        Dim networkStream As NetworkStream = Nothing
        Dim strbuf() As Byte
        Dim length As Integer = 0
        Dim cnt As Integer = 0
        Dim lenbytes() As Byte = {0, 0, 0, 0}
        Dim buffersize As Integer = 10240
        While True
            Try
                '与目标主机建立连接
                tcpClient = New TcpClient(ip, serverPort)
                Console.WriteLine("Connected to " + ip.ToString)
                tcpClient.ReceiveBufferSize = buffersize

                '获得用于网络访问的数据流
                networkStream = tcpClient.GetStream
                networkStream.ReadTimeout = 2000

                '等候并读取回应信息
                While True
                    Try
                        cnt = networkStream.Read(lenbytes, 0, 4)


                    Catch ex As Exception
                        '出现IOException，为超时
                        Exit While
                        'Continue While
                    End Try

                    If cnt = 0 Then
                        '未出现 IOException 但 cnt 为 0, 为网络连接异常
                        Console.WriteLine("网络连接异常")
                        Exit While
                    End If

                    length = BitConverter.ToUInt32(lenbytes, 0)


                    If (length < 4000 Or length > 60000) Then
                        Console.WriteLine("length")
                        '标记：待测试时调整
                        'Exit While
                    End If
                    ReDim strbuf(length)
                    Try
                        cnt = 0
                        Dim cntthis As Integer = 0
                        While (cnt < length)
                            If (length - cnt <= buffersize) Then
                                cntthis = networkStream.Read(strbuf, cnt, length - cnt)
                                cnt += cntthis
                            Else
                                cntthis = networkStream.Read(strbuf, cnt, buffersize)
                                cnt += cntthis
                            End If
                            If cntthis = 0 Then
                                Console.WriteLine("CNT THIS is zero")
                                networkStream.Close()
                                tcpClient.Close()
                            End If
                        End While

                        Label75.Text = CInt(Label75.Text) + 1

                        'msgQ.Enqueue(bytes)

                        '获取子包个数
                        Dim subpackets As UInt16 = BitConverter.ToUInt16(strbuf, 0)



                        '当前子包开始位置（在strbuf中的索引号）
                        Dim subpacstart As Integer = 2

                        '循环读取子包
                        For subpacnow = 0 To subpackets - 1

                            '获取子包长度
                            Dim subpaclen As UInt16 = BitConverter.ToUInt16(strbuf, subpacstart)

                            '该子包内包含的浮点数个数
                            Dim datalen As Integer = (subpaclen - 18) / 4

                            '获取测点名
                            Dim pointnum As UInt16 = BitConverter.ToUInt16(strbuf, subpacstart + 2)

                            If pointnum = allChNums(curBridgeNums(0)) Then
                                '如果测点名为指定测点，则读取其中数据
                                For i = 0 To datalen - 1
                                    'data2(i) = BitConverter.ToSingle(strbuf, subpacstart + 20 + 4 * i)
                                    'dataquene2.Enqueue(data2(i))
                                    dataquene1.Enqueue(BitConverter.ToSingle(strbuf, subpacstart + 20 + 4 * i))
                                Next

                                '如果队列过长，则清空数据
                                If dataquene1.Count > 600 Then
                                    dataquene1.Clear()
                                End If





                                'Dim j As Integer = 0
                                'Dim datamin As Single
                                'Dim datamax As Single
                                'For i = 0 To datalen - 1
                                '    If j = 0 Then
                                '        datamin = BitConverter.ToSingle(strbuf, subpacstart + 20 + 4 * i)
                                '        datamax = datamin
                                '    Else
                                '        Dim datanow As Single = BitConverter.ToSingle(strbuf, subpacstart + 20 + 4 * i)
                                '        datamax = IIf(datanow > datamax, datanow, datamax)
                                '        datamin = IIf(datanow < datamin, datanow, datamin)
                                '    End If

                                '    j = j + 1

                                '    If j = 4 Then
                                '        j = 0
                                '        dataquene1.Enqueue(datamax)
                                '        dataquene1.Enqueue(datamin)
                                '    End If

                                '    'data1(i) = BitConverter.ToSingle(strbuf, subpacstart + 20 + 4 * i)
                                '    'dataquene1.Enqueue(data1(i))
                                '    'dataquene1.Enqueue(BitConverter.ToSingle(strbuf, subpacstart + 20 + 4 * i))
                                'Next
                            End If

                            If pointnum = allChNums(curBridgeNums(1)) Then
                                '如果测点名为指定测点，则读取其中数据
                                'Dim data2(datalen - 1) As Single
                                For i = 0 To datalen - 1
                                    'data2(i) = BitConverter.ToSingle(strbuf, subpacstart + 20 + 4 * i)
                                    'dataquene2.Enqueue(data2(i))
                                    dataquene2.Enqueue(BitConverter.ToSingle(strbuf, subpacstart + 20 + 4 * i))
                                Next

                                '如果队列过长，则清空数据
                                If dataquene2.Count > 600 Then
                                    dataquene2.Clear()
                                End If

                            End If

                            If pointnum = allChNums(curBridgeNums(2)) Then
                                '如果测点名为指定测点，则读取其中数据
                                'Dim data3(datalen - 1) As Single
                                For i = 0 To datalen - 1
                                    'data3(i) = BitConverter.ToSingle(strbuf, subpacstart + 20 + 4 * i)
                                    'dataquene3.Enqueue(data3(i))
                                    dataquene3.Enqueue(BitConverter.ToSingle(strbuf, subpacstart + 20 + 4 * i))
                                Next

                                '如果队列过长，则清空数据
                                If dataquene3.Count > 600 Then
                                    dataquene3.Clear()
                                End If

                            End If

                            If pointnum = 15001 Then
                                '如果测点名为指定测点，则读取其中数据
                                'Dim data3(datalen - 1) As Single
                                'data3(i) = BitConverter.ToSingle(strbuf, subpacstart + 20 + 4 * i)
                                'dataquene3.Enqueue(data3(i))

                                humquene1.Enqueue(BitConverter.ToSingle(strbuf, subpacstart + 20))
                                '如果队列过长，则清空数据
                                If tempquene1.Count > 3 Then
                                    tempquene1.Clear()
                                End If

                            End If

                            If pointnum = 15002 Then
                                '如果测点名为指定测点，则读取其中数据
                                'Dim data3(datalen - 1) As Single
                                'data3(i) = BitConverter.ToSingle(strbuf, subpacstart + 20 + 4 * i)
                                'dataquene3.Enqueue(data3(i))
                                tempquene1.Enqueue(BitConverter.ToSingle(strbuf, subpacstart + 20))

                                '如果队列过长，则清空数据
                                If humquene1.Count > 2 Then
                                    humquene1.Clear()
                                End If

                            End If


                            If pointnum = 15003 Then
                                '如果测点名为指定测点，则读取其中数据
                                'Dim data3(datalen - 1) As Single
                                'data3(i) = BitConverter.ToSingle(strbuf, subpacstart + 20 + 4 * i)
                                'dataquene3.Enqueue(data3(i))
                                humquene2.Enqueue(BitConverter.ToSingle(strbuf, subpacstart + 20))

                                '如果队列过长，则清空数据
                                If tempquene2.Count > 2 Then
                                    tempquene2.Clear()
                                End If

                            End If

                            If pointnum = 15004 Then
                                '如果测点名为指定测点，则读取其中数据
                                'Dim data3(datalen - 1) As Single
                                'data3(i) = BitConverter.ToSingle(strbuf, subpacstart + 20 + 4 * i)
                                'dataquene3.Enqueue(data3(i))

                                tempquene2.Enqueue(BitConverter.ToSingle(strbuf, subpacstart + 20))
                                '如果队列过长，则清空数据
                                If humquene2.Count > 2 Then
                                    humquene2.Clear()
                                End If

                            End If

                            subpacstart = subpacstart + 20 + datalen * 4

                        Next







                    Catch ex As Exception
                        Console.WriteLine(">没有收到完整数据, 断开连接")
                        Exit While
                    End Try


                End While
            Catch ex As Exception
                Console.WriteLine(ex.Message)
            Finally
                '关闭连接
                If (Not networkStream Is Nothing) Then
                    networkStream.Close()
                    If (Not tcpClient Is Nothing) Then
                        tcpClient.Close()
                    End If
                End If
                System.Threading.Thread.Sleep(1000)
            End Try
        End While

        Console.WriteLine("STOPPP!")





    End Sub

    Private Sub TCPReceiveSystem2()
        Dim serverIP As IPAddress = IPAddress.Parse(My.Settings.ServerIP)
        'Dim serverIP As IPAddress = IPAddress.Parse("192.168.1.66")
        Dim serverPort As Integer = My.Settings.ServerPort2
        'Dim serverPort As Integer = 26546
        Dim ip As String = My.Settings.ServerIP
        'Dim ip As String = "192.168.1.66"


        Dim tcpClient As TcpClient = Nothing
        Dim networkStream As NetworkStream = Nothing
        Dim strbuf() As Byte
        Dim length As Integer = 0
        Dim cnt As Integer = 0
        Dim lenbytes() As Byte = {0, 0, 0, 0}
        Dim buffersize As Integer = 10240
        While True
            Try
                '与目标主机建立连接
                tcpClient = New TcpClient(ip, serverPort)
                Console.WriteLine("Connected to " + ip.ToString)
                tcpClient.ReceiveBufferSize = buffersize

                '获得用于网络访问的数据流
                networkStream = tcpClient.GetStream
                networkStream.ReadTimeout = 2000

                '等候并读取回应信息
                While True
                    Try
                        cnt = networkStream.Read(lenbytes, 0, 4)


                    Catch ex As Exception
                        '出现IOException，为超时
                        Exit While
                        'Continue While
                    End Try

                    If cnt = 0 Then
                        '未出现 IOException 但 cnt 为 0, 为网络连接异常
                        Console.WriteLine("网络连接异常")
                        Exit While
                    End If

                    length = BitConverter.ToUInt32(lenbytes, 0)


                    If (length < 4000 Or length > 60000) Then
                        Console.WriteLine("length")
                        '标记：待测试时调整
                        'Exit While
                    End If
                    ReDim strbuf(length)
                    Try
                        cnt = 0
                        Dim cntthis As Integer = 0
                        While (cnt < length)
                            If (length - cnt <= buffersize) Then
                                cntthis = networkStream.Read(strbuf, cnt, length - cnt)
                                cnt += cntthis
                            Else
                                cntthis = networkStream.Read(strbuf, cnt, buffersize)
                                cnt += cntthis
                            End If
                            If cntthis = 0 Then
                                Console.WriteLine("CNT THIS is zero")
                                networkStream.Close()
                                tcpClient.Close()
                            End If
                        End While

                        Label75.Text = CInt(Label75.Text) + 1

                        'msgQ.Enqueue(bytes)

                        '获取子包个数
                        Dim subpackets As UInt16 = BitConverter.ToUInt16(strbuf, 0)



                        '当前子包开始位置（在strbuf中的索引号）
                        Dim subpacstart As Integer = 2

                        '循环读取子包
                        For subpacnow = 0 To subpackets - 1

                            '获取子包长度
                            Dim subpaclen As UInt16 = BitConverter.ToUInt16(strbuf, subpacstart)

                            '该子包内包含的浮点数个数
                            Dim datalen As Integer = (subpaclen - 18) / 4

                            '获取测点名
                            Dim pointnum As UInt16 = BitConverter.ToUInt16(strbuf, subpacstart + 2)

                            If pointnum = allChNums(curBridgeNums(0)) Then
                                '如果测点名为指定测点，则读取其中数据
                                For i = 0 To datalen - 1
                                    'data2(i) = BitConverter.ToSingle(strbuf, subpacstart + 20 + 4 * i)
                                    'dataquene2.Enqueue(data2(i))
                                    dataquene1.Enqueue(BitConverter.ToSingle(strbuf, subpacstart + 20 + 4 * i))
                                Next

                                '如果队列过长，则清空数据
                                If dataquene1.Count > 600 Then
                                    dataquene1.Clear()
                                End If





                                'Dim j As Integer = 0
                                'Dim datamin As Single
                                'Dim datamax As Single
                                'For i = 0 To datalen - 1
                                '    If j = 0 Then
                                '        datamin = BitConverter.ToSingle(strbuf, subpacstart + 20 + 4 * i)
                                '        datamax = datamin
                                '    Else
                                '        Dim datanow As Single = BitConverter.ToSingle(strbuf, subpacstart + 20 + 4 * i)
                                '        datamax = IIf(datanow > datamax, datanow, datamax)
                                '        datamin = IIf(datanow < datamin, datanow, datamin)
                                '    End If

                                '    j = j + 1

                                '    If j = 4 Then
                                '        j = 0
                                '        dataquene1.Enqueue(datamax)
                                '        dataquene1.Enqueue(datamin)
                                '    End If

                                '    'data1(i) = BitConverter.ToSingle(strbuf, subpacstart + 20 + 4 * i)
                                '    'dataquene1.Enqueue(data1(i))
                                '    'dataquene1.Enqueue(BitConverter.ToSingle(strbuf, subpacstart + 20 + 4 * i))
                                'Next
                            End If

                            If pointnum = allChNums(curBridgeNums(1)) Then
                                '如果测点名为指定测点，则读取其中数据
                                'Dim data2(datalen - 1) As Single
                                For i = 0 To datalen - 1
                                    'data2(i) = BitConverter.ToSingle(strbuf, subpacstart + 20 + 4 * i)
                                    'dataquene2.Enqueue(data2(i))
                                    dataquene2.Enqueue(BitConverter.ToSingle(strbuf, subpacstart + 20 + 4 * i))
                                Next

                                '如果队列过长，则清空数据
                                If dataquene2.Count > 600 Then
                                    dataquene2.Clear()
                                End If

                            End If

                            If pointnum = allChNums(curBridgeNums(2)) Then
                                '如果测点名为指定测点，则读取其中数据
                                'Dim data3(datalen - 1) As Single
                                For i = 0 To datalen - 1
                                    'data3(i) = BitConverter.ToSingle(strbuf, subpacstart + 20 + 4 * i)
                                    'dataquene3.Enqueue(data3(i))
                                    dataquene3.Enqueue(BitConverter.ToSingle(strbuf, subpacstart + 20 + 4 * i))
                                Next

                                '如果队列过长，则清空数据
                                If dataquene3.Count > 600 Then
                                    dataquene3.Clear()
                                End If

                            End If

                            If pointnum = 15001 Then
                                '如果测点名为指定测点，则读取其中数据
                                'Dim data3(datalen - 1) As Single
                                'data3(i) = BitConverter.ToSingle(strbuf, subpacstart + 20 + 4 * i)
                                'dataquene3.Enqueue(data3(i))

                                humquene1.Enqueue(BitConverter.ToSingle(strbuf, subpacstart + 20))
                                '如果队列过长，则清空数据
                                If tempquene1.Count > 3 Then
                                    tempquene1.Clear()
                                End If

                            End If

                            If pointnum = 15002 Then
                                '如果测点名为指定测点，则读取其中数据
                                'Dim data3(datalen - 1) As Single
                                'data3(i) = BitConverter.ToSingle(strbuf, subpacstart + 20 + 4 * i)
                                'dataquene3.Enqueue(data3(i))
                                tempquene1.Enqueue(BitConverter.ToSingle(strbuf, subpacstart + 20))

                                '如果队列过长，则清空数据
                                If humquene1.Count > 2 Then
                                    humquene1.Clear()
                                End If

                            End If


                            If pointnum = 15003 Then
                                '如果测点名为指定测点，则读取其中数据
                                'Dim data3(datalen - 1) As Single
                                'data3(i) = BitConverter.ToSingle(strbuf, subpacstart + 20 + 4 * i)
                                'dataquene3.Enqueue(data3(i))
                                humquene2.Enqueue(BitConverter.ToSingle(strbuf, subpacstart + 20))

                                '如果队列过长，则清空数据
                                If tempquene2.Count > 2 Then
                                    tempquene2.Clear()
                                End If

                            End If

                            If pointnum = 15004 Then
                                '如果测点名为指定测点，则读取其中数据
                                'Dim data3(datalen - 1) As Single
                                'data3(i) = BitConverter.ToSingle(strbuf, subpacstart + 20 + 4 * i)
                                'dataquene3.Enqueue(data3(i))

                                tempquene2.Enqueue(BitConverter.ToSingle(strbuf, subpacstart + 20))
                                '如果队列过长，则清空数据
                                If humquene2.Count > 2 Then
                                    humquene2.Clear()
                                End If

                            End If

                            subpacstart = subpacstart + 20 + datalen * 4

                        Next







                    Catch ex As Exception
                        Console.WriteLine(">没有收到完整数据, 断开连接")
                        Exit While
                    End Try


                End While
            Catch ex As Exception
                Console.WriteLine(ex.Message)
            Finally
                '关闭连接
                If (Not networkStream Is Nothing) Then
                    networkStream.Close()
                    If (Not tcpClient Is Nothing) Then
                        tcpClient.Close()
                    End If
                End If
                System.Threading.Thread.Sleep(1000)
            End Try
        End While

        Console.WriteLine("STOPPP!")





    End Sub


    Private Sub TCPReceive()
        Dim serverIP As IPAddress = IPAddress.Parse("192.168.1.66")
        Dim serverPort As Integer = 26546
        While True
            Try
                Dim s As Socket = Nothing
                s = New Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
                Dim LocalEndPoint As New IPEndPoint(serverIP, serverPort)
                s.Connect(LocalEndPoint)
                Dim packets As Integer = 0
                'While True

                Try
                    While True
                        Dim strbuf(10000000) As Byte

                        Dim strlen As Integer = s.Receive(strbuf)
                        Label75.Text = CInt(Label75.Text) + 1
                        Console.WriteLine(strlen)

                        '获取数据包总长度
                        Dim paclen As UInt32 = BitConverter.ToUInt32(strbuf, 0)

                        If ((strlen <> paclen + 4) Or strlen <= 0) Then
                            Console.WriteLine("skipped, haven't received " + paclen.ToString)
                            Thread.Sleep(100)
                            Continue While
                        End If

                        '获取子包个数
                        Dim subpackets As UInt16 = BitConverter.ToUInt16(strbuf, 4)

                        '当前子包开始位置（在strbuf中的索引号）
                        Dim subpacstart As Integer = 6

                        '循环读取子包
                        For subpacnow = 0 To subpackets - 1

                            '获取子包长度
                            Dim subpaclen As UInt16 = BitConverter.ToUInt16(strbuf, subpacstart)

                            '该子包内包含的浮点数个数
                            Dim datalen As Integer = (subpaclen - 18) / 4

                            '获取测点名
                            Dim pointnum As UInt16 = BitConverter.ToUInt16(strbuf, subpacstart + 2)

                            If pointnum = 11001 Then
                                '如果测点名为指定测点，则读取其中数据
                                Dim data1(datalen - 1) As Single
                                For i = 0 To datalen - 1
                                    data1(i) = BitConverter.ToSingle(strbuf, subpacstart + 20 + 4 * i)
                                    dataquene1.Enqueue(data1(i))
                                Next
                            End If

                            If pointnum = 11002 Then
                                '如果测点名为指定测点，则读取其中数据
                                Dim data2(datalen - 1) As Single
                                For i = 0 To datalen - 1
                                    data2(i) = BitConverter.ToSingle(strbuf, subpacstart + 20 + 4 * i)
                                    dataquene2.Enqueue(data2(i))
                                Next
                            End If

                            If pointnum = 11003 Then
                                '如果测点名为指定测点，则读取其中数据
                                Dim data3(datalen - 1) As Single
                                For i = 0 To datalen - 1
                                    data3(i) = BitConverter.ToSingle(strbuf, subpacstart + 20 + 4 * i)
                                    dataquene3.Enqueue(data3(i))
                                Next
                            End If

                            subpacstart = subpacstart + 20 + datalen * 4

                        Next

                    End While
                Catch ex As Exception

                End Try

                'Try
                '    While True

                '        '获取数据包总长度
                '        Dim lenbytes(3) As Byte
                '        Dim strLen As Integer = s.Receive(lenbytes, 4, SocketFlags.None)
                '        If strLen <= 0 Then
                '            Throw New SocketException("No More Data")
                '        End If
                '        Dim uintlenTotal As UInt32 = BitConverter.ToUInt32(lenbytes, 0)

                '        '获取子包个数
                '        Dim subpackets(1) As Byte
                '        If (s.Receive(subpackets, 2, SocketFlags.None) <= 0) Then
                '            Throw New SocketException("No More Data")
                '        End If
                '        Dim uintSubPac As UInt16 = BitConverter.ToUInt16(subpackets, 0)

                '        Dim strbuf2d(uintSubPac - 1)() As Byte

                '        For pacnow = 0 To uintSubPac - 1
                '            '获取子包长度
                '            Dim lenpac(1) As Byte
                '            If (s.Receive(lenpac, 2, SocketFlags.None) <= 0) Then
                '                Throw New SocketException("No More Data")
                '            End If
                '            Dim uintpac As UInt16 = BitConverter.ToUInt16(lenpac, 0)

                '            '接收子包
                '            ReDim strbuf2d(pacnow)(uintpac - 1)
                '            If (s.Receive(strbuf2d(pacnow), uintpac, SocketFlags.None) <= 0) Then
                '                Throw New SocketException("No More Data")
                '            End If

                '            '获取测点名
                '            Dim pointnum As UInt16 = BitConverter.ToUInt16(strbuf2d(pacnow), 0)


                '        Next
                '        Label75.Text = CInt(Label75.Text) + 1
                '    End While


                '    'Label1.Text = packets
                '    ''TextBox1.Text = packets.ToString
                '    'packets += 1
                '    'Dim aa As Integer = s.Receive(strbuf)
                '    'Dim bb As String = (Encoding.Default.GetString(strbuf, 0, 2))
                '    ''Console.WriteLine(bb)
                '    'For i = 0 To strLen
                '    '    Dim aaaa = strbuf(i)
                '    'Next
                'Catch ex As Exception
                '    'Thread.Sleep(100)
                'Finally
                '    s.Close()
                'End Try



                'MsgBox(Encoding.ASCII.GetString(strbuf, 0, strLen))

                'End While
            Catch ex As Exception
                Thread.Sleep(1000)
            End Try

        End While

    End Sub

    Private Sub PictureBox21_Click(sender As Object, e As EventArgs) Handles PictureBox21.Click
        '单击上一页按钮
        'MsgBox(Form1.bridgeNames(curBridgeNums(0)))
        If (curBridgeNums(0) < 3) Then
            curBridgeNums = {0, 1, 2}
        Else
            For i = 0 To 2
                curBridgeNums(i) = curBridgeNums(i) - 3
            Next
        End If
        Label6.Text = Form1.bridgeNames(curBridgeNums(0)) & "监测状态"
        Label23.Text = Form1.bridgeNames(curBridgeNums(1)) & "监测状态"
        Label40.Text = Form1.bridgeNames(curBridgeNums(2)) & "监测状态"
        'MsgBox(Form1.bridgeNames(curBridgeNums(0)) & "," & Form1.bridgeNames(curBridgeNums(1)) & "," & Form1.bridgeNames(curBridgeNums(2)))
    End Sub

    Private Sub PictureBox22_Click(sender As Object, e As EventArgs) Handles PictureBox22.Click
        '单击下一页按钮
        If (curBridgeNums(2) > Form1.bridgeNum - 3) Then
            curBridgeNums = {Form1.bridgeNum - 3, Form1.bridgeNum - 2, Form1.bridgeNum - 1}
        Else
            For i = 0 To 2
                curBridgeNums(i) = curBridgeNums(i) + 3
            Next
        End If
        Label6.Text = Form1.bridgeNames(curBridgeNums(0)) & "监测状态"
        Label23.Text = Form1.bridgeNames(curBridgeNums(1)) & "监测状态"
        Label40.Text = Form1.bridgeNames(curBridgeNums(2)) & "监测状态"
        'MsgBox(Form1.bridgeNames(curBridgeNums(0)) & "," & Form1.bridgeNames(curBridgeNums(1)) & "," & Form1.bridgeNames(curBridgeNums(2)))
    End Sub

    Private Sub Button1_Click_1(sender As Object, e As EventArgs)
        curBridgeNums = {0, 1, 2}
        Label6.Text = Form1.bridgeNames(curBridgeNums(0)) & "监测状态"
        Label23.Text = Form1.bridgeNames(curBridgeNums(1)) & "监测状态"
        Label40.Text = Form1.bridgeNames(curBridgeNums(2)) & "监测状态"
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs)
        curBridgeNums = {3, 4, 5}
        Label6.Text = Form1.bridgeNames(curBridgeNums(0)) & "监测状态"
        Label23.Text = Form1.bridgeNames(curBridgeNums(1)) & "监测状态"
        Label40.Text = Form1.bridgeNames(curBridgeNums(2)) & "监测状态"
    End Sub

    Private Sub Button3_Click_1(sender As Object, e As EventArgs)
        curBridgeNums = {6, 7, 8}
        Label6.Text = Form1.bridgeNames(curBridgeNums(0)) & "监测状态"
        Label23.Text = Form1.bridgeNames(curBridgeNums(1)) & "监测状态"
        Label40.Text = Form1.bridgeNames(curBridgeNums(2)) & "监测状态"
    End Sub
End Class