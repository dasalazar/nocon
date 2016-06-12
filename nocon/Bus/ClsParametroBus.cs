using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using nocon.Dao;
using nocon.Data;

namespace nocon.Bus
{
    public class ClsParametroBus
    {
        public ClsParametro getValorParam(string chave)
        {
            string c = chave;
            ClsParametroDao paramD = new ClsParametroDao();
            DataSet dt = paramD.getValorParam(c);
            ClsParametro parametro = new ClsParametro();

            if (dt != null)
            {
                if (dt.Tables.Count > 0)
                {
                    if (dt.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow row in dt.Tables[0].Rows)
                        {
                            parametro.IdParam = int.Parse(row["ID_PARAM"].ToString());
                            parametro.ChaveParam = "COD_MAX_REEMBOLSO_RIOCARD";
                            parametro.Parametro = row["PARAMETRO"].ToString();
                            break;
                        }
                        return parametro;
                    }

                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public bool setValorParam(string chave, string valor)
        {
            ClsParametroDao paramD = new ClsParametroDao();
            try
            {
            string c = chave;
            string v = valor;
            return paramD.setValorParam(c, v);
            }
            catch(Exception ex)
            {
                throw ex;
                return false;
            }
            
            
            
        }
    }
}
