using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Collections;

/// <summary>
/// Descripción breve de Tamano_Controller
/// </summary>
public class Tamano_Controller
{
	public Tamano_Controller() {}

    // OBTIENE TODOS LOS TAMANOS
    public DataSet _Get_Carga_Tamano()
    {
        try
        {
            SqlConnection _Conexion_Local = new SqlConnection(ConfigurationManager.ConnectionStrings["SUPI_MASTER"].ConnectionString);
            DataSet _Ds_Store = new DataSet();
            SqlDataAdapter _Ds_Datos = new SqlDataAdapter("Select [TAM_ID] as ID,[TAM_DESCRIPCION] AS TAMANO ,[TAM_EXPLICACION] AS DEFINICION from tamano WITH(NOLOCK)", _Conexion_Local);
            _Ds_Datos.Fill(_Ds_Store, "DATOS");
            return _Ds_Store;
        }
        catch (Exception)
        {
            return null;
        }
    }


    // INSERTA NUEVO TAMANO
    public Boolean _Insert_Tamano(String _Tamano, String _Descripcion)
    {
        try
        {
            SqlConnection _Conexion_Local = new SqlConnection(ConfigurationManager.ConnectionStrings["SUPI_MASTER"].ConnectionString);
            DataSet _Ds_Store = new DataSet();
            SqlDataAdapter _Ds_Datos = new SqlDataAdapter("insert into tamano values ('" + _Tamano + "','" + _Descripcion + "')", _Conexion_Local);
            _Ds_Datos.Fill(_Ds_Store);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    // ACTUALIZA TAMANO EXISTENTE
    public Boolean _Update_Tamano(String _Tamano_New, String _Descripcion, String _Tam_Id)
    {
        try
        {
            SqlConnection _Conexion_Local = new SqlConnection(ConfigurationManager.ConnectionStrings["SUPI_MASTER"].ConnectionString);
            DataSet _Ds_Store = new DataSet();
            SqlDataAdapter _Ds_Datos = new SqlDataAdapter("update tamano set TAM_DESCRIPCION='" + _Tamano_New + "', TAM_EXPLICACION='" + _Descripcion + "' where TAM_ID = " + _Tam_Id, _Conexion_Local);
            _Ds_Datos.Fill(_Ds_Store);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

   

}