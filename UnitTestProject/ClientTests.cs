using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Server;

namespace UnitTestProject
{
    [TestClass]
    public class ClientTests
    {
        //to properly run test it's required to have connection with db
        //to pass this test there have to be user with name maciej and password 123 in db
        [TestMethod]
        public void ChangeUsernameTest()
        {
            ClientController controller = new ClientController();
            controller.getSession().setUser(new User("maciej", "123"));
            controller.ChangeUsername(controller.getSession().getUser().getPassword(), "gerwant");

            Assert.AreEqual("gerwant", controller.getSession().getUser().getLogin());
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "user exists")]
        public void RegisterUserTest()
        {
            //Don't know if it's as it should be but this test creates a row in db
            ClientController controller = new ClientController();
            controller.Register("JohnSnow", "KingoftheNorth", "KingoftheNorth");
        }

        [TestMethod]
        public void LoginTest()
        {
            ClientController controller = new ClientController();
            Assert.IsTrue(controller.LogIn("mchajda", "maciek1"));
        }
    }
}
