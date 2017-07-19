<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Logistica.aspx.cs" Inherits="MasterSupi_Logistica" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title> Maestro Logistica </title>

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

        //Validacion campos no vacios...
            $(function () {
                $('#btn_aceptar').click(function () {
                    var Estado = true;
                    var IsError = 0;
                    //Recorre todos los imput cuyas clases sean 'textbox' o 'combobox'..
                    $('.textbox, .combobox').each(function () {
                        $Valor = $(this).val().trim();
                        if ($Valor === '') {
                            IsError++;
                            $(this).css('background-color', "#f7eecd");
                        } else {
                            $(this).css('background-color', "white");
                        }
                    });

                    if (IsError > 0) {
                        Estado = false;
                        swal('Campos vacios', 'No deben de haber campos vacios', 'warning');
//                        $(".loader").hide();
                    } else {
                        $(".loader").fadeIn("slow");
//                        $(".loader").show();
                    }
                    return Estado;

                });
            });

        //Limpia todos los campos...
        $(function () {
            $('#btn_cancelar').click(function () {

                //Limpia y pintan en blanco todos los imput cuyas clases sean 'textbox' o 'combobox'..
                $('.textbox, .combobox').each(function () {
                    $(this).val("");
                    $(this).css('background-color', 'white');
                });

                //Habilita campo 'cbo inicio y termino' ...
                $("#cbo_comuna_inicio").val("");
                $("#cbo_comuna_inicio").attr('disabled', false);
                $("#cbo_comuna_inicio").addClass("combobox");

                $("#cbo_comuna_termino").val("");
                $("#cbo_comuna_termino").attr('disabled', false);
                $("#cbo_comuna_termino").addClass("combobox");

                //Foco a cbo inicio...
                $("#cbo_comuna_inicio").focus();

                //Renombrar boton aceptar...
                $("#btn_aceptar").prop('value', 'Agregar');

                //Renombrar label disponibilidad...
                $("#lbl_disponible").text('');

                return false;
            });
        });

        $(function () {
            $('#btn_go_emepleados, #btn_go_salas, #btn_go_estudio, #btn_go_menu').click(function () {
                $(".loader").fadeIn("slow");
//                $(".loader").show();
            });
        });

        function stopPostback() 
        {
            $Inicio = $('#cbo_comuna_inicio').val().trim();
            $Termino = $('#cbo_comuna_termino').val().trim();

            if ($Inicio === '0') {
                return true; // stop postback
            }
            else if ($Termino === '0') {
                return true; // stop postback
            }
            else {
                $(".loader").fadeIn("slow");
//                $(".loader").show();
                return false;
            }
        }

    </script> 

</head>

