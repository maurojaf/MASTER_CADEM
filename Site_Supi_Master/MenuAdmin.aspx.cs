using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

public partial class MenuAdmin : System.Web.UI.Page
{
    Usuario_Controller _U = new Usuario_Controller();
    Generico_Controller _G = new Generico_Controller();

    protected void Page_Load(object sender, EventArgs e)
    {
        // ***** ESTADO DEL SERVIDOR WEB ******
        Boolean _AccesoWeb = _G._Get_Estado_Servidor();
        if (!_AccesoWeb)
        {
            Response.Redirect("Mantenedores/MantencionServidor.aspx");
            return;
        }

        //Valida si el usuario logeado se o no admin
        if ((String)Session["Rut"] == null || (String)Session["Rut"] == "")
        {
            Response.Redirect("Login.aspx");
        }
        else
        {
            Boolean _Admin = _U._Get_Admin((String)Session["Rut"]);
            if (!_Admin)
            {
                Response.Redirect("Menu.aspx");
                return;
            }           
        }

        lbl_session.Text = (String)Session["Usuario"];

        try
        {
            String _Id = _U._Get_Id_Pagina(Request.Url.Segments[Request.Url.Segments.Length - 1]);
            _G._Set_Insert_Logs(Session["Id_Usuario"].ToString(), DateTime.Now.ToString("yyyy-MM-dd H:mm:ss"), "SELECCION VISTA", _Id, "", "");
        }
        catch (Exception) { }
    }

    protected void btn_session_Click(object sender, EventArgs e)
    {
        try
        {
            String _Id = _U._Get_Id_Pagina(Request.Url.Segments[Request.Url.Segments.Length - 1]);
            _G._Set_Insert_Logs(Session["Id_Usuario"].ToString(), DateTime.Now.ToString("yyyy-MM-dd H:mm:ss"), "LOGOUT", _Id, "", "");
        }
        catch (Exception) { }

        Session.Clear();
        Session.Abandon();

        //Cache.Remove("LECTURA");
        //Cache.Remove("ESCRITURA");
        //Cache.Remove("EXPORTACION");
        //Cache.Remove("ELIMINACION");
        Cache.Remove("Usuario");
        Cache.Remove("Id_Usuario");

        FormsAuthentication.SignOut();
        Response.Redirect(FormsAuthentication.LoginUrl); 
    }
    protected void btn_usuariosweb_Click(object sender, EventArgs e)
    {
        Response.Redirect("Mantenedores/PermisosUsuario.aspx");
    }
    protected void btn_logs_Click(object sender, EventArgs e)
    {
        Response.Redirect("Logs.aspx");
    }
    protected void btn_menu_Click(object sender, EventArgs e)
    {
        Response.Redirect("Menu.aspx");
    }
}