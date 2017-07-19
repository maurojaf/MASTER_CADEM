<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Menu.aspx.cs" Inherits="MasterSupi_Menu" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title> Menu General </title>

        <!-- Icono -->
        <link runat="server" rel="Shortcut Icon"  href="Images/icon.ico" type="image/x-icon" />

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

        <script>

        //facous pagina (sin necesidad de cargar)...
        $(document).ready(function () {
            $(".loader").fadeIn("slow");
        });

        //Pagna carga por completo ...
        $(window).load(function () {
            $(".loader").fadeOut();
        });

        $(function () {
            $('#btn_empleado, #btn_salas, #btn_logistica, #btn_estudios').click(function () {
//                $(".loader").show();
                $(".loader").fadeIn("slow");
            });
        });

      </script>

</head>
<body background="Images/b.png">
    <form id="form1" runat="server">

     <div id="header" class="divmenu" style="margin:0px; margin-top:0px;" > 
        <table style="margin:0px; width:100%" cellpadding="0px" cellspacing="0px">
            <tr>
                <td style="width:15%"> <asp:Image ID="Image2" runat="server" Height="10%" ImageUrl="~/Images/logo-cadem.png" />  </td>
                <td style="width:55%; text-align:center"> </td>
                <td style="width:20%; text-align:right"> <asp:Label ID="lbl_session" runat="server" CssClass="nombresession"></asp:Label></td>
                <td style="width:10%; text-align:right"> <asp:Button ID="Button1" runat="server" Text="Cerrar sesion" CssClass="close-session" onclick="btn_session_Click"/> </td>
            </tr>
        </table>
     </div> 
     
     

      <div class="loader"></div> 
       <br /><br /><br />
      <div id="body">   
            <center> 
            <div style="width:70%; height:80%" class="divlogin"> <br />
                <table width="100%" cellpadding="0"; cellspacing="0">
                    <tr><td style="height:50%"> <div id="titulo" class="neon-text" > <center>OPCIONES</center>  </div>  </td></tr>   
                    <tr>
                        <td align="center"> 
                            <asp:Button ID="btn_empleado" runat="server" Text="EMPLEADOS" CssClass="button-select-menu" onclick="btn_empleado_Click"  /> <br />
                            <asp:Button ID="btn_salas" runat="server" Text="SALAS" CssClass="button-select-menu" onclick="btn_salas_Click"/> <br />
                            <asp:Button ID="btn_logistica" runat="server" Text="LOGISTICA" CssClass="button-select-menu" onclick="btn_logistica_Click" /> <br />
                            <asp:Button ID="btn_estudios" runat="server" Text="ESTUDIOS" CssClass="button-select-menu" onclick="btn_estudios_Click"/> <br />
                            <asp:Button ID="btn_galeria" runat="server" Text="GALERIA" CssClass="button-select-menu" onclick="btn_galeria_Click"/><br />
                            <asp:Button ID="btn_launcher" runat="server" Text="LAUNCHER"  CssClass="button-select-menu" onclick="btn_launcher_Click" /><br />
                            <asp:Button ID="btn_quiz" runat="server" Text="QUIZ"   CssClass="button-select-menu" onclick="btn_quiz_Click"  /><br />
                            <asp:Button ID="btn_auditoria" runat="server" Text="AUDITORIAS"  CssClass="button-select-menu" onclick="btn_auditoria_Click"  /><br />
                            <asp:Button ID="btn_exportar" runat="server" Text="EXPORTAR DATOS"  CssClass="button-select-menu" onclick="btn_exportar_Click"/><br />
                            <asp:Button ID="btn_submenu" runat="server" Text="SUB MENU"  CssClass="button-select-menu" onclick="btn_submenu_Click" /><br />
                            <asp:Button ID="btn_prioridades" runat="server" Text="PRIORIDADES"   CssClass="button-select-menu" onclick="btn_prioridades_Click"  /><br />
                            <asp:Button ID="btn_trayecto" runat="server" Text="TRAYECTO" CssClass="button-select-menu" onclick="btn_trayecto_Click" Visible="true" /><br />
                            <asp:Button ID="btn_inicio_medicion" runat="server" 
                                Text="FOTOS INICIO MEDICION" CssClass="button-select-menu" Visible="true" 
                                onclick="btn_inicio_medicion_Click" /><br />
                            
                            <br />  
                        </td>
                    </tr>
                </table>    
            </div>
            </center>
        </div>
    </form>
</body>
</html>
