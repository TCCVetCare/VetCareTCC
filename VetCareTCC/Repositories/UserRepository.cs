﻿using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VetCareTCC.Models;
using VetCareTCC.ViewlModels;
using static VetCareTCC.Models.UserModel;

namespace VetCareTCC.Repositories
{
    public class UserRepository
    {
            readonly string connectionString;

            public UserRepository(string connectionString)
            {
                this.connectionString = connectionString;
            }

            public UserModel GetUserByEmailAndPassword(LoginViewModel model)
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    //verifica se o usuÃ¡rio Ã© um administrador
                    using (var command = new MySqlCommand("SELECT * FROM tbAdmin WHERE emailAdmin = @EmailAdmin AND passwordAdmin = @PasswordAdmin", connection))
                    {
                        command.Parameters.AddWithValue("@EmailAdmin", model.Email);
                        command.Parameters.AddWithValue("@PasswordAdmin", model.Password);



                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new UserModel
                                {
                                    id = reader.GetString(reader.GetOrdinal("idAdmin")),
                                    name = reader.GetString(reader.GetOrdinal("nameAdmin")),
                                    email = reader.GetString(reader.GetOrdinal("emailAdmin")),
                                    password = reader.GetString(reader.GetOrdinal("passwordAdmin")),
                                    role = UserRole.Admin



                                };
                            }
                        }
                    }

                    //verifica se o usuÃ¡rio Ã© um customer
                    using (var command = new MySqlCommand("SELECT * FROM tbCustomer WHERE emailCustomer = @EmailCustomer AND passwordCustomer = @PasswordCustomer", connection))
                    {
                        command.Parameters.AddWithValue("@EmailCustomer", model.Email);
                        command.Parameters.AddWithValue("@PasswordCustomer", model.Password);

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new UserModel
                                {
                                    id = reader.GetString(reader.GetOrdinal("idCustomer")),
                                    name = reader.GetString(reader.GetOrdinal("nameCustomer")),
                                    email = reader.GetString(reader.GetOrdinal("emailCustomer")),
                                    password = reader.GetString(reader.GetOrdinal("passwordCustomer")),
                                    role = UserRole.Customer
                                };
                            }
                        }
                    }
                }
                return null;
            }
        }
    }
