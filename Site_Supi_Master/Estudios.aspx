<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Estudios.aspx.cs" Inherits="MasterSupi_Estudios" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title> Maestro Estudios </title>

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

            $(function () {
                $('#cbo_estudio').change(function () {
                    $(".loader").fadeIn("slow");
                    $(".loader").show();
                 });
             });

            //Validacion campos no vacios...
             $(function () {
                 $('#btn_aceptar').click(function () 
                 {
                     var Estado = true;
                     var IsError = 0;
                     $Valor = $("#cbo_estudio").val().trim();
                     if ($Valor === '') {
                         IsError = 1;
                         $("#cbo_estudio").css('background-color', "#f7eecd");
                     } else {
                         $("#cbo_estudio").css('background-color', "white");
                     }

                     if (IsError > 0) {
                         Estado = false;
                         swal('Campos vacios', 'Primera buscar estudio', 'warning');
                     } else {
                         $(".loader").fadeIn("slow");
//                         $(".loader").show();
                     }
                     return Estado;

                 });
             });


            $(function () {
                $('#btn_go_empleados, #btn_go_salas, #btn_go_logistica, #btn_go_menu, #btn_tamano').click(function () {
                    $(".loader").fadeIn("slow");
//                    $(".loader").show();
                });
            });


      </script> 

