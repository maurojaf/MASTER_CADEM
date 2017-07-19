using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Collections;
using System.Data;
using System.Security.Cryptography;
using System.Configuration;

namespace Galleries.Controller
{
    public class Fotos_controller
    {
        //**** OBTIENE LISTADO DE ESTUDIOS DISPONIBLES FOT ENVIO DE FOTOS *************+
        public DataSet _Get_Carga_Listado_Estudios()
        {
            try
            {
                SqlConnection _Conexion_Local = new SqlConnection(ConfigurationManager.ConnectionStrings["SUPI"].ConnectionString);
                DataSet _Ds_Store = new DataSet();
                SqlDataAdapter _Ds_Datos = new SqlDataAdapter("select NOMBREESTUDIO, ID_ESTUDIO from ESTUDIO WITH(NOLOCK) where FECHACIERRE >=CAST(GETDATE() AS DATE) AND fotos='True' order by NOMBREESTUDIO", _Conexion_Local);
                _Ds_Datos.Fill(_Ds_Store, "DATOS");
                return _Ds_Store;

            }
            catch(Exception)
            {
                return null;
            }        
        }

        // **** OBTIENE LISTADO DE CALENDARIOS POR ESTUDIO ************
        public DataSet _Get_Listado_Calendarios_Estudio(String _Estudio)
        {
            try 
            {
                SqlConnection _Conexion_Local = new SqlConnection(ConfigurationManager.ConnectionStrings["SUPI"].ConnectionString);
                DataSet _Ds_Store = new DataSet();
                SqlDataAdapter _Ds_Datos = new SqlDataAdapter("SELECT top(5) C.DESCRIPCION, ID_CALENDARIO FROM CALENDARIO C WITH(NOLOCK), ESTUDIO E WITH(NOLOCK), CLIENTE CL WITH(NOLOCK) WHERE E.ID_ESTUDIO = C.ID_ESTUDIO AND E.FECHACIERRE>=CAST(GETDATE() AS DATE) AND CL.ID_CLIENTE= E.ID_CLIENTE AND CL.ACTIVO=1 AND E.ID_ESTUDIO=" + _Estudio + " ORDER BY ID_CALENDARIO DESC", _Conexion_Local);
                _Ds_Datos.Fill(_Ds_Store, "DATOS");
                return _Ds_Store;
            }
            catch (Exception) 
            {
                return null;
            }
        }

