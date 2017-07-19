using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Collections;
using System.Text.RegularExpressions;

/// <summary>
/// Descripción breve de Solicitud_Controller
/// </summary>
public class Solicitud_Controller
{
	public Solicitud_Controller()
	{
		//
		// TODO: Agregar aquí la lógica del constructor
		//
	}

    public Boolean _Get_Solicita_nueva_Pass(String _Email, String _Msj)
    {
        try
        {
            _Msj = _Msj.Replace(Regex.Replace(_Msj, @"[\r\n\r\]", "<br/><br/>"), "");

            String _Query = "SELECT [EMP_ID] ,[EMP_RUT] ,[EMP_NOMBRE] + ' ' +[EMP_APPATERNO] as NOMBRE, CARGO_DESCRIPCION ,EST_DESCRIPCION ,D.NOMBRE_DEPTO";
            _Query = _Query + " FROM [MCADEM].[dbo].[EMPLEADO] E WIth(NOLOCK), CARGO C WIth(NOLOCK),DEPARTAMENTO D WIth(NOLOCK), ESTADO_EMPLEADO ES WIth(NOLOCK)";
            _Query = _Query + " where E.[EMP_CARGO_ID]= C.CARGO_ID  AND D.ID_DEPTO = C.ID_DEPTO AND ES.EST_ID = E.EMP_EST_ID";
            _Query = _Query + " AND emp_correo='" + _Email + "'";
            SqlConnection _Conexion_Local = new SqlConnection(ConfigurationManager.ConnectionStrings["SUPI_MASTER"].ConnectionString);
            DataSet _Ds_Store = new DataSet();  
            SqlDataAdapter _Ds_Datos = new SqlDataAdapter("select * from empleado where emp_correo ='" + _Email  + "'", _Conexion_Local);
            _Ds_Datos.Fill(_Ds_Store, "DATOS");

            if (_Ds_Store.Tables["DATOS"].Rows.Count > 0)
            {
                _Ds_Store = new DataSet();
                _Ds_Datos = new SqlDataAdapter("exec [sp_solicita_pass] '" + _Email + "','" + _Msj + "'", _Conexion_Local);
                _Ds_Datos.Fill(_Ds_Store);
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

}