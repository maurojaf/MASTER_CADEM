<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title> Login </title>
           
    <!-- Include Estilos CSS -->
    <link href='http://fonts.googleapis.com/css?family=Open+Sans:300' rel='stylesheet' type='text/css'/>
    <script type="text/javascript" script-name="actor" src="http://use.edgefonts.net/actor.js"></script> 
    <link href="Css/Button.css" type="text/css" rel="Stylesheet" />
    <link href="Css/Label.css" type="text/css" rel="Stylesheet" />
    <link href="Css/Divs.css" type="text/css" rel="Stylesheet" />
    <link href="Css/TextBox.css" type="text/css" rel="Stylesheet" />

    <!-- Include Alert -->
    <script src="Js/Widgets/SweetAlert/lib/sweet-alert.min.js"></script> 
    <link rel="stylesheet" type="text/css" href="Js/Widgets/SweetAlert/lib/sweet-alert.css"/>

</head>
<body background="Images/b.png">
    <form id="form1" runat="server">
        <div id="cuerpo" style="height:700px;">
        <br /><br /><br /><br /><br /><br />
            <center>
                <div id="div_login" class="divlogin">
                <br />
                    <center>
                        <table width="70%" style="height:90%">
                            <tr> <td class="style1" > <div id="fondoestilo"> <div id="estilo" class="neon-label">Master SUPI</div> </div> </td>  </tr>
                            <tr> <td align="center" style="height:35px"> <asp:Image ID="Image1" runat="server" Height="26px" ImageUrl="~/Images/logo-cadem.png" /></td> </tr>
                            <tr> <td align="center"> <asp:TextBox ID="txt_user" runat="server"  class="textboxlogin" placeholder="Rut" MaxLength="9"></asp:TextBox> </td> </tr>
                            <tr> <td align="center"> <asp:TextBox ID="txt_pass" runat="server" class="textboxpass" placeholder="Password" TextMode="Password" autocomplete="off"></asp:TextBox> </td> </tr>
                            <tr> <td style="height:20px" align="center"> <asp:Label ID="lbl_error" runat="server" Text="El usuario o contraseña no son validos" Font-Bold="True"    Font-Size="X-Small" ForeColor="Red" Font-Names="Arial" Visible="False"></asp:Label></td> </tr>
                            <tr> <td align="right">
                                 <center>  <div id="login" style="width:80%">  <asp:Button ID="btn_login" runat="server" Text="LOGIN"  CssClass="button-enjoy" onclick="btn_login_Click1" ToolTip="Login Maestro SUPI"/>  </div>  </center>
                            </td> </tr>
                            <tr> <td style="height:20px" align="center">  <asp:Button ID="Button1"  runat="server" Text="CAMBIAR PASS"  CssClass="button-enjoy" ToolTip="Login Maestro SUPI" onclick="Button1_Click" Visible="false" />  </td> </tr>
                           
                        </table>
                    </center>
                </div>
            </center>
        </div>
    </form>
</body>
</html>
