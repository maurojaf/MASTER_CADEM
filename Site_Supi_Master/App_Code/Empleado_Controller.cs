using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Collections;

/// <summary>
/// Descripción breve de Empleado_Controller
/// </summary>
public class Empleado_Controller
{
	public Empleado_Controller(){}


    // ******************************************************************
    // ******************** OBTENCION DE DATOS **************************
    // ******************************************************************

    //Obtiene los datos de la tabla Comuna...
    public DataSet _Carga_Comuna()
    {
        try
        {
            SqlConnection _Conexion_Local = new SqlConnection(ConfigurationManager.ConnectionStrings["SUPI_MASTER"].ConnectionString);
            DataSet _Ds_Store = new DataSet();
            SqlDataAdapter _Ds_Datos = new SqlDataAdapter("SELECT '' AS NOMBRE, null AS ID UNION ALL SELECT COM_DESCRIPCION AS NOMBRE, COM_ID AS ID  FROM COMUNA with(nolock) order by NOMBRE", _Conexion_Local);
            _Ds_Datos.Fill(_Ds_Store, "DATOS");
            return _Ds_Store;
        }
        catch (Exception)
        {
            return null;
        }  
    }

    //Obtiene los datos de la tabla Cargo...
    public DataSet _Carga_Cargo()
    {
        try
        {
            SqlConnection _Conexion_Local = new SqlConnection(ConfigurationManager.ConnectionStrings["SUPI_MASTER"].ConnectionString);
            DataSet _Ds_Store = new DataSet();
            SqlDataAdapter _Ds_Datos = new SqlDataAdapter("SELECT '' AS NOMBRE, null AS ID UNION ALL SELECT CARGO_DESCRIPCION AS NOMBRE, CARGO_ID AS ID  FROM CARGO with(nolock) order by id", _Conexion_Local);
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

    //Obtiene los datos de la tabla Clasificacion...
    public DataSet _Carga_Clasificacion()
    {
        try
        {
            SqlConnection _Conexion_Local = new SqlConnection(ConfigurationManager.ConnectionStrings["SUPI_MASTER"].ConnectionString);
            DataSet _Ds_Store = new DataSet();
            SqlDataAdapter _Ds_Datos = new SqlDataAdapter("SELECT '' AS NOMBRE, null AS ID UNION ALL SELECT CLAS_DESCRIPCION AS NOMBRE, CLAS_ID AS ID  FROM CLASIFICACION with(nolock) order by id", _Conexion_Local);
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

    //Obtiene los datos de la tabla Estado del Empleado...
    public DataSet _Carga_EstadoEmpleado()
    {
        try
        {
            SqlConnection _Conexion_Local = new SqlConnection(ConfigurationManager.ConnectionStrings["SUPI_MASTER"].ConnectionString);
            DataSet _Ds_Store = new DataSet();
            SqlDataAdapter _Ds_Datos = new SqlDataAdapter("SELECT '' AS NOMBRE, null AS ID UNION ALL SELECT EST_DESCRIPCION AS NOMBRE, est_ID AS ID  FROM ESTADO_EMPLEADO with(nolock) order by id", _Conexion_Local);
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

        //DataTable _Dt = new DataTable();
        //SqlConnection _Cn = new SqlConnection();
        //_Cn.ConnectionString = ConfigurationManager.ConnectionStrings["SUPI_MASTER"].ConnectionString;

        //try
        //{
        //    _Cn.Open();
        //    SqlCommand _Cmd = new SqlCommand("SELECT DISTINCT(EST_DESCRIPCION) AS VALOR FROM ESTADO_EMPLEADO", _Cn);
        //    SqlDataReader _Rd = _Cmd.ExecuteReader();
        //    _Dt.Load(_Rd);
        //    _Dt.Rows.Add("");
        //    _Rd.Close();
        //    _Cn.Close();
        //    return _Dt;
        //}
        //catch (SqlException E)
        //{
        //    _Cn.Close();
        //    return null;
        //}
        //catch (Exception E)
        //{
        //    _Cn.Close();
        //    return null;
        //}
    }

    //Obtiene los datos de la tabla Grupo...
    public DataSet _Carga_Grupo()
    {
        try
        {
            SqlConnection _Conexion_Local = new SqlConnection(ConfigurationManager.ConnectionStrings["SUPI_MASTER"].ConnectionString);
            DataSet _Ds_Store = new DataSet();
            SqlDataAdapter _Ds_Datos = new SqlDataAdapter("SELECT '' AS NOMBRE, null AS ID UNION ALL SELECT GRUPO_DESCRIPCION AS NOMBRE, grupo_ID AS ID  FROM GRUPO with(nolock) order by id", _Conexion_Local);
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

    //Obtiene los datos de la tabla Finanzas...
    public DataSet _Carga_ClFinanzas()
    {
        try
        {
            SqlConnection _Conexion_Local = new SqlConnection(ConfigurationManager.ConnectionStrings["SUPI_MASTER"].ConnectionString);
            DataSet _Ds_Store = new DataSet();
            SqlDataAdapter _Ds_Datos = new SqlDataAdapter("SELECT '' AS NOMBRE, null AS ID UNION ALL SELECT CLFIN_DESCRIPCION AS NOMBRE, CLFIN_ID AS ID  FROM CL_FINANZAS with(nolock) order by id", _Conexion_Local);
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

    //Obtiene los datos de la tabla Jornada...
    public DataSet _Carga_Jornada()
    {
        try
        {
            SqlConnection _Conexion_Local = new SqlConnection(ConfigurationManager.ConnectionStrings["SUPI_MASTER"].ConnectionString);
            DataSet _Ds_Store = new DataSet();
            SqlDataAdapter _Ds_Datos = new SqlDataAdapter("SELECT '' AS NOMBRE, null AS ID UNION ALL SELECT JORN_DESCRIPCION AS NOMBRE, jorn_ID AS ID  FROM tipo_JORNADA with(nolock) order by id", _Conexion_Local);
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

    //Obtiene los datos de la tabla Encargado...
    public DataSet _Carga_Encargado()
    {
        try
        {
            SqlConnection _Conexion_Local = new SqlConnection(ConfigurationManager.ConnectionStrings["SUPI_MASTER"].ConnectionString);
            DataSet _Ds_Store = new DataSet();
            SqlDataAdapter _Ds_Datos = new SqlDataAdapter("select '',null union all SELECT DISTINCT EMP_NOMBRE + ' ' + EMP_APPATERNO, EMP_ID FROM EMPLEADO with(nolock) ORDER BY 1", _Conexion_Local);
            _Ds_Datos.Fill(_Ds_Store, "DATOS");
            return _Ds_Store;
        }
        catch (Exception)
        {
            return null;
        }
    }

    //oPTIENE LOS DATOS de la tabla nivel....
    public DataSet _Carga_Nivel()
    {
        try
        {
            SqlConnection _Conexion_Local = new SqlConnection(ConfigurationManager.ConnectionStrings["SUPI_MASTER"].ConnectionString);
            DataSet _Ds_Store = new DataSet();
            SqlDataAdapter _Ds_Datos = new SqlDataAdapter("SELECT '' AS NOMBRE, null AS ID UNION ALL SELECT NIVEL_DESCRIPCION AS NOMBRE, nivel_ID AS ID  FROM NIVEL with(nolock) order by id", _Conexion_Local);
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

        //DataTable _Dt = new DataTable();
        //SqlConnection _Cn = new SqlConnection();
        //_Cn.ConnectionString = ConfigurationManager.ConnectionStrings["SUPI_MASTER"].ConnectionString;

        //try
        //{
        //    _Cn.Open();
        //    SqlCommand _Cmd = new SqlCommand("SELECT DISTINCT [NIVEL_DESCRIPCION] AS VALOR FROM NIVEL", _Cn);
        //    SqlDataReader _Rd = _Cmd.ExecuteReader();
        //    _Dt.Load(_Rd);
        //    _Dt.Rows.Add("");
        //    _Rd.Close();
        //    _Cn.Close();
        //    return _Dt;
        //}
        //catch (SqlException E)
        //{
        //    _Cn.Close();
        //    return null;
        //}
        //catch (Exception E)
        //{
        //    _Cn.Close();
        //    return null;
        //}

    }



    //Obtiene los datos del empleado consultado...
    public DataSet _Get_Datos_Empleado(String _Rut)
    {
        try
        {
            SqlConnection _Conexion_Local = new SqlConnection(ConfigurationManager.ConnectionStrings["SUPI_MASTER"].ConnectionString);
            DataSet _Ds_Store = new DataSet();
            SqlDataAdapter _Ds_Datos = new SqlDataAdapter("EXEC _GET_DATOSEMPLEADO2 '" + _Rut + "'", _Conexion_Local);
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

    //GRILLA EMPLEADOS
    public DataTable _Get_Grilla_Emeplado()
    {
        DataTable _Dt = new DataTable();
        SqlConnection _Cn = new SqlConnection();
        _Cn.ConnectionString = ConfigurationManager.ConnectionStrings["SUPI_MASTER"].ConnectionString;

        try
        {
            _Cn.Open();
            SqlCommand _Cmd = new SqlCommand("SELECT e.EMP_ID, e.EMP_RUT AS RUT, E.EMP_APPATERNO AS [AP PATERNO], E.EMP_APMATERNO AS [AP MATERNO], E.EMP_NOMBRE AS NOMBRE, CAST(E.EMP_FONO AS VARCHAR) AS FONO, CO.COM_DESCRIPCION AS COMUNA, E.EMP_CORREO_PERSONAL AS [EMAIL PERSONAL] FROM EMPLEADO E with(nolock), COMUNA CO with(nolock) WHERE E.EMP_COM_ID = CO.COM_ID ORDER BY E.EMP_APPATERNO", _Cn);
            SqlDataReader _Rd = _Cmd.ExecuteReader();
            _Dt.Load(_Rd); ;
            _Rd.Close();
            _Cn.Close();
            return _Dt;
        }
        catch (SqlException E)
        {
            _Cn.Close();
            return null;
        }
        catch (Exception E)
        {
            _Cn.Close();
            return null;
        }
    }

    //exportar auditores
    public DataSet _Get_Exportar_Auditores() 
    {
        try
        {
            String _Query = "SELECT ID_AUDITOR,NOMBREAPELLIDO,QR ,";
            _Query = _Query + " upper((SELECT TOP 1 NOMBREAPELLIDO FROM AUDITOR AA WHERE AA.ID_AUDITOR = A.ID_SUPERVISOR)) AS COORDINADOR, CC.COMUNA_NOMBRE as COMUNA_ORIGEN, ";
            _Query = _Query + " CASE WHEN EST.EST_DESCRIPCION IS NULL THEN 'NO ACTIVO' ELSE EST.EST_DESCRIPCION END AS ESTADO, EMAIL";
            _Query = _Query + " FROM AUDITOR A WITH(NOLOCK) ";
            _Query = _Query + " LEFT JOIN MCADEM.dbo.EMPLEADO EM with(nolock) on em.EMP_ID_SUPI = a.ID_AUDITOR LEFT JOIN MCADEM.dbo.ESTADO_EMPLEADO EST ON EST.EST_ID = EM.EMP_EST_ID";
            _Query = _Query + " , COMUNA CC  WITH(NOLOCK)";
            _Query = _Query + " WHERE  ID_AUDITOR not in (1,327,414) AND CC.COMUNA_ID = A.COMUNA ORDER BY ESTADO, NOMBREAPELLIDO";

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





    // *****************************************************************************
    // ***************************** INSERT EMPLEADOS ******************************
    // *****************************************************************************

    // Ingresar Nuevo empleado...
    public String _Set_Insert_Empleado(String _Rut, String _Nombre, String _Paterno, String _Materno, String _Cargo, String _Clasificacion, String _Estado, String _Comuna, String _Jornada, String _Fecha, String _Finanza, String _Coordinador, String _Grupo, String _Fono, String _Correo_Cadem, String _Correo_Personal, String _Nivel)
    {
        try
        {
            String _Id_Auditor = "";
            if (_Cargo == "19" || _Cargo == "27") // 19 = AUDITOR, 27 = AUDITOR VOLANTE
            {
                if (_Grupo != "14")
                {
                    _Id_Auditor = _Get_ID_Coordinador_SUPI(_Coordinador);
                    if (_Id_Auditor == "0" || _Id_Auditor == null || _Id_Auditor == "")
                    {
                        return "HAS SELECCIONADO LA OPCION AUDITOR Y EL COORDINADOR (ENCARGADO) SELECCIONADO NO EXISTE, FAVOR SELECCIONAR OTRO COORDINADOR (ENCARGADO)";
                    }

                    String _Es_Coordinador = _Get_Es_Coordinador_SUPI(_Id_Auditor);
                    if (_Es_Coordinador != "1")
                    {
                        return "HAS SELECCIONADO UN AUDITOR QUE NO ES COORDINADOR, FAVOR SELECCIONA UN COORDINADOR";
                    }
                }
            }

            String Id = null;
            if (_Cargo == "19" || _Cargo == "20" || _Cargo == "24" || _Cargo == "27") // 19 = AUDITOR, 20 = COORDINADOR, 24 = AUDITOR SENIOR
            {
                Id = _SEt_Insert_Auditor(_Nombre + " " + _Paterno, _Rut, _Comuna, _Id_Auditor, _Paterno, _Cargo, _Fecha, _Nivel, _Grupo, _Correo_Cadem);
            }

            String _Query = "INSERT INTO [EMPLEADO] ([EMP_RUT],[EMP_NOMBRE],[EMP_APPATERNO],[EMP_APMATERNO] ,[EMP_CARGO_ID],[EMP_CLAS_ID],[EMP_EST_ID] ,[EMP_COM_ID],[EMP_JORN_ID],[EMP_FECHA_INGRESO],[EMP_CLFIN_ID],[EMP_COORDINADOR_ID],[EMP_GRUPO_ID] ,[EMP_FONO],[EMP_CORREO],EMP_CORREO_PERSONAL, [EMP_PASS], EMP_NIVEL_ID, EMP_ID_SUPI) ";
            _Query = _Query + " VALUES ('" + _Rut + "', '" + _Nombre + "', '" + _Paterno + "', '" + _Materno + "', " + _Cargo + "," + _Clasificacion + ", " + _Estado + ", " + _Comuna + ", " + _Jornada + ", '" + _Fecha + "', " + _Finanza + ", " + _Coordinador + ", " + _Grupo + ", " + _Fono + ", '" + _Correo_Cadem + "', '" + _Correo_Personal + "', '" + _Rut + "', " + _Nivel + ",'" + Id + "')";
            SqlConnection _Conexion_Local = new SqlConnection(ConfigurationManager.ConnectionStrings["SUPI_MASTER"].ConnectionString);
            DataSet _Ds_Store = new DataSet();
            SqlDataAdapter _Ds_Datos = new SqlDataAdapter(_Query, _Conexion_Local);
            _Ds_Datos.Fill(_Ds_Store);
            return "Success";
        }
        catch (Exception EX)
        {
            return EX.Message;
        }
    }

    // INGRESA AUDITOR/COORDINADOR NUEVO
    public String _SEt_Insert_Auditor(String _Nombre, String _Rut, String _Comuna, String _Id_Coordinador, String _Paterno, String _Cargo, String _Fecha, String _Nivel, String _Grupo, string _Email)
    {
        String _Comuna_Supi = _Get_ID_Comuna_MC(_Comuna);
        String _Zona = _Get_Zona(_Comuna_Supi);
        String _Clasificacion = _Get_ID_Clasificacion_MC(_Nivel);

        try
        {
            if (_Rut.Length == 8)
            {
                _Rut = _Rut.Substring(0, 7);
            }
            else
            {
                _Rut = _Rut.Substring(0, 8);
            }

            String _Pass = _Rut;
            String _Qr = _Nombre + _Pass;
            String _UserName = _Nombre.Substring(0, 1).ToLower() + _Paterno.ToLower();

            if (_Cargo == "20")
            {
                _Id_Coordinador = "1";
            }

            String _Query = "";
            if (_Grupo != "14")
            {
                _Query = "INSERT INTO [AUDITOR] ([NOMBREAPELLIDO],[USERNAME],[PASS],[EMAIL],[NUMEROTELEFONO],[ID_SUPERVISOR],[ACTIVO],[FECHA_INGRESO],[CLASIFICACION],[ZONA] ,[COMUNA],[QR]) VALUES('" + _Nombre + "','" + _UserName + "','" + _Pass + "','" + _Email + "',0," + _Id_Coordinador + ",1,'" + _Fecha + "','" + _Clasificacion + "','" + _Zona + "','" + _Comuna_Supi + "','" + _Qr + "')";
            }
            else
            {
                //AUDITORES ALMACEN
                _Id_Coordinador = "414";    //414 = CANAL TRADICIONAL, 403 = JESSICA ALARCON
                _Query = "INSERT INTO [AUDITOR] ([NOMBREAPELLIDO],[USERNAME],[PASS],[EMAIL],[NUMEROTELEFONO],[ID_SUPERVISOR],[ACTIVO],[FECHA_INGRESO],[CLASIFICACION],[ZONA] ,[COMUNA],[QR]) VALUES('" + _Nombre + "','" + _UserName + "','" + _Pass + "','" + _Email + "',0," + _Id_Coordinador + ",1,'" + _Fecha + "','" + _Clasificacion + "','" + _Zona +"','" + _Comuna_Supi + "','" + _Qr + "')";
            }

            SqlConnection _Conexion_Local = new SqlConnection(ConfigurationManager.ConnectionStrings["SUPI"].ConnectionString);
            DataSet _Ds_Store = new DataSet();
            SqlDataAdapter _Ds_Datos = new SqlDataAdapter(_Query, _Conexion_Local);
            _Ds_Datos.Fill(_Ds_Store);

            _Ds_Store = new DataSet();
            _Ds_Datos = new SqlDataAdapter("select max(id_auditor) from auditor", _Conexion_Local);
            _Ds_Datos.Fill(_Ds_Store, "DATOS");
            String _Id = _Ds_Store.Tables[0].Rows[0][0].ToString();
            return _Id;
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    // Actualiza estado y datos del empleado...
    public String _Set_Update_Empleado(String _Id_User, String _Rut, String _Nombre, String _Paterno, String _Materno, String _Cargo, String _Clasificacion, String _Estado, String _Comuna, String _Jornada, String _Fecha, String _Finanza, String _Coordinador, String _Grupo, String _Fono, String _Correo, String _Correo_Personal, String _Nivel)
    {
        String _Id_Coordinador_SUPI = "";
        if (_Cargo == "19" || _Cargo == "27" || _Cargo == "24") // 19 = AUDITOR, 20 = COORDINADOR, 24 = AUDITOR SENIOR
        {
            if (_Grupo != "14")
            {
                _Id_Coordinador_SUPI = _Get_ID_Coordinador_SUPI(_Coordinador);
                if (_Id_Coordinador_SUPI == "0" || _Id_Coordinador_SUPI == null || _Id_Coordinador_SUPI == "")
                {
                    return "HAS SELECCIONADO LA OPCION CARGO AUDITOR Y EL COORDINADOR (ENCARGADO) SELECCIONADO NO EXISTE, FAVOR SELECCIONAR OTRO COORDINADOR (ENCARGADO)";
                }

                String _Es_Coordinador = _Get_Es_Coordinador_SUPI(_Id_Coordinador_SUPI);
                if (_Es_Coordinador != "1")
                {
                    return "HAS SELECCIONADO UN AUDITOR QUE NO ES COORDINADOR, FAVOR SELECCIONA UN COORDINADOR";
                }
            }
        }

        try
        {
            String _Id_Auditor = null;
            if (_Cargo == "27" || _Cargo == "19" || _Cargo == "20" || _Cargo == "24" || _Cargo == "27") // 19 = AUDITOR, 20 = COORDINADOR, 24 = AUDITOR SENIOR
            {
                _Id_Auditor = _Get_Id_Auditor_SUPI(_Id_User);
                _Set_Update_Auditor(_Nombre + " " + _Paterno, _Rut, _Comuna, _Id_Coordinador_SUPI, _Paterno, _Cargo, _Fecha, _Correo, _Estado, _Fono, _Id_Auditor, _Nivel, _Grupo);
            }

            String _Query = "UPDATE EMPLEADO SET EMP_NOMBRE= '"+_Nombre+"', EMP_APPATERNO ='"+_Paterno+"', EMP_APMATERNO = '"+_Materno+"', EMP_CARGO_ID = "+_Cargo+", EMP_CLAS_ID="+_Clasificacion+", EMP_EST_ID="+_Estado+", EMP_COM_ID="+_Comuna+", EMP_JORN_ID="+_Jornada+", EMP_FECHA_INGRESO='"+_Fecha+"', EMP_CLFIN_ID="+_Finanza+", EMP_COORDINADOR_ID="+_Coordinador+", EMP_GRUPO_ID="+_Grupo+", EMP_FONO="+_Fono+", EMP_CORREO='"+_Correo+"', EMP_CORREO_PERSONAL='"+_Correo_Personal+"', EMP_NIVEL_ID="+_Nivel+", EMP_ID_SUPI='"+_Id_Auditor+"' WHERE EMP_ID='" + _Id_User + "'";
            SqlConnection _Conexion_Local = new SqlConnection(ConfigurationManager.ConnectionStrings["SUPI_MASTER"].ConnectionString);
            DataSet _Ds_Store = new DataSet();
            SqlDataAdapter _Ds_Datos = new SqlDataAdapter(_Query, _Conexion_Local);
            _Ds_Datos.Fill(_Ds_Store);
            return "Success";
        }
        catch (Exception E)
        {
            return E.Message;
        }
    }

    // ACTUALIZA AUDITOR/COORDINADOR
    public String _Set_Update_Auditor(String _Nombre, String _Rut, String _Comuna, String _Id_Coordinador, String _Paterno, String _Cargo, String _Fecha, String _EmailCadem, String _Estado, String _Fono, String _Id_Auditor,String _Nivel, String _GRupo)
    {
        String _Comuna_Supi = _Get_ID_Comuna_MC(_Comuna);
        String _Zona = _Get_Zona(_Comuna_Supi);
        String _Clasificacion = _Get_ID_Clasificacion_MC(_Nivel);

        try
        {
            if (_Cargo == "20")
            {
                _Id_Coordinador = "1";
            }

            //if (_Estado == "1") 
            //{
            //    _Estado = "1";
            //}else{
            //    _Estado = "0";
            //}

            String _Query = "UPDATE AUDITOR SET [NOMBREAPELLIDO]='" + _Nombre + "',[EMAIL]='" + _EmailCadem + "',[NUMEROTELEFONO]=" + _Fono + ",[ACTIVO]=" + _Estado + " ,[COMUNA]='" + _Comuna_Supi + "', CLASIFICACION='" + _Clasificacion + "', [ZONA]='"+_Zona+"'";   
            if (_GRupo != "14")
            {
                _Query = _Query + ",[ID_SUPERVISOR]=" + _Id_Coordinador;
            }
            _Query = _Query + " WHERE ID_AUDITOR=" + _Id_Auditor;

            SqlConnection _Conexion_Local = new SqlConnection(ConfigurationManager.ConnectionStrings["SUPI"].ConnectionString);
            DataSet _Ds_Store = new DataSet();
            SqlDataAdapter _Ds_Datos = new SqlDataAdapter(_Query, _Conexion_Local);
            _Ds_Datos.Fill(_Ds_Store);
            return "Success";     
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }






    // ****************************************************************************
    // ************************ BUSQUEDA DE ID'S **********************************
    // ****************************************************************************

    //OBTIENE ID COMUNA SUPI...
    public String _Get_ID_Comuna_MC(String _Comuna)
    {
        try
        {
            SqlConnection _Conexion_Local = new SqlConnection(ConfigurationManager.ConnectionStrings["SUPI_MASTER"].ConnectionString);
            DataSet _Ds_Store = new DataSet();
            SqlDataAdapter _Ds_Datos = new SqlDataAdapter("SELECT CAST(COM_ID_SUPI AS VARCHAR) FROM COMUNA WHERE COM_ID='" + _Comuna + "'", _Conexion_Local);
            _Ds_Datos.Fill(_Ds_Store, "DATOS");
            String _Id = _Ds_Store.Tables[0].Rows[0][0].ToString();
            return _Id;
        }
        catch (SqlException E)
        {
            return "0";
        }
        catch (Exception E)
        {
            return "0";
        }
    }

    //OBTIENE ID SUPI DE AUDITOR
    public String _Get_Id_Auditor_SUPI(String _Id_User)
    {
        try
        {
            SqlConnection _Conexion_Local = new SqlConnection(ConfigurationManager.ConnectionStrings["SUPI_MASTER"].ConnectionString);
            DataSet _Ds_Store = new DataSet();
            SqlDataAdapter _Ds_Datos = new SqlDataAdapter("SELECT EMP_ID_SUPI FROM EMPLEADO WHERE EMP_ID=" + _Id_User, _Conexion_Local);
            _Ds_Datos.Fill(_Ds_Store, "DATOS");
            String _Id = _Ds_Store.Tables[0].Rows[0][0].ToString();
            return _Id;
        }
        catch (Exception E)
        {
            return null;
        }
    }

    //OBTIENE nombre de clasificacion...
    public String _Get_ID_Clasificacion_MC(String _Nivel)
    {
        try
        {
            SqlConnection _Conexion_Local = new SqlConnection(ConfigurationManager.ConnectionStrings["SUPI_MASTER"].ConnectionString);
            DataSet _Ds_Store = new DataSet();
            SqlDataAdapter _Ds_Datos = new SqlDataAdapter("SELECT [NIVEL_DESCRIPCION] FROM [NIVEL] WHERE [NIVEL_ID]='" + _Nivel + "'", _Conexion_Local);
            _Ds_Datos.Fill(_Ds_Store, "DATOS");
            String _Id = _Ds_Store.Tables[0].Rows[0][0].ToString();
            return _Id;
        }
        catch (SqlException E)
        {
            return "0";
        }
        catch (Exception E)
        {
            return "0";
        }
    }

    //OBTIENE nombre de zona segun comuna seleccionada...
    public String _Get_Zona(String _Comuna)
    {
        try
        {
            SqlConnection _Conexion_Local = new SqlConnection(ConfigurationManager.ConnectionStrings["SUPI"].ConnectionString);
            DataSet _Ds_Store = new DataSet();
            SqlDataAdapter _Ds_Datos = new SqlDataAdapter("select z.ZONA_NOMBRE from ZONA z with(nolock), COMUNA c with(nolock), PROVINCIA p with(nolock), REGION r with(nolock) where z.ZONA = r.ZONA_ID and c.COMUNA_PROVINCIA_ID = p.PROVINCIA_ID and p.PROVINCIA_REGION_ID = r.REGION_ID and c.COMUNA_ID = " + _Comuna , _Conexion_Local);
            _Ds_Datos.Fill(_Ds_Store, "DATOS");
            String _Id = _Ds_Store.Tables[0].Rows[0][0].ToString();
            return _Id;
        }
        catch (Exception E)
        {
            return "0";
        }
    }

    //BUSCA ID COORDINADOR EN SUPI
    public String _Get_Es_Coordinador_SUPI(String _Empleado)
    {
        try
        {
            SqlConnection _Conexion_Local = new SqlConnection(ConfigurationManager.ConnectionStrings["SUPI"].ConnectionString);
            DataSet _Ds_Store = new DataSet();
            SqlDataAdapter _Ds_Datos = new SqlDataAdapter("SELECT ID_SUPERVISOR FROM AUDITOR WHERE ID_AUDITOR ='" + _Empleado + "'", _Conexion_Local);
            _Ds_Datos.Fill(_Ds_Store, "DATOS");
            String _Id = _Ds_Store.Tables[0].Rows[0][0].ToString();
            return _Id;
        }
        catch (SqlException E)
        {
            return "0";
        }
        catch (Exception E)
        {
            return "0";
        }
    }

    //BUSCA ID COORDINADOR EN SUPI
    public String _Get_ID_Coordinador_SUPI(String _Empleado)
    {
        try
        {
            SqlConnection _Conexion_Local = new SqlConnection(ConfigurationManager.ConnectionStrings["SUPI_MASTER"].ConnectionString);
            DataSet _Ds_Store = new DataSet();
            SqlDataAdapter _Ds_Datos = new SqlDataAdapter("SELECT EMP_ID_SUPI FROM EMPLEADO WHERE EMP_ID =" + _Empleado , _Conexion_Local);
            _Ds_Datos.Fill(_Ds_Store, "DATOS");
            String _Id = _Ds_Store.Tables[0].Rows[0][0].ToString();
            return _Id;
        }
        catch (SqlException E)
        {
            return "0";
        }
        catch (Exception E)
        {
            return "0";
        }
    }

    //OBTIENE ID EMPLEADO
    public String _Get_Id_User(String _Rut)
    {
        try
        {
            SqlConnection _Conexion_Local = new SqlConnection(ConfigurationManager.ConnectionStrings["SUPI_MASTER"].ConnectionString);
            DataSet _Ds_Store = new DataSet();
            SqlDataAdapter _Ds_Datos = new SqlDataAdapter("SELECT CAST([EMP_ID] AS VARCHAR) FROM [EMPLEADO] where EMP_RUT='" + _Rut + "'", _Conexion_Local);
            _Ds_Datos.Fill(_Ds_Store, "DATOS");
            String _Id = _Ds_Store.Tables[0].Rows[0][0].ToString();
            return _Id;
        }
        catch (SqlException E)
        {
            return "";
        }
        catch (Exception E)
        {
            return "";
        }

    }




    // ***********************************************************************
    // ****************************** VALIDACIONES ***************************
    // ***********************************************************************

    //Valida si existe empleado consultado...
    public String _Existe_Empleado(string _Rut)
    {
        //0 = No eixste...
        //diferente de 0 y 2 = Si existe...
        //2 = Error en la query...

        try
        {
            SqlConnection _Conexion_Local = new SqlConnection(ConfigurationManager.ConnectionStrings["SUPI_MASTER"].ConnectionString);
            DataSet _Ds_Store = new DataSet();
            SqlDataAdapter _Ds_Datos = new SqlDataAdapter("SELECT COUNT(EMP_RUT) FROM EMPLEADO WHERE EMP_RUT='" + _Rut + "'", _Conexion_Local);
            _Ds_Datos.Fill(_Ds_Store, "DATOS");
            String _Id = _Ds_Store.Tables[0].Rows[0][0].ToString();
            return _Id;
        }
        catch (SqlException E)
        {
            return "2";
        }
        catch (Exception E)
        {
            return "2";
        } 
    }

}