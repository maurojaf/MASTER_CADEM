using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Collections;
using System.Data;

public partial class Menu : System.Web.UI.Page
{
    Usuario_Controller _U = new Usuario_Controller();
    Generico_Controller _G = new Generico_Controller();
    Empleado_Controller _E = new Empleado_Controller();

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
            Cache["LECTURA_EMP"] = _U._Get_Roles_Pagina((String)Session["Rut"], Request.Url.Segments[Request.Url.Segments.Length - 1], "LECTURA");
            Cache["ESCRITURA_EMP"] = _U._Get_Roles_Pagina((String)Session["Rut"], Request.Url.Segments[Request.Url.Segments.Length - 1], "ESCRITURA");
            Cache["EXPORTACION_EMP"] = _U._Get_Roles_Pagina((String)Session["Rut"], Request.Url.Segments[Request.Url.Segments.Length - 1], "EXPORTACION");
            Cache["ELIMINACION_EMP"] = _U._Get_Roles_Pagina((String)Session["Rut"], Request.Url.Segments[Request.Url.Segments.Length - 1], "ELIMINAR");
            Response.Cache.SetExpires(DateTime.Now.AddMinutes(60));
            //********* RESETEO DE CACHEO PARA ESTA PAGINA *************//

            _Llena_ComboBox();
            txt_rut.Focus();

            dgw_empleados.DataSource = _E._Get_Grilla_Emeplado();
            dgw_empleados.DataBind();
           
