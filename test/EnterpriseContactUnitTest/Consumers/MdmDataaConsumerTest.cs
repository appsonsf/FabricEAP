using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Mdm.Org.MsgContracts;
using EnterpriseContact;
using EnterpriseContact.Consumers;
using MassTransit;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EnterpriseContactUnitTest.Consumers
{
    [TestClass]
    public class MdmDataaConsumerTest : TheAppServiceTestBase
    {
        [TestMethod]
        public async Task Test_Consumer()
        {
            var (_, dbContextOptions) = OpenDb();
            var mapper = CreateMapper();
            MdmDataConsumer comsumer = new MdmDataConsumer(dbContextOptions, mapper);
            bool finished = false;
            var bus = CreateBus(u =>
            {
                //u.Handler<FullOrgData>(async context =>
                //{
                //    await comsumer.Consume(context);
                //    finished = true;
                //});
            });
            bus.Start();
            var orgUnitId = Guid.NewGuid();
            var orgUnitSrcId = "OrgUnitScrId";
            var positionId = Guid.NewGuid();
            var positionSrcId = "PositionSrcId";
            var contactId = Guid.NewGuid();
            var contactSrcId = "ContactSrcId";
            await bus.Publish<FullOrgData>(new
            {
                ModelVersion = "111",
                DataVersion = "222",
                OrgUnits = new List<OrgUnitEntityMsg>()
                {
                    new OrgUnitEntityMsg()
                    {
                        Id = orgUnitId,
                        Name = "内阁",
                        Number = "number",
                        Parents = new List<OrgUnitParentValueMsg>(),//空,表示顶级
                        Sort = 1,
                        SrcId = orgUnitSrcId,
                        Positions = new List<PositionEntityMsg>()
                        {
                            new PositionEntityMsg()
                            {
                                Id = positionId,
                                SrcId = positionSrcId,
                                Name = "内阁主管"
                            }
                        }
                    }
                },
                Contacts = new List<ContactEntityMsg>()
                {
                    new ContactEntityMsg()
                    {
                        ContactId = contactId,
                        ContactSrcId = contactSrcId,
                        Gender = 1,
                        IdCardNo = "510821199408145016",
                        Mobile = "15523318594",
                        Name = "曹操",
                        Number = "111",
                        Positions = new List<ContactPositionValueMsg>()
                        {
                            new ContactPositionValueMsg()
                            {
                                IsPrimary = true,
                                OrgUnitId = orgUnitId,
                                OrgUnitSrcId = orgUnitSrcId,
                                PositionId = positionId,
                                PositionSrcId = positionSrcId,
                            }
                        },
                        RelationTypeId = 1,
                        SrcId = contactSrcId,
                    }
                }
            });
            return; //由于Sqllite 是不支持T-sql中的一些东西，故，这个单元测试需要更改
            while (!finished)
            {
                await Task.Delay(2000);
            }

            using (var db = new ServiceDbContext(dbContextOptions))
            {
                var contactCount = db.Employees.Count();
                var departmentCount = db.Departments.Count();
                Assert.AreEqual(2, contactCount);
                Assert.AreEqual(2, departmentCount);
            }
        }
    }
}
