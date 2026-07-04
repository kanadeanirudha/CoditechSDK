using Xunit;
using System;
using System.Runtime.Serialization;
using System.Linq;
using System.Reflection;

namespace Coditech.Engine.DBTM.Test.Tests.Generated.ServiceTests
{
    public class DBTMOrganisationCentreMasterServiceTests
    {
        [Fact]
        public void Constructor_Should_Create_Instance_Without_Running_Constructor()
        {
            var asm = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => a.GetName().Name == "Coditech.Engine.DBTM");
            Assert.NotNull(asm);
            var type = asm.GetType("Coditech.API.Service.DBTMOrganisationCentreMasterService");
            Assert.NotNull(type);
            var instance = FormatterServices.GetUninitializedObject(type);
            Assert.NotNull(instance);
        }

        [Fact(Skip = "TODO: implement unit tests for public methods in DBTMOrganisationCentreMasterService")]
        public void TODO() { }
    }
}
