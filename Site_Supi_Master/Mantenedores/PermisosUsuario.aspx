<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PermisosUsuario.aspx.cs" Inherits="Mantenedores_PermisosUsuario" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title> Permisos y accesos </title>
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

    
            $(function () {
                $("#dgw_permisos td").click(function (event) 
                {
                    if (event.target.type === 'checkbox') 
                    {
                        var column_num = parseInt($(this).index()) ;
                        var row_num = parseInt($(this).parent().index()) ;
                        var id = event.target.id;

                        //******* ACCESO A PAGINA *********
                        if (column_num == 2) {
                            if ($("#" + id).is(':checked')) {
                            }
                            else {
                                $('#dgw_permisos_chk_lectura_' + row_num).attr('checked', false);
                                $('#dgw_permisos_chk_escritura_' + row_num).attr('checked', false);
                                $('#dgw_permisos_chk_exportar_' + row_num).attr('checked', false);
                                $('#dgw_permisos_chk_eliminar_' + row_num).attr('checked', false);
                            }
                        }

                        //****** PERMISOS DE LECTURA *********
                        if (column_num == 3) {
                            if ($("#" + id).is(':checked')) {
                                $('#dgw_permisos_chk_acceso_' + row_num).attr('checked', true);
                            }
                            else {
                                $('#dgw_permisos_chk_acceso_' + row_num).attr('checked', false);
                            }
                        }

                        //****** PERMISOS DE ESCRITURA *********
                        if (column_num == 4) {
                            if ($("#" + id).is(':checked')) {
                                $('#dgw_permisos_chk_acceso_' + row_num).attr('checked', true);
                                $('#dgw_permisos_chk_lectura_' + row_num).attr('checked', true);
                            } else {
                            }
                        }

                        //******* PERMISOS DE EXPORTAR **********
                        if (column_num == 5) {
                            if ($("#" + id).is(':checked')) {
                                $('#dgw_permisos_chk_acceso_' + row_num).attr('checked', true);
                            };
                        }

                        //******** PERMISOS DE ELIMINAR ***********
                        if (column_num == 6) {
                            if ($("#" + id).is(':checked')) {
                                $('#dgw_permisos_chk_acceso_' + row_num).attr('checked', true);
                                $('#dgw_permisos_chk_lectura_' + row_num).attr('checked', true);
                            }
                        }
                    }
                });
            });


            //Validacion campos no vacios...
            $(function () {
                $('#btn_actualizar, #btn_cancelar, #btn_volver').click(function () {
                        $(".loader").fadeIn("slow");
                });
            });

            $(function () {
                $('#cbo_usuario').change(function () {
                    $(".loader").fadeIn("slow");
                });
            });

          </script>
