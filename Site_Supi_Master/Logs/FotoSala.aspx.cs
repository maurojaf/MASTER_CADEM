using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Collections;
using System.Security.Cryptography;
using System.Security;
using System.Text;
using System.Configuration;
using EnterpriseDT.Net.Ftp;

public partial class Logs_FotoSala : System.Web.UI.Page
{
    FotoSala_Controller fs = new FotoSala_Controller();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DataSet _Ds = fs._Get_Auditores();
            cbo_auditor.DataSource = _Ds.Tables[0];
            cbo_auditor.DataValueField = _Ds.Tables[0].Columns[1].ToString();
            cbo_auditor.DataTextField = _Ds.Tables[0].Columns[0].ToString();
            cbo_auditor.DataBind();
        }
    }

    protected void btn_session_Click(object sender, EventArgs e)
    {

    }

    protected void btn_filtros_Click(object sender, EventArgs e)
    {
        String _Div = "";
        DataSet _Salas = fs._Get_Visitas_por_sala(cbo_auditor.SelectedValue, txt_fecha.Text);
        if (_Salas != null) 
        {
            if (_Salas.Tables[0].Rows.Count > 0)
            {
                int _Total_Salas = 0;
                FTPConnection _Ftp = new FTPConnection();
                _Ftp.UserName = "administrador";
                _Ftp.Password = "smart123";
                _Ftp.ServerAddress = "200.29.139.242";
                _Ftp.ServerPort = 21;
                _Ftp.ServerDirectory = "BBDD_SUPI/FOTOS_SALA/" + txt_fecha.Text + "/";
                _Ftp.ConnectMode = FTPConnectMode.PASV;
                _Ftp.Timeout = 5000;
                _Ftp.TransferType = FTPTransferType.BINARY;
                _Ftp.Connect();
                _Ftp.Timeout = 10000;

                foreach (DataRow dataRow in _Salas.Tables[0].Rows)
                {
                    Boolean _Agrega = true;
                    int _Eentra = 0;
                    String _Llave = dataRow[0].ToString() + "_" + dataRow[1].ToString();
                    //String _Nombre_Auditor = dataRow[3].ToString(); ;
                    String _Fecha = dataRow[1].ToString(); ;

                    foreach (String _File in _Ftp.GetFiles(_Llave + "/*.jpg"))
                    {
                        String _Tamano = _Ftp.GetSize(_File).ToString();
                        if (_Tamano != "0")
                        {
                            _Eentra = 1;
                            if (_Agrega)
                            {
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

                            _Div = _Div + "<a href=\"" + "http://200.29.139.242/BBDD_SUPI/FOTOS_SALA/" + txt_fecha.Text + "/" + _File + "\"  data-sub-html=\"<h4>" + dataRow[1].ToString() + " " + dataRow[0].ToString() + "</h4>\">";
                            _Div = _Div + "<img class=\"img-responsive\" src=\"" + "http://200.29.139.242/BBDD_SUPI/FOTOS_SALA/" + txt_fecha.Text + "/" + _File + "\" width=100 height=100 />";
                            _Div = _Div + "</a>";
                            _Div = _Div + "</li>";
                        }
                    }

                    if (_Eentra == 1)
                    {
                        _Div = _Div + "</ul></div>";
                        _Eentra = 0;
                    }
                    //_Div = _Div + "</ul>";

                }
            }
        }

        _Div = _Div + "</ul></div>";
        galleryHTML.InnerHtml = _Div;
    }

    protected void btn_menu_Click(object sender, EventArgs e)
    {
        Response.Redirect("../Menu.aspx");
        return;
    }
}