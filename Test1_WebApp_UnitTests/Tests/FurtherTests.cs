using Test1_WebApp_MVC.DAL;
using Test1_WebApp_MVC.Models;
using Test1_WebApp_UnitTests.Mocks;
using Test1_WebApp_MVC.Controllers;

namespace Test1_WebApp_UnitTests.Tests
{
    [TestClass]
    public class FurtherTests
    {
        /*
         * Please see the existing test for my general approach to unit tests.
         * I don't like to write unit tests that rely on outside connections, I'd always rather mock the dependency and save the rest for integration tests. XML allows for unit testingin a way e.g. DB connections don't.
         * I like to include all negative scenarios
         * I would have included at the very least also tests for the following classes:
         * 
         *  Models > Users (methods)
         *  Services > UserDataServices (passing in my own mock context)
         *  Services > Utils
         */        
    }
}