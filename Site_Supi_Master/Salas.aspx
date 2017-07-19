<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Salas.aspx.cs" Inherits="MasterSupi_Salas" %>



<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title> Maestro Salas </title>

       <!-- Icono -->
        <link id="Link1" runat="server" rel="Shortcut Icon"  href="Images/icon.ico" type="image/x-icon" />

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
                $('#txt_folio').keypress(function (ev) {
                    var keycode = (ev.keyCode ? ev.keyCode : ev.which);
                    if (keycode == 13) {
//                        $(".loader").show();
                        $(".loader").fadeIn("slow");
                    }
                });
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
//                        $(".loader").show();
                        $(".loader").fadeOut("slow");
                    }
                    return Estado;

                });
            });


//            //Limpia todos los campos...
//            $(function () {
//                $('#btn_cancelar').click(function () {

//                    //Limpia y pintan en blanco todos los imput cuyas clases sean 'textbox' o 'combobox'..
//                    $('.textbox, .combobox').each(function () {
//                        $(this).val("");
//                        $(this).css('background-color', 'white');
//                    });

//                    //Habilita campo 'Textbox rut' ...
//                    $("#txt_folio").val("");
//                    $("#txt_folio").attr('disabled', false);
//                    $("#txt_folio").addClass("textbox");

//                    //Foco a texbox rut...
//                    $("#txt_folio").focus();

//                    //Renombrar boton aceptar...
//                    $("#btn_aceptar").prop('value', 'Agregar');

//                    return false;
//                });
//            });

            $(function () {
                $('#btn_go_emepleados, #btn_go_logistica, #btn_go_estudio, #btn_go_menu').click(function () {
//                    $(".loader").show();
                   $(".loader").fadeIn("slow");
                });
            });

      </script>

