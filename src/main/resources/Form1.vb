Imports MathWorks.MATLAB.NET.Arrays
Imports MathWorks.MATLAB.NET.Utility

Public Class Form1

    '桥梁总数目
    Public bridgeNum As Integer = 9
    Public bridgeNames(bridgeNum) As String

    Private Sub PictureBox1_Click(sender As Object, e As EventArgs)
        Form2.Show()
        Me.Hide()
        Try
            If demoData.IsAlive Then
                If demoData.ThreadState = Threading.ThreadState.Suspended Then
                    Try
                        demoData.Resume()
                        demoData2.Resume()
                        demoData3.Resume()
                    Catch ex As Exception

                    End Try

                End If
            End If
        Catch ex As Exception

        End Try


    End Sub





    Private Sub PictureBox2_Click(sender As Object, e As EventArgs)
        Form3.Show()
        Me.Hide()
    End Sub

    Private Sub Form1_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing

        If MsgBox("是否退出系统？", MsgBoxStyle.OkCancel) = MsgBoxResult.Cancel Then
            e.Cancel = True
            Exit Sub
        End If


        Shell("TASKKILL /F /IM SHM.exe")
        System.Threading.Thread.Sleep(10000)
    End Sub






    Private Sub PictureBox5_Click(sender As Object, e As EventArgs)
        Me.Close()
    End Sub

    Private Sub Label55_Click(sender As Object, e As EventArgs)
        Me.Close()
    End Sub

    Private Sub PictureBox3_Click(sender As Object, e As EventArgs)
        Form3.Show()
        Me.Hide()
    End Sub

    Private Sub PictureBox4_Click(sender As Object, e As EventArgs)
        Form3.Show()
        Me.Hide()
    End Sub


    Private Sub PictureBox1_Click_1(sender As Object, e As EventArgs) Handles PictureBox1.Click
        Form2.Show()
        Me.Hide()
        Dim thform2show As New System.Threading.Thread(AddressOf form2show)
        thform2show.Start()

    End Sub

    Public Sub form2show()
        System.Threading.Thread.Sleep(3000)
        Try
            If demoData.IsAlive Then
                If demoData.ThreadState = Threading.ThreadState.Suspended Then
                    Try
                        demoData.Resume()
                        demoData2.Resume()
                        demoData3.Resume()
                        thShowGroundAcc.Resume()
                        thShowHum.Resume()
                        thShowTemp.Resume()
                        thShowWeight.Resume()
                    Catch ex As Exception

                    End Try

                End If
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub PictureBox2_Click_1(sender As Object, e As EventArgs) Handles PictureBox2.Click
        Form3.Label13.Text = ""
        Form3.Label14.Text = ""

        Me.Hide()
        Form3.Show()
        Form3.Text = "SP4-SP7桥"
        bridgeSelected = 1
        Form3.Label4.Text = "加速度(m/s^2)"
        Form3.Label5.Text = "时间(s)"
        Form3.Label6.Text = ""
        Form3.TextBox1.Text = ""
        Form3.RadioButton4.Enabled = True
        Form3.RadioButton2.Checked = True

        Form3.PictureBox13.BackgroundImage = Image.FromFile("mainimage\jiemian2.png")

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

        Form3.PictureBox33.Visible = False

        '设置加速度路径名
        ReDim Form3.accfolders(6)
        Form3.accfolders = {"11006", "11005", "11004", "11003", "11002", "11001"}

        '设置位移路径名
        ReDim Form3.dispfolders(12)
        Form3.dispfolders = {"16012", "16009", "16011", "16008", "16010", "16007", "16006", "16003", "16005", "16002", "16004", "16001"}

        '设置应变传感器编号
        ReDim Form3.sensornum(24)
        Form3.sensornum = {"087745", "087752", "092432", "092430", "092417", "092429", "092407", "092420", "092422", "092408", "092433", "087749", "087748", "087747", "092434", "092423", "092400", "092431", "092406", "092405", "087746", "092412", "087754", "087751"}
        ReDim Form3.strainfolders(24)
        Form3.strainfolders = {"13056", "13055", "13054", "13053", "13052", "13051", "13050", "13049", "13048", "13047", "13046", "13045", "13044", "13043", "13042", "13041", "13040", "13039", "13038", "13037", "13036", "13035", "13034", "13033"}

        '设置应变传感器零点
        ReDim Form3.strainzero(25)
        Form3.strainzero = {2709.04, 2725.6, 2727.0, 2677.54, 2762.72, 2777.54, 2745.64, 2977.94, 2842.94, 2729.39, 2431.4, 2250.24, 2575.92, 2548.86, 2415.32, 2610.74, 2637.74, 2867.38, 2885.58, 2728.26, 2653.24, 2681.64, 2681.52, 2813.88, 0.0}




        '设置桩号
        Form3.Label7.Text = "SP4"
        Form3.Label8.Text = "SP5"
        Form3.Label9.Text = "SP6"
        Form3.Label10.Text = "SP7"

        '更改立体图中测点显示位置
        Form3.PictureBox25.Location = New Drawing.Point(126, 256)
        Form3.PictureBox2.Location = New Drawing.Point(195, 227)

        Form3.PictureBox22.Location = New Drawing.Point(266, 199)
        Form3.PictureBox20.Location = New Drawing.Point(301, 184)
        Form3.PictureBox21.Location = New Drawing.Point(335, 170)

        Form3.PictureBox16.Location = New Drawing.Point(401, 144)
        Form3.PictureBox19.Location = New Drawing.Point(432, 131)
        Form3.PictureBox17.Location = New Drawing.Point(463, 118)

    End Sub

    Private Sub PictureBox1_MouseDown(sender As Object, e As MouseEventArgs) Handles PictureBox1.MouseDown
        'Me.BackgroundImage = Image.FromFile("mainimage\hk\form2-1.png")
        'PictureBox1.BackgroundImage = Image.FromFile("mainimage\hk\backgroundgray.png")
    End Sub

    Private Sub PictureBox1_MouseUp(sender As Object, e As MouseEventArgs) Handles PictureBox1.MouseUp
        'PictureBox1.BackgroundImage = Nothing
        'Me.BackgroundImage = Image.FromFile("mainimage\hk\form2.png")
    End Sub

    Private Sub PictureBox3_Click_1(sender As Object, e As EventArgs) Handles PictureBox3.Click
        Form3.Label13.Text = ""
        Form3.Label14.Text = ""

        Form3.Show()
        Me.Hide()
        Form3.Text = "SP7-SP10桥"
        bridgeSelected = 2
        Form3.Label4.Text = "加速度(m/s^2)"
        Form3.Label5.Text = "时间(s)"
        Form3.Label6.Text = ""
        Form3.TextBox1.Text = ""
        Form3.RadioButton4.Enabled = True
        Form3.RadioButton2.Checked = True

        Form3.PictureBox13.BackgroundImage = Image.FromFile("mainimage\jiemian.png")

        '调整传感器位置偏移量
        Dim pyX As Integer = 68
        Dim pyY As Integer = 52


        Form3.PictureBox23.Location = New System.Drawing.Point(113 + pyX, 369 + pyY)
        Form3.PictureBox26.Location = New System.Drawing.Point(176 + pyX, 369 + pyY)
        Form3.PictureBox18.Location = New System.Drawing.Point(238 + pyX, 369 + pyY)
        Form3.PictureBox27.Location = New System.Drawing.Point(299 + pyX, 369 + pyY)
        Form3.PictureBox24.Location = New System.Drawing.Point(363 + pyX, 369 + pyY)
        Form3.PictureBox31.Location = New System.Drawing.Point(113 + pyX, 337 + pyY)
        Form3.PictureBox29.Location = New System.Drawing.Point(176 + pyX, 337 + pyY)
        Form3.PictureBox32.Location = New System.Drawing.Point(238 + pyX, 336 + pyY)
        Form3.PictureBox28.Location = New System.Drawing.Point(299 + pyX, 337 + pyY)
        Form3.PictureBox30.Location = New System.Drawing.Point(363 + pyX, 337 + pyY)

        Form3.PictureBox33.Visible = False

        '设置加速度路径名
        ReDim Form3.accfolders(6)
        Form3.accfolders = {"11007", "11008", "11009", "11010", "11011", "11012"}

        '设置位移路径名
        ReDim Form3.dispfolders(12)
        Form3.dispfolders = {"16016", "16013", "16017", "16014", "16018", "16015", "16022", "16019", "16023", "16020", "16024", "16021"}

        '设置应变传感器编号
        ReDim Form3.sensornum(25)
        Form3.sensornum = {"088828", "088833", "088825", "088832", "088823", "084781", "084793", "084783", "088816", "084782", "088826", "088835", "088824", "088827", "088821", "088818", "088829", "088814", "088810", "088830", "084794", "088834", "084792", "088836", "088831"}
        ReDim Form3.strainfolders(25)
        Form3.strainfolders = {"13061", "13060", "13059", "13058", "13057", "13002", "13001", "13064", "13063", "13062", "13007", "13006", "13005", "13004", "13003", "13012", "13011", "13010", "13009", "13008", "13017", "13016", "13015", "13014", "13013"}




        '设置应变传感器零点
        ReDim Form3.strainzero(25)
        Form3.strainzero = {2830.84, 2800.26, 2770.2, 2870.22, 2937.0, 2960.46, 2905.32, 2878.76, 2796.78, 2804.1, 2617.54, 2549.62, 2460.4, 2597.96, 2559.82, 2751.32, 2822.96, 2944.0, 2833.08, 2851.74, 2749.82, 2736.9, 2810.38, 2765.76, 2794.7}






        '设置桩号
        Form3.Label7.Text = "SP7"
        Form3.Label8.Text = "SP8"
        Form3.Label9.Text = "SP9"
        Form3.Label10.Text = "SP10"


        '更改立体图中测点显示位置
        Form3.PictureBox25.Location = New Drawing.Point(463, 118)
        Form3.PictureBox2.Location = New Drawing.Point(401, 144)

        Form3.PictureBox22.Location = New Drawing.Point(335, 170)
        Form3.PictureBox20.Location = New Drawing.Point(301, 184)
        Form3.PictureBox21.Location = New Drawing.Point(266, 199)

        Form3.PictureBox16.Location = New Drawing.Point(195, 227)
        Form3.PictureBox19.Location = New Drawing.Point(161, 241)
        Form3.PictureBox17.Location = New Drawing.Point(126, 256)
    End Sub

    Private Sub PictureBox4_Click_1(sender As Object, e As EventArgs) Handles PictureBox4.Click
        Form3.Label13.Text = ""
        Form3.Label14.Text = ""

        Form3.Show()
        Me.Hide()
        Form3.Text = "SP10-SP12桥"
        bridgeSelected = 3
        Form3.Label4.Text = "加速度(m/s^2)"
        Form3.Label5.Text = "时间(s)"
        Form3.Label6.Text = ""
        Form3.TextBox1.Text = ""
        Form3.RadioButton4.Enabled = False
        Form3.RadioButton2.Checked = True

        Form3.PictureBox13.BackgroundImage = Image.FromFile("mainimage\jiemian2.png")

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
        ReDim Form3.sensornum(15)
        Form3.sensornum = {"088822", "084780", "088813", "084784", "084791", "092427", "092428", "084785", "088819", "092436", "088815", "088838", "088839", "088840", "088841"}
        ReDim Form3.strainfolders(15)
        Form3.strainfolders = {"13022", "13021", "13020", "13019", "13018", "13027", "13026", "13025", "13024", "13023", "13032", "13031", "13030", "13029", "13028"}



        '设置应变传感器零点
        ReDim Form3.strainzero(15)
        Form3.strainzero = {2668.98, 2811.34, 2662.54, 2656.68, 2719.52, 2922.32, 3070.34, 3271.42, 3062.54, 3016.26, 2466.0, 2639.9, 2676.66, 2476.48, 2625.5}





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

    End Sub

    Private Sub PictureBox2_MouseDown(sender As Object, e As MouseEventArgs) Handles PictureBox2.MouseDown
        'Me.BackgroundImage = Image.FromFile("mainimage\hk\form2-2.png")
        PictureBox2.BackgroundImage = Image.FromFile("mainimage\hk\backgroundgray.png")
    End Sub

    Private Sub PictureBox2_MouseUp(sender As Object, e As MouseEventArgs) Handles PictureBox2.MouseUp
        'Me.BackgroundImage = Image.FromFile("mainimage\hk\form2.png")
        PictureBox2.BackgroundImage = Nothing
    End Sub

    Private Sub PictureBox3_MouseDown(sender As Object, e As MouseEventArgs) Handles PictureBox3.MouseDown
        'Me.BackgroundImage = Image.FromFile("mainimage\hk\form2-3.png")
        PictureBox3.BackgroundImage = Image.FromFile("mainimage\hk\backgroundgray.png")
    End Sub

    Private Sub PictureBox3_MouseUp(sender As Object, e As MouseEventArgs) Handles PictureBox3.MouseUp
        'Me.BackgroundImage = Image.FromFile("mainimage\hk\form2.png")
        PictureBox3.BackgroundImage = Nothing
    End Sub

    Private Sub PictureBox4_MouseDown(sender As Object, e As MouseEventArgs) Handles PictureBox4.MouseDown
        'Me.BackgroundImage = Image.FromFile("mainimage\hk\form2-4.png")
        PictureBox4.BackgroundImage = Image.FromFile("mainimage\hk\backgroundgray.png")
    End Sub

    Private Sub PictureBox4_MouseUp(sender As Object, e As MouseEventArgs) Handles PictureBox4.MouseUp
        'Me.BackgroundImage = Image.FromFile("mainimage\hk\form2.png")
        PictureBox4.BackgroundImage = Nothing
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        '初始化各桥梁名称
        bridgeNames(0) = "SP4-SP7桥"
        bridgeNames(1) = "SP7-SP10桥"
        bridgeNames(2) = "SP10-SP12桥"
        bridgeNames(3) = "A0-P22a桥"
        bridgeNames(4) = "A0-P22a桥"
        bridgeNames(5) = "A0-P22a桥"
        bridgeNames(6) = "B3-P36桥"
        bridgeNames(7) = "B3-P36桥"
        bridgeNames(8) = "B3-P36桥"

        AddHandler Application.ThreadException, AddressOf UIThreadException
        'Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException)
        AddHandler AppDomain.CurrentDomain.UnhandledException, AddressOf CurrentDomain_UnhandledException

        Control.CheckForIllegalCrossThreadCalls = False

        Me.FormBorderStyle = Windows.Forms.FormBorderStyle.FixedSingle


        '获取启动时的命令行参数，如果参数数量大于1，则说明是每日2点数据处理之后系统重启，此时应对系统进行初始化操作
        Dim myArg() As String
        myArg = System.Environment.GetCommandLineArgs

        If myArg.Length > 1 Then
            Try

                Form2.PictureBox18_Click(sender, e)
                Form2.Show()
                Dim thhideform1 As New Threading.Thread(AddressOf hideform1)
                thhideform1.Start()
                If CInt(myArg(2).ToString) = 9 Then
                    'Form2.Show()
                    'System.Threading.Thread.Sleep(3000)
                    'Form2.Hide()
                    'Form3.Show()
                    'System.Threading.Thread.Sleep(3000)
                    'Form3.Hide()
                End If
                If CInt(myArg(2).ToString) = 8 Then
                    'System.Threading.Thread.Sleep(3000)


                    'Form3.Show()
                    'System.Threading.Thread.Sleep(3000)

                    'Form2.Show()
                    'System.Threading.Thread.Sleep(3000)
                    'Form2.PictureBox18_Click(sender, e)
                    'System.Threading.Thread.Sleep(3000)
                    'Form3.Hide()
                    'System.Threading.Thread.Sleep(3000)
                    'Dim thhideform1 As New Threading.Thread(AddressOf hideform1)
                    'thhideform1.Start()
                End If

            Catch ex As Exception

            End Try

            'If myArg.Length = 4 Then
            '    Form2.PictureBox18_Click(sender, e)
            'End If


        End If

        '创建一个线程，自动定时拨号



        '创建一个线程，对matlab进行初始化操作
        Dim thInit As New System.Threading.Thread(AddressOf initDaily)
        thInit.Start()

        '开启ftp数据下载线程
        Dim thFTP As New System.Threading.Thread(AddressOf FTPDownladData)
        thFTP.Start()
        Dim thFTP62 As New System.Threading.Thread(AddressOf FTPDownladData62)
        thFTP62.Start()
        Dim thFTP63 As New System.Threading.Thread(AddressOf FTPDownladData63)
        thFTP63.Start()

        '开启线程保存前一天数据
        Dim thFTPyest As New System.Threading.Thread(AddressOf FTPDownladDatayest)
        thFTPyest.Start()
        Dim thFTPyest62 As New System.Threading.Thread(AddressOf FTPDownladDatayest62)
        thFTPyest62.Start()
        Dim thFTPyest63 As New System.Threading.Thread(AddressOf FTPDownladDatayest63)
        thFTPyest63.Start()

        '开启线程，保存金码DSC数据
        Dim thDSC As New System.Threading.Thread(AddressOf receiveDSC)
        'thDSC.Start()


    End Sub
    Private Sub hideform1()
        Threading.Thread.Sleep(3000)
        Me.Hide()
    End Sub

    Public Sub initDaily()
        Try
            Dim matlabInit As New szdm.Class1
            Dim dataarr() As Single = {0, 0}
            Dim data As MathWorks.MATLAB.NET.Arrays.MWNumericArray = dataarr
            Dim result As MathWorks.MATLAB.NET.Arrays.MWArray = matlabInit.denoise2(data)

            Console.WriteLine("Init Finished")
            While True
                System.Threading.Thread.Sleep(10000)
            End While

        Catch ex As Exception

        End Try

    End Sub





    Private Sub UIThreadException(ByVal sender As Object, ByVal t As System.Threading.ThreadExceptionEventArgs)
        Shell("SHM.bat")
        Shell("TASKKILL /F /IM SHM.exe")
        System.Threading.Thread.Sleep(10000)
    End Sub

    Private Sub CurrentDomain_UnhandledException(ByVal sender As Object, ByVal e As System.UnhandledExceptionEventArgs)
        Shell("SHM.bat")
        Shell("TASKKILL /F /IM SHM.exe")
        System.Threading.Thread.Sleep(10000)

    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        If Date.Now.Hour > 8 Then
            Timer2.Enabled = True
        End If
    End Sub

    Private Sub Timer2_Tick(sender As Object, e As EventArgs) Handles Timer2.Tick
        If Date.Now.Hour = 2 Then

            '下面dataTodayTh线程对前一天所有数据进行处理
            Dim dataTodayTh = New System.Threading.Thread(AddressOf dataToday)
            'dataTodayTh.Start()
            Try
                Shell("shutdown -r -t 1")
            Catch ex As Exception

            End Try

            'MsgBox("目前正在处理当日数据，稍后系统将自动保存并重启，在此期间请勿进行任何操作！", MsgBoxStyle.Exclamation, "注意！")
            Try
                If thTCPReceive.IsAlive Then
                    Shell("SHM.bat 8")
                    Shell("TASKKILL /F /IM SHM.exe")
                    System.Threading.Thread.Sleep(10000)
                End If
            Catch ex As Exception

            End Try

            Shell("SHM.bat 9")
            Shell("TASKKILL /F /IM SHM.exe")
            System.Threading.Thread.Sleep(10000)
        End If
    End Sub

    Private Sub dataToday()
        ''下面对前一天数据进行处理，提取出前几阶频率并保存

        'For i = 1 To 5

        '    '循环，读取每个传感器中每个文件的数据并放入数组dataAcc
        '    Dim dataAcc(370000) As Single
        '    Dim dataLen As Single = 0

        '    '读文件...



        '    Dim dataAccAvi(dataLen) As Single
        '    For j = 0 To dataLen
        '        dataAccAvi(j) = dataAcc(j)
        '    Next

        '    '调用matlab对该小时传感器数据进行计算，得到频率

        '    Try
        '        Dim modIden As New szdm.Class1
        '        Dim data_sel As MWNumericArray = dataAccAvi
        '        Dim result As MWArray = modIden.modal_identification(data_sel)
        '        Dim fd As Array = result.ToArray

        '        Dim freq(14) As Single
        '        Dim freqStr As String = ""
        '        For k = 0 To 14
        '            freq(k) = fd(k, 0)
        '            freqStr = freqStr + freq(k).ToString + " "
        '        Next

        '        '将频率信息保存成文件
        '        My.Computer.FileSystem.WriteAllText("data.txt", freqStr, True)
        '    Catch ex As Exception

        '    End Try





        'Next


        'Shell("SHM.bat")
        'Shell("TASKKILL /F /IM SHM.exe")
        'System.Threading.Thread.Sleep(10000)
    End Sub


    Private Sub FTPDownladData()
        While True
            '定义服务器ip、端口、用户名、密码
            Dim serverIP As String = "192.168.0.76"
            Dim serverPort As Integer = 21
            Dim username As String = "Anonymous"
            Dim password As String = ""








            '获取当前日期时间
            Dim datenow = Date.Now

            '调试用，假定一个包含当前日期的FTP目录
            Dim dirRel As String = "/u/Storage Data/"
            dirRel = dirRel + Date.Now.Year.ToString + "-" + Date.Now.Month.ToString.PadLeft(2, "0") + "/" + Date.Now.Day.ToString.PadLeft(2, "0") + "/"



            Dim ftpClient As New clsFTP(serverIP, "", username, password, serverPort)

            Try
                '如果登陆失败则退出
                If Not ftpClient.Login Then
                    Exit Sub
                End If

                Console.WriteLine("Connected")

                '列目录，列出传感器名称文件夹
                Dim filearray() As String = ftpClient.GetFileList(dirRel)

                '将传感器名称数组转换为List,并去掉其中的空字符串和未采集完成的文件
                Dim pointlist As Collections.Generic.List(Of String) = array2list(filearray)

                For Each pointname As String In pointlist
                    Try
                        ftpClient.CloseConnection()
                    Catch ex As Exception

                    End Try
                    ftpClient = New clsFTP(serverIP, "", username, password, serverPort)
                    Dim dirwhole As String = pointname + "/"
                    'Console.WriteLine(dirwhole)

                    Dim localdirwhole As String = "E:" + dirwhole

                    My.Computer.FileSystem.CreateDirectory(localdirwhole)

                    Dim filelist As Collections.Generic.List(Of String) = array2list(ftpClient.GetFileList(dirwhole))
                    For Each filename As String In filelist
                        If filename.Contains("Continued") Then
                            Continue For
                        End If
                        Dim filepathname As String = filename
                        Dim localfilepathname As String = "E:" + filename

                        '如果本地不存在此文件，则下载到本地
                        If Not My.Computer.FileSystem.FileExists(localfilepathname) Then
                            Try
                                ftpClient.DownloadFile(filepathname, localfilepathname)
                            Catch ex As Exception

                            End Try


                            '以下可以对新数据进行模态参数识别：

                            'Dim dataselected(3600 * NIFreq + 100) As Single

                            'Try
                            '    Dim fs2 As New System.IO.FileStream(localfilepathname, IO.FileMode.Open)
                            '    Dim bw2 As New System.IO.BinaryReader(fs2)
                            '    Dim numnow(3) As Byte
                            '    For j = 0 To (fs2.Length / 4 - 1)
                            '        For i = 0 To 3
                            '            numnow(i) = fs2.ReadByte
                            '        Next
                            '        dataselected(j) = BitConverter.ToSingle(numnow, 0)
                            '    Next

                            '    bw2.Close()
                            '    fs2.Close()



                            '    '以下进行模态参数识别
                            '    Dim modIden As New szdm.Class1
                            '    Dim data_sel As MWNumericArray = dataselected
                            '    Dim result As MWArray = modIden.modal_identification(data_sel)
                            '    Dim fd As Array = result.ToArray



                            '    '以下将识别的模态参数追加写入文件
                            '    Dim modStr As String = Date.Now.ToString + vbTab
                            '    For i = 0 To 14
                            '        modStr = modStr + fd(i, 0).ToString + vbTab
                            '    Next
                            '    modStr = modStr.Trim
                            '    modStr = modStr + vbCrLf
                            '    My.Computer.FileSystem.WriteAllText(localdirwhole + "mod.txt", modStr, True)


                            'Catch ex As Exception
                            'End Try

                        End If
                    Next

                Next







            Catch ex As Exception
                Console.WriteLine("Login Failed")
            Finally
                Try
                    ftpClient.CloseConnection()
                    Console.WriteLine("Connection Closed")
                Catch ex As Exception

                End Try


            End Try

            System.Threading.Thread.Sleep(60000)

        End While


    End Sub


    Private Sub FTPDownladData62()
        '62服务器
        While True
            '定义服务器ip、端口、用户名、密码
            Dim serverIP As String = "192.168.1.62"
            Dim serverPort As Integer = 21
            Dim username As String = "Anonymous"
            Dim password As String = ""








            '获取当前日期时间
            Dim datenow = Date.Now

            '调试用，假定一个包含当前日期的FTP目录
            Dim dirRel As String = "/u/Storage Data/"
            dirRel = dirRel + Date.Now.Year.ToString + "/" + Date.Now.Month.ToString.PadLeft(2, "0") + "-" + Date.Now.Day.ToString.PadLeft(2, "0") + "/"



            Dim ftpClient As New clsFTP(serverIP, "", username, password, serverPort)

            Try
                '如果登陆失败则退出
                If Not ftpClient.Login Then
                    Exit Sub
                End If

                Console.WriteLine("Connected")

                '列目录，列出传感器名称文件夹
                Dim filearray() As String = ftpClient.GetFileList(dirRel)

                '将传感器名称数组转换为List,并去掉其中的空字符串和未采集完成的文件
                Dim pointlist As Collections.Generic.List(Of String) = array2list(filearray)

                For Each pointname As String In pointlist
                    Try
                        ftpClient.CloseConnection()
                    Catch ex As Exception

                    End Try
                    ftpClient = New clsFTP(serverIP, "", username, password, serverPort)
                    Dim dirwhole As String = pointname + "/"
                    'Console.WriteLine(dirwhole)

                    Dim localdirwhole As String = "E:" + dirwhole

                    My.Computer.FileSystem.CreateDirectory(localdirwhole)

                    Dim filelist As Collections.Generic.List(Of String) = array2list(ftpClient.GetFileList(dirwhole))
                    For Each filename As String In filelist
                        If filename.Contains("Continued") Then
                            Continue For
                        End If
                        Dim filepathname As String = filename
                        Dim localfilepathname As String = "E:" + filename

                        '如果本地不存在此文件，则下载到本地
                        If Not My.Computer.FileSystem.FileExists(localfilepathname) Then
                            Try
                                ftpClient.DownloadFile(filepathname, localfilepathname)
                            Catch ex As Exception

                            End Try


                            '以下可以对新数据进行模态参数识别：

                            'Dim dataselected(3600 * NIFreq + 100) As Single

                            'Try
                            '    Dim fs2 As New System.IO.FileStream(localfilepathname, IO.FileMode.Open)
                            '    Dim bw2 As New System.IO.BinaryReader(fs2)
                            '    Dim numnow(3) As Byte
                            '    For j = 0 To (fs2.Length / 4 - 1)
                            '        For i = 0 To 3
                            '            numnow(i) = fs2.ReadByte
                            '        Next
                            '        dataselected(j) = BitConverter.ToSingle(numnow, 0)
                            '    Next

                            '    bw2.Close()
                            '    fs2.Close()



                            '    '以下进行模态参数识别
                            '    Dim modIden As New szdm.Class1
                            '    Dim data_sel As MWNumericArray = dataselected
                            '    Dim result As MWArray = modIden.modal_identification(data_sel)
                            '    Dim fd As Array = result.ToArray



                            '    '以下将识别的模态参数追加写入文件
                            '    Dim modStr As String = Date.Now.ToString + vbTab
                            '    For i = 0 To 14
                            '        modStr = modStr + fd(i, 0).ToString + vbTab
                            '    Next
                            '    modStr = modStr.Trim
                            '    modStr = modStr + vbCrLf
                            '    My.Computer.FileSystem.WriteAllText(localdirwhole + "mod.txt", modStr, True)


                            'Catch ex As Exception
                            'End Try

                        End If
                    Next

                Next







            Catch ex As Exception
                Console.WriteLine("Login Failed")
            Finally
                Try
                    ftpClient.CloseConnection()
                    Console.WriteLine("Connection Closed")
                Catch ex As Exception

                End Try


            End Try

            System.Threading.Thread.Sleep(60000)

        End While


    End Sub

    Private Sub FTPDownladData63()
        '63服务器
        While True
            '定义服务器ip、端口、用户名、密码
            Dim serverIP As String = "192.168.1.63"
            Dim serverPort As Integer = 21
            Dim username As String = "Anonymous"
            Dim password As String = ""








            '获取当前日期时间
            Dim datenow = Date.Now

            '调试用，假定一个包含当前日期的FTP目录
            Dim dirRel As String = "/u/Storage Data/"
            dirRel = dirRel + Date.Now.Year.ToString + "/" + Date.Now.Month.ToString.PadLeft(2, "0") + "-" + Date.Now.Day.ToString.PadLeft(2, "0") + "/"



            Dim ftpClient As New clsFTP(serverIP, "", username, password, serverPort)

            Try
                '如果登陆失败则退出
                If Not ftpClient.Login Then
                    Exit Sub
                End If

                Console.WriteLine("Connected")

                '列目录，列出传感器名称文件夹
                Dim filearray() As String = ftpClient.GetFileList(dirRel)

                '将传感器名称数组转换为List,并去掉其中的空字符串和未采集完成的文件
                Dim pointlist As Collections.Generic.List(Of String) = array2list(filearray)

                For Each pointname As String In pointlist
                    Try
                        ftpClient.CloseConnection()
                    Catch ex As Exception

                    End Try
                    ftpClient = New clsFTP(serverIP, "", username, password, serverPort)
                    Dim dirwhole As String = pointname + "/"
                    'Console.WriteLine(dirwhole)

                    Dim localdirwhole As String = "E:" + dirwhole

                    My.Computer.FileSystem.CreateDirectory(localdirwhole)

                    Dim filelist As Collections.Generic.List(Of String) = array2list(ftpClient.GetFileList(dirwhole))
                    For Each filename As String In filelist
                        If filename.Contains("Continued") Then
                            Continue For
                        End If
                        Dim filepathname As String = filename
                        Dim localfilepathname As String = "E:" + filename

                        '如果本地不存在此文件，则下载到本地
                        If Not My.Computer.FileSystem.FileExists(localfilepathname) Then
                            Try
                                ftpClient.DownloadFile(filepathname, localfilepathname)
                            Catch ex As Exception

                            End Try


                            '以下可以对新数据进行模态参数识别：

                            'Dim dataselected(3600 * NIFreq + 100) As Single

                            'Try
                            '    Dim fs2 As New System.IO.FileStream(localfilepathname, IO.FileMode.Open)
                            '    Dim bw2 As New System.IO.BinaryReader(fs2)
                            '    Dim numnow(3) As Byte
                            '    For j = 0 To (fs2.Length / 4 - 1)
                            '        For i = 0 To 3
                            '            numnow(i) = fs2.ReadByte
                            '        Next
                            '        dataselected(j) = BitConverter.ToSingle(numnow, 0)
                            '    Next

                            '    bw2.Close()
                            '    fs2.Close()



                            '    '以下进行模态参数识别
                            '    Dim modIden As New szdm.Class1
                            '    Dim data_sel As MWNumericArray = dataselected
                            '    Dim result As MWArray = modIden.modal_identification(data_sel)
                            '    Dim fd As Array = result.ToArray



                            '    '以下将识别的模态参数追加写入文件
                            '    Dim modStr As String = Date.Now.ToString + vbTab
                            '    For i = 0 To 14
                            '        modStr = modStr + fd(i, 0).ToString + vbTab
                            '    Next
                            '    modStr = modStr.Trim
                            '    modStr = modStr + vbCrLf
                            '    My.Computer.FileSystem.WriteAllText(localdirwhole + "mod.txt", modStr, True)


                            'Catch ex As Exception
                            'End Try

                        End If
                    Next

                Next







            Catch ex As Exception
                Console.WriteLine("Login Failed")
            Finally
                Try
                    ftpClient.CloseConnection()
                    Console.WriteLine("Connection Closed")
                Catch ex As Exception

                End Try


            End Try

            System.Threading.Thread.Sleep(60000)

        End While


    End Sub


    Private Sub FTPDownladDatayest()
        While True
            '定义服务器ip、端口、用户名、密码
            Dim serverIP As String = "192.168.0.76"
            Dim serverPort As Integer = 21
            Dim username As String = "Anonymous"
            Dim password As String = ""








            '获取当前日期时间
            Dim datenow = Date.Now
            datenow = DateAdd(DateInterval.Day, -1, datenow)

            '调试用，假定一个包含当前日期的FTP目录
            Dim dirRel As String = "/u/Storage Data/"
            dirRel = dirRel + datenow.Year.ToString + "-" + datenow.Month.ToString.PadLeft(2, "0") + "/" + datenow.Day.ToString.PadLeft(2, "0") + "/"



            Dim ftpClient As New clsFTP(serverIP, "", username, password, serverPort)

            Try
                '如果登陆失败则退出
                If Not ftpClient.Login Then
                    Exit Sub
                End If

                Console.WriteLine("Connected")

                '列目录，列出传感器名称文件夹
                Dim filearray() As String = ftpClient.GetFileList(dirRel)

                '将传感器名称数组转换为List,并去掉其中的空字符串和未采集完成的文件
                Dim pointlist As Collections.Generic.List(Of String) = array2list(filearray)

                For Each pointname As String In pointlist
                    Try
                        ftpClient.CloseConnection()
                    Catch ex As Exception

                    End Try
                    ftpClient = New clsFTP(serverIP, "", username, password, serverPort)
                    Dim dirwhole As String = pointname + "/"
                    'Console.WriteLine(dirwhole)

                    Dim localdirwhole As String = "E:" + dirwhole

                    My.Computer.FileSystem.CreateDirectory(localdirwhole)
                    Dim filelist As Collections.Generic.List(Of String) = array2list(ftpClient.GetFileList(dirwhole))
                    For Each filename As String In filelist
                        If filename.Contains("Continued") Then
                            Continue For
                        End If
                        Dim filepathname As String = filename
                        Dim localfilepathname As String = "E:" + filename

                        '如果本地不存在此文件，则下载到本地
                        If Not My.Computer.FileSystem.FileExists(localfilepathname) Then
                            Try
                                ftpClient.DownloadFile(filepathname, localfilepathname)
                            Catch ex As Exception

                            End Try

                            '以下可以对新数据进行模态参数识别：

                            'Dim dataselected(3600 * NIFreq + 100) As Single

                            'Try
                            '    Dim fs2 As New System.IO.FileStream(localfilepathname, IO.FileMode.Open)
                            '    Dim bw2 As New System.IO.BinaryReader(fs2)
                            '    Dim numnow(3) As Byte
                            '    For j = 0 To (fs2.Length / 4 - 1)
                            '        For i = 0 To 3
                            '            numnow(i) = fs2.ReadByte
                            '        Next
                            '        dataselected(j) = BitConverter.ToSingle(numnow, 0)
                            '    Next

                            '    bw2.Close()
                            '    fs2.Close()



                            '    '以下进行模态参数识别
                            '    Dim modIden As New szdm.Class1
                            '    Dim data_sel As MWNumericArray = dataselected
                            '    Dim result As MWArray = modIden.modal_identification(data_sel)
                            '    Dim fd As Array = result.ToArray



                            '    '以下将识别的模态参数追加写入文件
                            '    Dim modStr As String = Date.Now.ToString + vbTab
                            '    For i = 0 To 14
                            '        modStr = modStr + fd(i, 0).ToString + vbTab
                            '    Next
                            '    modStr = modStr.Trim
                            '    modStr = modStr + vbCrLf
                            '    My.Computer.FileSystem.WriteAllText(localdirwhole + "mod.txt", modStr, True)


                            'Catch ex As Exception
                            'End Try

                        End If
                    Next

                Next







            Catch ex As Exception
                Console.WriteLine("Login Failed")
            Finally
                Try
                    ftpClient.CloseConnection()
                    Console.WriteLine("Connection Closed")
                Catch ex As Exception

                End Try


            End Try

            System.Threading.Thread.Sleep(60000)

        End While

    End Sub

    Private Sub FTPDownladDatayest62()
        While True
            '定义服务器ip、端口、用户名、密码
            Dim serverIP As String = "192.168.1.62"
            Dim serverPort As Integer = 21
            Dim username As String = "Anonymous"
            Dim password As String = ""








            '获取当前日期时间
            Dim datenow = Date.Now
            datenow = DateAdd(DateInterval.Day, -1, datenow)

            '调试用，假定一个包含当前日期的FTP目录
            Dim dirRel As String = "/u/Storage Data/"
            dirRel = dirRel + datenow.Year.ToString + "/" + datenow.Month.ToString.PadLeft(2, "0") + "-" + datenow.Day.ToString.PadLeft(2, "0") + "/"



            Dim ftpClient As New clsFTP(serverIP, "", username, password, serverPort)

            Try
                '如果登陆失败则退出
                If Not ftpClient.Login Then
                    Exit Sub
                End If

                Console.WriteLine("Connected")

                '列目录，列出传感器名称文件夹
                Dim filearray() As String = ftpClient.GetFileList(dirRel)

                '将传感器名称数组转换为List,并去掉其中的空字符串和未采集完成的文件
                Dim pointlist As Collections.Generic.List(Of String) = array2list(filearray)

                For Each pointname As String In pointlist
                    Try
                        ftpClient.CloseConnection()
                    Catch ex As Exception

                    End Try
                    ftpClient = New clsFTP(serverIP, "", username, password, serverPort)
                    Dim dirwhole As String = pointname + "/"
                    'Console.WriteLine(dirwhole)

                    Dim localdirwhole As String = "E:" + dirwhole

                    My.Computer.FileSystem.CreateDirectory(localdirwhole)
                    Dim filelist As Collections.Generic.List(Of String) = array2list(ftpClient.GetFileList(dirwhole))
                    For Each filename As String In filelist
                        If filename.Contains("Continued") Then
                            Continue For
                        End If
                        Dim filepathname As String = filename
                        Dim localfilepathname As String = "E:" + filename

                        '如果本地不存在此文件，则下载到本地
                        If Not My.Computer.FileSystem.FileExists(localfilepathname) Then
                            Try
                                ftpClient.DownloadFile(filepathname, localfilepathname)
                            Catch ex As Exception

                            End Try

                            '以下可以对新数据进行模态参数识别：

                            'Dim dataselected(3600 * NIFreq + 100) As Single

                            'Try
                            '    Dim fs2 As New System.IO.FileStream(localfilepathname, IO.FileMode.Open)
                            '    Dim bw2 As New System.IO.BinaryReader(fs2)
                            '    Dim numnow(3) As Byte
                            '    For j = 0 To (fs2.Length / 4 - 1)
                            '        For i = 0 To 3
                            '            numnow(i) = fs2.ReadByte
                            '        Next
                            '        dataselected(j) = BitConverter.ToSingle(numnow, 0)
                            '    Next

                            '    bw2.Close()
                            '    fs2.Close()



                            '    '以下进行模态参数识别
                            '    Dim modIden As New szdm.Class1
                            '    Dim data_sel As MWNumericArray = dataselected
                            '    Dim result As MWArray = modIden.modal_identification(data_sel)
                            '    Dim fd As Array = result.ToArray



                            '    '以下将识别的模态参数追加写入文件
                            '    Dim modStr As String = Date.Now.ToString + vbTab
                            '    For i = 0 To 14
                            '        modStr = modStr + fd(i, 0).ToString + vbTab
                            '    Next
                            '    modStr = modStr.Trim
                            '    modStr = modStr + vbCrLf
                            '    My.Computer.FileSystem.WriteAllText(localdirwhole + "mod.txt", modStr, True)


                            'Catch ex As Exception
                            'End Try

                        End If
                    Next

                Next







            Catch ex As Exception
                Console.WriteLine("Login Failed")
            Finally
                Try
                    ftpClient.CloseConnection()
                    Console.WriteLine("Connection Closed")
                Catch ex As Exception

                End Try


            End Try

            System.Threading.Thread.Sleep(60000)

        End While

    End Sub

    Private Sub FTPDownladDatayest63()
        While True
            '定义服务器ip、端口、用户名、密码
            Dim serverIP As String = "192.168.1.63"
            Dim serverPort As Integer = 21
            Dim username As String = "Anonymous"
            Dim password As String = ""








            '获取当前日期时间
            Dim datenow = Date.Now
            datenow = DateAdd(DateInterval.Day, -1, datenow)

            '调试用，假定一个包含当前日期的FTP目录
            Dim dirRel As String = "/u/Storage Data/"
            dirRel = dirRel + datenow.Year.ToString + "/" + datenow.Month.ToString.PadLeft(2, "0") + "-" + datenow.Day.ToString.PadLeft(2, "0") + "/"



            Dim ftpClient As New clsFTP(serverIP, "", username, password, serverPort)

            Try
                '如果登陆失败则退出
                If Not ftpClient.Login Then
                    Exit Sub
                End If

                Console.WriteLine("Connected")

                '列目录，列出传感器名称文件夹
                Dim filearray() As String = ftpClient.GetFileList(dirRel)

                '将传感器名称数组转换为List,并去掉其中的空字符串和未采集完成的文件
                Dim pointlist As Collections.Generic.List(Of String) = array2list(filearray)

                For Each pointname As String In pointlist
                    Try
                        ftpClient.CloseConnection()
                    Catch ex As Exception

                    End Try
                    ftpClient = New clsFTP(serverIP, "", username, password, serverPort)
                    Dim dirwhole As String = pointname + "/"
                    'Console.WriteLine(dirwhole)

                    Dim localdirwhole As String = "E:" + dirwhole

                    My.Computer.FileSystem.CreateDirectory(localdirwhole)
                    Dim filelist As Collections.Generic.List(Of String) = array2list(ftpClient.GetFileList(dirwhole))
                    For Each filename As String In filelist
                        If filename.Contains("Continued") Then
                            Continue For
                        End If
                        Dim filepathname As String = filename
                        Dim localfilepathname As String = "E:" + filename

                        '如果本地不存在此文件，则下载到本地
                        If Not My.Computer.FileSystem.FileExists(localfilepathname) Then
                            Try
                                ftpClient.DownloadFile(filepathname, localfilepathname)
                            Catch ex As Exception

                            End Try

                            '以下可以对新数据进行模态参数识别：

                            'Dim dataselected(3600 * NIFreq + 100) As Single

                            'Try
                            '    Dim fs2 As New System.IO.FileStream(localfilepathname, IO.FileMode.Open)
                            '    Dim bw2 As New System.IO.BinaryReader(fs2)
                            '    Dim numnow(3) As Byte
                            '    For j = 0 To (fs2.Length / 4 - 1)
                            '        For i = 0 To 3
                            '            numnow(i) = fs2.ReadByte
                            '        Next
                            '        dataselected(j) = BitConverter.ToSingle(numnow, 0)
                            '    Next

                            '    bw2.Close()
                            '    fs2.Close()



                            '    '以下进行模态参数识别
                            '    Dim modIden As New szdm.Class1
                            '    Dim data_sel As MWNumericArray = dataselected
                            '    Dim result As MWArray = modIden.modal_identification(data_sel)
                            '    Dim fd As Array = result.ToArray



                            '    '以下将识别的模态参数追加写入文件
                            '    Dim modStr As String = Date.Now.ToString + vbTab
                            '    For i = 0 To 14
                            '        modStr = modStr + fd(i, 0).ToString + vbTab
                            '    Next
                            '    modStr = modStr.Trim
                            '    modStr = modStr + vbCrLf
                            '    My.Computer.FileSystem.WriteAllText(localdirwhole + "mod.txt", modStr, True)


                            'Catch ex As Exception
                            'End Try

                        End If
                    Next

                Next







            Catch ex As Exception
                Console.WriteLine("Login Failed")
            Finally
                Try
                    ftpClient.CloseConnection()
                    Console.WriteLine("Connection Closed")
                Catch ex As Exception

                End Try


            End Try

            System.Threading.Thread.Sleep(60000)

        End While

    End Sub


    Private Function array2list(ByRef filearray() As String) As Collections.Generic.List(Of String)
        '将文件名数组转换为List,并去掉其中的空字符串和未采集完成的文件
        Dim filelist As Collections.Generic.List(Of String) = filearray.ToList
        If filelist.Count > 0 Then
            For i = 0 To filelist.Count - 1
                filelist.Item(i) = filelist.Item(i).Trim
                If (filelist.Item(i).Length <= 0) Then ' Or filelist.Item(i).Contains("Continued")
                    filelist.RemoveAt(i)
                End If
            Next
        End If

        Return filelist
    End Function


    Private Sub FTPDownladData2()
        While True
            '定义服务器ip、端口、用户名、密码
            Dim serverIP As String = "192.168.1.66"
            Dim serverPort As Integer = 21
            Dim username As String = "Anonymous"
            Dim password As String = ""

            '获取当前日期时间
            Dim datenow = Date.Now

            '调试用，假定一个包含当前日期的FTP目录
            Dim dirRel As String = "/c/Storage Data/"
            dirRel = dirRel + Date.Now.Year.ToString + "/" + Date.Now.Month.ToString.PadLeft(2, "0") + "-" + Date.Now.Day.ToString.PadLeft(2, "0") + "/"



            Dim ftpClient As New clsFTP(serverIP, "", username, password, serverPort)

            Try
                '如果登陆失败则退出
                If Not ftpClient.Login Then
                    Exit Sub
                End If

                Console.WriteLine("Connected")

                '列目录，列出传感器名称文件夹
                Dim filearray() As String = ftpClient.GetFileList(dirRel)

                '将传感器名称数组转换为List,并去掉其中的空字符串
                Dim pointlist As Collections.Generic.List(Of String) = array2list(filearray)

                For Each pointname As String In pointlist
                    Dim dirwhole As String = dirRel + pointname + "/"
                    'Console.WriteLine(dirwhole)

                    Dim localdirwhole As String = "E:" + dirwhole

                    My.Computer.FileSystem.CreateDirectory(localdirwhole)

                    Dim filelist As Collections.Generic.List(Of String) = array2list(ftpClient.GetFileList(dirwhole))
                    For Each filename As String In filelist
                        Dim filepathname As String = dirwhole + filename
                        Dim localfilepathname As String = localdirwhole + filename

                        '如果本地不存在此文件，则下载到本地
                        If Not My.Computer.FileSystem.FileExists(localfilepathname) Then
                            ftpClient.DownloadFile(filepathname, localfilepathname)

                            '以下可以对新数据进行模态参数识别：

                            'Dim dataselected(370000) As Single

                            'Try
                            '    Dim fs2 As New System.IO.FileStream(localfilepathname, IO.FileMode.Open)
                            '    Dim bw2 As New System.IO.BinaryReader(fs2)
                            '    Dim numnow(3) As Byte
                            '    For j = 0 To (fs2.Length / 4 - 1)
                            '        For i = 0 To 3
                            '            numnow(i) = fs2.ReadByte
                            '        Next
                            '        dataselected(j) = BitConverter.ToSingle(numnow, 0)
                            '    Next

                            '    bw2.Close()
                            '    fs2.Close()



                            '    '以下进行模态参数识别
                            '    Dim modIden As New szdm.Class1
                            '    Dim data_sel As MWNumericArray = dataselected
                            '    Dim result As MWArray = modIden.modal_identification(data_sel)
                            '    Dim fd As Array = result.ToArray



                            '    '以下将识别的模态参数追加写入文件
                            '    Dim modStr As String = Date.Now.ToString + vbTab
                            '    For i = 0 To 14
                            '        modStr = modStr + fd(i, 0).ToString + vbTab
                            '    Next
                            '    modStr = modStr.Trim
                            '    modStr = modStr + vbCrLf
                            '    My.Computer.FileSystem.WriteAllText(localdirwhole + "mod.txt", modStr, True)


                            'Catch ex As Exception
                            'End Try

                        End If
                    Next

                Next







            Catch ex As Exception
                Console.WriteLine("Login Failed")
            Finally
                Try
                    ftpClient.CloseConnection()
                    Console.WriteLine("Connection Closed")
                Catch ex As Exception

                End Try


            End Try

            System.Threading.Thread.Sleep(60000)

        End While



    End Sub



    Private Sub receiveDSC()

        Dim mod1filename As String = "E:\dsc\DSC\DSC\DSCData\测量数据\34150283.txt"
        Dim mod2filename As String = "E:\dsc\DSC\DSC\DSCData\测量数据\34150291.txt"
        Dim modfileinfo = My.Computer.FileSystem.GetFileInfo(mod1filename)
        'MsgBox((Date.Now - mod1fileinfo.LastWriteTime).Minutes)
        'Dim mod1fileinfo = My.Computer.FileSystem.GetFileInfo(mod1filename)
        'MsgBox(mod1fileinfo.LastWriteTime.Minute)

        Dim destFolder As String = "E:\c\DSCData\"

        While True

            Try
                Dim mod1fileinfo = My.Computer.FileSystem.GetFileInfo(mod1filename)
                Dim mod2fileinfo = My.Computer.FileSystem.GetFileInfo(mod2filename)
                If ((Date.Now - mod1fileinfo.LastWriteTime).Minutes <= 8 And (Date.Now - mod1fileinfo.LastWriteTime).Minutes >= 1 And (Date.Now - mod2fileinfo.LastWriteTime).Minutes <= 8 And (Date.Now - mod2fileinfo.LastWriteTime).Minutes > 1) Then
                    Dim destPath As String = destFolder + Date.Now.Year.ToString + "\" + Date.Now.Month.ToString + "\"
                    My.Computer.FileSystem.CreateDirectory(destPath)
                    My.Computer.FileSystem.CopyFile(mod1filename, destPath + Date.Now.Day.ToString + "-34150283.txt", True)
                    My.Computer.FileSystem.CopyFile(mod2filename, destPath + Date.Now.Day.ToString + "-34150291.txt", True)

                End If



                If (Date.Now.Hour = 0 And Date.Now.Minute <= 10) Then
                    If ((Date.Now - mod1fileinfo.LastWriteTime).Minutes < 8 And (Date.Now - mod1fileinfo.LastWriteTime).Minutes >= 1 And (Date.Now - mod2fileinfo.LastWriteTime).Minutes < 8 And (Date.Now - mod2fileinfo.LastWriteTime).Minutes > 1) Then
                        My.Computer.FileSystem.DeleteFile(mod1filename)
                        My.Computer.FileSystem.DeleteFile(mod2filename)
                    End If

                End If
            Catch ex As Exception


            Finally
                System.Threading.Thread.Sleep(240000)
            End Try
        End While



    End Sub


    Private Sub Timer3_Tick(sender As Object, e As EventArgs) Handles Timer3.Tick
        Try
            '定时自动拨号
            Shell("rasdial Unicom /PHONE:*99#")
            Console.WriteLine("finishied")
        Catch ex As Exception

        End Try


    End Sub

    Private Sub PictureBox4_KeyDown(sender As Object, e As KeyEventArgs) Handles PictureBox4.KeyDown

    End Sub

    Private Sub PictureBox5_Click_1(sender As Object, e As EventArgs) Handles PictureBox5.Click
        bridgeSelected = 4
        Form8.Label13.Text = ""
        Form8.Label14.Text = ""

        Me.Hide()
        Form8.Show()
        Form8.Text = "A0-A3桥"
        bridgeSelected = 4
        Form8.Label4.Text = "加速度(m/s^2)"
        Form8.Label5.Text = "时间(s)"
        Form8.Label6.Text = ""
        Form8.TextBox1.Text = ""
        Form8.RadioButton4.Enabled = False
        Form8.RadioButton1.Checked = True
        Form8.RadioButton2.Checked = True

        Form8.PictureBox13.BackgroundImage = Image.FromFile("mainimage\jiemian3.png")

        '调整传感器位置偏移量
        Dim pyX As Integer = 68
        Dim pyY As Integer = 52

        'Form8.PictureBox23.Location = New System.Drawing.Point(113 + pyX, 369 + pyY)
        'Form8.PictureBox26.Location = New System.Drawing.Point(161 + pyX, 369 + pyY)
        ''Form8.PictureBox18.Location = New System.Drawing.Point(251 + pyX, 366 + pyY)
        'Form8.PictureBox27.Location = New System.Drawing.Point(318 + pyX, 369 + pyY)
        'Form8.PictureBox24.Location = New System.Drawing.Point(368 + pyX, 369 + pyY)
        'Form8.PictureBox31.Location = New System.Drawing.Point(113 + pyX, 337 + pyY)
        'Form8.PictureBox29.Location = New System.Drawing.Point(162 + pyX, 337 + pyY)
        'Form8.PictureBox32.Location = New System.Drawing.Point(240 + pyX, 336 + pyY)
        'Form8.PictureBox28.Location = New System.Drawing.Point(318 + pyX, 337 + pyY)
        'Form8.PictureBox30.Location = New System.Drawing.Point(368 + pyX, 337 + pyY)

        Form8.PictureBox33.Visible = False

        '设置加速度路径名
        ReDim Form8.accfolders(10)
        Form8.accfolders = {"11031", "11032", "11033", "11034", "11035", "11036", "11037", "11038", "11039", "11040"}

        '设置位移路径名
        ReDim Form8.dispfolders(6)
        Form8.dispfolders = {"16055", "16056", "16057", "16058", "16059", "16060", "16006", "16003", "16005", "16002", "16004", "16001"}

        '设置应变传感器编号
        ReDim Form8.sensornum(24)
        Form8.sensornum = {"087745", "087752", "092432", "092430", "092417", "092429", "092407", "092420", "092422", "092408", "092433", "087749", "087748", "087747", "092434", "092423", "092400", "092431", "092406", "092405", "087746", "092412", "087754", "087751"}
        ReDim Form8.strainfolders(24)
        Form8.strainfolders = {"14109", "14110", "14111", "14112", "14113", "14114", "14115", "14116", "14117", "14118", "14119", "14120", "14121", "14122", "14123", "13041", "13040", "13039", "13038", "13037", "13036", "13035", "13034", "13033"}

        '设置应变传感器零点
        ReDim Form8.strainzero(25)
        Form8.strainzero = {2709.04, 2725.6, 2727.0, 2677.54, 2762.72, 2777.54, 2745.64, 2977.94, 2842.94, 2729.39, 2431.4, 2250.24, 2575.92, 2548.86, 2415.32, 2610.74, 2637.74, 2867.38, 2885.58, 2728.26, 2653.24, 2681.64, 2681.52, 2813.88, 0.0}




        '设置桩号
        Form8.Label7.Text = "A3"
        Form8.Label8.Text = "A2"
        Form8.Label9.Text = "A1"
        Form8.Label10.Text = "A0"

        '更改立体图中测点显示位置
        'Form8.PictureBox25.Location = New Drawing.Point(126, 256)
        'Form8.PictureBox2.Location = New Drawing.Point(195, 227)

        'Form8.PictureBox22.Location = New Drawing.Point(266, 199)
        'Form8.PictureBox20.Location = New Drawing.Point(301, 184)
        'Form8.PictureBox21.Location = New Drawing.Point(335, 170)

        'Form8.PictureBox16.Location = New Drawing.Point(401, 144)
        'Form8.PictureBox19.Location = New Drawing.Point(432, 131)
        'Form8.PictureBox17.Location = New Drawing.Point(463, 118)
    End Sub

    Private Sub PictureBox5_MouseDown(sender As Object, e As MouseEventArgs) Handles PictureBox5.MouseDown
        PictureBox5.BackgroundImage = Image.FromFile("mainimage\hk\backgroundgray.png")
    End Sub

    Private Sub PictureBox5_MouseUp(sender As Object, e As MouseEventArgs) Handles PictureBox5.MouseUp
        PictureBox5.BackgroundImage = Nothing
    End Sub

    Private Sub PictureBox6_Click(sender As Object, e As EventArgs) Handles PictureBox6.Click
        bridgeSelected = 5
        Form8.Label13.Text = ""
        Form8.Label14.Text = ""

        Me.Hide()
        Form8.Show()
        Form8.Text = "A3-P22a桥"
        bridgeSelected = 5
        Form8.Label4.Text = "加速度(m/s^2)"
        Form8.Label5.Text = "时间(s)"
        Form8.Label6.Text = ""
        Form8.TextBox1.Text = ""
        Form8.RadioButton4.Enabled = False
        Form8.RadioButton1.Checked = True
        Form8.RadioButton2.Checked = True

        Form8.PictureBox13.BackgroundImage = Image.FromFile("mainimage\jiemian3.png")

        '调整传感器位置偏移量
        Dim pyX As Integer = 68
        Dim pyY As Integer = 52

        'Form8.PictureBox23.Location = New System.Drawing.Point(113 + pyX, 369 + pyY)
        'Form8.PictureBox26.Location = New System.Drawing.Point(161 + pyX, 369 + pyY)
        ''Form8.PictureBox18.Location = New System.Drawing.Point(251 + pyX, 366 + pyY)
        'Form8.PictureBox27.Location = New System.Drawing.Point(318 + pyX, 369 + pyY)
        'Form8.PictureBox24.Location = New System.Drawing.Point(368 + pyX, 369 + pyY)
        'Form8.PictureBox31.Location = New System.Drawing.Point(113 + pyX, 337 + pyY)
        'Form8.PictureBox29.Location = New System.Drawing.Point(162 + pyX, 337 + pyY)
        'Form8.PictureBox32.Location = New System.Drawing.Point(240 + pyX, 336 + pyY)
        'Form8.PictureBox28.Location = New System.Drawing.Point(318 + pyX, 337 + pyY)
        'Form8.PictureBox30.Location = New System.Drawing.Point(368 + pyX, 337 + pyY)

        Form8.PictureBox33.Visible = False

        '设置加速度路径名
        ReDim Form8.accfolders(6)
        Form8.accfolders = {"11041", "11042", "11043", "11044", "11045", "11046", "11047", "11048", "11049", "11050"}

        '设置位移路径名
        ReDim Form8.dispfolders(12)
        Form8.dispfolders = {"16061", "16062", "16063", "16064", "16065", "16066", "16006", "16003", "16005", "16002", "16004", "16001"}

        '设置应变传感器编号
        ReDim Form8.sensornum(24)
        Form8.sensornum = {"087745", "087752", "092432", "092430", "092417", "092429", "092407", "092420", "092422", "092408", "092433", "087749", "087748", "087747", "092434", "092423", "092400", "092431", "092406", "092405", "087746", "092412", "087754", "087751"}
        ReDim Form8.strainfolders(24)
        Form8.strainfolders = {"14097", "14098", "14099", "14100", "14101", "14102", "14103", "14104", "14105", "14106", "14107", "14108", "13044", "13043", "13042", "13041", "13040", "13039", "13038", "13037", "13036", "13035", "13034", "13033"}

        '设置应变传感器零点
        ReDim Form8.strainzero(25)
        Form8.strainzero = {2709.04, 2725.6, 2727.0, 2677.54, 2762.72, 2777.54, 2745.64, 2977.94, 2842.94, 2729.39, 2431.4, 2250.24, 2575.92, 2548.86, 2415.32, 2610.74, 2637.74, 2867.38, 2885.58, 2728.26, 2653.24, 2681.64, 2681.52, 2813.88, 0.0}




        '设置桩号
        Form8.Label7.Text = "P22a"
        Form8.Label8.Text = "A5"
        Form8.Label9.Text = "A4"
        Form8.Label10.Text = "A3"

        '更改立体图中测点显示位置
        'Form8.PictureBox25.Location = New Drawing.Point(126, 256)
        'Form8.PictureBox2.Location = New Drawing.Point(195, 227)

        'Form8.PictureBox22.Location = New Drawing.Point(266, 199)
        'Form8.PictureBox20.Location = New Drawing.Point(301, 184)
        'Form8.PictureBox21.Location = New Drawing.Point(335, 170)

        'Form8.PictureBox16.Location = New Drawing.Point(401, 144)
        'Form8.PictureBox19.Location = New Drawing.Point(432, 131)
        'Form8.PictureBox17.Location = New Drawing.Point(463, 118)
    End Sub

    Private Sub PictureBox6_MouseDown(sender As Object, e As MouseEventArgs) Handles PictureBox6.MouseDown
        PictureBox6.BackgroundImage = Image.FromFile("mainimage\hk\backgroundgray.png")
    End Sub

    Private Sub PictureBox6_MouseUp(sender As Object, e As MouseEventArgs) Handles PictureBox6.MouseUp
        PictureBox6.BackgroundImage = Nothing
    End Sub

    Private Sub PictureBox7_Click(sender As Object, e As EventArgs) Handles PictureBox7.Click
        bridgeSelected = 6
        Form8.Label13.Text = ""
        Form8.Label14.Text = ""

        Me.Hide()
        Form8.Show()
        Form8.Text = "B3-P33b桥"
        bridgeSelected = 6
        Form8.Label4.Text = "加速度(m/s^2)"
        Form8.Label5.Text = "时间(s)"
        Form8.Label6.Text = ""
        Form8.TextBox1.Text = ""
        Form8.RadioButton4.Enabled = False
        Form8.RadioButton1.Checked = True
        Form8.RadioButton2.Checked = True

        Form8.PictureBox13.BackgroundImage = Image.FromFile("mainimage\jiemian3.png")

        '调整传感器位置偏移量
        Dim pyX As Integer = 68
        Dim pyY As Integer = 52

        'Form8.PictureBox23.Location = New System.Drawing.Point(113 + pyX, 369 + pyY)
        'Form8.PictureBox26.Location = New System.Drawing.Point(161 + pyX, 369 + pyY)
        ''Form8.PictureBox18.Location = New System.Drawing.Point(251 + pyX, 366 + pyY)
        'Form8.PictureBox27.Location = New System.Drawing.Point(318 + pyX, 369 + pyY)
        'Form8.PictureBox24.Location = New System.Drawing.Point(368 + pyX, 369 + pyY)
        'Form8.PictureBox31.Location = New System.Drawing.Point(113 + pyX, 337 + pyY)
        'Form8.PictureBox29.Location = New System.Drawing.Point(162 + pyX, 337 + pyY)
        'Form8.PictureBox32.Location = New System.Drawing.Point(240 + pyX, 336 + pyY)
        'Form8.PictureBox28.Location = New System.Drawing.Point(318 + pyX, 337 + pyY)
        'Form8.PictureBox30.Location = New System.Drawing.Point(368 + pyX, 337 + pyY)

        Form8.PictureBox33.Visible = False

        '设置加速度路径名
        ReDim Form8.accfolders(6)
        Form8.accfolders = {"11024", "11023", "11022", "11021", "11020", "11019"}

        '设置位移路径名
        ReDim Form8.dispfolders(12)
        Form8.dispfolders = {"16042", "16041", "16040", "16039", "16038", "16037", "16006", "16003", "16005", "16002", "16004", "16001"}

        '设置应变传感器编号
        ReDim Form8.sensornum(24)
        Form8.sensornum = {"087745", "087752", "092432", "092430", "092417", "092429", "092407", "092420", "092422", "092408", "092433", "087749", "087748", "087747", "092434", "092423", "092400", "092431", "092406", "092405", "087746", "092412", "087754", "087751"}
        ReDim Form8.strainfolders(24)
        Form8.strainfolders = {"14089", "14090", "14091", "14092", "14093", "14094", "14095", "14096", "13048", "13047", "13046", "13045", "13044", "13043", "13042", "13041", "13040", "13039", "13038", "13037", "13036", "13035", "13034", "13033"}

        '设置应变传感器零点
        ReDim Form8.strainzero(25)
        Form8.strainzero = {2709.04, 2725.6, 2727.0, 2677.54, 2762.72, 2777.54, 2745.64, 2977.94, 2842.94, 2729.39, 2431.4, 2250.24, 2575.92, 2548.86, 2415.32, 2610.74, 2637.74, 2867.38, 2885.58, 2728.26, 2653.24, 2681.64, 2681.52, 2813.88, 0.0}




        '设置桩号
        Form8.Label7.Text = "B3"
        Form8.Label8.Text = "B4"
        Form8.Label9.Text = "B5"
        Form8.Label10.Text = "P33b"




        Form7.Label13.Text = ""
        Form7.Label14.Text = ""

        'Me.Hide()
        'Form7.Show()
        Form7.Text = "B3-P33b桥 - 英雄山路立交桥梁集群安全运营监测与预警系统"
        'bridgeSelected = 6
        Form7.Label4.Text = "加速度(m/s^2)"
        Form7.Label5.Text = "时间(s)"
        Form7.Label6.Text = ""
        Form7.TextBox1.Text = ""
        Form7.RadioButton4.Enabled = True
        Form7.RadioButton2.Checked = True

        Form7.PictureBox13.BackgroundImage = Image.FromFile("mainimage\jiemian2.png")

        '调整传感器位置偏移量
        'Dim pyX As Integer = 68
        'Dim pyY As Integer = 52

        Form7.PictureBox23.Location = New System.Drawing.Point(113 + pyX, 369 + pyY)
        Form7.PictureBox26.Location = New System.Drawing.Point(161 + pyX, 369 + pyY)
        Form7.PictureBox18.Location = New System.Drawing.Point(251 + pyX, 366 + pyY)
        Form7.PictureBox27.Location = New System.Drawing.Point(318 + pyX, 369 + pyY)
        Form7.PictureBox24.Location = New System.Drawing.Point(368 + pyX, 369 + pyY)
        Form7.PictureBox31.Location = New System.Drawing.Point(113 + pyX, 337 + pyY)
        Form7.PictureBox29.Location = New System.Drawing.Point(162 + pyX, 337 + pyY)
        Form7.PictureBox32.Location = New System.Drawing.Point(240 + pyX, 336 + pyY)
        Form7.PictureBox28.Location = New System.Drawing.Point(318 + pyX, 337 + pyY)
        Form7.PictureBox30.Location = New System.Drawing.Point(368 + pyX, 337 + pyY)

        Form7.PictureBox33.Visible = False

        '设置加速度路径名
        ReDim Form7.accfolders(6)
        Form7.accfolders = {"11024", "11023", "11022", "11021", "11020", "11019"}

        '设置位移路径名
        ReDim Form7.dispfolders(12)
        Form7.dispfolders = {"16042", "16041", "16040", "16039", "16038", "16037", "16006", "16003", "16005", "16002", "16004", "16001"}

        '设置应变传感器编号
        ReDim Form7.sensornum(24)
        Form7.sensornum = {"087745", "087752", "092432", "092430", "092417", "092429", "092407", "092420", "092422", "092408", "092433", "087749", "087748", "087747", "092434", "092423", "092400", "092431", "092406", "092405", "087746", "092412", "087754", "087751"}
        ReDim Form7.strainfolders(24)
        Form7.strainfolders = {"14096", "14095", "14094", "14093", "14092", "14091", "14090", "14089", "13048", "13047", "13046", "13045", "13044", "13043", "13042", "13041", "13040", "13039", "13038", "13037", "13036", "13035", "13034", "13033"}

        '设置应变传感器零点
        ReDim Form7.strainzero(25)
        Form7.strainzero = {2709.04, 2725.6, 2727.0, 2677.54, 2762.72, 2777.54, 2745.64, 2977.94, 2842.94, 2729.39, 2431.4, 2250.24, 2575.92, 2548.86, 2415.32, 2610.74, 2637.74, 2867.38, 2885.58, 2728.26, 2653.24, 2681.64, 2681.52, 2813.88, 0.0}




        '设置桩号
        Form7.Label7.Text = "B3"
        Form7.Label8.Text = "B4"
        Form7.Label9.Text = "B5"
        Form7.Label10.Text = "P33b"

        '更改立体图中测点显示位置
        'Form7.PictureBox25.Location = New Drawing.Point(126, 256)
        'Form7.PictureBox2.Location = New Drawing.Point(195, 227)

        'Form7.PictureBox22.Location = New Drawing.Point(266, 199)
        'Form7.PictureBox20.Location = New Drawing.Point(301, 184)
        'Form7.PictureBox21.Location = New Drawing.Point(335, 170)

        'Form7.PictureBox16.Location = New Drawing.Point(401, 144)
        'Form7.PictureBox19.Location = New Drawing.Point(432, 131)
        'Form7.PictureBox17.Location = New Drawing.Point(463, 118)
    End Sub

    Private Sub PictureBox7_MouseDown(sender As Object, e As MouseEventArgs) Handles PictureBox7.MouseDown
        PictureBox7.BackgroundImage = Image.FromFile("mainimage\hk\backgroundgray.png")
    End Sub

    Private Sub PictureBox7_MouseUp(sender As Object, e As MouseEventArgs) Handles PictureBox7.MouseUp
        PictureBox7.BackgroundImage = Nothing
    End Sub

    Private Sub PictureBox8_Click(sender As Object, e As EventArgs) Handles PictureBox8.Click
        Form3.Label13.Text = ""
        Form3.Label14.Text = ""

        Me.Hide()
        Form3.Show()
        Form3.Text = "P33(b)-P36"
        bridgeSelected = 7
        Form3.Label4.Text = "加速度(m/s^2)"
        Form3.Label5.Text = "时间(s)"
        Form3.Label6.Text = ""
        Form3.TextBox1.Text = ""
        Form3.RadioButton4.Enabled = True
        Form3.RadioButton2.Checked = True

        Form3.PictureBox13.BackgroundImage = Image.FromFile("mainimage\jiemian2.png")

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

        Form3.PictureBox33.Visible = False

        '设置加速度路径名
        ReDim Form3.accfolders(6)
        Form3.accfolders = {"11025", "11026", "11027", "11028", "11029", "11030"}

        '设置位移路径名
        ReDim Form3.dispfolders(12)
        Form3.dispfolders = {"16043", "16044", "16045", "16046", "16047", "16048", "16049", "16050", "16051", "16052", "16053", "16054"}

        '设置应变传感器编号
        ReDim Form3.sensornum(24)
        Form3.sensornum = {"087745", "087752", "092432", "092430", "092417", "092429", "092407", "092420", "092422", "092408", "092433", "087749", "087748", "087747", "092434", "092423", "092400", "092431", "092406", "092405", "087746", "092412", "087754", "087751"}
        ReDim Form3.strainfolders(24)
        Form3.strainfolders = {"14065", "14066", "14067", "14068", "14069", "14070", "14071", "14072", "14073", "14074", "14075", "14076", "14077", "14078", "14079", "14080", "14081", "14082", "14083", "14084", "14085", "14086", "14087", "14088"}

        '设置应变传感器零点
        ReDim Form3.strainzero(25)
        Form3.strainzero = {2709.04, 2725.6, 2727.0, 2677.54, 2762.72, 2777.54, 2745.64, 2977.94, 2842.94, 2729.39, 2431.4, 2250.24, 2575.92, 2548.86, 2415.32, 2610.74, 2637.74, 2867.38, 2885.58, 2728.26, 2653.24, 2681.64, 2681.52, 2813.88, 0.0}


        Form3.RadioButton4.Enabled = False

        '设置桩号
        Form3.Label7.Text = "P36"
        Form3.Label8.Text = "P35"
        Form3.Label9.Text = "P34"
        Form3.Label10.Text = "P33(b)"

        '更改立体图中测点显示位置
        Form3.PictureBox25.Location = New Drawing.Point(126, 256)
        Form3.PictureBox2.Location = New Drawing.Point(195, 227)

        Form3.PictureBox22.Location = New Drawing.Point(266, 199)
        Form3.PictureBox20.Location = New Drawing.Point(301, 184)
        Form3.PictureBox21.Location = New Drawing.Point(335, 170)

        Form3.PictureBox16.Location = New Drawing.Point(401, 144)
        Form3.PictureBox19.Location = New Drawing.Point(432, 131)
        Form3.PictureBox17.Location = New Drawing.Point(463, 118)
    End Sub

    Private Sub PictureBox8_MouseDown(sender As Object, e As MouseEventArgs) Handles PictureBox8.MouseDown
        PictureBox8.BackgroundImage = Image.FromFile("mainimage\hk\backgroundgray.png")
    End Sub

    Private Sub PictureBox8_MouseUp(sender As Object, e As MouseEventArgs) Handles PictureBox8.MouseUp
        PictureBox8.BackgroundImage = Nothing
    End Sub

    Private Sub PictureBox9_Click(sender As Object, e As EventArgs) Handles PictureBox9.Click
        Dim spanName = "A0-A3"
        FormConfig.spanName = spanName
        FormConfig.Label13.Text = ""
        FormConfig.Label14.Text = ""

        Me.Hide()
        FormConfig.Show()
        FormConfig.Text = Profile.ReadOneString(spanName, "title", "./config/A0.ini")
        bridgeSelected = 7
        FormConfig.Label4.Text = "加速度(m/s^2)"
        FormConfig.Label5.Text = "时间(s)"
        FormConfig.Label6.Text = ""
        FormConfig.TextBox1.Text = ""
        FormConfig.RadioButton2.Checked = True

        FormConfig.PictureBox12.BackgroundImage = Image.FromFile(Profile.ReadOneString(spanName, "pmpic", "./config/A0.ini"))
        FormConfig.PictureBox13.BackgroundImage = Image.FromFile("mainimage\jiemian4.png")

        '调整传感器位置偏移量
        Dim pyX As Integer = 0
        Dim pyY As Integer = 0

        FormConfig.PictureBox17.Location = New System.Drawing.Point(Profile.ReadOneString(spanName, "lm1pos", "./config/A0.ini").Split(",")(0) + pyX, Profile.ReadOneString(spanName, "lm1pos", "./config/A0.ini").Split(",")(1) + pyY)
        FormConfig.PictureBox34.Location = New System.Drawing.Point(Profile.ReadOneString(spanName, "lm2pos", "./config/A0.ini").Split(",")(0) + pyX, Profile.ReadOneString(spanName, "lm2pos", "./config/A0.ini").Split(",")(1) + pyY)
        FormConfig.PictureBox19.Location = New System.Drawing.Point(Profile.ReadOneString(spanName, "lm3pos", "./config/A0.ini").Split(",")(0) + pyX, Profile.ReadOneString(spanName, "lm3pos", "./config/A0.ini").Split(",")(1) + pyY)
        FormConfig.PictureBox35.Location = New System.Drawing.Point(Profile.ReadOneString(spanName, "lm4pos", "./config/A0.ini").Split(",")(0) + pyX, Profile.ReadOneString(spanName, "lm4pos", "./config/A0.ini").Split(",")(1) + pyY)
        FormConfig.PictureBox16.Location = New System.Drawing.Point(Profile.ReadOneString(spanName, "lm5pos", "./config/A0.ini").Split(",")(0) + pyX, Profile.ReadOneString(spanName, "lm5pos", "./config/A0.ini").Split(",")(1) + pyY)
        FormConfig.PictureBox21.Location = New System.Drawing.Point(Profile.ReadOneString(spanName, "lm6pos", "./config/A0.ini").Split(",")(0) + pyX, Profile.ReadOneString(spanName, "lm6pos", "./config/A0.ini").Split(",")(1) + pyY)
        FormConfig.PictureBox20.Location = New System.Drawing.Point(Profile.ReadOneString(spanName, "lm7pos", "./config/A0.ini").Split(",")(0) + pyX, Profile.ReadOneString(spanName, "lm7pos", "./config/A0.ini").Split(",")(1) + pyY)
        FormConfig.PictureBox22.Location = New System.Drawing.Point(Profile.ReadOneString(spanName, "lm8pos", "./config/A0.ini").Split(",")(0) + pyX, Profile.ReadOneString(spanName, "lm8pos", "./config/A0.ini").Split(",")(1) + pyY)
        FormConfig.PictureBox2.Location = New System.Drawing.Point(Profile.ReadOneString(spanName, "lm9pos", "./config/A0.ini").Split(",")(0) + pyX, Profile.ReadOneString(spanName, "lm9pos", "./config/A0.ini").Split(",")(1) + pyY)
        FormConfig.PictureBox25.Location = New System.Drawing.Point(Profile.ReadOneString(spanName, "lm10pos", "./config/A0.ini").Split(",")(0) + pyX, Profile.ReadOneString(spanName, "lm10pos", "./config/A0.ini").Split(",")(1) + pyY)



        '设置加速度路径名
        Dim accCount As Integer = Profile.ReadOneString(spanName, "accCount", "./config/A0.ini")
        Dim accNums = Profile.ReadOneString(spanName, "accNum", "./config/A0.ini").Split(",")
        ReDim FormConfig.accfolders(accCount)
        'FormConfig.accfolders = {"11025", "11026", "11027", "11028", "11029", "11030"}

        For i = 0 To accCount - 1
            FormConfig.accfolders(i) = accNums(i)
        Next

        '设置位移路径名
        Dim dispCount As Integer = Profile.ReadOneString(spanName, "dispCount", "./config/A0.ini")
        Dim dispNums = Profile.ReadOneString(spanName, "dispNum", "./config/A0.ini").Split(",")
        ReDim FormConfig.dispfolders(dispCount)
        For i = 0 To dispCount - 1
            FormConfig.dispfolders(i) = dispNums(i)
        Next
        'FormConfig.dispfolders = {"16043", "16044", "16045", "16046", "16047", "16048", "16049", "16050", "16051", "16052", "16053", "16054"}

        '设置应变传感器编号
        Dim strainCount As Integer = Profile.ReadOneString(spanName, "strainCount", "./config/A0.ini")
        Dim strainNums = Profile.ReadOneString(spanName, "strainNum", "./config/A0.ini").Split(",")
        ReDim FormConfig.sensornum(strainCount)
        FormConfig.sensornum = {"087745", "087752", "092432", "092430", "092417", "092429", "092407", "092420", "092422", "092408", "092433", "087749", "087748", "087747", "092434", "092423", "092400", "092431", "092406", "092405", "087746", "092412", "087754", "087751"}
        ReDim FormConfig.strainfolders(strainCount)
        For i = 0 To strainCount - 1
            FormConfig.strainfolders(i) = strainNums(i)
        Next
        'FormConfig.strainfolders = {"14065", "14066", "14067", "14068", "14069", "14070", "14071", "14072", "14073", "14074", "14075", "14076", "14077", "14078", "14079", "14080", "14081", "14082", "14083", "14084", "14085", "14086", "14087", "14088"}

        '设置应变传感器零点
        ReDim FormConfig.strainzero(25)
        FormConfig.strainzero = {2709.04, 2725.6, 2727.0, 2677.54, 2762.72, 2777.54, 2745.64, 2977.94, 2842.94, 2729.39, 2431.4, 2250.24, 2575.92, 2548.86, 2415.32, 2610.74, 2637.74, 2867.38, 2885.58, 2728.26, 2653.24, 2681.64, 2681.52, 2813.88, 0.0}


        If ("false" = Profile.ReadOneString(spanName, "hastemp", "./config/A0.ini")) Then
            FormConfig.RadioButton4.Enabled = False
        Else
            FormConfig.RadioButton4.Enabled = True
        End If


        '设置桩号
        FormConfig.Label7.Text = Profile.ReadOneString(spanName, "zh1", "./config/A0.ini")
        FormConfig.Label8.Text = Profile.ReadOneString(spanName, "zh2", "./config/A0.ini")
        FormConfig.Label9.Text = Profile.ReadOneString(spanName, "zh3", "./config/A0.ini")
        FormConfig.Label10.Text = Profile.ReadOneString(spanName, "zh4", "./config/A0.ini")

        '平面图文字
        FormConfig.Label12.Text = Profile.ReadOneString("A0-A3", "fw1", "./config/A0.ini")
        FormConfig.Label11.Text = Profile.ReadOneString("A0-A3", "fw2", "./config/A0.ini")

        '如果跨数为2，则遮挡剩余一跨
        If ("2" = Profile.ReadOneString(spanName, "span", "./config/A0.ini")) Then
            FormConfig.PictureBox33.Visible = True
        Else
            FormConfig.PictureBox33.Visible = False
        End If
    End Sub

    Private Sub PictureBox10_Click(sender As Object, e As EventArgs) Handles PictureBox10.Click
        Dim spanName = "A3-A5"
        FormConfig.spanName = spanName
        FormConfig.Label13.Text = ""
        FormConfig.Label14.Text = ""

        Me.Hide()
        FormConfig.Show()
        FormConfig.Text = Profile.ReadOneString(spanName, "title", "./config/A0.ini")
        bridgeSelected = 7
        FormConfig.Label4.Text = "加速度(m/s^2)"
        FormConfig.Label5.Text = "时间(s)"
        FormConfig.Label6.Text = ""
        FormConfig.TextBox1.Text = ""
        FormConfig.RadioButton2.Checked = True

        FormConfig.PictureBox12.BackgroundImage = Image.FromFile(Profile.ReadOneString(spanName, "pmpic", "./config/A0.ini"))
        FormConfig.PictureBox13.BackgroundImage = Image.FromFile("mainimage\jiemian4.png")

        '调整传感器位置偏移量
        Dim pyX As Integer = 0
        Dim pyY As Integer = 0

        FormConfig.PictureBox17.Location = New System.Drawing.Point(Profile.ReadOneString(spanName, "lm1pos", "./config/A0.ini").Split(",")(0) + pyX, Profile.ReadOneString(spanName, "lm1pos", "./config/A0.ini").Split(",")(1) + pyY)
        FormConfig.PictureBox34.Location = New System.Drawing.Point(Profile.ReadOneString(spanName, "lm2pos", "./config/A0.ini").Split(",")(0) + pyX, Profile.ReadOneString(spanName, "lm2pos", "./config/A0.ini").Split(",")(1) + pyY)
        FormConfig.PictureBox19.Location = New System.Drawing.Point(Profile.ReadOneString(spanName, "lm3pos", "./config/A0.ini").Split(",")(0) + pyX, Profile.ReadOneString(spanName, "lm3pos", "./config/A0.ini").Split(",")(1) + pyY)
        FormConfig.PictureBox35.Location = New System.Drawing.Point(Profile.ReadOneString(spanName, "lm4pos", "./config/A0.ini").Split(",")(0) + pyX, Profile.ReadOneString(spanName, "lm4pos", "./config/A0.ini").Split(",")(1) + pyY)
        FormConfig.PictureBox16.Location = New System.Drawing.Point(Profile.ReadOneString(spanName, "lm5pos", "./config/A0.ini").Split(",")(0) + pyX, Profile.ReadOneString(spanName, "lm5pos", "./config/A0.ini").Split(",")(1) + pyY)
        FormConfig.PictureBox21.Location = New System.Drawing.Point(Profile.ReadOneString(spanName, "lm6pos", "./config/A0.ini").Split(",")(0) + pyX, Profile.ReadOneString(spanName, "lm6pos", "./config/A0.ini").Split(",")(1) + pyY)
        FormConfig.PictureBox20.Location = New System.Drawing.Point(Profile.ReadOneString(spanName, "lm7pos", "./config/A0.ini").Split(",")(0) + pyX, Profile.ReadOneString(spanName, "lm7pos", "./config/A0.ini").Split(",")(1) + pyY)
        FormConfig.PictureBox22.Location = New System.Drawing.Point(Profile.ReadOneString(spanName, "lm8pos", "./config/A0.ini").Split(",")(0) + pyX, Profile.ReadOneString(spanName, "lm8pos", "./config/A0.ini").Split(",")(1) + pyY)
        FormConfig.PictureBox2.Location = New System.Drawing.Point(Profile.ReadOneString(spanName, "lm9pos", "./config/A0.ini").Split(",")(0) + pyX, Profile.ReadOneString(spanName, "lm9pos", "./config/A0.ini").Split(",")(1) + pyY)
        FormConfig.PictureBox25.Location = New System.Drawing.Point(Profile.ReadOneString(spanName, "lm10pos", "./config/A0.ini").Split(",")(0) + pyX, Profile.ReadOneString(spanName, "lm10pos", "./config/A0.ini").Split(",")(1) + pyY)


        '设置加速度路径名
        Dim accCount As Integer = Profile.ReadOneString(spanName, "accCount", "./config/A0.ini")
        Dim accNums = Profile.ReadOneString(spanName, "accNum", "./config/A0.ini").Split(",")
        ReDim FormConfig.accfolders(accCount)
        'FormConfig.accfolders = {"11025", "11026", "11027", "11028", "11029", "11030"}

        For i = 0 To accCount - 1
            FormConfig.accfolders(i) = accNums(i)
        Next

        '设置位移路径名
        Dim dispCount As Integer = Profile.ReadOneString(spanName, "dispCount", "./config/A0.ini")
        Dim dispNums = Profile.ReadOneString(spanName, "dispNum", "./config/A0.ini").Split(",")
        ReDim FormConfig.dispfolders(dispCount)
        For i = 0 To dispCount - 1
            FormConfig.dispfolders(i) = dispNums(i)
        Next
        'FormConfig.dispfolders = {"16043", "16044", "16045", "16046", "16047", "16048", "16049", "16050", "16051", "16052", "16053", "16054"}

        '设置应变传感器编号
        Dim strainCount As Integer = Profile.ReadOneString(spanName, "strainCount", "./config/A0.ini")
        Dim strainNums = Profile.ReadOneString(spanName, "strainNum", "./config/A0.ini").Split(",")
        ReDim FormConfig.sensornum(strainCount)
        FormConfig.sensornum = {"087745", "087752", "092432", "092430", "092417", "092429", "092407", "092420", "092422", "092408", "092433", "087749", "087748", "087747", "092434", "092423", "092400", "092431", "092406", "092405", "087746", "092412", "087754", "087751"}
        ReDim FormConfig.strainfolders(strainCount)
        For i = 0 To strainCount - 1
            FormConfig.strainfolders(i) = strainNums(i)
        Next
        'FormConfig.strainfolders = {"14065", "14066", "14067", "14068", "14069", "14070", "14071", "14072", "14073", "14074", "14075", "14076", "14077", "14078", "14079", "14080", "14081", "14082", "14083", "14084", "14085", "14086", "14087", "14088"}

        '设置应变传感器零点
        ReDim FormConfig.strainzero(25)
        FormConfig.strainzero = {2709.04, 2725.6, 2727.0, 2677.54, 2762.72, 2777.54, 2745.64, 2977.94, 2842.94, 2729.39, 2431.4, 2250.24, 2575.92, 2548.86, 2415.32, 2610.74, 2637.74, 2867.38, 2885.58, 2728.26, 2653.24, 2681.64, 2681.52, 2813.88, 0.0}


        If ("false" = Profile.ReadOneString(spanName, "hastemp", "./config/A0.ini")) Then
            FormConfig.RadioButton4.Enabled = False
        Else
            FormConfig.RadioButton4.Enabled = True
        End If


        '设置桩号
        FormConfig.Label7.Text = Profile.ReadOneString(spanName, "zh1", "./config/A0.ini")
        FormConfig.Label8.Text = Profile.ReadOneString(spanName, "zh2", "./config/A0.ini")
        FormConfig.Label9.Text = Profile.ReadOneString(spanName, "zh3", "./config/A0.ini")
        FormConfig.Label10.Text = Profile.ReadOneString(spanName, "zh4", "./config/A0.ini")

        '平面图文字
        FormConfig.Label12.Text = Profile.ReadOneString("A0-A3", "fw1", "./config/A0.ini")
        FormConfig.Label11.Text = Profile.ReadOneString("A0-A3", "fw2", "./config/A0.ini")

        '如果跨数为2，则遮挡剩余一跨
        If ("2" = Profile.ReadOneString(spanName, "span", "./config/A0.ini")) Then
            FormConfig.PictureBox33.Visible = True
        Else
            FormConfig.PictureBox33.Visible = False
        End If
    End Sub

    Private Sub PictureBox11_Click(sender As Object, e As EventArgs) Handles PictureBox11.Click
        Dim spanName = "A5-A8"
        FormConfig.spanName = spanName
        FormConfig.Label13.Text = ""
        FormConfig.Label14.Text = ""

        Me.Hide()
        FormConfig.Show()
        FormConfig.Text = Profile.ReadOneString(spanName, "title", "./config/A0.ini")
        bridgeSelected = 7
        FormConfig.Label4.Text = "加速度(m/s^2)"
        FormConfig.Label5.Text = "时间(s)"
        FormConfig.Label6.Text = ""
        FormConfig.TextBox1.Text = ""
        FormConfig.RadioButton2.Checked = True

        FormConfig.PictureBox12.BackgroundImage = Image.FromFile(Profile.ReadOneString(spanName, "pmpic", "./config/A0.ini"))
        FormConfig.PictureBox13.BackgroundImage = Image.FromFile("mainimage\jiemian4.png")

        '调整传感器位置偏移量
        Dim pyX As Integer = 0
        Dim pyY As Integer = 0

        FormConfig.PictureBox17.Location = New System.Drawing.Point(Profile.ReadOneString(spanName, "lm1pos", "./config/A0.ini").Split(",")(0) + pyX, Profile.ReadOneString(spanName, "lm1pos", "./config/A0.ini").Split(",")(1) + pyY)
        FormConfig.PictureBox34.Location = New System.Drawing.Point(Profile.ReadOneString(spanName, "lm2pos", "./config/A0.ini").Split(",")(0) + pyX, Profile.ReadOneString(spanName, "lm2pos", "./config/A0.ini").Split(",")(1) + pyY)
        FormConfig.PictureBox19.Location = New System.Drawing.Point(Profile.ReadOneString(spanName, "lm3pos", "./config/A0.ini").Split(",")(0) + pyX, Profile.ReadOneString(spanName, "lm3pos", "./config/A0.ini").Split(",")(1) + pyY)
        FormConfig.PictureBox35.Location = New System.Drawing.Point(Profile.ReadOneString(spanName, "lm4pos", "./config/A0.ini").Split(",")(0) + pyX, Profile.ReadOneString(spanName, "lm4pos", "./config/A0.ini").Split(",")(1) + pyY)
        FormConfig.PictureBox16.Location = New System.Drawing.Point(Profile.ReadOneString(spanName, "lm5pos", "./config/A0.ini").Split(",")(0) + pyX, Profile.ReadOneString(spanName, "lm5pos", "./config/A0.ini").Split(",")(1) + pyY)
        FormConfig.PictureBox21.Location = New System.Drawing.Point(Profile.ReadOneString(spanName, "lm6pos", "./config/A0.ini").Split(",")(0) + pyX, Profile.ReadOneString(spanName, "lm6pos", "./config/A0.ini").Split(",")(1) + pyY)
        FormConfig.PictureBox20.Location = New System.Drawing.Point(Profile.ReadOneString(spanName, "lm7pos", "./config/A0.ini").Split(",")(0) + pyX, Profile.ReadOneString(spanName, "lm7pos", "./config/A0.ini").Split(",")(1) + pyY)
        FormConfig.PictureBox22.Location = New System.Drawing.Point(Profile.ReadOneString(spanName, "lm8pos", "./config/A0.ini").Split(",")(0) + pyX, Profile.ReadOneString(spanName, "lm8pos", "./config/A0.ini").Split(",")(1) + pyY)
        FormConfig.PictureBox2.Location = New System.Drawing.Point(Profile.ReadOneString(spanName, "lm9pos", "./config/A0.ini").Split(",")(0) + pyX, Profile.ReadOneString(spanName, "lm9pos", "./config/A0.ini").Split(",")(1) + pyY)
        FormConfig.PictureBox25.Location = New System.Drawing.Point(Profile.ReadOneString(spanName, "lm10pos", "./config/A0.ini").Split(",")(0) + pyX, Profile.ReadOneString(spanName, "lm10pos", "./config/A0.ini").Split(",")(1) + pyY)


        '设置加速度路径名
        Dim accCount As Integer = Profile.ReadOneString(spanName, "accCount", "./config/A0.ini")
        Dim accNums = Profile.ReadOneString(spanName, "accNum", "./config/A0.ini").Split(",")
        ReDim FormConfig.accfolders(accCount)
        'FormConfig.accfolders = {"11025", "11026", "11027", "11028", "11029", "11030"}

        For i = 0 To accCount - 1
            FormConfig.accfolders(i) = accNums(i)
        Next

        '设置位移路径名
        Dim dispCount As Integer = Profile.ReadOneString(spanName, "dispCount", "./config/A0.ini")
        Dim dispNums = Profile.ReadOneString(spanName, "dispNum", "./config/A0.ini").Split(",")
        ReDim FormConfig.dispfolders(dispCount)
        For i = 0 To dispCount - 1
            FormConfig.dispfolders(i) = dispNums(i)
        Next
        'FormConfig.dispfolders = {"16043", "16044", "16045", "16046", "16047", "16048", "16049", "16050", "16051", "16052", "16053", "16054"}

        '设置应变传感器编号
        Dim strainCount As Integer = Profile.ReadOneString(spanName, "strainCount", "./config/A0.ini")
        Dim strainNums = Profile.ReadOneString(spanName, "strainNum", "./config/A0.ini").Split(",")
        ReDim FormConfig.sensornum(strainCount)
        FormConfig.sensornum = {"087745", "087752", "092432", "092430", "092417", "092429", "092407", "092420", "092422", "092408", "092433", "087749", "087748", "087747", "092434", "092423", "092400", "092431", "092406", "092405", "087746", "092412", "087754", "087751"}
        ReDim FormConfig.strainfolders(strainCount)
        For i = 0 To strainCount - 1
            FormConfig.strainfolders(i) = strainNums(i)
        Next
        'FormConfig.strainfolders = {"14065", "14066", "14067", "14068", "14069", "14070", "14071", "14072", "14073", "14074", "14075", "14076", "14077", "14078", "14079", "14080", "14081", "14082", "14083", "14084", "14085", "14086", "14087", "14088"}

        '设置应变传感器零点
        ReDim FormConfig.strainzero(25)
        FormConfig.strainzero = {2709.04, 2725.6, 2727.0, 2677.54, 2762.72, 2777.54, 2745.64, 2977.94, 2842.94, 2729.39, 2431.4, 2250.24, 2575.92, 2548.86, 2415.32, 2610.74, 2637.74, 2867.38, 2885.58, 2728.26, 2653.24, 2681.64, 2681.52, 2813.88, 0.0}


        If ("false" = Profile.ReadOneString(spanName, "hastemp", "./config/A0.ini")) Then
            FormConfig.RadioButton4.Enabled = False
        Else
            FormConfig.RadioButton4.Enabled = True
        End If


        '设置桩号
        FormConfig.Label7.Text = Profile.ReadOneString(spanName, "zh1", "./config/A0.ini")
        FormConfig.Label8.Text = Profile.ReadOneString(spanName, "zh2", "./config/A0.ini")
        FormConfig.Label9.Text = Profile.ReadOneString(spanName, "zh3", "./config/A0.ini")
        FormConfig.Label10.Text = Profile.ReadOneString(spanName, "zh4", "./config/A0.ini")

        '平面图文字
        FormConfig.Label12.Text = Profile.ReadOneString("A0-A3", "fw1", "./config/A0.ini")
        FormConfig.Label11.Text = Profile.ReadOneString("A0-A3", "fw2", "./config/A0.ini")

        '如果跨数为2，则遮挡剩余一跨
        If ("2" = Profile.ReadOneString(spanName, "span", "./config/A0.ini")) Then
            FormConfig.PictureBox33.Visible = True
        Else
            FormConfig.PictureBox33.Visible = False
        End If
    End Sub
End Class
