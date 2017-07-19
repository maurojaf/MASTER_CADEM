using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Collections;
using System.Data;

public partial class MasterSupi_SinAcceso : System.Web.UI.Page
{
    Generico_Controller _G = new Generico_Controller();
    Usuario_Controller _U = new Usuario_Controller();

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

            ArrayList _Datos = _G._Get_Ficha_Tecnica((String)Session["Rut"]);

            if (_Datos != null)
            {
                txt_rut.Text = _Datos[1].ToString();
                txt_nombre.Text = _Datos[2].ToString();
                txt_paterno.Text = _Datos[3].ToString();
                text_materno.Text = _Datos[4].ToString();
                txt_fono.Text = _Datos[14].ToString();
                txt_encargado.Text = _Datos[12].ToString();
                txt_comuna.Text = _Datos[8].ToString();
                txt_cargo.Text = _Datos[5].ToString();
                txt_email.Text = "";             
            }
         
            try
            {
                String _Id = _U._Get_Id_Pagina(Request.Url.Segments[Request.Url.Segments.Length - 1]);
                _G._Set_Insert_Logs(Session["Id_Usuario"].ToString(), DateTime.Now.ToString("yyyy-MM-dd H:mm:ss"), "SELECCION VISTA",  _Id,"", "");
            } 
            catch (Exception ee) { }
        }
    }

    protected void btn_aceptar_Click(object sender, EventArgs e)
    {
    }
  
    protected void btn_session_Click(object sender, EventArgs e)
    {
        //Controlador _Ct = new Controlador();
        
        try
        {
            String _Id = _U._Get_Id_Pagina(Request.Url.Segments[Request.Url.Segments.Length - 1]);
            Boolean _T = _G._Set_Insert_Logs(Session["Id_Usuario"].ToString(), DateTime.Now.ToString("yyyy-MM-dd H:mm:ss"), "LOGOUT", _Id,"", "");
        } 
        catch (Exception ee) { }

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
}