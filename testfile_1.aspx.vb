Imports denaro
Partial Class testfile_1
    Inherits System.Web.UI.Page
    Public vData As String = ""
    Public vScript As String = ""
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
     
    End Sub
 
    
    Protected Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click


        Dim c As New SqlClient.SqlConnection(connStr)
        Dim cm As New SqlClient.SqlCommand
        Dim rs As SqlClient.SqlDataReader
        Dim vPercent As String = ""
		Dim vItemSFG As String = ""
		Dim vSQLUpdate As String = ""
		Dim vSQLInsert As String = ""
		Dim vIsUpdate As Decimal = 0
		Dim vIsInsert As Decimal = 0
		Dim vDimension As String = ""
		Dim vIsRW As String = ""
		Dim vDimVal As String()
		Dim vWidth As String() = {0, 0}
		Dim vLenght As String() = {0, 0}
		Dim vWidthVal As String = ""
		Dim vLenghtVal As String = ""
		Dim vItemType As String = ""
		Dim vItemTypeVal As String = ""


		Try
            c.Open()
        Catch ex As SqlClient.SqlException
            vScript = "alert('Error occurred while trying to connect to database. Error is: " & _
                ex.Message.Replace(vbCrLf, "").Replace("'", "") & "');"
            c.Dispose()
            cm.Dispose()
            Exit Sub
        End Try

        cm.Connection = c

		cm.CommandText = "select *,(select count(item_cd) from item_master where item_Cd=itemCd) as vCtr from item_temp"
		'Response.Write(cm.CommandText)
		Try
            rs = cm.ExecuteReader

            Do While rs.Read
				vDimension = rs(5).ToString.Replace("RW (", "RW=(")

				If vDimension <> "" Then

					vIsRW = vDimension.ToString.Substring(0, 2)


					If vIsRW = "RW" Then
						vDimVal = vDimension.ToString.Trim.Split("X")


						vWidth = vDimVal(0).ToString.Trim.Split("=")


						Try
							vWidthVal = vWidth(1).Replace("""", "")
						Catch ex As Exception

						End Try

						Try
							vLenght = vDimVal(1).ToString.Trim.Split("=")
							vLenghtVal = vLenght(1)
						Catch ex As Exception

						End Try

						'If vDimVal(1).Length > 0 Then
						'	vLenght = vDimVal(1).ToString.Trim.Split("=")
						'End If

					End If

					'If vItemType <> "" Then
					vItemType = rs(1).ToString.Substring(0, 3)
						Select Case vItemType
							Case "MPR"
								vItemTypeVal = "RM-RESIN(IMP)"
							Case "MPF"
								vItemTypeVal = "RM-FILM(IMP)"
							Case "FLS"
								vItemTypeVal = "RM-SOLVENT"
						Case "AWC", "AWB", "AWD", "AWI", "AWH", "AWN", "AWR", "AWS", "AWT"
							vItemTypeVal = "FG"
							Case "FLI"
								vItemTypeVal = "RM-INK"
							Case "FLA"
								vItemTypeVal = "RM-ADHESIVE"
							Case Else
								vItemTypeVal = "RW"
						End Select
					'Else
					'	vItemTypeVal = "RW"
					'End If


				End If
				'vData += "<tr>" _
				'	& "<td>" & rs(0) & "</td>" _
				'	& "<td>" & rs(1) & "</td>" _
				'	& "<td>" & rs(2) & "</td>" _
				'	& "<td>" & rs(3) & "</td>" _
				'	& "<td>" & rs(4) & "</td>" _
				'	& "<td>" & vDimension & "</td>" _
				'	& "<td>" & rs(6) & "</td>" _
				'	& "<td>" & rs(7) & "</td>" _
				'	& "<td>" & rs(8) & "</td>" _
				'	& "<td>" & rs(9) & "</td>"

				If rs(9) = 1 Then
					vIsUpdate += 1
					vSQLUpdate += "update item_master set Descr='" & rs(2).ToString.Replace("'", " ") & "', " _
						& "RollWidth='" & vWidthVal.Trim & "'," _
						& "RepeatLenght='" & vLenghtVal.Trim & "'," _
						& "BagDimension='" & rs(5).ToString.Replace("""", "") & "'," _
						& "MaterialSpecs='" & rs(4) & "'," _
						& "QtyUOM_Cd='" & rs(8).ToString.Replace(".", "") & "'," _
						& "WeightUOM_Cd='" & rs(8).ToString.Replace(".", "") & "'," _
						& "WsUom_Cd='" & rs(8).ToString.Replace(".", "") & "'," _
						& "RtlUom_Cd='" & rs(8).ToString.Replace(".", "") & "'," _
						& "MinOrderQtyUOM_Cd='" & rs(8).ToString.Replace(".", "") & "' where item_cd='" & rs(1) & "'<br>"
				Else
					vIsInsert += 1
					vSQLInsert += "insert into item_master " _
						& "(item_cd,Descr,RollWidth,RepeatLenght,BagDimension,MaterialSpecs,QtyUOM_Cd,WeightUOM_Cd,WsUom_Cd,RtlUom_Cd,MinOrderQtyUOM_Cd,ItemType_Cd) values (" _
						& "'" & rs(1) & "','" & rs(2).ToString.Replace("'", " ") & "','" & vWidthVal & "','" & vLenghtVal & "','" & rs(5).ToString.Replace("""", "") & "','" & rs(4) & "'," _
						& "'" & rs(8).ToString.Replace(".", "") & "','" & rs(8).ToString.Replace(".", "") & "','" & rs(8).ToString.Replace(".", "") & "'," _
						& "'" & rs(8).ToString.Replace(".", "") & "','" & rs(8).ToString.Replace(".", "") & "','" & vItemTypeVal & "')<br>"
				End If

				vData += "<td></td></tr>"

				vDimension = ""
				vWidthVal = ""
				vLenghtVal = ""
			Loop

			Response.Write("Inser:" & vIsInsert & " <br>Update:" & vIsUpdate & "<br><br>" & vSQLInsert & vSQLUpdate)


			rs.Close()
        Catch ex As SqlClient.SqlException
            vScript = "alert('Error occurred while trying to buil shift reference. Error is: " & _
                ex.Message.Replace(vbCrLf, "").Replace("'", "") & "');"
        End Try

        c.Close()
        cm.Dispose()
        c.Dispose()
    End Sub
End Class
