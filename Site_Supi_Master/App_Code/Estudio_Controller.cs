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
/// Descripción breve de Estudio_Controller
/// </summary>
public class Estudio_Controller
{
	public Estudio_Controller(){}


    //**************************************************************
    //****************** OBTENCION DE DATOS ************************
    //**************************************************************

    //Obtiene los datos de la tabla Cliente...
    public DataSet _Get_Carga_Cliente()
    {
        try
        {
            SqlConnection _Conexion_Local = new SqlConnection(ConfigurationManager.ConnectionStrings["SUPI"].ConnectionString);
            DataSet _Ds_Store = new DataSet();
            SqlDataAdapter _Ds_Datos = new SqlDataAdapter("SELECT distinct [NOMBRECLIENTE] AS VALOR, id_cliente FROM [CLIENTE] with(nolock)", _Conexion_Local);
            _Ds_Datos.Fill(_Ds_Store, "DATOS");
            return _Ds_Store;
        }
        catch (Exception)
        {
            return null;
        }
 
    }

    //Obtiene los datos de la tabla Estudio...
    public DataSet _Get_Carga_EstadoEstudio()
    {
        try
        {
            SqlConnection _Conexion_Local = new SqlConnection(ConfigurationManager.ConnectionStrings["SUPI_MASTER"].ConnectionString);
            DataSet _Ds_Store = new DataSet();
            SqlDataAdapter _Ds_Datos = new SqlDataAdapter("SELECT DISTINCT(EST_DESCRIPCION) AS VALOR, [EST_ID] FROM ESTADO_ESTUDIO with(nolock)", _Conexion_Local);
            _Ds_Datos.Fill(_Ds_Store, "DATOS");
            return _Ds_Store;
        }
        catch (Exception)
        {
            return null;
        }
    }

