using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using Ionic.Zip;
using System.IO;
using System.Net;
using EnterpriseDT.Net.Ftp;
using System.Web.Security;
using Galleries.Controller;


public partial class Galeria : System.Web.UI.Page
{
    FTPConnection _Ftp = new FTPConnection();
    Fotos_controller _F = new Fotos_controller();
    Usuario_Controller _U = new Usuario_Controller();
    Generico_Controller _G = new Generico_Controller();

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

            //********* RESETEO DE CACHEO PARA ESTA PAGINA *************//
            Cache["LECTURA_GAL"] = _U._Get_Roles_Pagina((String)Session["Rut"], Request.Url.Segments[Request.Url.Segments.Length - 1], "LECTURA");
            Cache["ESCRITURA_GAL"] = _U._Get_Roles_Pagina((String)Session["Rut"], Request.Url.Segments[Request.Url.Segments.Length - 1], "ESCRITURA");
            Cache["EXPORTACION_GAL"] = _U._Get_Roles_Pagina((String)Session["Rut"], Request.Url.Segments[Request.Url.Segments.Length - 1], "EXPORTACION");
            Cache["ELIMINACION_GAL"] = _U._Get_Roles_Pagina((String)Session["Rut"], Request.Url.Segments[Request.Url.Segments.Length - 1], "ELIMINAR");
            Response.Cache.SetExpires(DateTime.Now.AddMinutes(60));
            //********* RESETEO DE CACHEO PARA ESTA PAGINA *************//

            DataSet _Ds_Clientes = _F._Get_Carga_Listado_Estudios();
            cbo_cliente.DataSource = _Ds_Clientes.Tables[0];
            cbo_cliente.DataValueField = _Ds_Clientes.Tables[0].Columns[1].ToString();
            cbo_cliente.DataTextField = _Ds_Clientes.Tables[0].Columns[0].ToString();
            cbo_cliente.DataBind();

            lbl_session.Text = (String)Session["Usuario"];
            cbo_cliente_SelectedIndexChanged(null, null);
      
            galleryHTML.InnerHtml = "<div class=\"textonaranjo\"> SELECCIONA UN ESTUDIO </div>";

