<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Solicitud_Pass.aspx.cs" Inherits="Solicitud_Pass" %>

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
                } 
                return Estado;
            });
        });

        //Limpia todos los campos...
//        $(function () {
//            $('#btn_cancelar').click(function () {

//                //Limpia y pintan en blanco todos los imput cuyas clases sean 'textbox' o 'combobox'..
//                $('.textbox, .combobox').each(function () {
//                    $(this).val("");
//                    $(this).css('background-color', 'white');
//                });

//                return false;
//            });
//        });

 

        function stopPostback() {

            $Inicio = $('#txt_email').val().trim();

            if ($Inicio === '') {
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
    
    <div style="height:45px;"></div>    
    <div class="loader"></div> 
    <br />
    <div id="cuerpo" style="width:100%">
            <center>           
                <div style="width:75%; height:90%" class="divcuerpo">                              
                    <br />
                    <asp:Label ID="Label1" runat="server" class="neon-text" Text="Solicitud nueva contraseña"  ></asp:Label>
                    <br /> <br />
                                
                    <center>
                        <table width="50%" cellspacing="0">
                            <tr><td align="center"> <div class="textonaranjo" style="text-align:left"> Tu Correo </div> </td> </tr>
                            <tr><td style="height:28px;" align="center"> <asp:TextBox ID="txt_email"  runat="server" class="textbox" Width="100%" TabIndex="1"  onkeydown="return (event.keyCode!=13);"  ></asp:TextBox>  </td>   </tr>  
                            <tr><td align="center"> <div class="textonaranjo" style="text-align:left"> Mensaje </div> </td> </tr>
                            <tr><td style="height:50px" align="center"> <asp:TextBox ID="txt_msj" Height="150px" runat="server" class="textbox" TabIndex="1" Width="100%"  TextMode="MultiLine" ></asp:TextBox>  </td>   </tr>  
                        </table> 
                    </center> 

                    <br />                                               
                    <asp:Button ID="btn_solicitar" runat="server" Text="Solicitar"  CssClass="button-enjoy" TabIndex="10" onclientclick="return Validacion3();"  onclick="btn_solicitar_Click"/>  
                    <asp:Button ID="btn_cancelar" runat="server" Text="Cancelar"  CssClass="button-red-enjoy"  TabIndex="11" onclick="btn_cancelar_Click"  />                      
                    <br /> <br /> 
        
                </div>
            </center>
        </div>    
  
    </form>
</body>
</html>

