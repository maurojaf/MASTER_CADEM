using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Collections;
using System.Data;
using System.Security.Cryptography;
using System.Configuration;

/// <summary>
/// Descripción breve de FotoSala_Controller
/// </summary>
public class FotoSala_Controller
{
	public FotoSala_Controller(){}

    //**** OBTIENE LISTADO DE ESTUDIOS DISPONIBLES FOT ENVIO DE FOTOS *************+
    public DataSet _Get_Auditores()
    {
        try
        {
            SqlConnection _Conexion_Local = new SqlConnection(ConfigurationManager.ConnectionStrings["SUPI"].ConnectionString);
            DataSet _Ds_Store = new DataSet();
            SqlDataAdapter _Ds_Datos = new SqlDataAdapter("SELECT  NOMBREAPELLIDO,ID_AUDITOR from auditor WITH(NOLOCK) where activo <> 0 and ID_AUDITOR not in(1,414,10607) order by NOMBREAPELLIDO", _Conexion_Local);
            _Ds_Datos.Fill(_Ds_Store, "DATOS");
            return _Ds_Store;

        }
        catch (Exception)
        {
            return null;
        }
    }


    public DataSet _Get_Visitas_por_sala(String _Auditor, String _Fecha) 
    {    
        try
        {
            String _Query = "select distinct s.FOLIOCADEM, cast(v.DIA as varchar), a.NOMBREAPELLIDO from visita v with(nolock), auditor a with(nolock), estudiosala es with(nolock), sala s with(nolock), FOTOS_PDA ft  with(nolock) where dia =cast('" + _Fecha + "' as date) and estado=4 and CHEQUEO = 0 and v.id_estudiosala = es.id_estudiosala and es.ID_SALA = s.ID_SALA and v.id_auditor= a.id_auditor and ft.ID_VISITA = v.ID_VISITA and ft.RUTA like '%inicial_sala%'";
            if (_Auditor != "0")
            {
                _Query = _Query + "and v.ID_AUDITOR = " + _Auditor;
            }
            _Query = _Query + " order by a.NOMBREAPELLIDO";

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