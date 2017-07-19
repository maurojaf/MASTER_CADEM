using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Collections;

/// <summary>
/// Descripción breve de Salas_Controller
/// </summary>
public class Salas_Controller
{
	public Salas_Controller(){}




    // ******************************************************************
    // ********************** CARGA DE DATOS ****************************
    // ******************************************************************

    //Obtiene los datos de la tabla Cadena...
    public DataSet _Get_Cadena() 
    {
        try
        {
            SqlConnection _Conexion_Local = new SqlConnection(ConfigurationManager.ConnectionStrings["SUPI_MASTER"].ConnectionString);
            DataSet _Ds_Store = new DataSet();
            SqlDataAdapter _Ds_Datos = new SqlDataAdapter("SELECT DISTINCT(CAD_DESCRIPCION) AS VALOR, CAD_ID FROM CADENA with(nolock)", _Conexion_Local);
            _Ds_Datos.Fill(_Ds_Store, "DATOS");
            return _Ds_Store;
        }
        catch (Exception)
        {
            return null;
        }
    }

    //Obtiene los datos de la tabla Tamano...
    public DataSet _Get_Carga_Tamano()
    {
        try
        {
            SqlConnection _Conexion_Local = new SqlConnection(ConfigurationManager.ConnectionStrings["SUPI_MASTER"].ConnectionString);
            DataSet _Ds_Store = new DataSet();
            SqlDataAdapter _Ds_Datos = new SqlDataAdapter("SELECT DISTINCT(TAM_DESCRIPCION),TAM_ID AS VALOR FROM TAMANO with(nolock)", _Conexion_Local);
            _Ds_Datos.Fill(_Ds_Store, "DATOS");
            return _Ds_Store;
        }
        catch (Exception)
        {
            return null;
        }
    }

    //Obtiene los datos de la tabla Canal...
    public DataSet _Get_Canal()
    {
        DataTable _Dt = new DataTable();
        SqlConnection _Cn = new SqlConnection();
        _Cn.ConnectionString = ConfigurationManager.ConnectionStrings["SUPI_MASTER"].ConnectionString;

        try
        {
            SqlConnection _Conexion_Local = new SqlConnection(ConfigurationManager.ConnectionStrings["SUPI_MASTER"].ConnectionString);
            DataSet _Ds_Store = new DataSet();
            SqlDataAdapter _Ds_Datos = new SqlDataAdapter("SELECT DISTINCT(CAN_DESCRIPCION) AS VALOR,CAN_ID FROM CANAL with(nolock)", _Conexion_Local);
            _Ds_Datos.Fill(_Ds_Store, "DATOS");
            return _Ds_Store;
        }
        catch (Exception)
        {
            return null;
        }
    }

    //Obtiene los datos de la tabla Comuna...
    public DataSet _Get_Comuna()
    {
        DataTable _Dt = new DataTable();
        SqlConnection _Cn = new SqlConnection();
        _Cn.ConnectionString = ConfigurationManager.ConnectionStrings["SUPI_MASTER"].ConnectionString;

        try
        {
            SqlConnection _Conexion_Local = new SqlConnection(ConfigurationManager.ConnectionStrings["SUPI_MASTER"].ConnectionString);
            DataSet _Ds_Store = new DataSet();
            SqlDataAdapter _Ds_Datos = new SqlDataAdapter("SELECT DISTINCT(COM_DESCRIPCION) AS VALOR, COM_ID  FROM COMUNA with(nolock)", _Conexion_Local);
            _Ds_Datos.Fill(_Ds_Store, "DATOS");
            return _Ds_Store;
        }
        catch (Exception)
        {
            return null;
        }

    }







    // ******************************************************************
    // ********************** OBTENCION DE DATOS ************************
    // ******************************************************************

