<%@ Page Language="VB" AutoEventWireup="false" CodeFile="itemmaster_request.aspx.vb" Inherits="itemmaster_request" %>
{
"draw":<%=vDraw %>,
"recordsTotal":<%=vCtr%>,
"recordsFiltered":<%=vCtr%>,
"data": [<%=vEmpRecords %>]
}