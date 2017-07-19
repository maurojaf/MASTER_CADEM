using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.IO;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;

public class Logistica_Controller
{
	public Logistica_Controller(){}

    //Obtiene los datos de la tabla Comuna...
    public DataSet _Get_Carga_Comuna()
    {
        try
        {
            SqlConnection _Conexion_Local = new SqlConnection(ConfigurationManager.ConnectionStrings["SUPI_MASTER"].ConnectionString);
            DataSet _Ds_Store = new DataSet();
            SqlDataAdapter _Ds_Datos = new SqlDataAdapter("SELECT '' AS NOMBRE, null AS ID UNION ALL SELECT COM_DESCRIPCION AS NOMBRE, COM_ID AS ID  FROM COMUNA order by NOMBRE", _Conexion_Local);
            _Ds_Datos.Fill(_Ds_Store, "DATOS");
            return _Ds_Store;
        }
        catch (Exception)
        {
            return null;
        }
    }

    //Obtiene los datos del trayecto consultado...
    public ArrayList _Get_Datos_Logistica(String _Comuna_Inicio, String _Comuna_Termino)
    {
        ArrayList arrlst = new ArrayList();
        SqlConnection _Cn = new SqlConnection();
        _Cn.ConnectionString = ConfigurationManager.ConnectionStrings["SUPI_MASTER"].ConnectionString;

        try
        {
            _Cn.Open();
            SqlCommand _Cmd = new SqlCommand("EXEC _GET_DATOSLOGISTICA " + _Comuna_Inicio + "," + _Comuna_Termino + "", _Cn);
            SqlDataReader _Rd = _Cmd.ExecuteReader();
            _Rd.Read();

            for (int i = 0; i < _Rd.FieldCount; i++)
            {
                arrlst.Add(_Rd.GetString(i));
            }
            _Rd.Close();
            _Cn.Close();
            return arrlst;
        }
        catch (SqlException E)
        {
            _Cn.Close();
            return null;
        }
        catch (Exception E)
        {
            _Cn.Close();
            return null;
        }
    }

    //Exporta todos los trayectos en formato Excel 2007
    public DataSet _Get_Exportar_Logisticas() 
    {
        try
        {
            SqlConnection _Conexion_Local = new SqlConnection(ConfigurationManager.ConnectionStrings["SUPI_MASTER"].ConnectionString);
            DataSet _Ds_Store = new DataSet();
            SqlDataAdapter _Ds_Datos = new SqlDataAdapter("SELECT (SELECT C.COM_DESCRIPCION FROM COMUNA C WITH(NOLOCK) WHERE C.COM_ID = [LOGIST_COMUNA_INICIO]) AS COMUNA_INICIO ,(SELECT C.COM_DESCRIPCION FROM COMUNA C WITH(NOLOCK) WHERE C.COM_ID = [LOGIST_COMUNA_FIN]) AS COMUNA_FIN,[LOGIST_TRAYECTO],[LOGIST_COSTO],[LOGIST_TRAMO] ,[LOGIST_TRAY_CON_TERMINAL] FROM [MCADEM].[dbo].[LOGISTICA] WITH(NOLOCK)", _Conexion_Local);
            _Ds_Datos.Fill(_Ds_Store, "DATOS");
            return _Ds_Store;
        }
        catch (Exception)
        {
            return null;
        }
    }

