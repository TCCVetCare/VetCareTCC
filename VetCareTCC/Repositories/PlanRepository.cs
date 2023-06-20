using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Numerics;
using System.Web;
using VetCareTCC.Models;
using VetCareTCC.ViewlModels;

namespace VetCareTCC.Repositories
{
    public class PlanRepository
    {
        DatabaseConnection con = new DatabaseConnection();

        public void insertPlan(PlanModel plan)
        {
            MySqlCommand cmd = new MySqlCommand("insert into tbPlan values(default, @namePlan, @descriptionPlan, @pricePlan, @imagePlan)", con.ConectarBD());
            cmd.Parameters.Add("@namePlan", MySqlDbType.VarChar).Value = plan.namePlan;
            cmd.Parameters.Add("@descriptionPlan", MySqlDbType.VarChar).Value = plan.descriptionPlan;
            cmd.Parameters.Add("@pricePlan", MySqlDbType.Double).Value = plan.pricePlan;
            cmd.Parameters.Add("@imagePlan", MySqlDbType.VarChar).Value = plan.imagePlan;
            cmd.ExecuteNonQuery();
            con.DesconectarBD();
        }

        public List<PlanModel> getPlan()
        {
            List<PlanModel> listaClientes = new List<PlanModel>();
            MySqlCommand cmd = new MySqlCommand("select * from tbPlan", con.ConectarBD());

            //adapter para lista
            MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);

            //tabela virtual
            DataTable db = new DataTable();
            adapter.Fill(db);

            con.DesconectarBD();

            //enquanto existir linhas(registros) no banco
            //o foreach irá adicionar os valors vindo do banco nos atributos da ModelCliente

            foreach (DataRow dr in db.Rows)
            {
                listaClientes.Add(
                    new PlanModel
                    {
                        idPlan = Convert.ToString(dr["idPlan"]),
                        namePlan = Convert.ToString(dr["namePlan"]),
                        descriptionPlan = Convert.ToString(dr["descriptionPlan"]),
                        pricePlan = Convert.ToDouble(dr["pricePlan"]),
                        imagePlan = Convert.ToString(dr["imagePlan"])

                    });
            }
            return listaClientes;

        }

        public bool deletePlan(int id)
        {
            MySqlCommand cmd = new MySqlCommand("delete from tbPlan where idPlan=@id", con.ConectarBD());
            cmd.Parameters.AddWithValue("id", id);

            int i = cmd.ExecuteNonQuery();
            con.DesconectarBD();
            if (i >= 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool updatePlan(PlanModel plan)
        {
            MySqlCommand cmd = new MySqlCommand("update tbPlan set namePlan=@namePlan, descriptionPlan=@descriptionPlan, pricePlan=@pricePlan, imagePlan=@imagePlan  " +
                " where " +
                "idPlan=@idPlan", con.ConectarBD());

            cmd.Parameters.Add("@idPlan", MySqlDbType.VarChar).Value = plan.idPlan;
            cmd.Parameters.Add("@namePlan", MySqlDbType.VarChar).Value = plan.namePlan;
            cmd.Parameters.Add("@descriptionPlan", MySqlDbType.VarChar).Value = plan.descriptionPlan;
            cmd.Parameters.Add("@pricePlan", MySqlDbType.Double).Value = plan.pricePlan;
            cmd.Parameters.Add("@imagePlan", MySqlDbType.VarChar).Value = plan.imagePlan;
            int i = cmd.ExecuteNonQuery();
            con.DesconectarBD();
            if (i >= 1)
            {
                return true;

            }
            else
            {
                return false;
            }

        }

        public void insertPlanAnimal(PlanAnimalViewModel planAnimal)
        {
            MySqlCommand cmd = new MySqlCommand("insert into PlanAnimal values(default, @idPlan, @idAnimal, @idFormOfPayment)", con.ConectarBD());
            cmd.Parameters.Add("@idPlan", MySqlDbType.VarChar).Value = planAnimal.idPlan;
            cmd.Parameters.Add("@idAnimal", MySqlDbType.VarChar).Value = planAnimal.idAnimal;
            cmd.Parameters.Add("@idFormOfPayment", MySqlDbType.VarChar).Value = planAnimal.idFormOfPayment;
            cmd.ExecuteNonQuery();
            con.DesconectarBD();
        }


        public void UpdateIdPlan(string idPlan, string idAnimal)
        {
            MySqlCommand cmd = new MySqlCommand("UPDATE tbPet SET idPlan = @idPlan WHERE idAnimal = @idAnimal", con.ConectarBD());
            cmd.Parameters.Add("@idPlan", MySqlDbType.Int32).Value = idPlan;
            cmd.Parameters.Add("@idAnimal", MySqlDbType.Int32).Value = idAnimal;
            cmd.ExecuteNonQuery();
            con.DesconectarBD();
        }

    }
}