            try
            {
                String _Id = _U._Get_Id_Pagina(Request.Url.Segments[Request.Url.Segments.Length - 1]);
                _G._Set_Insert_Logs(Session["Id_Usuario"].ToString(), DateTime.Now.ToString("yyyy-MM-dd H:mm:ss"), "SELECCION VISTA", _Id, "", "");
            } 
            catch (Exception) { }

        }
    }

    //Parametros FTP...
    public void _Parametros()
    {
        _Ftp.UserName = "administrador";
        _Ftp.Password = "smart123";
        _Ftp.ServerAddress = "200.29.139.242";
        _Ftp.ServerPort = 21;
        _Ftp.ServerDirectory = "BBDD_SUPI/";
        _Ftp.ConnectMode = FTPConnectMode.PASV;
        _Ftp.Timeout = 20000;
        _Ftp.TransferType = FTPTransferType.BINARY;
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
        //Cache.Remove("LECTURA");
        //Cache.Remove("ESCRITURA");
        //Cache.Remove("EXPORTACION");
        //Cache.Remove("ELIMINACION");
        Cache.Remove("Usuario");
        Cache.Remove("Id_Usuario");

        FormsAuthentication.SignOut();
        Response.Redirect(FormsAuthentication.LoginUrl); 
    }

    //Busca y Muestra fotos segun filtro seleccionado...
    protected void btn_filtros_Click(object sender, EventArgs e)
    {      
        //if (chk_fotos.Checked)
        //{
        //    if ((String)Cache["ELIMINACION_GAL"] == "1")
        //    {
        //        _Modo_Edicion(cbo_medicion.SelectedValue);
        //        return;
        //    }
        //    else 
        //    {
        //        ClientScript.RegisterStartupScript(this.GetType(), "Acceso", "swal('Denegado', 'No tienes privilegios para realizar esta accion', 'error');", true);
        //        return;
        //    }
        //}


        if ((String)Cache["LECTURA_GAL"] == "1")
        {
            dgw_fotos.Visible = false;
            //btn_zip_malas.Visible = false;
            if (cbo_medicion.Text != "")
            {
                System.Text.RegularExpressions.Regex _valida = new System.Text.RegularExpressions.Regex("[^0-9]");
                if (txt_folio.Text != "")
                {
                    txt_folio.Text = txt_folio.Text.Trim();
                    if (_valida.IsMatch(txt_folio.Text))
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "Acceso", "swal('FOLIO NO VALIDO', 'EL FOLIO SELECCIONADO PARACE SER NO VALIDO', 'error');", true);
                        return;
                    }
                }

                _Bucar_Fotos_Por_Medicion(cbo_medicion.SelectedValue, txt_folio.Text);
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "Acceso", "swal('SELECCIONAR FILTRO', 'SELECCIONAR FILTRO MEDICIONES', 'error');", true);
            }
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Acceso", "swal('Denegado', 'No tienes privilegios para realizar esta accion', 'error');", true);
        }
    }

    //Descarga Fotos...
    protected void btn_zip_Click(object sender, EventArgs e)
    {
        if ((String)Cache["EXPORTACION_GAL"] == "1")
        {
            //********** LOGS BUSQUEDA POR MEDICION ***********
            String _Id_Estudio = _F._Get_Id_Estudio(cbo_medicion.SelectedValue);
            try
            {
                String _Id_Pagina = _U._Get_Id_Pagina(Request.Url.Segments[Request.Url.Segments.Length - 1]);
                _G._Set_Insert_Logs(Session["Id_Usuario"].ToString(), DateTime.Now.ToString("yyyy-MM-dd H:mm:ss"), "DESCARGA FOTOS", _Id_Pagina, _Id_Estudio, "");
            }
            catch (Exception) { }

            String _Temporal = "Temp_" + DateTime.Now.ToString("ddmmyyhhmmssfff");

            if (!Directory.Exists(Server.MapPath("~/Pictures/") + _Temporal))
            {
                Directory.CreateDirectory(Server.MapPath("~/Pictures/") + _Temporal);
            }

            String[] _Todos = Directory.GetFiles(Server.MapPath("~/Pictures/") + _Temporal);
            foreach (String _Fila in _Todos)
            {
                try
                {
                    System.IO.File.Delete(_Fila);
                }
                catch (Exception)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "Fotos", "swal('Sin acceso', 'No se puede tener acceso a los archivos, otro usuario esta accediendo', 'error');", true);
                    return;
                }

            }

            ZipFile zip = new ZipFile();
            zip.AlternateEncodingUsage = ZipOption.AsNecessary;
            int _Contador = -1;

            foreach (String row in class1._Fotos)
            {
                _Contador++;
                String rtrt = Path.GetDirectoryName(row) + "/";

                String _Picture = row.Replace(rtrt, "");
                String _Folio = class1._Folio[_Contador];

                String RemoteFtpPath = "ftp://200.29.139.242/BBDD_SUPI/" + row;
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(RemoteFtpPath);
                request.Method = WebRequestMethods.Ftp.DownloadFile;
                request.KeepAlive = true;
                request.UsePassive = false;
                request.UseBinary = true;

                request.Credentials = new NetworkCredential("administrador", "smart123");
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream);

                using (FileStream writer = new FileStream(Server.MapPath("~/Pictures/") + _Temporal + "/" + rtrt.Replace("/","") + _Picture, FileMode.Create))
                {
                    long length = response.ContentLength;
                    int bufferSize = 2048;
                    int readCount;
                    byte[] buffer = new byte[2048];

                    readCount = responseStream.Read(buffer, 0, bufferSize);
                    while (readCount > 0)
                    {
                        writer.Write(buffer, 0, readCount);
                        readCount = responseStream.Read(buffer, 0, bufferSize);
                    }
                }

                reader.Close();
                response.Close();
                zip.AddFile(Server.MapPath("~/Pictures/") + _Temporal + "/" + rtrt.Replace("/", "") + _Picture, _Folio);
            }

            Response.Clear();
            Response.BufferOutput = false;
            Response.ContentType = "application/zip";
            Response.AddHeader("content-disposition", "attachment; filename=" + class1._NombreZip);
            zip.Save(Response.OutputStream);
            zip.Dispose();

            if (Directory.Exists(Server.MapPath("~/Pictures/") + _Temporal))
            {
                _Todos = Directory.GetFiles(Server.MapPath("~/Pictures/") + _Temporal);
                foreach (String _Fila in _Todos)
                {
                    System.IO.File.Delete(_Fila);
                }
                Directory.Delete(Server.MapPath("~/Pictures/") + _Temporal);
            }

            //Response.Write("$(function () { {$(\".loader\").hide();});");
        
            Response.End();
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Acceso", "swal('Denegado', 'No tienes privilegios para realizar esta accion', 'error');", true);
        }
    }
    

    //Obtiene calendario de cliente seleccionado
    protected void cbo_cliente_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataSet _Ds_Medicion = _F._Get_Listado_Calendarios_Estudio(cbo_cliente.SelectedValue);

        cbo_medicion.Items.Clear();
        //cbo_sala.Items.Clear();

        cbo_medicion.DataSource = _Ds_Medicion.Tables[0];
        cbo_medicion.DataValueField = _Ds_Medicion.Tables[0].Columns[1].ToString();
        cbo_medicion.DataTextField = _Ds_Medicion.Tables[0].Columns[0].ToString();
        cbo_medicion.DataBind();

        String _Div = "";
        //lbl_salas.Text = "";
        //lbl_salas.Visible = false;
        galleryHTML.InnerHtml = _Div;
        dgw_fotos.Visible = false;
        btn_eliminafoto.Visible = false;
        btn_zip.Visible = false;
    }

    protected void cbo_medicion_SelectedIndexChanged(object sender, EventArgs e)
    {
        //cbo_sala.Items.Clear();
        //cbo_sala.DataBind();
        galleryHTML.InnerHtml = "";
        class1._Folio.Clear();
        class1._Fotos.Clear();
        //lbl_salas.Text = "";
        //lbl_salas.Visible = false;
        dgw_fotos.Visible = false;
        btn_eliminafoto.Visible = false;
        btn_zip.Visible = false;
    }

    //Muestra fotos en modo edicion...
    public void _Modo_Edicion(String _Medicion) 
    {
        btn_eliminafoto.Visible = true;

        //********** LOGS BUSQUEDA POR MEDICION ***********
        String _Nombre_Calendario = _F._Get_Nombre_Calendario(cbo_medicion.SelectedValue);
        String _Id_Estudio = _F._Get_Id_Estudio(cbo_medicion.SelectedValue);

        try
        {
            String _Id_Pagina = _U._Get_Id_Pagina(Request.Url.Segments[Request.Url.Segments.Length - 1]);
            _G._Set_Insert_Logs(Session["Id_Usuario"].ToString(), DateTime.Now.ToString("yyyy-MM-dd H:mm:ss"), "BUSQUEDA FOTOS MODO EDICION", _Id_Pagina, _Id_Estudio, "");
        }
        catch (Exception) { }

        DataSet _Ds_Salas = _F._Get_Listado_Fotos(_Medicion, "");

        if (_Ds_Salas != null)
        {
            try
            {
                _Parametros();
                _Ftp.Connect();
            }
            catch (Exception)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "Acceso", "swal('Error FTP', 'No se puede conectar con el servidor FTP', 'error');", true);
                return;
            }

            if (_Ds_Salas.Tables[0].Rows.Count > 0)
            {
                dgw_fotos.Visible = true;
                DataTable dt = new DataTable();
                dt.Columns.Add("cc", typeof(Boolean));
                dt.Columns.Add("Auditor", typeof(string));
                dt.Columns.Add("Faculty", typeof(string));
                dt.Columns.Add("Fecha", typeof(string));
                dt.Columns.Add("Ruta", typeof(string));
                dt.Columns.Add("imageurl", typeof(string));

                foreach (DataRow dataRow in _Ds_Salas.Tables[0].Rows)
                {
                    try
                    {
                        foreach (String _File in _Ftp.GetFiles(dataRow[1].ToString() + "/*.jpg"))
                        {
                            String _Tamano = _Ftp.GetSize(_File).ToString();
                            if (_Tamano != "0")
                            {
                                int B = _File.IndexOf("NO_VALIDA");
                                if (B > 0)
                                {
                                    dt.Rows.Add(true, dataRow[4].ToString(), dataRow[0].ToString(), dataRow[3].ToString(), "http://200.29.139.242/BBDD_SUPI/" + _File, "http://200.29.139.242/BBDD_SUPI/" + _File);
                                }
                                else 
                                {
                                    dt.Rows.Add(false, dataRow[4].ToString(), dataRow[0].ToString(), dataRow[3].ToString(), "http://200.29.139.242/BBDD_SUPI/" + _File, "http://200.29.139.242/BBDD_SUPI/" + _File);
                                }
                             }
                        }                    
                    }
                    catch (Exception)
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "Acceso", "swal('ERROR', 'NO SE PUEDEN MOSTRAR LAS FOTOS', 'error');", true);
                        return;
                    }
                }

                galleryHTML.InnerHtml = "";
                dgw_fotos.DataSource = dt;
                dgw_fotos.DataBind();
                _Ftp.Close();
            }
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Acceso", "swal('SIN FOTOS', 'NO SE HAN ENCONTRADO FOTOS PARA MEDICION SELECCIONADA', 'error');", true);
        }
    }

    //Muestra fotos segun calendario seleccionado...
    public void _Bucar_Fotos_Por_Medicion(String _Medicion, String _Folio)
    {
        //********** LOGS BUSQUEDA POR MEDICION ***********
        String _Nombre_Calendario = _F._Get_Nombre_Calendario(cbo_medicion.SelectedValue);
        String _Id_Estudio = _F._Get_Id_Estudio(cbo_medicion.SelectedValue);
        String _Div = "";

        try
        {
            String _Id_Pagina = _U._Get_Id_Pagina(Request.Url.Segments[Request.Url.Segments.Length - 1]);
            _G._Set_Insert_Logs(Session["Id_Usuario"].ToString(), DateTime.Now.ToString("yyyy-MM-dd H:mm:ss"), "BUSQUEDA FOTOS", _Id_Pagina, _Id_Estudio, "");
        }
        catch (Exception) { }

        DataSet _Ds_Salas = _F._Get_Listado_Fotos(_Medicion, _Folio);

        if (_Ds_Salas != null)
        {
            class1._NombreZip = _Nombre_Calendario.Replace(" ", "_") + ".zip";
         
            try
            {
                _Parametros();
                _Ftp.Connect();
            }
            catch (Exception)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "Acceso", "swal('Error FTP', 'No se puede conectar con el servidor FTP', 'error');", true);
                return;
            }

            //cbo_sala.Items.Clear();
                
            if (_Ds_Salas.Tables[0].Rows.Count > 0)
            {
                int _Total_Salas = 0;
                //cbo_sala.Items.Clear();
                class1._Folio.Clear();
                class1._Fotos.Clear();
                foreach (DataRow dataRow in _Ds_Salas.Tables[0].Rows)
                {
                    Boolean _Agrega = true;
                    int _Eentra = 0;

                    try 
                    {
                        foreach (String _File in _Ftp.GetFiles(dataRow[1].ToString() + "/*.jpg"))
                        {
                            String _Tamano = _Ftp.GetSize(_File).ToString();
                            if (_Tamano != "0")
                            {
                                _Eentra = 1;
                                if (_Agrega)
                                {
                                    //cbo_sala.Items.Add(dataRow[0].ToString());
                                  

                                    _Total_Salas++;
                                    _Agrega = false;
                                    _Div = _Div + "<div class=\"titulo\">" + dataRow[0].ToString() + " " + dataRow[2].ToString() + "</div> ";
                                    _Div = _Div + "<div class=\"glihtbox\">";
                                    _Div = _Div + "<ul class=\"list-unstyled row\" style=\"margin-bottom: 15px; margin-top: 5px;padding-left:0px;\">";
                                    _Div = _Div + "<li style=\"padding-right:10px;\">";
                                }
                                else
                                {
                                    _Div = _Div + "<li style=\"padding-right:10px;\">";
                                }

                                _Div = _Div + "<a href=\"" + "http://200.29.139.242/BBDD_SUPI/" + _File + "\"  data-sub-html=\"<h4>" + dataRow[2].ToString() + " " + dataRow[0].ToString() + "</h4>\">";

                                int B = _File.IndexOf("NO_VALIDA");
                                if (B == -1)
                                {
                                    _Div = _Div + "<img class=\"img-responsive\" src=\"" + "http://200.29.139.242/BBDD_SUPI/" + _File + "\" width=100 height=100 />";
                                } else{
                                    _Div = _Div + "<img class=\"img-responsive\" style='border: 2px solid red;' src=\"" + "http://200.29.139.242/BBDD_SUPI/" + _File + "\" width=100 height=100 />";
                                
                                }

                                //_Div = _Div + "<img class=\"img-responsive\" src=\"" + "http://200.29.139.242/BBDD_SUPI/" + _File + "\" width=100 height=100 />";
                                _Div = _Div + "</a>";
                                _Div = _Div + "</li>";
                                class1._Fotos.Add(_File);
                                class1._Folio.Add(dataRow[0].ToString());
                            }
                        }
                        if (_Eentra == 1)
                        {
                            _Div = _Div + "</ul></div>";
                            _Eentra = 0;
                        }
                        _Div = _Div + "</ul>";
                    }
                    catch(Exception ex)
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "Acceso", "swal('ERROR', 'NO SE PUEDEN MOSTRAR LAS FOTOS', 'error');", true);         
                        return;
                    }
                }

                _Ftp.Close();
                btn_zip.Visible = true;
                //lbl_salas.Text = "SALAS CON FOTOS : " + _Total_Salas.ToString();
                //lbl_salas.Visible = true;
            }
            else
            {
            }
        }
        else
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Acceso", "swal('SIN FOTOS', 'NO SE HAN ENCONTRADO FOTOS PARA MEDICION SELECCIONADA', 'error');", true);
        }
        //cbo_sala.DataBind();
        _Div = _Div + "</ul></div>";
        galleryHTML.InnerHtml = _Div;


    }



    protected void btn_eliminafoto_Click(object sender, EventArgs e)
    {
        _Parametros();
        _Ftp.Connect();
        int _Fotos_Inhabilitadas = 0;
        int _Fotos_Habilitadas = 0;
        foreach (GridViewRow row in dgw_fotos.Rows)
        {
            CheckBox cb = (CheckBox)row.Cells[0].FindControl("CheckBox1");
            if (cb.Checked)
            {
                try
                {
                    Label _Ruta = (Label)row.Cells[4].FindControl("lbl_ruta");
                    String _R = _Ruta.Text.Replace("http://200.29.139.242/BBDD_SUPI/", "");

                    int B = _R.IndexOf("NO_VALIDA");
                    if (B == -1)
                    {
                        String _NoValido = _R.Replace(".jpg", "") + "_NO_VALIDA.jpg";
                        _Ftp.RenameFile(_R, _NoValido);
                        _Fotos_Inhabilitadas++;
                    }
                }
                catch (Exception) { };
            }
            else 
            {
                try
                {
                    Label _Ruta = (Label)row.Cells[4].FindControl("lbl_ruta");
                    String _R = _Ruta.Text.Replace("http://200.29.139.242/BBDD_SUPI/", "");

                    int B = _R.IndexOf("NO_VALIDA");
                    if (B > 0)
                    {
                        String _Valido = _R.Replace("_NO_VALIDA", "") ;
                        _Ftp.RenameFile(_R, _Valido);
                        _Fotos_Habilitadas++;
                    }
                }
                catch (Exception) { };
            }
        }

        if (_Fotos_Inhabilitadas != 0 && _Fotos_Habilitadas == 0)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Fotos", "swal('Fotos inhabilitadas', 'Se han inhabilitado " + _Fotos_Inhabilitadas.ToString() + " fotos', 'info');", true);
        }
        else if (_Fotos_Inhabilitadas == 0 && _Fotos_Habilitadas != 0)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Fotos", "swal('Fotos habilitadas', 'Se han habilitado " + _Fotos_Habilitadas.ToString() + " fotos', 'info');", true);
        }
        else if (_Fotos_Inhabilitadas != 0 && _Fotos_Habilitadas != 0)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Fotos", "swal('Fotos habilitadas/Inhabilitadas', 'Se han habilitado " + _Fotos_Habilitadas.ToString() + " fotos y se han Inhabilitado " + _Fotos_Inhabilitadas.ToString()  + " fotos', 'info');", true);
        }
     
        _Ftp.Close();
    }

    protected void dgw_fotos_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            CheckBox d = (CheckBox)e.Row.Cells[0].FindControl("CheckBox1");
            if (d.Checked)
            {
                e.Row.BackColor = System.Drawing.Color.Red;
            }
            //e.Row.Attributes.Add("onClick", "ChangeColor('" + "dgw_fotos','" + (e.Row.RowIndex + 1).ToString() + "')");
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

    protected void btn_go_empleado(object sender, EventArgs e)
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

}