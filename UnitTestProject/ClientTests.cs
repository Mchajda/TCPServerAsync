using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Server;

namespace UnitTestProject
{
    [TestClass]
    public class ClientTests
    {
        //to properly run test it's required to have connection with database
        [TestMethod]
        public void ChangeUsernameTest()
        {
            User u = new User("maciej", "123");
            ClientController controller = new ClientController();
            controller.getSession().setUser(u);
            controller.ChangeLogin(u.getLogin(), "gerwant", u.getPassword());

            Assert.AreEqual("gerwant", controller.getSession().getUser().getLogin());
        }

        [TestMethod]
        public void InsertRowTest()
        {
            UsersManager manager = new UsersManager();

            Assert.IsTrue(manager.insertRow("maciej", "123", "ROLE_USER"));
        }

        [TestMethod]
        public void RegisterUserTest()
        {
            ClientController controller = new ClientController();
            Assert.IsTrue(controller.Register("JohnSnow", "KingoftheNorth", "KingoftheNorth"));
        }
    }
}