</head>
<body background="Images/b.png" >
    <form id="form1" runat="server">
         <div id="header" class="divmenu" style="margin:0px; margin-top:0px;" > 
         <table style="margin:0px; width:100%" cellpadding="0px" cellspacing="0px">
            <tr>
                <td style="width:15%"> <asp:Image ID="Image1" runat="server" Height="10%" ImageUrl="~/Images/logo-cadem.png" />  </td>
                <td style="width:55%; text-align:left">
                    <asp:Button ID="Button1" runat="server" Text="Empleados"  CssClass="select-menu" CausesValidation="False" onclick="btn_go_emepleados_Click"/> 
                    <asp:Button ID="Button2" runat="server" Text="Salas"  CssClass="select-menu" CausesValidation="False" onclick="btn_go_salas_Click" /> 
                    <asp:Button ID="Button3" runat="server" Text="Logistica"   CssClass="select-menu" CausesValidation="False"  onclick="btn_go_logistica_Click"  />  
                    <asp:Button ID="Button4" runat="server" Text="Estudios"  CssClass="select-menu-activo" CausesValidation="False" onclientclick="return false"/> 
                    <asp:Button ID="Button5" runat="server" Text="Menu General"  CssClass="select-menu" CausesValidation="False"  onclick="btn_go_menu_Click"/>       
                </td>
                <td style="width:20%; text-align:right"> <asp:Label ID="lbl_session" runat="server" CssClass="nombresession" onmouseover="Mostrar();"></asp:Label> </td>
                <td style="width:10%; text-align:right"> <asp:Button ID="Button6"  runat="server" Text="Cerrar sesion" CssClass="close-session"  onclick="btn_session_Click" OnClientClick="return Confirmar();" /> </td>
            
            </tr>
        </table>
     </div> 
        <br />
        <div style="height:45px;"></div>    
        <div class="loader"></div> 
        
        <div id="cuerpo" style="width:100%">
            <center>    
                <div style="width:70%; height:630px" class="divcuerpo">                             
                  
                        
                   
                            <br />
                            <table style="width:100%">
                                <tr>
                                    <td> <div style="text-align:right"> <asp:Label ID="Label1" runat="server" class="neon-text" Text="Ingresar o Actualizar Estudios"  ></asp:Label> </div></td>
                                    <td align="center">   <asp:Button ID="btn_tamano" runat="server"  Text="NUEVO TAMAÑO"   CssClass="button-enjoy" onclick="btn_tamano_Click"    Width="121px" Height="20px"  />
                                         </td>
                                </tr>         
                            </table>
                         
                            <br />  
                            <table width="60%" cellspacing="0">  
                                <tr> 
                                    <td align="right" style="width:20%;height:25px"> <div class="textonaranjo"> Buscar Estudio</div></td>
                                    <td  style="height:30px; width:35%" align="left"> <asp:DropDownList ID="cbo_estudio" runat="server" class="combobox" AutoPostBack="True" ontextchanged="cbo_estudio_click" TabIndex="1">  </asp:DropDownList>     </td>
                                    <td rowspan="6" align="right">  
                                        <asp:Button ID="btn_aceptar" runat="server" Text="Actualizar"  CssClass="button-enjoy" onclick="btn_aceptar_Click" TabIndex="7"  Width="60px"/>  <br /><br />
                                        <asp:Button ID="btn_cancelar" runat="server" Text="Cancelar" CssClass="button-red-enjoy" CausesValidation="False" TabIndex="8"   onclick="btn_cancelar_Click" Width="60px" /> <br />
                                        <asp:Button ID="btn_exportar" runat="server" Text="Exportar"  
                                            CssClass="button-enjoy" onclick="btn_exportar_Click" TabIndex="7"   
                                            onclientclick="return Validacion4() ;" Width="60px"/> 
                                       <br />

                                    </td>
                                </tr> 
                                
                                <tr>
                                    <td style="width:15%;height:10px"> <div id="estudio0"> <div class="textonaranjo">  <asp:Label ID="lbl_estudio" runat="server" Text="Nombre Estudio"  Visible="False"></asp:Label></div> </div></td>
                                    <td style="width:35%;height:10px"> <div id="estudio1"> <asp:TextBox ID="txt_estudio" placeholder="" runat="server" class="textbox"   TabIndex="2" onkeydown="return (event.keyCode!=13);" autocomplete="off"  onChange="PintaSiVacio(this, this.value);" Enabled="False" Visible="False"></asp:TextBox> </div></td> 
                                </tr>
                                <tr>
                                    <td style="width:15%;height:25px"> <div class="textonaranjo">Id SUPI </div></td>
                                    <td style="width:35%;height:25px"> <asp:TextBox ID="txt_id_supi" placeholder=""  runat="server" class="textbox"  TabIndex="3"  onkeydown="return (event.keyCode!=13);"  onChange="validarSiNumero(this, this.value);" autocomplete="off"  onmouseout="ocultar(tool3);" onmouseover="mostrar(tool3);" Enabled="False"></asp:TextBox>  
                                </td> 
                                </tr>                   
                                <tr>
                                    <td style="width:15%;height:25px"> <div class="textonaranjo"> Cliente </div> </td>
                                    <td style="width:35%;height:25px"> <asp:DropDownList ID="cbo_cliente"  runat="server" class="combobox" TabIndex="4"  onChange="PintaSiVacio(this, this.value);" Enabled="False">  </asp:DropDownList>   </td> 
                                </tr>
                                <tr>
                                    <td style="width:15%;height:25px"> <div class="textonaranjo"> Ranking </div> </td>
                                    <td style="width:35%;height:25px"> <asp:TextBox ID="txt_ranking" placeholder="" runat="server" class="textbox"  TabIndex="5" onkeydown="return (event.keyCode!=13);" onChange="validarSiNumero(this, this.value);" autocomplete="off"></asp:TextBox>    </td> 
                                </tr>
                                <tr>
                                    <td style="width:15%;height:25px"> <div class="textonaranjo">Estado </div></td>
                                    <td style="width:35%;height:25px"> <asp:DropDownList ID="cbo_estado" runat="server"   class="combobox" TabIndex="6" onChange="PintaSiVacio(this, this.value);"  Enabled="False">  </asp:DropDownList>   </td> 
                                </tr>           
                            </table>
                            <br />
                            <table width="30%">
                                <tr>
                                    <td>
                                        <center>
                                        <asp:GridView ID="dgw_tiempos_estudios" runat="server" AutoGenerateColumns="False" BackColor="White" 
                                            BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px" CellPadding="0" 
                                             BackImageUrl="~/Images/FONDO2.jpg" Font-Names="Comic Sans MS" 
                                            Font-Size="XX-Small" CellSpacing="1" GridLines="Horizontal" 
                                                ondatabound="GridView1_DataBound" Width="100%">
                                            <AlternatingRowStyle BorderStyle="None" BackColor="#CCCCCC" />
                                            <Columns>

                                            <asp:BoundField DataField="TAMANO" HeaderText="TAMANO" />
                                            <asp:TemplateField HeaderText="Description">
                                            <ItemTemplate>
                                            <asp:DropDownList DataSource='<%# GetCategoryDescriptions() %>' DataTextField="Description" DataValueField="Description" ID="ddlDescription" runat="server">
                                            </asp:DropDownList>
                                            </ItemTemplate>
                                            </asp:TemplateField>
                                            </Columns>
                                            <FooterStyle BackColor="White" ForeColor="#000066" />
                                            <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White"  BorderStyle="None" Height="25px" />
                                            <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                                            <RowStyle ForeColor="#000066" BorderStyle="None" Height="30px" HorizontalAlign="Center"/>
                                            <FooterStyle BackColor="White" ForeColor="#000066" />
                                            <RowStyle ForeColor="#000066" />
                                            <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                                            <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                                            <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
                                        </asp:GridView>
                                        </center>
                                    </td>
                                </tr>
                            </table>
                        </div>
                                   
               
            </center>

        </div>
    </form>
</body>
</html>