using Test1_WebApp_MVC.DAL;
using Test1_WebApp_MVC.Models;
using Test1_WebApp_UnitTests.Mocks;

namespace Test1_WebApp_UnitTests.Tests
{
    [TestClass]
    public class XmlUserDataContext_Tests
    {
        #region CREATE TESTS

        private void 

        [TestMethod]
        public void CreateUser_should_CreateValidUserAtEndOfFile()
        {            
            var context = new XmlUserDataContext("myMockXmlFile.xml", new MockLogger());

            //Test not implemented

            Assert.IsTrue(true);
        }

        public void CreateUser_should_ExitOnInvalidUser()
        {
            var context = new XmlUserDataContext(new MockLogger());

            User testUser = null;
            Assert.IsFalse(context.CreateUser(testUser), "Should exit gracefully on null user");

            testUser = new User();
            Assert.IsFalse(context.CreateUser(testUser), "Should exit gracefully on unpopulated user");
        }

        #endregion CREATE TESTS


        #region UPDATE TESTS

        public void UpdateUser_should_UpdateValidUser()
        {
            var context = new XmlUserDataContext(new MockLogger());

            //Test not implemented:
            // - update on name
            // - update on surname
            // - update on phone nr
            // - update on all fields set
            // - update on all fields blank
            // - validation/negative test as for Create

            Assert.IsTrue(true);
        }

        #endregion UPDATE TESTS


        #region DELETE TESTS

        public void DeleteUser_should_DeleteValidId()
        {
            var context = new XmlUserDataContext(new MockLogger());

            //Test not implemented:

            Assert.IsTrue(true);
        }

        public void DeleteUser_should_ExitOnInvalidId()
        {
            var context = new XmlUserDataContext(new MockLogger());

            //Test not implemented:
            // - id is negative
            // - id is zero
            // - id is larger than file user count

            Assert.IsTrue(true);
        }

        #endregion DELETE TESTS


        #region GET USERS TESTS

        public void GetUsers_should_ReturnAllUsers()
        {
            var context = new XmlUserDataContext(new MockLogger());

            //Test not implemented:

            Assert.IsTrue(true);
        }

        #region GET USERS TESTS

    }
}