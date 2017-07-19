<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FotoSala.aspx.cs" Inherits="Logs_FotoSala" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
  <title> Galeria Fotos </title>

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
        
       

        <!-- Include Alert -->
        <script src="../Js/Widgets/SweetAlert/lib/sweet-alert.min.js"></script> 
        <link rel="../stylesheet" type="text/css" href="Js/Widgets/SweetAlert/lib/sweet-alert.css"/> 
        
        <!-- Include jQuery -->
        <link href="../Js/css/ui-lightness/jquery-ui-1.9.2.custom.css" rel="stylesheet">
	    <script src="../Js/js/jquery-1.8.3.js"></script>
	    <script src="../Js/js/jquery-ui-1.9.2.custom.js"></script>

        <!-- Validaciones -->
        <script src="../Js/Validaciones/Validaciones.js"></script>

        <!-- lightGallery -->
        <link type="text/css" rel="stylesheet" href="../css/lightGallery.css" /> 
    
        <script type="text/javascript">

            //facous pagina (sin necesidad de cargar)...
            $(document).ready(function () {
                $(".loader").fadeIn("slow");
            });

            //Pagna carga por completo ...
            $(window).load(function () {
                $(".loader").fadeOut();
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

   	<style type="text/css">
	    .glihtbox 
	    {
		    background-color:rgba(0, 0, 0, 0.0);
		    width: 95%;   
	    }
	        
	    .glihtbox ul 
	    { 
	        list-style: none; padding-top: 0px
	    }
	    
	    .glihtbox ul li  
	    {
	        display: inline; 
	    }
	    
	    .glihtbox ul img 
	    {
		    border: 2px solid coral;
		    border-width: 2px 2px 2px;
	    }
	    .glihtbox ul a:hover img 
	    {
		    border: 2px solid coral;
		    border-width: 2px 2px 2px;
		    color: #EEEEEE;
		    
	    }
	    .glihtbox ul a:hover 
	    { 
	        color: coral; 
	    }
	
	    .lb-outerContainer 
	    {
            max-width: 400px; 
            max-height: 400px;
        }
        
        .lb-dataContainer 
        {
            max-width: 400px; /* For the text below image */
        }
              
	    .titulo 
        {
          border: 1px solid coral;
          font: normal 13px/1 "Actor", Helvetica, sans-serif;
          font-family:"Helvetica Neue",Helvetica,Arial,sans-serif;
          color: coral;      
          text-align: left;
          -o-text-overflow: ellipsis;
          text-overflow: ellipsis;
          padding-left :1%;
          padding-top:4px;
          padding-bottom:4px;
          width:94%;         
          background-image: url("images/FONDO2.jpg");
          -webkit-border-radius:5px; 
          border-radius:5px;
        }
        
         .demo-gallery-poster 
         {
            background-color: rgba(0,0,0,.1);
            position: absolute;
            -webkit-transition: background-color .15s ease 0s;
            -o-transition: background-color .15s ease 0s;
            transition: background-color .15s ease 0s;
            border: 0px solid coral;
		    border-width: 0px 0px 0px;
         }
            
	</style>

</head>
<body background="../Images/b.png">
    <form id="form1" runat="server">
        <div id="header" class="divmenu" style="margin:0px; margin-top:0px;" > 
            <table style="margin:0px; width:100%" cellpadding="0px" cellspacing="0px">
                <tr>
                  <%--  <td style="width:15%"> <asp:Image ID="Image2" runat="server" Height="10%" ImageUrl="~/Images/logo-cadem.png" />  </td>
                  --%>
                    <td style="width:20%; text-align:right"> <asp:Label ID="lbl_session" runat="server" CssClass="nombresession" onmouseover="Mostrar();"></asp:Label></td>
                    <td style="width:10%; text-align:right"> <asp:Button ID="btn_session"  runat="server" Text="Cerrar sesion" CssClass="close-session" onclick="btn_session_Click" /> </td>
                </tr>
            </table>
        </div>           
        <div style="height:45px;"></div>    
        <div class="loader"></div> 


<div style="width:100%">   
         <table style="width:100%; height:100%">   
            <tr>
                <td style="width:25%" valign="top"><br />
                <div>
                    <table style="width:100%">
                        <tr>
                            <td style="width:7%"> </td>
                            <td style="width:93%">
                            <div id="Div1" style="height:100%; width:20%;  padding: 0; margin: 0;float:left;position:fixed;-webkit-border-radius: 15px; border-radius: 15px;">
                                <center>                                
                                    <table width="95%" class="tablagaleria" >
                                        <tr> <td class="textonaranjo" style="text-align:left; padding-top:20px"> <center><asp:Label ID="lbl_nombre" runat="server" Text="FOTOS INICIO DE MEDICION DE SALAS" CssClass="lbl_user"></asp:Label> </center> <br /> <br />  </td></tr>
                                        <tr style="width:40%; height:30%;"> <td class="textonaranjo" style="text-align:left; padding-top:15px">FECHA:</td> </tr>
                                        <tr style="width:40%; height:30%;"> <td align="left" style="padding-bottom:15px">  <div class="select" >  <asp:TextBox ID="txt_fecha" runat="server" class="textbox" ></asp:TextBox>   </div> </td> </tr>
                                        <tr style="width:40%; height:30%"> <td class="textonaranjo" style="text-align:left">AUDITOR:</td></tr>
                                        <tr style="width:40%; height:30%"> <td align="left" style="padding-bottom:15px"> <div class="select" >    
                                            <asp:DropDownList ID="cbo_auditor"  runat="server"  class="combobox"   style="width:230px"> </asp:DropDownList> </div> </td> </tr>
                                        <tr> 
                                            <td style="padding-top:40px; padding-bottom:70px"> 
                                               <center>
                                                <asp:Button ID="btn_buscar" runat="server" Text="BUSCAR"  CssClass="button-enjoy" onclick="btn_filtros_Click" /> 
                                                <asp:Button ID="btn_menu" runat="server" Text="MENU"  CssClass="button-enjoy" 
                                                       onclick="btn_menu_Click"/> 
                                                </center>
                                            </td>
                                        </tr>
                                    </table>
                                    
                                 </center>
                            </div>    
                            </td>
                        </tr>
                    </table>  
                </div>
                </td>
                <td > <br /> 
              
                <div ID="galleryHTML" runat="server" ></div> </td></tr>

                    <script type="text/javascript">
                        $(function () {
                            $('#galleryHTML').lightGallery({
                                selector: '.glihtbox a'
                            });
                        });
                    </script>

                    <script src="../js/lightgallery.js"></script>
                    <script src="../js/lg-thumbnail.js"></script>
                    <script src="../js/lg-fullscreen.js"></script>
                    <script src="../js/lg-autoplay.js"></script>
                    <script src="../js/lg-zoom.js"></script>
                    <script src="../js/lg-hash.js"></script>
                    <script src="../js/lg-pager.js"></script>
                    <script src="https://cdn.jsdelivr.net/picturefill/2.3.1/picturefill.min.js"></script>

        </table>   
    </div>

    </form>
</body>
</html>

