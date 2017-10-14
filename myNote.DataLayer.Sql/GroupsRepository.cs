using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using myNote.Model;
using System.Data.SqlClient;

namespace myNote.DataLayer.Sql
{
    public class GroupsRepository : IGroupsRepository
    {
        private readonly string connectionString;
            
        public GroupsRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public Group CreateGroup(Guid userId, string name)
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                using (var command = sqlConnection.CreateCommand())
                {
                    command.CommandText = "insert into Groups (Id, UserId, Name) values (@Id, @UserId, @Name)";

                    var group = new Group
                    {
                        Id = Guid.NewGuid(),
                        Name = name
                    };

                    command.Parameters.AddWithValue("@Id", group.Id);
                    command.Parameters.AddWithValue("@UserId", userId);
                    command.Parameters.AddWithValue("@Name", group.Name);

                    command.ExecuteNonQuery();

                    return group;
                }
            }
        }

        public IEnumerable<Group> GetUserGroups(Guid userId)
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                using (var command = sqlConnection.CreateCommand())
                {
                    command.CommandText = "select id, name from Groups where UserId = @userId";
                    command.Parameters.AddWithValue("@userId", userId);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            yield return new Group
                            {
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                Id = reader.GetGuid(reader.GetOrdinal("Id"))
                            };
                        }
                    }
                }
            }
        }
    }
}
