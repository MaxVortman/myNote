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

        public void CompareToken(Token expectedToken)
        {
            var db = new DataContext(connectionString);
            var actualToken = (from t in db.GetTable<Token>()
                         where t.UserId == expectedToken.UserId
                         select t).FirstOrDefault();
            if (actualToken == default(Token))
                throw new ArgumentException("Wrong UserId");
            if (actualToken.Key != expectedToken.Key)
                throw new ArgumentException("Invalid Token");
        }

        public Token CreateToken(Guid userId)
        {
            var db = new DataContext(connectionString);
            var tokenFromDb = (from t in db.GetTable<Token>()
                               where t.UserId == userId
                               select t).FirstOrDefault();
            if (tokenFromDb == default(Token))
                throw new ArgumentException($@"User with this id {userId} is note registred");
            tokenFromDb.Key = CreateKey(userId);
            db.SubmitChanges();
            return tokenFromDb;
        }

        private string CreateKey(Guid userId)
        {
            return userId + DateTime.Now.ToString(@"MM\/dd\/yyyy\_HH:mm");
        }

        public void CompareToken(Token accessToken, Guid userId)
        {
            if (accessToken.UserId != userId)
                throw new ArgumentException("You don't have access");
            CompareToken(accessToken);
        }
    }
}
