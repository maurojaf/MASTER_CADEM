
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using System.Web.Security;
using System.Data;

public partial class Logs : System.Web.UI.Page
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

            DataSet _Ds_Usuarios = _U._Get_Usuarios();
            cbo_empleado.DataSource = _Ds_Usuarios.Tables[0];
            cbo_empleado.DataValueField = _Ds_Usuarios.Tables[0].Columns[1].ToString();
            cbo_empleado.DataTextField = _Ds_Usuarios.Tables[0].Columns[0].ToString();
            cbo_empleado.DataBind();
        }
    }

    protected void btn_session_Click(object sender, EventArgs e)
    {
        try
        {
            String _Id = _U._Get_Id_Pagina(Request.Url.Segments[Request.Url.Segments.Length - 1]);
            Boolean _T = _G._Set_Insert_Logs(Session["Id_Usuario"].ToString(), DateTime.Now.ToString("yyyy-MM-dd H:mm:ss"), "LOGOUT", _Id, "", "");
        } 
        catch (Exception ee) { }

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

    protected void btn_buscar_Click(object sender, EventArgs e)
    {
        DataSet _Ds = _U._Get_All_Logs(cbo_empleado.Text,txt_fecha.Text);
        dgw_logs.Visible = true;
        dgw_logs.DataSource= _Ds.Tables[0];
        dgw_logs.DataBind();
    }

  
    protected void btn_volver_Click(object sender, EventArgs e)
    {
        Response.Redirect("MenuAdmin.aspx");
    }
}