    //Obtiene los datos del estudio consultado por SUPI o por MCADEM...
    public DataSet _Get_Busca_Datos_Estudio(String _Estudio)
    {
        try
        {
            SqlConnection _Conexion_Local = new SqlConnection(ConfigurationManager.ConnectionStrings["SUPI"].ConnectionString);
            DataSet _Ds_Store = new DataSet();
            SqlDataAdapter _Ds_Datos = new SqlDataAdapter("select id_estudio,nombreestudio, id_Cliente, case when (select top(1) EST_RUC from mcadem.dbo.estudio with(nolock) where EST_SUPI_ID = " + _Estudio + ") is null then 0 else (select top(1) EST_RUC from mcadem.dbo.estudio with(nolock) where EST_SUPI_ID = " + _Estudio + ") end  as ranking, case when (fechacierre>= cast(getdate() as date)) then 1 else 2 end as estado from estudio with(nolock) where [ID_ESTUDIO] = " + _Estudio, _Conexion_Local);
            _Ds_Datos.Fill(_Ds_Store, "DATOS");
            if (_Ds_Store.Tables[0].Rows.Count > 0)
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

    // OBTIENE TODOS LOS TIEMPOS
    public DataSet _Get_Tiempos()
    {
        try
        {
            SqlConnection _Conexion_Local = new SqlConnection(ConfigurationManager.ConnectionStrings["SUPI_MASTER"].ConnectionString);
            DataSet _Ds_Store = new DataSet();
            SqlDataAdapter _Ds_Datos = new SqlDataAdapter("SELECT DISTINCT TIEMPO AS Description FROM TIEMPOS WITH(NOLOCK) order by tiempo asc", _Conexion_Local);
            _Ds_Datos.Fill(_Ds_Store, "DATOS");
            return _Ds_Store;
        }
        catch (Exception)
        {
            return null;
        }
    }

    // OBTIENE TIEMPO ESPECIFICO
    public DataSet _Get_Tiempo_Especifico(String _Tamano, String _Id_Estudio)
    {
        try
        {
            SqlConnection _Conexion_Local = new SqlConnection(ConfigurationManager.ConnectionStrings["SUPI_MASTER"].ConnectionString);
            DataSet _Ds_Store = new DataSet();
            SqlDataAdapter _Ds_Datos = new SqlDataAdapter("SELECT left(ET.TIEMPO,8) as TIEMPO FROM TAMANO T WITH(NOLOCK) LEFT JOIN ESTUDIO_TIEMPO ET WITH(NOLOCK) ON ET.ID_TAMANO = T.TAM_ID AND ET.ID_ESTUDIO_SUPI=" + _Id_Estudio + " where t.TAM_DESCRIPCION='" + _Tamano + "'", _Conexion_Local);
            _Ds_Datos.Fill(_Ds_Store, "DATOS");
            return _Ds_Store;
        }
        catch (Exception)
        {
            return null;
        }
    }

    // EXPORTAR TODOS LOS ESTUDIOS A EXCEL
    public DataSet _Get_Exportar_Estudios() 
    {
        try
        {
            SqlConnection _Conexion_Local = new SqlConnection(ConfigurationManager.ConnectionStrings["SUPI"].ConnectionString);
            DataSet _Ds_Store = new DataSet();
            SqlDataAdapter _Ds_Datos = new SqlDataAdapter("SELECT E.ID_ESTUDIO, E.NOMBREESTUDIO ,C.NOMBRECLIENTE  FROM ESTUDIO E WITH(NOLOCK), CLIENTE C WITH(NOLOCK) WHERE E.ID_CLIENTE= C.ID_CLIENTE AND E.FECHACIERRE>=CAST(GETDATE() AS DATE) ORDER BY C.NOMBRECLIENTE", _Conexion_Local);
            _Ds_Datos.Fill(_Ds_Store, "DATOS");
            return _Ds_Store;
        }
        catch (Exception)
        {
            return null;
        }
    }

    // OBTIENE TODOS LOS TIPOS DE TAMAÑOS
    public DataSet _Get_Tamano_tIP(String _Is_Estudio)
    {
        try
        {
            SqlConnection _Conexion_Local = new SqlConnection(ConfigurationManager.ConnectionStrings["SUPI_MASTER"].ConnectionString);
            DataSet _Ds_Store = new DataSet();
            SqlDataAdapter _Ds_Datos = new SqlDataAdapter("SELECT T.TAM_DESCRIPCION AS TAMANO , ET.TIEMPO FROM TAMANO T WITH(NOLOCK) LEFT JOIN ESTUDIO_TIEMPO ET WITH(NOLOCK) ON ET.ID_TAMANO = T.TAM_ID AND ET.ID_ESTUDIO_SUPI=" + _Is_Estudio, _Conexion_Local);
            _Ds_Datos.Fill(_Ds_Store, "DATOS");
            return _Ds_Store;
        }
        catch (Exception)
        {
            return null;
        }
    }





    // **************************************************************
    // ******************* INSERT SALAS *****************************
    // **************************************************************

    // INSERTA O ACTUALIZA ESTUDIO
    public Boolean _Insert_O_Update_Estudio(String _Estudio, String _Descripcion, String _Cliente, String _Ranking, String _Estado)
    {
        try
        {
            if (_Ranking == "-") _Ranking = "0";
            int _Existe_Estudio_ = _Existe_Estudio(_Descripcion, _Estudio);
            String _Query = "";

            if (_Existe_Estudio_ == 0)
            {
                _Query = "INSERT INTO [ESTUDIO]([EST_SUPI_ID],[EST_DESCRIPCION],[EST_CLI_ID],[EST_RUC],[EST_ESTADO]) VALUES(" + _Estudio + ", '" + _Descripcion + "', " + _Cliente + "," + _Ranking + ", " + _Estado + ")";
            }
            else
            {
                _Query = "UPDATE [ESTUDIO] SET EST_DESCRIPCION='" + _Descripcion + "' ,[EST_CLI_ID]=" + _Cliente + ",[EST_RUC]=" + _Ranking + ",[EST_ESTADO]=" + _Estado + " WHERE [EST_SUPI_ID]=" + _Estudio;
            }

            SqlConnection _Conexion_Local = new SqlConnection(ConfigurationManager.ConnectionStrings["SUPI_MASTER"].ConnectionString);
            DataSet _Ds_Store = new DataSet();
            SqlDataAdapter _Ds_Datos = new SqlDataAdapter(_Query, _Conexion_Local);
            _Ds_Datos.Fill(_Ds_Store);
            return true;
        }
        catch (Exception) 
        {
            return false;
        }
    }

    // ACTUALIZA TAMANO POR ESTUDIO
    public Boolean _Update_Tamano_Estudio(String _Tamano, String _Tiempo, String _Id_Estudio)
    {
        try
        {
            SqlConnection _Conexion_Local = new SqlConnection(ConfigurationManager.ConnectionStrings["SUPI_MASTER"].ConnectionString);
            DataSet _Ds_Store = new DataSet();
            SqlDataAdapter _Ds_Datos = new SqlDataAdapter("SELECT [TAM_ID] from tamano with(nolock) WHERE [TAM_DESCRIPCION]= '" + _Tamano + "'", _Conexion_Local);
            _Ds_Datos.Fill(_Ds_Store, "DATOS");
            String _Id_Tamano = _Ds_Store.Tables[0].Rows[0][0].ToString(); ;

            _Ds_Store = new DataSet();
            _Ds_Datos = new SqlDataAdapter("SELECT * FROM ESTUDIO_TIEMPO with(nolock) WHERE ID_ESTUDIO_SUPI= " + _Id_Estudio + " AND ID_TAMANO in(select tt.TAM_ID from TAMANO tt with(nolock) where tt.TAM_DESCRIPCION= '" + _Tamano + "')", _Conexion_Local);
            _Ds_Datos.Fill(_Ds_Store, "DATOS");

            if (_Ds_Store.Tables["DATOS"].Rows.Count > 0)
            {
                //actualiza
                _Ds_Store = new DataSet();
                _Ds_Datos = new SqlDataAdapter("update estudio_tiempo set tiempo='" + _Tiempo + "' where id_Estudio_supi=" + _Id_Estudio + " and id_tamano=" + _Id_Tamano, _Conexion_Local);
                _Ds_Datos.Fill(_Ds_Store, "DATOS");
            }
            else
            {
                //crea
                _Ds_Store = new DataSet();
                _Ds_Datos = new SqlDataAdapter("INSERT INTO ESTUDIO_TIEMPO (ID_ESTUDIO_SUPI, ID_TAMANO, TIEMPO) VALUES(" + _Id_Estudio + "," + _Id_Tamano + ",'" + _Tiempo + "')", _Conexion_Local);
                _Ds_Datos.Fill(_Ds_Store, "DATOS");
            }
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }





    // ***************************************************************
    // ****************** BUSQUEDA DE ID'S ***************************
    // ***************************************************************

    // OBTIENE ID ESTUDIO(MCADEM) DE MCADEM
    public String _Get_IdEstudio(String _Estudio)
    {
        SqlConnection _Cn = new SqlConnection();
        _Cn.ConnectionString = ConfigurationManager.ConnectionStrings["SUPI_MASTER"].ConnectionString;
        try
        {
            _Cn.Open();
            SqlCommand _Cmd = new SqlCommand("SELECT CAST(EST_ID AS VARCHAR) FROM ESTUDIO WHERE '('  + CAST(EST_SUPI_ID AS VARCHAR) + ') ' + EST_DESCRIPCION ='" + _Estudio + "'", _Cn);
            SqlDataReader _Rd = _Cmd.ExecuteReader();
            _Rd.Read();
            String _Id = (_Rd.GetString(0));
            _Rd.Close();
            _Cn.Close();
            return _Id;
        }
        catch (SqlException E)
        {
            _Cn.Close();
            return "0";
        }
        catch (Exception E)
        {
            _Cn.Close();
            return "0";
        }
    }





    // ***************************************************************
    // ********************* VALIDACIONES ****************************
    // ***************************************************************

    //Valida si existe estudio consultado...
    public int _Existe_Estudio(String _Estudio, String Id_Supi)
    {
        //0 = No eixste...
        //1 = Si existe...
        //2 = Error en la query...
        try
        {
            SqlConnection _Conexion_Local = new SqlConnection(ConfigurationManager.ConnectionStrings["SUPI_MASTER"].ConnectionString);
            DataSet _Ds_Store = new DataSet();
            SqlDataAdapter _Ds_Datos = new SqlDataAdapter("SELECT * FROM ESTUDIO WHERE EST_SUPI_ID=" + Id_Supi, _Conexion_Local);
            _Ds_Datos.Fill(_Ds_Store, "DATOS");

            if(_Ds_Store.Tables[0].Rows.Count>0){
                return 1;
            }else{
                return 0;
            }        
        }
        catch (Exception)
        {
            return 2;
        }
    }


 
}