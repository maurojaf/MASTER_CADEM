using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Data;


public partial class Login : System.Web.UI.Page
{
    Usuario_Controller _U = new Usuario_Controller();
    Generico_Controller _G = new Generico_Controller();

    protected void Page_Load(object sender, EventArgs e)
    {
        // ***** ESTADO DEL SERVIDOR WEB ******
        Boolean _AccesoWeb = _G._Get_Estado_Servidor();
        if (!_AccesoWeb)
        {
            FormsAuthentication.SetAuthCookie("", true);
            Response.Redirect("Mantenedores/MantencionServidor.aspx", false);
            return;
        }

        if (!IsPostBack)
        {
            String _Us = (String)Session["Usuario"];
            String _I = (String)Session["Id_Usuario"];

            if (_I != null) 
            {             
                DataSet _Ds = _U._Get_Rut(_I);
                txt_user.Text = _Ds.Tables[0].Rows[0][0].ToString();
                txt_pass.Text = _Ds.Tables[0].Rows[0][1].ToString();
                btn_login_Click1(null, null);
            }
        }
    }


    protected void btn_login_Click1(object sender, EventArgs e)
    {
        int _Estado = _G._Login(txt_user.Text, txt_pass.Text);

        if (_Estado == 1)
        {
            Response.Cache.SetExpires(DateTime.Now.AddMinutes(60));
            Session["Id_Usuario"] = _G._Get_Id_User(txt_user.Text);
            Session["Usuario"] = _G._Get_Nombre_Usuario(txt_user.Text);
            Session["Rut"] = txt_user.Text;

            try
            {
                String _Id = _U._Get_Id_Pagina(Request.Url.Segments[Request.Url.Segments.Length - 1]);
                _G._Set_Insert_Logs(Session["Id_Usuario"].ToString(), DateTime.Now.ToString("yyyy-MM-dd H:mm:ss"), "LOGIN", _Id, "", "");
            }
            catch (Exception) { }

            FormsAuthentication.SetAuthCookie(txt_user.Text, true);
            try
            {
                //**** USUARIO ADMIN *****
                Boolean _Admin = _U._Get_Admin(Session["Rut"].ToString());
                if (_Admin)
                {
                    Response.Redirect("MenuAdmin.aspx", false);
                    return;
                }

                String _Url = Request.UrlReferrer.ToString();
                int B = _Url.IndexOf("?ReturnUrl");
                if (B != -1)
                {
                    _Url = _Url.Substring(B + 10);
                    _Url = _Url.Replace("=/Site_Supi_Master/", "");
                    _Url = _Url.Replace("=/", "");
                    if (_Url == "")
                    {
                        Response.Redirect("Menu.aspx", false);
                    }
                    else 
                    {
                        Response.Redirect(_Url, false);
                    }       
                }
                else 
                {
                    Response.Redirect("Menu.aspx", false);
                }
            }
            catch (Exception) 
            {
                Response.Redirect("Menu.aspx", false);
            }
        }
        else if (_Estado == 4)
        {   
            //Usuario Inahibilato..
            Response.Cache.SetExpires(DateTime.Now.AddMinutes(60));
            Session["Id_Usuario"] = _G._Get_Id_User(txt_user.Text);
            Session["Usuario"] = _G._Get_Nombre_Usuario(txt_user.Text);
            Session["Rut"] = txt_user.Text; 
       
            try
            {
                String _Id = _U._Get_Id_Pagina(Request.Url.Segments[Request.Url.Segments.Length - 1]);
                _G._Set_Insert_Logs(Session["Id_Usuario"].ToString(), DateTime.Now.ToString("yyyy-MM-dd H:mm:ss"), "LOGIN", _Id, "", "");
            }catch (Exception) { }

            FormsAuthentication.SetAuthCookie(txt_user.Text, true);
            Response.Redirect("SinAcceso.aspx",false);
        }
        else if (_Estado == 3)
        {
            //Error de servidor...
            Response.Cookies.Clear();
            lbl_error.Text = "No se puede conectar con el servidor";
            lbl_error.Visible = true;
            ClientScript.RegisterStartupScript(this.GetType(), "Acceso", "swal('Error de RED', 'No se puede conectar con el servidor', 'error');", true);
        }
        else if (_Estado == 2)
        {   
            //Usuario No existe...
            Response.Cookies.Clear();
            lbl_error.Text = "El usuario o contraseña no son validos";
            ClientScript.RegisterStartupScript(this.GetType(), "Acceso", "swal('Usuario no valido', 'El usuario o contraseña no son validos', 'error');", true);  
            lbl_error.Visible = true;
        }
    }

    protected void Button1_Click(object sender, EventArgs e)
    {

        FormsAuthentication.SetAuthCookie(txt_user.Text, true);
        Response.Redirect("Solicitud_Pass.aspx", false);
    }
}