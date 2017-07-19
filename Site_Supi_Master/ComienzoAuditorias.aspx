<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ComienzoAuditorias.aspx.cs" Inherits="ComienzoAuditorias" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
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
                $("#txt_fecha_inicio, #txt_fecha_fin").datepicker({
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
        .style1
        {
            width: 138px;
        }
    </style>

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
                <div style="width:70%; height:250px" class="divcuerpo">                              
                    <br />
                    <asp:Label ID="Label1" runat="server" class="neon-text" Text="Comienzo auditorias"  ></asp:Label>
                    <br /> 
                    <center>             
                        <table width="50%" cellspacing="0" cellpadding="0">  
                            <tr>
                                <td class="style1">  <div class="textonaranjo">Dese </div> </td>    
                                <td style="height:30px">  <asp:TextBox ID="txt_fecha_inicio" placeholder="" runat="server" class="textbox"  TabIndex="7" onkeydown = "return (event.keyCode!=13)" autocomplete="off"></asp:TextBox>    </td>   
                            </tr>  
                            <tr>
                                <td class="style1">  <div class="textonaranjo">Hasta </div> </td>    
                                <td style="height:30px">  <asp:TextBox ID="txt_fecha_fin" placeholder="" runat="server" class="textbox"  TabIndex="7" onkeydown = "return (event.keyCode!=13)" autocomplete="off"></asp:TextBox>    </td>   
                            </tr>      
                            <tr> 
                                <td class="style1"> <div class="textonaranjo">Auditor </div> </td> 
                                <td style="height:30px">  <asp:DropDownList ID="cbo_auditor" runat="server" class="combobox" TabIndex="5">  </asp:DropDownList>  </td>  
                            </tr>                                        
                            <tr> <td colspan="2" style="height:15px""> &nbsp;</td> </tr>
                            <tr><td colspan="2" align="center">  
                                <asp:Button ID="btn_revisar"  runat="server"   Text="Revisar"    CssClass="button-enjoy" TabIndex="8"  onclick="btn_revisar_Click"   />  
                                <asp:Button ID="btn_exportar"  runat="server" Text="Exportar"    
                                    CssClass="button-enjoy" TabIndex="8"  onclick="btn_exportar_Click"  />  
                                <asp:Button ID="btn_volver"  runat="server" Text="VOLVER"    CssClass="button-enjoy" TabIndex="8"  onclick="btn_volver_Click"   />  </td> 
                            </tr>
                        </table>
                    </center>                       
                </div>
                <br /> 
              
               <div style="width:70%">
                      <asp:GridView ID="dgw_auditorias" runat="server" BackColor="White"  BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="0px" CellPadding="0"  Width="100%" BackImageUrl="~/Images/FONDO2.jpg"  Font-Names="Comic Sans MS" Font-Size="X-Small" CellSpacing="1"   GridLines="Horizontal"  >
                        <AlternatingRowStyle BorderStyle="None" BackColor="#CCCCCC" />
                        <FooterStyle BackColor="White" ForeColor="#000066" />
                        <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White"  BorderStyle="None" Height="25px" />
                        <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                        <RowStyle ForeColor="#000066" BorderStyle="None"  Height="30px" HorizontalAlign="Center"/>
                        <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                        <SortedAscendingCellStyle BackColor="#F1F1F1" />
                        <SortedAscendingHeaderStyle BackColor="#007DBB" />
                        <SortedDescendingCellStyle BackColor="#CAC9C9" />
                        <SortedDescendingHeaderStyle BackColor="#00547E" />
                    </asp:GridView>
               </div>

            </center>
        </div>  
    </form>
</body>
</html>

