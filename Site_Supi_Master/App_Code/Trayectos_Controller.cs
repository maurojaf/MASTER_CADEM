using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Collections;
/// <summary>
/// Descripción breve de Trayectos_Controller
/// </summary>
public class Trayectos_Controller
{
	public Trayectos_Controller()
	{
		//
		// TODO: Agregar aquí la lógica del constructor
		//
	}

    // ******************************************************************
    // ********************** OBTENCION DE DATOS ************************
    // ******************************************************************

    //Obtiene los datos de la sala consultada...
    public DataSet _Get_Datos_Trayecto(String _Folio_Inicio, String _Folio_Fin)
    {  
        try
        {
            SqlConnection _Conexion_Local = new SqlConnection(ConfigurationManager.ConnectionStrings["SUPI_MASTER"].ConnectionString);
            DataSet _Ds_Store = new DataSet();
            SqlDataAdapter _Ds_Datos = new SqlDataAdapter("select T.TRA_DISTANCIA_GOOGLE,T.TRA_CLUSTER, DATEPART(hour,[TRA_TIEMPO_GOOGLE]) as hora_google, DATEPART(MINUTE,[TRA_TIEMPO_GOOGLE]) as minuto_google, DATEPART(hour,TRA_TIEMPO_SUPI) as hora_supi, DATEPART(MINUTE,TRA_TIEMPO_SUPI) as minuto_supi from [TRAYECTO] T with(nolock) where  [TRA_ORIGEN] = " + _Folio_Inicio + " and [TRA_DESTINO] = " + _Folio_Fin, _Conexion_Local);
            _Ds_Datos.Fill(_Ds_Store, "DATOS");
            return _Ds_Store;
        }
        catch (Exception)
        {
            return null;
        }
    }



    public Boolean _Insert_Tarecto(String _Folio_ini, String _Folio_Fin, String _Distancia, String _Cluster, String _Tirmpo_GG, String _Tiempo_SUPI) 
    {
        try
        {
            SqlConnection _Conexion_Local = new SqlConnection(ConfigurationManager.ConnectionStrings["SUPI_MASTER"].ConnectionString);
            DataSet _Ds_Store = new DataSet();
            String _Insert = "INSERT INTO [dbo].[TRAYECTO] ([TRA_ORIGEN] ,[TRA_DESTINO],[TRA_DISTANCIA_GOOGLE],[TRA_TIEMPO_GOOGLE], [TRA_CLUSTER] ,[TRA_TIEMPO_SUPI])  VALUES(";
            _Insert = _Insert + "'" + _Folio_ini + "','" + _Folio_Fin + "','" + _Distancia + "','" + _Tirmpo_GG + "','" + _Cluster + "','" + _Tiempo_SUPI + "')";
            SqlDataAdapter _Ds_Datos = new SqlDataAdapter(_Insert, _Conexion_Local);
            _Ds_Datos.Fill(_Ds_Store);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public Boolean _Update_Tarecto(String _Folio_ini, String _Folio_Fin, String _Distancia, String _Cluster, String _Tirmpo_GG, String _Tiempo_SUPI)
    {
        try
        {
            SqlConnection _Conexion_Local = new SqlConnection(ConfigurationManager.ConnectionStrings["SUPI_MASTER"].ConnectionString);
            DataSet _Ds_Store = new DataSet();
            String _Insert = "UPDATE [dbo].[TRAYECTO] SET [TRA_DISTANCIA_GOOGLE]='" + _Distancia.Replace(",",".") + "',[TRA_TIEMPO_GOOGLE]='" + _Tirmpo_GG + "', [TRA_CLUSTER]='" + _Cluster + "', [TRA_TIEMPO_SUPI]='" + _Tiempo_SUPI + "' WHERE [TRA_ORIGEN] ='" + _Folio_ini + "' AND [TRA_DESTINO] ='" + _Folio_Fin + "'";
            SqlDataAdapter _Ds_Datos = new SqlDataAdapter(_Insert, _Conexion_Local);
            _Ds_Datos.Fill(_Ds_Store);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public DataSet _Exportar_Trayecto()
    {
        try
        {
            SqlConnection _Conexion_Local = new SqlConnection(ConfigurationManager.ConnectionStrings["SUPI_MASTER"].ConnectionString);
            DataSet _Ds_Store = new DataSet();
            SqlDataAdapter _Ds_Datos = new SqlDataAdapter("select * from [TRAYECTO] T with(nolock) ", _Conexion_Local);
            _Ds_Datos.Fill(_Ds_Store, "DATOS");
            return _Ds_Store;
        }
        catch (Exception)
        {
            return null;
        }
    }



    // ***************************************************************
    // ********************* ID SOLICITADOS **************************
    // ***************************************************************

    public DataSet _Exista_Sala_MCadem(String _Sala)
    {
        try
        {
            SqlConnection _Conexion_Local = new SqlConnection(ConfigurationManager.ConnectionStrings["SUPI_MASTER"].ConnectionString);
            DataSet _Ds_Store = new DataSet();
            SqlDataAdapter _Ds_Datos = new SqlDataAdapter("SELECT * FROM SALA with(nolock) WHERE SALA_FOLIO ='" + _Sala + "'", _Conexion_Local);
            _Ds_Datos.Fill(_Ds_Store, "DATOS");
            return _Ds_Store;
        }
        catch (Exception)
        {
            return null;
        }
    }

}