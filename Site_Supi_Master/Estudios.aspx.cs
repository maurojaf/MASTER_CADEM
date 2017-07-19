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
using System.Configuration;

public partial class MasterSupi_Estudios : System.Web.UI.Page
{
    Generico_Controller _G = new Generico_Controller();
    Usuario_Controller _U = new Usuario_Controller();
    Estudio_Controller _ES = new Estudio_Controller();
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

            //********* RESETEO DE CACHE PARA ESTA PAGINA *************//
            Cache["LECTURA_EST"] = _U._Get_Roles_Pagina((String)Session["Rut"], Request.Url.Segments[Request.Url.Segments.Length - 1], "LECTURA");
            Cache["ESCRITURA_EST"] = _U._Get_Roles_Pagina((String)Session["Rut"], Request.Url.Segments[Request.Url.Segments.Length - 1], "ESCRITURA");
            Cache["EXPORTACION_EST"] = _U._Get_Roles_Pagina((String)Session["Rut"], Request.Url.Segments[Request.Url.Segments.Length - 1], "EXPORTACION");
            Cache["ELIMINACION_EST"] = _U._Get_Roles_Pagina((String)Session["Rut"], Request.Url.Segments[Request.Url.Segments.Length - 1], "ELIMINAR");
            Response.Cache.SetExpires(DateTime.Now.AddMinutes(60));

            DataSet _Ds = _ES._Get_Carga_Cliente();
            cbo_cliente.DataSource = _Ds.Tables[0];
            cbo_cliente.DataTextField = _Ds.Tables[0].Columns[0].ToString();
            cbo_cliente.DataValueField = _Ds.Tables[0].Columns[1].ToString();  
            cbo_cliente.DataBind();

            _Ds = _ES._Get_Carga_EstadoEstudio();
            cbo_estado.DataSource = _Ds.Tables[0];
            cbo_estado.DataTextField = _Ds.Tables[0].Columns[0].ToString();
            cbo_estado.DataValueField = _Ds.Tables[0].Columns[1].ToString(); 
            cbo_estado.DataBind();

            _Ds = _ET._Get_Carga_Estudios();
            cbo_estudio.DataSource = _Ds.Tables[0];
            cbo_estudio.DataTextField = _Ds.Tables[0].Columns[0].ToString();
            cbo_estudio.DataValueField = _Ds.Tables[0].Columns[1].ToString(); 
            cbo_estudio.DataBind();

            _Clear_Items();
            _Buscar();

