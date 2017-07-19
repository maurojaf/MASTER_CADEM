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


public partial class MasterSupi_Salas : System.Web.UI.Page
{
    Generico_Controller _G = new Generico_Controller();
    Usuario_Controller _U = new Usuario_Controller();
    Salas_Controller _S = new Salas_Controller();

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
            Cache["LECTURA_SALA"] = _U._Get_Roles_Pagina((String)Session["Rut"], Request.Url.Segments[Request.Url.Segments.Length - 1], "LECTURA");
            Cache["ESCRITURA_SALA"] = _U._Get_Roles_Pagina((String)Session["Rut"], Request.Url.Segments[Request.Url.Segments.Length - 1], "ESCRITURA");
            Cache["EXPORTACION_SALA"] = _U._Get_Roles_Pagina((String)Session["Rut"], Request.Url.Segments[Request.Url.Segments.Length - 1], "EXPORTACION");
            Cache["ELIMINACION_SALA"] = _U._Get_Roles_Pagina((String)Session["Rut"], Request.Url.Segments[Request.Url.Segments.Length - 1], "ELIMINAR");
            Response.Cache.SetExpires(DateTime.Now.AddMinutes(60));
            //********* RESETEO DE CACHEO PARA ESTA PAGINA *************//

            DataSet _Ds = _S._Get_Comuna();
            cbo_comuna.DataSource = _Ds.Tables[0];
            cbo_comuna.DataTextField = _Ds.Tables[0].Columns[0].ToString();
            cbo_comuna.DataValueField = _Ds.Tables[0].Columns[1].ToString();   
            cbo_comuna.DataBind();

            _Ds = _S._Get_Canal();
            cbo_canal.DataSource = _Ds.Tables[0];
            cbo_canal.DataTextField = _Ds.Tables[0].Columns[0].ToString();
            cbo_canal.DataValueField = _Ds.Tables[0].Columns[1].ToString();   
            cbo_canal.DataBind();

            _Ds = _S._Get_Cadena();
            cbo_cadena.DataSource = _Ds.Tables[0];
            cbo_cadena.DataTextField  = _Ds.Tables[0].Columns[0].ToString();
            cbo_cadena.DataValueField = _Ds.Tables[0].Columns[1].ToString();   
            cbo_cadena.DataBind();
       
            _Ds = _S._Get_Carga_Tamano();
            cbo_tam.DataSource = _Ds.Tables[0];
            cbo_tam.DataTextField  = _Ds.Tables[0].Columns[0].ToString();
            cbo_tam.DataValueField = _Ds.Tables[0].Columns[1].ToString();
            cbo_tam.DataBind();

            _Clear_Items();
          
