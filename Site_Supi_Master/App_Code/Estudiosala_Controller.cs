using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Collections;

/// <summary>
/// Descripción breve de Estudiosala_Controller
/// </summary>
public class Estudiosala_Controller
{
	public Estudiosala_Controller(){}


    // **************************************************
    // *************** CARGA DE DATOS *******************
    // **************************************************

    //Obtiene los datos de la tabla Estudios...
    public DataSet _Get_Carga_Estudios()
    {
        try
        {
            SqlConnection _Conexion_Local = new SqlConnection(ConfigurationManager.ConnectionStrings["SUPI"].ConnectionString);
            DataSet _Ds_Store = new DataSet();
            SqlDataAdapter _Ds_Datos = new SqlDataAdapter("SELECT '('+ cast([ID_ESTUDIO] as varchar) + ') ' + [NOMBREESTUDIO] AS VALOR, ID_ESTUDIO FROM [ESTUDIO] WITH(NOLOCK) WHERE FECHACIERRE >= CAST(GETDATE() AS DATE)", _Conexion_Local);
            _Ds_Datos.Fill(_Ds_Store, "DATOS");
            return _Ds_Store;
        }
        catch (Exception)
        {
            return null;
        }
    }



    // **************************************************
    // ************* BUSQUEDA DE ID'S *******************
    // **************************************************

    // BUSCA ID SALA EN SUPI
    public String _Get_Id_Sala_SUPI(String _Folio) 
    {
        try
        {
            SqlConnection _Conexion_Local = new SqlConnection(ConfigurationManager.ConnectionStrings["SUPI"].ConnectionString);
            DataSet _Ds_Store = new DataSet();
            SqlDataAdapter _Ds_Datos = new SqlDataAdapter("SELECT ID_SALA FROM SALA with(nolock) WHERE FOLIOCADEM='" + _Folio + "'", _Conexion_Local);
            _Ds_Datos.Fill(_Ds_Store, "DATOS");
            String _Id = _Ds_Store.Tables[0].Rows[0][0].ToString();
            return _Id;
        }
        catch (Exception)
        {
            return "0";
        }
    }




    // **************************************************************
    // *************** INSERT, UPDATE ESTUDIOSALA *******************
    // **************************************************************

    public Boolean _Insert_Estudiosala(String _Estudio, String _Folio) 
    {
        String _Id_Sala = _Get_Id_Sala_SUPI(_Folio);

        if (_Id_Sala == "0") return false;
        
        String _Existe = _Existe_Estudio_Sala(_Estudio, _Id_Sala);
        String _Query = "";

        if (_Existe == "1") 
        {
            _Query = "INSERT INTO ESTUDIOSALA ([ID_ESTUDIO],[ID_SALA],[FECHAINICIO],[FECHATERMINO],[TIPOMEDICION],[TARGET],[HORA_INICIOO]) VALUES(" + _Estudio + "," + _Id_Sala + ",CAST(GETDATE() AS DATE), CAST(DATEADD(ms,-3,DATEADD(yy,0,DATEADD(yy,DATEDIFF(yy,0,GETDATE())+1,0))) AS DATE), 6, 0 ,'07:00:00')";
        }
        else if (_Existe == "2")
        {
            _Query = "UPDATE ESTUDIOSALA SET [FECHATERMINO]= CAST(DATEADD(ms,-3,DATEADD(yy,0,DATEADD(yy,DATEDIFF(yy,0,GETDATE())+1,0))) AS DATE) WHERE ID_ESTUDIO=" + _Estudio + " AND ID_SALA=" + _Id_Sala;
        }

        try
        {
            SqlConnection _Conexion_Local = new SqlConnection(ConfigurationManager.ConnectionStrings["SUPI"].ConnectionString);
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




    // **************************************************************
    // ******************** VALIDACIONES ****************************
    // **************************************************************

    public String _Existe_Estudio_Sala(String _Id_Estudio, String _Id_Sala) 
    {
        // 1 = Si existe
        // 2 = No existe
        // 0 = error en la query

        try
        {
            SqlConnection _Conexion_Local = new SqlConnection(ConfigurationManager.ConnectionStrings["SUPI"].ConnectionString);
            DataSet _Ds_Store = new DataSet();
            SqlDataAdapter _Ds_Datos = new SqlDataAdapter("SELECT * from ESTUDIOSALA WHERE ID_eSTUDIO="+_Id_Estudio+" AND ID_SALA=" + _Id_Sala, _Conexion_Local);
            _Ds_Datos.Fill(_Ds_Store, "DATOS");
            if (_Ds_Store.Tables[0].Rows.Count > 0)
            {
                //Ya existe...
                return "2";
            }
            else 
            {
                //No existe...
                return "1";
            }     
        }
        catch (Exception)
        {
            //Error Query...
            return "0";
        }
    }

}