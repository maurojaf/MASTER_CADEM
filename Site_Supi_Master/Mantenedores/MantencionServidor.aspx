<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MantencionServidor.aspx.cs" Inherits="Mantenedores_MantencionServidor" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
     <title> Tamanos </title>
           
        <!-- Icono -->
        <link id="Link1" runat="server" rel="Shortcut Icon"  href="../../Images/icon.ico" type="image/x-icon" />

        <!-- Include Css -->
        <link href='http://fonts.googleapis.com/css?family=Open+Sans:300' rel='stylesheet' type='text/css'/>
        <script type="text/javascript" script-name="actor" src="http://use.edgefonts.net/actor.js"></script>            
        <link href="../Css/TextBox.css" type="text/css" rel="Stylesheet" />
        <link href="../Css/Label.css" type="text/css" rel="Stylesheet" />
        <link href="../Css/Divs.css" type="text/css" rel="Stylesheet" />
        <link href="../Css/Button.css" type="text/css" rel="Stylesheet" />
        <link href="../Css/Menu.css" type="text/css" rel="Stylesheet" />
        <link href="../Css/Tooltip.css" type="text/css" rel="Stylesheet" />

        <!-- Include Alert -->
        <script src="../Js/Widgets/SweetAlert/lib/sweet-alert.min.js"></script> 
        <link rel="stylesheet" type="text/css" href="../Js/Widgets/SweetAlert/lib/sweet-alert.css"/>

        <!-- Include jQuery -->
        <link href="../Js/css/ui-lightness/jquery-ui-1.9.2.custom.css" rel="stylesheet">
	    <script src="../Js/js/jquery-1.8.3.js"></script>
	    <script src="../Js/js/jquery-ui-1.9.2.custom.js"></script>

        <!-- Validaciones -->
        <script src="../Js/Validaciones/Validaciones.js"></script>

        <script type="text/javascript">

            //facous pagina (sin necesidad de cargar)...
            $(document).ready(function () {
                $(".loader").fadeIn("slow");
            });

            //Pagna carga por completo ...
            $(window).load(function () {
                $(".loader").fadeOut();
            });

    </script>

</head>
<body background="../Images/b.png" style="margin: 0px, 0px, 0px, 0px">
    <form id="form1" runat="server">
       
     <div class="loader"></div>           
     <div id="cuerpo" style="width:100%;">
            <center>
             <br />  <br />  <br /> 
                <div style="width:70%; height:450px" class="divcuerpo">                                   
                      <br /> <br /> 
                     <table style="width:70%">
                          <tr><td> <div style="text-align:center"> <asp:Label ID="lbl_nombre" runat="server" class="neon-text" Text="MANTENCION DEL SERVIDOR"></asp:Label> </div> </td></tr>         
                     </table>              
                     <br /> 
                     <table cellspacing="0" width="70%">                                          
                          <tr><td> <div class="textonaranjo" style="text-align:center"> LOS SENTIMOS EL SERVIDOR ESTA TEMPORALMENTE FUERA DE SERVICIO, EN UNOS MOMENTOS ESTARA DISPONIBLE  </td> </tr>                 
                     </table>
                     <br /> 
                     <table cellspacing="0" width="70%">                                          
                          <tr><td align="center">   <asp:Image ID="IMG_OFF" runat="server" Height="221px"  ImageUrl="~/Images/off.png" /></td> </tr>                 
                     </table>
                </div>
            </center>
        </div>
    </form>
</body>
</html>
