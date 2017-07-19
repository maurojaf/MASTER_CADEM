using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.Security;

public partial class Mantenedores_PermisosUsuario : System.Web.UI.Page
{
    Generico_Controller _G = new Generico_Controller();
    Usuario_Controller _U = new Usuario_Controller();

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
            //Valida si el usuario logeado se o no admin
            if ((String)Session["Rut"] == null || (String)Session["Rut"] == "")
            {
                Response.Redirect("~/Login.aspx");
            }
            else
            {
                Boolean _Admin = _U._Get_Admin((String)Session["Rut"]);
                if (!_Admin)
                {
                    Response.Redirect("~/Menu.aspx");
                    return;
                }              
            }

            lbl_session.Text = (String)Session["Usuario"];

            DataSet _Ds_Coordinador = _U._Get_Todos_Usuarios();
            if (_Ds_Coordinador.Tables[0].Rows.Count > 0)
            {
                cbo_usuario.DataSource = _Ds_Coordinador.Tables[0];
                cbo_usuario.DataValueField = _Ds_Coordinador.Tables[0].Columns[1].ToString();
                cbo_usuario.DataTextField = _Ds_Coordinador.Tables[0].Columns[0].ToString();
                cbo_usuario.DataBind();
            }
        }
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
        //Cache.Remove("LECTURA");
        //Cache.Remove("ESCRITURA");
        //Cache.Remove("EXPORTACION");
        //Cache.Remove("ELIMINACION");
        Cache.Remove("Usuario");
        Cache.Remove("Id_Usuario");

        FormsAuthentication.SignOut();
        Response.Redirect(FormsAuthentication.LoginUrl); 
    }

    protected void btn_volver_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/MenuAdmin.aspx");
    }

    protected void btn_cancelar_Click(object sender, EventArgs e)
    {
        dgw_permisos.DataSource = null;
        dgw_permisos.DataBind();
        chk_acceso_sistema.Checked = false;
        txt_pass.Text = "";
        cbo_usuario.SelectedIndex = 0;
        chk_acceso_sistema.Visible = false;
        chk_admin.Visible = false;
        chk_panel_control.Visible = false;
    }

    protected void btn_actualizar_Click(object sender, EventArgs e)
    {
        int i = -1;
        Boolean _Pasa ;
        String Id_User = cbo_usuario.SelectedValue;
        foreach (GridViewRow row in dgw_permisos.Rows)
        {
            i++;
            CheckBox sup = (CheckBox)dgw_permisos.Rows[i].FindControl("chk_acceso");
            String _Acceso = sup.Checked.ToString();
            sup = (CheckBox)dgw_permisos.Rows[i].FindControl("chk_lectura");
            String _Lectura = sup.Checked.ToString();
            sup = (CheckBox)dgw_permisos.Rows[i].FindControl("chk_escritura");
            String _Escritura = sup.Checked.ToString();
            sup = (CheckBox)dgw_permisos.Rows[i].FindControl("chk_exportar");
            String _Exportar = sup.Checked.ToString();
            sup = (CheckBox)dgw_permisos.Rows[i].FindControl("chk_eliminar");
            String _Eliminar = sup.Checked.ToString();
            Label _Pag = (Label)dgw_permisos.Rows[i].FindControl("lbl_id");
            String _Id_Pag = _Pag.Text;         
            _Pasa = _U._Get_Asigna_Roles(_Id_Pag, Id_User, _Acceso, _Lectura, _Escritura, _Exportar, _Eliminar);

            if (!_Pasa) 
            {
                ClientScript.RegisterStartupScript(this.GetType(), "Actualizacion Roles", "swal('Error', 'HUBO EN ERROR AL ASIGNAR ROLES, FAVOR INTENTA DENUEVO', 'error');", true);    
                return;
            }
        }


        if (i == -1)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Actualizacion Rol", "swal('Error', 'Realizar busqueda empleado', 'error');", true);
            return;
        }
        else 
        {
            Boolean _updatepass_usuario = false;
            if (txt_pass.Text != "")  _updatepass_usuario = true;
             
            Boolean _Existe_en_panelcontrol = _U._Existe_usuario_panelControl(cbo_usuario.SelectedValue.ToString());
            if (chk_panel_control.Checked)
            {
                if (!_Existe_en_panelcontrol)
                {
                    // **** SE CREA ACCESO PANEL DE CONTROL SUPI ****
                    _U._Nuevo_UsuarioPanelControl_Supi(cbo_usuario.SelectedValue.ToString(), true, _updatepass_usuario);
                }
            }
            else 
            { 
                if (_Existe_en_panelcontrol)
                {
                    // **** elimina ACCESO PANEL DE CONTROL SUPI ****
                    _U._Elimina_UsuarioPanelControl_Supi(cbo_usuario.SelectedValue.ToString());
                }
            }

            // *** OTORGAR ACCESO WEB ****
            Boolean _Success2 = _U._Set_Acceso_Web(cbo_usuario.SelectedValue, chk_acceso_sistema.Checked.ToString());


            if (txt_pass.Text != "")
            {
                // *** SETEAR PASS ****
                Boolean _Success = _U._Set_Resetar_Pass(cbo_usuario.SelectedValue, txt_pass.Text, chk_panel_control.Checked);
            }

            if (chk_admin.Checked)
            {
                // **** OTORGAR ADMIN WEB ****
                Boolean _Success3 = _U._Set_Acceso_Admin(Id_User);
            }
        }

        ClientScript.RegisterStartupScript(this.GetType(), "Actualizacion Rol", "swal('Actualizacion', 'Se han asignados los nuevos roles', 'success');", true);                
        btn_cancelar_Click(null, null);      
    }

    protected void cbo_usuario_TextChanged(object sender, EventArgs e)
    {
        String _User_Id = cbo_usuario.SelectedValue;
        DataSet _Roles = _U._Get_Todos_Roles(_User_Id);
        if (_User_Id != "0")
        {
            dgw_permisos.DataSource = _Roles.Tables[0];
            dgw_permisos.DataBind();

            txt_pass.TextMode = TextBoxMode.Password;
            chk_acceso_sistema.Checked = _U._Acceso_Sistema_MCADEM(cbo_usuario.Text);
            chk_panel_control.Checked = _U._Acceso_Sistema_panel_control(cbo_usuario.SelectedValue.ToString());

            chk_acceso_sistema.Visible = true;
            chk_admin.Visible = true;
            chk_panel_control.Visible = true;




        }else{
            btn_cancelar_Click(null,null);
        }
    }

    protected void dgw_permisos_DataBound(object sender, EventArgs e)
    {

    }
}