using System;
using AutoMapper;
using Coditech.API.Mapper;
using Coditech.API.Data;
using Coditech.Common.API.Model;
using Xunit;

namespace Coditech.Engine.DBTM.test.Tests
{
    public class AutoMapperConfigTests
    {
        [Fact]
        public void AutoMapper_Configuration_IsValid()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile(new AutoMapperConfig()));
            config.AssertConfigurationIsValid();
        }

        [Fact]
        public void AutoMapper_Maps_DBTMDeviceMaster_To_DBTMDeviceModel()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile(new AutoMapperConfig()));
            var mapper = config.CreateMapper();

            var source = new DBTMDeviceMaster
            {
                DBTMDeviceMasterId = 1,
                DeviceName = "Device A",
                DeviceSerialCode = "SN123",
                IsActive = true,
                IsMasterDevice = true,
                RegistrationDate = new DateTime(2020, 1, 1),
                WarrantyExpirationPeriodInMonth = 12,
                ManufacturedBy = "Maker",
                Description = "Desc",
                AdditionalFeatures = "Feat"
            };

            var dest = mapper.Map<DBTMDeviceModel>(source);

            Assert.Equal(source.DBTMDeviceMasterId, dest.DBTMDeviceMasterId);
            Assert.Equal(source.DeviceName, dest.DeviceName);
            Assert.Equal(source.DeviceSerialCode, dest.DeviceSerialCode);
            Assert.Equal(source.IsMasterDevice, dest.IsMasterDevice);
            Assert.Equal(source.IsActive, dest.IsActive);
            Assert.Equal(source.WarrantyExpirationPeriodInMonth, dest.WarrantyExpirationPeriodInMonth);
        }
    }
}
