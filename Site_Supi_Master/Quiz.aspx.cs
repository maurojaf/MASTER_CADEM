using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Web.Security;
using System.Data;
using ClosedXML.Excel;
using System.IO;

public partial class Quiz : System.Web.UI.Page
{
    Usuario_Controller _U = new Usuario_Controller();
    Generico_Controller _G = new Generico_Controller();
    Quiz_Controller _CC = new Quiz_Controller();

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
                Boolean _Acceso = _U._Tiene_Acceso_Pagina((String)Session["Rut"], Request.Url.Segments[Request.Url.Segments.Length - 1]);
                if (!_Acceso)
                {
                    Response.Redirect("Menu.aspx");
                    return;
                }
            }
            lbl_session.Text = (String)Session["Usuario"];

            //********* RESETEO DE CACHEO PARA ESTA PAGINA *************//
            Cache["LECTURA_QUIZ"] = _U._Get_Roles_Pagina((String)Session["Rut"], Request.Url.Segments[Request.Url.Segments.Length - 1], "LECTURA");
            Cache["ESCRITURA_QUIZ"] = _U._Get_Roles_Pagina((String)Session["Rut"], Request.Url.Segments[Request.Url.Segments.Length - 1], "ESCRITURA");
            Cache["EXPORTACION_QUIZ"] = _U._Get_Roles_Pagina((String)Session["Rut"], Request.Url.Segments[Request.Url.Segments.Length - 1], "EXPORTACION");
            Cache["ELIMINACION_QUIZ"] = _U._Get_Roles_Pagina((String)Session["Rut"], Request.Url.Segments[Request.Url.Segments.Length - 1], "ELIMINAR");
            Response.Cache.SetExpires(DateTime.Now.AddMinutes(60));
            //********* RESETEO DE CACHEO PARA ESTA PAGINA *************//

            DataSet _Ds_Usuarios = _CC._Get_Carga_Estudios_Quiz();
            cbo_estudio.DataSource = _Ds_Usuarios.Tables[0];
            cbo_estudio.DataValueField = _Ds_Usuarios.Tables[0].Columns[1].ToString();
            cbo_estudio.DataTextField = _Ds_Usuarios.Tables[0].Columns[0].ToString();
            cbo_estudio.DataBind();

            DataSet _Ds_Auditores = _CC._Get_Carga_Auditores_Quiz();
            cbo_auditor.DataSource = _Ds_Auditores.Tables[0];
            cbo_auditor.DataValueField = _Ds_Auditores.Tables[0].Columns[1].ToString();
            cbo_auditor.DataTextField = _Ds_Auditores.Tables[0].Columns[0].ToString();
            cbo_auditor.DataBind();

            try
            {
                String _Id = _U._Get_Id_Pagina(Request.Url.Segments[Request.Url.Segments.Length - 1]);
                _G._Set_Insert_Logs(Session["Id_Usuario"].ToString(), DateTime.Now.ToString("yyyy-MM-dd H:mm:ss"), "SELECCION VISTA", _Id, "", "");
            }
            catch (Exception) { }
        }

    }

    protected void btn_exportar_launcher_Click(object sender, EventArgs e)
    {
        if (txt_fecha_fin.Text == "" || txt_fecha_inicio.Text == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Maestro Sala", "swal('FECHA', 'FAVOR INGRESAR FECHA DE EXTRACCION', 'error');", true);
            return;
        }

        DataSet _Ds = new DataSet();
        _Ds = _CC._Get_Exportar_Quiz(txt_fecha_inicio.Text, txt_fecha_fin.Text, cbo_estudio.SelectedValue, cbo_auditor.SelectedValue);
        if (_Ds == null)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Maestro Sala", "swal('ERROR DE CONEXION', 'NO SE PUEDE CONECTAR CON EL SERVIDOR', 'error');", true);
            return;
        }

        if (_Ds.Tables[0].Rows.Count > 0)
        {
            var wb = new XLWorkbook();
            var ws = wb.Worksheets.Add("QUIZ");
            var tableWithData = ws.Cell(1, 1).InsertTable(_Ds.Tables[0].AsEnumerable());
            ws.SheetView.FreezeRows(1);
            MemoryStream m = new MemoryStream();
            wb.SaveAs(m);
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment;filename=QUIZ.xlsx");
            m.WriteTo(Response.OutputStream);
            Response.Flush();
            Response.End();
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Maestro Sala", "swal('ERROR DE EXTRACCION', 'NO HAY DATOS A EXTRAER', 'error');", true);
        }
    }

    protected void btn_volver_Click(object sender, EventArgs e)
    {
        Response.Redirect("Menu.aspx");
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
        Cache.Remove("LECTURA_QUIZ");
        Cache.Remove("ESCRITURA_QUIZ");
        Cache.Remove("EXPORTACION_QUIZ");
        Cache.Remove("ELIMINACION_QUIZ");
        Cache.Remove("Usuario");
        Cache.Remove("Id_Usuario");

        FormsAuthentication.SignOut();
        Response.Redirect(FormsAuthentication.LoginUrl); 
    }

    protected void btn_revisar_quiz_Click(object sender, EventArgs e)
    {
        if (txt_fecha_fin.Text == "" || txt_fecha_inicio.Text == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Maestro Sala", "swal('FECHA', 'FAVOR INGRESAR FECHA DE EXTRACCION', 'error');", true);
            return;
        }

        if ((String)Cache["LECTURA_QUIZ"] == "1")
        {
            if (txt_fecha_fin.Text == "" || txt_fecha_inicio.Text == "")
            {
                ClientScript.RegisterStartupScript(this.GetType(), "Maestro Sala", "swal('FECHA', 'FAVOR INGRESAR FECHA', 'error');", true);
                return;
            }
            DataSet _Ds = _CC._Get_All_Quiz(cbo_auditor.SelectedValue, cbo_estudio.SelectedValue, txt_fecha_inicio.Text, txt_fecha_fin.Text);
            dgw_quiz.Visible = true;
            dgw_quiz.DataSource = _Ds.Tables[0];
            dgw_quiz.DataBind();
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Acceso", "swal('Denegado', 'No tienes privilegios para realizar esta accion', 'error');", true);
        }
    }
}