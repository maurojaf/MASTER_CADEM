using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

public partial class MenuMantenedores : System.Web.UI.Page
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

        if (!IsPostBack)
        {
            if ((String)Session["Rut"] == null || (String)Session["Rut"] == "")
            {
                Response.Redirect("Login.aspx");
                return;
            }
            lbl_session.Text = (String)Session["Usuario"];

            try
            {
                String _Id = _U._Get_Id_Pagina(Request.Url.Segments[Request.Url.Segments.Length - 1]);
                _G._Set_Insert_Logs(Session["Id_Usuario"].ToString(), DateTime.Now.ToString("yyyy-MM-dd H:mm:ss"), "SELECCION VISTA", _Id, "", "");
            }
            catch (Exception) { }
        }
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

        //Eliminar todas las Cache del sistema.........
        //Cache.Remove("LECTURA");
        //Cache.Remove("ESCRITURA");
        //Cache.Remove("EXPORTACION");
        //Cache.Remove("ELIMINACION");
        Cache.Remove("Usuario");
        Cache.Remove("Id_Usuario");

        FormsAuthentication.SignOut();
        Response.Redirect(FormsAuthentication.LoginUrl);  
    }

    protected void btn_volver_Click(object sender, EventArgs e)
    {
        Response.Redirect("Menu.aspx");
    }

    protected void btn_tamano_Click(object sender, EventArgs e)
    {
        Boolean _Acceso = _U._Tiene_Acceso_Pagina((String)Session["Rut"], "TAMANO.ASPX");
        if (!_Acceso)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Acceso", "swal('Denegado', 'No tienes privilegios para realizar esta accion', 'error');", true);
            return;
        }

        Response.Redirect("Mantenedores/Tamano.aspx");
    }
    
    protected void btn_canal_Click(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(), "Acceso", "swal('Denegado', 'NO OPERATIVO', 'error');", true);    
    }

    protected void btn_cargo_Click(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(), "Acceso", "swal('Denegado', 'NO OPERATIVO', 'error');", true);
    }

    protected void btn_comunas_Click(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(), "Acceso", "swal('Denegado', 'NO OPERATIVO', 'error');", true);
    }

    protected void btn_cadenas_Click(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(), "Acceso", "swal('Denegado', 'NO OPERATIVO', 'error');", true);
    }
}