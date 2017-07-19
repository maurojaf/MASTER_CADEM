using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Collections;

/// <summary>
/// Descripción breve de ComienzoAuditorias_Controller
/// </summary>
public class ComienzoAuditorias_Controller
{
	public ComienzoAuditorias_Controller(){}

    // Carga auditores
    public DataSet _Get_Cargas_Auditores()
    {
        try
        {
            SqlConnection _Conexion_Local = new SqlConnection(ConfigurationManager.ConnectionStrings["SUPI"].ConnectionString);
            DataSet _Ds_Store = new DataSet();
            SqlDataAdapter _Ds_Datos = new SqlDataAdapter("SELECT ' --TODOS--' AS NOMBRE, 0 AS ID UNION ALL SELECT NOMBREAPELLIDO AS NOMBRE, ID_AUDITOR AS ID FROM AUDITOR WITH(NOLOCK) WHERE ACTIVO=1 AND ID_AUDITOR NOT IN(1,414) and id_supervisor <> 1 order by NOMBRE", _Conexion_Local);
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

    //Exportar datos formato Excel 2007...
    public DataSet _Get_All_Auditorias(String _Id_Usuario, String _Desde, String _Hasta)
    {
        try
        {
            String _Query = "  SELECT (SELECT TOP 1 AA.NOMBREAPELLIDO FROM AUDITOR AA WHERE AA.ID_aUDITOR = A.ID_SUPERVISOR) AS COORDINADOR, ";
            _Query = _Query + " A.NOMBREAPELLIDO AS AUDITOR, E.NOMBREESTUDIO AS ESTUDIO, LEFT(CAST(V.HORAINICIO AS DATE),10) AS DIA, LEFT(CAST(V.HORAINICIO AS TIME),5) AS [HORA INICIO] ";
            _Query = _Query + " from LOG_ALARMA_ENVIADA LA WITH(NOLOCK), VISITA V WITH(NOLOCK), AUDITOR A WITH(NOLOCK), ESTUDIOSALA ES WITH(NOLOCK), ESTUDIO E WITH(NOLOCK)";
            _Query = _Query + " WHERE V.ID_VISITA = LA.ID_VISITA AND A.ID_AUDITOR = V.ID_AUDITOR AND ES.ID_ESTUDIOSALA = V.ID_ESTUDIOSALA AND E.ID_ESTUDIO = ES.ID_ESTUDIO";
            _Query = _Query + " AND CAST(V.HORAINICIO AS DATE) BETWEEN '" + _Desde + "' AND '" + _Hasta + "' ";

            if (_Id_Usuario != "0") 
            {
                _Query = _Query + " and v.id_auditor = " + _Id_Usuario;
            }

            _Query = _Query + " ORDER BY V.HORAINICIO DESC";

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