using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Collections;
using System.Security.Cryptography;
using System.Security;
using System.Text;

/// <summary>
/// Descripción breve de Generico_Controller
/// </summary>
public class Generico_Controller
{
	public Generico_Controller()
	{

	}

    //Valida si existe usuario..
    public int _Login(String _User, String _Pass)
    {
        // ENCRIPTACION PASSWORD **********
        _Pass = CalculateMD5Hash(_Pass);

        //1 = Usuario habilitado
        //2 = Usuario no existe
        //3 = Error servidor
        //4 = Usuario inhabilitado

        try
        {
            SqlConnection _Conexion_MCcadem = new SqlConnection(ConfigurationManager.ConnectionStrings["SUPI_MASTER"].ConnectionString);
            DataSet _Ds_Store = new DataSet();
            SqlDataAdapter _Ds_Datos = new SqlDataAdapter("SELECT CAST(PRS.VALOR AS VARCHAR) FROM PAGINA_ROL_USER PRS with(nolock), EMPLEADO EM with(nolock), ROLES RO with(nolock) WHERE PRS.USER_ID=EM.EMP_ID AND PRS.PERM_ID=RO.PERM_ID AND RO.PERM_ID=7 AND EM.EMP_RUT='" + _User + "' AND EM.EMP_PASS='" + _Pass + "'", _Conexion_MCcadem);
            _Ds_Datos.Fill(_Ds_Store, "DATOS");

            if (_Ds_Store.Tables[0].Rows.Count > 0)
            {
                String _Estado = _Ds_Store.Tables[0].Rows[0][0].ToString();
                if (_Estado == "1")
                {
                    return 1;
                }
                else
                {
                    return 4;
                }
            }
            else
            {
                return 2;
            }
        }
        catch (SqlException E)
        {
            //53 = Error de red, Servidor no encontrado
            if (E.Number == 53)
            {
                return 3;
            }
            else
            {
                return 2;
            }
        }
        catch (Exception)
        {
            //_Cn.Close(); 
            return 2;
        }
    }

    //Se ingresa un registro de cada una de las acciones del usuario...
    public Boolean _Set_Insert_Logs(String _User, String _Fecha, String _Evento, String _Pagina, String _Accion1, String _Accion2)
    {
        try
        {
            SqlConnection _Conexion_Local = new SqlConnection(ConfigurationManager.ConnectionStrings["SUPI_MASTER"].ConnectionString);
            DataSet _Ds_Store = new DataSet();
            String _Query = "INSERT INTO [LOGS] ([ID_USUARIO] ,[FECHA_EVENTO] ,[EVENTO],[ID_PAGINA] ,[VALOR1],[VALOR2]) VALUES ('" + _User.ToUpper() + "', '" + _Fecha.ToString() + "', '" + _Evento + "','" + _Pagina + "', '" + _Accion1.ToUpper() + "', '" + _Accion2.ToUpper() + "')";
            SqlDataAdapter _Ds_Datos = new SqlDataAdapter(_Query, _Conexion_Local);
            _Ds_Datos.Fill(_Ds_Store);
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

    // OBTIENE NOMBRE USUARIO LOGEADO
    public String _Get_Nombre_Usuario(String _Rut)
    {
        SqlConnection _Cn = new SqlConnection();
        _Cn.ConnectionString = ConfigurationManager.ConnectionStrings["SUPI_MASTER"].ConnectionString;

        try
        {
            _Cn.Open();
            SqlCommand _Cmd = new SqlCommand("SELECT EMP_NOMBRE + ' ' + EMP_APPATERNO FROM EMPLEADO WHERE EMP_RUT =@Rut", _Cn);
            _Cmd.Parameters.AddWithValue("@Rut", _Rut);

            SqlDataReader _Rd = _Cmd.ExecuteReader();
            _Rd.Read();
            String _Usuario = _Rd.GetString(0);
            _Rd.Close();
            _Cn.Close();
            return _Usuario;
        }
        catch (SqlException E)
        {
            _Cn.Close();
            return "";
        }

    }

    // OBTIENE FICHA TECNICA
    public ArrayList _Get_Ficha_Tecnica(String _Rut)
    {
        ArrayList arrlst = new ArrayList();
        SqlConnection _Cn = new SqlConnection();
        _Cn.ConnectionString = ConfigurationManager.ConnectionStrings["SUPI_MASTER"].ConnectionString;

        try
        {
            _Cn.Open();
            SqlCommand _Cmd = new SqlCommand("EXEC _GET_DATOSEMPLEADO '" + _Rut + "'", _Cn);
            SqlDataReader _Rd = _Cmd.ExecuteReader();
            _Rd.Read();

            for (int i = 0; i < _Rd.FieldCount; i++)
            {
                arrlst.Add(_Rd.GetString(i));
            }
            _Rd.Close();
            _Cn.Close();
            return arrlst;
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

    //**** OBTIENE ID DEL USUSARIO **********
    public String _Get_Id_User(String _Rut)
    {
        SqlConnection _Cn = new SqlConnection();
        _Cn.ConnectionString = ConfigurationManager.ConnectionStrings["SUPI_MASTER"].ConnectionString;

        try
        {
            _Cn.Open();
            SqlCommand _Cmd = new SqlCommand("SELECT CAST([EMP_ID] AS VARCHAR)FROM [EMPLEADO] where EMP_RUT = @_Rut", _Cn);
            _Cmd.Parameters.AddWithValue("@_Rut", _Rut);
            SqlDataReader _Rd = _Cmd.ExecuteReader();
            _Rd.Read();
            String _Per = _Rd[0].ToString();

            _Rd.Close();
            _Cn.Close();

            return _Per;

        }
        catch (SqlException E)
        {
            _Cn.Close();
            return "";
        }
        catch (Exception E)
        {
            _Cn.Close();
            return "";
        }

    }

    //********* OBTIENE EL ESTADO DEL SERVIDOR
    public Boolean _Get_Estado_Servidor() 
    {
        try
        {
            SqlConnection _Conexion_Local = new SqlConnection(ConfigurationManager.ConnectionStrings["SUPI_MASTER"].ConnectionString);
            DataSet _Ds_Store = new DataSet();
            SqlDataAdapter _Ds_Datos = new SqlDataAdapter("SELECT ESTADO_WEB FROM WEB_ESTADO", _Conexion_Local);
            _Ds_Datos.Fill(_Ds_Store,"DATOS");
            String _Estado = _Ds_Store.Tables[0].Rows[0][0].ToString();
            if (_Estado == "True")
            {
                return true;
            }
            else {
                return false;
            }
        }
        catch (Exception E)
        {
            return false;
        }
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


}