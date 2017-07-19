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

public partial class Prioridades : System.Web.UI.Page
{
    Usuario_Controller _U = new Usuario_Controller();
    Prioridades_Controller _pI = new Prioridades_Controller();
    Generico_Controller _G = new Generico_Controller();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Boolean _AccesoWeb = _G._Get_Estado_Servidor();
            if (!_AccesoWeb)
            {
                Response.Redirect("Mantenedores/MantencionServidor.aspx");
                return;
            }

            if ((String)Session["Rut"] == null || (String)Session["Rut"] == "")
            {
                Response.Redirect("Login.aspx");
                return;
            }
            else
            {
                //Boolean _Acceso = _U._Tiene_Acceso_Pagina((String)Session["Rut"], Request.Url.Segments[Request.Url.Segments.Length - 1]);
                //if (!_Acceso)
                //{
                //    Response.Redirect("Menu.aspx");
                //    return;
                //}
            }
            lbl_session.Text = (String)Session["Usuario"];

            
            DataSet _Ds_Usuarios = _pI._Get_EstudioS();
            cbo_estudio.DataSource = _Ds_Usuarios.Tables[0];
            cbo_estudio.DataValueField = _Ds_Usuarios.Tables[0].Columns[1].ToString();
            cbo_estudio.DataTextField = _Ds_Usuarios.Tables[0].Columns[0].ToString();
            cbo_estudio.DataBind();
        }

    }

    //Exporta datos Excel 2007...
    protected void btn_exportar_launcher_Click(object sender, EventArgs e)
    {

        if (txt_fecha_fin.Text == "" || txt_fecha_inicio.Text == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Maestro Sala", "swal('FECHA', 'FAVOR INGRESAR FECHA DE EXTRACCION', 'error');", true);
            return;
        }

        DataSet _Ds = _pI._Get_Proiridades_EXcel(cbo_estudio.SelectedValue,txt_fecha_inicio.Text, txt_fecha_fin.Text);
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
            Response.AddHeader("content-disposition", "attachment;filename=PRIORIDADES.xlsx");
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

    //Cierra Session
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
        Cache.Remove("LECTURA_QUIZ");
        Cache.Remove("ESCRITURA_QUIZ");
        Cache.Remove("EXPORTACION_QUIZ");
        Cache.Remove("ELIMINACION_QUIZ");
        Cache.Remove("Usuario");
        Cache.Remove("Id_Usuario");

        FormsAuthentication.SignOut();
        Response.Redirect(FormsAuthentication.LoginUrl); 
    }
}