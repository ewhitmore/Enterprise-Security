using Enterprise.Web.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Enterprise.Web.Tests.Utils
{
    [TestClass()]
    public class HelperTests
    {
        [TestMethod()]
        public void HelperTests_GetHash_ReturnTrue()
        {
            // Arrange
            const string sha256HashOfMySecret = "SVYs/DsXE56gHEgLnIai3ayzj/Gy6dsb9murek4/H7U=";

            // Act
            var result = Helper.GetHash("MySecret");

            // Assert
            Assert.AreEqual(sha256HashOfMySecret, result);
        }
    }
}