    //Actualiza datos del trayecto...
    public Boolean _Update_Trayecto(String _Inicio, String _Termnmino, String _Trayecto_Nuevo, String _Costo, String _Tipo)
    {
        String _Query = "SELECT [LOGIST_TRAY_CON_TERMINAL],[LOGIST_TRAYECTO] FROM LOGISTICA WHERE LOGIST_COMUNA_INICIO=" + _Inicio + " AND LOGIST_COMUNA_FIN=" + _Termnmino;
        SqlConnection _Conexion_Local = new SqlConnection(ConfigurationManager.ConnectionStrings["SUPI_MASTER"].ConnectionString);
        DataSet _Ds_Store = new DataSet();
        SqlDataAdapter _Ds_Datos = new SqlDataAdapter(_Query, _Conexion_Local);
        _Ds_Datos.Fill(_Ds_Store, "DATOS");

        String _Trayecto_Terminal_original = _Ds_Store.Tables[0].Rows[0][0].ToString();
        String _Trayecto_ortiginal =         _Ds_Store.Tables[0].Rows[0][1].ToString();
        String _Nuevo_Trayecto_Terminal = "";


        if (_Trayecto_ortiginal != _Trayecto_Nuevo)
        {
            // *** SUMA + 30 MIN. A TRAYECTO TERMINAL ****
            DateTime _Terminal = Convert.ToDateTime(_Trayecto_Nuevo);
            _Terminal = _Terminal.AddMinutes(30);
            _Nuevo_Trayecto_Terminal = _Terminal.ToLongTimeString();
        }

        try
        {
            if (_Nuevo_Trayecto_Terminal != "")
            {
                _Query = "UPDATE [LOGISTICA] SET [LOGIST_TRAYECTO]='" + _Trayecto_Nuevo + "' ,[LOGIST_COSTO]=" + _Costo + " ,[LOGIST_TRAMO]='" + _Tipo + "', LOGIST_TRAY_CON_TERMINAL='" + _Nuevo_Trayecto_Terminal + "' WHERE LOGIST_COMUNA_INICIO=" + _Inicio + " AND LOGIST_COMUNA_FIN=" + _Termnmino;
            }
            else 
            {
                _Query = "UPDATE [LOGISTICA] SET [LOGIST_TRAYECTO]='" + _Trayecto_Nuevo + "' ,[LOGIST_COSTO]=" + _Costo + " ,[LOGIST_TRAMO]='" + _Tipo + "' WHERE LOGIST_COMUNA_INICIO=" + _Inicio + " AND LOGIST_COMUNA_FIN=" + _Termnmino;
            }
            _Conexion_Local = new SqlConnection(ConfigurationManager.ConnectionStrings["SUPI_MASTER"].ConnectionString);
            _Ds_Store = new DataSet();
            _Ds_Datos = new SqlDataAdapter(_Query, _Conexion_Local);
            _Ds_Datos.Fill(_Ds_Store, "DATOS");
            return true;
        }
        catch (Exception)
        {
            return false;
        } 

    }

    //Ingresar Nuevo Trayecto (Logistica)...
    public Boolean _Insert_Trayecto(String _Inicio, String _Termnmino, String _Trayecto_Nuevo, String _Costo, String _Tipo)
    {
        SqlConnection _Cn = new SqlConnection();
        _Cn.ConnectionString = ConfigurationManager.ConnectionStrings["SUPI_MASTER"].ConnectionString;

        // *** SUMA + 30 MIN. A TRAYECTO TERMINAL ****
        DateTime _Terminal = Convert.ToDateTime(_Trayecto_Nuevo);
        _Terminal = _Terminal.AddMinutes(30);
        String _Trayecto_Terminal = _Terminal.ToLongTimeString();

        try
        {
            String _Query = "INSERT INTO [LOGISTICA] ([LOGIST_COMUNA_INICIO],[LOGIST_COMUNA_FIN] ,[LOGIST_TRAYECTO] ,[LOGIST_COSTO] ,[LOGIST_TRAMO],LOGIST_TRAY_CON_TERMINAL) ";
            _Query = _Query + " VALUES (" + _Inicio + ", " + _Termnmino + ", '" + _Trayecto_Nuevo + "', " + _Costo + ",'" + _Tipo + "','" + _Trayecto_Terminal + "')";
            SqlConnection _Conexion_Local = new SqlConnection(ConfigurationManager.ConnectionStrings["SUPI_MASTER"].ConnectionString);
            DataSet _Ds_Store = new DataSet();
            SqlDataAdapter _Ds_Datos = new SqlDataAdapter(_Query, _Conexion_Local);
            _Ds_Datos.Fill(_Ds_Store, "DATOS");
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

}