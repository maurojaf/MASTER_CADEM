<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Galeria.aspx.cs" Inherits="Galeria" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title> Galeria Fotos </title>

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

        <!-- lightGallery -->
        <link type="text/css" rel="stylesheet" href="css/lightGallery.css" /> 
    
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
            $('#btn_zip').click(function () {              
                swal('Advertencia', 'Este proceso demorará dependiendo de la cantidad de fotos, favor se paciente', 'warning');
            });
        });

        $(function () {
            $('#btn_buscar').click(function () {
                if ($('#cbo_cliente').val().trim() === '--------') {
                    swal('Error', 'SELECCIONAR MEDICION', 'error');
                    return false;
                } else {
                    $(".loader").fadeIn("slow");
//                    $(".loader").show();
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
<body background="Images/b.png">
    <form id="form1" runat="server">
        <div id="header" class="divmenu" style="margin:0px; margin-top:0px;" > 
            <table style="margin:0px; width:100%" cellpadding="0px" cellspacing="0px">
                <tr>
                    <td style="width:15%"> <asp:Image ID="Image2" runat="server" Height="10%" ImageUrl="~/Images/logo-cadem.png" />  </td>
                    <td style="width:55%; text-align:left"> 
                        <asp:Button ID="Button1" runat="server" Text="Empleados" CssClass="select-menu" CausesValidation="False"  onclick="btn_go_empleado" /> 
                        <asp:Button ID="Button2" runat="server" Text="Salas" CssClass="select-menu" CausesValidation="False" onclick="btn_go_salas_Click" /> 
                        <asp:Button ID="Button3" runat="server" Text="Logistica" CssClass="select-menu" CausesValidation="False"  onclick="btn_go_logistica_Click"  />  
                        <asp:Button ID="Button4" runat="server" Text="Estudios" CssClass="select-menu" CausesValidation="False" onclick="btn_go_estudio_Click"/> 
                        <asp:Button ID="Button5" runat="server" Text="Menu General" CssClass="select-menu" CausesValidation="False"  onclick="btn_go_menu_Click"/>       
                    </td>
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
                                        <tr> <td class="textonaranjo" style="text-align:left; padding-top:20px"> <center><asp:Label ID="lbl_nombre" runat="server" Text="GALERIA DE FOTOS" CssClass="lbl_user"></asp:Label> </center> <br /> <br />  </td></tr>
                                        <tr style="width:40%; height:30%;"> <td class="textonaranjo" style="text-align:left; padding-top:15px">ESTUDIO:</td> </tr>
                                        <tr style="width:40%; height:30%;"> <td align="left" style="padding-bottom:15px">  <div class="select" > <asp:DropDownList ID="cbo_cliente" runat="server"  class="combobox" onselectedindexchanged="cbo_cliente_SelectedIndexChanged" style="width:230px"  AutoPostBack="True" > </asp:DropDownList> </div> </td> </tr>
                                        <tr style="width:40%; height:30%""> <td class="textonaranjo" style="text-align:left">MEDICION:</td></tr>
                                        <tr style="width:40%; height:30%""> <td align="left" style="padding-bottom:15px"> <div class="select" >    <asp:DropDownList ID="cbo_medicion"  runat="server"  class="combobox" onselectedindexchanged="cbo_medicion_SelectedIndexChanged"   style="width:230px" AutoPostBack="True"> </asp:DropDownList> </div> </td> </tr>
                                        <tr style="width:40%; height:30%""> <td class="textonaranjo" style="text-align:left">FOLIO:</td></tr>
                                        <tr style="width:40%; height:30%""> <td class="textonaranjo" style="text-align:left;" >
                                            <asp:TextBox ID="txt_folio" class="textbox" runat="server"></asp:TextBox>
                                        
                                        
                                        </td></tr>
                                        
                                       <%-- <tr style="width:40%; height:30%""> <td align="left" style="padding-bottom:15px"><div class="textomantenedores" >  <center> 
                                            <asp:CheckBox ID="chk_fotos" runat="server" Text="Modo Edicion" /></center>
                                            </div>  </td> </tr>
                                        <tr> <td class="textonaranjo" style="height:20px"> <center> <asp:Label ID="lbl_salas" runat="server" Text=" "  Visible="True"></asp:Label> </center> </td> </tr>
                                       --%>

                                        <tr> 
                                            <td style="padding-top:40px; padding-bottom:70px"> 
                                               <center>
                                                <asp:Button ID="btn_buscar" runat="server" Text="BUSCAR"  CssClass="button-enjoy" onclick="btn_filtros_Click" /> 
                                                <asp:Button ID="btn_zip" runat="server"  Text="ZIP"  CssClass="button-red-enjoy" onclick="btn_zip_Click" />
                                                <br /> <br />
                                                <asp:Button ID="btn_eliminafoto" runat="server" Text="ELIMINAR/HABILITAR"  CssClass="button-enjoy" onclick="btn_eliminafoto_Click" Visible="False" /> 
                                                <br /> <br />
                                                
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
                    <div style="width:95%"> 
                        <asp:GridView ID="dgw_fotos" runat="server" BackColor="White"  
                            BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px" CellPadding="0"  
                            Width="100%" BackImageUrl="~/Images/FONDO2.jpg" Font-Names="Comic Sans MS"  
                            Font-Size="X-Small" CellSpacing="1" GridLines="Horizontal" 
                            AutoGenerateColumns="False" onrowdatabound="dgw_fotos_RowDataBound" >
                            <AlternatingRowStyle BorderStyle="None" BackColor="#CCCCCC" />
                            <Columns>
                              
                                <asp:TemplateField  HeaderText="ELIMINAR FOTO">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="CheckBox1" runat="server" Checked='<%# Eval("cc") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                             
                                <asp:TemplateField HeaderText="AUDITOR">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_auditor" runat="server" Text='<%# Eval("Auditor") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                              
                                <asp:TemplateField HeaderText="SALA">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_sala" runat="server" Text='<%# Eval("Faculty") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="FECHA">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_folio" runat="server" Text='<%# Eval("Fecha") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="RUTA">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_ruta" runat="server" Text='<%# Eval("Ruta") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="IMAGEN">
                                    <ItemTemplate>
                                        <asp:Image ID="imageFaculty" ImageUrl='<%# Eval("imageurl") %>' Width="100px" Height="100px" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
        
                            </Columns>
                            <FooterStyle BackColor="White" ForeColor="#000066" />
                            <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White"  BorderStyle="None" Height="25px" />
                            <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                            <RowStyle ForeColor="#000066" BorderStyle="None" Height="30px" HorizontalAlign="Center"/>
                            <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                            <SortedAscendingCellStyle BackColor="#F1F1F1" />
                            <SortedAscendingHeaderStyle BackColor="#007DBB" />
                            <SortedDescendingCellStyle BackColor="#CAC9C9" />
                            <SortedDescendingHeaderStyle BackColor="#00547E" />
                        </asp:GridView>

                    
                    </div>
                    <div ID="galleryHTML" runat="server" ></div> </td></tr>

                    <script type="text/javascript">
                         $(function () {
                             $('#galleryHTML').lightGallery({
                                 selector: '.glihtbox a'
                             });
                         });
                    </script>

                    <script src="js/lightgallery.js"></script>
                    <script src="js/lg-thumbnail.js"></script>
                    <script src="js/lg-fullscreen.js"></script>
                    <script src="js/lg-autoplay.js"></script>
                    <script src="js/lg-zoom.js"></script>
                    <script src="js/lg-hash.js"></script>
                    <script src="js/lg-pager.js"></script>
                    <script src="https://cdn.jsdelivr.net/picturefill/2.3.1/picturefill.min.js"></script>

        </table>   
    </div>

    </form>
</body>
</html>
