using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Collections;

/// <summary>
/// Descripción breve de Quiz_Controller
/// </summary>
public class Quiz_Controller
{
	public Quiz_Controller()
	{
		//
		// TODO: Agregar aquí la lógica del constructor
		//
	}


    //*******************************************
    //******* CARGA DE DATOS NECESARIOS *********
    //*******************************************

    public DataSet _Get_Carga_Estudios_Quiz()
    {
        try
        {
            SqlConnection _Conexion_Local = new SqlConnection(ConfigurationManager.ConnectionStrings["SUPI"].ConnectionString);
            DataSet _Ds_Store = new DataSet();
            SqlDataAdapter _Ds_Datos = new SqlDataAdapter("SELECT ' --TODOS--' AS NOMBRE, 0 AS ID UNION ALL SELECT NOMBREESTUDIO AS NOMBRE, ID_ESTUDIO AS ID FROM ESTUDIO WITH(NOLOCK) WHERE  FECHACIERRE>=CAST(GETDATE() AS DATE)  union all SELECT 'QUIZ CAPACITACION' AS NOMBRE, 21 AS ID order by NOMBRE", _Conexion_Local);
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

    public DataSet _Get_Carga_Auditores_Quiz()
    {
        try
        {
            SqlConnection _Conexion_Local = new SqlConnection(ConfigurationManager.ConnectionStrings["SUPI"].ConnectionString);
            DataSet _Ds_Store = new DataSet();
            SqlDataAdapter _Ds_Datos = new SqlDataAdapter("SELECT ' --TODOS--' AS NOMBRE, 0 AS ID UNION ALL SELECT NOMBREAPELLIDO AS NOMBRE, ID_AUDITOR AS ID FROM AUDITOR WITH(NOLOCK) WHERE ACTIVO=1 AND ID_AUDITOR NOT IN(1,414) order by NOMBRE", _Conexion_Local);
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




    //Muestra todos los quiz segun fecha, estudio, auditor
    public DataSet _Get_All_Quiz(String _Id_Usuario,String _Id_Estudio, String _Desde, String _Hasta)
    {
        try
        {
            String _Query = "";
            if (_Id_Estudio == "21")
            {
                _Query = "SELECT  A.NOMBREAPELLIDO, RQC.FECHA, Q.PREGUNTA, RQ.RESPUESTA AS CORRECTA,";
                _Query = _Query + " (select r.RESPUESTA  from respuesta_quiz r  WITH(NOLOCK) where r.id = RQC.id_respuesta) AS CONTESTADA";
                _Query = _Query + " FROM QUIZ Q WITH(NOLOCK), RESPUESTA_QUIZ RQ WITH(NOLOCK), RESPUESTA_AUDITOR_QUIZ_CAPACITACION RQC WITH(NOLOCK) ,AUDITOR A WITH(NOLOCK)";
                _Query = _Query + " WHERE Q.ID = RQ.ID_QUIZ AND Q.ID_ESTUDIO=21  and CORRECTA = 1 AND RQC.ID_QUIZ = Q.ID AND A.ID_AUDITOR = RQC.ID_AUDITOR";
                _Query = _Query + " AND RQC.FECHA BETWEEN '" + _Desde + "' AND '" + _Hasta + "'";
                _Query = _Query + " ORDER BY RQC.ID_AUDITOR ";
            }
            else 
            {
                _Query = "SELECT S.FOLIOCADEM as FOLIO, A.NOMBREAPELLIDO AS AUDITOR, E.NOMBREESTUDIO AS ESTUDIO, LEFT(FECHA,10) AS FECHA , UPPER(Q.PREGUNTA) AS PREGUNTA,UPPER((select top 1 respuesta from respuesta_quiz qq with(nolock) where qq.id_quiz = raq.id_quiz and correcta=1)) AS CORRECTA, UPPER(RQ.RESPUESTA) AS CONTESTADA";
                _Query = _Query + " from respuesta_auditor_quiz RAQ WITH(NOLOCK), VISITA V WITH(NOLOCK), AUDITOR A WITH(NOLOCK),QUIZ Q WITH(NOLOCK),RESPUESTA_QUIZ RQ WITH(NOLOCK), ESTUDIOSALA ES WITH(NOLOCK), ESTUDIO E WITH(NOLOCK), SALA S WITH(NOLOCK)";
                _Query = _Query + " where V.ID_VISITA= RAQ.ID_VISITA AND A.ID_AUDITOR = V.ID_AUDITOR AND Q.ID = RAQ.ID_QUIZ AND RQ.ID = RAQ.ID_RESPUESTA AND ES.ID_ESTUDIOSALA = V.ID_ESTUDIOSALA AND ES.ID_ESTUDIO = E.ID_ESTUDIO AND S.ID_SALA = ES.ID_SALA";
                _Query = _Query + " AND RAQ.PUNTAJE=0 ";
                _Query = _Query + " AND A.ID_AUDITOR <> 1 AND CAST(V.HORAINICIO AS DATE) BETWEEN '" + _Desde + "' AND '" + _Hasta + "' ";
                if (_Id_Estudio != "0") _Query = _Query + " AND E.ID_ESTUDIO = " + _Id_Estudio;
                if (_Id_Usuario != "0") _Query = _Query + " AND a.id_auditor = " + _Id_Usuario;
                _Query = _Query + " AND E.ID_ESTUDIO <> 21";
            }
            
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

    //Exporta todos los datos de quiz
    public DataSet _Get_Exportar_Quiz(String _Fecha_Inicio, String _Fecha_fin, String _Id_Estudio, String _Id_Auditor)
    {
        try
        {
            String _Query = "";
            if (_Id_Estudio == "21")
            {
                _Query = "SELECT  A.NOMBREAPELLIDO, RQC.FECHA, Q.PREGUNTA, RQ.RESPUESTA AS CORRECTA,";
                _Query = _Query + " (select r.RESPUESTA  from respuesta_quiz r  WITH(NOLOCK) where r.id = RQC.id_respuesta) AS CONTESTADA";
                _Query = _Query + " FROM QUIZ Q WITH(NOLOCK), RESPUESTA_QUIZ RQ WITH(NOLOCK), RESPUESTA_AUDITOR_QUIZ_CAPACITACION RQC WITH(NOLOCK) ,AUDITOR A WITH(NOLOCK)";
                _Query = _Query + " WHERE Q.ID = RQ.ID_QUIZ AND Q.ID_ESTUDIO=21  and CORRECTA = 1 AND RQC.ID_QUIZ = Q.ID AND A.ID_AUDITOR = RQC.ID_AUDITOR";
                _Query = _Query + " AND RQC.FECHA BETWEEN '" + _Fecha_Inicio + "' AND '" + _Fecha_fin + "'";
                _Query = _Query + " ORDER BY RQC.ID_AUDITOR ";
            }
            else
            {
                _Query = "SELECT S.FOLIOCADEM,S.DIRECCION, A.NOMBREAPELLIDO , E.NOMBREESTUDIO, FECHA , UPPER(Q.PREGUNTA) AS PREGUNTA,UPPER((select top 1 respuesta from respuesta_quiz qq with(nolock) where qq.id_quiz = raq.id_quiz and correcta=1)) AS CORRECTA, UPPER(RQ.RESPUESTA) AS CONTESTADA";
                _Query = _Query + " from respuesta_auditor_quiz RAQ WITH(NOLOCK), VISITA V WITH(NOLOCK), AUDITOR A WITH(NOLOCK),QUIZ Q WITH(NOLOCK),RESPUESTA_QUIZ RQ WITH(NOLOCK), ESTUDIOSALA ES WITH(NOLOCK), ESTUDIO E WITH(NOLOCK), SALA S WITH(NOLOCK)";
                _Query = _Query + " where V.ID_VISITA= RAQ.ID_VISITA AND A.ID_AUDITOR = V.ID_AUDITOR AND Q.ID = RAQ.ID_QUIZ AND RQ.ID = RAQ.ID_RESPUESTA AND ES.ID_ESTUDIOSALA = V.ID_ESTUDIOSALA AND ES.ID_ESTUDIO = E.ID_ESTUDIO AND S.ID_SALA = ES.ID_SALA";
                _Query = _Query + " AND RAQ.PUNTAJE = 0";
                _Query = _Query + " AND A.ID_AUDITOR <> 1 AND CAST(V.HORAINICIO AS DATE) BETWEEN '" + _Fecha_Inicio + "' AND '" + _Fecha_fin + "' ";
                if (_Id_Estudio != "0") _Query = _Query + " AND E.ID_ESTUDIO = " + _Id_Estudio;
                if (_Id_Auditor != "0") _Query = _Query + " AND a.id_auditor = " + _Id_Auditor;
                _Query = _Query + " AND E.ID_ESTUDIO <> 21";
            }

            SqlConnection _Conexion_Local = new SqlConnection(ConfigurationManager.ConnectionStrings["SUPI"].ConnectionString);
            DataSet _Ds_Store = new DataSet();
            SqlDataAdapter _Ds_Datos = new SqlDataAdapter(_Query, _Conexion_Local);
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


}