    //Obtiene los datos de la sala consultada...
    public DataSet _Get_Datos_Sala(String _Folio)
    {
        //ArrayList arrlst = new ArrayList();
        SqlConnection _Cn = new SqlConnection();
        _Cn.ConnectionString = ConfigurationManager.ConnectionStrings["SUPI_MASTER"].ConnectionString;

        try
        {
            SqlConnection _Conexion_Local = new SqlConnection(ConfigurationManager.ConnectionStrings["SUPI_MASTER"].ConnectionString);
            DataSet _Ds_Store = new DataSet();
            SqlDataAdapter _Ds_Datos = new SqlDataAdapter("EXEC _GET_DATOSSALAS '" + _Folio + "'", _Conexion_Local);
            _Ds_Datos.Fill(_Ds_Store, "DATOS");
            return _Ds_Store;
        }
        catch (Exception) 
        {
            return null;
        }
    }

    //Exporta las salas en formato Excel 2007...
    public DataSet _Exportar_Salas() 
    {
        try
        {
            SqlConnection _Conexion_Local = new SqlConnection(ConfigurationManager.ConnectionStrings["SUPI"].ConnectionString);
            DataSet _Ds_Store = new DataSet();
            SqlDataAdapter _Ds_Datos = new SqlDataAdapter("SELECT S.ID_SALA, S.FOLIOCADEM,S.DIRECCION,C.COMUNA_NOMBRE FROM SALA S WITH(NOLOCK), COMUNA C WITH(NOLOCK) WHERE S.ID_COMUNA =C.COMUNA_ID", _Conexion_Local);
            _Ds_Datos.Fill(_Ds_Store, "DATOS");
            return _Ds_Store;
        }
        catch (Exception)
        {
            return null;
        }
    }




    // **************************************************************
    // ***************** INSERT, UPDATE SALAS ***********************
    // **************************************************************

