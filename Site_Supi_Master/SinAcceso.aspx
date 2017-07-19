<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SinAcceso.aspx.cs" Inherits="MasterSupi_SinAcceso" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title> Actualizar Mis Datos </title>
           
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
        <link href="Css/Tooltip.css" type="text/css" rel="Stylesheet" />

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
                    //Recorre todos los imput cuyas clases sean 'textbox' o 'combobox'...
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
                        $(".loader").fadeIn("slow");
                    }
                    return Estado;
                });
            }); 
    </script>
   
</head>
<body background="Images/b.png" style="margin: 0px, 0px, 0px, 0px">
    <form id="form1" runat="server">
    
     <div id="header" class="divmenu" style="margin:0px; margin-top:0px;" > 
         <table style="margin:0px; width:100%" cellpadding="0px" cellspacing="0px">
            <tr>
                <td style="width:15%"> <asp:Image ID="Image2" runat="server" Height="40px" ImageUrl="~/Images/logo-cadem.png" />  </td>
                <td style="width:55%; text-align:left"> </td>
                <td style="width:20%; text-align:right"> <asp:Label ID="lbl_session" runat="server" CssClass="nombresession"></asp:Label> </td>
                <td style="width:10%; text-align:right"> <asp:Button ID="Button6"  runat="server" Text="Cerrar sesion" CssClass="close-session"  onclick="btn_session_Click" OnClientClick="return Confirmar();" /> </td>
            
            </tr>
        </table>
     </div>


     <div style="height:45px;"></div>    
     <div class="loader"></div> 
             
     <div id="cuerpo" style="width:100%;">
            <center>
                <div style="width:70%; height:630px" class="divcuerpo">                                   
                    <div id="divprincipal" style="height:500px; width:95%">
                        <div id="general" style="width:98%; height:500px"> 
                        <br />                
                            <asp:Label ID="lbl_nombre" runat="server" class="neon-text" Text="Mis Datos Generales"  ></asp:Label>  
                            <center><br /> <br />
                                <table cellspacing="0" width="70%">  
                                <tr> <td> <div class="textonaranjo"> Rut </div> </td>
                                     <td> <asp:TextBox ID="txt_rut" placeholder="" runat="server" class="textbox" onkeydown = "return (event.keyCode!=13)" TabIndex="1"    Enabled="False"></asp:TextBox>   </td>
                                </tr>                     
                                <tr><td class="style1"> <div class="textonaranjo"> Nombre </div> </td>
                                    <td class="style2"> <asp:TextBox ID="txt_nombre" runat="server"  class="textbox"     TabIndex="1" onkeydown = "return (event.keyCode!=13)" autocomplete="off" Enabled="False"></asp:TextBox>  </td> 
                                    <td class="style1"> <div class="textonaranjo"> Jefe directo </div> </td>
                                    <td class="style2"> <asp:TextBox ID="txt_encargado" runat="server"  class="textbox" TabIndex="5" onkeydown = "return (event.keyCode!=13)" autocomplete="off" Enabled="False"></asp:TextBox>  </td> 
                                </tr>
                                <tr><td style="width:15%;height:25px"> <div class="textonaranjo">Ap Paterno </div></td>
                                    <td style="width:35%;height:25px"> <asp:TextBox ID="txt_paterno"  runat="server" class="textbox"  TabIndex="2"  onkeydown = "return (event.keyCode!=13)" autocomplete="off"  Enabled="False"></asp:TextBox>   </td> 
                                    <td class="style2"> <div class="textonaranjo">Comuna </div></td>
                                    <td style="width:35%;height:25px">   <asp:TextBox ID="txt_comuna" runat="server"  class="textbox" TabIndex="5" onkeydown = "return (event.keyCode!=13)" autocomplete="off"   Enabled="False"></asp:TextBox>              </td> 
                                </tr>                   
                                <tr><td style="width:15%;height:25px"> <div class="textonaranjo"> Ap Materno </div> </td>
                                    <td style="width:35%;height:25px"> <asp:TextBox ID="text_materno" runat="server" class="textbox"  TabIndex="3"   onkeydown = "return (event.keyCode!=13)" autocomplete="off"  Enabled="False"></asp:TextBox>  </td>        
                                    <td class="style2"> <div class="textonaranjo">Cargo</div></td>
                                    <td style="width:35%;height:25px">   <asp:TextBox ID="txt_cargo" runat="server"    class="textbox" TabIndex="5" onkeydown = "return (event.keyCode!=13)" autocomplete="off" Enabled="False"></asp:TextBox>              </td> 
                                </tr>
                                <tr><td class="style1"> <div class="textonaranjo"> Fono </div> </td>
                                    <td class="style2"> <asp:TextBox ID="txt_fono" runat="server" class="textbox" TabIndex="5" onkeydown = "return (event.keyCode!=13)"  autocomplete="off"></asp:TextBox>  </td> 
                                    <td class="style1"> <div class="textonaranjo"> Email Personal </div> </td>
                                    <td class="style2"> <asp:TextBox ID="txt_email"  runat="server" class="textbox"   TabIndex="6" onkeydown = "return (event.keyCode!=13)" autocomplete="off" ></asp:TextBox>  </td> 
                                </tr>
                                <tr><td colspan="6" style="height:20px;" align="left"> &nbsp;</td></tr>
                                <tr> <td colspan="6" align="center">  <asp:Button ID="btn_aceptar" runat="server" Text="Actualizar" CssClass="button-enjoy" onclick="btn_aceptar_Click"  />  </td></tr>              
                                </table>                                
                            </center>   
                        </div>
                    </div>           
                </div>
            </center>
        </div>
    </form>
</body>
</html>
