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



public partial class Trayectos : System.Web.UI.Page
{
    Generico_Controller _G = new Generico_Controller();
    Usuario_Controller _U = new Usuario_Controller();

    Trayectos_Controller _T = new Trayectos_Controller();

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
            Cache["LECTURA_TRAYECTO"] = _U._Get_Roles_Pagina((String)Session["Rut"], Request.Url.Segments[Request.Url.Segments.Length - 1], "LECTURA");
            Cache["ESCRITURA_TRAYECTO"] = _U._Get_Roles_Pagina((String)Session["Rut"], Request.Url.Segments[Request.Url.Segments.Length - 1], "ESCRITURA");
            Cache["EXPORTACION_TRAYECTO"] = _U._Get_Roles_Pagina((String)Session["Rut"], Request.Url.Segments[Request.Url.Segments.Length - 1], "EXPORTACION");
            Cache["ELIMINACION_TRAYECTO"] = _U._Get_Roles_Pagina((String)Session["Rut"], Request.Url.Segments[Request.Url.Segments.Length - 1], "ELIMINAR");
            Response.Cache.SetExpires(DateTime.Now.AddMinutes(60));
            //********* RESETEO DE CACHE PARA ESTA PAGINA *************//

            cbo_cluster.Items.Add("NO ESPECIFICADO");
            //cbo_cluster.Items.Add("NO URBANO");

            for (int i = 0; i <= 60; i++)
            {
                cbo_minutos_supi.Items.Add(i.ToString());
                cbo_minutos_google.Items.Add(i.ToString());
            }

            try
            {
                String _Id = _U._Get_Id_Pagina(Request.Url.Segments[Request.Url.Segments.Length - 1]);
                _G._Set_Insert_Logs(Session["Id_Usuario"].ToString(), DateTime.Now.ToString("yyyy-MM-dd H:mm:ss"), "SELECCION VISTA", _Id, "", "");
            }
            catch (Exception) { }
        }
        lbl_disponible.Visible = false;

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
        //Cache.Remove("LECTURA_EMP");
        //Cache.Remove("ESCRITURA_EMP");
        //Cache.Remove("EXPORTACION_EMP");
        //Cache.Remove("ELIMINACION_EMP");
        Cache.Remove("Usuario");
        Cache.Remove("Id_Usuario");

        FormsAuthentication.SignOut();
        Response.Redirect(FormsAuthentication.LoginUrl);  
    }




    protected void btn_aceptar_Click(object sender, EventArgs e)
    {
        if ((String)Cache["ESCRITURA_TRAYECTO"] == "1")
        {
            if (btn_aceptar.Text == "Agregar")
            {
                txt_dist_google.Text = txt_dist_google.Text.Replace(",", ".");
                DataSet _Ds = _T._Exista_Sala_MCadem(txt_folio_ini.Text);
                if (_Ds.Tables[0].Rows.Count > 0)
                {
                    _Ds = _T._Exista_Sala_MCadem(txt_folio_fin.Text);
                    if (_Ds.Tables[0].Rows.Count > 0)
                    {
                        String _T_GG = cbo_horas_google.Text + ":" + cbo_minutos_google.Text;
                        String _T_SUPI = cbo_horas_supi.Text + ":" + cbo_minutos_supi.Text;
                        Boolean _Success = _T._Insert_Tarecto(txt_folio_ini.Text, txt_folio_fin.Text, txt_dist_google.Text, cbo_cluster.Text, _T_GG, _T_SUPI);
                        if (_Success)
                        {
                            _LImpia_textos();
                            btn_aceptar.Text = "Agregar";
                            ClientScript.RegisterStartupScript(this.GetType(), "Acceso", "swal('Nuevo trayecto', 'Se han agregado el nuevo trayecto', 'success');", true);

                        }
                        else
                        {
                            ClientScript.RegisterStartupScript(this.GetType(), "Acceso", "swal('Error de RED', 'No se puede conectar con el servidor', 'error');", true);

                        }
                    }
                    else
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "Acceso", "swal('Error de RED', 'No se puede conectar con el servidor', 'error');", true);
                    }
                }
            }
            else
            {
                String _T_GG = cbo_horas_google.Text + ":" + cbo_minutos_google.Text;
                String _T_SUPI = cbo_horas_supi.Text + ":" + cbo_minutos_supi.Text;
                Boolean _Success = _T._Update_Tarecto(txt_folio_ini.Text, txt_folio_fin.Text, txt_dist_google.Text, cbo_cluster.Text, _T_GG, _T_SUPI);
                if (_Success)
                {
                    _LImpia_textos();
                    btn_aceptar.Text = "Agregar";
                    ClientScript.RegisterStartupScript(this.GetType(), "Acceso", "swal('Actualizacion', 'Se han actualizado los trayectos', 'success');", true);
                    txt_folio_ini.Focus();
                }
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "Acceso", "swal('Error de RED', 'No se puede conectar con el servidor', 'error');", true);
                }
            }
        }
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Acceso", "swal('Denegado', 'No tienes privilegios para realizar esta accion', 'error');", true);

        }
    }
   

  
    protected void btn_exportar_Click(object sender, EventArgs e)
    {
        if ((String)Cache["EXPORTACION_TRAYECTO"] == "1")
        {
            DataSet _Ds = _T._Exportar_Trayecto();
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

    protected void script1(object sender, EventArgs e) 
    {
        if ((String)Cache["LECTURA_TRAYECTO"] == "1")
        {
            DataSet _Datos = _T._Get_Datos_Trayecto(txt_folio_ini.Text, txt_folio_fin.Text);
            if (_Datos.Tables[0].Rows.Count > 0)
            {
                if (_Datos != null)
                {
                    String _H_G = _Datos.Tables[0].Rows[0][2].ToString();
                    String _H_S = _Datos.Tables[0].Rows[0][4].ToString();

                    if (_H_G.Length == 1) _H_G = "0" + _H_G;
                    if (_H_S.Length == 1) _H_S = "0" + _H_S;

                    txt_dist_google.Text = _Datos.Tables[0].Rows[0][0].ToString();
                    cbo_cluster.Text = _Datos.Tables[0].Rows[0][1].ToString().Trim();
                    cbo_horas_google.Text = _H_G;
                    cbo_minutos_google.Text = _Datos.Tables[0].Rows[0][3].ToString(); ;
                    cbo_horas_supi.Text = _H_S;
                    cbo_minutos_supi.Text = _Datos.Tables[0].Rows[0][5].ToString(); ;

                    btn_aceptar.Text = "Actualizar";
                    lbl_disponible.Visible = false;
                }
            }
            else
            {
                btn_aceptar.Text = "Agregar";
                txt_folio_fin.Enabled = false;
                txt_folio_ini.Enabled = false;
                lbl_disponible.Visible = true;
            }
        }
        {
            //ClientScript.RegisterStartupScript(this.GetType(), "Acceso", "swal('Denegado', 'No tienes privilegios para realizar esta accion', 'error');", true);

        }
    }

    public void _LImpia_textos() {
        
        txt_dist_google.Text = "";
        txt_folio_fin.Text = "";
        txt_folio_ini.Text = "";
        cbo_horas_google.Text = "00";
        cbo_minutos_google.Text = "0";
        cbo_horas_supi.Text = "00";
        cbo_minutos_supi.Text = "0";
        txt_folio_fin.Enabled = true;
        txt_folio_ini.Enabled = true;
        txt_folio_ini.Focus();
    }






    protected void btn_go_menu_Click(object sender, EventArgs e)
    {
        Response.Redirect("Menu.aspx");
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

    protected void btn_go_salas_Click(object sender, EventArgs e)
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

}