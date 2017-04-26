@Imports System
@Imports Softpark.WS.Controllers
@ModelType Softpark.Models.DomainContainer
@Code
    Function MontaMenuTem(vNumSecao,vTipoM)
        Dim vTempMenu=""

        Dim bg = (From s In Model.ASSMED_Secao.AsEnumerable
                  Join ss In Model.ASSMED_SistemaSecao
                      On s.NumSecao Equals ss.NumSecao
                  Where (IsDBNull(ss.Janela) Or ss.Janela Is "") AndAlso
                     ss.CodSistema = 99 AndAlso ss.NumSecao = vNumSecao
                  Select New With { .Menu = ss.Menu, .NumSecao = ss.NumSecao, .NomProg = s.NomProg,
                      .Imagem = s.Imagem, .ImagemI = s.ImagemI, .ImgTam = s.ImgTam, .ImgAlt = s.ImgAlt,
                      .DesMenu = s.DesSecao }).FirstOrDefault()

        If Not IsNothing(bg) Then
            Dim vProgPesquisa=""
            Dim vNomProg=""
            Dim bgo = Model.ASSMED_Programa.FirstOrDefault(Function(x) x.NomProg Is Trim(bg.NomProg))
            If Not IsNothing(bgo) Then
                vProgPesquisa=Trim(bgo.ProgPesquisa)
                vNomProg=vProgPesquisa
            End IF

            bgo=Nothing

            Dim vTemDireitos = False
            If Trim(bg.NomProg) > "" Then
                Dim bd = From d In Model.ASSMED_Direitos Where d.NumContrato = 22 AndAlso d.CodUsu = User.Usuario().CodUsu AndAlso d.NomProg = bg.NomProg
                If Not IsNothing(bd) Then
                    vTemDireitos = True
                End If

                bd=Nothing
            End If

            If vProgPesquisa <= "" Then vNomProg=Trim(bg.NomProg)
            If vNomProg <= "" or vTemDireitos=0 Then
                If vTipoM="P1" Then
                    vTempMenu=vTempMenu & "<a href='#nogo' class='mFirst'>"
                Else
                    vTempMenu=vTempMenu & "<a href='#nogo'>"
                End if
            Else
                vTempMenu=vTempMenu & "<a href='" & vNomProg & "'>"
                If vNomProg="ASSMED_Debugar" then
                    'vTempMenu=vTempMenu & "javascript:ModoDesenvolvedor();" & chr(34)
                Else
                    'vTempMenu=vTempMenu & "javascript:ChamaProgramaV2('','" & vNomProg & "','','','','');" & chr(34)
                End if
            End If

            If Trim(bg.Imagem) > "" Then
                'vTempMenu=vTempMenu & " onMouseOut=" & chr(34) & "MM_swapImgRestore();" & chr(34) & " onMouseOver=" & chr(34)
                'vTempMenu=vTempMenu & "MM_swapImage('MenuMenu" & Trim(bg("NumSecao")) & "','','BtMenu/" & Trim(bg("ImagemI")) & "',1);" & chr(34) & ">"
                'vTempMenu=vTempMenu & "<img name='MenuMenu" & Trim(bg("NumSecao")) & "' src='BtMenu/" & Trim(bg("Imagem")) & "' "
                'vTempMenu=vTempMenu & " width='" & cInt(bg("ImgTam")) & "' height='" & cInt(bg("ImgAlt")) & "' border='0' alt='" & Trim(bg("DesMenu")) & "'>
                vTempMenu=vTempMenu & Trim(bg.DesMenu)
            Else
                vTempMenu=vTempMenu & Trim(bg.DesMenu)
            End If

            If vTipoM="U" Then
                vTempMenu = "<li>" & vTempMenu & "</a></li>"
            Else
                vTempMenu=vTempMenu & "<span class='fa arrow'></a>"
            End If
        End If

       bg=Nothing

       MontaMenuTem=vTempMenu
    End Function

    Dim vMenuPrin=""
    Dim arMenuPrin(10)
    Dim arSubMenu
    Dim arItensMenu
    For jm=0 to 10
        arMenuPrin(jm)=""
    Next

    For jm=0 to 9
        vPes=" Select Top 1 IsNull(s.Menu,0) as Menu,IsNull(s.NumSecao,0) as NumSecao,'' as ProgPesquisa,'' as NomProg,s.Imagem,s.ImagemI,IsNull(s.ImgTam,0) as ImgTam,IsNull(s.ImgAlt,0) as ImgAlt,s.DesSecao as DesMenu "
        vPes=vPes & " From ASSMED_Secao s,ASSMED_SistemaSecao ss Where (s.NomProg <='' or s.NomProg is Null) and s.NumSecao=ss.NumSecao and ss.Janela<='' and ss.CodSistema=" & vCodSistema & " and ss.Menu=" & jm
                vPes=vPes & " Order By 1,4 "
        Set bI=objConn.Exec(vPes)
        If Not bi.Eof and NOt bi.Bof Then
    While Not bi.Eof
    If arMenuPrin(jm) > "" Then arMenuPrin(jm)=arMenuPrin(jm) & ";"
                        arMenuPrin(jm)=arMenuPrin(jm) & cInt(bi("NumSecao"))
                        bi.MoveNext
            WEnd
        End If
                bi.Close
        Set bi=Nothing
    Next
    For jm=0 to 9
                    vPes=""
                    'vPes=" Select Top 1 IsNull(s.Menu,0) as Menu,IsNull(s.NumSecao,0) as NumSecao,'' as ProgPesquisa,'' as NomProg,s.Imagem,s.ImagemI,IsNull(s.ImgTam,0) as ImgTam,IsNull(s.ImgAlt,0) as ImgAlt,s.DesSecao as DesMenu "
                    'vPes=vPes & " From ASSMED_Secao s,ASSMED_SistemaSecao ss Where (s.NomProg <='' or s.NomProg is Null) and s.NumSecao=ss.NumSecao and ss.Janela<='' and ss.CodSistema=" & vCodSistema & " and ss.Menu=" & jm
                    'vPes=vPes & " UNION "
                    vPes=vPes & "Select IsNull(ss.Menu,0) as Menu,IsNull(ss.NumSecao,0) as NumSecao,p.ProgPesquisa,s.NomProg,s.Imagem,s.ImagemI,IsNull(s.ImgTam,0) as ImgTam,IsNull(s.ImgAlt,0) as ImgAlt,s.DesSecao as DesMenu "
                    vPes=vPes & " From ASSMED_Programa p,ASSMED_Secao s,ASSMED_SistemaSecao ss Where p.NomProg=s.NomProg and s.NumSecao=ss.NumSecao and ss.Janela<='' and ss.CodSistema=" & vCodSistema & " and ss.Menu=" & jm ' & " * 10 and ss.Menu<=(" & jm & " * 10) + 9 "
                    vPes=vPes & " Order By 1,4 "
        Set bI=objConn.Exec(vPes)
        If Not bi.Eof and NOt bi.Bof Then
    While Not bi.Eof
    If arMenuPrin(jm) > "" Then arMenuPrin(jm)=arMenuPrin(jm) & ";"
                            arMenuPrin(jm)=arMenuPrin(jm) & cInt(bi("NumSecao"))
                            bi.MoveNext
            WEnd
        End If
                    bi.Close
        Set bi=Nothing
    Next

    For jm=0 to 9
                    arMenuI=""
                    For jmi=0 to 9
                        arMenuI=""
                        vPes=" Select Top 1 IsNull(s.Menu,0) as Menu,IsNull(s.NumSecao,0) as NumSecao,'' as ProgPesquisa,'' as NomProg,s.Imagem,s.ImagemI,IsNull(s.ImgTam,0) as ImgTam,IsNull(s.ImgAlt,0) as ImgAlt,s.DesSecao as DesMenu "
                        If jm=0 Then
                            vPes=vPes & " From ASSMED_Secao s,ASSMED_SistemaSecao ss Where (s.NomProg <='' or s.NomProg is Null) and s.NumSecao=ss.NumSecao and ss.Janela<='' and ss.CodSistema=" & vCodSistema & " and ss.Menu=100 + " & jmi
                        Else
                            vPes=vPes & " From ASSMED_Secao s,ASSMED_SistemaSecao ss Where (s.NomProg <='' or s.NomProg is Null) and s.NumSecao=ss.NumSecao and ss.Janela<='' and ss.CodSistema=" & vCodSistema & " and ss.Menu=(" & jm  & " * 10) + " & jmi
                        End If
                        vPes=vPes & " UNION "
                        vPes=vPes & "Select IsNull(ss.Menu,0) as Menu,IsNull(ss.NumSecao,0) as NumSecao,p.ProgPesquisa,s.NomProg,s.Imagem,s.ImagemI,IsNull(s.ImgTam,0) as ImgTam,IsNull(s.ImgAlt,0) as ImgAlt,s.DesSecao as DesMenu "
                        If jm=0 Then
                            vPes=vPes & " From ASSMED_Programa p,ASSMED_Secao s,ASSMED_SistemaSecao ss Where p.NomProg=s.NomProg and s.NumSecao=ss.NumSecao and ss.Janela<='' and ss.CodSistema=" & vCodSistema & " and ss.Menu=100 + " & jmi
                        Else
                            vPes=vPes & " From ASSMED_Programa p,ASSMED_Secao s,ASSMED_SistemaSecao ss Where p.NomProg=s.NomProg and s.NumSecao=ss.NumSecao and ss.Janela<='' and ss.CodSistema=" & vCodSistema & " and ss.Menu=(" & jm  & " * 10) + " & jmi
                        End If
                        vPes=vPes & " Order By 1,4 "
            Set bI=objConn.Exec(vPes)
            If Not bi.Eof and NOt bi.Bof Then
    While Not bi.Eof
    If arMenuI > "" Then arMenuI=arMenuI & "|"
                                arMenuI=arMenuI & cInt(bi("NumSecao"))
                                bi.MoveNext
                WEnd
            End If
                        bi.Close
            Set bi=Nothing
            If arMenuI > "" Then
    If arMenuPrin(jm) > "" Then arMenuPrin(jm)=arMenuPrin(jm) & ";"
                            arMenuPrin(jm)=arMenuPrin(jm) & arMenuI
                        End If
    Next
    'If arMenuI > "" Then
    '   If arMenuPrin(jm) > "" Then arMenuPrin(jm)=arMenuPrin(jm) & ";"
    '   arMenuPrin(jm)=arMenuPrin(jm) & arMenuI
    'End If

    Next

                vTextoMenu=""
                For jm=0 to 9
    If arMenuPrin(jm) > "" Then
                        'response.write jm & " " & arMenuPrin(jm) & "<br>"
                        arSubMenu=Split(arMenuPrin(jm),";")
                        If uBound(arSubMenu) > 0 Then
                            'response.write arSubMenu(0) & "<br>"
                            vTextoMenu=vTextoMenu & "<li>
    " & MontaMenuTem(arSubMenu(0),"P1")
                            vTextoMenu=vTextoMenu & "<ul class='nav nav-second-level'>
        "
                            For jms=1 to uBound(arSubMenu)
    'response.write arSubMenu(jms) & "
    <br>"
        arItensMenu=Split(arSubMenu(jms),"|")
        If Ubound(arItensMenu) > 0 Then
        vTextoMenu=vTextoMenu & "
        <li>
            " & MontaMenuTem(arItensMenu(0),"P")
            vTextoMenu=vTextoMenu & "<ul class='nav nav-third-level'>
                "
                For jmi=1 to  Ubound(arItensMenu)
                vTextoMenu=vTextoMenu & MontaMenuTem(arItensMenu(jmi),"U")
                Next
                vTextoMenu=vTextoMenu & "
            </ul>"
            vTextoMenu=vTextoMenu & "
        </li>"
        Else
        vTextoMenu=vTextoMenu & MontaMenuTem(arItensMenu(0),"U")
        End If
        Next
        vTextoMenu=vTextoMenu & "
    </ul>"
    vTextoMenu=vTextoMenu & "
</li>"
            Else
                '---Um Item Só
                vTextoMenu=vTextoMenu & MontaMenuTem(arSubMenu(0),"U")
            End If
        End If
    Next

@End Code