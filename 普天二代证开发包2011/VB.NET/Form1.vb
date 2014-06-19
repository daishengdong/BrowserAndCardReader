Imports System.Io
Imports System.Drawing
Imports System.Text
Public Class Form1
    Dim person As Card2.PERSONINFOW
    Dim birthday As String
    Dim validDate As String
    Dim image As Image
    Dim maxErrorTextLen As Integer

    Public Sub New()

        ' 此调用是 Windows 窗体设计器所必需的。
        InitializeComponent()

        ' 在 InitializeComponent() 调用之后添加任何初始化。
        person = New Card2.PERSONINFOW()
        birthday = ""
        validDate = ""
        image = Nothing
        maxErrorTextLen = 32
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim result As Int32
        Dim errorText As StringBuilder

        errorText = New StringBuilder(maxErrorTextLen)
        '参数1为端口号。1表示串口1，2表示串口2，依次类推。1001表示USB。0表示自动选择。
        '参数2为标志位。0x02表示启用重复读卡。0x04表示读卡后接着读取新地址。
        '各个数值可以用“按位或”运算符组合起来。
        '参数3为波特率。使用串口阅读器的程序应正确设置此参数。出厂机器的波特率一般为115200。
        result = Card2.OpenCardReader(0, 2, 115200)
        textResult.Text = Convert.ToString(result)
        Card2.GetErrorTextW(errorText, maxErrorTextLen)
        textDescription.Text = errorText.ToString()
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Dim result As Int32
        Dim imagePath As String
        Dim errorText As StringBuilder

        errorText = New StringBuilder(maxErrorTextLen)
        If image IsNot Nothing Then
            image.Dispose()
            image = Nothing
        End If
        imagePath = Path.GetTempPath() + "image.bmp"
        '当程序打开设备后，可以多次调用读取信息函数。
        result = Card2.GetPersonMsgW(person, imagePath)
        textResult.Text = Convert.ToString(result)
        Card2.GetErrorTextW(errorText, maxErrorTextLen)
        textDescription.Text = errorText.ToString()

        If 0 = result Then
            birthday = ConvertDate(person.birthday, 1)
            validDate = ConvertDate(person.validStart, 2) & "-"
            validDate += ConvertDate(person.validEnd, 2)
            image = New Bitmap(imagePath)
        Else
            birthday = ""
            validDate = ""
        End If
        Invalidate()
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        Dim result As Int32
        Dim errorText As StringBuilder

        errorText = New StringBuilder(maxErrorTextLen)
        '读完卡后，必须关闭设备。
        result = Card2.CloseCardReader()
        textResult.Text = Convert.ToString(result)
        Card2.GetErrorTextW(errorText, maxErrorTextLen)
        textDescription.Text = errorText.ToString()
    End Sub

    Sub SetRect(ByRef rect As RectangleF, ByVal x As Single, ByVal y As Single, ByVal width As Single, ByVal height As Single)
        rect.X = x
        rect.Y = y
        rect.Width = width
        rect.Height = height
    End Sub
    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        MyBase.OnPaint(e)
        Dim g As Graphics
        Dim font As Font
        Dim brush As Brush
        Dim rect As RectangleF

        g = e.Graphics
        font = New Font("宋体", 12, FontStyle.Regular)
        brush = New SolidBrush(Color.FromArgb(192, 128, 128))
        rect = New RectangleF()
        SetRect(rect, 10, 10, 90, 16)
        g.DrawString("姓名", font, brush, rect)
        SetRect(rect, 10, 30, 90, 16)
        g.DrawString("性别", font, brush, rect)
        SetRect(rect, 10, 50, 90, 16)
        g.DrawString("民族", font, brush, rect)
        SetRect(rect, 10, 70, 90, 16)
        g.DrawString("出生日期", font, brush, rect)
        SetRect(rect, 10, 90, 90, 16)
        g.DrawString("地址", font, brush, rect)
        SetRect(rect, 10, 130, 90, 16)
        g.DrawString("身份证号码", font, brush, rect)
        SetRect(rect, 10, 150, 90, 16)
        g.DrawString("签发机关", font, brush, rect)
        SetRect(rect, 10, 170, 90, 16)
        g.DrawString("有效期限", font, brush, rect)
        brush.Dispose()

        If person.name IsNot Nothing Then
            brush = New SolidBrush(Color.Black)
            SetRect(rect, 100, 10, 300, 16)
            g.DrawString(person.name, font, brush, rect)
            SetRect(rect, 100, 30, 100, 16)
            g.DrawString(person.sex, font, brush, rect)
            SetRect(rect, 100, 50, 150, 16)
            g.DrawString(person.nation, font, brush, rect)
            SetRect(rect, 100, 70, 300, 16)
            g.DrawString(birthday, font, brush, rect)
            SetRect(rect, 100, 90, 300, 34)
            g.DrawString(person.address, font, brush, rect)
            SetRect(rect, 100, 130, 300, 16)
            g.DrawString(person.cardId, font, brush, rect)
            SetRect(rect, 100, 150, 300, 16)
            g.DrawString(person.police, font, brush, rect)
            SetRect(rect, 100, 170, 300, 16)
            g.DrawString(validDate, font, brush, rect)
            brush.Dispose()
        End If
        font.Dispose()

        If image IsNot Nothing Then
            g.DrawImage(image, 400, 10)
        End If
    End Sub
    Function ConvertDate(ByVal str As String, ByVal mode As Integer) As String
        Dim year As String
        Dim month As String
        Dim day As String
        If 1 = mode Then
            If str.Length >= 8 Then
                year = str.Substring(0, 4)
                month = str.Substring(4, 2)
                day = str.Substring(6, 2)
                Return String.Format("{0}年{1}月{2}日", year, month, day)
            End If
        ElseIf 2 = mode Then
            If str.Equals("长期") Then
                Return "长期"
            Else
                If str.Length >= 8 Then
                    year = str.Substring(0, 4)
                    month = str.Substring(4, 2)
                    day = str.Substring(6, 2)
                    Return String.Format("{0}.{1}.{2}", year, month, day)
                End If
            End If
        End If
        Return ""
    End Function
End Class
