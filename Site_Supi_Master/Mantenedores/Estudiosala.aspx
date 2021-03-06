﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Estudiosala.aspx.cs" Inherits="Mantenedores_Estudiosala" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title> Ingresar Estudiosala </title>

        <!-- Icono -->
        <link id="Link1" runat="server" rel="Shortcut Icon"  href="../Images/icon.ico" type="image/x-icon" />

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

            //Validacion campos no vacios...
            $(function () {
                $('#btn_aceptar').click(function () {

                    var Estado = true;
                    var IsError = 0;

                    //Recorre todos los imput cuyas clases sean 'textbox' o sean cbo_cliente, cbo_estado..

                    $Valor = $("#cbo_estudio").val().trim();
                    if ($Valor === '') {
                        IsError = 1;
//                        alert("fd");
                        $("#cbo_estudio").css('background-color', "#f7eecd");
                    } else {
                        $("#cbo_estudio").css('background-color', "white");
                    }

                    if (IsError > 0) {
                        Estado = false;
                        swal('Campos vacios', 'Primera buscar estudio', 'warning');
                    } else {
//                        $(".loader").show();
                        $(".loader").fadeIn("slow");
                    }
                    return Estado;

                });
            });


      </script> 

</head>
<body background="../Images/b.png" >
    <form id="form1" runat="server">
         <div id="header" class="divmenu" style="margin:0px; margin-top:0px;" > 
         <table style="margin:0px; width:100%" cellpadding="0px" cellspacing="0px">
            <tr>
                <td style="width:15%"> <asp:Image ID="Image1" runat="server" Height="10%" ImageUrl="~/Images/logo-cadem.png" />  </td>
                <td style="width:55%; text-align:left"> </td>
                <td style="width:20%; text-align:right"> <asp:Label ID="lbl_session" runat="server" CssClass="nombresession" onmouseover="Mostrar();"></asp:Label> </td>
                <td style="width:10%; text-align:right"> <asp:Button ID="Button6"  runat="server" Text="Cerrar sesion" CssClass="close-session"  onclick="btn_session_Click" /> </td>
            
            </tr>
        </table>
     </div> 
        
        <div style="height:45px;"></div>    
        <div class="loader"></div> 
        <br />
        <div id="cuerpo" style="width:100%">
            <center>    
                <div style="width:70%; height:250px" class="divcuerpo">                             
                   
                       
                   
                            <br />
                            <table style="width:100%">
                                <tr>
                                    <td> <div style="text-align:right"> <asp:Label ID="Label1" runat="server" class="neon-text" Text="Asociar folio a estudio"  ></asp:Label> </div></td>
                                    <td align="center">  
                                        <asp:Button ID="btn_volver" runat="server" Text="VOLVER"   CssClass="button-enjoy" onclick="btn_tamano_Click"  Width="121px" Height="20px"  />  </td>
                                </tr>         
                            </table>
                         
                            <br />  
                            <table width="50%" cellspacing="0">  
                                <tr>
                                    <td style="width:5%;height:10px"> <div id="estudio0"> <div class="textonaranjo"> <asp:Label ID="lbl_estudio" runat="server" Text="Folio" ></asp:Label></div> </div></td>
                                    <td style="width:35%;height:10px"> <div id="estudio1"> <asp:TextBox ID="txt_folio" 
                                            placeholder="" runat="server" class="textbox" TabIndex="2" 
                                            onkeydown="return (event.keyCode!=13);" autocomplete="off"  
                                            onChange="PintaSiVacio(this, this.value);" Width="86%"></asp:TextBox> </div></td> 
                                </tr>                                   
                                <tr>
                                    <td style="width:5%;height:25px"> <div class="textonaranjo">Estudio </div> </td>
                                    <td style="width:35%;height:25px"> <asp:DropDownList ID="cbo_estudio"  runat="server" class="combobox" TabIndex="4"  onChange="PintaSiVacio(this, this.value);" Width="90%">  </asp:DropDownList>   </td> 
                                </tr>
                            </table>
                            <br />
                            <table>
                                <tr>  
                                    <td align="center" colspan="2">  
                                        <asp:Button ID="btn_aceptar" runat="server" Text="Asociar" CssClass="button-enjoy" onclick="btn_aceptar_Click" TabIndex="7"  onclientclick="return Validacion4() ;" Width="60px"/> 
                                        <asp:Button ID="btn_cancelar" runat="server" Text="Cancelar" CssClass="button-red-enjoy" CausesValidation="False" TabIndex="8"   onclick="btn_cancelar_Click" Width="60px" />
                                    </td>
                                </tr>      
                            </table>
                            <br />
                            
                        
                                
                </div>
            </center>

        </div>
    </form>
</body>
</html>