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

public partial class MasterSupi_Logistica : System.Web.UI.Page
{
    Generico_Controller _G = new Generico_Controller();
    Usuario_Controller _U = new Usuario_Controller();
    Logistica_Controller _L = new Logistica_Controller();

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
            Cache["LECTURA_LOGIS"] = _U._Get_Roles_Pagina((String)Session["Rut"], Request.Url.Segments[Request.Url.Segments.Length - 1], "LECTURA");
            Cache["ESCRITURA_LOGIS"] = _U._Get_Roles_Pagina((String)Session["Rut"], Request.Url.Segments[Request.Url.Segments.Length - 1], "ESCRITURA");
            Cache["EXPORTACION_LOGIS"] = _U._Get_Roles_Pagina((String)Session["Rut"], Request.Url.Segments[Request.Url.Segments.Length - 1], "EXPORTACION");
            Cache["ELIMINACION_LOGIS"] = _U._Get_Roles_Pagina((String)Session["Rut"], Request.Url.Segments[Request.Url.Segments.Length - 1], "ELIMINAR");
            Response.Cache.SetExpires(DateTime.Now.AddMinutes(60));
            //********* RESETEO DE CACHEO PARA ESTA PAGINA *************//

            DataSet _Ds_Comuna = _L._Get_Carga_Comuna();
            cbo_comuna_inicio.DataSource = _Ds_Comuna.Tables[0];
            cbo_comuna_inicio.DataValueField = _Ds_Comuna.Tables[0].Columns[1].ToString();
            cbo_comuna_inicio.DataTextField = _Ds_Comuna.Tables[0].Columns[0].ToString();
            cbo_comuna_inicio.DataBind();

            //DataSet _Ds_Comuna_F = _L._Get_Comuna();
            cbo_comuna_termino.DataSource = _Ds_Comuna.Tables[0];
            cbo_comuna_termino.DataValueField = _Ds_Comuna.Tables[0].Columns[1].ToString();
            cbo_comuna_termino.DataTextField = _Ds_Comuna.Tables[0].Columns[0].ToString();
            cbo_comuna_termino.DataBind();

            cbo_urbano.Items.Add("URBANO");
            cbo_urbano.Items.Add("NO URBANO");
            cbo_urbano.Items.Add("");

            cbo_horas.Items.Add("");
            cbo_horas.Items.Add("00");
            cbo_horas.Items.Add("01");
            cbo_horas.Items.Add("02");
            cbo_horas.Items.Add("03");
            cbo_horas.Items.Add("04");
            cbo_horas.Items.Add("05");
            cbo_horas.Items.Add("06");
            cbo_horas.Items.Add("07");
            cbo_horas.Items.Add("08");
            cbo_horas.Items.Add("09");
            cbo_horas.Items.Add("10");
            cbo_horas.Items.Add("11");
            cbo_horas.Items.Add("12");
                       
            cbo_minutos.Items.Add("");
            cbo_minutos.Items.Add("00");
            cbo_minutos.Items.Add("10");
            cbo_minutos.Items.Add("15");
            cbo_minutos.Items.Add("20");
            cbo_minutos.Items.Add("25");
            cbo_minutos.Items.Add("30");
            cbo_minutos.Items.Add("35");
            cbo_minutos.Items.Add("40");
            cbo_minutos.Items.Add("45");
            cbo_minutos.Items.Add("50");
            cbo_minutos.Items.Add("55");

            cbo_comuna_inicio.Text = "";
            cbo_comuna_termino.Text = "";
            cbo_comuna_inicio.Text = "";
            cbo_comuna_termino.Text = "";
            cbo_urbano.Text = "";
            cbo_horas.Text = "";
            cbo_minutos.Text = "";

            cbo_comuna_inicio.Focus();
                       
