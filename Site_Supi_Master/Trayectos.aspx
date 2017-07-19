<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Trayectos.aspx.cs" Inherits="Trayectos" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Trayectos</title>
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
                    } else {

                        $valor = $('#txt_dist_google').val().trim();
                        $reg = /^-?[0-9]+([,\.][0-9]*)?$/;
                        if (!$reg.test($valor)) {
                            swal('Valor no permitido', 'Solo se permiten valores numericos', 'error');
                            $('#txt_dist_google').focus();
                            Estado = false;
                        } else {
                            $(".loader").fadeIn("slow");
                        
                        }  
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
                    $("#txt_folio_ini").val("");
                    $("#txt_folio_ini").attr('disabled', false);
                    $("#txt_folio_ini").addClass("textbox");
                    $("#txt_folio_fin").val("");
                    $("#txt_folio_fin").attr('disabled', false);
                    $("#txt_folio_fin").addClass("textbox");

                    //Foco a cbo inicio...
                    $("#txt_folio_ini").focus();

                    //Renombrar boton aceptar...
                    $("#btn_aceptar").prop('value', 'Agregar');

                    //oculta label dinponibilidad...
                    $("#lbl_visible").attr('visible', false);

                    return false;
                });
            });

            $(function () {
                $('#btn_go_emepleados, #btn_go_salas, #btn_go_estudio, #btn_go_menu').click(function () {
                    $(".loader").fadeIn("slow");
                });
            });

            function stopPostback() {

                $Inicio = $('#txt_folio_ini').val().trim();
                $Termino = $('#txt_folio_fin').val().trim();

                if ($Inicio === '') {
                    swal('Ingresar Folios', 'Debes ingresar folio destino y folio termino', 'error');
                    return true; // stop postback
                }
                else if ($Termino === '') {
                    swal('Ingresar Folios', 'Debes ingresar folio destino y folio termino', 'error');
                    return true; // stop postback
                }
                else {
                    $(".loader").fadeIn("slow");
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
                    <asp:Button ID="btn_go_logistica" runat="server" Text="Logistica"  CssClass="select-menu" CausesValidation="False"   onclick="btn_go_logistica_Click" />  
                    <asp:Button ID="btn_go_estudio" runat="server" Text="Estudios"  CssClass="select-menu" CausesValidation="False" onclick="btn_go_estudio_Click"/> 
                    <asp:Button ID="btn_go_menu" runat="server" Text="Menu General"  CssClass="select-menu" CausesValidation="False"  onclick="btn_go_menu_Click"/>          
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
                <div style="width:75%; height:90%" class="divcuerpo">                              
                            <br />
                            <asp:Label ID="Label1" runat="server" class="neon-text" Text="Ingresar o Actualizar Trayectos"  ></asp:Label>
                            <br /> <br />
                                <center>
                                    <table width="30%" cellspacing="0">
                                        <tr>
                                            <td> <div class="textonaranjo" style="text-align:left"> Folio Inicio </div> </td>
                                            <td> <div class="textonaranjo" style="text-align:left"> Folio destino </div> </td>
                                            <td> </td>
                                        </tr>
                                        <tr> 
                                            <td style="height:28px; width:40%" align="left">  <asp:TextBox ID="txt_folio_ini" runat="server" class="textbox" TabIndex="1" onkeydown="return (event.keyCode!=13);" onChange="validarSiNumero(this, this.value);" ></asp:TextBox>  </td>   
                                            <td style="height:25px; width:40%" align="left">  <asp:TextBox ID="txt_folio_fin" runat="server" class="textbox" TabIndex="2" onkeydown="return (event.keyCode!=13);" onChange="validarSiNumero(this, this.value);" ></asp:TextBox>  </td>           
                                            <td>  
                                                <div style="float:left"> 
                                                    <asp:Button ID="btn_search" runat="server" Text="" CssClass="button-search" CausesValidation="False" onclick="script1"  OnClientClick="if (stopPostback()) return false;"   UseSubmitBehavior="False"   Height="25px" TabIndex="2" />
                                                </div>
                                            </td> 
                                        </tr>  
                                        <tr><td colspan="4" align="center" valign="top" style="height:20px">   <asp:Label ID="lbl_disponible" runat="server" Text="Trayecto no encontrado, disponible para ingresar" ForeColor="#009933" Font-Size="Small" Font-Names="Arial Narrow" Visible="False"></asp:Label></td></tr>
                                    </table>  
                                                                                
                                    <table width="60%" cellspacing="0">  
                                        <tr>
                                            <td class="style1">  <div class="textonaranjo">Distancia Google en KM</div> </td>    
                                            <td style="height:30px">  <asp:TextBox ID="txt_dist_google" runat="server" class="textbox" TabIndex="3"  autocomplete="off"></asp:TextBox>  </td>   
                                        </tr>        
                                        <tr> 
                                            <td> <div class="textonaranjo">Cluster</div> </td> 
                                            <td style="height:30px">  <asp:DropDownList ID="cbo_cluster" runat="server" class="combobox" TabIndex="4" onChange="PintaSiVacio(this, this.value);">  </asp:DropDownList>  </td>  
                                        </tr>    
                                        <tr> 
                                            <td><div class="textonaranjo"> Tiempo Google</div></td>           
                                            <td style="height:30px"> 
                                                <table width="80%">
                                                    <tr><td style="width:10%">  <div class="textomantenedores"> Hora</div> </td>
                                                        <td style="width:40%"> 
                                                            <asp:DropDownList ID="cbo_horas_google" runat="server" class="combobox" Width="55px"   TabIndex="6" onChange="PintaSiVacio(this, this.value);">
                                                                <asp:ListItem>00</asp:ListItem>
                                                                <asp:ListItem>01</asp:ListItem>
                                                                <asp:ListItem>03</asp:ListItem>
                                                                <asp:ListItem>04</asp:ListItem>
                                                                <asp:ListItem>05</asp:ListItem>
                                                                <asp:ListItem>06</asp:ListItem>
                                                                <asp:ListItem>07</asp:ListItem>
                                                                <asp:ListItem>08</asp:ListItem>
                                                            </asp:DropDownList> 
                                                        </td>
                                                        <td style="width:10%"> <div class="textomantenedores">  Min</div> </td>
                                                        <td>  <asp:DropDownList ID="cbo_minutos_google" runat="server" class="combobox" Width="55px"   TabIndex="7" onChange="PintaSiVacio(this, this.value);"> </asp:DropDownList> </td>
                                                    </tr>
                                                </table>
                                            </td>  
                                        </tr>

                                        <tr>
                                            <td><div class="textonaranjo"> Tiempo SUPI</div></td>           
                                            <td style="height:30px"> 
                                                <table width="80%">
                                                    <tr><td style="width:10%">  <div class="textomantenedores"> Hora</div> </td>
                                                        <td style="width:40%"> 
                                                            <asp:DropDownList ID="cbo_horas_supi" runat="server" class="combobox" Width="55px"   TabIndex="8" onChange="PintaSiVacio(this, this.value);">  
                                                                <asp:ListItem>00</asp:ListItem>
                                                                <asp:ListItem>01</asp:ListItem>
                                                                <asp:ListItem>03</asp:ListItem>
                                                                <asp:ListItem>04</asp:ListItem>
                                                                <asp:ListItem>05</asp:ListItem>
                                                                <asp:ListItem>06</asp:ListItem>
                                                                <asp:ListItem>07</asp:ListItem>
                                                                <asp:ListItem>08</asp:ListItem>
                                                            </asp:DropDownList> </td>
                                                        <td style="width:10%"> <div class="textomantenedores">  Min</div> </td>
                                                        <td>  <asp:DropDownList ID="cbo_minutos_supi" runat="server" class="combobox" Width="55px"   TabIndex="9" onChange="PintaSiVacio(this, this.value);"> </asp:DropDownList> </td>
                                                    </tr>
                                                </table>
                                            </td>  
                                        </tr>
                                        <tr> <td colspan="2" style="height:15px""> &nbsp;</td> </tr>
                                        <tr> 
                                            <td colspan="2" align="center"> 
                                                <asp:Button ID="btn_aceptar" runat="server" Text="Agregar"   CssClass="button-enjoy" onclick="btn_aceptar_Click" TabIndex="10"   onclientclick="return Validacion3();"/>  
                                                <asp:Button ID="btn_cancelar" runat="server" Text="Cancelar"  CssClass="button-red-enjoy"    CausesValidation="False" TabIndex="11"  />
                                                <asp:Button ID="btn_exportar" runat="server" Text="Exportar"  CssClass="button-enjoy" TabIndex="12"   onclientclick="return Validacion3();"  onclick="btn_exportar_Click"/>  
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

