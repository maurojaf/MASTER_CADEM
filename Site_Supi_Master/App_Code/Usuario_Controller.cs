using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Collections;
using System.Security.Cryptography;
using System.Security;
using System.Text;

public class Usuario_Controller
{
	public Usuario_Controller()
	{
	}

    //Obtiene todos los usuario registrados en el sistema
    public DataSet _Get_Usuarios()
    {
        try
        {
            SqlConnection _Conexion_Local = new SqlConnection(ConfigurationManager.ConnectionStrings["SUPI_MASTER"].ConnectionString);
            DataSet _Ds_Store = new DataSet();
            SqlDataAdapter _Ds_Datos = new SqlDataAdapter("SELECT ([EMP_NOMBRE] + ' ' + [EMP_APPATERNO] + ' ' + [EMP_RUT]) AS VALOR, EMP_ID FROM [EMPLEADO] order by EMP_NOMBRe", _Conexion_Local);
            _Ds_Datos.Fill(_Ds_Store, "DATOS");         
            return _Ds_Store;
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
        //    SqlCommand _Cmd = new SqlCommand("SELECT ([EMP_NOMBRE] + ' ' + [EMP_APPATERNO] + ' ' + [EMP_RUT]) AS VALOR, EMP_ID FROM [EMPLEADO] order by EMP_NOMBRE", _Cn);
        //    SqlDataReader _Rd = _Cmd.ExecuteReader();
        //    _Dt.Load(_Rd);
        //    _Dt.Rows.Add("");
        //    _Cn.Close();
        //    return _Dt;
        //}
        //catch (SqlException E)
        //{
        //    return null;
        //}
    }

    //Obtiene true o false, true = tiene acceso a la pagina consultada, false = no tiene acceso a pagina consultada
    public Boolean _Tiene_Acceso_Pagina(String _Usuario, String _Pagina)
    {
        SqlConnection _Cn = new SqlConnection();
        _Cn.ConnectionString = ConfigurationManager.ConnectionStrings["SUPI_MASTER"].ConnectionString;

        try
        {
            _Cn.Open();
            SqlCommand _Cmd = new SqlCommand("SELECT CAST(PP.VALOR AS VARCHAR) FROM [PAGINA_ROL_USER] PP, PAGINA PA, ROLES PE, EMPLEADO EM WHERE PP.PAG_ID = PA.PAG_ID AND PP.USER_ID = EM.EMP_ID and PE.PERM_ID=1 AND PE.PERM_ID = PP.PERM_ID AND PA.PAG_URL = @Pagina AND em.EMP_RUT=@Rut", _Cn);
            _Cmd.Parameters.AddWithValue("@Pagina", _Pagina);
            _Cmd.Parameters.AddWithValue("@Rut", _Usuario);
            SqlDataReader _Rd = _Cmd.ExecuteReader();
            _Rd.Read();
            String _Acceso = _Rd.GetString(0);
            _Rd.Close();
            _Cn.Close();

            if (_Acceso == "1") { return true; } else { return false; }
        }
        catch (SqlException E)
        {
            return false;
        }
        catch (Exception E)
        {
            return false;
        }
    }

    //Obtiene el valor 'Admin'...
    public Boolean _Get_Admin(String _Usuario)
    {
        SqlConnection _Cn = new SqlConnection();
        _Cn.ConnectionString = ConfigurationManager.ConnectionStrings["SUPI_MASTER"].ConnectionString;
        try
        {
            _Cn.Open();
            SqlCommand _Cmd = new SqlCommand("SELECT CAST(COUNT(RUT) AS VARCHAR) FROM ADMIN WHERE RUT='" + _Usuario + "'", _Cn);
            SqlDataReader _Rd = _Cmd.ExecuteReader();
            _Rd.Read();
            String _Id = (_Rd.GetString(0));
            _Cn.Close();
            if (_Id == "1") { return true; } else { return false; }
        }
        catch (SqlException E)
        {
            return false;
        }
    }

    public DataSet _Get_Rut(String _Ide)
    {
        try
        {
            SqlConnection _Conexion_Local = new SqlConnection(ConfigurationManager.ConnectionStrings["SUPI_MASTER"].ConnectionString);
            DataSet _Ds_Store = new DataSet();
            SqlDataAdapter _Ds_Datos = new SqlDataAdapter("SELECT EMP_RUT, EMP_PASS from EMPLEADO WHERE EMP_ID =" + _Ide, _Conexion_Local);
            _Ds_Datos.Fill(_Ds_Store, "DATOS");
            return _Ds_Store;
        }
        catch (Exception)
        {
            return null;
        }
    }



    //Actualiza los Roles al usuario....
    public Boolean _Insert_Acceso_Sistema(String _Rut, String _Pagina, String Permiso, Boolean _Check)
    {
        try
        {
            //String _Rut = _Get_Rut_Usuario(_Usuario);
            String _Id_Pagina = _Get_Id_Pagina(_Pagina);
            String _Id_Permiso = _Get_Id_Permiso(Permiso);
            String _Id_Usuario = _Get_Id_User(_Rut);

            SqlConnection _Cn = new SqlConnection();
            _Cn.ConnectionString = ConfigurationManager.ConnectionStrings["SUPI_MASTER"].ConnectionString;

            String _Valor = "0";
            if (_Check)
            {
                _Valor = "1";
            }
     
            _Cn.Open();
            SqlCommand _Cmd = new SqlCommand("INSERT INTO [PAGINA_ROL_USER] ([PAG_ID],[PERM_ID],[USER_ID],[VALOR]) VALUES (@_Pagina, @_Rol, @_Usuario, @_Valor)", _Cn);
            _Cmd.Parameters.AddWithValue("@_Pagina", int.Parse(_Id_Pagina));
            _Cmd.Parameters.AddWithValue("@_Rol", int.Parse(_Id_Permiso));
            _Cmd.Parameters.AddWithValue("@_Usuario", int.Parse(_Id_Usuario));
            _Cmd.Parameters.AddWithValue("@_Valor", int.Parse(_Valor));

            _Cmd.ExecuteNonQuery();
            return true;
        }
        catch (SqlException E)
        {
            return false;
        }
        catch (Exception E)
        {
            return false;
        }
    }

    public Boolean _Acceso_Sistema_MCADEM(String _Usuario)
    {
        try
        {
            SqlConnection _Conexion_Local = new SqlConnection(ConfigurationManager.ConnectionStrings["SUPI_MASTER"].ConnectionString);
            DataSet _Ds_Store = new DataSet();
            SqlDataAdapter _Ds_Datos = new SqlDataAdapter("SELECT CAST(PRS.VALOR AS VARCHAR) FROM PAGINA_ROL_USER PRS, EMPLEADO EM WHERE PRS.USER_ID = EM.EMP_ID AND PRS.PERM_ID=7 AND EM.EMP_ID=" + _Usuario, _Conexion_Local);
            _Ds_Datos.Fill(_Ds_Store, "DATOS");
            String _Acceso = _Ds_Store.Tables[0].Rows[0][0].ToString();
            if (_Acceso == "1")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        catch (Exception)
        {
            return false;
        }


        //SqlConnection _Cn = new SqlConnection();
        //_Cn.ConnectionString = ConfigurationManager.ConnectionStrings["SUPI_MASTER"].ConnectionString;

        //try
        //{
        //    _Cn.Open();
        //    SqlCommand _Cmd = new SqlCommand("SELECT CAST(PRS.VALOR AS VARCHAR) FROM PAGINA_ROL_USER PRS, EMPLEADO EM WHERE PRS.USER_ID = EM.EMP_ID AND PRS.PERM_ID=7 AND EM.EMP_ID=@_User", _Cn);
        //    _Cmd.Parameters.AddWithValue("@_User", int.Parse(_Usuario));
        //    SqlDataReader _Rd = _Cmd.ExecuteReader();
        //    _Rd.Read();
        //    String _Acceso = _Rd.GetString(0);
        //    _Rd.Close();
        //    _Cn.Close();

        //    if (_Acceso == "1")
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }

        //}

        //catch (Exception)
        //{
        //    return false;
        //}
    }

    public Boolean _Acceso_Sistema_panel_control(String _Usuario)
    {
        try
        {
            SqlConnection _Conexion_Local = new SqlConnection(ConfigurationManager.ConnectionStrings["SUPI_MASTER"].ConnectionString);
            DataSet _Ds_Store = new DataSet();
            SqlDataAdapter _Ds_Datos = new SqlDataAdapter("SELECT * from EMPLEADO_USUARIO_SUPI where EMP_ID=" + _Usuario, _Conexion_Local);
            _Ds_Datos.Fill(_Ds_Store, "DATOS");

            if (_Ds_Store.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            else 
            {
                return false;
            }

        }
        catch (Exception)
        {
            return false;
        }
    }


    public Boolean _Existe_usuario_panelControl(String _Usuario_ID) 
    {
        try
        {
            SqlConnection _Conexion_Local = new SqlConnection(ConfigurationManager.ConnectionStrings["SUPI_MASTER"].ConnectionString);
            DataSet _Ds_Store = new DataSet();
            SqlDataAdapter _Ds_Datos = new SqlDataAdapter("SELECT EMP_ID_SUPI FROM [EMPLEADO_USUARIO_SUPI] WITH(NOLOCK) WHERE [EMP_ID]=" + _Usuario_ID, _Conexion_Local);
            _Ds_Datos.Fill(_Ds_Store, "DATOS");

            if (_Ds_Store.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        catch (Exception)
        {
            return false;
        }
    }


    public Boolean _Nuevo_UsuarioPanelControl_Supi(String _Usuario_ID, Boolean _Otorga_Acceso, Boolean _Nueva_pass)
    {
        try
        {
            DataSet _Ds_Nombres = _Get_Datos_empleado_panel_supi(_Usuario_ID);
            if (_Ds_Nombres != null)
            {
                String _Nombre = _Ds_Nombres.Tables[0].Rows[0][0].ToString() + " " + _Ds_Nombres.Tables[0].Rows[0][1].ToString();

                // cuando se crea un nuevo usuario la pass por defecto es el rut, por eso asume que la pass es el rut, va a buscar la pass directamente.....
                String _Rut = _Ds_Nombres.Tables[0].Rows[0][2].ToString();
                String _Pass = CalculateMD5Hash(_Rut);

                // ** ingresa nuevo usuario panel  control supi
                String _Query = "INSERT INTO [dbo].[USUARIO] ([ID_CLIENTE] ,[NOMBRE],[USUARIO],[PASSWORD],[ID_TIPO],[NIVEL],[TIPO_PERFIL]) ";
                _Query = _Query + " VALUES (7,'" + _Nombre + "','" + _Rut + "','" + _Pass + "', 18, 1, 3)";
                SqlConnection _Conexion_MCADEM = new SqlConnection(ConfigurationManager.ConnectionStrings["SUPI"].ConnectionString);
                DataSet _Ds_Store = new DataSet();
                SqlDataAdapter _Ds_Datos = new SqlDataAdapter(_Query, _Conexion_MCADEM);
                _Ds_Datos.Fill(_Ds_Store);

                // ** obtiene nuevo id de usuario recien creado (panel control supi)
                _Query = "SELECT ID_USUARIO FROM USUARIO WHERE PASSWORD='" + _Pass + "' AND USUARIO ='" + _Rut + "'";
                _Ds_Store = new DataSet();
                _Ds_Datos = new SqlDataAdapter(_Query, _Conexion_MCADEM);
                _Ds_Datos.Fill(_Ds_Store, "D");
                String Id_Supi = _Ds_Store.Tables["D"].Rows[0][0].ToString();
                
                // ** ASOCIA ID MACDEM CON ID SUPI
                _Query = "INSERT INTO [dbo].[EMPLEADO_USUARIO_SUPI] ([EMP_ID],[EMP_ID_SUPI]) VALUES ("+_Usuario_ID+","+ Id_Supi+")";
                _Conexion_MCADEM = new SqlConnection(ConfigurationManager.ConnectionStrings["SUPI_MASTER"].ConnectionString);
                _Ds_Store = new DataSet();
                _Ds_Datos = new SqlDataAdapter(_Query, _Conexion_MCADEM);
                _Ds_Datos.Fill(_Ds_Store);

                if (!_Nueva_pass) 
                {
                    _Conexion_MCADEM = new SqlConnection(ConfigurationManager.ConnectionStrings["SUPI_MASTER"].ConnectionString);
                    _Ds_Store = new DataSet();
                    _Ds_Datos = new SqlDataAdapter("exec sp_nuevo_usuario_web '" + _Rut + "','" + _Rut + "'", _Conexion_MCADEM);
                    _Ds_Datos.Fill(_Ds_Store);
                }

                return true;
            }
            else
            {
                return false;
            }
        }
        catch (Exception) { return false; }
    }

    public Boolean _Elimina_UsuarioPanelControl_Supi(String _Usuario_ID)
    {
        try
        {
            DataSet _Ds_Nombres = _Get_Datos_empleado_panel_supi(_Usuario_ID);
            if (_Ds_Nombres != null)
            {
                String _Nombre = _Ds_Nombres.Tables[0].Rows[0][0].ToString() + " " + _Ds_Nombres.Tables[0].Rows[0][1].ToString();
                String _Rut = _Ds_Nombres.Tables[0].Rows[0][2].ToString();
                String _Pass = CalculateMD5Hash(_Rut);

                // ** obtiene nuevo id de usuario recien creado ***********
                String _Query = "SELECT ID_USUARIO FROM USUARIO WHERE USUARIO ='" + _Rut + "'";
                //String _Query = "SELECT ID_USUARIO FROM USUARIO WHERE PASSWORD='" + _Pass + "' AND USUARIO ='" + _Rut + "'";
                SqlConnection _Conexion_Local = new SqlConnection(ConfigurationManager.ConnectionStrings["SUPI"].ConnectionString);

                DataSet _Ds_Store = new DataSet();
                SqlDataAdapter _Ds_Datos = new SqlDataAdapter(_Query, _Conexion_Local);
                _Ds_Datos.Fill(_Ds_Store, "D");
                String Id_Supi = _Ds_Store.Tables["D"].Rows[0][0].ToString();

                // ** elimina ASOCIACION ID MACDEM CON ID SUPI ************************
                _Query = "DELETE FROM [dbo].[EMPLEADO_USUARIO_SUPI] WHERE EMP_ID = " + _Usuario_ID;
                _Conexion_Local = new SqlConnection(ConfigurationManager.ConnectionStrings["SUPI_MASTER"].ConnectionString);
                _Ds_Store = new DataSet();
                _Ds_Datos = new SqlDataAdapter(_Query, _Conexion_Local);
                _Ds_Datos.Fill(_Ds_Store);

                // ELIMINA USUARIO DE SUPI ****************************
                _Query = "DELETE FROM [dbo].[USUARIO] WHERE ID_USUARIO = " + Id_Supi;
                _Conexion_Local = new SqlConnection(ConfigurationManager.ConnectionStrings["SUPI"].ConnectionString);
                _Ds_Store = new DataSet();
                _Ds_Datos = new SqlDataAdapter(_Query, _Conexion_Local);
                _Ds_Datos.Fill(_Ds_Store);

                return true;
            }
            else
            {
                return false;
            }
        }
        catch (Exception) { return false; }
    }




    public String CalculateMD5Hash(String input)
    {
        MD5 _Md5 = System.Security.Cryptography.MD5.Create();
        Byte[] _InputBytes = System.Text.Encoding.ASCII.GetBytes(input);
        Byte[] _Hash = _Md5.ComputeHash(_InputBytes);
        StringBuilder _Sb = new StringBuilder();

        for (int i = 0; i < _Hash.Length; i++)
        {
            _Sb.Append(_Hash[i].ToString("x2"));
        }

        return _Sb.ToString();
    }


    // *****************************************************
    // ************** OBTIENE LOS ID'S *********************
    // *****************************************************




    public String _Get_Roles_Pagina(String _Rut, String _Pagina, String _Permiso)
    {
        SqlConnection _Cn = new SqlConnection();
        _Cn.ConnectionString = ConfigurationManager.ConnectionStrings["SUPI_MASTER"].ConnectionString;

        try
        {
            _Cn.Open();
            SqlCommand _Cmd = new SqlCommand("SELECT CAST(PRS.[VALOR] AS VARCHAR) FROM [PAGINA_ROL_USER] PRS, PAGINA PA, EMPLEADO EM,ROLES RO WHERE PRS.PAG_ID= PA.PAG_ID AND PRS.USER_ID = EM.EMP_ID AND PRS.PERM_ID=RO.PERM_ID AND EM.EMP_RUT = @_Rut AND PA.PAG_URL= @_Pagina AND RO.PERM_DESCRIPCION=@_Permiso ", _Cn);
            _Cmd.Parameters.AddWithValue("@_Rut", _Rut);
            _Cmd.Parameters.AddWithValue("@_Pagina", _Pagina);
            _Cmd.Parameters.AddWithValue("@_Permiso", _Permiso);
            SqlDataReader _Rd = _Cmd.ExecuteReader();
            _Rd.Read();

            String _Per = _Rd[0].ToString();

            _Rd.Close();
            _Cn.Close();

            return _Per;

        }
        catch (Exception)
        {
            _Cn.Close();
            return "";
        }
    }

    public DataSet _Get_All_Logs(String _Empleado, String _Fecha)
    {
        try
        {
            String _Query = "SELECT e.EMP_NOMBRE + ' ' + e.EMP_APPATERNO AS NOMBRE, l.FECHA_EVENTO AS FECHA, l.EVENTO, P.PAG_DESCRIPCION AS PAGINA,";
            _Query = _Query + " case WHEN P.PAG_URL='EMPLEADOS.ASPX' THEN (SELECT TOP 1 EE.EMP_NOMBRE + ' ' + Ee.EMP_APPATERNO FROM EMPLEADO EE WITH(NOLOCK) WHERE EE.EMP_ID=L.VALOR1)"; 
            _Query = _Query + " WHEN P.PAG_URL='SALAS.ASPX' THEN (SELECT TOP 1 SS.SALA_FOLIO FROM SALA SS WITH(NOLOCK) WHERE SS.SALA_ID = L.VALOR1)";
            _Query = _Query + " WHEN P.PAG_URL='LOGISTICA.ASPX' THEN (SELECT TOP 1 CC.COM_DESCRIPCION FROM COMUNA CC WITH(NOLOCK) WHERE CC.COM_ID=L.VALOR1)"; 
            _Query = _Query + " WHEN P.PAG_URL='ESTUDIOS.ASPX' THEN (SELECT TOP 1 ESS.EST_DESCRIPCION FROM ESTUDIO ESS WITH(NOLOCK) WHERE ESS.EST_ID= L.VALOR1)"; 
            _Query = _Query + " WHEN P.PAG_URL='PermisosUsuario.aspx' THEN (SELECT TOP 1 EE.EMP_NOMBRE + ' ' + Ee.EMP_APPATERNO FROM EMPLEADO EE WITH(NOLOCK) WHERE EE.EMP_ID=L.VALOR1)"; 
            _Query = _Query + " WHEN P.PAG_URL='GALERIA.ASPX' THEN (SELECT TOP 1 EE.EST_DESCRIPCION FROM ESTUDIO EE WITH(NOLOCK) WHERE EE.EST_SUPI_ID=L.VALOR1)";
            _Query = _Query + " WHEN P.PAG_URL='LOGIN.ASPX' THEN '' END AS VALOR_BUSCADO";
            _Query = _Query + " FROM LOGS L WITH(NOLOCK), EMPLEADO E WITH(NOLOCK), PAGINA P WITH(NOLOCK)"; 
            _Query = _Query + " WHERE E.EMP_ID = L.ID_USUARIO AND P.PAG_ID = L.ID_PAGINA";
            _Query = _Query + " AND E.EMP_ID='" + _Empleado + "'";
            _Query = _Query + " AND CAST(L.FECHA_EVENTO AS DATE)>='" + _Fecha + "'";

            SqlConnection _Conexion_Local = new SqlConnection(ConfigurationManager.ConnectionStrings["SUPI_MASTER"].ConnectionString);
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

    public DataSet _Get_Datos_empleado_panel_supi(String _Usuario_id)
    {
        try
        {
            String _Query = "SELECT EMP_NOMBRE, EMP_APPATERNO, EMP_RUT from EMPLEADO with(nolock) where EMP_ID=" + _Usuario_id;
            SqlConnection _Conexion_Local = new SqlConnection(ConfigurationManager.ConnectionStrings["SUPI_MASTER"].ConnectionString);
            DataSet _Ds_Store = new DataSet();
            SqlDataAdapter _Ds_Datos = new SqlDataAdapter(_Query, _Conexion_Local);

            _Ds_Datos.Fill(_Ds_Store, "DATOS");
            if(_Ds_Store.Tables[0].Rows.Count>0)
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




    // **** NUEVO FORMATO *************
    public DataSet _Get_Todos_Roles(String _Id_Usuario) 
    {
         try
        {
            String _Query = " SELECT P.PAG_ID, P.PAG_DESCRIPCION as PAGINA" ;
            _Query = _Query + " ,(SELECT PR.VALOR FROM PAGINA_ROL_USER PR WITH(NOLOCK) WHERE PR.PAG_ID = P.PAG_ID AND PR.USER_ID=PRS.USER_ID AND PR.PERM_ID = 1) AS ACCESO";
            _Query = _Query + " ,(SELECT PR.VALOR FROM PAGINA_ROL_USER PR WITH(NOLOCK) WHERE PR.PAG_ID = P.PAG_ID AND PR.USER_ID=PRS.USER_ID AND PR.PERM_ID = 3) AS LECTURA";
            _Query = _Query + " ,(SELECT PR.VALOR FROM PAGINA_ROL_USER PR WITH(NOLOCK) WHERE PR.PAG_ID = P.PAG_ID AND PR.USER_ID=PRS.USER_ID AND PR.PERM_ID = 4) AS ESCRITURA";
            _Query = _Query + " ,(SELECT PR.VALOR FROM PAGINA_ROL_USER PR WITH(NOLOCK) WHERE PR.PAG_ID = P.PAG_ID AND PR.USER_ID=PRS.USER_ID AND PR.PERM_ID = 5) AS EXPORTAR";
            _Query = _Query + " ,(SELECT PR.VALOR FROM PAGINA_ROL_USER PR WITH(NOLOCK) WHERE PR.PAG_ID = P.PAG_ID AND PR.USER_ID=PRS.USER_ID AND PR.PERM_ID = 6) AS ELIMINAR ";
            _Query = _Query + " FROM PAGINA P WITH(NOLOCK) LEFT JOIN PAGINA_ROL_USER PRS WITH(NOLOCK) ON PRS.PAG_ID = P.PAG_ID AND PRS.USER_ID=" + _Id_Usuario;
            _Query = _Query + "  WHERE P.PAG_ID NOT IN (5,6,7,8,10,15,16,17) GROUP BY P.PAG_ID, P.PAG_DESCRIPCION, PRS.PAG_ID, PRS.USER_ID";

            SqlConnection _Conexion_Local = new SqlConnection(ConfigurationManager.ConnectionStrings["SUPI_MASTER"].ConnectionString);
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

    // ***** OBTIENE USUARIO DEL SISTEMA *********
    public DataSet _Get_Todos_Usuarios()
    {
        try
        {
            SqlConnection _Conexion_Local = new SqlConnection(ConfigurationManager.ConnectionStrings["SUPI_MASTER"].ConnectionString);
            DataSet _Ds_Store = new DataSet();
            SqlDataAdapter _Ds_Datos = new SqlDataAdapter("SELECT '' AS VALOR, 0 UNION ALL SELECT ([EMP_NOMBRE] + ' ' + [EMP_APPATERNO] + ' ' + [EMP_RUT]) AS VALOR, EMP_ID FROM [EMPLEADO] order by VALOR", _Conexion_Local);
            _Ds_Datos.Fill(_Ds_Store, "DATOS");
            return _Ds_Store;
        }
        catch (Exception)
        {
            return null;
        }
    }



    //********************************************************
    // ******************* ROLES *****************************
    //********************************************************

    // ***** ASIGNACION DE ROLES POR USUARIO *************
    public Boolean _Get_Asigna_Roles(String _Id_Pag, String Id_User, String _Acceso, String _Lectura, String _Escritura, String _Exportar, String _Eliminar)
    {
        Boolean _Pasa = false;
        try {
            _Pasa = _Get_Nuevos_Roles(_Id_Pag, Id_User, "1", _Acceso); // acceso pagina    
            if (!_Pasa) return false;
        }catch (Exception e) { return false;}

        try {
            _Pasa = _Get_Nuevos_Roles(_Id_Pag, Id_User, "3", _Lectura); //lectura
            if (!_Pasa) return false;
        }catch (Exception e) {return false;}

        try{
            _Pasa = _Get_Nuevos_Roles(_Id_Pag, Id_User, "4", _Escritura); //escritura   
            if (!_Pasa) return false;
        }catch (Exception e) { return false; }

        try{
            _Pasa = _Get_Nuevos_Roles(_Id_Pag, Id_User, "5", _Exportar); //exportar   
            if (!_Pasa) return false;
        } catch (Exception e) { return false; }

        try{
            _Pasa = _Get_Nuevos_Roles(_Id_Pag, Id_User, "6", _Eliminar); //eliminar
            if (!_Pasa) return false;
        }catch (Exception e) { return false; }

        return true;
    }
    
    // ***** INSERTA O ACTUALIZA NUEVOS ROLES **********
    public Boolean _Get_Nuevos_Roles(String _Pag, String _User, String _Permiso, String _Valor) 
    {
        try
        {
            String _Query = "";
            if (_Valor == "True")
            {
                _Valor = "1";
            } else {
                _Valor = "0";
            }

            SqlConnection _Conexion_Local = new SqlConnection(ConfigurationManager.ConnectionStrings["SUPI_MASTER"].ConnectionString);
            DataSet _Ds_Store = new DataSet();
            SqlDataAdapter _Ds_Datos = new SqlDataAdapter("SELECT * FROM PAGINA_ROL_USER WHERE PAG_ID = "+ _Pag + " AND USER_ID = " + _User + " AND PERM_ID = " + _Permiso, _Conexion_Local);
            _Ds_Datos.Fill(_Ds_Store);

            if (_Ds_Store.Tables[0].Rows.Count > 0)
            {
                // Actualiza
                _Query = "UPDATE PAGINA_ROL_USER SET VALOR = " + _Valor + " WHERE USER_ID = " + _User + " AND PAG_ID = " + _Pag + " AND PERM_ID = " + _Permiso;
            }else { 
                // Inserta
                _Query = "INSERT INTO PAGINA_ROL_USER VALUES(" + _Pag + "," + _Permiso + "," + _User + "," + _Valor + ")";
            }

            _Ds_Store = new DataSet();
            _Ds_Datos = new SqlDataAdapter(_Query, _Conexion_Local);
            _Ds_Datos.Fill(_Ds_Store);
            return true;
        }
        catch (SqlException E)
        {
            return false;
        }
    }

    // *** RESETEA PASS USUARIO *********
    public Boolean _Set_Resetar_Pass(String _User, String _Pass_encrypted, Boolean _Habilitado_panel_supi) 
    {
        try
        {
            String _Pass_nornal = _Pass_encrypted;
            _Pass_encrypted = CalculateMD5Hash(_Pass_encrypted);

            SqlConnection _Conexion_MCADEM = new SqlConnection(ConfigurationManager.ConnectionStrings["SUPI_MASTER"].ConnectionString);
            DataSet _Ds_Store = new DataSet();
            SqlDataAdapter _Ds_Datos = new SqlDataAdapter("UPDATE EMPLEADO SET EMP_PASS = '" + _Pass_encrypted + "' WHERE EMP_ID=" + _User, _Conexion_MCADEM);
            _Ds_Datos.Fill(_Ds_Store);

            if (_Habilitado_panel_supi)
            {
                _Ds_Store = new DataSet();
                _Ds_Datos = new SqlDataAdapter("select EMP_ID_SUPI from EMPLEADO_USUARIO_SUPI WHERE EMP_ID=" + _User, _Conexion_MCADEM);
                _Ds_Datos.Fill(_Ds_Store, "id");
                String _Id_Supi = _Ds_Store.Tables[0].Rows[0][0].ToString();

                if (_Id_Supi != "")
                {
                    _Conexion_MCADEM = new SqlConnection(ConfigurationManager.ConnectionStrings["SUPI"].ConnectionString);
                    _Ds_Store = new DataSet();
                    _Ds_Datos = new SqlDataAdapter("UPDATE USUARIO SET PASSWORD = '" + _Pass_encrypted + "' WHERE ID_USUARIO=" + _Id_Supi, _Conexion_MCADEM);
                    _Ds_Datos.Fill(_Ds_Store);
                }
            }

            // ***envia correo informando de nueva contraseña ***
            _Conexion_MCADEM = new SqlConnection(ConfigurationManager.ConnectionStrings["SUPI_MASTER"].ConnectionString);
            _Ds_Store = new DataSet();
            _Ds_Datos = new SqlDataAdapter("exec sp_resetea_pass_usuario '" + _User + "','" + _Pass_nornal + "'", _Conexion_MCADEM);
            _Ds_Datos.Fill(_Ds_Store);

            return true;
        }
        catch (Exception) {
            return false;
        }
    }

    // ***** OTORGA / DENEGA ACCESO AL SISTEMA *****
    public Boolean _Set_Acceso_Web(String _User, String _Acceso)
    {
        try
        {
            SqlConnection _Conexion_Local = new SqlConnection(ConfigurationManager.ConnectionStrings["SUPI_MASTER"].ConnectionString);
            DataSet _Ds_Store = new DataSet();
            SqlDataAdapter _Ds_Datos = new SqlDataAdapter("SELECT * FROM PAGINA_ROL_USER WHERE PAG_ID = 5 AND [PERM_ID] = 7 and USER_ID =" + _User, _Conexion_Local);
            _Ds_Datos.Fill(_Ds_Store);

            String _Query = "";
            if (_Ds_Store.Tables[0].Rows.Count > 0)
            {
                // Actualiza
                if (_Acceso == "True")
                {
                    _Acceso = "1";
                }
                else 
                {
                    _Acceso = "0";
                }
                _Query = "UPDATE PAGINA_ROL_USER SET VALOR = " + _Acceso + " WHERE USER_ID = " + _User + " AND PAG_ID = 5 and [PERM_ID] = 7 ";
            }
            else
            {
                // Inserta
                if (_Acceso == "True")
                {
                    _Acceso = "1";
                }
                else
                {
                    _Acceso = "0";
                }
                _Query = "INSERT INTO PAGINA_ROL_USER ([PAG_ID],[PERM_ID],[USER_ID],[VALOR]) VALUES(5,7," + _User + "," + _Acceso + ")";
            }

            _Ds_Store = new DataSet();
            _Ds_Datos = new SqlDataAdapter(_Query, _Conexion_Local);
            _Ds_Datos.Fill(_Ds_Store);
            return true;

            //return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }

    // *** OTORGA PERMISOS ADMINISTRAIVOS ***********
    public Boolean _Set_Acceso_Admin(String _User) 
    {
        try
        {
            String _Rut = _Get_Rut_Usuario(_User);
            SqlConnection _Conexion_Local = new SqlConnection(ConfigurationManager.ConnectionStrings["SUPI_MASTER"].ConnectionString);
            DataSet _Ds_Store = new DataSet();
            SqlDataAdapter _Ds_Datos = new SqlDataAdapter("INSERT INTO ADMIN VALUES ('" + _Rut + "')", _Conexion_Local);
            _Ds_Datos.Fill(_Ds_Store);
            return true;
        }
        catch (Exception e)
        {
            return false;
        }

    }


    // ***********************************************
    // ********  OBTENCION DE ID'S *******************
    // ***********************************************

    // *** OBTIENE ID USUARIO ***************
    public String _Get_Id_User(String _Rut)
    {
        try
        {
            SqlConnection _Conexion_Local = new SqlConnection(ConfigurationManager.ConnectionStrings["SUPI_MASTER"].ConnectionString);
            DataSet _Ds_Store = new DataSet();
            SqlDataAdapter _Ds_Datos = new SqlDataAdapter("SELECT CAST([EMP_ID] AS VARCHAR)FROM [EMPLEADO] where EMP_RUT ='" + _Rut + "'", _Conexion_Local);
            _Ds_Datos.Fill(_Ds_Store, "DATOS");
            String _Id = _Ds_Store.Tables[0].Rows[0][0].ToString();
            return _Id;
        }
        catch (Exception)
        {
            return "";
        }

        //try
        //{

        //SqlConnection _Cn = new SqlConnection();
        //_Cn.ConnectionString = ConfigurationManager.ConnectionStrings["SUPI_MASTER"].ConnectionString;

        // _Cn.Open();
        //    SqlCommand _Cmd = new SqlCommand("SELECT CAST([EMP_ID] AS VARCHAR)FROM [EMPLEADO] where EMP_RUT = @_Rut", _Cn);
        //    _Cmd.Parameters.AddWithValue("@_Rut", _Rut);
        //    SqlDataReader _Rd = _Cmd.ExecuteReader();
        //    _Rd.Read();
        //    String _Per = _Rd[0].ToString();

        //    _Rd.Close();
        //    _Cn.Close();

        //    return _Per;

        //}
        //catch (SqlException E)
        //{
        //    _Cn.Close();
        //    return "";
        //}
        //catch (Exception E)
        //{
        //    _Cn.Close();
        //    return "";
        //}
    }

    // **** OBTIENE ID PAGINA *******
    public String _Get_Id_Pagina(String _Pagina)
    {
        try
        {
            SqlConnection _Conexion_Local = new SqlConnection(ConfigurationManager.ConnectionStrings["SUPI_MASTER"].ConnectionString);
            DataSet _Ds_Store = new DataSet();
            SqlDataAdapter _Ds_Datos = new SqlDataAdapter("SELECT CAST([PAG_ID] AS VARCHAR) FROM [PAGINA] WITH(NOLOCK) where PAG_URL ='" + _Pagina + "'", _Conexion_Local);
            _Ds_Datos.Fill(_Ds_Store, "DATOS");
            String _Id = _Ds_Store.Tables[0].Rows[0][0].ToString();
            return _Id;
        }
        catch (Exception)
        {
            return "";
        }

        //SqlConnection _Cn = new SqlConnection();
        //_Cn.ConnectionString = ConfigurationManager.ConnectionStrings["SUPI_MASTER"].ConnectionString;

        //try
        //{
        //    _Cn.Open();
        //    SqlCommand _Cmd = new SqlCommand("SELECT CAST([PAG_ID] AS VARCHAR)FROM [PAGINA] where PAG_URL =@_Nombre", _Cn);
        //    _Cmd.Parameters.AddWithValue("@_Nombre", _Pagina);
        //    SqlDataReader _Rd = _Cmd.ExecuteReader();
        //    _Rd.Read();
        //    String _Per = _Rd[0].ToString();

        //    _Rd.Close();
        //    _Cn.Close();

        //    return _Per;

        //}
        //catch (SqlException E)
        //{
        //    _Cn.Close();
        //    return "";
        //}
        //catch (Exception E)
        //{
        //    _Cn.Close();
        //    return "";
        //}

    }

    // **** OBTIENE ID PERMISO ********
    public String _Get_Id_Permiso(String _Permiso)
    {
        try
        {
            SqlConnection _Conexion_Local = new SqlConnection(ConfigurationManager.ConnectionStrings["SUPI_MASTER"].ConnectionString);
            DataSet _Ds_Store = new DataSet();
            SqlDataAdapter _Ds_Datos = new SqlDataAdapter("SELECT CAST([PERM_ID] AS VARCHAR)FROM [ROLES] where PERM_DESCRIPCION ='" + _Permiso + "'", _Conexion_Local);
            _Ds_Datos.Fill(_Ds_Store, "DATOS");
            String _Id = _Ds_Store.Tables[0].Rows[0][0].ToString();
            return _Id;
        }
        catch (Exception)
        {
            return "";
        }
    }

    // **** OBTIENE RUT USUARIO *********
    public String _Get_Rut_Usuario(String _Usuario)
    {
        try
        {
            SqlConnection _Conexion_Local = new SqlConnection(ConfigurationManager.ConnectionStrings["SUPI_MASTER"].ConnectionString);
            DataSet _Ds_Store = new DataSet();
            SqlDataAdapter _Ds_Datos = new SqlDataAdapter("SELECT EMP_RUT FROM EMPLEADO where EMP_ID ='" + _Usuario + "'", _Conexion_Local);
            _Ds_Datos.Fill(_Ds_Store, "DATOS");
            //if(_Ds_Store.Tables[0].Rows.Count>0)
            //{
            String _Rut = _Ds_Store.Tables[0].Rows[0][0].ToString();
            return _Rut;
            //}else{
            //    return "";
            //}
        }
        catch (Exception e)
        {
            return "";
        }
    }

}