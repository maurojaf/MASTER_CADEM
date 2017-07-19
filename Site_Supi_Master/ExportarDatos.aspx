<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ExportarDatos.aspx.cs" Inherits="ExportarDatos" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
 <title> Exportacion de datos </title>

        <!-- Icono -->
        <link id="Link1" runat="server" rel="Shortcut Icon"  href="Images/icon.ico" type="image/x-icon" />

        <!-- Include Css -->
        <link href='http://fonts.googleapis.com/css?family=Open+Sans:300' rel='stylesheet' type='text/css'/>
        <script type="text/javascript" script-name="actor" src="http://use.edgefonts.net/actor.js"></script>   
        <link href="Css/Label.css" type="text/css" rel="Stylesheet" />
        <link href="Css/Divs.css" type="text/css" rel="Stylesheet" />
        <link href="Css/Button.css" type="text/css" rel="Stylesheet" />
    
       <!-- Include jQuery -->
        <link href="Js/css/ui-lightness/jquery-ui-1.9.2.custom.css" rel="stylesheet">
	    <script src="Js/js/jquery-1.8.3.js"></script>
	    <script src="Js/js/jquery-ui-1.9.2.custom.js"></script>

        <!-- Include Alert -->
        <script src="Js/Widgets/SweetAlert/lib/sweet-alert.min.js"></script> 
        <link rel="stylesheet" type="text/css" href="Js/Widgets/SweetAlert/lib/sweet-alert.css"/>

        <!-- Validaciones -->
        <script src="Js/Validaciones/Validaciones.js"></script>

</head>
<body background="Images/b.png">
    <form id="form1" runat="server">

     <div id="header" class="divmenu" style="margin:0px; margin-top:0px;" > 
        <table style="margin:0px; width:100%" cellpadding="0px" cellspacing="0px">
            <tr>
                <td style="width:15%"> <asp:Image ID="Image2" runat="server" Height="10%" ImageUrl="~/Images/logo-cadem.png" />  </td>
                <td style="width:55%; text-align:center"> </td>
                <td style="width:20%; text-align:right"> <asp:Label ID="lbl_session" runat="server" CssClass="nombresession"></asp:Label></td>
                <td style="width:10%; text-align:right"> <asp:Button ID="btn_session" runat="server" Text="Cerrar sesion" CssClass="close-session"   onclick="btn_session_Click"/> </td>
            </tr>
        </table>
     </div> 
     
      <br /><br /><br />

      <div id="body">   
            <center> 
                <div style="width:60%; height:80%" class="divlogin"> <br />
                    <table width="100%" cellpadding="0"; cellspacing="0">
                        <tr><td style="height:50%"> <div id="titulo" class="neon-text" > <center>EXPORTACION DE DATOS</center>  </div>  </td></tr>   
                        <tr>
                            <td align="center"> 
                                <br />
                                <asp:Button ID="btn_salas" runat="server"  Text="EXPORTAR SALAS" CssClass="button-select-menu" onclick="btn_salas_Click" /> <br />
                                <asp:Button ID="btn_auditores" runat="server" Text="EXPORTAR AUDITORES" CssClass="button-select-menu" onclick="btn_auditores_Click" /><br />
                                <asp:Button ID="btn_estudios" runat="server" Text="EXPORTAR ESTUDIOS"  CssClass="button-select-menu" onclick="btn_estudios_Click" /> <br />
                          <br /><br /><br />

                                <asp:Button ID="btn_volver" runat="server" CssClass="button-select-menu"  Text="VOLVER" onclick="btn_volver_Click" /> <br /><br /><br />   
                             
                            </td>
                        </tr>
                    </table>    
                </div>
            </center>
        </div>
    </form>
</body>
</html>
