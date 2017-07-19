using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Solicitud_Pass : System.Web.UI.Page
{
    Solicitud_Controller _S = new Solicitud_Controller();

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btn_solicitar_Click(object sender, EventArgs e)
    {
        Boolean _Success = _S._Get_Solicita_nueva_Pass(txt_email.Text,txt_msj.Text);
    }

    protected void btn_cancelar_Click(object sender, EventArgs e)
    {
        Response.Redirect("Login.aspx");
    }
}