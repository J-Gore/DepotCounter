Imports System.IO
Imports System.Net
Imports Newtonsoft.Json

Module Program
    Sub Main()
        Dim request = WebRequest.Create("https://portal.rockgympro.com/portal/public/4f7e4c65977f6cd9be6d61308c7d7cc2/occupancy")
        Dim response = request.GetResponse()
        Dim responseStream = response.GetResponseStream
        Dim streamReader = New StreamReader(responseStream)
        Dim obj = "{"
        Dim reading = False

        While True
            Dim x = streamReader.ReadLine
            If x.Contains("var data = ") Then
                reading = True
                Continue While
            ElseIf x.Contains("};") Then
                obj += "}}"
                Exit While
            End If
            If reading Then obj += x.Replace(Chr(39), Chr(34))
        End While

        responseStream.Close()
        streamReader.Close()
        response.Close()

        Dim item As Object = JsonConvert.DeserializeObject(Of Object)(obj)
        Dim man = item("MAN")
        Dim count = man("count")
        Dim capacity = man("capacity")
        Dim lastupdate = man("lastUpdate")

        Dim timeStr As String
        timeStr = lastupdate.ToString
        Dim s = timeStr.IndexOf("(") + 1
        Dim e = timeStr.IndexOf(")")

        timeStr = timeStr.Substring(s, e - s)
        Dim out = Date.Now.ToString("yyyy/MM/dd") + " " + timeStr + "," + count.ToString + "," + capacity.ToString

        Dim w = New StreamWriter("out.csv", True)
        w.WriteLine(out)
        w.Close()
    End Sub
End Module
