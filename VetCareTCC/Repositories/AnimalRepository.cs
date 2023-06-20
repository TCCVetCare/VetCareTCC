using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using VetCareTCC.Models;
using MySql.Data.MySqlClient;

namespace VetCareTCC.Repositories
{
    public class AnimalRepository
    {
        DatabaseConnection con = new DatabaseConnection();

        public void insertAnimal(AnimalModel animal)
        {
            // @idCustomer,
            MySqlCommand cmd = new MySqlCommand(
                "insert into tbPet values(default, @nameAnimal, @idCustomer, @idBreedAnimal, @ageAnimal, @idGenderAnimal, @idSpeciesAnimal, @imageAnimal, @IdPlan)",
                con.ConectarBD()
            );
            cmd.Parameters.Add("@idCustomer", MySqlDbType.VarChar).Value = animal.idCustomer;
            cmd.Parameters.Add("@nameAnimal", MySqlDbType.VarChar).Value = animal.nameAnimal;
            cmd.Parameters.Add("@idBreedAnimal", MySqlDbType.VarChar).Value = animal.idBreedAnimal;
            cmd.Parameters.Add("@idSpeciesAnimal", MySqlDbType.VarChar).Value =
                animal.idSpeciesAnimal;
            cmd.Parameters.Add("@idGenderAnimal", MySqlDbType.VarChar).Value = animal.idGenderAnimal;
            cmd.Parameters.Add("@ageAnimal", MySqlDbType.VarChar).Value = animal.ageAnimal;
            cmd.Parameters.Add("@imageAnimal", MySqlDbType.VarChar).Value = animal.imageAnimal;
            cmd.Parameters.Add("@idPlan", MySqlDbType.VarChar).Value = animal.idPlan;

            cmd.ExecuteNonQuery();
            con.DesconectarBD();
        }

