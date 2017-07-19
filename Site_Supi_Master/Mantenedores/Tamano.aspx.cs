using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

public partial class Mantenedores_Tamano : System.Web.UI.Page
{
    Tamano_Controller _T = new Tamano_Controller();
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
                Response.Redirect("~/Login.aspx");
                return;
            }
            else
            {
                Boolean _Acceso = _U._Tiene_Acceso_Pagina((String)Session["Rut"], "TAMANO.aspx");
                if (!_Acceso)
                {
                    Response.Redirect("~/Menu.aspx");
                    return;
                }
            }

            lbl_session.Text = (String)Session["Usuario"];

            //********* RESETEO DE CACHEO PARA ESTA PAGINA *************//
            Cache["LECTURA_TAM"] = _U._Get_Roles_Pagina((String)Session["Rut"], Request.Url.Segments[Request.Url.Segments.Length - 1], "LECTURA");
            Cache["ESCRITURA_TAM"] = _U._Get_Roles_Pagina((String)Session["Rut"], Request.Url.Segments[Request.Url.Segments.Length - 1], "ESCRITURA");
            Cache["EXPORTACION_TAM"] = _U._Get_Roles_Pagina((String)Session["Rut"], Request.Url.Segments[Request.Url.Segments.Length - 1], "EXPORTACION");
            Cache["ELIMINACION_TAM"] = _U._Get_Roles_Pagina((String)Session["Rut"], Request.Url.Segments[Request.Url.Segments.Length - 1], "ELIMINAR");
            Response.Cache.SetExpires(DateTime.Now.AddMinutes(60));
            //********* RESETEO DE CACHEO PARA ESTA PAGINA *************//

            dgw_tamano.DataSource = _T._Get_Carga_Tamano();
            dgw_tamano.DataBind();
        }
    }
    
    protected void btn_aceptar_Click(object sender, EventArgs e)
    {
        if ((String)Cache["ESCRITURA_TAM"] == "1")
        {
            if (btn_aceptar.Text == "Agregar")
            {
                _Ingresar();
            }
            else
            {
                _Actualizar();
            }
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Acceso", "swal('Denegado', 'No tienes privilegios para realizar esta accion', 'error');", true);
        }
    }

    protected void btn_cancelar_Click(object sender, EventArgs e)
    {
        txt_def.Text = "";
        txt_tam.Text = "";
        txt_tam.Focus();
        btn_aceptar.Text = "Agregar";
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

        //Eliminar todas las Cache del sistema.........
        Cache.Remove("LECTURA");
        Cache.Remove("ESCRITURA");
        Cache.Remove("EXPORTACION");
        Cache.Remove("ELIMINACION");
        Cache.Remove("Usuario");
        Cache.Remove("Id_Usuario");

        FormsAuthentication.SignOut();
        Response.Redirect(FormsAuthentication.LoginUrl); 
    }

    protected void btn_volver_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Estudios.aspx");
    }


    protected void dgw_empleados_SelectedIndexChanged(object sender, EventArgs e)
    {
        txt_id.Text = dgw_tamano.SelectedRow.Cells[1].Text;
        txt_tam.Text = dgw_tamano.SelectedRow.Cells[2].Text.Replace("&nbsp;","");
        txt_def.Text = dgw_tamano.SelectedRow.Cells[3].Text.Replace("&nbsp;", "");
        btn_aceptar.Text = "Actualizar";
        txt_pos.Text = dgw_tamano.SelectedIndex.ToString();
    }


    public void _Ingresar() 
    {
        foreach (GridViewRow row in dgw_tamano.Rows)
        {
            String _Tamano = row.Cells[2].Text;
            if (_Tamano == txt_tam.Text.Trim())
            {
                ClientScript.RegisterStartupScript(this.GetType(), "error", "swal('ERROR DE INGRESO', 'NO SE PUEDE INGRESAR UN TAMAÑO EXISTENTE', 'error');", true);
                return;
            }
        }

            //Controlador _C = new Controlador();
            Boolean _Success = _T._Insert_Tamano(txt_tam.Text.Trim().ToUpper(), txt_def.Text.Trim().ToUpper());
            if (_Success)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "error", "swal('NUEVO INGRESO', 'DATO INGRESADO EXITOSAMENTE', 'success');", true);
                txt_def.Text = "";
                txt_tam.Text = "";
                dgw_tamano.DataSource = _T._Get_Carga_Tamano();
                dgw_tamano.DataBind();
                txt_tam.Focus();
            }
            else 
            {
                ClientScript.RegisterStartupScript(this.GetType(), "error", "swal('ERROR DE INGRESO', 'NO SE PUEDE INGRESAR NUEVO DATO', 'error');", true);
                txt_tam.Focus();
            }
        
    }

    public void _Actualizar() 
    {
        int i=-1;
        foreach (GridViewRow row in dgw_tamano.Rows)
        {
            i++;
            if (row.Cells[2].Text == txt_tam.Text.Trim() && txt_pos.Text != i.ToString())
            {
                ClientScript.RegisterStartupScript(this.GetType(), "error", "swal('ERROR DE INGRESO', 'NO SE PUEDE INGRESAR UN NOMBRE TAMAÑO EXISTENTE', 'error');", true);
                return;
            }
        }

        //Controlador _C = new Controlador();
        Boolean _Success = _T._Update_Tamano(txt_tam.Text.Trim().ToUpper(), txt_def.Text.Trim().ToUpper(),txt_id.Text);
        if (_Success)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "error", "swal('ACTUALIZACION', 'ACTUALIZACION EXITOSA', 'success');", true);
            txt_def.Text = "";
            txt_tam.Text = "";
            dgw_tamano.DataSource = _T._Get_Carga_Tamano();
            dgw_tamano.DataBind();
            btn_aceptar.Text = "Ingresar";
            txt_tam.Focus();
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "error", "swal('ERROR DE INGRESO', 'NO SE PUEDE INGRESAR NUEVO DATO', 'error');", true);
            txt_tam.Focus();
        }
    }
}