<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Empleados.aspx.cs" Inherits="Menu" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title> Maestro Empleados </title>
           
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
                    //Recorre todos los imput cuyas clases sean 'textbox' o 'combobox'..
                    $('.textbox').each(function () {
                        $Valor = $(this).val().trim();
                        if ($Valor === '') {
                            IsError++;
                            $(this).css('background-color', "#f7eecd");
                        } else {
                            $(this).css('background-color', "white");
                        }
                    });

                    $('.combobox').each(function () 
                    {
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
                        $(".loader").fadeIn("slow");
                    }
                    return Estado;

                });
            });


            $(function () {
                $('#txt_rut').keypress(function (ev) 
                {                 
                    var keycode = (ev.keyCode ? ev.keyCode : ev.which);
                    if (keycode == 13) {
                        $(".loader").fadeIn("slow");
                    }       
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

                    //Oculta boton Eliminar...
//                    $("#btn_eliminar").hide();

                    //Habilita campo 'Textbox rut' ...
                    $("#txt_rut").val("");
                    $("#txt_rut").attr('disabled', false);
                    $("#txt_rut").addClass("textbox");
                    $("#txt_rut").focus();

                    //Renombrar boton aceptar...
                    //$("#btn_aceptar").prop('value', 'Agregar');

                    return true;
                });
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
                  firstDay: 1 ,
                  autoSize: false,
                  beforeShow: function(){    
                      $(".ui-datepicker").css('font-size', 12) 
                  }
              }); 
        });

        $(function () {
            $('#Button2, #Button3, #Button4, #Button5').click(function () {
                $(".loader").fadeIn("slow");
            });
        });

        $(function () {
            $('#btn_eliminar').click(function () {
                var Estado;
                $Valor = $('#txt_rut').val().trim();

                if ($Valor === '')
                 {  
                    $('#txt_rut').css('background-color', "#f7eecd");
                    swal('Campos vacios', 'Primero debes buscar un RUT', 'warning');
                    Estado = false;
                }
                else 
                {
                    if (confirm("¿Eliminar Registro?") == true) {
//                        $(".loader").show();
                        $(".loader").fadeIn("slow");
                        Estado = true;
                    } else {
                        Estado = false;
                    }

                    return Estado;       
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
                <td style="width:15%"> <asp:Image ID="Image1" runat="server" Height="10%" ImageUrl="~/Images/logo-cadem.png" />  </td>
                <td style="width:55%; text-align:left">
                    <asp:Button ID="Button1" runat="server" Text="Empleados"  CssClass="select-menu-activo" CausesValidation="False"   onclientclick="return false" onclick="Button1_Click" /> 
                    <asp:Button ID="Button2" runat="server" Text="Salas"  CssClass="select-menu" CausesValidation="False" onclick="btn_go_salas_Click" /> 
                    <asp:Button ID="Button3" runat="server" Text="Logistica"   CssClass="select-menu" CausesValidation="False"  onclick="btn_go_logistica_Click"  />  
                    <asp:Button ID="Button4" runat="server" Text="Estudios"  CssClass="select-menu" CausesValidation="False" onclick="btn_go_estudio_Click"/> 
                    <asp:Button ID="Button5" runat="server" Text="Menu General"  CssClass="select-menu" CausesValidation="False"  onclick="btn_go_menu_Click"/>       
                </td>
                <td style="width:20%; text-align:right"> <asp:Label ID="lbl_session" runat="server" CssClass="nombresession" onmouseover="Mostrar();"></asp:Label> </td>
                <td style="width:10%; text-align:right"> <asp:Button ID="Button6"  runat="server" Text="Cerrar sesion" CssClass="close-session"  onclick="btn_session_Click" OnClientClick="return Confirmar();" /> </td>
            </tr>
        </table>
     </div> 
     
         
     <div style="height:45px;"></div>    
     <div class="loader"></div> 
         <br />    
     <div id="cuerpo" style="width:100%;">
    
            <center>          
                <div style="width:80%; height:100%" class="divcuerpo">                                            
                    <div style="text-align:center"> <asp:Label ID="lbl_nombre" runat="server" class="neon-text" Text="Ingresar o Actualizar Empleados"  ></asp:Label> </div>
                    <center>
                        <br /> 
                        <table cellspacing="0" width="95%">                                  
                        <tr >
                            <td>  <div class="textonaranjo"> Rut </div> </td>
                                <td >
                                <div>
                                    <center>
                                    <div style="float:left" > 
                                        <asp:Panel id="panel1" runat="server" DefaultButton="bt1" >
                                            <asp:TextBox ID="txt_rut" placeholder="" runat="server" class="textbox" Width="150px" Height="15px" MaxLength="9"  TabIndex="1" style="-webkit-border-radius: 3px 0 0 3px ; border-radius:3px 0 0 3px;  border: 1px solid rgb(255,127,80);"></asp:TextBox> 
                                            <asp:Button id="bt1" OnClick="script1" Text="Default" runat="server"  style="position:absolute; visibility:hidden; left: 458px;"/> 
                                        </asp:Panel> 
                                    </div>      
                                    <div style="float:left"> 
                                        <asp:Button ID="btn_search" runat="server"  Text="" CssClass="button-search"  CausesValidation="False" onclick="script1" />  
                                    </div>
                                    </center>
                                    </div>
                                </td>
                        </tr> 
                        <tr>
                            <td colspan="5" style="height:10px"> <asp:Label ID="lbl_id" runat="server" Text="Label" Visible="False"></asp:Label> </td>
                            <td rowspan="10">
                                <asp:Button ID="btn_aceptar" runat="server" Text="Agregar" CssClass="button-enjoy" onclick="btn_aceptar_Click" Width="100px" /> <br /> <br />
                                <asp:Button ID="btn_cancelar" runat="server" Text="Limpiar" CssClass="button-red-enjoy" onclick="btn_cancelar_Click" Width="100px"/> <br /> <br />
                            </td>
                        </tr>                
                        <tr>
                            <td style="width:15%;height:25px"> <div class="textonaranjo"> Nombre </div> </td>
                            <td style="width:35%;height:25px"> <asp:TextBox ID="txt_nombre" placeholder="" runat="server" class="textbox"     TabIndex="1" onkeydown = "return (event.keyCode!=13)" autocomplete="off" onChange="PintaSiVacio(this, this.value);"></asp:TextBox>  </td> 
                            <td style="width:15%;height:25px"> <div class="textonaranjo"> Estado </div> </td>
                            <td style="width:35%;height:25px"> <asp:DropDownList ID="cbo_estado" runat="server" class="combobox" TabIndex="8"  onChange="PintaSiVacio(this, this.value);">  </asp:DropDownList>    </td> 
                        </tr>
                        <tr>
                            <td style="width:15%;height:25px"> <div class="textonaranjo">Ap Paterno </div></td>
                            <td style="width:35%;height:25px"> <asp:TextBox ID="txt_paterno" placeholder="" runat="server" class="textbox"  TabIndex="2" onkeydown = "return (event.keyCode!=13)" autocomplete="off" onChange="PintaSiVacio(this, this.value);"></asp:TextBox>   </td> 
                            <td style="width:15%;height:25px"> <div class="textonaranjo">Grupo </div></td>
                            <td style="width:35%;height:25px"> <asp:DropDownList ID="cbo_grupo" runat="server" class="combobox" TabIndex="9"  onChange="PintaSiVacio(this, this.value);">  </asp:DropDownList> </td> 
                        </tr>                   
                        <tr>
                            <td style="width:15%;height:25px"> <div class="textonaranjo"> Ap Materno </div> </td>
                            <td style="width:35%;height:25px"> <asp:TextBox ID="text_materno" placeholder="" runat="server" class="textbox"  TabIndex="3" onkeydown = "return (event.keyCode!=13)" autocomplete="off" onChange="PintaSiVacio(this, this.value);"></asp:TextBox>  </td>        
                            <td style="width:15%;height:25px"> <div class="textonaranjo">Cargo</div></td>
                            <td style="width:35%;height:25px"> <asp:DropDownList ID="cbo_cargo" runat="server" class="combobox" TabIndex="10"  onChange="PintaSiVacio(this, this.value);">  </asp:DropDownList>     </td> 
                        </tr>
                        <tr>
                            <td style="width:15%;height:25px"> <div class="textonaranjo"> Encargado </div></td>
                            <td style="width:35%;height:25px"> <asp:DropDownList ID="cbo_coordinador" runat="server" class="combobox"  TabIndex="4" onChange="PintaSiVacio(this, this.value);">  </asp:DropDownList>            </td> 
                            <td style="width:15%;height:25px"> <div class="textonaranjo">Clasif.</div></td>
                            <td style="width:35%;height:25px"> <asp:DropDownList ID="cbo_clasificacion" runat="server" class="combobox"  TabIndex="11"  onChange="PintaSiVacio(this, this.value);">  </asp:DropDownList>  </td> 
                        </tr>
                        <tr>
                            <td style="width:15%;height:25px"> <div class="textonaranjo"> Fono </div> </td>
                            <td style="width:35%;height:25px"> <asp:TextBox ID="txt_fono" placeholder="" runat="server" class="textbox" TabIndex="5" onkeydown = "return (event.keyCode!=13)" onChange="validarSiNumero(this, this.value);" autocomplete="off"></asp:TextBox>  </td> 
                            <td style="width:15%;height:25px"> <div class="textonaranjo"> Cl fin. </div> </td>
                            <td style="width:35%;height:25px"> <asp:DropDownList ID="cbo_finanzas" runat="server" class="combobox"   TabIndex="12"  onChange="PintaSiVacio(this, this.value);">  </asp:DropDownList>     </td> 
                        </tr>
                        <tr>
                            <td style="width:15%;height:25px"> <div class="textonaranjo"> Email Cadem</div> </td>
                            <td style="width:35%;height:25px"> <asp:TextBox ID="txt_email" placeholder="" runat="server" class="textbox"   TabIndex="6" onkeydown = "return (event.keyCode!=13)" autocomplete="off"  onChange="validaemail(this, this.value);"></asp:TextBox>  </td> 
                            <td style="width:15%;height:25px"> <div class="textonaranjo">Jornada.</div></td>
                            <td style="width:35%;height:25px"> <asp:DropDownList ID="cbo_jornada" runat="server" class="combobox"   TabIndex="13"  onChange="PintaSiVacio(this, this.value);">  </asp:DropDownList>   </td> 
                        </tr>
                        <tr>
                            <td style="width:15%;height:25px"> <div class="textonaranjo"> Email Personal </div> </td>
                            <td style="width:35%;height:25px"> <asp:TextBox ID="txt_mail_personal" placeholder="" runat="server" class="textbox"  TabIndex="7" onkeydown = "return (event.keyCode!=13)" autocomplete="off" onChange="validaemail(this, this.value);"></asp:TextBox>               </td> 
                            <td style="width:15%;height:25px"> <div class="textonaranjo">Relacion </div></td>
                            <td style="width:35%;height:25px"> <asp:DropDownList ID="cbo_nivel" runat="server" class="combobox" TabIndex="14"  onChange="PintaSiVacio(this, this.value);">  </asp:DropDownList>              </td> 
                        </tr> 
                        <tr>
                            <td style="width:15%;height:25px"> <div class="textonaranjo"> Ingreso </div> </td>
                            <td style="width:35%;height:25px"> <asp:TextBox ID="txt_fecha" placeholder="" runat="server" class="textbox"  TabIndex="7" onkeydown = "return (event.keyCode!=13)" autocomplete="off"></asp:TextBox>               </td> 
                            <td style="width:15%;height:25px"> <div class="textonaranjo">Comuna</div></td>
                            <td style="width:35%;height:25px"> <asp:DropDownList ID="cbo_comuna" runat="server" class="combobox" TabIndex="14"  onChange="PintaSiVacio(this, this.value);">  </asp:DropDownList>              </td> 
                        </tr>   
                                             
                        </table>  
                        <br />
                        
                        <table width="98%" class="textonaranjo" border="0" cellpadding="0" cellspacing="1" >
                            <tr align="center" 
                                style=" font-size: x-small; height:25px;  color: #FFFFFF;">
                                <td style="width:4%"> </td>
                                <td style="width:6%;border:1px solid transparent;background-color: #018dc4">RUT </td>
                                <td style="width:11%;border:1px solid transparent;background-color: #018dc4">AP PATERNO </td>
                                <td style="width:9%;border:1px solid transparent;background-color: #018dc4">AP MATERNO </td>
                                <td style="width:13%;border:1px solid transparent;background-color: #018dc4">NOMBRE </td>
                                <td style="width:8%;border:1px solid transparent;background-color: #018dc4">FONO </td>
                                <td style="width:14%;border:1px solid transparent;background-color: #018dc4">COMUNA </td>
                                <td style="border:1px solid transparent;background-color: #018dc4">EMAIL </td>
                            </tr>
                        </table>
                     
                        <div style="width:98%;height:150px; overflow-x: hidden;" >
                            <asp:GridView ID="dgw_empleados" runat="server" BackColor="White"  
                                BorderColor="#CCCCCC" BorderStyle="Solid" BorderWidth="0px" CellPadding="0"  
                                Width="100%" BackImageUrl="~/Images/FONDO2.jpg" Font-Names="Comic Sans MS"  
                                Font-Size="XX-Small" CellSpacing="1" GridLines="Horizontal"  
                                onselectedindexchanged="dgw_empleados_SelectedIndexChanged" 
                                ShowHeader="False" onrowdatabound="dgw_empleados_RowDataBound">
                                <AlternatingRowStyle BorderStyle="None" BackColor="#CCCCCC" />
                                <Columns >
                                    <asp:CommandField ButtonType="Image" SelectImageUrl="~/Images/table_select_row.png"  ControlStyle-Height="20px" ShowSelectButton="True"  HeaderText="SELECCIONAR">
                                    <ControlStyle Height="20px"></ControlStyle>

                                        <ItemStyle HorizontalAlign="Center" Width="50px" />
                                    </asp:CommandField>
                                </Columns>
                                <FooterStyle BackColor="White" ForeColor="#000066" />
                                <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White"  BorderStyle="None" Height="25px" />
                                <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                                <RowStyle ForeColor="#000066" BorderStyle="None" />
                                <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                                <SortedAscendingCellStyle BackColor="#F1F1F1" />
                                <SortedAscendingHeaderStyle BackColor="#007DBB" />
                                <SortedDescendingCellStyle BackColor="#CAC9C9" />
                                <SortedDescendingHeaderStyle BackColor="#00547E" />
                            </asp:GridView>
                            
                        </div>                            
                    </center>
                    <br />           
                </div>  
            </center>
        </div>
    </form>
</body>
</html>