<body background="Images/b.png" >
    <form id="form1" runat="server">

    <div id="header" class="divmenu" style="margin:0px; margin-top:0px;" > 
         <table style="margin:0px; width:100%" cellpadding="0px" cellspacing="0px">
            <tr>
                <td style="width:15%"> <asp:Image ID="Image1" runat="server" Height="10%" ImageUrl="~/Images/logo-cadem.png" />  </td>
                <td style="width:55%; text-align:left">
                    <asp:Button ID="btn_go_emepleados" runat="server" Text="Empleados"  CssClass="select-menu" CausesValidation="False"  onclick="btn_go_emepleados_Click"/> 
                    <asp:Button ID="btn_go_salas" runat="server" Text="Salas"  CssClass="select-menu" CausesValidation="False" onclick="btn_go_salas_Click" /> 
                    <asp:Button ID="btn_go_logistica" runat="server" Text="Logistica"   
                        CssClass="select-menu-activo" CausesValidation="False" 
                        onclientclick="return false" onclick="btn_go_logistica_Click" />  
                    <asp:Button ID="btn_go_estudio" runat="server" Text="Estudios"  CssClass="select-menu" CausesValidation="False" onclick="btn_go_estudio_Click"/> 
                    <asp:Button ID="btn_go_menu" runat="server" Text="Menu General"  CssClass="select-menu" CausesValidation="False"  onclick="btn_go_menu_Click"/>          
                </td>
                <td style="width:20%; text-align:right"> <asp:Label ID="lbl_session" runat="server" CssClass="nombresession"></asp:Label> </td>
                <td style="width:10%; text-align:right"> <asp:Button ID="Button6"  runat="server" Text="Cerrar sesion" CssClass="close-session"  onclick="btn_session_Click" OnClientClick="return Confirmar();" /> </td>
            
            </tr>
        </table>
     </div> 
      <br />
        <div style="height:45px;"></div>    
        <div class="loader"></div> 

        <div id="cuerpo" style="width:100%">
            <center>           
                <div style="width:70%; height:90%" class="divcuerpo">                              
                   
                                
                            <br />
                            <asp:Label ID="Label1" runat="server" class="neon-text" Text="Ingresar o Actualizar Trayectos"  ></asp:Label>
                            <br /> <br />
                                <center>
                                    <table width="40%" cellspacing="0">
                                        <tr>
                                            <td> <div class="textonaranjo" style="text-align:left"> Comuna Inicio </div> </td>
                                            <td> <div class="textonaranjo" style="text-align:left"> Comuna destino </div> </td>
                                        </tr>
                                        <tr> 
                                            <td style="height:28px; width:50%" align="left">  <asp:DropDownList ID="cbo_comuna_inicio" runat="server" class="combobox"  Width="170px" TabIndex="1" onsubmit="return BusarTrayecto();"  ontextchanged="cbo_buscar" onChange="if (stopPostback()) return false;"  AutoPostBack="True">  </asp:DropDownList> </td>   
                                            <td style="height:25px; width:50%" align="left">  <asp:DropDownList ID="cbo_comuna_termino" runat="server" class="combobox"   Width="170px" TabIndex="2" AutoPostBack="True"  onsubmit="return BusarTrayecto();" ontextchanged="cbo_buscar"  onChange="if (stopPostback()) return false;">  </asp:DropDownList>  </td>           
                                        </tr>  
                                        <tr><td colspan="2" align="center" valign="top" style="height:20px">   <asp:Label ID="lbl_disponible" runat="server" Text=" " ForeColor="#009933" Font-Size="Small" Font-Names="Arial Narrow"></asp:Label></td></tr>
                                    </table>  
                                                                                
                                    <table width="40%" cellspacing="0">  
                                        <tr>
                                            <td class="style1">  <div class="textonaranjo">Costo trayecto </div> </td>    
                                            <td style="height:30px">  <asp:TextBox ID="txt_costo" runat="server" class="textbox" TabIndex="4" onkeydown="return (event.keyCode!=13);" onChange="validarSiNumero(this, this.value);" autocomplete="off"></asp:TextBox>  </td>   
                                        </tr>        
                                        <tr> 
                                            <td> <div class="textonaranjo">Tipo trayecto </div> </td> 
                                            <td style="height:30px">  <asp:DropDownList ID="cbo_urbano" runat="server" class="combobox" TabIndex="5" onChange="PintaSiVacio(this, this.value);">  </asp:DropDownList>  </td>  
                                        </tr>    
                                        <tr> 
                                            <td><div class="textonaranjo"> Tiempo trayecto</div></td>           
                                            <td style="height:30px"> 
                                                <table width="90%">
                                                    <tr><td style="width:10%">  <div class="textomantenedores"> Hora</div> </td>
                                                        <td style="width:40%"> <asp:DropDownList ID="cbo_horas" runat="server" class="combobox" Width="55px"   TabIndex="6" onChange="PintaSiVacio(this, this.value);">  </asp:DropDownList> </td>
                                                        <td style="width:10%"> <div class="textomantenedores">  Min</div> </td>
                                                        <td>  <asp:DropDownList ID="cbo_minutos" runat="server" class="combobox" Width="55px"   TabIndex="7" onChange="PintaSiVacio(this, this.value);"> </asp:DropDownList> </td>
                                                    </tr>
                                                </table>
                                            </td>  
                                        </tr>
                                        <tr> <td colspan="2" style="height:15px""> &nbsp;</td> </tr>
                                        <tr> 
                                            <td colspan="2" align="center"> 
                                                <asp:Button ID="btn_aceptar" runat="server" Text="Agregar"   CssClass="button-enjoy" onclick="btn_aceptar_Click" TabIndex="8"   onclientclick="return Validacion3();"/>  
                                                <asp:Button ID="btn_cancelar" runat="server" Text="Cancelar"   CssClass="button-red-enjoy"    CausesValidation="False" TabIndex="9" />
                                                <asp:Button ID="btn_exportar" runat="server" Text="Exportar"    CssClass="button-enjoy" TabIndex="8"   onclientclick="return Validacion3();"  onclick="btn_exportar_Click"/>  
                                                
                                            </td>
                                        </tr>
                                        <tr><td colspan="2" style="height:15px""></td> </tr>
                                    </table>
                                </center>
                            <br />   
                        
                                   
                </div>
            </center>
        </div>      
    </form>
</body>
</html>
