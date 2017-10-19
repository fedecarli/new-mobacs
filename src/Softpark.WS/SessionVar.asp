<%
Dim sT, page
Response.ContentType = "text/plain; charset=utf-8"
If Request.ServerVariables("REMOTE_ADDR") = Request.ServerVariables("LOCAL_ADDR") Then
    sT = Request("SessionVar")
    If trim(sT) <> "" Then
      Response.Write Session(sT)
    End If
End If
%>