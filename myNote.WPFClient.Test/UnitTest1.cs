using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using myNote.Model;
using myNote.WPFClient.ApiInteractions;

namespace myNote.WPFClient.Test
{
    [TestClass]
    public class ServiceClientTest
    {
        const string ConnectionString = @"http://localhost:64625/api";
        ServiceClient serviceClient = new ServiceClient(ConnectionString);
        private Dictionary<string, Token> tempUser = new Dictionary<string, Token>();

        [TestMethod]
        public void ShouldRegisterService()
        {
            //arrange
            var credential = new Credential { Login = "Max", Password = GetPassword("vavava") };

            //act
            serviceClient.Register(credential);
            var token = serviceClient.Login(credential).Result;
            tempUser.Add(credential.Login, token);
        }

        #region Helping method

        internal static byte[] GetPassword(string password)
        {
            var md5 = new MD5Cng();
            return md5.ComputeHash(Encoding.ASCII.GetBytes(password));
        }

        #endregion

        #region Clean Up temp

        [TestCleanup]
        public void CleanData()
        {
            foreach (var keyvalue in tempUser)
            {
                serviceClient.Delete(keyvalue.Key, keyvalue.Value);
            }
        }

        #endregion

    }
}
