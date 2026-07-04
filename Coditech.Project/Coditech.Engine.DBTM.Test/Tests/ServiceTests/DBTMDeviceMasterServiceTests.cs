using System;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Collections.Generic;
using Xunit;
using Moq;
using Coditech.API.Service;
using Coditech.API.Data;
using Coditech.Common.Service;

namespace Coditech.Engine.DBTM.Test.Tests.ServiceTests
{
    public class DBTMDeviceMasterServiceTests
    {
        private static object CreateServiceWithRepository(Mock<ICoditechRepository<DBTMDeviceMaster>> repoMock)
        {
            // create instance without running constructor
            var svc = FormatterServices.GetUninitializedObject(typeof(DBTMDeviceMasterService));

            // set private fields
            var repoField = typeof(DBTMDeviceMasterService).GetField("_dBTMDeviceMasterRepository", BindingFlags.NonPublic | BindingFlags.Instance);
            repoField.SetValue(svc, repoMock.Object);

            var spField = typeof(DBTMDeviceMasterService).GetField("_serviceProvider", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
            spField.SetValue(svc, Mock.Of<IServiceProvider>());

            var logField = typeof(DBTMDeviceMasterService).GetField("_coditechLogging", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
            logField.SetValue(svc, Mock.Of<object>());

            return svc;
        }

        [Fact]
        public void IsValidDeviceSerialCode_Throws_On_NullOrWhitespace()
        {
            var repoMock = new Mock<ICoditechRepository<DBTMDeviceMaster>>();
            var svc = (DBTMDeviceMasterService)CreateServiceWithRepository(repoMock);

            Assert.ThrowsAny<ArgumentException>(() => svc.IsValidDeviceSerialCode(null));
            Assert.ThrowsAny<ArgumentException>(() => svc.IsValidDeviceSerialCode(""));
            Assert.ThrowsAny<ArgumentException>(() => svc.IsValidDeviceSerialCode("   "));
        }

        [Fact]
        public void IsValidDeviceSerialCode_ReturnsTrue_When_Exists()
        {
            var data = new List<DBTMDeviceMaster>
            {
                new DBTMDeviceMaster { DBTMDeviceMasterId = 1, DeviceSerialCode = "SN123" }
            }.AsQueryable();

            var repoMock = new Mock<ICoditechRepository<DBTMDeviceMaster>>();
            repoMock.Setup(r => r.Table).Returns(data);

            var svc = (DBTMDeviceMasterService)CreateServiceWithRepository(repoMock);

            var result = svc.IsValidDeviceSerialCode("SN123");
            Assert.True(result);

            var result2 = svc.IsValidDeviceSerialCode("NoSuch");
            Assert.False(result2);
        }

        [Fact]
        public void GetDBTMDeviceMasterDetailsByCode_Returns_Entity_When_Found()
        {
            var data = new List<DBTMDeviceMaster>
            {
                new DBTMDeviceMaster { DBTMDeviceMasterId = 1, DeviceSerialCode = "SNABC", IsActive = true }
            }.AsQueryable();

            var repoMock = new Mock<ICoditechRepository<DBTMDeviceMaster>>();
            repoMock.Setup(r => r.Table).Returns(data);

            var svc = (DBTMDeviceMasterService)CreateServiceWithRepository(repoMock);

            var entity = svc.GetDBTMDeviceMasterDetailsByCode("SNABC");
            Assert.NotNull(entity);
            Assert.Equal(1, entity.DBTMDeviceMasterId);

            var notFound = svc.GetDBTMDeviceMasterDetailsByCode("SNX");
            Assert.Null(notFound);
        }

        [Fact]
        public void IsDBTMDeviceSerialCodeAlreadyExist_ProtectedMethod_Works()
        {
            var data = new List<DBTMDeviceMaster>
            {
                new DBTMDeviceMaster { DBTMDeviceMasterId = 1, DeviceSerialCode = "SN1" },
                new DBTMDeviceMaster { DBTMDeviceMasterId = 2, DeviceSerialCode = "SN2" }
            }.AsQueryable();

            var repoMock = new Mock<ICoditechRepository<DBTMDeviceMaster>>();
            repoMock.Setup(r => r.Table).Returns(data);

            var svc = CreateServiceWithRepository(repoMock);
            var svcType = svc.GetType();

            var method = svcType.GetMethod("IsDBTMDeviceSerialCodeAlreadyExist", BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.NotNull(method);

            // existing serial with different id should return true
            var res1 = (bool)method.Invoke(svc, new object[] { "SN1", 2L });
            Assert.True(res1);

            // same id should be considered not exist (false)
            var res2 = (bool)method.Invoke(svc, new object[] { "SN1", 1L });
            Assert.False(res2);

            // new serial should return false
            var res3 = (bool)method.Invoke(svc, new object[] { "SNX", 0L });
            Assert.False(res3);
        }

        [Fact]
        public void GetDBTMDevice_Throws_When_IdLessThanOne()
        {
            var repoMock = new Mock<ICoditechRepository<DBTMDeviceMaster>>();
            var svc = (DBTMDeviceMasterService)CreateServiceWithRepository(repoMock);

            Assert.ThrowsAny<Exception>(() => svc.GetDBTMDevice(0));
            Assert.ThrowsAny<Exception>(() => svc.GetDBTMDevice(-5));
        }

        [Fact]
        public void GetDBTMDevice_Returns_Model_When_Found()
        {
            // If mapping extension methods are available, this will return a model. We just assert non-null when entity exists.
            var entity = new DBTMDeviceMaster { DBTMDeviceMasterId = 5, DeviceName = "D1", DeviceSerialCode = "S1", IsActive = true };
            var data = new List<DBTMDeviceMaster> { entity }.AsQueryable();

            var repoMock = new Mock<ICoditechRepository<DBTMDeviceMaster>>();
            repoMock.Setup(r => r.Table).Returns(data);

            var svc = (DBTMDeviceMasterService)CreateServiceWithRepository(repoMock);

            var model = svc.GetDBTMDevice(5);
            Assert.NotNull(model);
            // If the model includes id property, try to check it via reflection
            var modelType = model.GetType();
            var idProp = modelType.GetProperty("DBTMDeviceMasterId");
            if (idProp != null)
            {
                var val = idProp.GetValue(model);
                Assert.Equal(5, Convert.ToInt64(val));
            }
        }
    }
}
