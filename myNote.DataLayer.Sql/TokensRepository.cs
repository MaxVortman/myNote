using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using myNote.Model;

namespace myNote.DataLayer.Sql
{
    public class TokensRepository : ITokensRepository
    {
        private readonly string connectionString;

        public TokensRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public Token GetToken(Guid userId)
        {
            var db = new DataContext(connectionString);
            var token = (from t in db.GetTable<Token>()
                         where t.UserId == userId
                         select t).FirstOrDefault();
            if (token == default(Token))
                throw new ArgumentException("Wrong UserId");
            return token;
        }
    }
}