        public List<AnimalModel> getAnimal()
        {
            List<AnimalModel> listaClientes = new List<AnimalModel>();
            MySqlCommand cmd = new MySqlCommand("select * from tbPet", con.ConectarBD());

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
                    new AnimalModel
                    {
                        idAnimal = Convert.ToString(dr["idAnimal"]),
                        idCustomer = Convert.ToString(dr["idCustomer"]),
                        nameAnimal = Convert.ToString(dr["nameAnimal"]),
                        idBreedAnimal = Convert.ToString(dr["idBreedAnimal"]),
                        idSpeciesAnimal = Convert.ToString(dr["idSpeciesAnimal"]),
                        idGenderAnimal = Convert.ToString(dr["idGenderAnimal"]),
                        ageAnimal = Convert.ToString(dr["ageAnimal"]),
                        imageAnimal = Convert.ToString(dr["imageAnimal"])
                    }
                );
            }
            return listaClientes;
        }

        public List<AnimalModel> getAnimalByIdCustomer(string id)
        {
            List<AnimalModel> listaAnimais = new List<AnimalModel>();
            MySqlCommand cmd = new MySqlCommand(
                "SELECT * FROM tbPet WHERE idCustomer = @id",
                con.ConectarBD()
            );
            cmd.Parameters.AddWithValue("@id", id);

            // adapter para lista
            MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);

            // tabela virtual
            DataTable db = new DataTable();
            adapter.Fill(db);

            con.DesconectarBD();

            // enquanto existir linhas (registros) no banco
            // o foreach irá adicionar os valores vindos do banco nos atributos da classe Animal

            foreach (DataRow dr in db.Rows)
            {
                listaAnimais.Add(
                    new AnimalModel
                    {
                        idAnimal = Convert.ToString(dr["idAnimal"]),
                        idCustomer = Convert.ToString(dr["idCustomer"]),
                        nameAnimal = Convert.ToString(dr["nameAnimal"]),
                        idBreedAnimal = Convert.ToString(dr["idBreedAnimal"]),
                        idSpeciesAnimal = Convert.ToString(dr["idSpeciesAnimal"]),
                        idGenderAnimal = Convert.ToString(dr["idGenderAnimal"]),
                        ageAnimal = Convert.ToString(dr["ageAnimal"]),
                        imageAnimal = Convert.ToString(dr["imageAnimal"])
                    }
                );
            }

            return listaAnimais;
        }





        //public List<Animal> GetAnimaisByCustomerId(int customerId)
        //{
        //    List<Animal> animais = new List<Animal>();


        //    using (MySqlConnection connection = new MySqlConnection(connectionString))
        //    {
        //        connection.Open();

        //        string query = "SELECT idAnimal, nameAnimal, breedAnimal, ageAnimal, genderAnimal, speciesAnimal FROM tbPet WHERE idCustomer = @customerId";
        //        MySqlCommand command = new MySqlCommand(query, connection);
        //        command.Parameters.AddWithValue("@customerId", customerId);

        //        using (MySqlDataReader reader = command.ExecuteReader())
        //        {
        //            while (reader.Read())
        //            {
        //                Animal animal = new Animal
        //                {
        //                    idAnimal = reader.GetString("idAnimal"),
        //                    nameAnimal = reader.GetString("nameAnimal"),
        //                    breedAnimal = reader.GetString("breedAnimal"),
        //                    speciesAnimal = reader.GetString("speciesAnimal"),
        //                    genderAnimal = reader.GetString("genderAnimal"),
        //                    ageAnimal = reader.GetString("ageAnimal")

        //                };

        //                animais.Add(animal);
        //            }
        //        }
        //    }

        // return animais;
        //}


        //        SELECT tbCustomer.idCustomer, tbCustomer.nameCustomer
        //FROM tbCustomer
        //JOIN tbPet ON tbPet.idCustomer = tbCustomer.idCustomer;
        public bool deleteAnimal(int id)
        {
            MySqlCommand cmd = new MySqlCommand(
                "delete from tbPet where idAnimal=@id",
                con.ConectarBD()
            );
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

        public bool atualizarAnimal(AnimalModel animal)
        {
            MySqlCommand cmd = new MySqlCommand(
                "update tbPet set nameAnimal=@nameAnimal, idCustomer=@idCustomer, idBreedAnimal=@breedAnimal,  ageAnimal=@ageAnimal, idGenderAnimal=@genderAnimal,"
                    + "idSpeciesAnimal=@speciesAnimal, imageAnimal=@imageAnimal where "
                    + "idAnimal=@idAnimal",
                con.ConectarBD()
            );

            cmd.Parameters.AddWithValue("@nameAnimal", animal.nameAnimal);
            cmd.Parameters.AddWithValue("@breedAnimal", animal.idBreedAnimal);
            cmd.Parameters.AddWithValue("@speciesAnimal", animal.idSpeciesAnimal);
            cmd.Parameters.AddWithValue("@genderAnimal", animal.idGenderAnimal);
            cmd.Parameters.AddWithValue("@ageAnimal", animal.ageAnimal);
            cmd.Parameters.AddWithValue("@idAnimal", animal.idAnimal);
            cmd.Parameters.AddWithValue("@imageAnimal", animal.imageAnimal);
            cmd.Parameters.AddWithValue("@idCustomer", animal.idCustomer);

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

        public string GetNameCustomerById(AnimalModel id)
        {
            string nameCustomer = string.Empty;
            using (
            MySqlConnection con = new MySqlConnection(
                    "Server=localhost;DataBase=teste2;User=root;pwd=12345678"
                )
            )
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand(
                    "SELECT nameCustomer FROM tbCustomer WHERE idCustomer = @idCustomer",
                    con
                );
                cmd.Parameters.AddWithValue("@idCustomer", id);
                object result = cmd.ExecuteScalar();

                if (result != null)
                {
                    nameCustomer = result.ToString();
                }

                con.Close(); // fechando conexão
            }

            return nameCustomer;
        }
    }
}
