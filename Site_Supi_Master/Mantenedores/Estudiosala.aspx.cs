using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Collections;
using System.Data;
using System.Net.Mail;
using System.Net;
using System.Text;

public partial class Mantenedores_Estudiosala : System.Web.UI.Page
{
    Estudio_Controller _ES = new Estudio_Controller();
    Usuario_Controller _U = new Usuario_Controller();
    Generico_Controller _G = new Generico_Controller();
    Estudiosala_Controller _ET = new Estudiosala_Controller();

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
                Response.Redirect("~/Login.aspx");
                return;
            }
            else
            {
                Boolean _Acceso = _U._Tiene_Acceso_Pagina((String)Session["Rut"], "ESTUDIOSALA.aspx");
                if (!_Acceso)
                {
                    Response.Redirect("~/Menu.aspx");
                    return;
                }    
            }

            lbl_session.Text = (String)Session["Usuario"];

            //********* RESETEO DE CACHEO PARA ESTA PAGINA *************//
            Cache["LECTURA_EST_SALA"] = _U._Get_Roles_Pagina((String)Session["Rut"], Request.Url.Segments[Request.Url.Segments.Length - 1], "LECTURA");
            Cache["ESCRITURA_EST_SALA"] = _U._Get_Roles_Pagina((String)Session["Rut"], Request.Url.Segments[Request.Url.Segments.Length - 1], "ESCRITURA");
            Cache["EXPORTACION_EST_SALA"] = _U._Get_Roles_Pagina((String)Session["Rut"], Request.Url.Segments[Request.Url.Segments.Length - 1], "EXPORTACION");
            Cache["ELIMINACION_EST_SALA"] = _U._Get_Roles_Pagina((String)Session["Rut"], Request.Url.Segments[Request.Url.Segments.Length - 1], "ELIMINAR");
            Response.Cache.SetExpires(DateTime.Now.AddMinutes(60));
            //********* RESETEO DE CACHEO PARA ESTA PAGINA *************//

            DataSet _Ds = _ET._Get_Carga_Estudios();
            cbo_estudio.DataSource = _Ds.Tables[0];
            cbo_estudio.DataTextField =  _Ds.Tables[0].Columns[0].ToString();
            cbo_estudio.DataValueField = _Ds.Tables[0].Columns[1].ToString();  
            cbo_estudio.DataBind();       
        }
    }

    protected void btn_session_Click(object sender, EventArgs e)
    {
        try
        {
            String _Id = _U._Get_Id_Pagina(Request.Url.Segments[Request.Url.Segments.Length - 1]);
            Boolean _T = _G._Set_Insert_Logs(Session["Id_Usuario"].ToString(), DateTime.Now.ToString("yyyy-MM-dd H:mm:ss"), "LOGOUT", _Id, "", "");
        }
        catch (Exception) { }

        Session.Clear();
        Session.Abandon();

        //Eliminar todas las Cache del sistema.........
        //Cache.Remove("LECTURA");
        //Cache.Remove("ESCRITURA");
        //Cache.Remove("EXPORTACION");
        //Cache.Remove("ELIMINACION");
        //Cache.Remove("Usuario");
        //Cache.Remove("Id_Usuario");

        FormsAuthentication.SignOut();
        Response.Redirect(FormsAuthentication.LoginUrl); 
    }

    protected void btn_tamano_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Salas.aspx");
    }

    protected void btn_aceptar_Click(object sender, EventArgs e)
    {
        if ((String)Cache["ESCRITURA_EST_SALA"] == "1")
        {
            Boolean _Success = _ET._Insert_Estudiosala(cbo_estudio.SelectedValue, txt_folio.Text);
            if (_Success)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "Estudio sala", "swal('ACTUALIZACION', 'DATOS ACTUALIZADOS CORRECTAMENTE', 'success');", true);
                txt_folio.Text = "";
                txt_folio.Focus();
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "Estudio sala", "swal('ERROR', 'NO SE HAN ACTUALIZADO LOS DATOS', 'error');", true);
            }
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Acceso", "swal('Denegado', 'No tienes privilegios para realizar esta accion', 'error');", true);
        }
    }

    protected void btn_cancelar_Click(object sender, EventArgs e)
    {
        txt_folio.Text = "";
        txt_folio.Focus();
    }
}