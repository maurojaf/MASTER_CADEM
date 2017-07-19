using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Collections;


/// <summary>
/// Descripción breve de Launcher_Controller
/// </summary>
public class Launcher_Controller
{
	public Launcher_Controller()
	{
		//
		// TODO: Agregar aquí la lógica del constructor
		//
	}

    //Realiza la carga de Categorias para launcher...
    public DataSet _Get_Carga_Categorias()
    {
        try
        {
            SqlConnection _Conexion_Local = new SqlConnection(ConfigurationManager.ConnectionStrings["SUPI"].ConnectionString);
            DataSet _Ds_Store = new DataSet();
            SqlDataAdapter _Ds_Datos = new SqlDataAdapter("SELECT DISTINCT CATEGORIA,CATEGORIA FROM EAN_NUEVO WITH(NOLOCK) WHERE CATEGORIA IS NOT NULL AND ID_VISITA IN (SELECT ID_VISITA FROM VISITA WITH(NOLOCK) WHERE ID_ESTUDIOSALA IN ( SELECT ID_ESTUDIOSALA FROM ESTUDIOSALA WITH(NOLOCK) WHERE ID_ESTUDIO IN (SELECT ID_ESTUDIO FROM ESTUDIO WITH(NOLOCK) WHERE FECHACIERRE>=CAST(GETDATE() AS DATE))))", _Conexion_Local);
            _Ds_Datos.Fill(_Ds_Store, "DATOS");

            if (_Ds_Store.Tables["DATOS"].Rows.Count > 0)
            {
                return _Ds_Store;
            }
            else
            {
                return null;
            }
        }
        catch (Exception)
        {
            return null;
        }
    }

    //Exportar los datos de launcher por categoria...
    public DataSet _Get_Exportar_Datos_Launcher(String _Fecha, String _Categoria, Boolean _3_OMAS)
    {
        try
        {
            String _Query = "SELECT s.FOLIOCADEM, v.dia, EAN,DESCRIPCION,MARCA,FORMATO,GRAMAJE,PRECIO_FLEJE,CATEGORIA,ALTO,ANCHO,FONDO,FOTO, (select nombreapellido ";
            _Query = _Query + " from AUDITOR aud WITH(NOLOCK), VISITA WITH(NOLOCK) where aud.ID_AUDITOR = v.ID_AUDITOR and v.ID_VISITA = ID_VISITA) AUDITOR";
            _Query = _Query + " FROM [EAN_NUEVO] WITH(NOLOCK),[VISITA] v WITH(NOLOCK), [ESTUDIOSALA] es WITH(NOLOCK), [SALA] s  WITH(NOLOCK) ";
            _Query = _Query + " where EAN_NUEVO.ID_VISITA = v.ID_VISITA and v.ID_ESTUDIOSALA = es.ID_ESTUDIOSALA and es.ID_SALA = s.ID_SALA and v.ID_AUDITOR != 1 ";
            _Query = _Query + " and ANCHO is not null AND V.DIA>='" + _Fecha + "'";
            _Query = _Query + " and CATEGORIA in (" + _Categoria + ") ";

            if (_3_OMAS)
            {
                _Query = _Query + " and ean in (";
                _Query = _Query + " SELECT EAN";
                _Query = _Query + " FROM [EAN_NUEVO] WITH(NOLOCK),[VISITA] v WITH(NOLOCK), [ESTUDIOSALA] es WITH(NOLOCK), [SALA] s  WITH(NOLOCK)  ";
                _Query = _Query + " where EAN_NUEVO.ID_VISITA = v.ID_VISITA and v.ID_ESTUDIOSALA = es.ID_ESTUDIOSALA and es.ID_SALA = s.ID_SALA and v.ID_AUDITOR != 1  and ANCHO is not null AND V.DIA>='" + _Fecha + "' and CATEGORIA in (" + _Categoria + ") ";
                _Query = _Query + " group by ean having count(*)>=3";
                _Query = _Query + " )";
            }

            SqlConnection _Conexion_Local = new SqlConnection(ConfigurationManager.ConnectionStrings["SUPI"].ConnectionString);
            DataSet _Ds_Store = new DataSet();
            SqlDataAdapter _Ds_Datos = new SqlDataAdapter(_Query, _Conexion_Local);
            _Ds_Datos.Fill(_Ds_Store, "DATOS");

            return _Ds_Store;

        }
        catch (Exception)
        {
            return null;
        }
    }


}