    //Ingresar Nuevo Sala...
    public Boolean _Insert_Sala(String _Folio, String _Direccion, String _Comuna, String _Latitud, String _Longitud, String _Canal, String _M2, String _Cadena, String _Tamano)
    {
        _Direccion = _Direccion.ToUpper().Trim().Replace("'","");
        try
        {
            // ********** EJECUTAR INSERT SALA NUEVA *******************
            String _Query = "INSERT INTO [SALA] ([SALA_FOLIO], [SALA_DIRECCION] ,[SALA_COM_ID], [SALA_LATITUD], [SALA_LONGITUD], [SALA_CANAL_ID], [SALA_M2], [SALA_CAD_ID], [SALA_TAM_ID], [SALA_RANKING], [SALA_ESTSALA_ID], [SALA_SEGMENTO], [SALA_CONTACTO], [SALA_ZNIELSEN_ID], [SALA_FORM_ID], [SALA_NOMBRECADEM]) ";
            _Query = _Query + " VALUES (" + _Folio + ",'" + _Direccion + "'," + _Comuna + ",'" + _Latitud + "','" + _Longitud + "'," + _Canal + ",'" + _M2 + "'," + _Cadena + "," + _Tamano + ", 0, 1, NULL, NULL, NULL, NULL,'" + _Direccion + "')";
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

    //Actualiza estado y datos de la sala...
    public Boolean _Set_Update_Sala(String _Folio, String _Direccion, String _Comuna, String _Latitud, String _Longitud, String _Canal, String _M2, String _Cadena, String _Tamano)
    {
        _Direccion = _Direccion.ToUpper().Trim().Replace("'", "");
        try
        {
            // *********** SALA ACTUALIZA EN MCADEM ************
            String _Query = "UPDATE [SALA] SET [SALA_NOMBRECADEM]= '" + _Direccion + "', [SALA_DIRECCION]= '" + _Direccion + "', [SALA_COM_ID]= " + _Comuna + " , [SALA_LATITUD]= '" + _Latitud + "', [SALA_LONGITUD]= '" + _Longitud + "',[SALA_CANAL_ID]= " + _Canal + ",[SALA_M2]= " + _M2 + ", [SALA_CAD_ID]=" + _Cadena + ", [SALA_TAM_ID]=" + _Tamano + " WHERE SALA_FOLIO='" + _Folio + "'";
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





    // ***************************************************************
    // ********************* ID SOLICITADOS **************************
    // ***************************************************************

    // OBTIENE ID SALA 
    public String _Get_Id_Sala_MCadem(String _Sala)
    {
        try
        {
            SqlConnection _Conexion_Local = new SqlConnection(ConfigurationManager.ConnectionStrings["SUPI_MASTER"].ConnectionString);
            DataSet _Ds_Store = new DataSet();
            SqlDataAdapter _Ds_Datos = new SqlDataAdapter("SELECT CAST(SALA_ID AS VARCHAR) FROM SALA WHERE SALA_FOLIO ='" + _Sala + "'", _Conexion_Local);
            _Ds_Datos.Fill(_Ds_Store, "DATOS");
            String _Id = _Ds_Store.Tables[0].Rows[0][0].ToString();
            return _Id;
        }
        catch (Exception)
        {
            return "0";
        }
    }

   



    // ***************************************************************
    // ********************* VALIDACIONES ****************************
    // ***************************************************************

    //Valida si existe sala consultada...
    
    public int _Existe_Sala(String _FolioCadem)
    {
        //0 = No eixste...
        //1 = Si existe...
        //2 = Error en la query...
        try
        {
            //******** VALIDA SI EXISTE EN SUPI ************
            SqlConnection _Conexion_Local = new SqlConnection(ConfigurationManager.ConnectionStrings["SUPI"].ConnectionString);
            DataSet _Ds_Store = new DataSet();
            SqlDataAdapter _Ds_Datos = new SqlDataAdapter("SELECT * FROM SALA WHERE FOLIOCADEM = " + _FolioCadem, _Conexion_Local);
            _Ds_Datos.Fill(_Ds_Store, "0");
            String _Existe_Supi = _Ds_Store.Tables[0].Rows.Count.ToString();

            //******** VALIDA SI EXISTE EN MCADEM ************
            _Conexion_Local = new SqlConnection(ConfigurationManager.ConnectionStrings["SUPI_MASTER"].ConnectionString);
            _Ds_Store = new DataSet();
            _Ds_Datos = new SqlDataAdapter("SELECT * FROM SALA WHERE SALA_FOLIO = " + _FolioCadem, _Conexion_Local);
            _Ds_Datos.Fill(_Ds_Store, "0");
            String _Existe_Master = _Ds_Store.Tables[0].Rows.Count.ToString();

            if (_Existe_Supi == "0" && _Existe_Master == "0")
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }
        catch (Exception)
        {
            return 2;
        }
    }

    // VALIDAD SI EXISTE DIRECCION 
    public int _Existe_Direccion(String _Direccion, String _Comuna, String _Folio)
    {
        //0 = No eixste...
        //1 = Si existe...
        //2 = Error en la query...
        try
        {
            //********* VALIDA SI EXISTE EN MACDEM ************
            SqlConnection _Conexion_Local = new SqlConnection(ConfigurationManager.ConnectionStrings["SUPI_MASTER"].ConnectionString);
            DataSet _Ds_Store = new DataSet();
            SqlDataAdapter _Ds_Datos = new SqlDataAdapter("SELECT * FROM SALA with(nolock) WHERE SALA_DIRECCION = '" + _Direccion + "' and [SALA_COM_ID] = " + _Comuna + " AND SALA_FOLIO NOT IN(" + _Folio + ")", _Conexion_Local);
            _Ds_Datos.Fill(_Ds_Store, "0");
            String _Existe_Cadem = _Ds_Store.Tables[0].Rows.Count.ToString();

            //********* VALIDA SI EXISTE EN SUPI ************
            _Conexion_Local = new SqlConnection(ConfigurationManager.ConnectionStrings["SUPI"].ConnectionString);
            _Ds_Store = new DataSet();
            _Ds_Datos = new SqlDataAdapter("SELECT * FROM SALA with(nolock) WHERE DIRECCION='" + _Direccion + "' AND [ID_COMUNA] = ( select COM_ID_SUPI from [MCADEM].[dbo].[COMUNA] with(nolock) where [COM_ID] = " + _Comuna + ") AND FOLIOCADEM NOT IN(" + _Folio + ")", _Conexion_Local);
            _Ds_Datos.Fill(_Ds_Store, "0");
            String _Existe_Supi = _Ds_Store.Tables[0].Rows.Count.ToString();

            if (_Existe_Cadem == "0" && _Existe_Supi == "0")
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }
        catch (Exception)
        {
            return 2;
        }
    }




}