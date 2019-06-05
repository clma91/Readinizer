using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Readinizer.Backend.Business.Services;
using Readinizer.Backend.DataAccess.UnityOfWork;

namespace Readinizer.Backend.Business.Tests
{
    [TestClass()]
    public class RsoPPotServiceTests : BaseReadinizerTestData
    {
        public static RSoPPotService rsopPotService { get; set; } = new RSoPPotService(new UnitOfWork());

        [TestMethod()]
        public void FillRsopPotListTest()
        {
            var sortedRsopsByDomain = Rsops.OrderBy(x => x.Domain.ParentId).ToList();
            var rsopPots = rsopPotService.FillRsopPotList(sortedRsopsByDomain);
            Assert.AreEqual(2, rsopPots.Count);
        }

        [TestMethod()]
        public void RsopPotFactoryTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void RsopPotsEqualTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void SettingsEqualTest()
        {
            Assert.Fail();
        }
    }
}