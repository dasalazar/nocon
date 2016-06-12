using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using nocon.Dao;
using nocon.Data;

namespace nocon.Bus
{
    public class ClsEquipamentoBus
    {
        public List<ClsEquipamento> getEquipamentos()
        {
            ClsEquipamentoDao ed = new ClsEquipamentoDao();
            DataSet dt;
            List<ClsEquipamento> lstEqp = new List<ClsEquipamento>();
            ClsEquipamento eqp;
            try
            {
                dt = ed.getEquipamentos();
                if (dt != null)
                {
                    if (dt.Tables.Count > 0)
                    {
                        if (dt.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow row in dt.Tables[0].Rows)
                            {
                                eqp = new ClsEquipamento();

                                eqp.IdEqp = long.Parse(row["ID_EQUIPAMENTO"].ToString());
                                eqp.NomeEqp = row["NOME_EQUIPAMENTO"].ToString();
                                eqp.IdLocal = int.Parse(row["ID_LOCAL"].ToString());
                                eqp.IdCliente = int.Parse(row["ID_CLIENTE"].ToString());
                                eqp.NomeCliente = row["NOME_CLIENTE"].ToString();
                                lstEqp.Add(eqp);
                            }
                        }
                    }
                }
                return lstEqp;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