            try
            {
                String _Id = _U._Get_Id_Pagina(Request.Url.Segments[Request.Url.Segments.Length - 1]);
                _G._Set_Insert_Logs(Session["Id_Usuario"].ToString(), DateTime.Now.ToString("yyyy-MM-dd H:mm:ss"), "SELECCION VISTA", _Id, "", "");
            }
            catch (Exception) { }
        }
    }
    




    //***** ACCIONES *********

    protected void btn_session_Click(object sender, EventArgs e)
    {        
        try
        {
            String _Id = _U._Get_Id_Pagina(Request.Url.Segments[Request.Url.Segments.Length - 1]);
            Boolean _T = _G._Set_Insert_Logs(Session["Id_Usuario"].ToString(), DateTime.Now.ToString("yyyy-MM-dd H:mm:ss"), "LOGOUT", _Id, "", "");
        }
        catch (Exception ) { }

        _Clear_Items();
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
     

    // ACTUALIZA O INGRESAR ESTUDIOS 
    protected void btn_aceptar_Click(object sender, EventArgs e)
    {
        if ((String)Cache["ESCRITURA_EST"] == "1")
        {
            Boolean _Status = _ES._Insert_O_Update_Estudio(txt_id_supi.Text, txt_estudio.Text.ToUpper(), cbo_cliente.Text, txt_ranking.Text, cbo_estado.Text);
            if (_Status)
            {
                int i = -1;
                foreach (GridViewRow row in dgw_tiempos_estudios.Rows)
                {
                    i++;
                    DropDownList sup = (DropDownList)dgw_tiempos_estudios.Rows[i].FindControl("ddlDescription");
                    String _Tiempo = sup.Text;
                    String _Tamano = row.Cells[0].Text;
                    String _Estudio = txt_id_supi.Text;
                    Boolean _Succes = _ES._Update_Tamano_Estudio(_Tamano, _Tiempo, _Estudio);
                    if (!_Succes)
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "Actualizacion Estudio", " swal('Error', 'los Datos no han sido actualizados', 'error');", true);
                        return;
                    }
                }

                try
                {
                    String _Id = _ES._Get_IdEstudio(cbo_estudio.Text);
                    String _Id_Pag = _U._Get_Id_Pagina(Request.Url.Segments[Request.Url.Segments.Length - 1]);
                    _G._Set_Insert_Logs(Session["Id_Usuario"].ToString(), DateTime.Now.ToString("yyyy-MM-dd H:mm:ss"), "ACTUALIZACION", _Id_Pag, _Id, "");
                }
                catch (Exception) { }

                _Clear_Items();
                ClientScript.RegisterStartupScript(this.GetType(), "Actualizacion Estudio", "swal('Actualizacion', 'Estudio actualizado correctamente', 'success');", true);
                dgw_tiempos_estudios.DataSource = null;
                dgw_tiempos_estudios.DataBind();
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "Actualizacion Estudio", " swal('Error', 'los Datos no han sido actualizados', 'error');", true);
            }
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Acceso", "swal('Denegado', 'No tienes privilegios para realizar esta accion', 'error');", true);
        }
    }

    // EXPORTAR DATOS A FORMATO EXCEL 2007
    protected void btn_exportar_Click(object sender, EventArgs e)
    {
        if ((String)Cache["EXPORTACION_EST"] == "1")
        {
            DataSet _Ds = _ES._Get_Exportar_Estudios();
            if (_Ds.Tables[0].Rows.Count > 0)
            {
                var wb = new XLWorkbook();
                var ws = wb.Worksheets.Add("ESTUDIOS");
                var tableWithData = ws.Cell(1, 1).InsertTable(_Ds.Tables[0].AsEnumerable());
                ws.SheetView.FreezeRows(1);
                MemoryStream m = new MemoryStream();
                wb.SaveAs(m);
                Response.Clear();
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=ESTUDIOS.xlsx");
                m.WriteTo(Response.OutputStream);
                Response.Flush();
                Response.End();
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "Maestro Sala", "swal('ERROR DE CONEXION', 'NO SE PUEDE CONECTAR CON EL SERVIDOR', 'error');", true);

            }
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Acceso", "swal('Denegado', 'No tienes privilegios para realizar esta accion', 'error');", true);
        }
    }

    // LIMPIA BOTONES
    protected void btn_cancelar_Click(object sender, EventArgs e)
    {
        _Clear_Items();

    }

    // BUSCA DATOS DEL ESTUDIO SELECCIONADO
    protected void cbo_estudio_click(object sender, EventArgs e) 
    {
        if ((String)Cache["LECTURA_EST"] == "1")
        {
            if (cbo_estudio.Text != "")
            {
                _Buscar();
            }
        }else {
            ClientScript.RegisterStartupScript(this.GetType(), "Acceso", "swal('Denegado', 'No tienes privilegios para realizar esta accion', 'error');", true);
        }
    }



    public void _Buscar() 
    {
        DataSet _Datos = _ES._Get_Busca_Datos_Estudio(cbo_estudio.Text);
        if (_Datos != null && _Datos.Tables[0].Rows.Count>0)
        {
            txt_id_supi.Text = _Datos.Tables[0].Rows[0][0].ToString();
            txt_estudio.Text = _Datos.Tables[0].Rows[0][1].ToString();
            cbo_cliente.Text = _Datos.Tables[0].Rows[0][2].ToString();
            txt_ranking.Text = _Datos.Tables[0].Rows[0][3].ToString();
            cbo_estado.Text =  _Datos.Tables[0].Rows[0][4].ToString();
            btn_aceptar.Text = "Actualizar";
            txt_estudio.Enabled = false;

            // ******** BUSCAR TIEMPOS **********
            DataSet _Ds = _ES._Get_Tamano_tIP(_Datos.Tables[0].Rows[0][0].ToString());
            dgw_tiempos_estudios.DataSource = _Ds.Tables[0];
            dgw_tiempos_estudios.DataBind();
        
            try 
            {
                String _Id = _ES._Get_IdEstudio(cbo_estudio.Text);
                String _Id_Pag = _U._Get_Id_Pagina(Request.Url.Segments[Request.Url.Segments.Length - 1]);
                _G._Set_Insert_Logs(Session["Id_Usuario"].ToString(), DateTime.Now.ToString("yyyy-MM-dd H:mm:ss"), "BUSQUEDA", _Id_Pag, _Id, "");
            }
            catch (Exception) {}
        }
        else
        {
            _Clear_Items();
            ClientScript.RegisterStartupScript(this.GetType(), "Busqueda Estudio", "swal('Estudio no encontrado', 'No se ha encontrado estudio consultado', 'error');", true);
        }
    }

    public void _Clear_Items() 
    {
        txt_estudio.Text = "";
        txt_ranking.Text = "";
        txt_id_supi.Text = "";
        btn_aceptar.Text = "Actualizar";
        lbl_estudio.Visible = true;
        txt_estudio.Visible = false;
        lbl_estudio.Visible = false;
        cbo_estudio.Enabled = true;
        cbo_estudio.Focus();
        dgw_tiempos_estudios.DataSource = null;
        dgw_tiempos_estudios.DataBind();
    }

    

    //******* Acceso a paginas *********

    protected void btn_go_salas_Click(object sender, EventArgs e)
    {
        Boolean _Acceso = _U._Tiene_Acceso_Pagina((String)Session["Rut"], "Salas.aspx");
        if (_Acceso)
        {
            Response.Redirect("Salas.aspx");
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Acceso", "swal('Denegado', 'No tienes privilegios de acceso a esta pagina', 'error');", true);
        }
    }

    protected void btn_go_logistica_Click(object sender, EventArgs e)
    {
        Boolean _Acceso = _U._Tiene_Acceso_Pagina((String)Session["Rut"], "Logistica.aspx");
        if (_Acceso)
        {
            Response.Redirect("Logistica.aspx");
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Acceso", "swal('Denegado', 'No tienes privilegios de acceso a esta pagina', 'error');", true);
        }
    }
   
    protected void btn_go_emepleados_Click(object sender, EventArgs e)
    {
        Boolean _Acceso = _U._Tiene_Acceso_Pagina((String)Session["Rut"], "Empleados.aspx");
        if (_Acceso)
        {
            Response.Redirect("Empleados.aspx");
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Acceso", "swal('Denegado', 'No tienes privilegios de acceso a esta pagina', 'error');", true);
        }
    }
    
    protected void btn_go_menu_Click(object sender, EventArgs e)
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






    public DataSet GetCategoryDescriptions()
    {
        DataSet ds = _ES._Get_Tiempos();
        return ds;
    }

    protected void GridView1_DataBound(object sender, EventArgs e)
    {
        //Controlador _C = new Controlador();
        int i = -1;
        foreach (GridViewRow row in dgw_tiempos_estudios.Rows)
        {
            i++;
            //string _Tamano = row.Cells[0].Text;
            DataSet ds = _ES._Get_Tiempo_Especifico(row.Cells[0].Text, txt_id_supi.Text);
            String _Tiempo = ds.Tables[0].Rows[0][0].ToString();
            DropDownList sup = (DropDownList)dgw_tiempos_estudios.Rows[i].FindControl("ddlDescription");
            if (_Tiempo != "")
            {
                sup.Items.Add(_Tiempo);
                sup.Text = _Tiempo;
                //sup.SelectedValue=_Tiempo;   
            }
            else {
                sup.Text = "00:00:00";
            }
        }

    }
 

}