using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Collections;

/// <summary>
/// Descripción breve de Prioridades_Controller
/// </summary>
public class Prioridades_Controller
{
	public Prioridades_Controller()
	{
		//
		// TODO: Agregar aquí la lógica del constructor
		//
	}

    //Obtiene los datos de la tabla Estudio...
    public DataSet _Get_EstudioS()
    {
        try
        {
            String _qUERY = "SELECT NOMBREESTUDIO, ID_ESTUDIO FROM ESTUDIO with(nolock) WHERE FECHACIERRE >=CAST(GETDATE() AS DATE)  AND ID_ESTUDIO IN (SELECT DISTINCT ID_ESTUDIO FROM PRIORIDAD WITH(NOLOCK) WHERE ID_ESTUDIO IS NOT NULL) ORDER BY NOMBREESTUDIO";
            SqlConnection _Conexion_SqlServ = new SqlConnection(ConfigurationManager.ConnectionStrings["SUPI"].ConnectionString);
            DataSet _Ds_Store = new DataSet();
            SqlDataAdapter _Ds_Datos = new SqlDataAdapter(_qUERY, _Conexion_SqlServ);
            _Ds_Datos.Fill(_Ds_Store, "DATOS");
            return _Ds_Store;
        }
        catch (Exception) 
        {
            return null;
        }
    }

    //Exporta datos formato excel 2007...
    public DataSet _Get_Proiridades_EXcel(String _Id_Estudio, String _Desde, String _Hasta)
    {
        try
        {
            String _Query = "select FOLIO, DIRECCION, ESTUDIO, DIA, AUDITOR, OBSERVACION, COMENTARIOS as COMENTARIO_CAMPO from PRIORIDAD p WITH(NOLOCK) where p.ID_ESTUDIO = " + _Id_Estudio + " and cast(p.HORA_INICIO as date) ";
            _Query = _Query + " BETWEEN '" + _Desde + "' AND '" + _Hasta + "' ";

            SqlConnection _Conexion_SqlServ = new SqlConnection(ConfigurationManager.ConnectionStrings["SUPI"].ConnectionString);
            DataSet _Ds_Store = new DataSet();
            SqlDataAdapter _Ds_Datos = new SqlDataAdapter(_Query, _Conexion_SqlServ);
            _Ds_Datos.Fill(_Ds_Store, "DATOS");
            return _Ds_Store;
        }
        catch (Exception)
        {
            return null;
        }
    }


}