</head>
<body background="Images/b.png" >
    <form id="form1" runat="server">

    <div id="header" class="divmenu" style="margin:0px; margin-top:0px;" > 
         <table style="margin:0px; width:100%" cellpadding="0px" cellspacing="0px">
            <tr>
                <td style="width:15%"> <asp:Image ID="Image2" runat="server" Height="10%" ImageUrl="~/Images/logo-cadem.png" />  </td>
                <td style="width:55%; text-align:left">
                    <asp:Button ID="btn_go_emepleados" runat="server" Text="Empleados"   CssClass="select-menu" CausesValidation="False"   onclick="btn_go_emepleados_Click"/> 
                    <asp:Button ID="btn_go_salas" runat="server" Text="Salas"  CssClass="select-menu-activo" CausesValidation="False" onclientclick="return false"/> 
                    <asp:Button ID="btn_go_logistica" runat="server" Text="Logistica"  CssClass="select-menu" CausesValidation="False" onclick="btn_go_logistica_Click"   />  
                    <asp:Button ID="btn_go_estudio" runat="server" Text="Estudios"  CssClass="select-menu" CausesValidation="False" onclick="btn_go_estudio_Click"/> 
                    <asp:Button ID="btn_go_menu" runat="server" Text="Menu General"   CssClass="select-menu" CausesValidation="False"   onclick="btn_go_menu_Click"/>              
                </td>
                <td style="width:20%; text-align:right"> <asp:Label ID="lbl_session" runat="server" CssClass="nombresession"></asp:Label> </td>
                <td style="width:10%; text-align:right"> <asp:Button ID="Button6"  runat="server" Text="Cerrar sesion" CssClass="close-session"  onclick="btn_session_Click" OnClientClick="return Confirmar();" /> </td>          
            </tr>
        </table>
     </div>

    

       <div style="height:45px;"></div>    
       <div class="loader"></div> 

        <div id="cuerpo" style="width:100%">
            <center>  
             <br />
                <div style="width:70%; height:90%" class="divcuerpo">                  
                    <div id="divprincipal" style="height:500px; width:95%">
                        <div id="general" style="width:98%; height:100%">                    
                            <br />  

                            <table style="width:100%">
                                <tr>
                                    <td> <div style="text-align:right"> <asp:Label ID="Label2" runat="server" class="neon-text" Text="Ingresar o Actualizar Salas"  ></asp:Label> </div></td>
                                    <td align="center">   <asp:Button ID="btn_est_sala" runat="server"   Text="ESTUDIO SALA"   CssClass="button-enjoy" onclick="btn_est_sala_Click"     Width="121px" Height="20px"  />  </td>
                                </tr>         
                            </table>

  
                             <br />  
                             
                                <table cellspacing="0" width="40%">  
                                    <tr>    
                                    <td colspan="8">
                                       <center>
                                        <div style="width:100%">
                                            <div style="float:left"> 
                                            <asp:Panel id="panel1" runat="server" DefaultButton="bt1">
                                                <asp:TextBox ID="txt_folio" placeholder="BUSACR FOLIO" runat="server"  Width="150px" class="textbox" TabIndex="1" MaxLength="8" onChange="validarSiNumero(this, this.value);" style="-webkit-border-radius: 3px 0 0 3px ; border-radius:3px 0 0 3px;  border: 1px solid rgb(255,127,80);"></asp:TextBox>
                                                <asp:Button id="bt1" OnClick="script1" Text="Default" runat="server" style="position:absolute; visibility:hidden" CausesValidation="False" />                                                     
                                            </asp:Panel>
                                            </div>      
                                            <div style="float:left"> 
                                                <asp:Button ID="btn_search" runat="server" Text="" CssClass="button-search" CausesValidation="False" onclick="script1"   UseSubmitBehavior="False"   Height="25px" TabIndex="2" />
                                            </div>
                                        </div>
                                        </center>
                                    </td>
                                    </tr> 
                                </table>
                                                     
                                <table width="60%" cellspacing="0">                          
                                    <tr> <td colspan="2" style="height:20px"> </td> </tr>     
                                    <tr> <td style="width:10%;height:25px"> <div class="textonaranjo"> M2 </div> </td>
                                         <td style="width:35%;height:25px"> <asp:TextBox ID="txt_m2" placeholder="" runat="server" class="textbox" TabIndex="3" onkeydown = "return (event.keyCode!=13)" onChange="validarSiNumero(this, this.value);" autocomplete="off"></asp:TextBox>   </td> 
                                        
                                    </tr>
                                    <tr> <td style="width:10%;height:25px"> <div class="textonaranjo">Direccion </div></td>
                                         <td style="width:35%;height:25px"> <asp:TextBox ID="txt_direccion" placeholder="" runat="server" class="textbox"  TabIndex="4" onkeydown="return (event.keyCode!=13);" autocomplete="off" onChange="PintaSiVacio(this, this.value);"></asp:TextBox>  </td>                      
                                    
                                    </tr>                   
                                    <tr> <td style="width:10%;height:25px"> <div class="textonaranjo"> Latitud </div> </td>
                                         <td style="width:35%;height:25px"> <asp:TextBox ID="txt_lat" placeholder="" runat="server" class="textbox"  TabIndex="5" onkeydown="return (event.keyCode!=13);" autocomplete="off" onChange="PintaSiVacio(this, this.value);"></asp:TextBox>   </td> 
                                    </tr>
                                    <tr> <td style="width:10%;height:25px"> <div class="textonaranjo"> Longitud </div> </td>
                                         <td style="width:35%;height:25px"> <asp:TextBox ID="txt_lon" placeholder="" runat="server" class="textbox"   TabIndex="6" onkeydown="return (event.keyCode!=13);" autocomplete="off" onChange="PintaSiVacio(this, this.value);"></asp:TextBox> </td> 
                                    </tr>
                                    <tr> <td style="width:10%;height:25px"> <div class="textonaranjo">Canal </div></td>
                                         <td style="width:35%;height:25px"> <asp:DropDownList ID="cbo_canal" runat="server" class="combobox" TabIndex="7" onChange="PintaSiVacio(this, this.value);">  </asp:DropDownList>  </td> 
                                         <td> <asp:Button ID="Button1" runat="server" Text="+" CssClass="button-enjoymore" Height="20px" /> </td>
                                    </tr>
                                    <tr> <td style="width:10%;height:25px"> <div class="textonaranjo">Cadena </div></td>
                                         <td style="width:35%;height:25px"> <asp:DropDownList ID="cbo_cadena" runat="server" class="combobox" TabIndex="7" onChange="PintaSiVacio(this, this.value);">  </asp:DropDownList>  </td> 
                                         <td> <asp:Button ID="Button2" runat="server" Text="+" CssClass="button-enjoymore" Height="20px" /> </td>
                                    </tr>
                                    <tr> <td style="width:10%;height:25px"> <div class="textonaranjo">Comuna </div> </td>
                                         <td style="width:35%;height:25px"> <asp:DropDownList ID="cbo_comuna" runat="server" class="combobox" TabIndex="8" onChange="PintaSiVacio(this, this.value);">  </asp:DropDownList>   </td> 
                                         <td> <asp:Button ID="btn_addcomuna" runat="server" Text="+" 
                                                 CssClass="button-enjoymore" Height="20px" onclick="btn_addcomuna_Click" /> </td>
                                    </tr>
                                    <tr> <td style="width:10%;height:25px"> <div class="textonaranjo">Tamaño </div> </td>
                                         <td style="width:35%;height:25px"> <asp:DropDownList ID="cbo_tam" runat="server" class="combobox" TabIndex="9" onChange="PintaSiVacio(this, this.value);">  </asp:DropDownList>   </td> 
                                        <td> <asp:Button ID="Button4" runat="server" Text="+" CssClass="button-enjoymore" Height="20px" /> </td>
                                    </tr>
                                    <tr> <td colspan="2" style="height:10px"">  &nbsp;</td>
                                    </tr>
                                    <tr>
                                       
                                        <td colspan=3> 
                                            <asp:Button ID="btn_aceptar" runat="server" Text="Agregar"  CssClass="button-enjoy" onclick="btn_aceptar_Click" TabIndex="9"  onclientclick="return Validacion2();"/> 
                                            <asp:Button ID="btn_cancelar" runat="server" Text="Limpiar"  CssClass="button-red-enjoy"  CausesValidation="False" TabIndex="10"   onclick="btn_cancelar_Click" />
                                            <asp:Button ID="btn_exportar"   runat="server" Text="Exportar"   CssClass="button-enjoy"  onclick="btn_exportar_Click" TabIndex="9"  onclientclick="return Validacion2();" /> 
                                        </td>
                                    </tr>   
                                </table>       
                        </div>
                    </div>
                </div>
            </center>
        </div>
    </form>
</body>
</html>