            try
            {
                String _Id = _U._Get_Id_Pagina(Request.Url.Segments[Request.Url.Segments.Length - 1]);
                _G._Set_Insert_Logs(Session["Id_Usuario"].ToString(), DateTime.Now.ToString("yyyy-MM-dd H:mm:ss"), "SELECCION VISTA", _Id, "", "");
            }
            catch (Exception) { }
        }
    }

    
    protected void btn_cancelar_Click(object sender, EventArgs e)
    {
        _Clear_Items();
        cbo_comuna_inicio.Enabled = true;
        cbo_comuna_termino.Enabled = true;
    }

    //Cierra Session...
    protected void btn_session_Click(object sender, EventArgs e)
    {
        try
        {
            String _Id = _U._Get_Id_Pagina(Request.Url.Segments[Request.Url.Segments.Length - 1]);
            _G._Set_Insert_Logs(Session["Id_Usuario"].ToString(), DateTime.Now.ToString("yyyy-MM-dd H:mm:ss"), "LOGOUT", _Id,"", "");
        }
        catch (Exception) { }

        Session.Clear();
        Session.Abandon();

        //Eliminar todas las Cache del sistema.........
        Cache.Remove("Usuario");
        Cache.Remove("Id_Usuario");

        FormsAuthentication.SignOut();
        Response.Redirect(FormsAuthentication.LoginUrl);  
    }

    // Ingresar / Actualiza Tractecto...
    protected void btn_aceptar_Click(object sender, EventArgs e)
    {
        if ((String)Cache["ESCRITURA_LOGIS"] == "1")
        {
            if (btn_aceptar.Text == "Agregar")  //Nuevo dato
            {
                String _Horas = cbo_horas.Text + ":" + cbo_minutos.Text + ":00";
                Boolean _Status = _L._Insert_Trayecto(cbo_comuna_inicio.Text, cbo_comuna_termino.Text, _Horas, txt_costo.Text, cbo_urbano.Text);
                if (_Status)
                {
                    try
                    {
                        String _Id_Pag = _U._Get_Id_Pagina(Request.Url.Segments[Request.Url.Segments.Length - 1]);
                        _G._Set_Insert_Logs(Session["Id_Usuario"].ToString(), DateTime.Now.ToString("yyyy-MM-dd H:mm:ss"), "NUEVO TRAYECTO", _Id_Pag, cbo_comuna_inicio.Text, cbo_comuna_termino.Text);
                    }
                    catch (Exception) { }
                    ClientScript.RegisterStartupScript(this.GetType(), "Maestro Logistica", "swal('Nuevo trayecto', 'trayecto ingresado correctamente', 'success');", true);
                    _Clear_Items();
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "Maestro Logistica", "swal('Error de Ingreso', ''No se puede ingresar nuevo trayecto', 'error');", true);
                }
            }
            else
            {
                //Actualizacion
                String _Horas = cbo_horas.Text + ":" + cbo_minutos.Text + ":00";
                Boolean _Status = _L._Update_Trayecto(cbo_comuna_inicio.Text, cbo_comuna_termino.Text, _Horas, txt_costo.Text, cbo_urbano.Text);
                if (_Status)
                {
                    try
                    {
                        String _Id_Pag = _U._Get_Id_Pagina(Request.Url.Segments[Request.Url.Segments.Length - 1]);
                        _G._Set_Insert_Logs(Session["Id_Usuario"].ToString(), DateTime.Now.ToString("yyyy-MM-dd H:mm:ss"), "ACTUALIZACION", _Id_Pag, cbo_comuna_inicio.Text, cbo_comuna_termino.Text);
                    }
                    catch (Exception) { }

                    ClientScript.RegisterStartupScript(this.GetType(), "Actualizacion Trayecto", "swal('Actualizacion trayecto', 'trayecto actualizado correctamente', 'success');", true);
                    _Clear_Items();
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "Actualizacion Trayecto", " swal('Advertencia', 'los Datos no han sido actualizados', 'error');", true);
                }
                cbo_comuna_inicio.Enabled = true;
                cbo_comuna_termino.Enabled = true;
            }
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Acceso", "swal('Denegado', 'No tienes privilegios para realizar esta accion', 'error');", true);
        }
    }

    //Exportar Trayectos formato excel 2007
    protected void btn_exportar_Click(object sender, EventArgs e)
    {
        if ((String)Cache["EXPORTACION_LOGIS"] == "1")
        {
            DataSet _Ds = _L._Get_Exportar_Logisticas();
            if (_Ds.Tables[0].Rows.Count > 0)
            {
                var wb = new XLWorkbook();
                var ws = wb.Worksheets.Add("TRAYECTOS");
                var tableWithData = ws.Cell(1, 1).InsertTable(_Ds.Tables[0].AsEnumerable());
                ws.SheetView.FreezeRows(1);
                MemoryStream m = new MemoryStream();
                wb.SaveAs(m);
                Response.Clear();
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=TRAYECTOS.xlsx");
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

    //Buscar Trayecto...
    protected void cbo_buscar(object sender, EventArgs e)
    {
        if ((String)Cache["ESCRITURA_LOGIS"] == "1")
        {
            if (cbo_comuna_inicio.Text != "" && cbo_comuna_termino.Text != "")
            {
                //Busqueda de datos asociados al recorrido...
                ArrayList _Datos = _L._Get_Datos_Logistica(cbo_comuna_inicio.Text, cbo_comuna_termino.Text);

                if (_Datos != null)
                {
                    if (int.Parse(_Datos[2].ToString()) < 10)
                    {
                        cbo_horas.Text = "0" + _Datos[2].ToString();
                    }
                    else
                    {
                        cbo_horas.Text = _Datos[2].ToString();
                    }

                    if (int.Parse(_Datos[3].ToString()) == 0)
                    {
                        cbo_minutos.Text = "0" + _Datos[3].ToString();
                    }
                    else
                    {
                        cbo_minutos.Text = _Datos[3].ToString();
                    }

                    txt_costo.Text = _Datos[5].ToString();
                    cbo_urbano.Text = _Datos[6].ToString();
                    cbo_comuna_inicio.Enabled = false;
                    cbo_comuna_termino.Enabled = false;

                    lbl_disponible.Text = "";
                    btn_aceptar.Text = "Actualizar";

                    try
                    {
                        String _Id_Pag = _U._Get_Id_Pagina(Request.Url.Segments[Request.Url.Segments.Length - 1]);
                        _G._Set_Insert_Logs(Session["Id_Usuario"].ToString(), DateTime.Now.ToString("yyyy-MM-dd H:mm:ss"), "BUSQUEDA", _Id_Pag, cbo_comuna_inicio.Text, cbo_comuna_termino.Text);
                    }
                    catch (Exception) { }

                }
                else
                {
                    lbl_disponible.Text = "Trayecto disponible para ingresar";
                    btn_aceptar.Text = "Agregar";
                }
                txt_costo.Focus();
            }
            else
            {
                cbo_comuna_inicio.Enabled = true;
                cbo_comuna_termino.Enabled = true;
            }
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Acceso", "swal('Denegado', 'No tienes privilegios para realizar esta accion', 'error');", true);
        }
    }





 



    public void _Clear_Items()
    {
        cbo_horas.Text = "";
        cbo_minutos.Text = "";
        cbo_comuna_inicio.SelectedIndex = 0;
        cbo_comuna_termino.SelectedIndex = 0;
        cbo_urbano.Text = "";
        txt_costo.Text = "";
        lbl_disponible.Text = "";
        btn_aceptar.Text = "Agregar";
        cbo_comuna_inicio.Focus();
    }


    //Accessos a paginas...

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

    protected void btn_go_estudio_Click(object sender, EventArgs e)
    {
        Boolean _Acceso = _U._Tiene_Acceso_Pagina((String)Session["Rut"], "Estudios.aspx");
        if (_Acceso)
        {
            Response.Redirect("Estudios.aspx");
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
}