        //**** OBTIENE EL LISTADO DE TODAS LAS MEDICIONES DEL ESTUDIO OCNSULTADO **********
        public DataSet _Get_Listado_Fotos(String _Calendario, String _Foliocadem)
        {
            try
            {
                String _Query = "SELECT distinct ss.FOLIOCADEM, v.ID_VISITA ,ss.DIRECCION,  cast(LEFT(v.HORAINICIO,11) as date) as DIA,  aa.NOMBREAPELLIDO FROM VISITA v with(nolock), ESTUDIOSALA e with(nolock) ,SALA ss with(nolock), auditor aa with(nolock) WHERE v.ESTADO = 4 and v.CHEQUEO = 0 and v.ID_ESTUDIOSALA= e.ID_ESTUDIOSALA and e.ID_SALA = ss.id_sala and v.ID_AUDITOR= aa.ID_AUDITOR and cast(v.HORAINICIO as date) between (select top 1 FECHADESDE from calendario with(nolock) where  id_calendario='" + _Calendario + "') and (select top 1 FECHAHASTA from calendario with(nolock) where id_calendario='" + _Calendario + "') and e.ID_ESTUDIO= (select top 1 ID_ESTUDIO from calendario where id_calendario='" + _Calendario + "') ";           
            
                if (_Foliocadem != "") 
                {
                    //_Query = "SELECT distinct ss.FOLIOCADEM, v.ID_VISITA ,ss.DIRECCION,  cast(LEFT(v.HORAINICIO,11) as date) as DIA,  aa.NOMBREAPELLIDO FROM VISITA v with(nolock), ESTUDIOSALA e with(nolock) ,SALA ss with(nolock), auditor aa with(nolock) WHERE v.ESTADO = 4 and v.CHEQUEO = 0 and v.ID_ESTUDIOSALA= e.ID_ESTUDIOSALA and e.ID_SALA = ss.id_sala and v.ID_AUDITOR= aa.ID_AUDITOR and cast(v.HORAINICIO as date) between (select top 1 FECHADESDE from calendario with(nolock) where  descripcion='" + _Calendario + "') and (select top 1 FECHAHASTA from calendario with(nolock) where id_calendario='" + _Calendario + "') and e.ID_ESTUDIO= (select top 1 ID_ESTUDIO from calendario where id_calendario='" + _Calendario + "') and ss.FOLIOCADEM =" + _Foliocadem + " ORDER BY DIA desc";
                    _Query = _Query + " and ss.FOLIOCADEM =" + _Foliocadem ;
                }

                _Query = _Query + " ORDER BY DIA desc";

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

        // *** OBTIENE DATOS DE MEDICION DE VISIA CONSULTADA ************
        public DataSet _Get_Datos_Medicion(String _Id_Visita)
        {
            SqlConnection _Conexion_Local = new SqlConnection(ConfigurationManager.ConnectionStrings["SUPI"].ConnectionString);
            DataSet _Ds_Store = new DataSet();
            SqlDataAdapter _Ds_Datos = new SqlDataAdapter("select e.NOMBREESTUDIO,s.FOLIOCADEM,v.ESTADO,s.DIRECCION, c.DESCRIPCION, s.id_sala, v.id_visita from VISITA v with(nolock), SALA s with(nolock), ESTUDIOSALA es with(nolock), ESTUDIO e with(nolock), calendario c with(nolock) where v.ID_ESTUDIOSALA = es.ID_ESTUDIOSALA and s.ID_SALA = es.ID_SALA and e.ID_ESTUDIO = es.ID_ESTUDIO and v.ID_VISITA=" + _Id_Visita + " and c.FECHADESDE<=v.DESDE and c.FECHAHASTA>=v.HASTA and c.ID_ESTUDIO= e.ID_ESTUDIO", _Conexion_Local);
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

        //*** OBTIENE ID DEL ESTUDIO BUSCADO ***********
        public String _Get_Id_Estudio(String _Medicion)
        {
            try
            {
                SqlConnection _Conexion_Local = new SqlConnection(ConfigurationManager.ConnectionStrings["SUPI"].ConnectionString);
                DataSet _Ds_Store = new DataSet();
                SqlDataAdapter _Ds_Datos = new SqlDataAdapter("SELECT TOP 1 ID_ESTUDIO FROM CALENDARIO WITH(NOLOCK) WHERE ID_CALENDARIO ='" + _Medicion + "'", _Conexion_Local);
                _Ds_Datos.Fill(_Ds_Store, "DATOS");
                String _Id = _Ds_Store.Tables[0].Rows[0][0].ToString();
                return _Id;
            }
            catch (Exception)
            {
                return "";
            }
        }

        //*** OBTIENE NOMBRE DEL CALENDARIO BUSCADO ***********
        public String _Get_Nombre_Calendario(String _Medicion)
        {
            try
            {
                SqlConnection _Conexion_Local = new SqlConnection(ConfigurationManager.ConnectionStrings["SUPI"].ConnectionString);
                DataSet _Ds_Store = new DataSet();
                SqlDataAdapter _Ds_Datos = new SqlDataAdapter("SELECT TOP 1 DESCRIPCION FROM CALENDARIO WITH(NOLOCK) WHERE ID_CALENDARIO ='" + _Medicion + "'", _Conexion_Local);
                _Ds_Datos.Fill(_Ds_Store, "DATOS");
                String _Id = _Ds_Store.Tables[0].Rows[0][0].ToString();
                return _Id;
            }
            catch (Exception)
            {
                return "";
            }
        }

    }
}