            try
            {
                String _Id = _U._Get_Id_Pagina(Request.Url.Segments[Request.Url.Segments.Length - 1]);
                _G._Set_Insert_Logs(Session["Id_Usuario"].ToString(), DateTime.Now.ToString("yyyy-MM-dd H:mm:ss"), "SELECCION VISTA", _Id, "", "");
            }
            catch (Exception) { }
        }
    }
   
    //Agregar / actualiza empleado...
    protected void btn_aceptar_Click(object sender, EventArgs e)
    {
        if ((String)Cache["ESCRITURA_EMP"]=="1")
        {
            if (btn_aceptar.Text == "Agregar") 
            {
                String _Existe = _E._Existe_Empleado(txt_rut.Text);
                if (_Existe == "0")
                {
                    String _Status_Nuevo_empleado = _E._Set_Insert_Empleado(txt_rut.Text, txt_nombre.Text.ToUpper(), txt_paterno.Text.ToUpper(), text_materno.Text.ToUpper(), cbo_cargo.Text.ToUpper(), cbo_clasificacion.Text, cbo_estado.Text, cbo_comuna.Text, cbo_jornada.Text, txt_fecha.Text, cbo_finanzas.Text, cbo_coordinador.SelectedValue.ToString(), cbo_grupo.Text, txt_fono.Text, txt_email.Text.ToUpper(), txt_mail_personal.Text, cbo_nivel.Text);
                    if (_Status_Nuevo_empleado == "Success")
                    {
                        try
                        {
                            String _Id = _E._Get_Id_User(txt_rut.Text);
                            String _Id_Pag = _U._Get_Id_Pagina(Request.Url.Segments[Request.Url.Segments.Length - 1]);
                            _G._Set_Insert_Logs(Session["Id_Usuario"].ToString(), DateTime.Now.ToString("yyyy-MM-dd H:mm:ss"), "NUEVO", _Id_Pag, _Id, "");
                        }
                        catch (Exception ) { }

                        _Asignar_Roles();
                        DataSet _Ds_Coordinador = _E._Carga_Encargado();
                        cbo_coordinador.DataSource = _Ds_Coordinador.Tables[0];
                        cbo_coordinador.DataValueField = _Ds_Coordinador.Tables[0].Columns[1].ToString();
                        cbo_coordinador.DataTextField = _Ds_Coordinador.Tables[0].Columns[0].ToString();
                        cbo_coordinador.DataBind();
                        _Clear_Items();
                        ClientScript.RegisterStartupScript(this.GetType(), "Maestro Empleado", "swal('Nuevo empleado', 'Empleado ingresados correctamente', 'success');", true);
                    }
                    else
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "Maestro Empleado", " swal('Error', '" + _Status_Nuevo_empleado.Replace("'", "") + "' , 'error');", true);
                    }
                    txt_rut.Enabled = true;
                }
                else if (_Existe == "2")
                {
                    //Error de conexion 
                    ClientScript.RegisterStartupScript(this.GetType(), "Maestro Empleado", "swal('ERROR DE CONEXION', 'NO SE PUEDE CONECTAR CON EL SERVIDOR', 'error');", true);
                    txt_rut.Focus();
                }
                else
                {
                    //Si ya existe Rut, no se ingresara...
                    ClientScript.RegisterStartupScript(this.GetType(), "Maestro Empleado", "swal('Advertencia', 'Ya existe este RUT en la BD, favor ingresar otro RUT', 'warning');", true);
                    txt_rut.Focus();
                }  
            } 
            else 
            {
                String _Status = _E._Set_Update_Empleado(lbl_id.Text, txt_rut.Text, txt_nombre.Text.ToUpper(), txt_paterno.Text.ToUpper(), text_materno.Text.ToUpper(), cbo_cargo.Text, cbo_clasificacion.Text, cbo_estado.Text, cbo_comuna.Text, cbo_jornada.Text, txt_fecha.Text.ToUpper(), cbo_finanzas.Text, cbo_coordinador.SelectedValue.ToString(), cbo_grupo.Text, txt_fono.Text, txt_email.Text.ToUpper(), txt_mail_personal.Text, cbo_nivel.Text);
                if (_Status == "Success")
                {
                    btn_aceptar.Text = "Agregar";
                    cbo_coordinador.SelectedValue = null;

                    try
                    {
                        String _Id = _E._Get_Id_User(txt_rut.Text);
                        String _Id_Pag = _U._Get_Id_Pagina(Request.Url.Segments[Request.Url.Segments.Length - 1]);
                        _G._Set_Insert_Logs(Session["Id_Usuario"].ToString(), DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"), "ACTUALIZACION", _Id_Pag, _Id, "");
                    }
                    catch (Exception ) { }

                    //btn_eliminar.Visible = false;
                    _Clear_Items();
                    ClientScript.RegisterStartupScript(this.GetType(), "Maestro Empleado", "swal('Actualizacion', 'Los datos se han actualizado correctamente', 'success');", true);
                }
                else
                {
                    String _Msj = _Status;
                    ClientScript.RegisterStartupScript(this.GetType(), "Maestro Empleado", " swal('Error', '" + _Status + "' , 'error');", true);
                }
                txt_rut.Enabled = true;   
            }

            dgw_empleados.DataSource = _E._Get_Grilla_Emeplado();
            dgw_empleados.DataBind();
        }
        else
        {
             ClientScript.RegisterStartupScript(this.GetType(), "Acceso", "swal('Denegado', 'No tienes privilegios para realizar esta accion', 'error');", true); 
        }
    }
    
    //Cierra Session...
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
        //Cache.Remove("LECTURA_EMP");
        //Cache.Remove("ESCRITURA_EMP");
        //Cache.Remove("EXPORTACION_EMP");
        //Cache.Remove("ELIMINACION_EMP");
        Cache.Remove("Usuario");
        Cache.Remove("Id_Usuario");

        FormsAuthentication.SignOut();
        Response.Redirect(FormsAuthentication.LoginUrl);  
    }
  
    //Buscar empleado...
    protected void script1(object sender, EventArgs e)
    {
        if ((String)Cache["LECTURA_EMP"]=="1")
        {
            _Buscar();
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Acceso", "swal('Denegado', 'No tienes privilegios de acceso a esta pagina', 'error');", true);
        }
    }

    //Busca empleado...
    public void _Buscar() 
    {
        if (txt_rut.Text != "")
        {
            //txt_fecha.Text = DateTime.Now.ToString("yyyy-MM-dd");          
            DataSet _Datos = _E._Get_Datos_Empleado(txt_rut.Text);

            if (_Datos != null)
            {
                lbl_id.Text = _Datos.Tables[0].Rows[0][0].ToString(); // _Datos[0].ToString(); 
                txt_rut.Text =  _Datos.Tables[0].Rows[0][1].ToString(); //_Datos[1].ToString();              
                txt_nombre.Text = _Datos.Tables[0].Rows[0][2].ToString(); //_Datos[2].ToString();            
                txt_paterno.Text = _Datos.Tables[0].Rows[0][3].ToString(); //_Datos[3].ToString();          
                text_materno.Text = _Datos.Tables[0].Rows[0][4].ToString(); //_Datos[4].ToString(); 

                cbo_cargo.SelectedValue = _Datos.Tables[0].Rows[0][5].ToString(); //_Datos[5].ToString();
                cbo_clasificacion.SelectedValue = _Datos.Tables[0].Rows[0][6].ToString(); //_Datos[6].ToString();
                cbo_estado.SelectedValue = _Datos.Tables[0].Rows[0][7].ToString(); //_Datos[7].ToString();
                cbo_comuna.SelectedValue = _Datos.Tables[0].Rows[0][8].ToString(); //_Datos[8].ToString();
                cbo_jornada.SelectedValue = _Datos.Tables[0].Rows[0][9].ToString(); //_Datos[9].ToString();
                txt_fecha.Text = _Datos.Tables[0].Rows[0][10].ToString(); //_Datos[10].ToString();
                cbo_finanzas.SelectedValue = _Datos.Tables[0].Rows[0][11].ToString(); //_Datos[11].ToString();
                cbo_grupo.SelectedValue = _Datos.Tables[0].Rows[0][13].ToString(); //_Datos[13].ToString();
                txt_fono.Text = _Datos.Tables[0].Rows[0][14].ToString(); //_Datos[14].ToString();
                txt_email.Text = _Datos.Tables[0].Rows[0][15].ToString(); // _Datos[15].ToString();
                txt_mail_personal.Text = _Datos.Tables[0].Rows[0][16].ToString(); //_Datos[16].ToString();
                cbo_nivel.SelectedValue = _Datos.Tables[0].Rows[0][17].ToString(); //_Datos[17].ToString();   
                cbo_coordinador.SelectedValue = _Datos.Tables[0].Rows[0][18].ToString(); //_Datos[18].ToString();

                txt_rut.Enabled = false;
                btn_aceptar.Text = "Actualizar";
            
                try
                {
                    String _Id = _E._Get_Id_User(txt_rut.Text);
                    String _Id_Pag = _U._Get_Id_Pagina(Request.Url.Segments[Request.Url.Segments.Length - 1]);
                    Boolean _T = _G._Set_Insert_Logs(Session["Id_Usuario"].ToString(), DateTime.Now.ToString("yyyy-MM-dd H:mm:ss"), "BUSQUEDA", _Id_Pag, _Id, "");
                }
                catch (Exception) { }
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "Maestro Empleado", "swal('Rut no encontrado', 'No se han encontrado datos con RUT ingresado', 'warning');", true);
                txt_rut.Enabled = true;
                txt_fecha.Text = "";
            }
        }
        else 
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Maestro Empleado", "swal('Campo vacio', 'El campo RUT no puedes estar vacio', 'warning');", true);              
            txt_rut.Enabled = true;
        }
        //btn_eliminar.Visible = false;
    }

 



    //Asigna roles a usuario actual...
    public void _Asignar_Roles()
    {
        _U._Insert_Acceso_Sistema(txt_rut.Text, "SISTEMA", "ACCESO SISTEMA", false);
    }

    //Limpiar contenedors...
    public void _Clear_Items()
    {
        txt_rut.Text = "";
        txt_nombre.Text = "";
        txt_paterno.Text = "";
        text_materno.Text = "";
        txt_fecha.Text = "";
        txt_email.Text = "";
        txt_fecha.Text = "";
        txt_email.Text = "";
        txt_fono.Text = "";
        txt_mail_personal.Text = "";

        try
        {
            cbo_nivel.SelectedValue = "0";
            cbo_cargo.SelectedValue = "0";
            cbo_clasificacion.SelectedValue = "0";
            cbo_comuna.SelectedValue = "0";
            cbo_estado.SelectedValue = "0";
            cbo_finanzas.SelectedValue = "0";
            cbo_grupo.SelectedValue = "0";
            cbo_jornada.SelectedValue = "0";
        }
        catch (Exception)
        {
            cbo_nivel.SelectedValue = null;
            cbo_cargo.SelectedValue = null;
            cbo_clasificacion.SelectedValue = null;
            cbo_comuna.SelectedValue = null;
            cbo_estado.SelectedValue = null;
            cbo_finanzas.SelectedValue = null;
            cbo_grupo.SelectedValue = null;
            cbo_jornada.SelectedValue = null;
        }
        //cbo_coordinador.Text = "";

        txt_rut.Focus();

    }

    //Carga ComboBox...
    public void _Llena_ComboBox()
    {
        DataSet _Ds = _E._Carga_Cargo();
        cbo_cargo.DataSource = _Ds.Tables[0];
        cbo_cargo.DataValueField = _Ds.Tables[0].Columns[1].ToString();
        cbo_cargo.DataTextField = _Ds.Tables[0].Columns[0].ToString();
        cbo_cargo.DataBind();

        _Ds = _E._Carga_Clasificacion();
        cbo_clasificacion.DataSource = _Ds.Tables[0];
        cbo_clasificacion.DataValueField = _Ds.Tables[0].Columns[1].ToString();
        cbo_clasificacion.DataTextField = _Ds.Tables[0].Columns[0].ToString();
        cbo_clasificacion.DataBind();

        _Ds = _E._Carga_EstadoEmpleado();
        cbo_estado.DataSource = _Ds.Tables[0];
        cbo_estado.DataValueField = _Ds.Tables[0].Columns[1].ToString();
        cbo_estado.DataTextField = _Ds.Tables[0].Columns[0].ToString();
        cbo_estado.DataBind();

        _Ds = _E._Carga_Grupo();
        cbo_grupo.DataSource = _Ds.Tables[0];
        cbo_grupo.DataValueField = _Ds.Tables[0].Columns[1].ToString();
        cbo_grupo.DataTextField = _Ds.Tables[0].Columns[0].ToString();
        cbo_grupo.DataBind();

        _Ds = _E._Carga_ClFinanzas();
        cbo_finanzas.DataSource = _Ds.Tables[0];
        cbo_finanzas.DataValueField = _Ds.Tables[0].Columns[1].ToString();
        cbo_finanzas.DataTextField = _Ds.Tables[0].Columns[0].ToString();
        cbo_finanzas.DataBind();

        _Ds = _E._Carga_Comuna();
        cbo_comuna.DataSource = _Ds.Tables[0];
        cbo_comuna.DataValueField = _Ds.Tables[0].Columns[1].ToString();
        cbo_comuna.DataTextField = _Ds.Tables[0].Columns[0].ToString();
        cbo_comuna.DataBind();

        _Ds = _E._Carga_Jornada();
        cbo_jornada.DataSource = _Ds.Tables[0];
        cbo_jornada.DataValueField = _Ds.Tables[0].Columns[1].ToString();
        cbo_jornada.DataTextField = _Ds.Tables[0].Columns[0].ToString();
        cbo_jornada.DataBind();

        _Ds = _E._Carga_Encargado();
        cbo_coordinador.DataSource = _Ds.Tables[0];
        cbo_coordinador.DataValueField = _Ds.Tables[0].Columns[1].ToString();
        cbo_coordinador.DataTextField = _Ds.Tables[0].Columns[0].ToString();
        cbo_coordinador.DataBind();

        _Ds = _E._Carga_Nivel();
        cbo_nivel.DataSource = _Ds.Tables[0];
        cbo_nivel.DataValueField = _Ds.Tables[0].Columns[1].ToString();
        cbo_nivel.DataTextField = _Ds.Tables[0].Columns[0].ToString();
        cbo_nivel.DataBind();

        cbo_coordinador.Text = "";
        cbo_jornada.Text = "";
        cbo_grupo.Text = "";
        cbo_finanzas.Text = "";
        cbo_estado.Text = "";
        cbo_comuna.Text = "";
        cbo_clasificacion.Text = "";
        cbo_cargo.Text = "";
        cbo_nivel.Text = "";
    }



    //Accessos a paginas...

    protected void btn_go_salas_Click(object sender, EventArgs e)
    {
        Boolean _Acceso = _U._Tiene_Acceso_Pagina((String)Session["Rut"], "Salas.aspx");
        if (_Acceso) {
            Response.Redirect("Salas.aspx");
        }else {
            ClientScript.RegisterStartupScript(this.GetType(), "Acceso", "swal('Denegado', 'No tienes privilegios de acceso a esta pagina', 'error');", true);                  
        }
    }

    protected void btn_go_logistica_Click(object sender, EventArgs e)
    {
        Boolean _Acceso = _U._Tiene_Acceso_Pagina((String)Session["Rut"], "Logistica.aspx");
        if (_Acceso){
            Response.Redirect("Logistica.aspx");
        }else{
            ClientScript.RegisterStartupScript(this.GetType(), "Acceso", "swal('Denegado', 'No tienes privilegios de acceso a esta pagina', 'error');", true);
        }
    }

    protected void btn_go_estudio_Click(object sender, EventArgs e)
    {
        Boolean _Acceso = _U._Tiene_Acceso_Pagina((String)Session["Rut"], "Estudios.aspx");
        if (_Acceso){
            Response.Redirect("Estudios.aspx");
        } else{
            ClientScript.RegisterStartupScript(this.GetType(), "Acceso", "swal('Denegado', 'No tienes privilegios de acceso a esta pagina', 'error');", true);
        }
    }

    protected void btn_go_menu_Click(object sender, EventArgs e)
    {
        Response.Redirect("Menu.aspx");
    }


    protected void dgw_empleados_SelectedIndexChanged(object sender, EventArgs e)
    {
        if ((String)Cache["ESCRITURA_EMP"] == "1")
        {
            txt_rut.Text = dgw_empleados.SelectedRow.Cells[2].Text;
            _Buscar();
            btn_aceptar.Text = "Actualizar";
        }
        else {
            ClientScript.RegisterStartupScript(this.GetType(), "Maestro Empleado", "swal('Advertencia', 'No tienes privilegios para realizar esta accion', 'warning');", true);                    
        }     
    }

    protected void btn_cancelar_Click(object sender, EventArgs e)
    {
        btn_aceptar.Text = "Agregar";
        txt_rut.Enabled = true;
        _Clear_Items();
    }

    protected void Button1_Click(object sender, EventArgs e)
    {

    }
    protected void dgw_empleados_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        e.Row.Cells[1].Visible = false;
    }
}