</head>
<body background="../Images/b.png">
    <form id="form1" runat="server">
    
     <div id="header" class="divmenu" style="margin:0px; margin-top:0px;"> 
        <table style="margin:0px; width:100%" cellpadding="0px" cellspacing="0px" border="0">
            <tr>
                <td style="width:15%"> <asp:Image ID="Image2" runat="server" Height="10%" ImageUrl="~/Images/logo-cadem.png" />  </td>
                <td style="width:55%; text-align:left"> </td>
                <td style="width:20%; text-align:right"> <asp:Label ID="lbl_session" runat="server" CssClass="nombresession"></asp:Label> </td>
                <td style="width:10%; text-align:right"> <asp:Button ID="Button6"  runat="server" Text="Cerrar sesion" CssClass="close-session" onclick="btn_session_Click" OnClientClick="return Confirmar();" /> </td>
            </tr>
        </table>
     </div>  
        
     <br /> <br /> 
     <div class="loader"></div>
     <div id="cuerpo" style="height:100%;">
    
        <center>
        <br />
            <div id="div_mantenedor" class="divmantenedor" style="background-image:url(../images/FONDO2.jpg)">                                         
                <br />
                <table width="90%" cellspacing="0" style="height:100%">  
                    <tr>
                        <td colspan="3"><div id="titulo" class="neon-text" style="height:50px;width:100%"> <asp:Label ID="Label1" runat="server" Text="Actualizar Permisos del Usuario"  style="vertical-align:middle" Font-Underline="False"></asp:Label> </div> </td>
                    </tr>  
                    <tr> 
                        <td style="width:70%;vertical-align:top"> <div class="textomantenedores" style="width:100%; text-align:left"> Nombre Usuario </div> <asp:DropDownList ID="cbo_usuario" runat="server" class="combobox" Width="90%"   TabIndex="3" AutoPostBack="True" ontextchanged="cbo_usuario_TextChanged">  </asp:DropDownList> </td>   
                        <td style="width:30%;vertical-align:top"> <div class="textomantenedores" style="width:100%; text-align:left"> Resetear Clave Acceso </div> <asp:TextBox ID="txt_pass" placeholder="Contraseña" runat="server" class="textbox"  TabIndex="2" onkeydown = "return (event.keyCode!=13)"  autocomplete="off" Width="100%" ></asp:TextBox> </td> 
                    </tr>
                </table >
              
                <div class="textomantenedores" style="width:90%;text-align:center;font-size:x-large" >Paginas Master SUPI </div>
                <table width="90%" class="textonaranjo" border="0" cellpadding="0" cellspacing="1" >
                    <tr align="center" style=" font-size: x-small; height:25px;  color: #FFFFFF;">
                        <td style="width:18%;border:1px solid transparent;background-color: #018dc4">PAGINA </td>
                        <td style="width:17%;border:1px solid transparent;background-color: #018dc4">ACCESO </td>
                        <td style="width:14%;border:1px solid transparent;background-color: #018dc4">LECTURA </td>
                        <td style="width:17%;border:1px solid transparent;background-color: #018dc4">ESCRITURA </td>    
                        <td style="width:17%;border:1px solid transparent;background-color: #018dc4">EXPORTAR </td>
                        <td style="border:1px solid transparent;background-color: #018dc4">ELIMINAR </td>
                        <td style="width:2%;border:1px solid transparent;background-color: transparent"> </td>
                    </tr>
                </table>

                <table  width="90%" cellspacing="0" style="height:100%">                
                    <tr>
                        <td colspan="3">  
                            <div style="width:100%; height:300px;overflow-y:auto;" >
                                <asp:GridView ID="dgw_permisos" runat="server" BackColor="White"  BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="1px" CellPadding="0"  Width="100%" BackImageUrl="~/Images/FONDO2.jpg" Font-Names="Comic Sans MS"  Font-Size="X-Small" CellSpacing="1" GridLines="Horizontal"  AutoGenerateColumns="False" ondatabound="dgw_permisos_DataBound"  ShowHeader="False">
                                    <AlternatingRowStyle BorderStyle="None" BackColor="#CCCCCC" />
                                    <Columns>
                                        <asp:TemplateField FooterText="ID" HeaderText="ID" Visible="False">
                                            <ItemTemplate>  <asp:Label ID="lbl_id" runat="server" Text='<%# Eval("PAG_ID") %>'></asp:Label> </ItemTemplate>   
                                            <ItemStyle Wrap="False" />
                                        </asp:TemplateField>
                                        <asp:TemplateField FooterText="PAGINA" HeaderText="PAGINA">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_pagina" runat="server" Text='<%# Eval("PAGINA") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="150px" Wrap="False" />
                                        </asp:TemplateField>
                                        <asp:TemplateField FooterText="ACCESO" HeaderText="ACCESO">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chk_acceso" runat="server" onclick="set_click(this.id, '2');" Checked='<%# bool.Parse(Eval("ACCESO").ToString() == "1" ? "True": "False") %>'/>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField FooterText="LECTURA" HeaderText="LECTURA" >
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chk_lectura" runat="server" onclick="set_click(this.id, '3');" Checked='<%# bool.Parse(Eval("LECTURA").ToString() == "1" ? "True": "False") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField FooterText="ESCRITURA" HeaderText="ESCRITURA">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chk_escritura" runat="server" onclick="set_click(this.id, '4');" Checked='<%# bool.Parse(Eval("ESCRITURA").ToString() == "1" ? "True": "False") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField FooterText="EXPORTAR" HeaderText="EXPORTAR">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chk_exportar" runat="server" onclick="set_click(this.id, '5');" Checked='<%# bool.Parse(Eval("EXPORTAR").ToString() == "1" ? "True": "False") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField FooterText="ELIMINAR" HeaderText="ELIMINAR">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chk_eliminar" runat="server" onclick="set_click(this.id, '6');" Checked='<%# bool.Parse(Eval("ELIMINAR").ToString() == "1" ? "True": "False") %>'/>
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
                        </td>
                    </tr>
                </table>
               
                <table>
                    <tr>
                        <td> <div class="textomantenedores" style="text-align:left"> <asp:CheckBox ID="chk_admin" runat="server"  Text="Usuario admin" Visible="false" /> </div></td>
                        <td> <div class="textomantenedores" style="text-align:left"> <asp:CheckBox ID="chk_acceso_sistema" runat="server"  Text="Habilitar acceso a MASTER" Visible="false"/> </div>  </td>
                        <td> <div class="textomantenedores" style="text-align:left"> <asp:CheckBox ID="chk_panel_control" runat="server" Text="Habilitar acceso PANEL DE CONTROL SUPI" Visible="false"/> </div>  </td>
                    </tr>
                    <tr>
                        <td colspan="6" align="center"> 
                            <asp:Button ID="btn_actualizar" runat="server" Text="Actualizar"   CssClass="button-enjoy" onclick="btn_actualizar_Click" TabIndex="7"/> 
                            <asp:Button ID="btn_cancelar" runat="server" Text="Cancelar" CssClass="button-red-enjoy"  CausesValidation="False" TabIndex="8"  onclick="btn_cancelar_Click" Width="77px" /> 
                            <asp:Button ID="btn_volver" runat="server" Text="Volver"  CssClass="button-enjoy-back" onclick="btn_volver_Click"  CausesValidation="False"  TabIndex="9" />
                        </td>
                    </tr>       
                </table>   
                <br />                             
            </div>        
        </center>
    </div>


    </form>
</body>
</html>
