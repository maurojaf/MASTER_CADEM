using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Collections;
using System.Data;
using ClosedXML.Excel;
using System.IO;

public partial class ExportarDatos : System.Web.UI.Page
{
    Usuario_Controller _U = new Usuario_Controller();
    Generico_Controller _G = new Generico_Controller();
    Salas_Controller _S = new Salas_Controller();
    Empleado_Controller _E = new Empleado_Controller();
    Estudio_Controller _ES = new Estudio_Controller();

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

    public void _Exportar_Excel(DataSet _Ds, String _Nombre_Hoja, String _Nombre_Archivo)
    {
        if (_Ds.Tables[0].Rows.Count > 0)
        {
            var wb = new XLWorkbook();
            var ws = wb.Worksheets.Add(_Nombre_Hoja);
            var tableWithData = ws.Cell(1, 1).InsertTable(_Ds.Tables[0].AsEnumerable());
            ws.SheetView.FreezeRows(1);
            MemoryStream m = new MemoryStream();
            wb.SaveAs(m);
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment;filename=" + _Nombre_Archivo + ".xlsx");
            m.WriteTo(Response.OutputStream);
            Response.Flush();
            Response.End();
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Maestro Sala", "swal('ERROR DE CONEXION', 'NO SE PUEDE CONECTAR CON EL SERVIDOR', 'error');", true);
        }
    }



    protected void btn_salas_Click(object sender, EventArgs e)
    {
        String _Exportar = _U._Get_Roles_Pagina((String)Session["Rut"], "SALAS.ASPX", "EXPORTACION");
        if (_Exportar == "1")
        {
            DataSet _Ds = _S._Exportar_Salas();
            _Exportar_Excel(_Ds, "SALAS", "SALAS");
        }
        else {
            ClientScript.RegisterStartupScript(this.GetType(), "Acceso", "swal('Denegado', 'No tienes privilegios para realizar esta accion', 'error');", true);
        }
    }

    protected void btn_auditores_Click(object sender, EventArgs e)
    {
        String _Exportar = _U._Get_Roles_Pagina((String)Session["Rut"], "EMPLEADOS.ASPX", "EXPORTACION");
        if (_Exportar == "1")
        {
            DataSet _Ds = _E._Get_Exportar_Auditores();
            _Exportar_Excel(_Ds, "AUDITORES", "AUDITORES");
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Acceso", "swal('Denegado', 'No tienes privilegios para realizar esta accion', 'error');", true);
        }
    }

    protected void btn_estudios_Click(object sender, EventArgs e)
    {
        String _Exportar = _U._Get_Roles_Pagina((String)Session["Rut"], "ESTUDIOS.ASPX", "EXPORTACION");
        if (_Exportar == "1")
        {
            DataSet _Ds = _ES._Get_Exportar_Estudios();
            _Exportar_Excel(_Ds, "ESTUDIOS", "ESTUDIOS");
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Acceso", "swal('Denegado', 'No tienes privilegios para realizar esta accion', 'error');", true);
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
            _G._Set_Insert_Logs(Session["Id_Usuario"].ToString(), DateTime.Now.ToString("yyyy-MM-dd H:mm:ss"), "LOGOUT", _Id, "", "");
        }
        catch (Exception) { }

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


}