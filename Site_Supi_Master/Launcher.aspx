<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Launcher.aspx.cs" Inherits="Launcher" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Exportar Launcher</title>
 <!-- Icono -->
        <link id="Link1" runat="server" rel="Shortcut Icon"  href="../Images/icon.ico" type="image/x-icon" />

        <!-- Include Css -->
        <link href='http://fonts.googleapis.com/css?family=Open+Sans:300' rel='stylesheet' type='text/css'/>
        <script type="text/javascript" script-name="actor" src="http://use.edgefonts.net/actor.js"></script>   
        <link href="Css/TextBox.css" type="text/css" rel="Stylesheet" />
        <link href="Css/Label.css" type="text/css" rel="Stylesheet" />
        <link href="Css/Divs.css" type="text/css" rel="Stylesheet" />
        <link href="Css/Button.css" type="text/css" rel="Stylesheet" />
        <link href="Css/Menu.css" type="text/css" rel="Stylesheet" />
        
        <!-- Include Alert -->
        <script src="Js/Widgets/SweetAlert/lib/sweet-alert.min.js"></script> 
        <link rel="stylesheet" type="text/css" href="Js/Widgets/SweetAlert/lib/sweet-alert.css"/> 
        
        <!-- Include jQuery -->
        <link href="Js/css/ui-lightness/jquery-ui-1.9.2.custom.css" rel="stylesheet">
	    <script src="Js/js/jquery-1.8.3.js"></script>
	    <script src="Js/js/jquery-ui-1.9.2.custom.js"></script>

        <!-- Validaciones -->
        <script src="Js/Validaciones/Validaciones.js"></script>

        <script type="text/javascript">

            //facous pagina (sin necesidad de cargar)...
            $(document).ready(function () {
                $(".loader").fadeIn("slow");
            });

            //Pagna carga por completo ...
            $(window).load(function () {
                $(".loader").fadeOut();
            });

           
           
            $(function () {
                $('#btn_go_emepleados, #btn_go_salas, #btn_go_estudio, #btn_go_menu').click(function () {
                    $(".loader").fadeIn("slow");
                    //                $(".loader").show();
                });
            });


            //Calendario...
            $(function () {
                $("#txt_fecha").datepicker({
                    renderer: $.ui.datepicker.defaultRenderer,
                    dateFormat: 'yy-mm-dd',
                    monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                    monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic'],
                    dayNames: ['Domingo', 'Lunes', 'Martes', 'Miercoles', 'Jueves', 'Viernes', 'Sabado'],
                    dayNamesShort: ['Dom', 'Lun', 'Mar', 'Mie', 'Juv', 'Vie', 'Sab'],
                    dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
                    firstDay: 1,
                    autoSize: false,
                    beforeShow: function () {
                        $(".ui-datepicker").css('font-size', 12)
                    }
                });
            });


    </script> 

</head>

<body background="Images/b.png" >
    <form id="form1" runat="server">

    <div id="header" class="divmenu" style="margin:0px; margin-top:0px;" > 
         <table style="margin:0px; width:100%" cellpadding="0px" cellspacing="0px">
            <tr>
                <td style="width:15%"> <asp:Image ID="Image1" runat="server" Height="10%" ImageUrl="~/Images/logo-cadem.png" />  </td>
                <td style="width:55%; text-align:left">
                   
                </td>
                <td style="width:20%; text-align:right"> <asp:Label ID="lbl_session" runat="server" CssClass="nombresession"></asp:Label> </td>
                <td style="width:10%; text-align:right"> <asp:Button ID="Button6"  runat="server" Text="Cerrar sesion" CssClass="close-session"  onclick="btn_session_Click" OnClientClick="return Confirmar();" /> </td>
            
            </tr>
        </table>
     </div> 
      
        <div style="height:45px;"></div>    
        <div class="loader"></div> 
        <br />
        <div id="cuerpo" style="width:100%">
            <center>   
                <div style="width:70%; height:500px" class="divcuerpo">                                        
                    <br />
                    <asp:Label ID="Label1" runat="server" class="neon-text" Text="Exportar datos Launcher"  ></asp:Label>
                    <br /> <br />
                    <center>             
                        <table width="50%" cellspacing="0">  
                            <tr>
                                <td class="style1">  <div class="textonaranjo">Fecha</div> </td>    
                                <td style="height:30px">  <asp:TextBox ID="txt_fecha" placeholder="" runat="server" class="textbox"  TabIndex="7" onkeydown = "return (event.keyCode!=13)" autocomplete="off"></asp:TextBox>    </td>   
                            </tr>    
                            <tr>
                                <td class="style1">  <div class="textonaranjo"></div> </td>    
                                <td style="height:30px"> 
                                  <div class="textomantenedores" style="text-align:left"> 
                                      <asp:CheckBox ID="chk_3" runat="server" Text="3 o MAS EANS CON ANCHO" 
                                          Font-Bold="True" Font-Overline="False" /> </div> </td>   
                            </tr>    
                            <tr> 
                                <td valign="top"> <div class="textonaranjo" >Categorias</div> </td> 
                                <td style="height:30px">  
                                    <%-- <asp:DropDownList ID="cbo_estudio1" runat="server" class="combobox" TabIndex="5">  </asp:DropDownList>  --%>
                                    <asp:ListBox ID="cbo_estudio" runat="server" SelectionMode="Multiple" Rows="15" style="margin-left: 5px;"></asp:ListBox>
                                </td>  
                            </tr>    
                                       
                            <tr> <td colspan="2" style="height:15px""> <div class="textonaranjo" >Para saleccionar 2 o mas categorias presiona tecla CTRL</div></td> </tr>
                            <tr><td colspan="2" align="center">  <asp:Button ID="btn_exportar_launcher"  runat="server" Text="Exportar"    CssClass="button-enjoy" TabIndex="8"  onclick="btn_exportar_launcher_Click"  /> 
                                <asp:Button ID="btn_volver"  runat="server" Text="VOLVER"    CssClass="button-enjoy" TabIndex="8"  onclick="btn_volver_Click"   /> </td> 
                            </tr>
                                        
                        </table>
                    </center>
                    <br />   
                        
                                  
                </div>
            </center>
        </div>      
    </form>
</body>
</html>
