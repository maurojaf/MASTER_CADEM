﻿using System;
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

public partial class Launcher : System.Web.UI.Page
{
    Usuario_Controller _U = new Usuario_Controller();
    Generico_Controller _G = new Generico_Controller();
    Launcher_Controller _L = new Launcher_Controller();

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
            Cache["LECTURA_LAU"] = _U._Get_Roles_Pagina((String)Session["Rut"], Request.Url.Segments[Request.Url.Segments.Length - 1], "LECTURA");
            Cache["ESCRITURA_LAU"] = _U._Get_Roles_Pagina((String)Session["Rut"], Request.Url.Segments[Request.Url.Segments.Length - 1], "ESCRITURA");
            Cache["EXPORTACION_LAU"] = _U._Get_Roles_Pagina((String)Session["Rut"], Request.Url.Segments[Request.Url.Segments.Length - 1], "EXPORTACION");
            Cache["ELIMINACION_LAU"] = _U._Get_Roles_Pagina((String)Session["Rut"], Request.Url.Segments[Request.Url.Segments.Length - 1], "ELIMINAR");
            Response.Cache.SetExpires(DateTime.Now.AddMinutes(60));
            //********* RESETEO DE CACHEO PARA ESTA PAGINA *************//

            DataSet _Ds_Estudios = _L._Get_Carga_Categorias();
            cbo_estudio.DataSource = _Ds_Estudios.Tables[0];
            cbo_estudio.DataValueField = _Ds_Estudios.Tables[0].Columns[1].ToString();
            cbo_estudio.DataTextField = _Ds_Estudios.Tables[0].Columns[0].ToString();
            cbo_estudio.DataBind();

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
        if (txt_fecha.Text == "") {
            ClientScript.RegisterStartupScript(this.GetType(), "Maestro Sala", "swal('FECHA', 'FAVOR INGRESAR FECHA DE EXTRACCION', 'error');", true);
            return;
        }

        String _Categorias = "";
        DataSet _Ds = new DataSet();
        foreach (ListItem li in cbo_estudio.Items)
        {
            if (li.Selected)
            {
                _Categorias = _Categorias + "'" + li.Value.ToString() + "',";
            }
        }
        _Categorias = _Categorias.TrimEnd(',');

        if (_Categorias == "") {
            ClientScript.RegisterStartupScript(this.GetType(), "Maestro Sala", "swal('ERROR DE SELECCION', 'DEBES SELECIONAR AL MENOS UNA CATEGORIA', 'error');", true);
            return;
        }

        //_Ds = _ES._Get_Datos_Launcher(txt_fecha.Text, cbo_estudio.SelectedValue);
        _Ds = _L._Get_Exportar_Datos_Launcher(txt_fecha.Text, _Categorias, chk_3.Checked);
        if (_Ds == null) {
            ClientScript.RegisterStartupScript(this.GetType(), "Maestro Sala", "swal('ERROR DE CONEXION', 'NO SE PUEDE CONECTAR CON EL SERVIDOR', 'error');", true);
            return;
        }

        if (_Ds.Tables[0].Rows.Count > 0)
        {
            var wb = new XLWorkbook();
            var ws = wb.Worksheets.Add("LAUNCHER");
            var tableWithData = ws.Cell(1, 1).InsertTable(_Ds.Tables[0].AsEnumerable());
            ws.SheetView.FreezeRows(1);
            MemoryStream m = new MemoryStream();
            wb.SaveAs(m);
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment;filename=LAUNCHER.xlsx");
            m.WriteTo(Response.OutputStream);
            Response.Flush();
            Response.End();
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Maestro Sala", "swal('SIN DATOS', 'NO HAY DATOS A EXTRAER', 'info');", true);
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

        //Eliminar todas las Cache del sistema.........
        Cache.Remove("LECTURA_LAU");
        Cache.Remove("ESCRITURA_LAU");
        Cache.Remove("EXPORTACION_LAU");
        Cache.Remove("ELIMINACION_LAU");
        Cache.Remove("Usuario");
        Cache.Remove("Id_Usuario");

        FormsAuthentication.SignOut();
        Response.Redirect(FormsAuthentication.LoginUrl); 
    }
  
    protected void btn_volver_Click(object sender, EventArgs e)
    {
        Response.Redirect("Menu.aspx");
    }
}