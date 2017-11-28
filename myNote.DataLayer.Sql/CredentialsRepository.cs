using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using myNote.Model;

namespace myNote.DataLayer.Sql
{
    public class CredentialsRepository : ICredentialsRepository
    {

        private readonly string connectionString;
        private readonly ITokensRepository tokensRepository;
        private readonly IUsersRepository usersRepository;

        public CredentialsRepository(string connectionString, IUsersRepository usersRepository, ITokensRepository tokensRepository)
        {
            this.connectionString = connectionString;
            this.tokensRepository = tokensRepository;
            this.usersRepository = usersRepository;
        }

        public void Delete(string login, Token accessToken)
        {
            new TokensRepository(connectionString).CompareToken(accessToken, new UsersRepository(connectionString, new GroupsRepository(connectionString)).GetUser(login).Id);

            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                using (var command = sqlConnection.CreateCommand())
                {
                    command.CommandText = "delete from Credentials where Login = @Login";
                    command.Parameters.AddWithValue("@Login", login);
                    command.ExecuteNonQuery();
                }
            }
        }

        public Token Login(Credential credential)
        {
            if (!CheckForAvailability(credential))
                throw new ArgumentException("User with this Login is not registered or password is invalid");
            return tokensRepository.CreateToken(usersRepository.GetUser(credential.Login).Id);
        }

        public void Register(Credential credential)
        {
            if (CheckForAvailability(credential))
                throw new ArgumentException("User with this Login already registered");
            var db = new DataContext(connectionString);
            db.GetTable<Credential>().InsertOnSubmit(credential);
            db.SubmitChanges();
            usersRepository.CreateUser(new User { Login = credential.Login });
        }

        private bool CheckForAvailability(Credential credential)
        {
            var db = new DataContext(connectionString);
            var credentialFromDb = (from c in db.GetTable<Credential>()
                                    where c.Login == credential.Login
                                    select c).FirstOrDefault();
            if (credentialFromDb == default(Credential))
                return false;
            if (!credentialFromDb.Password.SequenceEqual(credential.Password))
                return false;
            return true;
        }
    }
}
