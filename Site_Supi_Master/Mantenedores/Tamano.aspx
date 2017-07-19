<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Tamano.aspx.cs" Inherits="Mantenedores_Tamano" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title> Tamanos </title>
           
        <!-- Icono -->
        <link id="Link1" runat="server" rel="Shortcut Icon"  href="../../Images/icon.ico" type="image/x-icon" />

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
<body background="../Images/b.png" style="margin: 0px, 0px, 0px, 0px">
    <form id="form1" runat="server">
    <div id="divcabecera" class="divmenu" style="margin:0px; margin-top:0px;" > 
    <table style="margin:0px; width:100%" cellpadding="0px" cellspacing="0px">
        <tr>
            <td style="width:20%"> <asp:Image ID="Image2" runat="server" Height="10%" ImageUrl="~/Images/logo-cadem.png" />  </td>
            <td style="width:60%; text-align:right"> <asp:Label ID="lbl_session" runat="server" CssClass="nombresession" onmouseover="Mostrar();"></asp:Label></td>
            <td style="width:20%; text-align:right"> <asp:Button ID="Button6"  runat="server" Text="Cerrar sesion" CssClass="close-session"  onclick="btn_session_Click" OnClientClick="return Confirmar();" /> </td>
        </tr>
     </table>
     </div>   
         
     <div style="height:45px;"></div>    
     <div class="loader"></div> 
     <br />  
     <div id="cuerpo" style="width:100%;">
        <center>
        <div style="width:70%; height:450px" class="divcuerpo">                                       
        <br />  
            <table style="width:90%">
                <tr><td> <div style="text-align:left"> <asp:Label ID="lbl_nombre" runat="server" class="neon-text" Text="Ingresar Nuevo tamaño"></asp:Label> </div> </td>
                    <td align="right">  <asp:Button ID="btn_volver" runat="server" Text="VOLVER"   CssClass="button-enjoy" onclick="btn_volver_Click" Width="130px"  />  </td>   
                </tr>         
            </table>                          
            <br /> 
            <table cellspacing="0" width="90%">                                          
                <tr><td class="style1"> <div class="textonaranjo"> TAMANO </div> </td> 

                    <td class="style2"> <asp:TextBox ID="txt_tam" placeholder="" runat="server" class="textbox" TabIndex="1" onkeydown = "return (event.keyCode!=13)"  autocomplete="off" onChange="PintaSiVacio(this, this.value);"></asp:TextBox>  </td>  
                    <td rowspan="2">
                        <asp:Button ID="btn_aceptar" runat="server" Text="Agregar" CssClass="button-enjoy" onclick="btn_aceptar_Click"  /> 
                        <asp:Button ID="btn_cancelar" runat="server" Text="Limpiar" CssClass="button-red-enjoy" onclick="btn_cancelar_Click" />
                        <asp:TextBox ID="txt_id" runat="server" Enabled="False" Visible="False"></asp:TextBox>
                        <asp:TextBox ID="txt_pos" runat="server" Enabled="False" Visible="False"></asp:TextBox>
                    </td>
                </tr>

                <tr><td style="width:15%;height:25px"> <div class="textonaranjo">DEFINICION </div></td>
                    <td style="width:35%;height:25px"> <asp:TextBox ID="txt_def" placeholder=""  runat="server" class="textbox"  TabIndex="2"  onkeydown = "return (event.keyCode!=13)" autocomplete="off"  onChange="PintaSiVacio(this, this.value);"></asp:TextBox>   </td> 
                </tr>                                
            </table>

            <br />  
              
             <div id="grilla" style="width:80%">                        
                <asp:GridView ID="dgw_tamano" runat="server" BackColor="White" BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px" CellPadding="0"  Width="100%" BackImageUrl="~/Images/FONDO2.jpg" Font-Names="Comic Sans MS" Font-Size="XX-Small" CellSpacing="1" GridLines="Horizontal"   onselectedindexchanged="dgw_empleados_SelectedIndexChanged">
                    <AlternatingRowStyle BorderStyle="None" BackColor="#CCCCCC" />
                    <Columns>
                        <asp:CommandField ButtonType="Image"  SelectImageUrl="~/Images/table_select_row.png" ControlStyle-Height="20px" ShowSelectButton="True"  HeaderText="SELECCIONAR" >
                        <ItemStyle HorizontalAlign="Center" Width="100px" />
                        </asp:CommandField>
                    </Columns>
                    <FooterStyle BackColor="White" ForeColor="#000066" />
                    <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White"  BorderStyle="None" Height="25px" />
                    <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                    <RowStyle ForeColor="#000066" BorderStyle="None"  HorizontalAlign="Center"/>
                    <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                    <SortedAscendingCellStyle BackColor="#F1F1F1" />
                    <SortedAscendingHeaderStyle BackColor="#007DBB" />
                    <SortedDescendingCellStyle BackColor="#CAC9C9" />
                    <SortedDescendingHeaderStyle BackColor="#00547E" />
                </asp:GridView> 
            </div> 
            <br />                       
        </div>
        </center>
    </div>
    </form>
</body>
</html>
