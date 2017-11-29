using myNote.Api.Filters;
using myNote.DataLayer;
using myNote.DataLayer.Sql;
using myNote.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace myNote.Api.Controllers
{
    public class CredentialController : ApiController
    {
        private ICredentialsRepository credentialsRepository;
        private const string ConnectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=test;Integrated Security=true";

        public CredentialController()
        {
            credentialsRepository = new CredentialsRepository(ConnectionString, new UsersRepository(ConnectionString, new GroupsRepository(ConnectionString)), new TokensRepository(ConnectionString));
        }

        /// <summary>
        /// Регистрация нового пользователя
        /// </summary>
        /// <param name="credential">Логин и пароль</param>
        /// <returns>Созданый пользователь</returns>
        [HttpPost]
        [Route("api/register")]
        [ArgumentExceptionFilter]
        public void Post([FromBody] Credential credential)
        {
            Logger.Log.Instance.Info("Создание пользователя с логином: {0}", credential.Login);
            try
            {
                credentialsRepository.Register(credential);
            }
            catch (ArgumentException e)
            {
                Logger.Log.Instance.Error(e.Message);
                throw new ArgumentException(e.Message);
            }
        }

        /// <summary>
        /// Авторизация пользователя
        /// </summary>
        /// <param name="credential">Логин и пароль</param>
        /// <returns>Пользователь</returns>
        [HttpPut]
        [Route("api/login")]
        [ArgumentExceptionFilter]
        public Token Get([FromBody] Credential credential)
        {
            try
            {
                return credentialsRepository.Login(credential);
            }
            catch (ArgumentException e)
            {
                Logger.Log.Instance.Error(e.Message);
                throw new ArgumentException(e.Message);
            }
        }

        /// <summary>
        /// Удаление пользователя
        /// </summary>
        /// <param name="login">Логин пользователя</param>
        /// <param name="accessToken">Токен доступа</param>
        [HttpPut]
        [Route("api/delete/{login}")]
        public void Delete(string login, [FromBody] Token accessToken)
        {
            Logger.Log.Instance.Info("Удаление пользователя: {0}", login);
            credentialsRepository.Delete(login, accessToken);
        }
    }
}
