using myNote.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myNote.DataLayer.Sql
{
    public class UsersRepository : IUsersRepository
    {
        private readonly string connectionString;
        private readonly IGroupsRepository groupsRepository;

        public UsersRepository(string connectionString, IGroupsRepository groupsRepository)
        {
            this.connectionString = connectionString;
            this.groupsRepository = groupsRepository;
        }

        public User CreateUser(User user)
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                using (var command = sqlConnection.CreateCommand())
                {
                    command.CommandText = "insert into Users (Id, Name, Birthday, Email, RegisterDate) values (@Id, @Name, @Birthday, @Email, @RegisterDate)";
                    user.Id = Guid.NewGuid();
                    command.Parameters.AddWithValue("@Id", user.Id);
                    command.Parameters.AddWithValue("@Name", user.Name);
                    command.Parameters.AddWithValue("@Email", user.Email);

                    if (user.Birthday != default(DateTime))
                        command.Parameters.AddWithValue("@Birthday", user.Birthday);
                    else
                        command.Parameters.AddWithValue("@Birthday", DBNull.Value);

                    if (user.RegisterDate != default(DateTime))
                        command.Parameters.AddWithValue("@RegisterDate", user.RegisterDate);
                    else
                        command.Parameters.AddWithValue("@RegisterDate", DBNull.Value);

                    command.ExecuteNonQuery();

                    return user;
                }
            }
        }

        public void DeleteUser(Guid id)
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                using (var command = sqlConnection.CreateCommand())
                {
                    command.CommandText = "delete from users where Id = @Id";
                    command.Parameters.AddWithValue("@Id", id);
                    command.ExecuteNonQuery();
                }
            }
        }

        public User GetUser(Guid id)
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                using (var command = sqlConnection.CreateCommand())
                {
                    command.CommandText = "select * from Users where Id = @Id";
                    command.Parameters.AddWithValue("@Id", id);

                    using (var reader = command.ExecuteReader())
                    {
                        if (!reader.Read())
                            throw new ArgumentException($"Пользователь с id {id} не найден");
                        
                        var user = new User
                        {                            
                            Id = reader.GetGuid(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            Email = reader.GetString(reader.GetOrdinal("Email")),
                        };
                        if (!reader.IsDBNull(reader.GetOrdinal("Birthday")))
                            user.Birthday = reader.GetDateTime(reader.GetOrdinal("Birthday"));
                        if (!reader.IsDBNull(reader.GetOrdinal("RegisterDate")))
                            user.Birthday = reader.GetDateTime(reader.GetOrdinal("RegisterDate"));
                        
                        user.UserGroups = groupsRepository.GetUserGroups(user.Id);
                        return user;
                    }
                }
            }
        }
    }
}