            try
            {
                String _Id = _U._Get_Id_Pagina(Request.Url.Segments[Request.Url.Segments.Length - 1]);
                _G._Set_Insert_Logs(Session["Id_Usuario"].ToString(), DateTime.Now.ToString("yyyy-MM-dd H:mm:ss"), "SELECCION VISTA", _Id, "", "");
            } 
            catch (Exception) { }
        }
    }

    //Cierra Session actual
    protected void btn_session_Click(object sender, EventArgs e)
    {

        try
        {
            String _Id_Pag = _U._Get_Id_Pagina(Request.Url.Segments[Request.Url.Segments.Length - 1]);
            Boolean _T = _G._Set_Insert_Logs(Session["Id_Usuario"].ToString(), DateTime.Now.ToString("yyyy-MM-dd H:mm:ss"), "LOGOUT", _Id_Pag, "", "");
        } 
        catch (Exception) { }

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


    protected void btn_aceptar_Click(object sender, EventArgs e)
    {
        if ((String)Cache["ESCRITURA_SALA"] == "1")
        {
            if (btn_aceptar.Text == "Agregar")
            {
                _Agregar();
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


    //Exporta salas a formato Excel 2007
    protected void btn_exportar_Click(object sender, EventArgs e)
    {
        if ((String)Cache["EXPORTACION_SALA"] == "1")
        {
            DataSet _Ds = _S._Exportar_Salas();
            if (_Ds.Tables[0].Rows.Count > 0)
            {
                var wb = new XLWorkbook();
                var ws = wb.Worksheets.Add("SALAS");
                var tableWithData = ws.Cell(1, 1).InsertTable(_Ds.Tables[0].AsEnumerable());
                ws.SheetView.FreezeRows(1);
                MemoryStream m = new MemoryStream();
                wb.SaveAs(m);
                Response.Clear();
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=SALAS.xlsx");
                m.WriteTo(Response.OutputStream);
                Response.Flush();
                Response.End();
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "Maestro Sala", "swal('ERROR DE CONEXION', 'NO SE PUEDE CONECTAR CON EL SERVIDOR', 'error');", true);
            }
            //_Exportar();
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Acceso", "swal('Denegado', 'No tienes privilegios para realizar esta accion', 'error');", true);

        }
    }


    //Busca sala solicitada
    protected void script1(object sender, EventArgs e)
    {
        if ((String)Cache["LECTURA_SALA"] == "1")
        {
            //_Buscar();
            if (txt_folio.Text != "")
            {
                DataSet _Datos = _S._Get_Datos_Sala(txt_folio.Text);

                if (_Datos != null && _Datos.Tables[0].Rows.Count>0)
                {
                    txt_folio.Text = _Datos.Tables[0].Rows[0][1].ToString();
                    txt_direccion.Text = _Datos.Tables[0].Rows[0][2].ToString();
                    cbo_comuna.Text = _Datos.Tables[0].Rows[0][3].ToString();
                    txt_lat.Text = _Datos.Tables[0].Rows[0][4].ToString();
                    txt_lon.Text = _Datos.Tables[0].Rows[0][5].ToString();
                    cbo_canal.Text = _Datos.Tables[0].Rows[0][6].ToString();
                    txt_m2.Text = _Datos.Tables[0].Rows[0][7].ToString();
                    cbo_tam.Text = _Datos.Tables[0].Rows[0][8].ToString();
                    cbo_cadena.Text = _Datos.Tables[0].Rows[0][9].ToString();

                    txt_folio.Enabled = false;
                    btn_aceptar.Text = "Actualizar";

                    try
                    {
                        String _Id = _S._Get_Id_Sala_MCadem(txt_folio.Text);
                        String _Id_Pag = _U._Get_Id_Pagina(Request.Url.Segments[Request.Url.Segments.Length - 1]);
                        _G._Set_Insert_Logs(Session["Id_Usuario"].ToString(), DateTime.Now.ToString("yyyy-MM-dd H:mm:ss"), "BUSQUEDA", _Id_Pag, _Id, "");
                    }
                    catch (Exception) { }
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "Maestro Sala", "swal('FOLIO no encontrado', 'No se han encontrado datos con FOLIO ingresado', 'warning');", true);
                }
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "Maestro Sala", "swal('Campo vacio', 'El campo folio no pueder estar vacio', 'warning');", true);

            }
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Acceso", "swal('Denegado', 'No tienes privilegios para realizar esta accion', 'error');", true);
        }
    }


    //Inserta nueva sala
    public void _Agregar() 
    {
        //*************** VALIDA SI EXISTE 'FOLIO' ****************
        int _Existe = _S._Existe_Sala(txt_folio.Text);
        if (_Existe == 0)
        {
            //*************** VALIDA SI EXISTE LLAVE 'DIRECCION-COMUNA' ****************
            _Existe = _S._Existe_Direccion(txt_direccion.Text, cbo_comuna.Text,txt_folio.Text);
            if (_Existe == 1) 
            {
                ClientScript.RegisterStartupScript(this.GetType(), "Maestro Sala", " swal('YA EXISTE', 'YA EXISTE LA DIRECCION INGRESADA', 'error');", true);  
            }
            else if (_Existe == 2) 
            {
                ClientScript.RegisterStartupScript(this.GetType(), "Maestro Sala", " swal('ERROR DE CONEXION', 'NO SE PUEDE CONECTAR CON EL SERVIDOR', 'error');", true);  
            }
            else if (_Existe == 0)
            {
                //***************** INGRESA SALA *******************
                Boolean _Status = _S._Insert_Sala(txt_folio.Text, txt_direccion.Text.ToUpper(), cbo_comuna.SelectedValue, txt_lat.Text, txt_lon.Text, cbo_canal.SelectedValue, txt_m2.Text, cbo_cadena.SelectedValue, cbo_tam.SelectedValue);
                if (_Status)
                {
                    try
                    {
                        String _Id = _S._Get_Id_Sala_MCadem(txt_folio.Text);
                        String _Id_Pag = _U._Get_Id_Pagina(Request.Url.Segments[Request.Url.Segments.Length - 1]);
                        _G._Set_Insert_Logs(Session["Id_Usuario"].ToString(), DateTime.Now.ToString("yyyy-MM-dd H:mm:ss"), "BUSQUEDA", _Id_Pag, _Id, "");
                    }
                    catch (Exception)
                    {}

                    ClientScript.RegisterStartupScript(this.GetType(), "Maestro Sala", "swal('NUEVA SALA', 'SALA INGRESADA CORRECTAMENTE', 'success');", true);
                    _Clear_Items();
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "Maestro Sala", " swal('ERROR', 'NO SE HA INGRESADO LA SALA', 'error');", true);
                }
                txt_folio.Enabled = true;
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "Maestro Sala", " swal('ERROR DESCONOCIDO', 'OPS!! A OCURRIDO UN ERROR INESPERADO', 'error');", true);
            }
        }
        else if (_Existe == 2) 
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Maestro Sala", "swal('ERROR DE CONEXION', 'NO SE PUEDE CONECTAR CON EL SERVIDOR', 'error');", true);
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Maestro Sala", "swal('YA EXISTE', 'YA EXISTE ESTA SALA, NO SE PUEDE INGRESAR', 'error');", true);
        }

    }


    //Actualiza sala solicitada
    public void _Actualizar() 
    {
        //*************** VALIDA SI EXISTE LLAVE 'DIRECCION-COMUNA' ****************
        int _Existe = _S._Existe_Direccion(txt_direccion.Text, cbo_comuna.Text,txt_folio.Text);
        if (_Existe == 0)
        {
            // ********** ACTUALIZA REGISTROS *****************
            Boolean _Status = _S._Set_Update_Sala(txt_folio.Text, txt_direccion.Text.ToUpper(), cbo_comuna.Text, txt_lat.Text, txt_lon.Text, cbo_canal.Text, txt_m2.Text,cbo_cadena.Text, cbo_tam.Text);
            if (_Status)
            {
                try
                {
                    String _Id = _S._Get_Id_Sala_MCadem(txt_folio.Text);
                    String _Id_Pag = _U._Get_Id_Pagina(Request.Url.Segments[Request.Url.Segments.Length - 1]);
                    _G._Set_Insert_Logs(Session["Id_Usuario"].ToString(), DateTime.Now.ToString("yyyy-MM-dd H:mm:ss"), "ACTUALIZACION", _Id_Pag, _Id, "");
                }
                catch (Exception) { }

                ClientScript.RegisterStartupScript(this.GetType(), "Maestro Salas", "swal('Actualizacion', 'Los datos se han actualizado correctamente', 'success');", true);
                _Clear_Items();
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "Maestro Salas", " swal('Error', 'Los datos no se han actualizado', 'error');", true);
            }
            txt_folio.Enabled = true;
            txt_folio.Focus();
        }
        else if (_Existe == 2)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Maestro Sala", "swal('ERROR DE CONEXION', 'NO SE PUEDE CONECTAR CON EL SERVIDOR', 'error');", true);
        }
        else if (_Existe == 1)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Maestro Sala", "swal('YA EXISTE', 'YA EXISTE ESTA DIRECCION, NO SE PUEDE ACTUALIZAR', 'error');", true);
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Maestro Sala", "swal('ERROR DESCONOCIDO', 'OPS!!! A OCURRIDO UN ERROR DESCONOCIDO', 'error');", true);
        }
    }

    
    //Limpia todos los textos....
    public void _Clear_Items()
    {
        txt_folio.Text = "";
        txt_direccion.Text = "";
        txt_lat.Text = "";
        txt_lon.Text = "";
        txt_m2.Text = "";
        //cbo_canal.Text = "";
        //cbo_comuna.Text = "";
        //cbo_tam.Text = "";
        //cbo_cadena.Text = "";
        btn_aceptar.Text = "Agregar";
    }



    //Accessos a paginas...

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



    protected void btn_cancelar_Click(object sender, EventArgs e)
    {
        txt_folio.Enabled = true;
        txt_direccion.Text = "";
        txt_folio.Text="";
        txt_lat.Text = "";
        txt_lon.Text = "";
        txt_m2.Text = "";
        btn_aceptar.Text = "Agregar";
        //cbo_comuna.Text = "";
        //cbo_cadena.Text = "";
        //cbo_canal.Text = "";
        //cbo_tam.Text = "";
    }
  
    protected void btn_est_sala_Click(object sender, EventArgs e)
    {
        Boolean _Acceso = _U._Tiene_Acceso_Pagina((String)Session["Rut"], "ESTUDIOSALA.aspx");
        if (!_Acceso)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Acceso", "swal('Denegado', 'No tienes privilegios para realizar esta accion', 'error');", true);
            return;
        } 
        Response.Redirect("Mantenedores/Estudiosala.aspx");
    }


   
    protected void btn_addcomuna_Click(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(), "Acceso", "swal('Denegado', 'NO OPERATIVO', 'error');", true);
        
        //Boolean _Acceso = _U._Tiene_Acceso_Pagina((String)Session["Rut"], "COMUNA.aspx");
        //if (!_Acceso)
        //{
        //    ClientScript.RegisterStartupScript(this.GetType(), "Acceso", "swal('Denegado', 'No tienes privilegios para realizar esta accion', 'error');", true);
        //    return;
        //}
        //Response.Redirect("Mantenedores/Comuna.aspx");
    }
}

