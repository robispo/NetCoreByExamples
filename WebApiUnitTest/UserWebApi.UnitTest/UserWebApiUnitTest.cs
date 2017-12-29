using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UserWebApi.Controllers;
using UserWebApi.Models;
using UserWebApi.Services;

namespace UserWebApi.UnitTest
{
    [TestClass]
    public class UserWebApiUnitTest
    {
        UserController _userController;
        DataBaseELContext _dataBaseELContext;
        IJwtService _jwtService;

        public UserWebApiUnitTest()
        {
            DbContextOptions<DataBaseELContext> dbOptions;
            DbContextOptionsBuilder<DataBaseELContext> dboBuilder = new DbContextOptionsBuilder<DataBaseELContext>();
            dboBuilder.UseInMemoryDatabase("WebapiTest");
            dbOptions = dboBuilder.Options;

            _dataBaseELContext = new DataBaseELContext(dbOptions);
            _jwtService = new JwtService();

            _userController = new UserController(_dataBaseELContext, _jwtService);
        }

        [TestMethod]
        public void GetUserAll()
        {
            IActionResult actionResult;
            ObjectResult objectResult;

            actionResult = _userController.GetUserAll();

            Assert.IsNotNull(actionResult);
            objectResult = (ObjectResult)actionResult;

            Assert.IsNotNull(objectResult.StatusCode);

            Assert.AreEqual(objectResult.StatusCode.Value, 200);
        }

        [TestMethod]
        public void GetUserWithData()
        {
            IActionResult actionResult;
            ObjectResult objectResult;

            actionResult = _userController.GetUser("robispo");

            Assert.IsNotNull(actionResult);
            objectResult = (ObjectResult)actionResult;

            Assert.IsNotNull(objectResult.StatusCode);
            Assert.IsNotNull(objectResult.Value);

            Assert.AreEqual(objectResult.StatusCode.Value, (int)HttpStatusCode.OK);

        }

        [TestMethod]
        public void GetUserWithNotData()
        {
            IActionResult actionResult;
            ObjectResult objectResult;

            actionResult = _userController.GetUser("");

            Assert.IsNotNull(actionResult);
            objectResult = (ObjectResult)actionResult;

            Assert.IsNotNull(objectResult.StatusCode);

            Assert.AreEqual(objectResult.StatusCode.Value, (int)HttpStatusCode.BadRequest);
        }

        //[TestMethod]
        //public void GetUserWithEagerLoading()
        //{
        //    HttpResponseMessage response = TestUserWithEagerLoading().Result;

        //    Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
        //}

        //public async Task<HttpResponseMessage>  TestUserWithEagerLoading()
        //{
        //    using (HttpClient client = new HttpClient())
        //    {
        //        client.BaseAddress = new Uri("http://localhost:8610/");
        //        client.DefaultRequestHeaders.Accept.Clear();
        //        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        //        using (HttpResponseMessage response = await client.GetAsync("api/users/robispo?with=Roles"))
        //        {
        //            return
        //                response;
        //        }
        //    }
        //}
    }
}